﻿using System.ComponentModel.DataAnnotations;

namespace Baranggay_Health_Records.Data
{
    public class ResidentHealthStatusModel
    {
        public int ID { get; set; }

        [Required]
        public int ResidentId { get; set; } = 0;

        [Required]
        public int TypeofillnessID { get; set; } = 0;

        [Required]
        public string Weight { get; set; } = "";

        [Required]
        public string Height { get; set; } = "";

        [Required]
        public string Temperature { get; set; } = "";

        [Required]
        public string BloodPressure { get; set; } = "";

        [Required]
        public DateTime DiagnosedDate { get; set; }

        public int GetID()
        {
            return this.ID;
        }

        public int GetIllnessType()
        {
            return this.TypeofillnessID;
        }

        public string GetWeight()
        {
            return this.Weight;
        }

        public string GetHeight()
        {
            return this.Height;
        }

        public string GetTemperature()
        {
            return this.Temperature;
        }

        public string GetBloodPressure()
        {
            return this.BloodPressure;
        }

        public string GetDiagnosedDate()
        {
            return this.DiagnosedDate.ToShortDateString();
        }
    }
}
