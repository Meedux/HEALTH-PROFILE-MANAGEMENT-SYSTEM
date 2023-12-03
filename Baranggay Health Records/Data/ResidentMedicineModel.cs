using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Contracts;

namespace Baranggay_Health_Records.Data
{
    public class ResidentMedicineModel
    {
        public int? ID { get; set; }


        [Required] public string MedicineName { get; set; }

        [Required] public string? Description { get; set; }

        [Required] public int? Quality { get; set; }

        [Required] public string? ExpirationDate { get; set; }

        [Required] public string? ReleaseDate { get; set; }

        [Required] public int? Stock { get; set; }

        public int? GetID()
        {
            return ID;
        }

        public int? GetStock()
        {
            return Stock;
        }

        public string? GetMedicineName()
        {
            return MedicineName;
        }

        public string? GetDescription()
        {
            return Description;
        }

        public int? GetQuality()
        {
            return Quality;
        }

        public string? GetExpirationDate()
        {
            return ExpirationDate;
        }

        public string? GetReleaseDate()
        {
            return ReleaseDate;
        }
    }
}
