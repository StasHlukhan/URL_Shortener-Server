using Microsoft.AspNetCore.Mvc;
using URL_Shortener_Server.Interfaces;
using URL_Shortener_Server.Models;

namespace URL_Shortener_Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUserRepository _userRepository;

        public AuthController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] User user)
        {
            if (string.IsNullOrWhiteSpace(user.Username) || string.IsNullOrWhiteSpace(user.Password))
            {
                return BadRequest("Невірні дані користувача.");
            }

            var existingUser = _userRepository.GetUserByUsername(user.Username);
            if (existingUser != null)
            {
                return Conflict("Користувач з таким іменем вже існує.");
            }

            _userRepository.AddUser(user);

            return Ok("Користувач зареєстрований успішно.");
        }

        [HttpPost("login")]
        public IActionResult UserLogin([FromBody] User model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = _userRepository.GetUserByUsername(model.Username);

            if (user != null && user.Password == model.Password)
            {
                return Ok(new { message = "Успішно ввійшли", user });
            }
            else
            {
                return BadRequest("Неправильний логін або пароль");
            }
        }

    }
}