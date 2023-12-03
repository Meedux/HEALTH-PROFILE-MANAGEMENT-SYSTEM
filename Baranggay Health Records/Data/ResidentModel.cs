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
        public string? Gender { get; set; } = "";
        public string? Civil_status { get; set; } = "";
        public string? Religion { get; set; } = "";
        public string? Occupation { get; set; } = "";
        public int Num_of_fam { get; set; }
        public string? Ed_attain { get; set; } = "";
        public int Household_number { get; set; }
        public string? Purok { get; set; } = "";

        public bool? IsPWD { get; set; } = false;

        public bool? IsSenior { get; set; } = false;

        public bool? isPrenate { get; set; } = false;


        //Resident Model Queries

        public int GetID()
        {
            return ID;
        }

        public int? GetResidentID()
        {
            return ID;
        }

        public string? GetResidentFirstName()
        {
            return FirstName;
        }

        public string? GetResidentMiddleName()
        {
            return MiddleName;
        }

        public string? GetResidentLastName()
        {
            return LastName;
        }

        public string? GetResidentSuffix()
        {
            return Suffix;
        }

        public string? GetResidentDOB()
        {
            return Dob;
        }

        public int GetResidentAge()
        {
            return Age;
        }

        public string? GetResidentGender()
        {
            return Gender;
        }

        public string? GetResidentCivilStatus()
        {
            return Civil_status;
        }

        public string? GetResidentReligion()
        {
            return Religion;
        }

        public string? GetOccupation()
        {
            return Occupation;
        }

        public string? GetEducationalAttainment()
        {
            return Ed_attain;
        }

        public string? GetPurok()
        {
            return Purok;
        }

        public int GetHouseholdNumber()
        {
            return Household_number;
        }
    }
}
