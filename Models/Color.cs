using System.ComponentModel.DataAnnotations;

namespace Pustok2.Models
{
    public class Color
    {
        public int Id { get; set; }
        [MaxLength(32)]
        public string Name { get; set; }
        [MaxLength(6), MinLength(3)]
        public string HexCode { get; set; }
        public ICollection<ProductColor> ProductColors { get; set; }
    }
}
