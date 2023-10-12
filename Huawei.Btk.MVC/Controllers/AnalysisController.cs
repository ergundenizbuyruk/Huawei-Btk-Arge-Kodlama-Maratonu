using Huawei.Btk.Application.Services.UserServices;
using Huawei.Btk.Application.Services.UserServices.Dtos;
using Huawei.Btk.Core.Context;
using Huawei.Btk.Core.Models;
using Huawei.Btk.MVC.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.IO;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Huawei.Btk.MVC.Controllers
{
	[Authorize]
	public class AnalysisController : Controller
	{

		private readonly IUserService _userService;
		private readonly ApplicationDbContext _context;
		private readonly ServerInformation _serverInformation;

		public AnalysisController(IUserService userService, ApplicationDbContext context,
			IOptions<ServerInformation> serverInformation)
		{
			_userService = userService;
			_context = context;
			_serverInformation = serverInformation.Value;
		}

		public IActionResult Create()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromForm] IFormFile FormFile)
		{
			Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
			var user = await _userService.GetByIdAsync(userId);
			

			var extent = Path.GetExtension(FormFile.FileName);
			var randomName = ($"{Guid.NewGuid()}{extent}");
			var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img", randomName);

			var userDto = new UserDto
			{
				Name = user.Name,
				Surname = user.Surname,
				PhoneNumber = user.PhoneNumber,
				Age = user.Age,
				Tall = user.Tall,
				Weight = user.Weight,
				DefaultLanguage = user.DefaultLanguage,
				Allergies = user.Allergies.Select(x => x.AllergyText).ToList(),
				ActiveIngredients = user.ActiveIngredients.Select(x => x.ActiveIngredientText).ToList(),
				PhotoPath = path
			};

			Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\img"));

			using (var stream = new FileStream(path, FileMode.Create))
			{
				await FormFile.CopyToAsync(stream);
			}

			Analysis analysis = new Analysis
			{
				Result = "",
				CreatedDate = DateTime.Now,
				UserId = userId,
				ImagePath = randomName
			};

			await _context.Analyses.AddAsync(analysis);
			await _context.SaveChangesAsync();

			//send to AI
			using (var client = new HttpClient())
			{
				client.BaseAddress = new Uri(_serverInformation.AIAppIp + ":" + _serverInformation.AiAppPort);
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				//byte[] data;
				//using (var br = new BinaryReader(FormFile.OpenReadStream()))
				//	data = br.ReadBytes((int)FormFile.OpenReadStream().Length);
				//ByteArrayContent bytes = new ByteArrayContent(data);

				//var serializeUser = JsonSerializer.Serialize(userDto);

				//MultipartFormDataContent multiContent = new MultipartFormDataContent();

				var serializeUser = JsonConvert.SerializeObject(userDto);
				StringContent stringContent = new StringContent(serializeUser, Encoding.UTF8, "application/json");
				//multiContent.Add(bytes, "file", FormFile.FileName);
				//var stringContent = new StringContent(JsonConvert.SerializeObject(userDto), Encoding.UTF8, "application/json");
				//multiContent.Add(stringContent);
				HttpResponseMessage response = await client.PostAsync($"/api/ai", stringContent);

				if (!response.IsSuccessStatusCode)
				{
					return View();
				}
				var result = await response.Content.ReadAsStringAsync();
				analysis.Result = result;
				_context.Analyses.Update(analysis);
				await _context.SaveChangesAsync();
				return RedirectToAction("Index", "Home");
			}

			////send to OBS
			//using (var client = new HttpClient())
			//{
			//	client.BaseAddress = new Uri(_serverInformation.ObsUrl);
			//	client.DefaultRequestHeaders.Accept.Clear();
			//	client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

			//	byte[] data;
			//	using (var br = new BinaryReader(FormFile.OpenReadStream()))
			//		data = br.ReadBytes((int)FormFile.OpenReadStream().Length);
			//	ByteArrayContent bytes = new ByteArrayContent(data);

			//	MultipartFormDataContent multiContent = new MultipartFormDataContent
			//	{
			//		{ bytes, "file", FormFile.FileName }
			//	};
			//	HttpResponseMessage response = await client
			//		.PostAsync($"/{analysis.Id}{Path.GetExtension(FormFile.FileName)}?append&position=0", multiContent);

			//	if (!response.IsSuccessStatusCode)
			//	{
			//		// open detail page
			//		return View();
			//	}
			//}

			return RedirectToAction("Index", "Home");
		}

		public async Task<IActionResult> Detail(int id)
		{
			var analysis = await _context.Analyses.FirstOrDefaultAsync(x => x.Id == id);
			return View(analysis);
		}
	}
}
