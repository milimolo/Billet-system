using EasyNetQ;
using OrderApi.Data.Repository;
using SharedModels;
using SharedModels.Messages.OrderMessages;

namespace OrderApi.Infrastructure
{
    public class MessageListener
    {
        IServiceProvider _provider;
        string _connectionString;
        IBus bus;

        public MessageListener(IServiceProvider provider, string connectionString)
        {
            _provider = provider;
            _connectionString = connectionString;
        }

        public void Start()
        {
            using (bus = RabbitHutch.CreateBus(_connectionString))
            {
                bus.PubSub.Subscribe<OrderAcceptedMessage>("orderApiAccepted", HandleOrderAccepted);
                bus.PubSub.Subscribe<OrderRejectedMessage>("orderApiRejected", HandleOrderRejected);


                lock (this)
                {
                    Monitor.Wait(this);
                }
            }
        }

        private async Task HandleOrderAccepted(OrderAcceptedMessage message)
        {
            using (var scope = _provider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var orderRepos = services.GetService<IOrderRepository>();

                // Mark as completed
                var order = await orderRepos.GetAsync(message.OrderId);
                order.OrderStatus = OrderStatus.Completed;
                await orderRepos.EditAsync(order);
            }
        }

        private async Task HandleOrderRejected(OrderRejectedMessage message)
        {
            using (var scope = _provider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var orderRepos = services.GetService<IOrderRepository>();

                // Delete tentative order.
                await orderRepos.RemoveAsync(message.OrderId);
            }
        }
    }
}
