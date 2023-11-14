using System.ComponentModel.DataAnnotations;

namespace Baranggay_Health_Records.Data
{
    public class HouseholdModel
    {
        public int ID { get; set; }

        [Required] public String? FathersName { get; set; }

        [Required] public String? MothersName { get; set; }

        [Required] public String? HeadofFamily { get; set; }

        [Required] public String? Member { get; set; }

        [Required] public int? MemberID { get; set; }

        [Required] public int FamilyCount { get; set; }

        public int GetFamilyCount()
        {
            return this.FamilyCount;
        }

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
