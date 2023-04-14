using Microsoft.EntityFrameworkCore;
using SharedModels;

namespace OrderApi.Data.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderApiContext _context;

        public OrderRepository(OrderApiContext context)
        {
            _context = context;
        }

        public async Task<Order> AddAsync(Order entity, CancellationToken cancellationToken = default)
        {
            if (entity.Date == null)
                entity.Date = DateTime.Now;

            Order newOrder = _context.Orders.Add(entity).Entity;
            await _context.SaveChangesAsync(cancellationToken);

            foreach (var ol in newOrder.OrderLines)
            {
                ol.Order = null;
            };

            return newOrder;
        }

        public async Task EditAsync(Order entity, CancellationToken cancellationToken = default)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Orders.ToListAsync(cancellationToken);
        }

        public async Task<Order> GetAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderLines)
                    .FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

                foreach (var ol in order.OrderLines)
                {
                    ol.Order = null;
                };

                return order;
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("When trying to get customer, following error message recieved: " + e.Message);
                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetByCustomerAsync(int customerId, CancellationToken cancellationToken = default)
        {
            var ordersForCustomer = await _context.Orders
                .Where(o => o.CustomerId == customerId)
                .ToListAsync(cancellationToken);

            return ordersForCustomer;
        }

        public async Task RemoveAsync(int id, CancellationToken cancellationToken = default)
        {
            try
            {
                Order? order = await GetAsync(id, cancellationToken);
                _context.Orders.Remove(order);
                await _context.SaveChangesAsync();
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("When trying to remove order, following error message recieved: " + e.Message);
                throw;
            }
        }
    }
}
