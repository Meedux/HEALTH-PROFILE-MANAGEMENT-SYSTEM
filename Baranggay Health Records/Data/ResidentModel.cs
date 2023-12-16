using Baranggay_Health_Records.Pages.Secretary;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Baranggay_Health_Records.Data
{
    public class ResidentModel
    {
        public int ID { get; set; }

        [Required]
        public string? FirstName { get; set; } = "";

        [Required]
        public string? MiddleName { get; set; } = "";

        [Required]
        public string? LastName { get; set; } = "";

        [Required]
        public string? Suffix { get; set; } = "";

        [Required]
        public string? Dob { get; set; } = "";

        [Range(0, 200)]
        public int Age { get; set; }
        public string Gender { get; set; } = "";
        public string? Civil_status { get; set; } = "";
        public string? Religion { get; set; } = "";
        public string? Occupation { get; set; } = "";
        public int Num_of_fam { get; set; }
        public string? Ed_attain { get; set; } = "";
        public int Household_number { get; set; }
        public string Purok { get; set; } = "";

        public bool? IsPWD { get; set; } = false;

        public bool? IsSenior { get; set; } = false;

        public bool? isPrenate { get; set; } = false;


        //Resident Model Queries

        public int GetID()
        {
            return this.ID;
        }

        public int? GetResidentID()
        {
            return this.ID;
        }

        public string? GetResidentFirstName()
        {
            return this.FirstName;
        }

        public string? GetResidentMiddleName()
        {
            return this.MiddleName;
        }

        public string? GetResidentLastName()
        {
            return this.LastName;
        }

        public string? GetResidentSuffix()
        {
            return this.Suffix;
        }

        public string? GetResidentDOB()
        {
            return this.Dob;
        }

        public int GetResidentAge()
        {
            return this.Age;
        }

        public string GetResidentGender()
        {
            return this.Gender;
        }

        public string? GetResidentCivilStatus()
        {
            return this.Civil_status;
        }

        public string? GetResidentReligion()
        {
            return Religion;
        }

        public string? GetOccupation()
        {
            return this.Occupation;
        }

        public string? GetEducationalAttainment()
        {
            return this.Ed_attain;
        }

        public string GetPurok()
        {
            return this.Purok;
        }

        public int GetHouseholdNumber()
        {
            return this.Household_number;
        }
    }
}
