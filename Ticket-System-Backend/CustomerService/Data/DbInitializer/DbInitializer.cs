using CustomerService.Models;

namespace CustomerService.Data.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        public void Initialize(CustomerServiceContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Look for any Customers
            if (context.Customers.Any())
            {
                return;   // DB has been seeded
            }

            List<Customer> customers = new List<Customer>
            {
                new Customer { Name = "Paul", Email = "Paul-walker@gmail.com", Phone = 12345678},
                new Customer { Name = "Emil", Email = "Emil-Skov@gmail.dk", Phone = 76145678}
            };

            context.Customers.AddRange(customers);
            context.SaveChanges();
        }
    }
}
