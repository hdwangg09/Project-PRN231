using BusinessObject.Modals;
using DataAccess.Repositories.AuthorRepo;
using DataAccess.Repositories.PublisherRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BookWebAPI.Controllers
{
    [Route("publisher")]
    [ApiController]
    public class PublisherAPI : ControllerBase
    {
        private readonly IPublisherRepository _publisherRepository;

        public PublisherAPI(IPublisherRepository publisherRepository)
        {
            _publisherRepository = publisherRepository;
        }
        [HttpGet]
        public IActionResult GetAllPublisher()
        {
            var publisher = _publisherRepository.GetAllPublisher();
            return Ok(publisher);
        }

        [HttpGet("details/{id}")]
        public IActionResult GetPublisherById(int id)
        {
            var publisher = _publisherRepository.GetPublisherByID(id);
            if (publisher == null)
            {
                return NotFound();
            }
            return Ok(publisher);
        }
        [HttpPost("add")]
        public IActionResult CreatePublisher([FromBody] Publisher publisher)
        {
            if (publisher == null)
            {
                return BadRequest();
            }

            bool result = _publisherRepository.AddPublisher(publisher);
            return result ? StatusCode(201) : StatusCode(409);
        }
        [HttpPut("update/{id}")]
        public IActionResult UpdatePublisher(int id, [FromBody] Publisher publisher)
        {
            if (publisher == null || publisher.PibId != id)
            {
                return BadRequest();
            }

            var existingPublisher = _publisherRepository.GetPublisherByID(id);
            if (existingPublisher == null)
            {
                return NotFound();
            }

            bool result = _publisherRepository.UpdatePublisher(publisher);
            return result ? StatusCode(201) : StatusCode(409);
        }
        [HttpDelete("delete/{id}")]
        public IActionResult DeletePublisher(int id)
        {
            var authorToDelete = _publisherRepository.GetPublisherByID(id);
            if (authorToDelete == null)
            {
                return NotFound();
            }

            bool result = _publisherRepository.DeletePublisher(id);
            return result ? StatusCode(201) : StatusCode(409);
        }
    }
}
