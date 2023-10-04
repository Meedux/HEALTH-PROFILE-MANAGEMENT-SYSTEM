using Baranggay_Health_Records.Data;

namespace Baranggay_Health_Records.Controller
{
    public class Context
    {

        public List<Resident>? Residents { get; set; }
        public SQLConnector Connector = new SQLConnector("", "", "", "");
        public String PurokHealthViewTracker = "Purok 1A";


        public bool Login(string username, string password)
        {
            return true;
        }

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

        //Dashboard
        public int GetResidentCount()
        {
            return 1000;
        }

        public int GetTotalSeniorCitizenCount()
        {
            return 0;
        }

        public int GetTotalMinorCount()
        {
            return 0;
        }

        public int GetTotalAdultCount()
        {
            return 0;
        }

        public int GetTotalNewBorn()
        {
            return 0;
        }

        public int GetTotalHouseholds()
        {
            return 0;
        }

        public int GetTotalMedicine() {
            return 0;
        }

        public int GetTotalIllnessesOccured()
        {
            return 0;
        }

        public int GetTotalPrenates()
        {
            return 0;
        }

        public int GetTotalPWD()
        {
            return 0;
        }

        //Table Queries
        public List<Household>? GetHouseholds()
        {
            return null;
        }

        public List<Archive>? GetArchives()
        {
            return null;
        } 

        public List<Resident>? GetResidents()
        {
            return null;
        }

        public List<ResidentHealthStatus>? GetResidentHealthStatuses()
        {
            return null;
        }

        public List<ResidentMedicine>? GetMedicines()
        {
            return null;
        }
    }
}
