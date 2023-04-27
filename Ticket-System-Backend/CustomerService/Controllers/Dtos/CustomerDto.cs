using CustomerApi.Models;

namespace CustomerApi.Controllers.Dtos
{
    public class CustomerDto
    {
        public CustomerDto(Customer customer)
        {
            Id = customer.Id;
            Name = customer.Name;
            Email = customer.Email;
            Phone = customer.Phone;
            IsAdmin = customer.IsAdmin;
        }

        public int Id { get; set; }
        public string? Name { get; set; }
        public string Email { get; set; }
        public int Phone { get; set; }
        public bool IsAdmin { get; set; }
    }
}
