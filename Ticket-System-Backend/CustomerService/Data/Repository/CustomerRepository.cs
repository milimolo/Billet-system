using CustomerService.Models;
using Microsoft.EntityFrameworkCore;

namespace CustomerService.Data.Repository
{
    public class CustomerRepository : IRepository<Customer>
    {
        private readonly CustomerServiceContext _context;

        public CustomerRepository(CustomerServiceContext context)
        {
            _context = context;
        }

        public async Task<Customer> AddAsync(Customer entity, CancellationToken cancellationToken = default)
        {
            Customer newCustomer = _context.Customers.Add(entity).Entity;
            await _context.SaveChangesAsync(cancellationToken);
            return newCustomer;
        }

        public async Task EditAsync(Customer entity, CancellationToken cancellationToken = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<Customer> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                return await _context.Customers.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("When trying to get customer, following error message recieved: " + e.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Customer>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Customers.ToListAsync(cancellationToken);
        }

        public async Task RemoveAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                Customer? customer = await GetAsync(id, cancellationToken);
                _context.Customers.Remove(customer);
                await _context.SaveChangesAsync();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("When trying to remove customer, following error message recieved: " + e.Message);
                throw;
            }
        }
    }
}
