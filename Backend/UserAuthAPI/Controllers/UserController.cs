using Microsoft.AspNetCore.Mvc;
using UserAuthAPI.Models;
using UserAuthAPI.Data; // DbContext erişimi için

namespace UserAuthAPI.Controllers
{
    [ApiController]
    [Route("user")]
    public class UserController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public UserController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges(); // Veritabanına yazar
            return Ok("Kayıt başarılı.");
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _context.Users.ToList(); // Veritabanından çek
            return Ok(users);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest login)
        {
            var user = _context.Users.FirstOrDefault(u =>
                u.Email == login.Email && u.Password == login.Password);

            if (user == null)
                return Unauthorized("Email veya şifre hatalı.");

            return Ok($"Hoş geldin, {user.Name}!");
        }

    }
}
