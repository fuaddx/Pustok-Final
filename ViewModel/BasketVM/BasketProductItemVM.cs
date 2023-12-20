using Pustok2.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Pustok2.ViewModel.BasketVM
{
    public class BasketProductItemVM
    {
        public int Id { get; set; }

        public int Count { get; set; }
        public string Name { get; set; }
        public decimal SellPrice { get; set; }
        public decimal CostPrice { get; set; }
        public float Discount { get; set; }
        public string? ImageUrl { get; set; }
    }
}
