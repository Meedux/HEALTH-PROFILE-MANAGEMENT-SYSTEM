using Baranggay_Health_Records.Pages.Secretary;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;

namespace Baranggay_Health_Records.Data
{
    public class Resident
    {
        public int Id { get; set; }

        [Required]
        public string? FirstName { get; set; }

        [Required]
        public string? MiddleName { get; set; }

        [Required]
        public string? LastName { get; set; }

        [Required]
        public string? Suffix { get; set; }

        [Required]
        public string? Dob { get; set; }

        [Range(0, 200)]
        public int Age { get; set;} 
        public string? Gender { get; set; }
        public string? Civil_status { get; set; }
        public string? Religion { get; set; }
        public string? Occupation { get; set; }
        public string? Fathers_name { get; set; }
        public string? Mothers_name { get; set; }
        public string? Head { get; set; }
        public int Num_of_fam { get; set; } 
        public string? Ed_attain { get; set; }
        public int Household_number { get; set;} 
        public string? Purok { get; set; }
        public string? Status_type { get; set; }


        
    }
}
