

namespace Baranggay_Health_Records.Data
{
    public class ResidentIllnesses
    {
        public int id { get; set; } = 0;

        public int residentId { get; set; } = 0;

        public int illnessId { get; set; } = 0;


        public int GetID()
        {
            return this.id;
        }

        public int GetResidentID()
        {
            return this.residentId;
        }

        public int GetIllnessID()
        {
            return this.illnessId;
        }

    }
}
