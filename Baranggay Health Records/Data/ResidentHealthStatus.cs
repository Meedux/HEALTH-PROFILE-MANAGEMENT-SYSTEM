using System.ComponentModel.DataAnnotations;

namespace Baranggay_Health_Records.Data
{
    public class ResidentHealthStatus
    {
        public int ID { get; set; }

        [Required] public int? ResidentId { get; set; }

        [Required] public String? Typeofillness { get; set; }

        [Required] public int? Weight { get; set; }

        [Required] public int? Height { get; set; }

        [Required] public int? Temperature { get; set; }

        [Required] public String? BloodPressure { get; set; }

        public int? GetID()
        {
            return this.ID;
        }

        new public String? GetType()
        {
            return this.Typeofillness;
        }

        public int? GetWeight()
        {
            return this.Weight;
        }

        public int? GetHeight()
        {
            return this.Height;
        }

        public int? GetTemperature()
        {
            return this.Temperature;
        }

        public String? GetBloodPressure()
        {
            return this.BloodPressure;
        }
    }
}
