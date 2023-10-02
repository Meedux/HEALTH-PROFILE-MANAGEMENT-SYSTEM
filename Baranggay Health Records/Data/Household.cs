using System.ComponentModel.DataAnnotations;

namespace Baranggay_Health_Records.Data
{
    public class Household
    {
        public int Id { get; set; }

        [Required] public String? FathersName { get; set; }

        [Required] public String? MothersName { get; set; }

        [Required] public String? HeadofFamily { get; set; }

        [Required] public int? HeadofFamilyID { get; set; }

        [Required] public String? Member { get; set; }

        [Required] public int? MemberID { get; set; }

            
    }
}
