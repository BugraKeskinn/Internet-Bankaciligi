using UserAuthAPI.Models;

public class Account
{
    public string AccountNumber { get; set; }   
    public string AccountType { get; set; }     
    public decimal Balance { get; set; }

    public int UserId { get; set; }
    public User User { get; set; }             
}
