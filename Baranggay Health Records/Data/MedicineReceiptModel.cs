using System.ComponentModel.DataAnnotations;

namespace Baranggay_Health_Records.Data
{
    public class MedicineReceiptModel
    {
        public int? ID { get; set; }

        [Required] public string? NewArrival { get; set; }
        [Required] public int? NewArrivalID { get; set; }
        [Required] public int? OldStock { get; set; }
        [Required] public DateTime? ExpirationDate { get; set; }
        [Required] public int? MonthlyDistrobution { get; set; }

        public int? GetID()
        {
            return ID;
        }

        public string? GetNewArrival()
        {
            return NewArrival;
        }

        public int? GetNewArrivalID()
        {
            return NewArrivalID;
        }

        public int? GetOldStock()
        {
            return OldStock;
        }

        public DateTime? GetExpirationDate()
        {
            return ExpirationDate;
        }

        public int? MonthlyDistribution()
        {
            return MonthlyDistrobution;
        }
    }
}
