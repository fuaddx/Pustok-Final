using Pustok2.Models;
using System.ComponentModel.DataAnnotations;

namespace Pustok2.ViewModel.AuthorVM
{
    public class AuthorCreateVm
    {
        [MaxLength(32)]
        public string Name { get; set; }
        [MaxLength(32)]
        public string Surname { get; set; }
        public IEnumerable<Blog>? Blogs { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}
