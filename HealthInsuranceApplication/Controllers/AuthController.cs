
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;

public class AuthController : Controller
{
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuthController(IHttpClientFactory httpClientFactory, IHttpContextAccessor httpContextAccessor)
    {
        _httpClientFactory = httpClientFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    [HttpPost]
    [Route("/SendOtp")]
    public async Task<IActionResult> SendOtp(string mobile, string email)
    {
        var client = _httpClientFactory.CreateClient();
        var requestBody = new { mobile, email };
        var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

        var response = await client.PostAsync("https://localhost:7027/sendotp", content);

        if (response.IsSuccessStatusCode)
        {
            // Set session variables for email and mobile
            HttpContext.Session.SetString("Email", email);
            HttpContext.Session.SetString("Mobile", mobile);

            var responseData = await response.Content.ReadAsStringAsync();
            TempData["SuccessMessage"] = JsonConvert.DeserializeObject<dynamic>(responseData)?.message;

            return Ok(new { message = "OTP sent successfully." });
        }
        else
        {
            var responseData = await response.Content.ReadAsStringAsync();
            var errorMessage = JsonConvert.DeserializeObject<dynamic>(responseData)?.message;

            return BadRequest(new { message = errorMessage });
        }
    }

    [HttpPost]
    [Route("/VerifyOtp")]
    public async Task<IActionResult> VerifyOtp(string otp)
    {
        var email = HttpContext.Session.GetString("Email");
        var mobile = HttpContext.Session.GetString("Mobile");

        if (email == null || mobile == null)
        {
            return BadRequest(new { message = "Session expired. Please try again." });
        }

        var client = _httpClientFactory.CreateClient();
        var requestBody = new { mobile, otp };
        var content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

        var response = await client.PostAsync("https://localhost:7027/verifyotp", content);

        if (response.IsSuccessStatusCode)
        {
            var responseData = await response.Content.ReadAsStringAsync();
            TempData["SuccessMessage"] = JsonConvert.DeserializeObject<dynamic>(responseData)?.message;

            return RedirectToAction("Index", "Home");
        }
        else
        {
            var responseData = await response.Content.ReadAsStringAsync();
            var errorMessage = JsonConvert.DeserializeObject<dynamic>(responseData)?.message;

            return BadRequest(new { message = errorMessage });
        }
    }
    public IActionResult Index()
    {
        return View();
    }


    public IActionResult Login()
    {
        return View();
    }

 
    public async Task GoogleLogin()
    {
        await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new AuthenticationProperties
        {   
            RedirectUri = Url.Action("GoogleResponse")
        });
    }

    public async Task<IActionResult> GoogleResponse()
    {
        var result = await HttpContext.AuthenticateAsync(GoogleDefaults.AuthenticationScheme);

        if (result?.Principal != null)
        {
            var claims = result.Principal.Identities.FirstOrDefault().Claims;
            var email = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (!string.IsNullOrEmpty(email))
            {
                HttpContext.Session.SetString("Email", email);
            }

            // Assuming you have a method to get the mobile number, if required.
            // var mobile = GetMobileNumberForUser(email);
            // if (!string.IsNullOrEmpty(mobile))
            // {
            //     HttpContext.Session.SetString("Mobile", mobile);
            // }

            // Redirect to HomeController/Index
            return RedirectToAction("Index", "Home");
        }

        return RedirectToAction("Login", "Auth");
    }


    public IActionResult SignOut()
    {
        HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        var storedcookies = Request.Cookies.Keys;
        foreach (var cookie in storedcookies)
        {
            Response.Cookies.Delete(cookie);
        }
        return RedirectToAction("Login","Auth");
    }
}


