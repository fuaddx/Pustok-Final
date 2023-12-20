namespace Pustok2.Models
{
    public class Tag
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public IEnumerable<BlogTag>? TagBlog { get; set; }
    }
}
        