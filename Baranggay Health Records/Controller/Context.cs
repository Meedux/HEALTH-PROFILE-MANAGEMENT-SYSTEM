using Baranggay_Health_Records.Data;

namespace Baranggay_Health_Records.Controller
{
    public class Context
    {

        public List<Resident>? Residents { get; set; }
        public SQLConnector Connector = new SQLConnector("", "", "", "");
        public String PurokHealthViewTracker = "Purok 1A";

       

        public void AddResident(Resident temp)
        {
            Residents?.Add(temp);
        }

        public List<Resident>? GetAllResidents()
        {
            return Residents;
        }
            
        public void ChangeViewTracker(String Purok)
        {
            PurokHealthViewTracker = Purok;
        }
    }
}
