using Microsoft.EntityFrameworkCore;
using TicketApi.Models;

namespace TicketApi.Data.Repository
{
    public class TicketRepository : IRepository<Ticket>
    {
        private readonly TicketApiContext _context;

        public TicketRepository(TicketApiContext ticketRepository)
        {
            _context = ticketRepository;
        }

        public async Task<Ticket> AddAsync(Ticket entity, CancellationToken cancellationToken = default)
        {
            Ticket newTickets = _context.Tickets.Add(entity).Entity;
            await _context.SaveChangesAsync(cancellationToken);
            return newTickets;
        }

        public async Task EditAsync(Ticket entity, CancellationToken cancellationToken = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Ticket>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Tickets.ToListAsync(cancellationToken);
        }

        public async Task<Ticket> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Tickets.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);
        }

        public async Task<IEnumerable<Ticket>> GetFirstFiveTicketsAsync(CancellationToken cancellationToken)
        {
            return await _context.Tickets.Take(5).ToListAsync(cancellationToken);
        }

        public async Task RemoveAsync(int id, CancellationToken cancellationToken = default)
        {
            Ticket? ticket = await GetAsync(id, cancellationToken);
            _context.Tickets.Remove(ticket);
            await _context.SaveChangesAsync();
        }
    }
}
