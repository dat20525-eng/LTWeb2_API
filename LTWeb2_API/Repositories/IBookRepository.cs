using LTWeb2_API.Models.Domain;
using LTWeb2_API.Models.DTO;

namespace LTWeb2_API.Repositories
{
    public interface IBookRepository
    {
        List<BookWithAuthorAndPublisherDTO> GetAllBooks();

        BookWithAuthorAndPublisherDTO GetBookById(int id);

        AddBookRequestDTO AddBook(
            AddBookRequestDTO addBookRequestDTO);

        AddBookRequestDTO? UpdateBookById(
            int id,
            AddBookRequestDTO bookDTO);

        Book? DeleteBookById(int id);
    }
}