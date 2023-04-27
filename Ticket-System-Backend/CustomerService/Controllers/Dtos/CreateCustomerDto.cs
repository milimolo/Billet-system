namespace CustomerApi.Controllers.Dtos
{
    public class CreateCustomerDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int Phone { get; set; }
        public bool IsAdmin { get; set; }
    }
}
