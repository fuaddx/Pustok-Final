using System.ComponentModel.DataAnnotations;

namespace Pustok2.ViewModel.TagVm
{
    public class CreateTagVm
    {
        [Required]
        public string Title { get; set; }
    }
}
