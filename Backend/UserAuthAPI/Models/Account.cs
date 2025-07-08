namespace UserAuthAPI.Models
{
    public class Account
    {
        public int AccountId { get; set; }    
        public string AccountNumber { get; set; } = string.Empty;
        public string AccountType { get; set; } = string.Empty;
        public decimal Balance { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}
