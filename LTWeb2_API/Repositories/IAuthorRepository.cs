using LTWeb2_API.Models.Domain;
using LTWeb2_API.Models.DTO;

namespace LTWeb2_API.Repositories
{
    public interface IAuthorRepository
    {
        List<AuthorDTO> GellAllAuthors();

        AuthorNoIdDTO GetAuthorById(int id);

        AddAuthorRequestDTO AddAuthor(
            AddAuthorRequestDTO addAuthorRequestDTO);   

        AuthorNoIdDTO UpdateAuthorById(
            int id,
            AuthorNoIdDTO authorNoIdDTO);

        Author? DeleteAuthorById(int id);
    }
}