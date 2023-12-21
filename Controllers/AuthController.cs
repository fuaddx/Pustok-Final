using Pustok2.Helpers;
using Pustok2.Models;
using Pustok2.ViewModel.AuthVM;
using Microsoft.AspNetCore.Http.Metadata;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Pustok2.ExternalServices.Interfaces;
using Pustok2.ExternalServices.Inplements;

namespace Pustok2.Controllers
{
	
	public class AuthController : Controller
    {
        SignInManager<AppUser> _signInManager { get; }
        UserManager<AppUser> _userManager { get; }
        RoleManager<IdentityRole> _roleManager { get; }
		IEmailService _emailService { get; }

        public AuthController(SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IEmailService emailService)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }
        
		public IActionResult SendMail()
		{
			_emailService.Send("fuad.flash.2002@gmail.com", "Salam", "Bu bir testdir");
			return Ok();
		}
        public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterVM vm)
		{
			if (!ModelState.IsValid)
			{
				return View(vm);
			}
			var user = new AppUser
			{
				FirstName = vm.Fisrtname,
				LastName = vm.Lastname,
				Email = vm.Email,
				UserName = vm.Username
			};
			var result = await _userManager.CreateAsync(user, vm.Password);
			if (!result.Succeeded)
			{
				foreach (var error in result.Errors)
				{
					ModelState.AddModelError("", error.Description);
				}
				return View(vm);
			}

			var roleResult = await _userManager.AddToRoleAsync(user, Roles.Member.ToString());
			if (!roleResult.Succeeded)
			{
				ModelState.AddModelError("", "Something went wrong. Please contact admin");
				return View(vm);
			}
			//Mail gonderir
			/*using StreamReader reader = new StreamReader(Path.Combine(PathConstants.RootPath, "htmlpage.html"));
            string template = reader.ReadToEnd();
            _emailService.Send("", "Salam", template);*/

			await _sendConfirmation(user);
			return View();
        }

        //Send
        public async Task<IActionResult>SendConfirmationEmail(string username)
		{
			await _sendConfirmation(await _userManager.FindByNameAsync(username));

            return Content("Email Sent!");
        }

       
        async Task _sendConfirmation(AppUser user)
		{
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var link = Url.Action("EmailConfirmed", "Auth", new
            {
                token = token,
                username = user.UserName
            }, Request.Scheme);
            _emailService.Send(user.Email, "Confirmation Email", $"<a href='{link}'>Confirm Email</a>");
        }
		//Confirm  

		public async Task<IActionResult>EmailConfirmed(string token,string username)
		{
			var result = await _userManager.ConfirmEmailAsync(await _userManager.FindByNameAsync(username),token);
			if (result.Succeeded) return Ok();
			return Problem();
		}

		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(string? returnUrl,LoginVm vm)
		{
			AppUser user;
			if(!ModelState.IsValid)
			{
				return View();
			}
			if (vm.UsernameOrEmail.Contains("@"))
			{
				user = await _userManager.FindByEmailAsync(vm.UsernameOrEmail);
			}
			else
			{
				user = await _userManager.FindByNameAsync(vm.UsernameOrEmail);
			}
			var result = await _signInManager.PasswordSignInAsync(user, vm.Password, vm.IsRemember, true);
			if (!result.Succeeded)
			{
				if (result.IsLockedOut)
				{
					ModelState.AddModelError("", "Too many attempts wait until " + DateTime.Parse(user.LockoutEnd.ToString()).ToString("HH:mm"));
				}
				else if (!user.EmailConfirmed)
				{
					var param = new
					{
						username = user.UserName
					};

					ViewBag.Link = $"Go to <a href='{Url.Action("SendConfirmationEmail", "Auth", param)}'> confirm</a>";
				}
				else
				{
					ModelState.AddModelError("", "Username or password is wrong");
				}
				return View(vm);
			}
			if (returnUrl != null)
			{
					return LocalRedirect(returnUrl);
			}

			return RedirectToAction("Index", "Home");

		}

		public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
		public async Task<bool> CreateRoles()
		{
			foreach (var item in Enum.GetValues(typeof(Roles)))
			{
				if (!await _roleManager.RoleExistsAsync(item.ToString()))
				{
					var result = await _roleManager.CreateAsync(new IdentityRole
					{
						Name = item.ToString()
					});
					if (!result.Succeeded)
					{
						return false;
					}
				}
			}
			return true;
		}

		
	
    }
}
