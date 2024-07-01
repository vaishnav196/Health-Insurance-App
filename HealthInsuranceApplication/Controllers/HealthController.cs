using Microsoft.AspNetCore.Mvc;

namespace HealthInsuranceApplication.Controllers
{
    public class HealthController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AgeSelection()
        {
            return View();
        }
      

   
        public IActionResult PolicySelection()
        {
            return View();
        }

        public IActionResult PremiumCalculator()
        {
            return View();
        }


    }
}
