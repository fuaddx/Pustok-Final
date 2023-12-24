using Pustok2.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pustok2.ViewModel.ProductVM;

public class ProductCreateVM
{
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
    public int CategoryId { get; set; }
    public IEnumerable<int> ColorIds{ get; set; }
	public IFormFile? MainImage { get; set; }
    public IFormFile HoverImage { get; set; }
    public IEnumerable<IFormFile>? Images { get; set; }
    public Tag?  Tag{ get; set; }
    public IEnumerable<int>? TagsId { get; set; }

}
