using System.ComponentModel.DataAnnotations;

namespace Baranggay_Health_Records.Data
{
    public class ResidentMedicine
    {
        public int? ID { get; set; }

        [Required] public String? Name { get; set; }

        [Required] public String? MedicineName { get; set; }

        [Required] public String? Description { get; set; }

        [Required] public int? Quality { get; set; }

        [Required] public DateTime? ExpirationDate { get; set; }
    }
}
