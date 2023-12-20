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
		PustokDbContext _db { get; }
		UserManager<AppUser> _userManager { get; }
		public UserPageController(PustokDbContext db, UserManager<AppUser> userManager)
		{
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
				Name = userr.Fullname,
				Surname = userr.Fullname,
				ProfileImageURL = userr.ProfileImageUrl,
			};
			return View(UserVm);
		}
		/*
		public IActionResult Index()
		{
			return View();
		}
		 */

		/*
		 public async Task<IActionResult> UserPages(string? user)
		{
			if (user == null) return BadRequest();
			var name = User.Identity.Name;
			if (name == null || name != user) return NotFound();
			var userr = await _userManager.FindByNameAsync(user);
			if (userr == null) return NotFound();
			var UserVm = new UserPageVM
			{
				Username = userr.UserName,
				Email = userr.Email,
				Name = userr.Fullname,
				Surname = userr.Fullname,
				ProfileImageURL = userr.ProfileImageUrl,
			};
			return View(UserVm);
		}
		 */

		/*
		 * 		[HttpPost]
		public async Task<IActionResult> UserPages(string? user , UserPageVM vm)
		{
			var userr = await _userManager.FindByNameAsync(user);
			if (userr == null) return NotFound();
			if (!ModelState.IsValid)
			{
				return View(vm);
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
			if (vm.ProfileImage != null)
			{
				string filePath = Path.Combine(PathConstants.RootPath, userr.ProfileImageUrl);
				if (System.IO.File.Exists(filePath))
				{
					System.IO.File.Delete(filePath);
				}
				// Kohne sekli yaddasdan silmek

				userr.ProfileImageUrl = await vm.ProfileImage.SaveAsync(PathConstants.Product);
				//Yeni sekli save elemek
			}

			userr.Fullname = vm.Name + " " + vm.Surname;
			userr.Email = vm.Email;

			await _userManager.UpdateAsync(userr);

			return RedirectToAction("UserPages","Userpage");
		}
		 */


		[HttpPost]
		public async Task<IActionResult> UserPages(UserPageVM vm)
		{
			var userr = await _userManager.FindByNameAsync(User.Identity.Name);
			if (userr == null) return NotFound();
			if (!ModelState.IsValid)
			{
				// View adi yazmamisdin, gedib UserPages.cshtml axtarirdi "Index" yazdm cunki sende Index.cshtml di
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
			if (vm.ProfileImage != null)
			{
				string filePath = Path.Combine(PathConstants.RootPath, userr.ProfileImageUrl);
				if (System.IO.File.Exists(filePath))
				{
					System.IO.File.Delete(filePath);
				}
				// Kohne sekli yaddasdan silmek

				userr.ProfileImageUrl = await vm.ProfileImage.SaveAsync(PathConstants.Product);
				//Yeni sekli save elemek
			}

			userr.Fullname = vm.Name + " " + vm.Surname;
			userr.Email = vm.Email;

			await _userManager.UpdateAsync(userr);

			// burdada Indexe redirect elemek lazimdi, sende UserPages actionunu deyisdim Indexnen cunki view adin uygun gelmirdi
			return RedirectToAction(nameof(Index));
			}
	}
	
}
