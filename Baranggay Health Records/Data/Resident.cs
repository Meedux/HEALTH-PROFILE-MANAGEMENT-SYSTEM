using Baranggay_Health_Records.Pages.Secretary;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Baranggay_Health_Records.Data
{
    public class Resident
    {
        public string firstName = "";
        string middleName = "";
        string lastName = "";
        string suffix = "";
        string dob = "";
        int age = 0;
        string gender = "";
        string civil_status = "";
        string religion = "";
        string occupation = "";
        string fathers_name = "";
        string mothers_name = "";
        string head = "";
        int num_of_fam = 0;
        string ed_attain = "";
        int household_number = 0;
        string purok = "";
        string status_type = "";

        public Resident(
            String firstName,
            String middleName,
            String lastName,
            String suffix,
            String dob,
            int age,
            String civil_status,
            String religion,
            String occupation,
            String fathers_name,
            String mothers_name,
            String head,
            String ed_attain,
            String purok,
            String status_type
       )
        {
            this.firstName = firstName;
            this.middleName = middleName; 
            this.lastName = lastName;
            this.suffix = suffix;
            this.dob = dob;
            this.age = age;
            this.civil_status = civil_status;
            this.religion = religion;
            this.occupation = occupation;
            this.fathers_name = fathers_name;
            this.mothers_name = mothers_name;
            this.head = head;
            this.ed_attain = ed_attain;
            this.purok = purok;
            this.status_type = status_type;
        }
        
    }
}
