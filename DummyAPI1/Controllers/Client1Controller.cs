using Microsoft.AspNetCore.Mvc;

namespace Client1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Client1Controller : Controller
    {
        private readonly HttpClient _httpClient;
        public Client1Controller(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyClient");
        }

        [HttpGet("Test")]
        public async Task<IActionResult> Test()
        {
            return Ok("Client1 Ok");
        }

        [HttpGet("SendRequestToClient2")]
        public async Task<IActionResult> SendRequest()
        {
            var response = await _httpClient.GetAsync("api/Client2/test");
            var content = await response.Content.ReadAsStringAsync();

            return Ok(content);
        }
    }
}
