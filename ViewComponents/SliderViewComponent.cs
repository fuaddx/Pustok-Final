using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pustok2.Contexts;

using Pustok2.ViewModel.SliderVM;

namespace Pustok2.ViewComponents
{
    public class SliderViewComponent: ViewComponent
    {
        PustokDbContext _db { get; }
        public SliderViewComponent(PustokDbContext db)
        {
            _db = db;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View(await _db.Sliders.Select(p => new SliderListItemVM
            {
                Id = p.Id,
                ImageUrl = p.ImageUrl,
                IsLeft = p.IsLeft,
                Title2 = p.Title2,
                Title = p.Title,
                Text = p.Text,
            }).ToListAsync());
        }

    }
}
