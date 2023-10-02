using System.ComponentModel.DataAnnotations;

namespace Baranggay_Health_Records.Data
{
    public class ResidentHealthStatus
    {
        public int Id { get; set; }

        [Required] public int? ResidentId { get; set; }

        [Required] public String? Typeofillness { get; set; }

        [Required] public int? Weight { get; set; }

        [Required] public int? Height { get; set; }

        [Required] public int? Temperature { get; set; }

        [Required] public String? BloodPressure { get; set; }


    }
}
