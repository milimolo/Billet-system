using SharedModels;

namespace OrderApi.Data.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        public void Initialize(OrderApiContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            // Look for any Customers
            if (context.Orders.Any())
            {
                return;   // DB has been seeded
            }

            var mockOrderLines = new List<OrderLine>
            {
                new OrderLine { NoOfItems = 3, OrderID = 1, ProductId = 1, Price = 300 }
            };

            List<Order> orders = new List<Order>
            {
                new Order { Date = DateTime.Today, OrderStatus = OrderStatus.Completed, OrderLines = mockOrderLines, CustomerId = 1, TotalPrice = 300 }
            };

            context.Orders.AddRange(orders);
            context.SaveChanges();
        }
    }
}
