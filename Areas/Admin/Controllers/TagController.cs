using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Pustok2.Contexts;
using Pustok2.Models;
using Pustok2.ViewModel.TagVm;

namespace Pustok2.Areas.Admin.Controllers
{
    [Area("Admin")]
    /*[Authorize(Roles = "SuperAdmin,Admin,Moderator")]*/

    [Authorize]
    public class TagController : Controller
    {
        PustokDbContext _pustokDbContext;

        public TagController(PustokDbContext pustokDbContext)
        {
            _pustokDbContext = pustokDbContext;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _pustokDbContext.Tags.Select(c=> new ListTagVm
            {
                Id = c.Id,
                Title = c.Title,
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
        public async Task<IActionResult> Create(CreateTagVm vm)
        {
            if (!ModelState.IsValid)
            {
                return View(vm);
            }
            await _pustokDbContext.Tags.AddAsync(new Models.Tag
            {
                Title = vm.Title,
            });
            await _pustokDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _pustokDbContext.Tags.FindAsync(id);
            if(data == null) return BadRequest();
            _pustokDbContext.Tags.Remove(data);
            await _pustokDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _pustokDbContext.Tags.FindAsync(id);
            if (data == null) return NotFound();
            return View(new UpdateTagVm
            {
                Title = data.Title,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Update(int? id,UpdateTagVm updateTagVm)
        {
            if(id == null|| id<=0) return BadRequest();
            if (!ModelState.IsValid)
            {
                return View(updateTagVm);
            }
            var data = await _pustokDbContext.Tags.FindAsync(id);
            if(data==null) return NotFound();
            data.Title = updateTagVm.Title;
            await _pustokDbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
