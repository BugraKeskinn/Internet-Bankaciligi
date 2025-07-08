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
            _context.SaveChanges(); 

            return Ok(new { message = "Kayıt başarılı.", userId = user.Id, name = user.Name });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var users = _context.Users.ToList(); 
            return Ok(users);
        }

        [HttpPost("login")]
        public IActionResult Login(LoginRequest login)
        {
            var user = _context.Users.FirstOrDefault(u =>
                u.Email == login.Email && u.Password == login.Password);

            if (user == null)
                return Unauthorized(new { message = "Email veya şifre hatalı." }); 

            return Ok(new { message = $"Hoş geldin, {user.Name}!", userId = user.Id, name = user.Name });
        }
    }
}    


