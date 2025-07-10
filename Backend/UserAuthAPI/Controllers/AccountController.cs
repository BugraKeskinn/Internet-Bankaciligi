using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Linq;
using System;

using UserAuthAPI.Data;
using UserAuthAPI.Models;

namespace UserAuthAPI.Controllers
{
    [ApiController]
    [Route("api/accounts")]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public AccountController(ApplicationDbContext context)
        {
            _context = context;
        }

        // 1) Kullanıcıya yeni hesap aç
        [HttpPost]
        public async Task<IActionResult> CreateAccount([FromBody] CreateAccountDto dto)
        {
            var existing = await _context.Accounts
                .FirstOrDefaultAsync(a => a.UserId == dto.UserId && a.AccountType == dto.AccountType);

            if (existing != null)
                return BadRequest("Bu döviz türünden zaten hesabınız var.");

            string accountNumber;
            do
            {
                accountNumber = GenerateRandomAccountNumber(10);
            }
            while (await _context.Accounts.AnyAsync(a => a.AccountNumber == accountNumber));

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
                .Select(a => new
                {
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

        // 4) Para transferi yap
        [HttpPost("transfer")]
        public async Task<IActionResult> Transfer([FromBody] TransferRequest req)
        {
            var sender = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == req.SenderAccountNumber);
            var receiver = await _context.Accounts.FirstOrDefaultAsync(a => a.AccountNumber == req.ReceiverAccountNumber);

            if(sender == receiver) {
                return BadRequest(new { success = false, message = "Gönderen ve alıcı hesap aynı olamaz." });
            }
            if (sender == null || receiver == null)
                return BadRequest(new { success = false, message = "Hesap bulunamadı." });
            if (sender.Balance < req.Amount)
                return BadRequest(new { success = false, message = "Yetersiz bakiye." });

            // *** Hesap tiplerini karşılaştır! ***
            if (!string.Equals(sender.AccountType, receiver.AccountType, StringComparison.OrdinalIgnoreCase))
                return BadRequest(new { success = false, message = "Hesap türleri farklı, transfer yapılamaz." });

            sender.Balance -= req.Amount;
            receiver.Balance += req.Amount;
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Transfer başarılı." });
        }



        private string GenerateRandomAccountNumber(int length)
        {
            var random = new Random();
            string result = "";
            for (int i = 0; i < length; i++)
                result += random.Next(0, 10).ToString();
            return result;
        }
    }

    // DTO'lar (Controller'ın DIŞINDA ayrı olarak)
    public class CreateAccountDto
    {
        public int UserId { get; set; }
        public string AccountType { get; set; }
        public decimal Balance { get; set; }
    }

    public class TransferRequest
    {
        public string SenderAccountNumber { get; set; }
        public string ReceiverAccountNumber { get; set; }
        public decimal Amount { get; set; }
    }
}
