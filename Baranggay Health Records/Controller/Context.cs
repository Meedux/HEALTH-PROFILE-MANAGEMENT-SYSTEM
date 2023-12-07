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
            using (var connection = _sqlConnector.GetConnection())
            {
                var query = "SELECT Type FROM accounts WHERE Email = @Email AND Password = @Password";
                var type = connection.ExecuteScalar<string>(query, new { Email = email, Password = password });

                if (type != "")
                {
                    if (type == "bhw")
                    {
                        Console.WriteLine("BHW");
                        navigationManager.NavigateTo("/bhw/");
                    }
                    else if (type == "secretary")
                    {
                        navigationManager.NavigateTo("/secretary/");
                    }
                    return true;
                }
                return false;
            }
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
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    int currentYear = DateTime.Now.Year;

                    const string query = @"
                SELECT COUNT(*) FROM resident WHERE SUBSTRING(dob, -4) = @CurrentYear;
            ";

                    int totalCount = connection.ExecuteScalar<int>(query, new { CurrentYear = currentYear });
                    return totalCount;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }

        public int GetTotalHouseholds()
        {
            using (var connection = _sqlConnector.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM household";

                try
                {
                    int householdCount = connection.QuerySingle<int>(query);
                    return householdCount;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting household count: {ex.Message}");
                    return -1;
                }
            }
        }

        public int GetTotalMedicine() {
            using (var connection = _sqlConnector.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM medicine";

                try
                {
                    int medicineCount = connection.QuerySingle<int>(query);
                    return medicineCount;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting medicine count: {ex.Message}");
                    return -1;
                }
            }
        }

        public int GetTotalIllnessesOccured()
        {
            int totalOccurrence = 0;

            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    const string query = "SELECT SUM(Occurence) FROM Illness";
                    totalOccurrence = connection.QueryFirstOrDefault<int>(query);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }

            return totalOccurrence;

        }

        public int GetTotalPrenates()
        {
            using (var connection = _sqlConnector.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM Resident WHERE isPrenate = 1";

                try
                {
                    int totalPregnant = connection.QuerySingle<int>(query);
                    return totalPregnant;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting total pregnant count: {ex.Message}");
                    return -1; // or any suitable error value
                }
            }
        }


        public int GetTotalPWD()
        {
            using (var connection = _sqlConnector.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM Resident WHERE isPWD = 1";

                try
                {
                    int totalPWD = connection.QuerySingle<int>(query);
                    return totalPWD;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting total PWD count: {ex.Message}");
                    return -1; // or any suitable error value
                }
            }
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
                Console.WriteLine("Fetching Archive Data");
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
                var residentHealthStatuses = connection.Query<ResidentHealthStatusModel>("SELECT * FROM rhs").ToList();
                Console.WriteLine("Fetching Resident Health Status Data");
                return residentHealthStatuses;
            }
        }

        public List<ResidentHealthStatusModel> GetResidentHealthStatusesByIllness(string? illness)
        {
            using (MySqlConnection connection = _sqlConnector.GetConnection())
            {
                var residentHealthStatuses = connection.Query<ResidentHealthStatusModel>($"SELECT * FROM rhs WHERE Typeofillness = '{illness}'").ToList();
                Console.WriteLine("Fetching Resident Health Status Data");
                return residentHealthStatuses;
            }
        }

        public List<ResidentMedicineModel> GetMedicines()
        {
            using (MySqlConnection connection = _sqlConnector.GetConnection())
            {
                var residentMedicines = connection.Query<ResidentMedicineModel>("SELECT * FROM medicine").ToList();
                Console.WriteLine("Fetching Medicine Data");
                return residentMedicines;
            }
        }

        public List<IllnessModel> GetIllnesses()
        {
            using (MySqlConnection connection = _sqlConnector.GetConnection())
            {
                var ill = connection.Query<IllnessModel>("SELECT * FROM illness").ToList();
                Console.WriteLine("Fetching Medicine Data");
                return ill;
            }
        }

        public List<ResidentModel> GetPurokByAdult(string? purok)
        {
            using (MySqlConnection connection = _sqlConnector.GetConnection())
            {
                var residents = connection.Query<ResidentModel>("SELECT * FROM resident WHERE age >= 18 AND purok = @purok AND age < 60", new { purok = purok }).ToList();
                Console.WriteLine("Fetching Resident Data");
                return residents;
            }
        }

        public List<ResidentModel> GetPurokByMinor(string? purok)
        {
            using (MySqlConnection connection = _sqlConnector.GetConnection())
            {
                var residents = connection.Query<ResidentModel>("SELECT * FROM resident WHERE age < 18 AND purok = @purok", new { purok = purok }).ToList();
                Console.WriteLine("Fetching Resident Data");
                return residents;
            }
        }


        public List<ResidentModel> GetPurokBySenior(string? purok)
        {
            using (MySqlConnection connection = _sqlConnector.GetConnection())
            {
                var residents = connection.Query<ResidentModel>("SELECT * FROM resident WHERE age >= 60 AND purok = @purok", new { purok = purok }).ToList();
                Console.WriteLine("Fetching Resident Data");
                return residents;
            }
        }

        public List<ResidentModel> GetPurokByNewBorn(string? purok)
        {
            using (MySqlConnection connection = _sqlConnector.GetConnection())
            {
                int currentYear = DateTime.Now.Year;
                var residents = connection.Query<ResidentModel>("SELECT * FROM resident WHERE SUBSTRING(dob, -4) = @CurrentYear;", new { CurrentYear = currentYear }).ToList();
                Console.WriteLine("Fetching New Born Data");
                return residents;
            }
        }

        public List<HouseholdModel> GetHouseholdByPurok(string? purok)
        {
            using (MySqlConnection connection = _sqlConnector.GetConnection())
            {
                var query = @"
                SELECT h.* 
                FROM household h
                INNER JOIN resident u ON h.MemberID = u.ID
                WHERE u.Purok = @Purok";

                var households = connection.Query<HouseholdModel>(query, new { Purok = purok }).ToList();
                Console.WriteLine("Fetching Household Data");
                return households;
            }
        }


        //Single Query
        public ResidentModel GetResident(int? ID)
        {
            using (var connection = _sqlConnector.GetConnection())
            {
                ResidentModel resident = new ResidentModel();

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

        public HouseholdModel? GetHousehold(int? ID)
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

        public ResidentHealthStatusModel? GetResidentHealthStatus(int? ID)
        {

            using (var connection = _sqlConnector.GetConnection())
            {
                ResidentHealthStatusModel? residentHealthStatus = null; // Initialize with null

                string query = "SELECT * FROM rhs WHERE ResidentID = @ID";
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
        public int GetMedicineTypeCount(string type)
        {
            using (var connection = _sqlConnector.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM Medicine WHERE medicineType = @Type";

                try
                {
                    int medicineTypeCount = connection.QuerySingle<int>(query, new { Type = type });
                    return medicineTypeCount;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting medicine type count: {ex.Message}");
                    return -1; // or any suitable error value
                }
            }
        }

        //Purok Health Details

        public int GetIllnessCount(int ID)
        {
            using (var connection = _sqlConnector.GetConnection())
            {
                string query = "SELECT Occurence FROM illness WHERE ID = @ID";

                try
                {
                    int illnessCount = connection.QuerySingle<int>(query, new { ID });
                    return illnessCount;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return -1; // or any suitable error value
                }
            }
        }


        public List<ResidentModel> GetIllnessesByIllnessID(int? ID)
        {
            using (MySqlConnection connection = _sqlConnector.GetConnection())
            {
                var query = @"
                    SELECT r.* 
                    FROM resident r
                    JOIN rhs ON r.ID = rhs.ResidentId
                    WHERE rhs.TypeofillnessID = @ID";

                var residents = connection.Query<ResidentModel>(query, new { ID }).ToList();
                Console.WriteLine("Fetching Resident Data");
                return residents;
            }
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


        public int CreateResident(ResidentModel resident)
        {
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    const string query = @"
                INSERT INTO Resident (FirstName, MiddleName, LastName, Suffix, Dob, Age, Gender, Civil_status, Religion, Occupation, Ed_attain, Household_number, Purok, IsPWD, IsSenior)
                VALUES (@FirstName, @MiddleName, @LastName, @Suffix, @Dob, @Age, @Gender, @Civil_status, @Religion, @Occupation, @Ed_attain, @Household_number, @Purok, @IsPWD, @IsSenior);
                SELECT LAST_INSERT_ID();";

                    int newResidentId = connection.ExecuteScalar<int>(query, resident);
                    return newResidentId;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }

        public int CreateResidentHealthStatus(ResidentHealthStatusModel RHS)
        {
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    const string query = @"
                    INSERT INTO rhs (ResidentId, TypeofillnessID, Weight, Height, Temperature, BloodPressure)
                    VALUES (@ResidentId, @TypeofillnessID, @Weight, @Height, @Temperature, @BloodPressure);
                    SELECT LAST_INSERT_ID();
                    ";

                    int newRHS_ID = connection.ExecuteScalar<int>(query, new { ResidentId = RHS.ResidentId, TypeofillnessID = RHS.TypeofillnessID, Weight = RHS.Weight, Height = RHS.Height, Temperature = RHS.Temperature, BloodPressure = RHS.BloodPressure });
                    return newRHS_ID;
                }
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }

        }

        public int CreateHousehold(HouseholdModel Model)
        {
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    const string query = @"
                    INSERT INTO household (FathersName, MothersName, HeadofFamily, Member, MemberID, FamilyCount)
                    VALUES (@FathersName, @MothersName, @HeadofFamily, @Member, @MemberID, @FamilyCount);
                    SELECT LAST_INSERT_ID();
                    ";

                    int newHousehold_ID = connection.ExecuteScalar<int>(query, Model);
                    return newHousehold_ID;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }

        public int AddMedicine(ResidentMedicineModel medicine)
        {
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    const string query = @"
                        INSERT INTO medicine (MedicineName, Description, Quality, ExpirationDate, ReleaseDate, Stock)
                        VALUES (@MedicineName, @Description, @Quality, @ExpirationDate, @ReleaseDate, @Stock);
                        SELECT LAST_INSERT_ID();";

                    int newMedicineId = connection.ExecuteScalar<int>(query, new {
                        MedicineName = medicine.MedicineName,
                        Description = medicine.Description,
                        Quality = medicine.Quality,
                        ExpirationDate = medicine.ExpirationDate,
                        ReleaseDate = medicine.ReleaseDate,
                        Stock = 0
                    });
                    return newMedicineId;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }

        public void UpdateMedicineStock(int medicineID, int newStock)
        {
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    const string query = @"
                    UPDATE medicine SET stock = @newStock WHERE ID = @medicineID
                    ";

                    connection.Execute(query, new
                    {
                       newStock,
                       medicineID
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }


        public void UpdateResident(ResidentModel model, int? ID)
        {
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    const string query = @"
                UPDATE Resident
                SET FirstName = @FirstName, MiddleName = @MiddleName, LastName = @LastName,
                    Suffix = @Suffix, Dob = @Dob, Age = @Age, Gender = @Gender,
                    Civil_status = @Civil_status, Religion = @Religion, Occupation = @Occupation,
                    Ed_attain = @Ed_attain, Household_number = @Household_number,
                    Purok = @Purok, IsPWD = @IsPWD, IsSenior = @IsSenior
                WHERE ID = @ID;
            ";

                    connection.Execute(query, new
                    {
                        model.FirstName,
                        model.MiddleName,
                        model.LastName,
                        model.Suffix,
                        model.Dob,
                        model.Age,
                        model.Gender,
                        model.Civil_status,
                        model.Religion,
                        model.Occupation,
                        model.Ed_attain,
                        model.Household_number,
                        model.Purok,
                        model.IsPWD,
                        model.IsSenior,
                        ID
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Handle exceptions as needed
            }
        }

        public void UpdateHousehold(HouseholdModel model, int? ID)
        {
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    const string query = @"
                UPDATE household
                SET FathersName = @FathersName, MothersName = @MothersName,
                    HeadofFamily = @HeadofFamily, Member = @Member, FamilyCount = @FamilyCount
                WHERE MemberID = @ID;
            ";

                    connection.Execute(query, new
                    {
                        model.FathersName,
                        model.MothersName,
                        model.HeadofFamily,
                        model.Member,
                        model.FamilyCount,
                        ID
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                // Handle exceptions as needed
            }
        }

        public void UpdateResidentHealthStatus(ResidentHealthStatusModel model, int? ID)
        {
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    const string query = @"
                UPDATE rhs
                SET TypeofillnessID = @TypeofillnessID, Weight = @Weight, Height = @Height,
                    Temperature = @Temperature, BloodPressure = @BloodPressure
                WHERE ResidentId = @ID;
            ";

                    connection.Execute(query, new
                    {
                        model.TypeofillnessID,
                        model.Weight,
                        model.Height,
                        model.Temperature,
                        model.BloodPressure,
                        ID
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void ArchiveData(string name, string type, int dataId)
        {
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    var todayDate = DateTime.Today.ToString("MM/dd/yyyy");

                    const string query = @"
                        INSERT INTO archive (Name, Type, ReferenceID, Date)
                        VALUES (@Name, @Archive_Type, @Archive_ReferenceID, @Date);
                    ";

                    connection.Execute(query, new { Name = name, Archive_Type = type, Archive_ReferenceID = dataId, Date = todayDate });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        public void DeleteData(string type, int dataId)
        {
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    if (type == "resident")
                    {
                        Console.WriteLine("Resident");
                        connection.Execute(
                            @"DELETE FROM resident WHERE ID = @DataId;
                              DELETE FROM rhs WHERE ResidentId = @DataId;
                              DELETE FROM household WHERE MemberID = @DataId",
                            new { DataId = dataId });
                    }
                    else if (type == "rhs")
                    {
                        Console.WriteLine("RHS");
                        connection.Execute(
                            @"DELETE FROM rhs WHERE ID = @DataId;
                              DELETE FROM resident WHERE ID = (SELECT ResidentId FROM rhs WHERE ID = @DataId);
                              DELETE FROM household WHERE MemberID = (SELECT ResidentId FROM rhs WHERE ID = @DataId)",
                            new { DataId = dataId });
                    }
                    else if (type == "household")
                    {
                        Console.WriteLine("Household");
                        connection.Execute(
                            @"DELETE FROM resident WHERE ID = (SELECT MemberID FROM household WHERE MemberID = @DataId);
                              DELETE FROM rhs WHERE ResidentId = (SELECT MemberID FROM household WHERE MemberID = @DataId);
                              DELETE FROM household WHERE MemberID = @DataId",
                            new { DataId = dataId });
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

        }

        public void DeleteResident(int dataId)
        {
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    Console.WriteLine("Resident");
                    connection.Execute(
                        @"DELETE FROM resident WHERE ID = @DataId;
                              DELETE FROM rhs WHERE ResidentId = @DataId;
                              DELETE FROM household WHERE MemberID = @DataId",
                        new { DataId = dataId });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void DeleteRHS(int dataId)
        {
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    Console.WriteLine("RHS");
                    connection.Execute(
                        @"DELETE FROM rhs WHERE ID = @DataId;
                              DELETE FROM resident WHERE ID = (SELECT ResidentId FROM rhs WHERE ID = @DataId);
                              DELETE FROM household WHERE MemberID = (SELECT ResidentId FROM rhs WHERE ID = @DataId)",
                        new { DataId = dataId });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void DeleteHousehold(int dataId)
        {
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    Console.WriteLine("Household");
                    connection.Execute(
                        @"DELETE FROM resident WHERE ID = (SELECT MemberID FROM household WHERE MemberID = @DataId);
                              DELETE FROM rhs WHERE ResidentId = (SELECT MemberID FROM household WHERE MemberID = @DataId);
                              DELETE FROM household WHERE MemberID = @DataId",
                        new { DataId = dataId });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public bool isArchived(HouseholdModel item)
        {
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    const string query = "SELECT COUNT(*) FROM archive WHERE ReferenceID = @ID AND Type = 'household'";
                    int count = connection.ExecuteScalar<int>(query, new { ID = item.MemberID });
                    return !(count > 0);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool isArchived(ResidentModel item)
        {
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    const string query = "SELECT COUNT(*) FROM archive WHERE ReferenceID = @ID AND Type = 'resident'";
                    int count = connection.ExecuteScalar<int>(query, new { ID = item.ID });
                    return !(count > 0);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool isArchived(ResidentHealthStatusModel item)
        {
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    const string query = "SELECT COUNT(*) FROM archive WHERE ReferenceID = @ID AND Type = 'rhs'";
                    int count = connection.ExecuteScalar<int>(query, new { ID = item.ResidentId });
                    return !(count > 0);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool isArchived(ResidentMedicineModel item)
        {
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    const string query = "SELECT COUNT(*) FROM archive WHERE ReferenceID = @ID AND Type = 'medicine'";
                    int count = connection.ExecuteScalar<int>(query, new { ID = item.ID });
                    return !(count > 0);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public void UnArchive(int ID, string type)
        {
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    const string query = @"
                        DELETE FROM archive
                        WHERE ReferenceID = @ID AND Type = @Type;
                    ";

                    connection.Execute(query, new { ID = ID, Type = type });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void AddIllness(IllnessModel ill)
        {
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    const string query = @"
                        INSERT INTO Illness (IllnessName, RecommendedMedicine, MedicineID, Occurence)
                        VALUES (@IllnessName, @RecommendedMedicine, @MedicineID, @Occurence);
                    ";

                    connection.Execute(query, new
                    {
                        ill.IllnessName,
                        ill.RecommendedMedicine, 
                        ill.MedicineID,
                        Occurence = 0
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void IncrementIllness(int ID)
        {
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    const string query = @"
                        UPDATE illness
                        SET Occurence = Occurence + 1
                        WHERE ID = @ID;
                    ";

                    connection.Execute(query, new { ID });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void DeleteMedicine(int? ID)
        {
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    const string query = "DELETE FROM medicine WHERE ID = @ID";
                    connection.Execute(query, new { ID });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        //public void DeleteResident(int? ID)
        //{
        //    try
        //    {
        //        using (var connection = _sqlConnector.GetConnection())
        //        {
        //            const string query = "DELETE FROM resident WHERE ID = @ID";
        //            connection.Execute(query, new { ID });
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //    }
        //}

        //public void DeleteRHS(int ID)
        //{
        //    try
        //    {
        //        using (var connection = _sqlConnector.GetConnection())
        //        {
        //            const string query = "DELETE FROM rhs WHERE ID = @ID";
        //            connection.Execute(query, new { ID });
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //    }
        //}

        //public void DeleteHousehold(int? ID)
        //{
        //    try
        //    {
        //        using (var connection = _sqlConnector.GetConnection())
        //        {
        //            const string query = "DELETE FROM household WHERE ID = @ID";
        //            connection.Execute(query, new { ID });
        //        }
        //    }
        //    catch (Exception e)
        //    {
        //        Console.WriteLine(e);
        //    }
        //}

    }
}


