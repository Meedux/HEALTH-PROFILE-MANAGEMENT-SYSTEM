using Baranggay_Health_Records.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;

namespace Baranggay_Health_Records.Controller
{
    public class Context
    {

        public List<ResidentModel>? Residents { get; set; } = new List<ResidentModel>();

        public List<HouseholdModel>? Households { get; set; } = new List<HouseholdModel>();
        public List<ResidentHealthStatusModel>? ResidentHealthStatus { get; set; } = new List<ResidentHealthStatusModel>();

        public String PurokHealthViewTracker = "Purok 1A";

        private readonly SQLConnector _sqlConnector;

        public Context(IConfiguration configuration)
        {
            _sqlConnector = new SQLConnector(configuration);
        }



        public bool Login(String? email, String? password, NavigationManager navigationManager)
        {
            if (email == "bhw@gmail.com" && password == "123")
            {
                Console.WriteLine("BHW");
                navigationManager.NavigateTo("/bhw/");
                return true;
            } else if (email == "secretary@gmail.com" && password == "123")
            {
                navigationManager.NavigateTo("/secretary/");
                return true;
            }
            return false;
        }

        public void AddResident(ResidentModel temp)
        {
            Residents?.Add(temp);
        }

        public List<ResidentModel>? GetAllResidents()
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
            return 2000;
        }

        public int GetTotalSeniorCitizenCount()
        {
            return 100;
        }

        public int GetTotalMinorCount()
        {
            return 100;
        }

        public int GetTotalAdultCount()
        {
            return 100;
        }

        public int GetTotalNewBorn()
        {
            return 100;
        }

        public int GetTotalHouseholds()
        {
            return 100;
        }

        public int GetTotalMedicine() {
            return 100;
        }

        public int GetTotalIllnessesOccured()
        {
            return 100;
        }

        public int GetTotalPrenates()
        {
            return 100;
        }

        public int GetTotalPWD()
        {
            return 100;
        }

        //Table Queries
        public List<HouseholdModel> GetHouseholds()
        {
            List<HouseholdModel> households = new List<HouseholdModel>();
            Console.WriteLine("Fetching Household Data");
            return households;
        }

        public List<ArchiveModel> GetArchives()
        {
            List<ArchiveModel> archives = new List<ArchiveModel>();
            Console.WriteLine("Fetching Archive Data");

            return archives;
        } 

        public List<ResidentModel> GetResidents()
        {
            List<ResidentModel> residents  = new List<ResidentModel>();
            Console.WriteLine("Fetching Resident Data");
            return residents;
        }

        public List<ResidentHealthStatusModel> GetResidentHealthStatuses()
        {
            List<ResidentHealthStatusModel> residentHealthStatuses = new List<ResidentHealthStatusModel>();
            Console.WriteLine("Fetching Resident Health Status Data");
            return residentHealthStatuses;
        }

        public List<ResidentMedicineModel> GetMedicines()
        {
            List<ResidentMedicineModel> residentMedicines = new List<ResidentMedicineModel>();
            Console.WriteLine("Fetching Medicine Data");
            return residentMedicines;
        }


        //Single Query
        public ResidentModel GetResident(int ID)
        {
            ResidentModel resident = new ResidentModel();
            Console.WriteLine($"Fetching Resident Data with ID of {ID}");
            return resident;
        }

        public HouseholdModel GetHousehold(int ID)
        {
            HouseholdModel household = new HouseholdModel();
            Console.WriteLine($"Fetching Household Data with Resident ID of {ID}");
            return household;
        }


        //Table Queries


        public void GetHouseholdData()
        {
            Console.WriteLine("Fetching Household Data");
        }

        public void GetMedicineData()
        {
            Console.WriteLine("Fetching Medicine Data");
        }

        public void GetSeniorCitizenData()
        {
            Console.WriteLine("Fetching Senior Citizen Data");
        }

        //Medicine Inventory
        public int GetParacetamolCount()
        {
            return 100;
        }

        public int GetMefinamicCount()
        {
            return 100;
        }

        public int GetBiogesicCount()
        {
            return 100;
        }

        //Purok Health Details

        public int GetFluCount()
        {
            return 200;
        }

        public int GetMalnourishedCount()
        {
            return 200;
        }

        public int GetMeaslesCount()
        {
            return 200;
        }

        public int GetTuberculosisCount()
        {
            return 200;
        }

        public int GetDewormingCount()
        {
            return 200;
        }

        public int GetPrenatalCount()
        {
            return 200;
        }

        public int GetHouseholdCount()
        {
            return 200;
        }

        public int GetSeniorCitizenCount()
        {
            return 200;
        }
       


    }
}
