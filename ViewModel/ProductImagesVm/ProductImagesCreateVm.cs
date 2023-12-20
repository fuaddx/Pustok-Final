using Pustok2.Models;

namespace Pustok2.ViewModel.ProductImagesVm
{
    public class ProductImagesCreateVm
    {
        public string ImagePath { get; set; }
        public Product Product { get; set; }
        public bool IsActive { get; set; }
        public IFormFile ImageFile { get; set; }
        public int ProductId { get; set; }
    }
}
