using System.ComponentModel.DataAnnotations;

namespace Baranggay_Health_Records.Data
{
    public class ResidentHealthStatusModel
    {
        public int ID { get; set; }

        [Required]
        public int ResidentId { get; set; } = 0;

        [Required]
        public String Typeofillness { get; set; } = "";

        [Required]
        public String Weight { get; set; } = "";

        [Required]
        public String Height { get; set; } = "";

        [Required]
        public String Temperature { get; set; } = "";

        [Required]
        public String BloodPressure { get; set; } = "";

        public int? GetID()
        {
            return this.ID;
        }

        new public String? GetType()
        {
            return this.Typeofillness;
        }

        public String? GetWeight()
        {
            return this.Weight;
        }

        public String? GetHeight()
        {
            return this.Height;
        }

        public String? GetTemperature()
        {
            return this.Temperature;
        }

        public String? GetBloodPressure()
        {
            return this.BloodPressure;
        }
    }
}
