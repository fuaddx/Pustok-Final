using System.ComponentModel.DataAnnotations;

namespace Pustok2.ViewModel.ColorVM
{
    public class ColorCreateVm
    {
        [Required,MaxLength(32)]
        public string Name { get; set; }
        [Required,MaxLength(7),MinLength(7)]
        public string HexCode { get; set; }
    }
}
