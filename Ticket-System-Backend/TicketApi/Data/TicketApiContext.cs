using Microsoft.EntityFrameworkCore;
using TicketApi.Models;

namespace TicketApi.Data
{
    public class TicketApiContext : DbContext
    {
        public TicketApiContext(DbContextOptions<TicketApiContext> options) : base(options)
        {
        }

        public DbSet<Ticket> Tickets { get; set; }
    }
}
