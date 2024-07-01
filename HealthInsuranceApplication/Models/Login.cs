using System.ComponentModel.DataAnnotations;

namespace HealthInsuranceApplication.Models
{
    public class Login
    {
        [Required(ErrorMessage ="Enter Mobile Number")]
       public string Mobile { get; set; }
        [Required(ErrorMessage = "Enter Email Address")]
        public string Email { get; set; }   
    }
}