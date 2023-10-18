using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace Baranggay_Health_Records.Data
{
    public class ResidentMedicineModel
    {
        public int? ID { get; set; }


        [Required] public String? MedicineName { get; set; }

        [Required] public String? Description { get; set; }

        [Required] public int? Quality { get; set; }

        [Required] public DateTime? ExpirationDate { get; set; }

        [Required] public DateTime? ReleaseDate { get; set; }

        [Required] public int? Stock { get; set; }

        public int? GetID()
        {
            return this.ID;
        }

        public int? GetStock()
        {
            return this.Stock;
        }

        public String? GetMedicineName()
        {
            return this.MedicineName;
        }

        public String? GetDescription()
        {
            return this.Description;
        }

        public int? GetQuality()
        {
            return this.Quality;
        }

        public String? GetExpirationDate()
        {
            return this.ExpirationDate.ToString();
        }

        public String? GetReleaseDate()
        {
            return this.ReleaseDate.ToString();
        }
    }
}
