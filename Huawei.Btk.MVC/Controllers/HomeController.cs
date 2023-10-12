using Huawei.Btk.Application.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Huawei.Btk.MVC.Controllers
{
	
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> _logger;
		private readonly IUserService _userService;

		public HomeController(ILogger<HomeController> logger, IUserService userService)
		{
			_logger = logger;
			_userService = userService;
		}

		[Authorize]
		public async Task<IActionResult> Index()
		{
			Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
			var user = await _userService.GetByIdAsync(userId);
			return View(user.Analyses.OrderByDescending(x => x.CreatedDate).ToList());
		}

		public IActionResult Notification()
		{
			return View();
		}
	}
}