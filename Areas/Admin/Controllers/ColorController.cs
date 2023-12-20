/*using Microsoft.AspNetCore.Http.HttpResults;*/
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok2.Contexts;
using Pustok2.ViewModel.ColorVM;


namespace Pustok2.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ColorController : Controller
    {
        PustokDbContext _db { get; }
        public ColorController(PustokDbContext db)
        {
            _db = db;
        }        
        public async Task<IActionResult> Index()
        {
            return View(await _db.Color.Select(c=>new ColorListItemVM
            {
                Id=c.Id,
                Name=c.Name,
                HexCode =c.HexCode
            }).ToListAsync());
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
        public async Task<IActionResult> Create(ColorCreateVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            await _db.Color.AddAsync(new Models.Color
            {
                Name = vm.Name,
                HexCode = vm.HexCode.Substring(1)
        });
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult>Delete (int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Color.FindAsync(id);
            if (data == null) return NotFound();
            _db.Color.Remove(data);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Color.FindAsync(id);
            if (data == null) return NotFound();
            return View(new ColorUpdateVm
            {
                Name = data.Name,
                HexCode = data.HexCode.Substring(1),
            });
        }
        [HttpPost]
        public async Task<IActionResult> Update(int? id,ColorUpdateVm vm)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var data = await _db.Color.FindAsync(id);
            if (data == null) return NotFound();
            data.Name = vm.Name;
            data.HexCode = vm.HexCode.Substring(1);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
