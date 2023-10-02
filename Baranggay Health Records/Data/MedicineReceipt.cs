using System.ComponentModel.DataAnnotations;

namespace Baranggay_Health_Records.Data
{
    public class MedicineReceipt
    {
        public int? ID { get; set; }

        [Required] public String? NewArrival { get; set; }
        [Required] public int? NewArrivalID { get; set; }
        [Required] public int? OldStock { get; set; }
        [Required] public DateTime? ExpirationDate { get; set; }
        [Required] public int? MonthlyDistrobution { get; set; }

    }
}
