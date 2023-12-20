/*using Pustok2.ViewModel.BasketVM;*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Pustok2.Areas.Admin.Controllers;
using Pustok2.Contexts;
using Pustok2.Models;
/*using Pustok2.ViewModel.BasketVM;*/
using Pustok2.ViewModel.HomeVm;
using Pustok2.ViewModel.ProductVM;
using Pustok2.ViewModel.SliderVM;
using System.Diagnostics;

namespace Pustok2.Controllers
{
    public class HomeController : Controller
    {
        PustokDbContext _context { get; }

        public HomeController(PustokDbContext context)
        {
            _context = context;
        }
        /*1 ci versiya*/
        /*public async Task<IActionResult> Index()
        {
            var sliders = await _context.Sliders.ToListAsync();
            var products = await _context.Products.ToListAsync();
            ProductSliderVM md = new ProductSliderVM();
            md.Sliders = sliders;
            md.Products = products;
            return View(md);
        }*/
        /*2 ci versiya*/
        public async Task<IActionResult> Index() {

            HomeVm vm = new HomeVm
            {
                Sliders = await _context.Sliders.Select(s => new SliderListItemVM
                {
                    Id = s.Id,
                    ImageUrl = s.ImageUrl,
                    IsLeft = s.IsLeft,
                    Title = s.Title,
                    Text = s.Text,
                }).ToListAsync(),
                Products = await _context.Products.Where(p => !p.IsDeleted).Select(p => new ProductListVM
                {
                    Id = p.Id,
                    Category = p.Category,
                    Discount = p.Discount,
                    Name = p.Name,
                    ImageUrl = p.ImageUrl,
                    UrlImage2 = p.UrlImage2,
                    Quantity = p.Quantity,
                    SellPrice = p.SellPrice,
                    CostPrice = p.CostPrice,
                    About = p.About,
                }).ToListAsync()
            };
            return View(vm);
        }


        /*        public async Task<IActionResult> AddBasket(int? id)
                {
                    if (id == null || id <= 0) return BadRequest();
                    if (!await _context.Products.AnyAsync(p => p.Id == id)) return NotFound();
                    var basket = JsonConvert.DeserializeObject<List<BasketProductAndCountVM>>(HttpContext.Request.Cookies["basket"] ?? "[]");
                    var existItem = basket.Find(b => b.Id == id);
                    if (existItem == null)
                    {
                        basket.Add(new BasketProductAndCountVM
                        {
                            Id = (int)id,
                            Count = 1
                        });
                    }
                    else
                    {
                        existItem.Count++;
                    }
                    HttpContext.Response.Cookies.Append("basket", JsonConvert.SerializeObject(basket), new CookieOptions
                    {
                        MaxAge = TimeSpan.MaxValue
                    });
                    return Ok();
                }
        */

        /* public string GetSession(string key)
         {
             return HttpContext.Session.GetString(key) ?? "";
         }*/
        /*public void SetSession(string key,string value) 
        {
            HttpContext.Session.SetString(key, value);
        }*/
        public string GetCookie(string key)
        {
            return HttpContext.Request.Cookies[key] ?? "";
        }
        public IActionResult GetBasket()
        {   
            return ViewComponent("Basket");
        }
        /*public void SetCookie(string key,string value)
        {
            HttpContext.Response.Cookies.Append(key, value, new CookieOptions
            {
                MaxAge = TimeSpan.FromSeconds(30)
            }); 
        }*/
    }
}