using EasyNetQ;
using SharedModels;
using SharedModels.Messages.OrderMessages;

namespace OrderApi.Infrastructure
{
    public class MessagePublisher : IMessagePublisher
    {
        readonly IBus bus;

        public MessagePublisher(string connectionString)
        {
            bus = RabbitHutch.CreateBus(connectionString);
        }

        public void Dispose()
        {
            bus.Dispose();
        }
        public void PublishOrderCreatedMessage(int customerId, int orderId, IList<OrderLine> orderLines)
        {
            var message = new OrderCreatedMessage
            {
                CustomerId = customerId,
                OrderId = orderId,
                OrderLines = orderLines
            };

            bus.PubSub.Publish(message);
        }
    }
}
