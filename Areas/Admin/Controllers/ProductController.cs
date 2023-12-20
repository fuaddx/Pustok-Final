using Pustok2.Contexts;
using Pustok2.Models;
using Pustok2.ViewModel.ProductVM;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok2.Helpers;
using Microsoft.AspNetCore.Mvc.Rendering;

using Pustok2.Migrations;
using Microsoft.AspNetCore.Identity;

namespace Pustok2.ViewModel.ProductVM
{
    [Area("Admin")]
    public class ProductController : Controller
    {
       PustokDbContext _db { get; }
        IWebHostEnvironment _env { get; }

        public ProductController(PustokDbContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }

        public IActionResult Index()
        {

            return View(_db.Products.Select(p => new ProductListVM
            {
                Id = p.Id,
                Name = p.Name,
                CostPrice = p.CostPrice,
                Discount = p.Discount,
                Category = p.Category,
                ProductCode = p.ProductCode,
                About = p.About,
                Description = p.Description,
                ImageUrl = p.ImageUrl,
                UrlImage2 = p.UrlImage2,
                IsDeleted = p.IsDeleted,
                Quantity = p.Quantity,
                SellPrice = p.SellPrice,
                Colors = p.ProductColors.Select(p => p.Color)
            }));
        }
        public IActionResult Create()
        {
            ViewBag.Categories = _db.Categories;
            ViewBag.Colors = new SelectList(_db.Color, "Id", "Name");
            return View();
        }
        public IActionResult Cancel()
        {
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductCreateVM vm)
        {
            if(vm.MainImage!= null)
            {
                if(!vm.MainImage.IsCorrectType())
                {
                    ModelState.AddModelError("MainImage", "Wrong fie type");
                }
                if(!vm.MainImage.IsValidSize(200))
                {
                    ModelState.AddModelError("MainImage", "Image must less than given kb");
                }
            }
            if (vm.HoverImage != null)
            {
                if (!vm.HoverImage.IsCorrectType())
                {
                    ModelState.AddModelError("HoverImage", "Wrong file type");
                }
                if (!vm.HoverImage.IsValidSize(200))
                {
                    ModelState.AddModelError("HoverImage", "Files length must be less than kb");
                }
            }
            if (vm.Images != null)
            {
                foreach (var img in vm.Images)
                {
                    if (!img.IsCorrectType())
                    {
                        ModelState.AddModelError("", "Wrong file type (" + img.FileName + ")");
                    }
                    if (!img.IsValidSize(600))
                    {
                        ModelState.AddModelError("", "Files length must be less than kb (" + img.FileName + ")");

                    }
                }
            }
            if (vm.CostPrice > vm.SellPrice)
            {
                ModelState.AddModelError("CostPrice", "Sell price must be bigger than cost price");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Categories = _db.Categories;
                ViewBag.Colors = new SelectList(_db.Color, "Id", "Name");
                return View(vm);
            }
            if (!await _db.Categories.AnyAsync(c => c.Id == vm.CategoryId))
            {
                ModelState.AddModelError("CategoryId", "Category doesnt exist");
                ViewBag.Categories = _db.Categories;
                ViewBag.Colors = new SelectList(_db.Color, "Id", "Name");
                return View(vm);
            }
            if (await _db.Color.Where(c => vm.ColorIds.Contains(c.Id)).Select(c => c.Id).CountAsync() != vm.ColorIds.Count())
            {
                ModelState.AddModelError("ColorIds", "Color doesnt exist");
                ViewBag.Categories = _db.Categories;
                ViewBag.Colors = new SelectList(_db.Color, "Id", "Name");
                return View(vm);
            }
            Product prod = new Product
            {
                Name = vm.Name,
                About = vm.About,
                Quantity = vm.Quantity,
                Description = vm.Description,
                Discount = vm.Discount,
                ProductCode = vm.ProductCode,
                ImageUrl = await vm.MainImage.SaveAsync(PathConstants.Product),
                CostPrice = vm.CostPrice,
                SellPrice = vm.SellPrice,
                UrlImage2= await vm.HoverImage.SaveAsync(PathConstants.Product),
                CategoryId = vm.CategoryId,
                ProductColors = vm.ColorIds.Select(id => new ProductColor
                {
                    ColorId = id,
                }).ToList(),
                ProductImages = vm.Images.Select(i => new ProductImages
                {
                    ImagePath = i.SaveAsync(PathConstants.Product).Result
                }).ToList()
            };

            await _db.Products.AddAsync(prod);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            ViewBag.Colors = new SelectList(_db.Color, "Id", "Name");
            ViewBag.Categories = _db.Categories;
            var data = await _db.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductColors)
                .SingleOrDefaultAsync(p => p.Id == id);
            //.Include(p => p.ProductColors)
            //    .ThenInclude(pc => pc.Color)
            //.Include(p => p.Category)
            if (data == null) return NotFound();


            var vm = new ProductUpdateVm
            {
                
                About = data.About,
                CategoryId = data.CategoryId,
                ColorIds = data.ProductColors?.Select(i => i.ColorId),
                CostPrice = data.CostPrice,
                Description = data.Description,
                Discount = data.Discount,
                Name = data.Name,
                ProductCode = data.ProductCode,
                Quantity = data.Quantity,
                SellPrice = data.SellPrice,
                ImageUrls = data.ProductImages?.Select(pi => new ProductImgVm
                {
                    Id = pi.Id,
                    Url = pi.ImagePath
                }),
                CoverImageUrl = data.ImageUrl
            };

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id, ProductUpdateVm vm)
        {
            if (id == null || id <= 0) return BadRequest();

            if (vm. MainImage != null)
            {
                if (!vm.MainImage.IsCorrectType())
                {
                    ModelState.AddModelError("ImageFile", "Wrong file type");
                }
                if (!vm.MainImage.IsValidSize())
                {
                    ModelState.AddModelError("ImageFile", "Files length must be less than kb");
                }
            }
            if (vm.Images != null)
            {
                //string message = string.Empty;
                foreach (var img in vm.Images)
                {
                    if (!img.IsCorrectType())
                    {
                        ModelState.AddModelError("", "Wrong file type (" + img.FileName + ")");
                        //message += "Wrong file type (" + img.FileName + ") \r\n";
                    }
                    if (!img.IsValidSize(200))
                    {
                        ModelState.AddModelError("", "Files length must be less than kb (" + img.FileName + ")");
                        //message += "Files length must be less than kb (" + img.FileName + ") \r\n";
                    }
                }
            }
            if (vm.CostPrice > vm.SellPrice)
            {
                ModelState.AddModelError("CostPrice", "Sell price must be bigger than cost price");
            }

            if (!vm.ColorIds.Any())
            {
                ModelState.AddModelError("ColorIds", "You must add at least 1 color");
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Colors = new SelectList(_db.Color, "Id", "Name");
                ViewBag.Categories = _db.Categories;
                return View(vm);
            }

            var data = await _db.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductColors)
                .SingleOrDefaultAsync(p => p.Id == id);
            data.Name = vm.Name;
            data.About = vm.About ;
            data.CategoryId = vm.CategoryId;
            data.Quantity = vm.Quantity;
            data.SellPrice = data.SellPrice;
            data.ProductCode = vm.ProductCode;
            vm.CoverImageUrl = data.ImageUrl;
            if (data == null) return NotFound();

            if (vm.MainImage != null)
            {
                string filePath = Path.Combine(PathConstants.RootPath, data.ImageUrl);
                if (System.IO.File.Exists(filePath))
                {
                    System.IO.File.Delete(filePath);
                }
                // Kohne sekli yaddasdan silmek

                data.ImageUrl = await vm.MainImage.SaveAsync(PathConstants.Product);
                //Yeni sekli save elemek
            }
            if (vm.Images != null)
            {
                var imgs = vm.Images.Select(i => new ProductImages
                {
                    ImagePath = i.SaveAsync(PathConstants.Product).Result,
                    ProductId = data.Id
                });

                data.ProductImages.AddRange(imgs);
            }

            if (!Enumerable.SequenceEqual(data.ProductColors?.Select(p => p.ColorId), vm.ColorIds))
            {
                data.ProductColors = vm.ColorIds.Select(c => new ProductColor { ColorId = c, ProductId = data.Id }).ToList();
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DeleteImage(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.ProductImages.FindAsync(id);
            if (data == null) return NotFound();
            _db.ProductImages.Remove(data);
            await _db.SaveChangesAsync();
            return Ok();
        }
        public async Task<IActionResult> DeleteImageCSharp(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.ProductImages.FindAsync(id);
            if (data == null) return NotFound();
            _db.ProductImages.Remove(data);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Update), new { id = data.ProductId });
        }

        public async Task<IActionResult>DeleteProduct(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Products.FindAsync(id);
            if (data == null) return NotFound();
            _db.Products.Remove(data);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

       
       
    }
}
