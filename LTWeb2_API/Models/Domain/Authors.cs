using System.ComponentModel.DataAnnotations;

namespace LTWeb2_API.Models.Domain
{
    public class Author
    {
        [Key]
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;

        // One author has many book_author
        public List<Book_Author> Book_Authors { get; set; }
            = new List<Book_Author>();
    }
}