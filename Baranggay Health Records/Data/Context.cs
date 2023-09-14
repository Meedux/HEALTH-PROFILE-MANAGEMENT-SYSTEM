namespace Baranggay_Health_Records.Data
{
    public class Context
    {
        List<Resident> List { get; set; }

        public void AddResident(Resident resident)
        {
            this.List.Add(resident);
        }
    }
}
