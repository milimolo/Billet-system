using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CustomerApi.Models;
using CustomerApi.Data.Repository;
using CustomerApi.Controllers.Dtos;

namespace CustomerApi.Controllers
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
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomersAsync(CancellationToken cancellationToken)
        {
            var customers = await repository.GetAllAsync(cancellationToken);
            if (!customers.Any())
            {
                return NotFound();
            }
            var customerDtos = customers.Select(x => new CustomerDto(x));
            return Ok(customerDtos);
        }

        // GET: Customers/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerDto>> GetCustomerAsync(int id, CancellationToken cancellationToken)
        {
            var customer = await repository.GetAsync(id, cancellationToken);

            if (customer == null)
            {
                return NotFound();
            }
            var customerDto = new CustomerDto(customer);

            return customerDto;
        }

        // PUT: Customers/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCustomerAsync(int id, Customer customer, CancellationToken cancellationToken)
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
        public async Task<ActionResult<Customer>> PostCustomerAsync(Customer customer, CancellationToken cancellationToken)
        {
            var newCustomer = await repository.AddAsync(customer, cancellationToken);
            return newCustomer;
        }

        // DELETE: Customers/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCustomerAsync(int id, CancellationToken cancellationToken)
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
