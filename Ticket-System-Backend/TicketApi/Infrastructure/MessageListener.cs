using EasyNetQ;
using SharedModels;
using SharedModels.Messages.OrderMessages;
using TicketApi.Data.Repository;
using TicketApi.Models;

namespace TicketApi.Infrastructure
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
                bus.PubSub.Subscribe<OrderCreatedMessage>("customerApiCreated", HandleOrderCreated);


                lock (this)
                {
                    Monitor.Wait(this);
                }
            }
        }

        private async Task HandleOrderCreated(OrderCreatedMessage message)
        {
            using (var scope = _provider.CreateScope())
            {
                var services = scope.ServiceProvider;
                var ticketRepo = services.GetService<IRepository<Ticket>>();

                CancellationToken cancellationToken = default;
                if (await TicketsAvailable(message.OrderLines, ticketRepo, cancellationToken))
                {
                    // Reserve items and publish an OrderAcceptedMessage
                    foreach (var ol in message.OrderLines)
                    {
                        var ticket = await ticketRepo.GetAsync(ol.ProductId, cancellationToken);
                        ticket.TicketsReserved += ol.NoOfItems;
                        ticket.TicketsRemaining -= ol.NoOfItems;
                        await ticketRepo.EditAsync(ticket, cancellationToken);
                    }

                    var replyMessage = new OrderAcceptedMessage
                    {
                        OrderId = message.OrderId
                    };

                    bus.PubSub.Publish(replyMessage);
                }
                else
                {
                    // Publish rejected Order message
                    var replyMessage = new OrderRejectedMessage
                    {
                        OrderId = message.OrderId
                    };

                    bus.PubSub.Publish(replyMessage);
                }
            }
        }

        private async Task<bool> TicketsAvailable(IEnumerable<OrderLine> orderLines, IRepository<Ticket> ticketRepo, CancellationToken cancellationToken)
        {
            foreach (var ol in orderLines)
            {
                var ticket = await ticketRepo.GetAsync(ol.ProductId, cancellationToken);
                if (ol.NoOfItems > ticket.TicketsRemaining - ticket.TicketsReserved)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
