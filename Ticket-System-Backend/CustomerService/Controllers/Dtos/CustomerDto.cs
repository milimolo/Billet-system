namespace CustomerService.Controllers.Dtos
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public bool IsAdmin { get; set; }
    }
}
