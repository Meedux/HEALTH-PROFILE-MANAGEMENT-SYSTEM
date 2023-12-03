using System.ComponentModel.DataAnnotations;

namespace Baranggay_Health_Records.Data
{
    public class HouseholdModel
    {
        public int ID { get; set; }

        [Required] public string? FathersName { get; set; }

        [Required] public string? MothersName { get; set; }

        [Required] public string? HeadofFamily { get; set; }

        [Required] public string? Member { get; set; }

        [Required] public int? MemberID { get; set; }

        [Required] public int FamilyCount { get; set; }

        public int GetFamilyCount()
        {
            return FamilyCount;
        }

        public int? GetID()
        {
            return ID;
        }

        public string? GetFathersName()
        {
            return FathersName;
        }

        public string? GetMothersName()
        {
            return MothersName;
        }

        public string? GetHeadofFamily()
        {
            return HeadofFamily;
        }


        public string? GetMember()
        {
            return Member;
        }

        public int? GetMemberID()
        {
            return MemberID;
        }
    }
}
