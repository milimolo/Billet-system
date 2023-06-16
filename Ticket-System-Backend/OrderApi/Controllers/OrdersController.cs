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

        [HttpGet("lines")]
        public async Task<IEnumerable<OrderLine>> GetAllOrderLines(CancellationToken cancellationToken)
        {
            var orderLines = await repository.GetAllOrderLinesAsync(cancellationToken);
            return orderLines;
        }

        // GET orders/5
        [HttpGet("{id}", Name = "GetOrder")]
        public async Task<IActionResult> Get(int id, CancellationToken cancellationToken)
        {
            if (id == 0)
            {
                return NotFound();
            }
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
                newOrder.OrderLines.ForEach(ol => ol.Order = null);
                //Publish message: OrderCreatedMessage
                _messagePublisher.PublishOrderCreatedMessage(
                    newOrder.CustomerId, newOrder.Id, newOrder.OrderLines);

                // Wait for orderStatus to return "Completed"
                bool completed = false;
                int maxAttempts = 5;
                int currentAttempt = 0;
                while (!completed)
                {
                    if(currentAttempt == maxAttempts)
                    {
                        repository.RemoveAsync(order.Id, cancellationToken);
                        return StatusCode(500, "The order timed out. Please try wait 5 minutes and try again.");
                    }
                    else
                    {
                        currentAttempt++;
                        var tentativeOrder = await repository.GetForMessageAsync(newOrder.Id, cancellationToken);
                        if (tentativeOrder.OrderStatus == OrderStatus.Completed)
                        {
                            completed = true;
                        }
                        Thread.Sleep(500);
                    }
                }

                return CreatedAtRoute("GetOrder", new { id = newOrder.Id }, newOrder);
            }
            catch
            {
                repository.RemoveAsync(order.Id, cancellationToken);
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
