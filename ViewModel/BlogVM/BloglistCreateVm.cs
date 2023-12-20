using Pustok2.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pustok2.ViewModel.BlogVM
{
    public class BloglistCreateVm
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Author? Author { get; set; }
        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }
        [Required]
        public List<Tag>? Tags { get; set; }
        public IEnumerable<int>? TagsId { get; set; }
    }
}
