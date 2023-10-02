using System.ComponentModel.DataAnnotations;

namespace Baranggay_Health_Records.Data
{
    public class Household
    {
        public int ID { get; set; }

        [Required] public String? FathersName { get; set; }

        [Required] public String? MothersName { get; set; }

        [Required] public String? HeadofFamily { get; set; }

        [Required] public int? HeadofFamilyID { get; set; }

        [Required] public String? Member { get; set; }

        [Required] public int? MemberID { get; set; }

        public int? GetID()
        {
            return this.ID;
        }

        public String? GetFathersName()
        {
            return this.FathersName;
        }

        public String? GetMothersName()
        {
            return this.MothersName;
        }

        public String? GetHeadofFamily()
        {
            return this.HeadofFamily;
        }

        public int? GetHeadofFamilyID()
        {
            return this.HeadofFamilyID;
        }

        public String? GetMember()
        {
            return this.Member;
        }

        public int? GetMemberID()
        {
            return this.MemberID;
        }
    }
}
