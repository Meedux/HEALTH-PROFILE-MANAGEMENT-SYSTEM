using Baranggay_Health_Records.Data;

namespace Baranggay_Health_Records.Controller
{
    public class Context
    {

        public List<Resident>? Residents { get; set; }
        private SQLConnector sqlConnector = new SQLConnector("", "", "", "");

        public void AddResident(Resident temp)
        {
            Residents?.Add(temp);
        }

        public List<Resident> GetAllResidents()
        {
            return Residents;
        }

        public Resident GetPresident(int index)
        {
            return null;
        }

        public bool UpdateResident(Resident temp, int index)
        {
            return true;
        }

        public bool DeleteResident(Resident temp, int index)
        {
            return true;
        }

        public void Login(string username, string password)
        {
            
        }

       

    }
}
