using Microsoft.AspNetCore.Mvc;
using Pustok2.Contexts;
using Microsoft.EntityFrameworkCore;
using Pustok2.ViewModel.BlogVM;
using Microsoft.AspNetCore.Mvc.Rendering;
using Pustok2.Models;
using Pustok2.Helpers;
using Microsoft.AspNetCore.Authorization;

namespace Pustok2.Areas.Admin.Controllers
{
    [Area("Admin")]
	/*[Authorize(Roles = "SuperAdmin,Admin,Moderator")]*/
	[Authorize]
	public class BlogController : Controller
    {
        PustokDbContext _db { get; }
        public BlogController(PustokDbContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {

            return View(_db.Blogs.Select(c => new BlogListVM
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                CreatedAt = c.CreatedAt,
                Author = c.Author,
                UptadedAt = c.UptadedAt,
                Tags = c.BlogTag.Select(i => i.Tag),
            }));
        }
        public IActionResult Create()
        {
            ViewBag.Author = _db.Author;
            ViewBag.Tags = new SelectList(_db.Tags.ToList(), "Id", "Title");
            return View();
        }
        public IActionResult Cancel()
        {
            return RedirectToAction(nameof(Index));
        }
        [HttpPost]
        public async Task<IActionResult> Create(BloglistCreateVm vm)
        {
            if (await _db.Tags?.Where(c => vm.TagsId.Contains(c.Id)).Select(c => c.Id).CountAsync() != vm.TagsId?.Count())
            {
                ModelState.AddModelError("TagsId", "TagsId doesnt exist");
                ViewBag.Author = _db.Author;
                ViewBag.Tags = new SelectList(_db.Tags.ToList(), "Id", "Title");
                return View(vm);
            }
            Blog prod = new Blog
            {
                Title = vm.Title,
                Description = vm.Description,
                AuthorId = vm.AuthorId,

                BlogTag = vm.TagsId.Select(id => new BlogTag
                {
                    TagId = id
                }).ToList()
            };
            await _db.Blogs.AddAsync(prod);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult>Update(int? id)
        {
            if (id == null) return BadRequest();
            ViewBag.Author = _db.Author;
            ViewBag.Tags = new SelectList(_db.Tags.ToList(), "Id", "Title");
            var data = await _db.Blogs.FindAsync(id);
            if (data == null) return NotFound();
            return View(new BlogUpdate
            {
                Title = data.Title,
                Description = data.Description,
                AuthorId = data.AuthorId
            });
        }
       

        [HttpPost]
        public async Task<IActionResult>Update(int? id,BlogUpdate vm)
        {
            if (id == null || id <= 0) return BadRequest();
            if (!ModelState.IsValid)
            {
                ViewBag.Author = _db.Author;
                ViewBag.Tags = new SelectList(_db.Tags.ToList(), "Id", "Title");
                return View(vm);
            }
            if(!vm.TagsId.Any())
            {
                ModelState.AddModelError("TagsId", "You must select at least 1 Tag");
            }
            var data = await _db.Blogs
                .Include(p => p.BlogTag)
                .SingleOrDefaultAsync(p => p.Id == id);
            data.Title = vm.Title;
            data.Description = vm.Description;
            data.Author = vm.Author;
            data.AuthorId = vm.AuthorId;
            if (!Enumerable.SequenceEqual(data.BlogTag?.Select(p => p.TagId), vm.TagsId))
            {
                data.BlogTag = vm.TagsId.Select(c => new BlogTag { TagId = c, BlogId = data.Id }).ToList();
            }
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult>Delete(int? id)
        {
            if (id == null) return BadRequest();
            var data = await _db.Blogs.FindAsync(id);
            if (data == null) return NotFound();
            _db.Blogs.Remove(data);
            await _db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
    }
}
