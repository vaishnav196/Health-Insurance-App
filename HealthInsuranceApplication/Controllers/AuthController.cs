using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace YourNamespace.Controllers
{
    public class AuthController : Controller
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7125/Api/Login/sendotp", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ResponseMessage>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return Json(result);
            }
            else
            {
                var errorData = await response.Content.ReadAsStringAsync();
                var errorResult = JsonSerializer.Deserialize<ResponseMessage>(errorData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return BadRequest(errorResult);
            }
        }

        [HttpPost]
        public async Task<IActionResult> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonContent = new StringContent(JsonSerializer.Serialize(request), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://localhost:7125/Api/Login/verifyotp", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                var responseData = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<ResponseMessage>(responseData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return Json(result);
            }
            else
            {
                var errorData = await response.Content.ReadAsStringAsync();
                var errorResult = JsonSerializer.Deserialize<ResponseMessage>(errorData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return BadRequest(errorResult);
            }
        }
    }

    public class SendOtpRequest
    {
        public string Mobile { get; set; }
        public string Email { get; set; }
    }

    public class VerifyOtpRequest
    {
        public string Otp { get; set; }
    }

    public class ResponseMessage
    {
        public string Message { get; set; }
    }
}
