using System.ComponentModel.DataAnnotations;

namespace Pustok2.ViewModel.CategoryVM
{
    public class CategoryCreateVM
    {
        [Required, MaxLength(16)]
        public string Name { get; set; }
    }
}
