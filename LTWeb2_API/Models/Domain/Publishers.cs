using System.ComponentModel.DataAnnotations;

namespace LTWeb2_API.Models.Domain
{
    public class Publisher
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        // One publisher has many books
        public List<Book> Books { get; set; }
            = new List<Book>();
    }
}