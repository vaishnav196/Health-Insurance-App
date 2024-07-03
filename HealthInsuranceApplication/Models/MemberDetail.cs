using System.ComponentModel.DataAnnotations;

namespace HealthInsuranceApplication.Models
{
    public class MemberDetail
    {
        [Key]
        public int Id {  get; set; }    
        public string Name { get; set; }
        public int Age { get; set; }
        public DateTime DateOfBirth { get; set; }
    }
}
