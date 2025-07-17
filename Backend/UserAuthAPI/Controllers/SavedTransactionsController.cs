using Microsoft.AspNetCore.Mvc;
using UserAuthAPI.Models;
using UserAuthAPI.Data;

namespace UserAuthAPI.Controllers
{
    [ApiController]
    [Route("user/SavedTransactions")]
    public class SavedTransactionController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SavedTransactionController(ApplicationDbContext context)
        {
            _context = context;
        }
        }
    }

