using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomerService.Models;
using CustomerService.Data.Repository;

namespace CustomerService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly IRepository<Customer> repository;

        public CustomersController(IRepository<Customer> repo)
        {
            repository = repo;
        }

        // GET: Customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers(CancellationToken cancellationToken)
        {
            var customers = await repository.GetAllAsync(cancellationToken);
            return Ok(customers);
        }

        // GET: Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Customer>> GetCustomer(int id, CancellationToken cancellationToken)
        {
            var customer = await repository.GetAsync(id, cancellationToken);

            if (customer == null)
            {
                return NotFound();
            }

            return customer;
        }

        // PUT: Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomer(int id, Customer customer, CancellationToken cancellationToken)
        {
            if (id != customer.Id)
            {
                return BadRequest();
            }

            await repository.EditAsync(customer, cancellationToken);

            return NoContent();
        }

        // POST: Customers
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Customer>> PostCustomer(Customer customer, CancellationToken cancellationToken)
        {
            var newCustomer = await repository.AddAsync(customer, cancellationToken);
            return newCustomer;
        }

        // DELETE: Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomer(int id, CancellationToken cancellationToken)
        {
            var customer = await repository.GetAsync(id, cancellationToken);
            if (customer == null)
            {
                return NotFound();
            }

            await repository.RemoveAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
