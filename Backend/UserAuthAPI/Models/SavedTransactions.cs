namespace UserAuthAPI.Models;
using System.ComponentModel.DataAnnotations;

public class SavedTransaction
{
    [Key]
    public int TransactionId { get; set; }
    public string TransactionName { get; set; }
    public int UserId { get; set; }
    public string RecieverAccountNumber { get; set; }
    public string SenderAccountNumber { get; set; } // ← Burası eksik olabilir
    public string AccountType { get; set; }
    public decimal Amount { get; set; }

    public User User { get; set; }
}
