using Pustok2.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pustok2.ViewModel.BlogVM
{
    public class BlogUpdate
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Author? Author { get; set; }
        public int AuthorId { get; set; }
        public List<Tag>? Tags { get; set; }
        public IEnumerable<int>? TagsId { get; set; }
    }
}
