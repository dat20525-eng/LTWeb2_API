namespace LTWeb2_API.Models.DTO
{
    public class AuthorDTO
    {
        public int Id { get; set; }

        public string FullName { get; set; } = string.Empty;
    }

    public class AuthorNoIdDTO
    {
        public string FullName { get; set; } = string.Empty;
    }
}