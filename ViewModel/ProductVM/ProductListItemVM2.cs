using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Pustok2.Models;

namespace Pustok2.ViewModel.ProductVM
{
    public class ProductListItemVM2
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? About { get; set; }
        public string? Description { get; set; }
        public float SellPrice { get; set; }
        public float Discount { get; set; }
        public ushort Quantity { get; set; }
        public string? ProductCode { get; set; }
        public int CategoryId { get; set; }
        public bool IsDeleted { get; set; } = false;
        public IEnumerable<Tag>? Tags { get; set; }
       
    }
}
