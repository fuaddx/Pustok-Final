using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Pustok2.Contexts;
using Pustok2.Helpers;
using Pustok2.Models;
using Pustok2.ViewModel.UserPageVm;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Pustok2.Controllers
{
	public class UserPageController : Controller
	{
        private readonly IWebHostEnvironment env;

        PustokDbContext _db { get; }
		UserManager<AppUser> _userManager { get; }
		public UserPageController(IWebHostEnvironment env, PustokDbContext db, UserManager<AppUser> userManager)
		{
            this.env = env;
            _db = db;
			_userManager = userManager;
		}


		public async Task<IActionResult> Index()
		{
			var name = User.Identity.Name;

			var userr = await _userManager.FindByNameAsync(name);

			if (userr == null) return NotFound();
			var UserVm = new UserPageVM
			{
				Username = userr.UserName,
				Email = userr.Email,
				Name = userr.FirstName,
				Surname = userr.LastName,
				ProfileImageURL = userr.ProfileImageUrl,
			};
			return View(UserVm);
		}


		[HttpPost]
		public async Task<IActionResult> UserPages(UserPageVM vm)
		{
			var userr = await _userManager.FindByNameAsync(User.Identity.Name);
			if (userr == null) return NotFound();
			if (!ModelState.IsValid)
			{
				return View("Index",vm);
			}
			if(vm.ProfileImage != null)
			{
				if (!vm.ProfileImage.IsCorrectType())
				{
					ModelState.AddModelError("ProfileImage", "Wrong File Type");
				}
				if(!vm.ProfileImage.IsValidSize(2000)) 
				{
					ModelState.AddModelError("ProfileImage", "File cannot larger than");
				}				
			}
			userr.FirstName = vm.Name;
			userr.LastName = vm.Surname;
			userr.UserName = vm.Username;
			userr.Email = vm.Email;

			if(vm.CurrentPassword != null && vm.ConfirmPassword == vm.Password)
			{
				var pr = await _userManager.ChangePasswordAsync(userr, vm.CurrentPassword, vm.Password);
			}	
			

            // user sekil yukleyib yeni
			// 1 kohneni sil eger varsa
			// 2 yenisini save ele
            if (vm.ProfileImage != null) 
			{
				// istifadecinin artiq sekli varsa sil
				if(userr.ProfileImageUrl != null)
				{

				string filePath = Path.Combine(env.WebRootPath, userr.ProfileImageUrl);
				if (System.IO.File.Exists(filePath))
				{
					System.IO.File.Delete(filePath);
				}
				}
				
				// Kohne sekli yaddasdan silmek
				//Yeni sekli save elemek
				userr.ProfileImageUrl = await vm.ProfileImage.SaveAsync(PathConstants.Users);
			}

			
			await _userManager.UpdateAsync(userr);

		
			return RedirectToAction(nameof(Index));
			}
	}
	
}
