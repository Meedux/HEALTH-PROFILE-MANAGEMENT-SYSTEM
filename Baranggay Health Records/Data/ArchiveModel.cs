namespace Baranggay_Health_Records.Data
{
    public class ArchiveModel
    {
        public int? ID { get; set; }
        public string? Name { get; set; }

        public string? Type { get; set; }

        public int ReferenceID { get; set; }

        public DateTime Date { get; set; }

        public int? GetID()
        {
            return this.ID;
        }

        public string? GetName()
        {
            return this.Name;
        }

        public string? GetReferenceType()
        {
            return this.Type;
        }

        public int GetReferenceID()
        {
            return this.ReferenceID;
        }

        public string? GetDate()
        {
            return this.Date.ToShortDateString();
        }
    }
}
