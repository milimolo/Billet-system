using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using SharedModels;
using System.Text;

namespace CacheApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CacheController : ControllerBase
    {
        private readonly IMemoryCache _cache;
        private readonly HttpClient _httpClient;

        public CacheController(IMemoryCache memoryCache, IHttpClientFactory httpClientFactory)
        {
            _cache = memoryCache;
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllTickets()
        {
            // Try to get all tickets from the cache
            if (_cache.TryGetValue("all_tickets", out List<TicketDto> tickets))
            {
                return Ok(tickets);
            }

            // If the tickets are not in the cache, fetch them from the ticket API
            var response = await _httpClient.GetAsync("http://TicketApi/tickets");
            response.EnsureSuccessStatusCode();
            tickets = await response.Content.ReadFromJsonAsync<List<TicketDto>>();

            // Add the tickets to the cache, using appropriate options
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(1) // Each ticket takes up one unit of cache space
                .SetPriority(CacheItemPriority.High) 
                .SetSlidingExpiration(TimeSpan.FromSeconds(60)); // Expire after 60 seconds of inactivity

            _cache.Set("all_tickets", tickets, cacheEntryOptions);

            return Ok(tickets);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTicket(int id)
        {
            // Try to get the ticket from the cache
            if (_cache.TryGetValue(id, out TicketDto ticket))
            {
                return Ok(ticket);
            }

            // If the ticket is not in the cache, fetch it from the ticket API
            var response = await _httpClient.GetAsync($"http://TicketApi/tickets/{id}");
            response.EnsureSuccessStatusCode();
            ticket = await response.Content.ReadFromJsonAsync<TicketDto>();

            // Add the ticket to the cache, using LRU eviction policies
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(1) // Each ticket takes up one unit of cache space
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromSeconds(60)); // Expire after 60 seconds of inactivity

            _cache.Set(id, ticket, cacheEntryOptions);

            return Ok(ticket);
        }

        [HttpPost("order")]
        public async Task<IActionResult> PostOrder([FromBody] Order order, CancellationToken cancellationToken)
        {
            // Serialize the order object to JSON
            string jsonOrderContent = JsonConvert.SerializeObject(order);
            StringContent content = new StringContent(jsonOrderContent, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync($"http://OrderApi/orders/", content);
            if (response.IsSuccessStatusCode)
            {
                foreach (var orderLine in order.OrderLines)
                {
                    if (_cache.TryGetValue(orderLine.ProductId, out TicketDto ticket))
                    {
                        ticket.TicketsReserved += orderLine.NoOfItems;
                        UpdateCachedTicket(ticket);
                    }
                    if(_cache.TryGetValue("all_tickets", out List<TicketDto> tickets))
                    {
                        tickets.Where(t => t.Id == orderLine.ProductId).ToList().ForEach(t => t.TicketsReserved += orderLine.NoOfItems);
                        UpdateAllCachedTickets(tickets);
                    }
                }

                return Ok(order);
            } 
            else
            {
                return StatusCode(500, "The order could not be created.");
            }
        }

        private async void UpdateCachedTicket(TicketDto ticket)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(1) // Each ticket takes up one unit of cache space
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromSeconds(60)); // Expire after 60 seconds of inactivity

            _cache.Set(ticket.Id, ticket, cacheEntryOptions);
        }

        private async void UpdateAllCachedTickets(List<TicketDto> tickets)
        {
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(1) // Each ticket takes up one unit of cache space
                .SetPriority(CacheItemPriority.High)
                .SetSlidingExpiration(TimeSpan.FromSeconds(60)); // Expire after 60 seconds of inactivity

            _cache.Set("all_tickets", tickets, cacheEntryOptions);
        }
    }
}
