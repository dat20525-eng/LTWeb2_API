using LTWeb2_API.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LTWeb2_API.Models.DTO;

namespace LTWeb2_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly AppDbContext _dbContext;

        public BooksController(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        // ===== DÁN GetAllBooks Ở ĐÂY =====

        [HttpGet("get-all-books")]
        public IActionResult GetAllBooks()
        {
            var allBooksDomain = _dbContext.Books;

            var allBooksDTO = allBooksDomain.Select(book =>
                new Models.DTO.BookWithAuthorAndPublisherDTO()
                {
                    Id = book.Id,
                    Title = book.Title,
                    Description = book.Description,
                    IsRead = book.IsRead,
                    DateRead = book.IsRead ? book.DateRead : null,
                    Rate = book.IsRead ? book.Rate : null,
                    Genre = book.Genre,
                    CoverUrl = book.CoverUrl,
                    DateAdded = book.DateAdded,

                    PublisherName = book.Publisher != null
                        ? book.Publisher.Name
                        : "Unknown",

                    AuthorNames = book.Book_Authors
                        .Where(x => x.Author != null)
                        .Select(x => x.Author.FullName)
                        .ToList()
                })
                .ToList();

            return Ok(allBooksDTO);
        }
        [HttpGet]
        [Route("get-book-by-id/{id:int}")]
        public IActionResult GetBookById([FromRoute] int id)
        {
            // Get bookDomain object from Database
            var bookDomain = _dbContext.Books
                .Include(b => b.Publisher)
                .Include(b => b.Book_Authors)
                .ThenInclude(ba => ba.Author)
                .FirstOrDefault(b => b.Id == id);

            if (bookDomain == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy sách"
                });
            }

            // Map Domain Model sang DTO
            var bookDTO =
                new Models.DTO.BookWithAuthorAndPublisherDTO()
                {
                    Id = bookDomain.Id,
                    Title = bookDomain.Title,
                    Description = bookDomain.Description,
                    IsRead = bookDomain.IsRead,
                    DateRead = bookDomain.DateRead,
                    Rate = bookDomain.Rate,
                    Genre = bookDomain.Genre,
                    CoverUrl = bookDomain.CoverUrl,
                    DateAdded = bookDomain.DateAdded,

                    PublisherName =
                        bookDomain.Publisher != null
                        ? bookDomain.Publisher.Name
                        : "Unknown",

                    AuthorNames = bookDomain.Book_Authors
                        .Where(x => x.Author != null)
                        .Select(x => x.Author.FullName)
                        .ToList()
                };

            return Ok(bookDTO);
        }
        [HttpPost("add-book")]
        public IActionResult AddBook(
    [FromBody] AddBookRequestDTO addBookRequestDTO)
        {
            // Check if publisher exists or not
            var publisherDomain = _dbContext.Publishers
                .FirstOrDefault(x =>
                    x.Id == addBookRequestDTO.PublisherID);

            if (publisherDomain == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy NXB"
                });
            }

            // Create a new Book Domain object
            var bookDomain = new Models.Domain.Book()
            {
                Title = addBookRequestDTO.Title,
                Description = addBookRequestDTO.Description,
                IsRead = addBookRequestDTO.IsRead,
                DateRead = addBookRequestDTO.DateRead,
                Rate = addBookRequestDTO.Rate,
                Genre = addBookRequestDTO.Genre,
                CoverUrl = addBookRequestDTO.CoverUrl,
                DateAdded = addBookRequestDTO.DateAdded,
                PublisherID = publisherDomain.Id
            };

            _dbContext.Books.Add(bookDomain);
            _dbContext.SaveChanges();

            // Check authors and add to Book_Author
            foreach (var authorId in addBookRequestDTO.AuthorIds)
            {
                var authorDomain = _dbContext.Authors
                    .FirstOrDefault(x => x.Id == authorId);

                if (authorDomain == null)
                {
                    return NotFound(new
                    {
                        message = "Không tìm thấy tác giả"
                    });
                }

                var bookAuthorDomain =
                    new Models.Domain.Book_Author()
                    {
                        BookId = bookDomain.Id,
                        AuthorId = authorDomain.Id
                    };

                _dbContext.Books_Authors.Add(bookAuthorDomain);
                _dbContext.SaveChanges();
            }

            return Ok();
        }



        [HttpPut("update-book-by-id/{id:int}")]
        public IActionResult UpdateBookById(
    int id,
    [FromBody] AddBookRequestDTO addBookRequestDTO)
        {
            var bookDomain = _dbContext.Books
                .FirstOrDefault(x => x.Id == id);

            if (bookDomain != null)
            {
                bookDomain.Title = addBookRequestDTO.Title;
                bookDomain.Description = addBookRequestDTO.Description;
                bookDomain.IsRead = addBookRequestDTO.IsRead;
                bookDomain.DateRead = addBookRequestDTO.DateRead;
                bookDomain.Rate = addBookRequestDTO.Rate;
                bookDomain.Genre = addBookRequestDTO.Genre;
                bookDomain.CoverUrl = addBookRequestDTO.CoverUrl;
                bookDomain.DateAdded = addBookRequestDTO.DateAdded;
                bookDomain.PublisherID = addBookRequestDTO.PublisherID;

                _dbContext.SaveChanges();
            }

            var existingBookAuthors = _dbContext.Books_Authors
                .Where(x => x.BookId == id)
                .ToList();

            if (existingBookAuthors != null &&
                existingBookAuthors.Count > 0)
            {
                _dbContext.Books_Authors
                    .RemoveRange(existingBookAuthors);

                _dbContext.SaveChanges();
            }

            foreach (var authorId in addBookRequestDTO.AuthorIds)
            {
                var authorDomain = _dbContext.Authors
                    .FirstOrDefault(x => x.Id == authorId);

                if (authorDomain == null)
                {
                    return NotFound();
                }

                var bookAuthorDomain =
                    new Models.Domain.Book_Author()
                    {
                        BookId = bookDomain!.Id,
                        AuthorId = authorDomain.Id
                    };

                _dbContext.Books_Authors.Add(bookAuthorDomain);
                _dbContext.SaveChanges();
            }

            return Ok(addBookRequestDTO);
        }


        [HttpDelete("delete-book-by-id/{id:int}")]
        public IActionResult DeleteBookById(int id)
        {
            // Tìm Book theo Id
            var bookDomain = _dbContext.Books
                .FirstOrDefault(x => x.Id == id);

            // Nếu không tìm thấy Book
            if (bookDomain == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy sách"
                });
            }

            // Tìm các tác giả đang liên kết với Book
            var existingBookAuthors = _dbContext.Books_Authors
                .Where(x => x.BookId == id)
                .ToList();

            // Xóa dữ liệu trong bảng trung gian trước
            if (existingBookAuthors != null &&
                existingBookAuthors.Count > 0)
            {
                _dbContext.Books_Authors
                    .RemoveRange(existingBookAuthors);

                _dbContext.SaveChanges();
            }

            // Xóa Book
            _dbContext.Books.Remove(bookDomain);

            _dbContext.SaveChanges();

            return Ok(bookDomain);
        }
    } 
} 