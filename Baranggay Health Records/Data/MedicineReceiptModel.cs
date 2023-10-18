using System.ComponentModel.DataAnnotations;

namespace Baranggay_Health_Records.Data
{
    public class MedicineReceiptModel
    {
        public int? ID { get; set; }

        [Required] public String? NewArrival { get; set; }
        [Required] public int? NewArrivalID { get; set; }
        [Required] public int? OldStock { get; set; }
        [Required] public DateTime? ExpirationDate { get; set; }
        [Required] public int? MonthlyDistrobution { get; set; }

        public int? GetID()
        {
            return this.ID;
        }

        public String? GetNewArrival()
        {
            return this.NewArrival;
        }

        public int? GetNewArrivalID()
        {
            return this.NewArrivalID;
        }

        public int? GetOldStock()
        {
            return this.OldStock;
        }

        public DateTime? GetExpirationDate()
        {
            return this.ExpirationDate;
        }

        public int? MonthlyDistribution()
        {
            return this.MonthlyDistrobution;
        }
    }
}
