using BusinessObject.Modals;
using DataAccess.Repositories.AuthorRepo;
using DataAccess.Repositories.BookRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookWebAPI.Controllers
{
    [Route("book")]
    [ApiController]
    public class BookAPI : ControllerBase
    {
        private readonly IBookRepository _bookRepository;

        public BookAPI(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }
        [HttpGet]
        public IActionResult GetAllBook()
        {
            var books = _bookRepository.GetAllBook();
            return Ok(books);
        }
        [HttpGet("details/{id}")]
        public IActionResult GetBookById(int id)
        {
            var book = _bookRepository.GetBookByID(id);
            if (book == null)
            {
                return NotFound();
            }
            return Ok(book);
        }
        [HttpPost("add")]
        public IActionResult CreateBook([FromBody] Book book)
        {
            if (book == null)
            {
                return BadRequest();
            }

            bool result = _bookRepository.AddBook(book);
            return result ? StatusCode(201) : StatusCode(409);
        }
        [HttpPut("update/{id}")]
        public IActionResult UpdateBook(int id, [FromBody] Book book)
        {
            if (book == null || book.BookId != id)
            {
                return BadRequest();
            }

            var existingBook = _bookRepository.GetBookByID(id);
            if (existingBook == null)
            {
                return NotFound();
            }

            bool result = _bookRepository.UpdateBook(book);
            return result ? StatusCode(201) : StatusCode(409);
        }
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteBook(int id)
        {
            var bookToDelete = _bookRepository.GetBookByID(id);
            if (bookToDelete == null)
            {
                return NotFound();
            }

            bool result = _bookRepository.DeleteBook(id);
            return result ? StatusCode(201) : StatusCode(409);
        }
    }
}
