using Microsoft.AspNetCore.Mvc;

namespace CacheComponent.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CacheController : ControllerBase
    {
        private readonly Cache<string, string> _cache;

        private readonly ILogger<CacheController> _logger;

        public CacheController(Cache<string, string> cache, ILogger<CacheController> logger)
        {
            _logger = logger;
            _cache = cache;
            _cache.RemoveItem += (key, value) => Console.WriteLine($"Item Removed from cache with key: {key} and value: {value}");
        }

        
        [HttpGet(Name = "GetKey")]
        public IActionResult Get(string key)
        {
            var value = _cache.GetValue(key);
            if(value == null)
            {
                return NotFound();
            }
            return Ok(value);
        }

        [HttpPost]
        public IActionResult Set([FromQuery] string key, [FromQuery] string value)
        {
            _cache.SetValue(key, value);
            return Ok();
        }


    }
}
