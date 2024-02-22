using BusinessObject.Modals;
using DataAccess.Repositories.AuthorRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookWebAPI.Controllers
{
    [Route("author")]
    [ApiController]
    public class AuthorAPI : ControllerBase
    {
        private readonly IAuthorRepository _authorRepository;

        public AuthorAPI(IAuthorRepository authorRepository)
        {
            _authorRepository = authorRepository;
        }
        [HttpGet]
        public IActionResult GetAuthors()
        {
            var authors = _authorRepository.GetAllAuthor();
            return Ok(authors);
        }
        [HttpGet("details/{id}")]
        public IActionResult GetAuthorById(int id)
        {
            var author = _authorRepository.GetAuthorById(id);
            if (author == null)
            {
                return NotFound();
            }
            return Ok(author);
        }
        [HttpPost("add")]
        public IActionResult CreateAuthor([FromBody] Author author)
        {
            if (author == null)
            {
                return BadRequest();
            }

            bool result = _authorRepository.AddAuthor(author);
            return result ? StatusCode(201) : StatusCode(409);
        }
        [HttpPut("update/{id}")]
        public IActionResult UpdateAuthor(int id, [FromBody] Author author)
        {
            if (author == null || author.AuthorId != id)
            {
                return BadRequest();
            }

            var existingAuthor = _authorRepository.GetAuthorById(id);
            if (existingAuthor == null)
            {
                return NotFound();
            }

            bool result = _authorRepository.UpdateAuthor(author);
            return result ? StatusCode(201) : StatusCode(409);
        }
        [HttpDelete("delete/{id}")]
        public IActionResult DeleteAuthor(int id)
        {
            var authorToDelete = _authorRepository.GetAuthorById(id);
            if (authorToDelete == null)
            {
                return NotFound();
            }

            bool result = _authorRepository.DeleteAuthor(id);
            return result ? StatusCode(201) : StatusCode(409);
        }
    }
}
