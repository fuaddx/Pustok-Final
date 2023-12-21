using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok2.Contexts;
using Pustok2.Helpers;
using Pustok2.Models;
using Pustok2.ViewModel.ProductImagesVm;
using Pustok2.ViewModel.ProductVM;
using Pustok2.ViewModel.SettingVm;

namespace Pustok2.Areas.Admin.Controllers
{

    [Area("Admin")]
	/*[Authorize(Roles = "SuperAdmin,Admin,Moderator")]*/
	[Authorize]
	public class ProductImagesController : Controller
    {
        PustokDbContext _db { get; }
        IWebHostEnvironment _env { get; }

        public ProductImagesController(PustokDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _db.ProductImages.Select(c => new ProductImagesListVm
            {
                Id = c.Id,
                ImagePath = c.ImagePath,
                IsActive = c.IsActive,
                Product = c.Product
            }).ToListAsync());
        }
        public IActionResult Create()
        {
            ViewBag.Product = _db.Products;
            return View();
        }
        public IActionResult Cancel()
        {
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductImagesCreateVm vm)
        {
            /*if (vm.ImageFile != null)
            {
                if (!vm.ImageFile.IsCorrectType())
                {
                    ModelState.AddModelError("ImageFile", "Wrong file type");
                }
                if (!vm.ImageFile.IsValidSize())
                {
                    ModelState.AddModelError("ImageFile", "Files length must be less than kb");
                }
            }*/
            ProductImages productImages = new ProductImages
            {
                ImagePath = vm.ImagePath,
                IsActive = vm.IsActive,
                ProductId = vm.ProductId,
            };

            await _db.ProductImages.AddAsync(productImages);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
		public async Task<IActionResult> Update(int? id)
		{
			if (id == null || id <= 0) return BadRequest();
			var data = await _db.ProductImages.FindAsync(id);
			if (data == null) return NotFound();
			return View(new ProductImagesListVm
			{
				Id = data.Id,
				ImagePath = data.ImagePath,
				IsActive = data.IsActive,
				Product = data.Product
			});
		}
		[HttpPost]
        public async Task<IActionResult> Update(int? id, ProductImagesListVm vm)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);

            }
            var data = await _db.ProductImages.FindAsync(id);
            if (data == null) return NotFound();

            data.Id = vm.Id;
            data.ImagePath = vm.ImagePath;
            data.IsActive = vm.IsActive;
            data.Product = vm.Product;
			await _db.SaveChangesAsync();
			TempData["Salam"] = true;
			return RedirectToAction(nameof(Index));
		}
		}
}
