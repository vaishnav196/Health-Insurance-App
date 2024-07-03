using System.ComponentModel.DataAnnotations;

namespace HealthInsuranceApplication.Models
{
    public class PurchaseDetails
    {
        [Key]
        public string UserId { get; set; }
        public List<MemberDetail> Members { get; set; }
        public string Mobile { get; set; }
        public string SelectedPlan { get; set; }
        public decimal Premium { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
