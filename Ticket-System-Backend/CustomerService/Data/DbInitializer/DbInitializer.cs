using CustomerService.Data.Helpers;
using CustomerService.Models;

namespace CustomerService.Data.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly IAuthenticationHelper authenticationHelper;

        public DbInitializer(IAuthenticationHelper authHelper)
        {
            authenticationHelper = authHelper;
        }

        public void Initialize(CustomerServiceContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Look for any Customers
            if (context.Customers.Any())
            {
                return;   // DB has been seeded
            }

            string password = "12345";
            byte[] passwordHashPaul, passwordSaltPaul, passwordHashEmil, passwordSaltEmil;
            authenticationHelper.CreatePasswordHash(password, out passwordHashPaul, out passwordSaltPaul);
            authenticationHelper.CreatePasswordHash(password, out passwordHashEmil, out passwordSaltEmil);

            List<Customer> customers = new List<Customer>
            {
                new Customer {
                    Name = "Paul",
                    Email = "Paul-walker@gmail.com",
                    Phone = 12345678,
                    PasswordHash = passwordHashPaul,
                    PasswordSalt = passwordSaltPaul,
                    IsAdmin = true
                },
                new Customer {
                    Name = "Emil",
                    Email = "Emil-Skov@gmail.dk",
                    Phone = 76145678,
                    PasswordHash = passwordHashEmil,
                    PasswordSalt = passwordSaltEmil,
                    IsAdmin = false
                }
            };

            context.Customers.AddRange(customers);
            context.SaveChanges();
        }
    }
}
