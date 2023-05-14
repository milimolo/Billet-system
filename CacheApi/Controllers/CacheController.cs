using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using SharedModels;

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
        public async Task<IActionResult> GetTicket(int id)
        {
            // Try to get the ticket from the cache
            if (_cache.TryGetValue(id, out TicketDto ticket))
            {
                return Ok(ticket);
            }

            // If the ticket is not in the cache, fetch it from the ticket API
            var response = await _httpClient.GetAsync($"https://localhost:56666/tickets/{id}");
            response.EnsureSuccessStatusCode();
            ticket = await response.Content.ReadFromJsonAsync<TicketDto>();

            // Add the ticket to the cache, using write-back and LRU eviction policies
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSize(1) // Each ticket takes up one unit of cache space
                .SetPriority(CacheItemPriority.NeverRemove) // Prevent eviction due to memory pressure
                .SetAbsoluteExpiration(TimeSpan.FromMinutes(10)); // Expire after 60 seconds of inactivity

            _cache.Set(id, ticket, cacheEntryOptions);
            if (_cache.TryGetValue(id, out TicketDto ticket2))
            {
                Console.WriteLine(ticket2.Name);
            }

            return Ok(ticket);
        }
    }
}
