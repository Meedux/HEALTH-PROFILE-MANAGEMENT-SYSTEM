using System.ComponentModel.DataAnnotations;

namespace Baranggay_Health_Records.Data
{
    public class HouseholdModel
    {
        public int ID { get; set; }

        [Required] public string FathersName { get; set; } = "";

        [Required] public string MothersName { get; set; } = "";

        [Required] public string HeadofFamily { get; set; } = "";

        [Required] public string Member { get; set; } = "";

        [Required] public int MemberID { get; set; }

        [Required] public int FamilyCount { get; set; }

        public int GetFamilyCount()
        {
            return this.FamilyCount;
        }

        public int GetID()
        {
            return this.ID;
        }

        public string GetFathersName()
        {
            return this.FathersName;
        }

        public string GetMothersName()
        {
            return this.MothersName;
        }

        public string GetHeadofFamily()
        {
            return this.HeadofFamily;
        }


        public string GetMember()
        {
            return this.Member;
        }

        public int GetMemberID()
        {
            return this.MemberID;
        }
    }
}
