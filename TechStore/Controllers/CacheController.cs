using TechStore.Data;
using TechStore.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TechStore.Models;

namespace TechStore.Controllers
{
    public class CacheController : ControllerBase
    {
        private readonly RedisCacheService _redisCache;

        public CacheController(RedisCacheService redisCache)
        {
            _redisCache = redisCache;
        }

        //[HttpGet("test-redis")]
        //public async Task<IActionResult> TestRedis()
        //{
        //    var testCacheKey = "test";
        //    await _redisCache.SetValueAsync(testCacheKey, "This is a test");

        //    var testValue = await _redisCache.GetValueAsync(testCacheKey);

        //    if (string.IsNullOrEmpty(testValue))
        //    {
        //        return BadRequest("No value found in Redis.");
        //    }

        //    return Ok(new { message = "Value from Redis: " + testValue });
        //}

        [HttpGet("check-redis")]
        public async Task<IActionResult> CheckRedisCache()
        {
            var brandsCacheKey = "brands";  
            var brands = await _redisCache.GetValueAsync(brandsCacheKey);

            if (string.IsNullOrEmpty(brands))
            {
                // If no data found, return an empty message with status 200 OK
                return Ok("No brand data found in cache.");
            }
            else
            {
                // Deserialize the JSON data and get the list of brands
                var brandList = JsonConvert.DeserializeObject<List<Brand>>(brands);

                // Return the list of brands as part of the response body
                return Ok(new { brands = brandList });
            }
        }

        //[HttpPost("set")]
        //public async Task<IActionResult> SetCache(string key, string value)
        //{
        //    await _redisCache.SetValueAsync(key, value);
        //    return Ok("Value set in Redis");
        //}

        //[HttpGet("get")]
        //public async Task<IActionResult> GetCache(string key)
        //{
        //    var value = await _redisCache.GetValueAsync(key);
        //    if (value == null)
        //    {
        //        return NotFound("Key not found in Redis");
        //    }
        //    return Ok(value);
        //}
    }
}
