using Pustok2.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok2.ViewModel.CategoryVM;
using Pustok2.ViewModel.CommonVM;
using Pustok2.ViewModel.ProductVM;
using Microsoft.AspNetCore.Authorization;

namespace Pustok2.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "SuperAdmin,Admin,Moderator")]
    public class CategoryController : Controller
    {
        PustokDbContext _db { get; }

        public CategoryController(PustokDbContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            int take = 4;
            var items = _db.Categories.Take(take).Select(p => new CategoryListItemVM
            {
                Id = p.Id,
                Name = p.Name,
            });
            int count = await _db.Categories.CountAsync();
            PaginatonVM<IEnumerable<CategoryListItemVM>> pag = new(count, 1, (int)Math.Ceiling((decimal)count / take), items);
            return View(pag);
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
        public async Task<IActionResult> Create(CategoryCreateVM vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            if (await _db.Categories.AnyAsync(x => x.Name == vm.Name))
            {
                ModelState.AddModelError("Name", vm.Name + " already exist");
                return View(vm);
            }
            await _db.Categories.AddAsync(new Models.Category {
                Name = vm.Name,
            });
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Categories.FindAsync(id);
            if (data == null) return NotFound();
            _db.Categories.Remove(data);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Categories.FindAsync(id);
            if (data == null) return NotFound();
            return View(new CategoryUpdateVm
            {
                Name = data.Name,
            });

        }

        [HttpPost]
        public async Task<IActionResult>Update(int? id,CategoryUpdateVm vm)
        {
            if(id==null|| id<=0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            var data = await _db.Categories.FindAsync(id);
            if(data==null) return NotFound();
            data.Name= vm.Name;
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> ProductPagination(int page = 1, int count = 8)
        {

            var items = _db.Categories.Skip((page-1)*count).Take(count).Select(p => new CategoryListItemVM
            {
                Id = p.Id,
                Name = p.Name,
            });
            int Totalcount = await _db.Categories.CountAsync();
            PaginatonVM<IEnumerable<CategoryListItemVM>> pag = new(count, page, (int)Math.Ceiling((decimal)Totalcount / count), items);

            return PartialView("_ProductPaginationPartial", pag);
        }
    }
}
