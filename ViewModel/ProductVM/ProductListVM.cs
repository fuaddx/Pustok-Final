using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Pustok2.Models;

namespace Pustok2.ViewModel.ProductVM
{
    public class ProductListVM
    {
        public int Id { get; set; }
        [MaxLength(64)]
        public string Name { get; set; }
        [MaxLength(128)]
        public string? About { get; set; }
        public string? Description { get; set; }
        [Column(TypeName = "smallmoney")]
        public decimal SellPrice { get; set; }
        [Column(TypeName = "smallmoney")]   
        public decimal CostPrice { get; set; }
        [Range(0, 100)]
        public float Discount { get; set; }
        public ushort Quantity { get; set; }
        public string? ProductCode { get; set; }
        public Category? Category { get; set; }
        public int CategoryId { get; set; }
        public string? ImageUrl { get; set; }
        public string? UrlImage2 { get; set; }
        public bool IsDeleted { get; set; } = false;
		public IEnumerable<string> ImageUrls { get; set; }
		public IEnumerable<Color> Colors { get; set; }
        public IEnumerable<Tag>? Tags { get; set; }
    }
}
