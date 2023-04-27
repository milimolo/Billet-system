namespace CustomerService.Models
{
    public class Customer
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public byte[] PasswordHash { get; set; }
        public byte[] PasswordSalt { get; set; }
        public bool IsAdmin { get; set; }
    }
}
