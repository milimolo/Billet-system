using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels
{
    public class TicketDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Location { get; set; }
        public DateTime EventDate { get; set; }
        public decimal Price { get; set; }
        public int TicketsRemaining { get; set; }
        public int TicketsReserved { get; set; }
    }
}
