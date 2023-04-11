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

        public Customer Add(Customer entity)
        {
            Customer newCustomer = _context.Customers.Add(entity).Entity;
            _context.SaveChanges();
            return newCustomer;
        }

        public void Edit(Customer entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();
        }

        public Customer Get(int id)
        {
            try
            {
                return _context.Customers.FirstOrDefault(c => c.Id == id);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("When trying to get customer, following error message recieved: " + e.Message);
                throw;
            }
        }

        public IEnumerable<Customer> GetAll()
        {
            return _context.Customers.ToList();
        }

        public void Remove(int id)
        {
            try
            {
                Customer? customer = _context.Customers.FirstOrDefault(c => c.Id == id);
                _context.Customers.Remove(customer);
                _context.SaveChanges();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("When trying to remove customer, following error message recieved: " + e.Message);
                throw;
            }
        }
    }
}
