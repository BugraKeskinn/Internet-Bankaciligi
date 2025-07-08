using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;

namespace UserAuthAPI.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountController : ControllerBase
    {
        private readonly AppDbContext _context;
        public AccountController(AppDbContext context)
        {
            _context = context;
        }

        // 1) Kullanıcıya yeni hesap aç
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto dto)
        {
            // Aynı kullanıcıda aynı hesap türü var mı kontrol et
            var existing = await _context.Accounts
                .FirstOrDefaultAsync(a => a.UserId == dto.UserId && a.AccountType == dto.AccountType);

            if (existing != null)
                return BadRequest("Bu döviz türünden zaten hesabınız var.");

            // Benzersiz hesap numarası üret
            string accountNumber;
            do
            {
                accountNumber = GenerateRandomAccountNumber(10);
            }
            while (await _context.Accounts.AnyAsync(a => a.AccountNumber == accountNumber));

            // Yeni hesap oluştur
            var account = new Account
            {
                UserId = dto.UserId,
                AccountType = dto.AccountType,
                Balance = dto.Balance,
                AccountNumber = accountNumber
            };

            _context.Accounts.Add(account);
            await _context.SaveChangesAsync();

            return Ok(account);
        }

        // 2) Bir kullanıcının tüm hesaplarını getir
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAccountsByUser(int userId)
        {
            var accounts = await _context.Accounts
                .Where(a => a.UserId == userId)
                .Select(a => new {
                    a.AccountId,
                    a.AccountNumber,
                    a.AccountType,
                    a.Balance
                })
                .ToListAsync();

            return Ok(accounts);
        }

        // 3) Hesap detayını getir (opsiyonel)
        [HttpGet("{accountId}")]
        public async Task<IActionResult> GetAccount(int accountId)
        {
            var account = await _context.Accounts
                .FirstOrDefaultAsync(a => a.AccountId == accountId);

            if (account == null)
                return NotFound();

            return Ok(account);
        }

        // 4) 10 haneli random hesap numarası üreten fonksiyon
        private string GenerateRandomAccountNumber(int length)
        {
            var random = new Random();
            string result = "";
            for (int i = 0; i < length; i++)
                result += random.Next(0, 10).ToString();
            return result;
        }
    }

    // DTO
    public class CreateAccountDto
    {
        public int UserId { get; set; }
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
    }
}
