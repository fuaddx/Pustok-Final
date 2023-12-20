using Pustok2.ViewModel.CategoryVM;
using Pustok2.ViewModel.ProductVM;
using Pustok2.ViewModel.SliderVM;
using Pustok2.ViewModel.CommonVM;
using Pustok2.ViewModel.AuthorVM;

namespace Pustok2.ViewModel.HomeVm
{
    public class HomeVm
    {
        public IEnumerable<SliderListItemVM> Sliders { get; set; }
        public IEnumerable<ProductListVM> Products { get; set; }
        public PaginatonVM<IEnumerable<CategoryListItemVM>> PaginatedCategories { get; set; }
        public PaginatonVM<IEnumerable<AuthorListVm>> PaginatedAuthors { get; set; }
        public IEnumerable<CategoryListItemVM> Categories { get; set; }
        public IEnumerable<ProductCreateVM>ProductCreates { get; set; }
    }
}
