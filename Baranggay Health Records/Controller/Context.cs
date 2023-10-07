using Baranggay_Health_Records.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;

namespace Baranggay_Health_Records.Controller
{
    public class Context
    {

        public List<Resident>? Residents { get; set; } = new List<Resident>();
        public List<Archive>? Archives { get; set; } = new List<Archive>();
        public List<Household>? Households { get; set; } = new List<Household>();
        public List<ResidentHealthStatus>? ResidentHealthStatus { get; set; } = new List<ResidentHealthStatus>();


        public SQLConnector Connector = new SQLConnector("", "", "", "");
        public String PurokHealthViewTracker = "Purok 1A";



        public bool Login(String? email, String? password, NavigationManager navigationManager)
        {
            if(email == "bhw@gmail.com" && password == "123")
            {
                Console.WriteLine("BHW");
                navigationManager.NavigateTo("/bhw/");
                return true;
            }else if(email == "secretary@gmail.com" && password == "123")
            {
                navigationManager.NavigateTo("/secretary/");
                return true;
            }
            return false;
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
        public void GetHouseholds()
        {
            Console.WriteLine("Fetching Household Data");
        }

        public void GetArchives()
        {
            Console.WriteLine("Fetching Archive Data");
        } 

        public void GetResidents()
        {
            Console.WriteLine("Fetching Resident Data");
        }

        public void GetResidentHealthStatuses()
        {
            Console.WriteLine("Fetching Resident Health Status Data");
        }

        public void GetMedicines()
        {
            Console.WriteLine("Fetching Medicine Data");
        }



        //Table Queries

        public void GetHealthMonitorData()
        {
            Console.WriteLine("Fetching Health Monitor Data");
        }

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
