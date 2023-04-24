using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedModels
{
    public class Order
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public int CustomerId { get; set; }
        public List<OrderLine>? OrderLines { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public double TotalPrice { get; set; }
    }
}
