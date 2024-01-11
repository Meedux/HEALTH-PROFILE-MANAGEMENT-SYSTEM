namespace Baranggay_Health_Records.Data
{
    public class ResidentStatuses
    {
        public int id { get; set; } = 0;

        public int residentId { get; set; } = 0;

        public int statusId { get; set; } = 0;

        public int GetID()
        {
            return this.id;
        }

        public int GetResidentID()
        {
            return this.residentId;
        }

        public int GetStatusID()
        {
            return this.statusId;
        }
    }
}
