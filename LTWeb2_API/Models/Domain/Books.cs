using System.ComponentModel.DataAnnotations;

namespace LTWeb2_API.Models.Domain
{
    public class Book
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public bool IsRead { get; set; }

        public DateTime? DateRead { get; set; }

        public int? Rate { get; set; }

        public string Genre { get; set; } = string.Empty;

        public string? CoverUrl { get; set; }

        public DateTime DateAdded { get; set; }


        // One publisher has many books
        public int PublisherID { get; set; }

        public Publisher Publisher { get; set; } = null!;


        // One book has many book_author
        public List<Book_Author> Book_Authors { get; set; }
            = new List<Book_Author>();
    }
}