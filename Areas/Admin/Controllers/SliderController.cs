using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok2.Contexts;
using Pustok2.Models;
using Pustok2.ViewModel.SliderVM;

namespace Pustok2.Areas.Admin.Controllers
{
     [Area("Admin")]
	/* [Authorize(Roles = "SuperAdmin,Admin,Moderator")]*/
	//[Authorize]
	public class SliderController : Controller
    {
        PustokDbContext _db { get; }
        public SliderController(PustokDbContext db)
        {
            _db = db;
        }


        public async Task<IActionResult>Index()
        {
            

           var list = await _db.Sliders.Select(s => new SliderListItemVM
            {
                Title = s.Title,
                Title2 = s.Title2,
                Text = s.Text,
                IsLeft = s.IsLeft,
                ImageUrl = s.ImageUrl,
                Id = s.Id
            }).ToListAsync();  
            return View(list);
        }
        public IActionResult Create()
        {
            return View();
        }
        public IActionResult Cancel()
        {
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Create(SliderCreateVM vm)
        {
            if (vm.Position < -1 || vm.Position > 1)
            {
                ModelState.AddModelError("Position", "Wrong input");
            }
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            Slider slider = new Slider
            {
                Title = vm.Title,
                Title2 = vm.Title2,
                Text = vm.Text,
                ImageUrl = vm.ImageUrl,
                IsLeft = vm.Position switch
                {
                    0 => null,
                    -1 => true,
                    1 => false
                }
            };
            await _db.Sliders.AddAsync(slider);
            await _db.SaveChangesAsync();
            TempData["Created"] = true;
            return RedirectToAction(nameof(Index));
        }
        
        //Update Part
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null || id <= 0) return BadRequest();
            var data = await _db.Sliders.FindAsync(id);
            if (data == null) return NotFound();
            return View(new SliderUpdateVM
            {
                ImageUrl = data.ImageUrl,
                Position = data.IsLeft switch
                {
                    true => -1,
                    null => 0,
                    false => 1
                },
                Text = data.Text,
                Title = data.Title,
                Title2 = data.Title2,
            });
        }
        //Change Somethin In Uptade
        [HttpPost]
        public async Task<IActionResult>Update(int? id, SliderUpdateVM vm)
        {
            if (id == null || id <= 0) return BadRequest();
            if (vm.Position < -1 || vm.Position > 1)
            {
                ModelState.AddModelError("Position", "Wrong Input");
            }
            if (!ModelState.IsValid)
            {
                return View(vm);

            }
            var data = await _db.Sliders.FindAsync(id);
            if (data == null) return NotFound();
            
                if (data.Title != vm.Title || data.Text != vm.Text || data.ImageUrl != vm.ImageUrl || data.IsLeft != (vm.Position switch
                {
                    -1 => true,
                    0 => null,
                    1 => false
                }))
                {
                    data.Title = vm.Title;
                     data.Title2 = vm.Title2;
                    data.Text = vm.Text;
                    data.ImageUrl = vm.ImageUrl;
                    data.IsLeft = vm.Position switch
                    {
                        -1 => true,
                        0 => null,
                        1 => false
                    };
                    await _db.SaveChangesAsync();
                    TempData["Salam"] = true;
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["Salam"] = false;
                }
            return RedirectToAction(nameof(Index));

        }
        public async Task<IActionResult> Delete(int? id)
        {
            TempData["Response"] = false;
            if (id == null) return BadRequest();

            var data = await _db.Sliders.FindAsync(id);
            if (data == null) return NotFound();
            _db.Sliders.Remove(data);
            await _db.SaveChangesAsync();
            TempData["Response"] = true;
            return RedirectToAction(nameof(Index));

        }

    }
}
