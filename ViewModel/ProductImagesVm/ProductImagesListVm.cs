using Pustok2.Models;

namespace Pustok2.ViewModel.ProductImagesVm
{
    public class ProductImagesListVm
    {
        public int Id { get; set; }
        public string ImagePath { get; set; }
        public Product Product { get; set; }

        public IFormFile ImageFile { get; set; }
        public bool IsActive { get; set; }


    }
}
