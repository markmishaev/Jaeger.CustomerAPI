using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Jaeger.CustomerAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly ILogger<CustomerController> _logger;
        private readonly IHttpClientFactory _factory;

        public CustomerController(ILogger<CustomerController> logger, IHttpClientFactory factory)
        {
            _logger = logger;
            _factory = factory;
        }

        // GET api/values  
        [HttpGet]
        public async Task<IEnumerable<Order>> GetAsync()
        {
            var orders = await GetOrders();

            return orders;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetAsync(int id)
        {
            var order = await GetOrderById(id);
            return order;            
        }

        private async Task<Order> GetOrderById(int id)
        {
            var client = _factory.CreateClient("orderApi");

            var requestMsg = new HttpRequestMessage(HttpMethod.Get, $"http://localhost:62686/api/orders/{id}");

            var responseMsg = await client.SendAsync(requestMsg);

            var orderJson = await responseMsg.Content.ReadAsStringAsync();

            Order order = JsonConvert.DeserializeObject<Order>(orderJson);

            return order;
        }


        private async Task<IEnumerable<Order>> GetOrders()
        {
            var client = _factory.CreateClient("orderApi");

            var requestMsg = new HttpRequestMessage(HttpMethod.Get, "http://localhost:62686/api/orders");

            var responseMsg = await client.SendAsync(requestMsg);

            var ordersJson = await responseMsg.Content.ReadAsStringAsync();

            List<Order> orders = JsonConvert.DeserializeObject<List<Order>>(ordersJson);

            return orders;
        }
    }
}
