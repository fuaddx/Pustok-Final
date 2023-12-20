using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pustok2.Models
{
    public class Blog
    {
        public int Id { get; set; }

        [MaxLength(64)]
        public string Title { get; set; }
        public string Description { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime? UptadedAt { get; set; }

        public Author? Author { get; set; }

        [ForeignKey(nameof(Author))]
        public int AuthorId { get; set; }

        public bool IsDeleted { get; set; } = false;

        public IEnumerable<BlogTag>? BlogTag { get; set; }

    }
}
