using BusinessObject.DTOs;
using BusinessObject.Modals;
using DataAccess.Repositories.BookRepo;
using DataAccess.Repositories.UserRepo;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace BookWebAPI.Controllers
{
	[Route("user")]
	[ApiController]
	public class UserAPI : ControllerBase
	{
		private readonly IUserRepository _userRepository;

		public UserAPI(IUserRepository userRepository)
		{
			_userRepository = userRepository;
		}

		[HttpGet("details/{id}")]
		public IActionResult Details(int id)
		{
			User user = _userRepository.GetUserByID(id);
			return user != null ? Ok(user) : StatusCode(409);
		}

		[HttpPost("updateProfile")]
		public IActionResult UpdateProfile([FromBody] User user)
		{
			if (user == null)
			{
				return BadRequest();
			}
			bool result = _userRepository.UpdateUser(user);
			return result ? StatusCode(201) : StatusCode(409);
		}
		[HttpPost("login")]
		public IActionResult Login([FromBody] User? user)
		{
			if (user == null)
			{
				return BadRequest();
			}
			User result = _userRepository.Login(user);
			return result != null ? Ok(result) : StatusCode(409);
		}
		[HttpPost("register")]
		public IActionResult Register([FromBody] User user)
		{
			if (user == null)
			{
				return BadRequest();
			}
			if (_userRepository.GetUserByEmail(user.EmailAddress) != null)
			{
				return UnprocessableEntity("Email already used!");
			}
			user.RoleId = 2;
			bool result = _userRepository.Register(user);
			return result ? Ok() : Conflict();
		}
        [HttpPost("changePassword")]
        public IActionResult ChangePassword([FromBody] User user)
        {
            if (user == null)
            {
                return BadRequest();
            }
            var result = _userRepository.ChangePassword(user);
           
            return result ? Ok() : Conflict();
        }
        [HttpPost("isPasswordUser")]
        public IActionResult isPasswordUser(int userId, string oldPassword)
        {
            if (oldPassword == null)
            {
                return BadRequest();
            }
            var result = _userRepository.IsPasswordUser(userId, oldPassword);

            return result ? Ok() : Conflict();
        }
        [HttpPost("existEmail")]
        public IActionResult ExistEmail(string email)
        {
            var result = _userRepository.checkExistEmail(email);
			if(result == true)
			{
                return StatusCode(200);
            }
			else
			{
				return StatusCode(500);
			}
        }
    }
}
