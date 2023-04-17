using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderApi.Data.Repository;
using OrderApi.Infrastructure;
using SharedModels;
using SharedModels.Messages.OrderMessages;

namespace OrderApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository repository;
        private readonly IMessagePublisher _messagePublisher;

        public OrdersController(IOrderRepository repos, IMessagePublisher messagePublisher)
        {
            repository = repos;
            _messagePublisher = messagePublisher;
        }

        // GET: orders
        [HttpGet]
        public async Task<IEnumerable<Order>> GetAll(CancellationToken cancellationToken)
        {
            var orders = await repository.GetAllAsync(cancellationToken);
            return orders;
        }

        // GET orders/5
        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            var item = await repository.GetAsync(id, cancellationToken);
            if (item == null)
            {
                return BadRequest("Could not find order.");
            }
            return new ObjectResult(item);
        }

        // POST orders
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Order order, CancellationToken cancellationToken)
        {
            if (order == null)
            {
                return BadRequest("Order is null, please try again.");
            }

            try
            {
                // Create the tentative order
                order.OrderStatus = OrderStatus.Tentative;
                var newOrder =  await repository.AddAsync(order, cancellationToken);

                //Publish message: OrderCreatedMessage
                _messagePublisher.PublishOrderCreatedMessage(
                    newOrder.CustomerId, newOrder.Id, newOrder.OrderLines);

                // Wait for orderStatus to return "Completed"
                bool completed = false;
                while (!completed)
                {
                    var tentativeOrder = await repository.GetAsync(newOrder.Id, cancellationToken);
                    if (tentativeOrder.OrderStatus == OrderStatus.Completed)
                    {
                        completed = true;
                    }
                    Thread.Sleep(500);
                }

                return CreatedAtRoute("GetOrder", new { id = newOrder.Id }, newOrder);
            }
            catch
            {
                return StatusCode(500, "The order could not be created. Please try again.");
            }
        }

        // PUT orders/5/cancel
        // This action method cancels an order and publishes an OrderStatusChangedMessage
        // with topic set to "cancelled".
        [HttpPut("{id}/cancel")]
        public IActionResult Cancel(int id)
        {
            throw new NotImplementedException();

            // Add code to implement this method.
        }
    }
}
