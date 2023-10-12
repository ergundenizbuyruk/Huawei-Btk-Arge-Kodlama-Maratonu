using Huawei.Btk.Application.Services.EmailServices;
using Huawei.Btk.Application.Services.UserServices;
using Huawei.Btk.Application.Services.UserServices.Dtos;
using Huawei.Btk.Core.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Net;
using System.Net.Mail;
using System.Security.Claims;
using System.Web;

namespace Huawei.Btk.MVC.Controllers
{
	public class AccountController : Controller
	{
		private readonly IUserService _userService;
		private readonly UserManager<User> _userManager;
		private readonly SignInManager<User> _signInManager;
		private readonly IEmailSender _emailSender;

		public AccountController(IUserService userService,
			UserManager<User> userManager,
			SignInManager<User> signInManager,
			IEmailSender emailSender)
		{
			_userService = userService;
			_userManager = userManager;
			_signInManager = signInManager;
			_emailSender = emailSender;
		}

		[HttpGet]
		public IActionResult Register()
		{
			IEnumerable<SelectListItem> languages =
				(new string[] { "en", "tr", "fr", "ar", "az", "bg", "de", "es", "it", "pt" })
				.Select(x => new SelectListItem
				{
					Text = x,
					Value = x,
					Selected = x == "tr"
				});
			ViewBag.languages = languages;
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Register(UserRegisterDto userRegisterDto)
		{
			IEnumerable<SelectListItem> languages =
				(new string[] { "en", "tr", "fr", "ar", "az", "bg", "de", "es", "it", "pt" })
				.Select(x => new SelectListItem
				{
					Text = x,
					Value = x,
					Selected = x == "tr"
				});
			ViewBag.languages = languages;

			if (!ModelState.IsValid)
			{
				return View();
			}

			var user = new User
			{
				Email = userRegisterDto.Email,
				PhoneNumber = userRegisterDto.PhoneNumber.Replace("-", ""),
				UserName = userRegisterDto.Email,
				Name = userRegisterDto.Name,
				Surname = userRegisterDto.Surname,
				Tall = userRegisterDto.Tall,
				Weight = userRegisterDto.Weight,
				Age = userRegisterDto.Age,
				DefaultLanguage = userRegisterDto.DefaultLanguage,
			};

			var result = await _userManager.CreateAsync(user, userRegisterDto.Password);

			if (!result.Succeeded)
			{
				TempData["RegisterError"] = result.Errors.First().Description;
				return View();
			}

			// create confirm token and send email
			var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
			var fromAddress = new MailAddress("ergun2127@gmail.com", "mindbenders");
			var toAddress = new MailAddress(user.Email, user.Name + user.Surname);
			var url = Url.ActionLink("ConfirmEmail", "Account");
			var smtp = new SmtpClient
			{
				Host = "smtp.gmail.com",
				Port = 587,
				EnableSsl = true,
				DeliveryMethod = SmtpDeliveryMethod.Network,
				UseDefaultCredentials = false,
				Credentials = new NetworkCredential(fromAddress.Address, "cxipiynakpaijbsf")
			};
			using (var mailMessage = new MailMessage(fromAddress, toAddress)
			{
				Subject = "Bilinçli Tüketici Email Doğrulama",
				IsBodyHtml = true,
				Body = $"<p>Aşağıdaki linke tıklayarak e-postanızı doğrulayabilirsiniz.</p>" +
					$"<p>{url + "?email=" + user.Email + "&token=" + HttpUtility.UrlEncode(token)}</p>" +
					$"<p></p>"
			})
			{
				smtp.Send(mailMessage);
			}

			TempData["SuccessMessage"] = "Kaydınız alınmıtır. Giriş yapabilmek için lütfen mail hesabınızı onaylayınız.";
			return RedirectToAction("Notification", "Home");
		}

		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginDto loginDto)
		{
			if (!ModelState.IsValid)
			{
				return View();
			}

			var user = await _userManager.FindByEmailAsync(loginDto.Email);

			if (user == null)
			{
				TempData["LoginError"] = "E-posta veya Parola Hatalı";
				return View();
			}

			var result = await _signInManager.PasswordSignInAsync(user, loginDto.Password, false, false);

			if (result.IsNotAllowed)
			{
				TempData["LoginError"] = "Lütfen E-postanızı doğrulayınız.";
				return View();
			}

			if (!result.Succeeded)
			{
				TempData["LoginError"] = "E-posta veya Parola Hatalı";
				return View();
			}

			return RedirectToAction("Index", "Home");
		}

		[Authorize]
		public async Task<IActionResult> Logout()
		{
			await _signInManager.SignOutAsync();
			return RedirectToAction("Login", "Account");
		}

		public async Task<IActionResult> ConfirmEmail([FromQuery(Name = "email")] string email,
			[FromQuery(Name = "token")] string token)
		{
			if (token == null || email == null)
			{
				return RedirectToAction("Index", "Home");
			}

			var emailDto = new ConfirmEmailDto
			{
				Email = email,
				Token = token
			};

			var user = await _userManager.FindByEmailAsync(email);

			if (user == null)
			{
				TempData["ErrorMessage"] = "Kullanıcı Bulunamadı";
				return RedirectToAction("Notification", "Home");
			}

			var result = await _userManager.ConfirmEmailAsync(user, token);

			if (!result.Succeeded)
			{
				TempData["ErrorMessage"] = result.Errors.First().Description;
				return RedirectToAction("Notification", "Home");
			}

			TempData["SuccessMessage"] = "E-posta adresiniz başarıyla doğrulandı. Sisteme giriş yapabilirsiniz.";
			return RedirectToAction("Notification", "Home");
		}

		[HttpGet]
		[Authorize]
		public async Task<IActionResult> Profile()
		{
			Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
			var user = await _userService.GetByIdAsync(userId);

			UpdateUserDto updateUserDto = new UpdateUserDto
			{
				Name = user.Name,
				Surname = user.Surname,
				PhoneNumber = user.PhoneNumber,
				Weight = user.Weight,
				Tall = user.Tall,
				DefaultLanguage = user.DefaultLanguage,
				Age = user.Age,
				Allergies = string.Join(", ", user.Allergies.Select(x => x.AllergyText)),
				ActiveIngredients = string.Join(", ", user.ActiveIngredients.Select(x => x.ActiveIngredientText))
			};

			IEnumerable<SelectListItem> languages =
				(new string[] { "en", "tr", "fr", "ar", "az", "bg", "de", "es", "it", "pt" })
				.Select(x => new SelectListItem
				{
					Text = x,
					Value = x,
					Selected = x == "tr"
				});
			ViewBag.languages = languages;

			return View(updateUserDto);
		}

		[HttpPost]
		[Authorize]
		public async Task<IActionResult> Profile(UpdateUserDto updateUserDto)
		{
			IEnumerable<SelectListItem> languages =
				(new string[] { "en", "tr", "fr", "ar", "az", "bg", "de", "es", "it", "pt" })
				.Select(x => new SelectListItem
				{
					Text = x,
					Value = x,
					Selected = x == "tr"
				});
			ViewBag.languages = languages;
			if (!ModelState.IsValid)
			{
				return View(updateUserDto);
			}

			Guid userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
			await _userService.UpdateAsync(updateUserDto, userId);
			return View(updateUserDto);
		}
	}
}
