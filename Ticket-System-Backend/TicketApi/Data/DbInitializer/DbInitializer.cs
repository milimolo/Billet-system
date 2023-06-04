using TicketApi.Models;

namespace TicketApi.Data.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        public void Initialize(TicketApiContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Look for any Tickets
            if (context.Tickets.Any())
            {
                return;   // DB has been seeded
            }

            List<Ticket> tickets = new List<Ticket>
            {
                new Ticket { Name = "Lasse Remer Stand-up show", Price = 100, TicketsRemaining = 500, TicketsReserved = 0, EventDate = DateTime.Now.AddDays(20), Location = "Esbjerg" },
                new Ticket { Name = "AC/DC Sidste koncert", Price = 90, TicketsRemaining = 5000, TicketsReserved = 1324, EventDate = DateTime.Now.AddDays(80), Location = "Odense" },
                new Ticket { Name = "Lukas Graham", Price = 120, TicketsRemaining = 2000, TicketsReserved = 0, EventDate = DateTime.Now.AddDays(150), Location = "Aarhus" },
                new Ticket { Name = "Ukendt Kunster", Price = 110, TicketsRemaining = 1500, TicketsReserved = 0, EventDate = DateTime.Now.AddDays(150), Location = "København" },
                new Ticket { Name = "Grøn koncert", Price = 120, TicketsRemaining = 1000, TicketsReserved = 0, EventDate = DateTime.Now.AddDays(150), Location = "København" }
            };

            context.Tickets.AddRange(tickets);
            context.SaveChanges();
        }
    }
}
