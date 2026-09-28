using LTWeb2_API.Models.Domain;
using LTWeb2_API.Models.DTO;

namespace LTWeb2_API.Repositories
{
    public interface IBookRepository
    {
        List<BookWithAuthorAndPublisherDTO> GetAllBooks(
    string? filterOn = null,
    string? filterQuery = null,
    string? sortBy = null,
    bool isAscending = true,
    int pageNumber = 1,
    int pageSize = 1000);

        BookWithAuthorAndPublisherDTO GetBookById(int id);

        AddBookRequestDTO AddBook(
            AddBookRequestDTO addBookRequestDTO);

        AddBookRequestDTO? UpdateBookById(
            int id,
            AddBookRequestDTO bookDTO);

        Book? DeleteBookById(int id);
    }
}