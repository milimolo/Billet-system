using SharedModels;

namespace OrderApi.Data.Repository
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetByCustomerAsync(int customerId, CancellationToken cancellationToken);
        Task<IEnumerable<OrderLine>> GetAllOrderLinesAsync(CancellationToken cancellationToken);
        Task<Order> GetForMessageAsync(int id, CancellationToken cancellationToken = default);
    }
}
