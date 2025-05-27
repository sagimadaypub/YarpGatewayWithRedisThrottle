using Microsoft.AspNetCore.Mvc;

namespace Client2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Client2Controller : Controller
    {
        private readonly HttpClient _httpClient;
        public Client2Controller(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("MyClient");
        }

        [HttpGet("Test")]
        public async Task<IActionResult> Test()
        {
            return Ok("Client2 Ok");
        }

        [HttpGet("SendRequestToClient1")]
        public async Task<IActionResult> SendRequest()
        {
            var response = await _httpClient.GetAsync("api/Client1/test");
            var content = await response.Content.ReadAsStringAsync();

            return Ok(content);
        }
    }
}
