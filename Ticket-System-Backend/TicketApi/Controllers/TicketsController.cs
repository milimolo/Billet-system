using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TicketApi.Data.Repository;
using TicketApi.Models;

namespace TicketApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TicketsController : ControllerBase
    {
        private readonly IRepository<Ticket> repository;

        public TicketsController(IRepository<Ticket> repo)
        {
            repository = repo;
        }

        // GET: Tickets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Ticket>>> GetTicketsAsync(CancellationToken cancellationToken)
        {
            var tickets = await repository.GetAllAsync(cancellationToken);
            return Ok(tickets);
        }

        // GET: Tickets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Ticket>> GetTicketAsync(int id, CancellationToken cancellationToken)
        {
            var tickets = await repository.GetAsync(id, cancellationToken);

            if (tickets == null)
            {
                return NotFound();
            }

            return tickets;
        }

        // PUT: Tickets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTicketAsync(int id, Ticket tickets, CancellationToken cancellationToken)
        {
            if (id != tickets.Id)
            {
                return BadRequest();
            }

            await repository.EditAsync(tickets, cancellationToken);

            return NoContent();
        }

        // POST: Tickets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Ticket>> PostTicketAsync(Ticket tickets, CancellationToken cancellationToken)
        {
            var newTicket = await repository.AddAsync(tickets, cancellationToken);
            return newTicket;
        }

        // DELETE: Tickets/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTicketAsync(int id, CancellationToken cancellationToken)
        {
            var tickets = await repository.GetAsync(id, cancellationToken);
            if (tickets == null)
            {
                return NotFound();
            }

            await repository.RemoveAsync(id, cancellationToken);

            return NoContent();
        }
    }
}
