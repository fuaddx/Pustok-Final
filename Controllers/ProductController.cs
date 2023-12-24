using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using NuGet.Versioning;
using Pustok2.Contexts;
using Pustok2.ViewModel.BasketVM;
using Pustok2.ViewModel.ProductVM;

namespace Pustok2.Controllers
{
    public class ProductController : Controller
    {
        PustokDbContext _db { get; }

        public ProductController(PustokDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index(string? q,List<int>? catIds,List<int>? tagIds)
        {
            var queries = HttpContext.Request.Query;
            ViewBag.Categories = _db.Categories.Include(c=>c.Products);
            ViewBag.Tags = _db.Tags;
            return View();
        }
        /*[HttpPost]
        public async Task<IActionResult> Index(string? q,List<int>? catIds, )
        {
            ViewBag.Categories = _db.Categories.Include(c => c.Products);
            ViewBag.ProductCount = await _db.Products.CountAsync();
            ViewBag.Tags = _db.Tags;
            return View();
        }*/
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var data = await _db.Products.Select(p => new ProductListVM
            {
                Discount = p.Discount,
                Category = p.Category,
                Colors = p.ProductColors.Select(p => p.Color),
                Id = p.Id,
                ImageUrl = p.ImageUrl,
                ImageUrls = p.ProductImages.Select(p => p.ImagePath),
                Name = p.Name,
                Quantity = p.Quantity,
                Description = p.Description,
                About = p.About,
                SellPrice = (p.SellPrice - (p.SellPrice * (decimal)p.Discount) / 100)
            }).SingleOrDefaultAsync(p => p.Id == id);
            if (data == null) return NotFound();
            return View(data);
        }
        public async Task<IActionResult> AddBasket(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!await _db.Products.AnyAsync(p => p.Id == id)) return NotFound();
            var basket = JsonConvert.DeserializeObject<List<BasketProductAndCountVM>>(HttpContext.Request.Cookies["basket"] ?? "[]");
            var existItem = basket.Find(b=>b.Id == id);
            if (existItem == null)
            {
                basket.Add(new BasketProductAndCountVM() 
                {
                    Id =(int)id,
                    Count = 1
                });
            }
            else
            {
                existItem.Count++;
            }
            HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket));
            return Ok();
        }

        public async Task<IActionResult>RemoveBasket(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!await _db.Products.AnyAsync(p => p.Id == id)) return NotFound();
            var dasket = JsonConvert.DeserializeObject<List<BasketProductAndCountVM>>(HttpContext.Request.Cookies["basket"] ?? "[]");
            var existItem = dasket.Find(b => b.Id == id);
            if (existItem.Count == 1)
            {
                dasket.Remove(new BasketProductAndCountVM()
                {
                    Id = (int)id,
                    Count = 1
                });
            }
            else
            {
                existItem.Count--;
            }
            HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(dasket));
            return Ok();
        }
    }
}
