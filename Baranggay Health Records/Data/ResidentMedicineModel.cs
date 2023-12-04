using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace Baranggay_Health_Records.Data
{
    public class ResidentMedicineModel
    {
        public int ID { get; set; }


        [Required] public string MedicineName { get; set; } = "";

        [Required] public string? Description { get; set; }

        [Required] public int Quality { get; set; }

        [Required] public string? ExpirationDate { get; set; }

        [Required] public string ReleaseDate { get; set; } = "";

        [Required] public int Stock { get; set; }

        public int GetID()
        {
            return this.ID;
        }

        public int GetStock()
        {
            return this.Stock;
        }

        public string? GetMedicineName()
        {
            return this.MedicineName;
        }

        public string? GetDescription()
        {
            return this.Description;
        }

        public int? GetQuality()
        {
            return this.Quality;
        }

        public string? GetExpirationDate()
        {
            return this.ExpirationDate;
        }

        public string GetReleaseDate()
        {
            return this.ReleaseDate;
        }
    }
}
