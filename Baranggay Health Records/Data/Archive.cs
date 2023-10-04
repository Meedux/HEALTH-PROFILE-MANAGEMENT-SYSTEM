namespace Baranggay_Health_Records.Data
{
    public class Archive
    {
        public String? Name { get; set; }

        public String? Date { get; set; }

        public String? GetName()
        {
            return this.Name;
        }

        public String? GetDate()
        {
            return this.Date;
        }
    }
}
