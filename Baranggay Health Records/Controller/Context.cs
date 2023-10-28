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
            using (MySqlConnection connection = _sqlConnector.GetConnection())
            {
                var households = connection.Query<HouseholdModel>("SELECT * FROM household").ToList();
                Console.WriteLine("Fetching Household Data");
                return households;
            }
        }

        public List<ArchiveModel> GetArchives()
        {
            using (MySqlConnection connection = _sqlConnector.GetConnection())
            {
                var archives = connection.Query<ArchiveModel>("SELECT * FROM archive").ToList();
                Console.WriteLine("Fetching Resident Data");
                return archives;
            }
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
            using (MySqlConnection connection = _sqlConnector.GetConnection())
            {
                var residentHealthStatuses = connection.Query<ResidentHealthStatusModel>("SELECT * FROM resident_health_status").ToList();
                Console.WriteLine("Fetching Resident Health Status Data");
                return residentHealthStatuses;
            }
        }

        public List<ResidentMedicineModel> GetMedicines()
        {
            using(MySqlConnection connection = _sqlConnector.GetConnection())
            {
                var residentMedicines = connection.Query<ResidentMedicineModel>("SELECT * FROM medicine").ToList();
                Console.WriteLine("Fetching Medicine Data");
                return residentMedicines;
            }
        }


        //Single Query
        public ResidentModel? GetResident(int ID)
        {
            using (var connection = _sqlConnector.GetConnection())
            {
                ResidentModel? resident = null; // Initialize with null

                string query = "SELECT * FROM Resident WHERE ID = @ID";
                try
                {
                    resident = connection.QuerySingleOrDefault<ResidentModel>(query, new { ID });

                    if (resident == null)
                    {
                        Console.WriteLine($"Resident with ID {ID} not found.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting resident: {ex.Message}");
                }
                
                return resident;
            }
        }

        public HouseholdModel? GetHousehold(int ID)
        {
            using (var connection = _sqlConnector.GetConnection())
            {
                HouseholdModel? household = null; // Initialize with null

                string query = "SELECT * FROM household WHERE MemberID = @ID";
                try
                {
                    household = connection.QuerySingleOrDefault<HouseholdModel>(query, new { ID });

                    if (household == null)
                    {
                        Console.WriteLine($"Household with ID: {ID} not found.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting Household: {ex.Message}");
                }

                return household;
            }
        }

        public ResidentHealthStatusModel? GetResidentHealthStatus(int ID)
        {

            using (var connection = _sqlConnector.GetConnection())
            {
                ResidentHealthStatusModel? residentHealthStatus = null; // Initialize with null

                string query = "SELECT * FROM resident_health_status WHERE ResidentID = @ID";
                try
                {
                    residentHealthStatus = connection.QuerySingleOrDefault<ResidentHealthStatusModel>(query, new { ID });

                    if (residentHealthStatus == null)
                    {
                        Console.WriteLine($"Resident Health Status with ID: {ID} not found.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting resident health status: {ex.Message}");
                }

                return residentHealthStatus;
            }
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
