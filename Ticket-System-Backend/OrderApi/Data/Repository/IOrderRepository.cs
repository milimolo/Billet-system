using SharedModels;

namespace OrderApi.Data.Repository
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetByCustomerAsync(int customerId, CancellationToken cancellationToken);
    }
}
