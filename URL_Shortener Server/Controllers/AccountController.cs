using Microsoft.AspNetCore.Mvc;
using URL_Shortener_Server.Interfaces;
using URL_Shortener_Server.Models;

[Route("api/[controller]")]
[ApiController]
public class AuthController : Controller
{
    private readonly IUserRepository _userRepository;
    private readonly IAdminRepository _adminRepository;

    public AuthController(IUserRepository userRepository, IAdminRepository adminRepository)
    {
        _userRepository = userRepository;
        _adminRepository = adminRepository;
    }
    [HttpPost("user/login")]
    public IActionResult UserLogin([FromBody] User model)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState); // Повертаємо помилки валідації, якщо дані некоректні
        }

        // Далі виконуємо логіку авторизації користувача
        User user = _userRepository.GetUserByUsername(model.Username);

        if (user != null && user.Password == model.Password)
        {
            return Ok(new { message = "Успішно ввійшли", user });
        }
        else
        {
            return Ok(new { success = false, message = "Неправильний логін або пароль" });
        }
    }



    [HttpGet("admin/login")]
    public IActionResult AdminLogin()
    {
        return View();
    }

    // Обробка POST-запиту для логіну адміністратора
    [HttpPost("admin/login")]
    public IActionResult AdminLogin(string username, string password)
    {
        // Перевірка адміністратора в базі даних
        Admin admin = _adminRepository.GetAdminByUsername(username);

        if (admin != null && admin.Password == password)
        {
            // Успішний логін адміністратора - додайте код для авторизації адміністратора

            return RedirectToAction("AdminDashboard", "Admin");
        }
        else
        {
            // Невдалий логін - повідомлення про помилку
            ViewBag.ErrorMessage = "Неправильний логін або пароль";
            return View();
        }
    }
    [HttpPost("user/register")]
    public IActionResult UserRegister(string username, string password)
    {
        // Перевірка, чи користувач з таким іменем існує вже в базі даних
        if (_userRepository.GetUserByUsername(username) != null)
        {
            ModelState.AddModelError("", "user already exists");
            return StatusCode(422, ModelState);
        }

        // Створення нового користувача і збереження його в базі даних
        User newUser = new User
        {
            Username = username,
            Password = password
        };

        _userRepository.AddUser(newUser);

        // Додайте код для авторизації нового користувача
        return Ok(newUser);

    }

    [HttpPost("admin/register")]
    public IActionResult AdminRegister(string username, string password)
    {
        // Перевірка, чи адміністратор з таким іменем існує вже в базі даних
        if (_adminRepository.GetAdminByUsername(username) != null)
        {
            ViewBag.ErrorMessage = "Адміністратор з таким іменем вже існує";
            return View();
        }

        // Створення нового адміністратора і збереження його в базі даних
        Admin newAdmin = new Admin
        {
            Username = username,
            Password = password
        };

        _adminRepository.AddAdmin(newAdmin);

        // Додайте код для авторизації нового адміністратора

        return RedirectToAction("AdminDashboard", "Admin");
    }
}
