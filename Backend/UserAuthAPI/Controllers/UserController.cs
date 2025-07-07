using Microsoft.AspNetCore.Mvc;
using UserAuthAPI.Models;

namespace UserAuthAPI.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private static List<User> users = new();

        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            user.Id = users.Count + 1;
            users.Add(user);
            return Ok("Kayıt başarılı.");
        }

        [HttpPost("login")]
        public IActionResult Login(User login)
        {
            var user = users.FirstOrDefault(u => 
                u.Email == login.Email && u.Password == login.Password);

            if (user == null)
                return Unauthorized("Email veya şifre hatalı.");

            return Ok($"Hoş geldin, {user.Name}!");
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            return Ok(users);
        }
    }
}
