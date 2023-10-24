using Baranggay_Health_Records.Data;
using Dapper;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.SignalR;
using MySql.Data.MySqlClient;
using System.Data;
using System.Data.Common;

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



        public bool Login(string? email, string? password, NavigationManager navigationManager)
        {
            if (IsValidCredentials(email, password))
            {
                if (email == "bhw@gmail.com")
                {
                    Console.WriteLine("BHW");
                    navigationManager.NavigateTo("/bhw/");
                }
                else if (email == "secretary@gmail.com")
                {
                    navigationManager.NavigateTo("/secretary/");
                }
                return true;
            }
            return false;
        }

        private bool IsValidCredentials(string? email, string? password)
        {
            // Replace this with your actual authentication logic, e.g., querying a database
            if ((email == "bhw@gmail.com" && password == "123") || (email == "secretary@gmail.com" && password == "123"))
            {
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
            using (var connection = _sqlConnector.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM Resident";

                try
                {
                    int residentCount = connection.QuerySingle<int>(query);
                    return residentCount;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting resident count: {ex.Message}");
                    return -1;
                }
            }
        }

        public int GetTotalSeniorCitizenCount()
        {
            using (var connection = _sqlConnector.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM Resident WHERE Age >= 60";

                try
                {
                    int residentCount = connection.QuerySingle<int>(query);
                    return residentCount;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting resident count: {ex.Message}");
                    return -1;
                }
            }
        }

        public int GetTotalMinorCount()
        {
            using (var connection = _sqlConnector.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM Resident WHERE Age < 18";

                try
                {
                    int residentCount = connection.QuerySingle<int>(query);
                    return residentCount;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting resident count: {ex.Message}");
                    return -1;
                }
            }
        }

        public int GetTotalAdultCount()
        {
            using (var connection = _sqlConnector.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM Resident WHERE Age >= 18";

                try
                {
                    int residentCount = connection.QuerySingle<int>(query);
                    return residentCount;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting resident count: {ex.Message}");
                    return -1;
                }
            }
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
            using (MySqlConnection connection = _sqlConnector.GetConnection())
            {
                var residents = connection.Query<ResidentModel>("SELECT * FROM resident").ToList();
                Console.WriteLine("Fetching Resident Data");
                return residents;
            }
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


        public void CreateResident(ResidentModel resident)
        {
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    connection.Execute(@"
                        INSERT INTO Resident (FirstName, MiddleName, LastName, Suffix, Dob, Age, Gender, Civil_status, Religion, Occupation, Ed_attain, Household_number, Purok, IsPWD, IsSenior)
                        VALUES (@FirstName, @MiddleName, @LastName, @Suffix, @Dob, @Age, @Gender, @Civil_status, @Religion, @Occupation, @Ed_attain, @Household_number, @Purok, @IsPWD, @IsSenior);
                    ", resident);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void CreateResidentHealthStatus()
        {

        }
    }
}
