using EasyNetQ;

namespace CustomerApi.Infrastructure
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
                //bus.PubSub.Subscribe<OrderCreatedMessage>("customerApiCreated", HandleOrderCreated);
                //bus.PubSub.Subscribe<OrderPayMessage>("customerApiPay", HandleOrderPaid);

                // Block the thread so that it will not exit and stop subscribing.
                lock (this)
                {
                    Monitor.Wait(this);
                }
            }
        }
    }
}
