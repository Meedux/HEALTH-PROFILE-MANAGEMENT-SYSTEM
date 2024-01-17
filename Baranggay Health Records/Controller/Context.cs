using Baranggay_Health_Records.Data;
using Dapper;
using Microsoft.AspNetCore.Components;
using MySql.Data.MySqlClient;
using Microsoft.AspNetCore.Hosting;

using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using System.Collections.Generic;
using System.IO;
using Microsoft.JSInterop;


namespace Baranggay_Health_Records.Controller
{
    public class Context
    {
       
        public List<ResidentModel>? Residents { get; set; } = new List<ResidentModel>();
        public List<HouseholdModel>? Households { get; set; } = new List<HouseholdModel>();
        public List<ResidentHealthStatusModel>? ResidentHealthStatus { get; set; } = new List<ResidentHealthStatusModel>();
        public String PurokHealthViewTracker = "Purok 1A";
        private readonly SQLConnector _sqlConnector;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public Context(IConfiguration configuration, IWebHostEnvironment webHostEnvironment)
        {
            _sqlConnector = new SQLConnector(configuration);
            _webHostEnvironment = webHostEnvironment;
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
                const string query = @"SELECT COUNT(DISTINCT resident.ID) AS TotalResidents
                        FROM resident resident
                        WHERE NOT EXISTS (
                            SELECT 1
                            FROM Archive
                            WHERE ReferenceID = resident.ID
                            AND Type = 'resident' 
                        )";

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
                string query = @"
            SELECT COUNT(*)
            FROM resident r
            LEFT JOIN Archive a ON r.ID = a.ReferenceID AND a.Type = 'Resident'
            WHERE r.Age >= 60
            AND a.ReferenceID IS NULL";

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
                const string query = @"SELECT COUNT(DISTINCT resident.ID) AS TotalResidents
                        FROM resident resident
                        WHERE NOT EXISTS (
                            SELECT 1
                            FROM Archive
                            WHERE ReferenceID = resident.ID
                            AND Type = 'resident' 
                        ) AND age <= 18";

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

                    const string query = @"SELECT COUNT(DISTINCT resident.ID) AS TotalResidents
                        FROM resident resident
                        WHERE NOT EXISTS (
                            SELECT 1
                            FROM Archive
                            WHERE ReferenceID = resident.ID
                            AND Type = 'resident' 
                        ) AND SUBSTRING(dob, -4) = @CurrentYear";

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
                const string query = @"SELECT COUNT(DISTINCT household.ID) AS TotalHouseholds
                        FROM household household
                        WHERE NOT EXISTS (
                            SELECT 1
                            FROM Archive
                            WHERE ReferenceID = household.ID
                            AND Type = 'household' 
                        )";

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
                    const string query = @"SELECT COUNT(DISTINCT rhs.ResidentId) AS TotalResidentsWithIllness
                        FROM rhs rhs
                        WHERE NOT EXISTS (
                            SELECT 1
                            FROM Archive
                            WHERE ReferenceID = rhs.ID
                            AND Type = 'resident' OR Type = 'rhs'
                        )";
                    int? result = connection.QueryFirstOrDefault<int?>(query);
                    totalOccurrence = result.GetValueOrDefault();
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

        public List<ResidentHealthStatusModel> GetResidentHealthStatusesByIllness(int illness, string purok)
        {
            using (MySqlConnection connection = _sqlConnector.GetConnection())
            {
                string query = @"
                    SELECT *
                    FROM rhs
                    INNER JOIN resident r ON r.ID = rhs.ResidentId
                    INNER JOIN resident_illnesses ON r.ID = resident_illnesses.ResidentId
                    WHERE resident_illnesses.illnessId = @Illness
                    AND r.Purok = @Purok";
                var residentHealthStatuses = connection.Query<ResidentHealthStatusModel>(query, new { Illness = illness, Purok = purok }).ToList();
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
                ResidentModel? resident = new ResidentModel();

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

                ResidentModel temp = (resident != null) ? resident : new ResidentModel();

                return temp;
            }
        }

        public HouseholdModel GetHousehold(int? ID)
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

                HouseholdModel temp = (household != null) ? household : new HouseholdModel();

                return temp;
            }
        }

        public ResidentHealthStatusModel GetResidentHealthStatus(int? ID)
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

                ResidentHealthStatusModel temp = (residentHealthStatus != null) ? residentHealthStatus : new ResidentHealthStatusModel();

                return temp;
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

        public int GetIllnessCount(int ID, string Purok)
        {
            using (var connection = _sqlConnector.GetConnection())
            {
                string query = @"
                    SELECT COUNT(*)
                    FROM rhs
                    INNER JOIN resident r ON r.ID = rhs.ResidentId
                    INNER JOIN resident_illnesses ON r.ID = resident_illnesses.ResidentId
                    WHERE resident_illnesses.illnessId = @ID
                    AND r.Purok = @Purok";
                try
                {
                    int illnessCount = connection.QuerySingle<int>(query, new { ID, Purok });
                    return illnessCount;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return -1;
                }
            }
        }

        public int GetAllIllnessCount(int ID)
        {
            using (var connection = _sqlConnector.GetConnection())
            {
                string query = @"
                    SELECT COUNT(*)
                    FROM resident_illnesses
                    INNER JOIN resident r ON r.ID = resident_illnesses.residentId
                    LEFT JOIN Archive a ON r.ID = a.ReferenceID AND a.Type = 'rhs'
                    WHERE resident_illnesses.illnessId = @ID
                    AND a.ReferenceID IS NULL";
                try
                {
                    int illnessCount = connection.QuerySingle<int>(query, new { ID });
                    return illnessCount;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    return -1;
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
                    JOIN resident_illnesses ON r.ID = resident_illnesses.ResidentId
                    WHERE resident_illnesses.illnessId = @ID";

                var residents = connection.Query<ResidentModel>(query, new { ID }).ToList();
                Console.WriteLine("Fetching Resident Data");
                return residents;
            }
        }


        public int CreateResident(ResidentModel resident)
        {
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    const string query = @"
                INSERT INTO Resident (FirstName, MiddleName, LastName, Suffix, Dob, Age, Gender, Civil_status, Religion, Occupation, Ed_attain, Household_number, Purok, IsPWD, IsSenior, IDNo, statusId)
                VALUES (@FirstName, @MiddleName, @LastName, @Suffix, @Dob, @Age, @Gender, @Civil_status, @Religion, @Occupation, @Ed_attain, @Household_number, @Purok, @IsPWD, @IsSenior, @IDNo, @statusId);
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
                    INSERT INTO rhs (ResidentId,  Weight, Height, Temperature, BloodPressure)
                    VALUES (@ResidentId,  @Weight, @Height, @Temperature, @BloodPressure);
                    SELECT LAST_INSERT_ID();
                    ";

                    int newRHS_ID = connection.ExecuteScalar<int>(query, new { ResidentId = RHS.ResidentId, Weight = RHS.Weight, Height = RHS.Height, Temperature = RHS.Temperature, BloodPressure = RHS.BloodPressure });
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

        
        public int CreateResidentIllness(int illnessId, int residentId)
        {
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    const string query = @"
                    INSERT INTO resident_illnesses (residentId, illnessId)
                    VALUES (@residentId, @illnessId);
                    SELECT LAST_INSERT_ID();
                    ";

                    int newID = connection.ExecuteScalar<int>(query, new { residentId = residentId, illnessId = illnessId });
                    return newID;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return -1;
            }
        }

        public int CreateResidentStatus(int statusId, int residentId)
        {
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    const string query = @"
                    INSERT INTO resident_statuses (residentId, statusId)
                    VALUES (@residentId, @statusId);
                    SELECT LAST_INSERT_ID();
                    ";

                    int newID = connection.ExecuteScalar<int>(query, new { residentId = residentId, statusId = statusId });
                    return newID;
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
                        INSERT INTO medicine (MedicineName, Description, ExpirationDate, ReleaseDate, Stock)
                        VALUES (@MedicineName, @Description, @ExpirationDate, @ReleaseDate, @Stock);
                        SELECT LAST_INSERT_ID();";

                    int newMedicineId = connection.ExecuteScalar<int>(query, new {
                        MedicineName = medicine.MedicineName,
                        Description = medicine.Description,
                        ExpirationDate = medicine.ExpirationDate,
                        ReleaseDate = medicine.ReleaseDate,
                        Stock = medicine.Stock
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
                    Purok = @Purok, IsPWD = @IsPWD, IsSenior = @IsSenior, IDNo = @IDNo, statusId = @statusId
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
                        model.IDNo,
                        model.statusId,
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
                SET  Weight = @Weight, Height = @Height,
                    Temperature = @Temperature, BloodPressure = @BloodPressure
                WHERE ResidentId = @ID;
            ";

                    connection.Execute(query, new
                    {
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
                        INSERT INTO archive (Name, Type, ReferenceID)
                        VALUES (@Name, @Archive_Type, @Archive_ReferenceID);
                    ";

                    connection.Execute(query, new { Name = name, Archive_Type = type, Archive_ReferenceID = dataId});
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
                              DELETE FROM household WHERE MemberID = @DataId;
                              DELETE FROM resident_illnesses WHERE residentId = @DataId;
                              DELETE FROM resident_statuses WHERE residentId = @DataId;",
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
                              DELETE FROM household WHERE MemberID = (SELECT ResidentId FROM rhs WHERE ID = @DataId);
                              DELETE FROM resident_illnesses WHERE residentId = (SELECT ResidentId FROM rhs WHERE ID = @DataId);
                              DELETE FROM resident_statuses WHERE residentId = (SELECT ResidentId FROM rhs WHERE ID = @DataId);",
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

        public async void GenerateSeniorResident(List<ResidentModel> residents, IJSRuntime JS)
        {
            string fileName = "Residents.docx"; // Name of the generated file
            string file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", fileName); // Path within wwwroot)

            if (File.Exists(file))
            {
                File.Delete(file);
            }

            using (var document = WordprocessingDocument.Create(file, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = document.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());
                HeaderPart headerPart = mainPart.AddNewPart<HeaderPart>();

                string headerMarkup = @"<w:hdr xmlns:w='http://schemas.openxmlformats.org/wordprocessingml/2006/main'>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/> <!-- Center align -->
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='48'/> <!-- Enlarge font size -->
                                            <w:b/> <!-- Bold -->
                                        </w:rPr>
                                        <w:t>Baranggay Health Profiling</w:t>
                                    </w:r>
                                </w:p>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/>
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='36'/> <!-- Enlarge font size -->
                                            <w:b/>
                                        </w:rPr>
                                        <w:t>Municipality of Midsayap Barangay Central Bulanan</w:t>
                                    </w:r>
                                </w:p>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/>
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='36'/> <!-- Enlarge font size -->
                                            <w:b/>
                                        </w:rPr>
                                        <w:t>Senior Citizens Masterlist</w:t>
                                    </w:r>
                                </w:p>
                            </w:hdr>";

                using (StreamWriter streamWriter = new StreamWriter(headerPart.GetStream(FileMode.Create)))
                {
                    streamWriter.Write(headerMarkup);
                }

                SectionProperties sectionProperties = new SectionProperties();
                HeaderReference headerReference = new HeaderReference() { Type = HeaderFooterValues.Default, Id = mainPart.GetIdOfPart(headerPart) };
                sectionProperties.Append(headerReference);
                if (mainPart.Document.Body != null)
                {
                    mainPart.Document.Body.Append(sectionProperties);
                }
                Table table = new Table();
                TableProperties props = new TableProperties(
                    new TableWidth { Width = "100%", Type = TableWidthUnitValues.Pct },
                    new TableBorders(
                        new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 }
                    )
                );
                table.AppendChild<TableProperties>(props);

                TableRow headerRow = new TableRow();
                headerRow.Append(
                    CreateHeaderCell("ID"),
                    CreateHeaderCell("Full Name"),
                    CreateHeaderCell("Age"),
                    CreateHeaderCell("Gender"),
                    CreateHeaderCell("Household Number"),
                    CreateHeaderCell("Purok")
                );
                table.AppendChild(headerRow);
                Console.WriteLine(residents.Count);

                foreach (var resident in residents)
                {
                    TableRow dataRow = new TableRow();
                    dataRow.Append(
                        CreateTableCell(resident.GetIDNum().ToString()),
                        CreateTableCell($"{resident.GetResidentFirstName()} {resident.GetResidentMiddleName()} {resident.GetResidentLastName()} {resident.GetResidentSuffix()}"),
                        CreateTableCell(resident.Age.ToString()),
                        CreateTableCell(resident.GetResidentGender()),
                        CreateTableCell(resident.GetHouseholdNumber().ToString()),
                        CreateTableCell(resident.GetPurok())
                    );
                    table.AppendChild(dataRow);
                }
                body.Append(table);
                mainPart.Document.Save();
            }

            await DownloadFileFromStream(fileName, file, JS);
        }

        public async void GenerateResident(List<ResidentModel> residents, IJSRuntime JS)
        {
            string fileName = "Residents.docx";
            string file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", fileName);
            if (File.Exists(file))
            {
                File.Delete(file);
            }

            using (var document = WordprocessingDocument.Create(file, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = document.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());
                HeaderPart headerPart = mainPart.AddNewPart<HeaderPart>();

                string headerMarkup = @"<w:hdr xmlns:w='http://schemas.openxmlformats.org/wordprocessingml/2006/main'>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/> <!-- Center align -->
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='48'/> <!-- Enlarge font size -->
                                            <w:b/> <!-- Bold -->
                                        </w:rPr>
                                        <w:t>Baranggay Health Profiling</w:t>
                                    </w:r>
                                </w:p>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/>
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='36'/> <!-- Enlarge font size -->
                                            <w:b/>
                                        </w:rPr>
                                        <w:t>Municipality of Midsayap Barangay Central Bulanan</w:t>
                                    </w:r>
                                </w:p>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/>
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='36'/> <!-- Enlarge font size -->
                                            <w:b/>
                                        </w:rPr>
                                        <w:t>Residents Masterlist</w:t>
                                    </w:r>
                                </w:p>
                            </w:hdr>";



                using (StreamWriter streamWriter = new StreamWriter(headerPart.GetStream(FileMode.Create)))
                {
                    streamWriter.Write(headerMarkup);
                }

                SectionProperties sectionProperties = new SectionProperties();
                HeaderReference headerReference = new HeaderReference() { Type = HeaderFooterValues.Default, Id = mainPart.GetIdOfPart(headerPart) };
                sectionProperties.Append(headerReference);
                if(mainPart.Document.Body != null)
                {
                    mainPart.Document.Body.Append(sectionProperties);
                }

                Table table = new Table();
                TableProperties props = new TableProperties(
                    new TableWidth { Width = "100%", Type = TableWidthUnitValues.Pct },
                    new TableBorders(
                        new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 }
                    )
                );
                table.AppendChild<TableProperties>(props);

                TableRow headerRow = new TableRow();
                headerRow.Append(
                    CreateHeaderCell("ID"),
                    CreateHeaderCell("Full Name"),
                    CreateHeaderCell("Age"),
                    CreateHeaderCell("Gender"),
                    CreateHeaderCell("Household Number"),
                    CreateHeaderCell("Purok")
                );
                table.AppendChild(headerRow);

                Console.WriteLine(residents.Count);
                foreach (var resident in residents)
                {
                    TableRow dataRow = new TableRow();
                    dataRow.Append(
                        CreateTableCell(resident.GetID().ToString()),
                        CreateTableCell($"{resident.GetResidentFirstName()} {resident.GetResidentMiddleName()} {resident.GetResidentLastName()} {resident.GetResidentSuffix()}"),
                        CreateTableCell(resident.Age.ToString()),
                        CreateTableCell(resident.GetResidentGender()),
                        CreateTableCell(resident.GetHouseholdNumber().ToString()),
                        CreateTableCell(resident.GetPurok())
                    );
                    table.AppendChild(dataRow);
                }

                body.Append(table);
                mainPart.Document.Save();
            }

            await DownloadFileFromStream(fileName, file, JS);
        }

        public async void GenerateSeniorPurokResident(string? purok, List<ResidentModel> residents, IJSRuntime JS)
        {
            string fileName = $"Purok {purok} Senior Citizens.docx"; // Name of the generated file
            string file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", fileName); // Path within wwwroot)

            if (File.Exists(file))
            {
                File.Delete(file);
            }

            using (var document = WordprocessingDocument.Create(file, WordprocessingDocumentType.Document))
            {
                // Add a main document part
                MainDocumentPart mainPart = document.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());

                // Add a header part
                HeaderPart headerPart = mainPart.AddNewPart<HeaderPart>();

                string headerMarkup = $@"<w:hdr xmlns:w='http://schemas.openxmlformats.org/wordprocessingml/2006/main'>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/> <!-- Center align -->
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='48'/> <!-- Enlarge font size -->
                                            <w:b/> <!-- Bold -->
                                        </w:rPr>
                                        <w:t>Baranggay Health Profiling</w:t>
                                    </w:r>
                                </w:p>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/>
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='36'/> <!-- Enlarge font size -->
                                            <w:b/>
                                        </w:rPr>
                                        <w:t>Municipality of Midsayap Barangay Central Bulanan</w:t>
                                    </w:r>
                                </w:p>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/>
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='36'/> <!-- Enlarge font size -->
                                            <w:b/>
                                        </w:rPr>
                                        <w:t>Purok {purok} Senior Citizens Masterlist</w:t>
                                    </w:r>
                                </w:p>
                            </w:hdr>";

                using (StreamWriter streamWriter = new StreamWriter(headerPart.GetStream(FileMode.Create)))
                {
                    streamWriter.Write(headerMarkup);
                }

                SectionProperties sectionProperties = new SectionProperties();
                HeaderReference headerReference = new HeaderReference() { Type = HeaderFooterValues.Default, Id = mainPart.GetIdOfPart(headerPart) };
                sectionProperties.Append(headerReference);
                if (mainPart.Document.Body != null)
                {
                    mainPart.Document.Body.Append(sectionProperties);
                }

                Table table = new Table();
                TableProperties props = new TableProperties(
                    new TableWidth { Width = "100%", Type = TableWidthUnitValues.Pct },
                    new TableBorders(
                        new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 }
                    )
                );
                table.AppendChild<TableProperties>(props);

                TableRow headerRow = new TableRow();
                headerRow.Append(
                    CreateHeaderCell("ID"),
                    CreateHeaderCell("Full Name"),
                    CreateHeaderCell("Age"),
                    CreateHeaderCell("Gender"),
                    CreateHeaderCell("Household Number"),
                    CreateHeaderCell("Purok")
                );
                table.AppendChild(headerRow);

                // Populate table with resident data
                Console.WriteLine(residents.Count);
                foreach (var resident in residents)
                {
                    TableRow dataRow = new TableRow();
                    dataRow.Append(
                        CreateTableCell(resident.GetIDNum().ToString()),
                        CreateTableCell($"{resident.GetResidentFirstName()} {resident.GetResidentMiddleName()} {resident.GetResidentLastName()} {resident.GetResidentSuffix()}"),
                        CreateTableCell(resident.Age.ToString()),
                        CreateTableCell(resident.GetResidentGender()),
                        CreateTableCell(resident.GetHouseholdNumber().ToString()),
                        CreateTableCell(resident.GetPurok())
                    );
                    table.AppendChild(dataRow);
                }

                // Append the table to the document body and save the document
                body.Append(table);
                mainPart.Document.Save();

            }
            await DownloadFileFromStream(fileName, file, JS);
        }

        public async void GeneratePrenatePurokResident(string? purok, List<ResidentModel> residents, IJSRuntime JS)
        {
            string fileName = $"Purok {purok} Prenate.docx"; // Name of the generated file
            string file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", fileName); // Path within wwwroot)

            if (File.Exists(file))
            {
                File.Delete(file);
            }

            using (var document = WordprocessingDocument.Create(file, WordprocessingDocumentType.Document))
            {
                // Add a main document part
                MainDocumentPart mainPart = document.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());

                // Add a header part
                HeaderPart headerPart = mainPart.AddNewPart<HeaderPart>();

                string headerMarkup = $@"<w:hdr xmlns:w='http://schemas.openxmlformats.org/wordprocessingml/2006/main'>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/> <!-- Center align -->
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='48'/> <!-- Enlarge font size -->
                                            <w:b/> <!-- Bold -->
                                        </w:rPr>
                                        <w:t>Baranggay Health Profiling</w:t>
                                    </w:r>
                                </w:p>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/>
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='36'/> <!-- Enlarge font size -->
                                            <w:b/>
                                        </w:rPr>
                                        <w:t>Municipality of Midsayap Barangay Central Bulanan</w:t>
                                    </w:r>
                                </w:p>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/>
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='36'/> <!-- Enlarge font size -->
                                            <w:b/>
                                        </w:rPr>
                                        <w:t>Purok {purok} Prenate Masterlist</w:t>
                                    </w:r>
                                </w:p>
                            </w:hdr>";

                using (StreamWriter streamWriter = new StreamWriter(headerPart.GetStream(FileMode.Create)))
                {
                    streamWriter.Write(headerMarkup);
                }

                SectionProperties sectionProperties = new SectionProperties();
                HeaderReference headerReference = new HeaderReference() { Type = HeaderFooterValues.Default, Id = mainPart.GetIdOfPart(headerPart) };
                sectionProperties.Append(headerReference);
                if (mainPart.Document.Body != null)
                {
                    mainPart.Document.Body.Append(sectionProperties);
                }

                Table table = new Table();
                TableProperties props = new TableProperties(
                    new TableWidth { Width = "100%", Type = TableWidthUnitValues.Pct },
                    new TableBorders(
                        new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 }
                    )
                );
                table.AppendChild<TableProperties>(props);

                TableRow headerRow = new TableRow();
                headerRow.Append(
                    CreateHeaderCell("ID"),
                    CreateHeaderCell("Full Name"),
                    CreateHeaderCell("Age"),
                    CreateHeaderCell("Date of Birth"),
                    CreateHeaderCell("Household Number"),
                    CreateHeaderCell("Purok")
                );
                table.AppendChild(headerRow);

                // Populate table with resident data
                Console.WriteLine(residents.Count);
                foreach (var resident in residents)
                {
                    TableRow dataRow = new TableRow();
                    dataRow.Append(
                        CreateTableCell(resident.GetIDNum().ToString()),
                        CreateTableCell($"{resident.GetResidentFirstName()} {resident.GetResidentMiddleName()} {resident.GetResidentLastName()} {resident.GetResidentSuffix()}"),
                        CreateTableCell(resident.Age.ToString()),
                        CreateTableCell(resident.GetResidentDOB()),
                        CreateTableCell(resident.GetHouseholdNumber().ToString()),
                        CreateTableCell(resident.GetPurok())
                    );
                    table.AppendChild(dataRow);
                }

                // Append the table to the document body and save the document
                body.Append(table);
                mainPart.Document.Save();

            }
            await DownloadFileFromStream(fileName, file, JS);
        }

        public async void GeneratePWDPurokResident(string? purok, List<ResidentModel> residents, IJSRuntime JS)
        {
            string fileName = $"Purok {purok} PWD.docx"; // Name of the generated file
            string file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", fileName); // Path within wwwroot)

            if (File.Exists(file))
            {
                File.Delete(file);
            }

            using (var document = WordprocessingDocument.Create(file, WordprocessingDocumentType.Document))
            {
                // Add a main document part
                MainDocumentPart mainPart = document.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());

                // Add a header part
                HeaderPart headerPart = mainPart.AddNewPart<HeaderPart>();

                string headerMarkup = $@"<w:hdr xmlns:w='http://schemas.openxmlformats.org/wordprocessingml/2006/main'>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/> <!-- Center align -->
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='48'/> <!-- Enlarge font size -->
                                            <w:b/> <!-- Bold -->
                                        </w:rPr>
                                        <w:t>Baranggay Health Profiling</w:t>
                                    </w:r>
                                </w:p>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/>
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='36'/> <!-- Enlarge font size -->
                                            <w:b/>
                                        </w:rPr>
                                        <w:t>Municipality of Midsayap Barangay Central Bulanan</w:t>
                                    </w:r>
                                </w:p>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/>
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='36'/> <!-- Enlarge font size -->
                                            <w:b/>
                                        </w:rPr>
                                        <w:t>Purok {purok} PWD Masterlist</w:t>
                                    </w:r>
                                </w:p>
                            </w:hdr>";

                using (StreamWriter streamWriter = new StreamWriter(headerPart.GetStream(FileMode.Create)))
                {
                    streamWriter.Write(headerMarkup);
                }

                SectionProperties sectionProperties = new SectionProperties();
                HeaderReference headerReference = new HeaderReference() { Type = HeaderFooterValues.Default, Id = mainPart.GetIdOfPart(headerPart) };
                sectionProperties.Append(headerReference);
                if (mainPart.Document.Body != null)
                {
                    mainPart.Document.Body.Append(sectionProperties);
                }

                Table table = new Table();
                TableProperties props = new TableProperties(
                    new TableWidth { Width = "100%", Type = TableWidthUnitValues.Pct },
                    new TableBorders(
                        new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 }
                    )
                );
                table.AppendChild<TableProperties>(props);

                TableRow headerRow = new TableRow();
                headerRow.Append(
                    CreateHeaderCell("ID"),
                    CreateHeaderCell("Full Name"),
                    CreateHeaderCell("Age"),
                    CreateHeaderCell("Gender"),
                    CreateHeaderCell("Household Number"),
                    CreateHeaderCell("Purok")
                );
                table.AppendChild(headerRow);

                // Populate table with resident data
                Console.WriteLine(residents.Count);
                foreach (var resident in residents)
                {
                    TableRow dataRow = new TableRow();
                    dataRow.Append(
                        CreateTableCell(resident.GetIDNum().ToString()),
                        CreateTableCell($"{resident.GetResidentFirstName()} {resident.GetResidentMiddleName()} {resident.GetResidentLastName()} {resident.GetResidentSuffix()}"),
                        CreateTableCell(resident.Age.ToString()),
                        CreateTableCell(resident.GetResidentGender()),
                        CreateTableCell(resident.GetHouseholdNumber().ToString()),
                        CreateTableCell(resident.GetPurok())
                    );
                    table.AppendChild(dataRow);
                }

                // Append the table to the document body and save the document
                body.Append(table);
                mainPart.Document.Save();

            }
            await DownloadFileFromStream(fileName, file, JS);
        }

        public async void GenerateResidentWithTitle(string title, List<ResidentModel> residents, IJSRuntime JS)
        {
            string fileName = $"{title} List.docx"; // Name of the generated file
            string file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", fileName); // Path within wwwroot)

            if (File.Exists(file))
            {
                File.Delete(file);
            }

            using (var document = WordprocessingDocument.Create(file, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = document.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());
                HeaderPart headerPart = mainPart.AddNewPart<HeaderPart>();

                string headerMarkup = $@"<w:hdr xmlns:w='http://schemas.openxmlformats.org/wordprocessingml/2006/main'>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/> <!-- Center align -->
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='48'/> <!-- Enlarge font size -->
                                            <w:b/> <!-- Bold -->
                                        </w:rPr>
                                        <w:t>Baranggay Health Profiling</w:t>
                                    </w:r>
                                </w:p>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/>
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='36'/> <!-- Enlarge font size -->
                                            <w:b/>
                                        </w:rPr>
                                        <w:t>Municipality of Midsayap Barangay Central Bulanan</w:t>
                                    </w:r>
                                </w:p>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/>
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='36'/> <!-- Enlarge font size -->
                                            <w:b/>
                                        </w:rPr>
                                        <w:t>{title} Masterlist</w:t>
                                    </w:r>
                                </w:p>
                            </w:hdr>";

                using (StreamWriter streamWriter = new StreamWriter(headerPart.GetStream(FileMode.Create)))
                {
                    streamWriter.Write(headerMarkup);
                }

                SectionProperties sectionProperties = new SectionProperties();
                HeaderReference headerReference = new HeaderReference() { Type = HeaderFooterValues.Default, Id = mainPart.GetIdOfPart(headerPart) };
                sectionProperties.Append(headerReference);
                if (mainPart.Document.Body != null)
                {
                    mainPart.Document.Body.Append(sectionProperties);
                }

                Table table = new Table();
                TableProperties props = new TableProperties(
                    new TableWidth { Width = "100%", Type = TableWidthUnitValues.Pct },
                    new TableBorders(
                        new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 }
                    )
                );
                table.AppendChild<TableProperties>(props);

                TableRow headerRow = new TableRow();
                headerRow.Append(
                    CreateHeaderCell("ID"),
                    CreateHeaderCell("Full Name"),
                    CreateHeaderCell("Age"),
                    CreateHeaderCell("Gender"),
                    CreateHeaderCell("Household Number"),
                    CreateHeaderCell("Purok")
                );
                table.AppendChild(headerRow);

                Console.WriteLine(residents.Count);
                foreach (var resident in residents)
                {
                    TableRow dataRow = new TableRow();
                    dataRow.Append(
                        CreateTableCell(resident.GetID().ToString()),
                        CreateTableCell($"{resident.GetResidentFirstName()} {resident.GetResidentMiddleName()} {resident.GetResidentLastName()} {resident.GetResidentSuffix()}"),
                        CreateTableCell(resident.Age.ToString()),
                        CreateTableCell(resident.GetResidentGender()),
                        CreateTableCell(resident.GetHouseholdNumber().ToString()),
                        CreateTableCell(resident.GetPurok())
                    );
                    table.AppendChild(dataRow);
                }

                body.Append(table);
                mainPart.Document.Save();

            }
            await DownloadFileFromStream(fileName, file, JS);
        }

        public async void GeneratePurokResident(string? purok, string title, List<ResidentModel> residents, IJSRuntime JS)
        {
            string fileName = $"Purok {purok} {title}.docx"; // Name of the generated file
            string file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", fileName); // Path within wwwroot)

            if (File.Exists(file))
            {
                File.Delete(file);
            }

            using (var document = WordprocessingDocument.Create(file, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = document.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());
                HeaderPart headerPart = mainPart.AddNewPart<HeaderPart>();

                string headerMarkup = $@"<w:hdr xmlns:w='http://schemas.openxmlformats.org/wordprocessingml/2006/main'>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/> <!-- Center align -->
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='48'/> <!-- Enlarge font size -->
                                            <w:b/> <!-- Bold -->
                                        </w:rPr>
                                        <w:t>Baranggay Health Profiling</w:t>
                                    </w:r>
                                </w:p>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/>
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='36'/> <!-- Enlarge font size -->
                                            <w:b/>
                                        </w:rPr>
                                        <w:t>Municipality of Midsayap Barangay Central Bulanan</w:t>
                                    </w:r>
                                </w:p>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/>
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='36'/> <!-- Enlarge font size -->
                                            <w:b/>
                                        </w:rPr>
                                        <w:t>Purok {purok} {title} Masterlist</w:t>
                                    </w:r>
                                </w:p>
                            </w:hdr>";

                using (StreamWriter streamWriter = new StreamWriter(headerPart.GetStream(FileMode.Create)))
                {
                    streamWriter.Write(headerMarkup);
                }

                SectionProperties sectionProperties = new SectionProperties();
                HeaderReference headerReference = new HeaderReference() { Type = HeaderFooterValues.Default, Id = mainPart.GetIdOfPart(headerPart) };
                sectionProperties.Append(headerReference);
                if (mainPart.Document.Body != null)
                {
                    mainPart.Document.Body.Append(sectionProperties);
                }

                Table table = new Table();
                TableProperties props = new TableProperties(
                    new TableWidth { Width = "100%", Type = TableWidthUnitValues.Pct },
                    new TableBorders(
                        new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 }
                    )
                );
                table.AppendChild<TableProperties>(props);

                TableRow headerRow = new TableRow();
                headerRow.Append(
                    CreateHeaderCell("ID"),
                    CreateHeaderCell("Full Name"),
                    CreateHeaderCell("Age"),
                    CreateHeaderCell("Gender"),
                    CreateHeaderCell("Household Number"),
                    CreateHeaderCell("Purok")
                );
                table.AppendChild(headerRow);

                Console.WriteLine(residents.Count);
                foreach (var resident in residents)
                {
                    TableRow dataRow = new TableRow();
                    dataRow.Append(
                        CreateTableCell(resident.GetID().ToString()),
                        CreateTableCell($"{resident.GetResidentFirstName()} {resident.GetResidentMiddleName()} {resident.GetResidentLastName()} {resident.GetResidentSuffix()}"),
                        CreateTableCell(resident.Age.ToString()),
                        CreateTableCell(resident.GetResidentGender()),
                        CreateTableCell(resident.GetHouseholdNumber().ToString()),
                        CreateTableCell(resident.GetPurok())
                    );
                    table.AppendChild(dataRow);
                }

                body.Append(table);
                mainPart.Document.Save();

            }
                await DownloadFileFromStream(fileName, file, JS);
        }


        public async void GenerateHousehold(List<HouseholdModel> households, IJSRuntime JS)
        {
            string fileName = "Households.docx"; // Name of the generated file
            string file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", fileName); // Path within wwwroot)

            if (File.Exists(file))
            {
                File.Delete(file);
            }

            using (var document = WordprocessingDocument.Create(file, WordprocessingDocumentType.Document))
            {
                // Add a main document part
                MainDocumentPart mainPart = document.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());

                // Add a header part
                HeaderPart headerPart = mainPart.AddNewPart<HeaderPart>();

                string headerMarkup = @"<w:hdr xmlns:w='http://schemas.openxmlformats.org/wordprocessingml/2006/main'>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/> <!-- Center align -->
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='48'/> <!-- Enlarge font size -->
                                            <w:b/> <!-- Bold -->
                                        </w:rPr>
                                        <w:t>Baranggay Health Profiling</w:t>
                                    </w:r>
                                </w:p>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/>
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='36'/> <!-- Enlarge font size -->
                                            <w:b/>
                                        </w:rPr>
                                        <w:t>Municipality of Midsayap Barangay Central Bulanan</w:t>
                                    </w:r>
                                </w:p>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/>
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='36'/> <!-- Enlarge font size -->
                                            <w:b/>
                                        </w:rPr>
                                        <w:t>Households Masterlist</w:t>
                                    </w:r>
                                </w:p>
                            </w:hdr>";

                using (StreamWriter streamWriter = new StreamWriter(headerPart.GetStream(FileMode.Create)))
                {
                    streamWriter.Write(headerMarkup);
                }

                // Set the reference to the header
                SectionProperties sectionProperties = new SectionProperties();
                HeaderReference headerReference = new HeaderReference() { Type = HeaderFooterValues.Default, Id = mainPart.GetIdOfPart(headerPart) };
                sectionProperties.Append(headerReference);
                if (mainPart.Document.Body != null)
                {
                    mainPart.Document.Body.Append(sectionProperties);
                }

                // Create the table and its properties
                Table table = new Table();
                TableProperties props = new TableProperties(
                    new TableWidth { Width = "100%", Type = TableWidthUnitValues.Pct },
                    new TableBorders(
                        new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 }
                    )
                );
                table.AppendChild<TableProperties>(props);

                // Table headers
                TableRow headerRow = new TableRow();
                headerRow.Append(
                    CreateHeaderCell("ID"),
                    CreateHeaderCell("Father's Name"),
                    CreateHeaderCell("Mother's Name"),
                    CreateHeaderCell("Member"),
                    CreateHeaderCell("Family Count")
                );
                table.AppendChild(headerRow);
                foreach (var household in households)
                {
                    TableRow dataRow = new TableRow();
                    dataRow.Append(
                        CreateTableCell(household.GetID().ToString()),
                        CreateTableCell(household.GetFathersName()),
                        CreateTableCell(household.GetMothersName()),
                        CreateTableCell(household.GetMember()),
                        CreateTableCell(household.GetFamilyCount().ToString())
                    );
                    table.AppendChild(dataRow);
                }

                // Append the table to the document body and save the document
                body.Append(table);
                mainPart.Document.Save();

            }
                await DownloadFileFromStream(fileName, file, JS);
        }

        public async void GeneratePurokHousehold(string? purok, List<HouseholdModel> households, IJSRuntime JS)
        {
            string fileName = $"Purok {purok} Households.docx"; // Name of the generated file
            string file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", fileName); // Path within wwwroot)

            if (File.Exists(file))
            {
                File.Delete(file);
            }

            using (var document = WordprocessingDocument.Create(file, WordprocessingDocumentType.Document))
            {
                // Add a main document part
                MainDocumentPart mainPart = document.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());

                // Add a header part
                HeaderPart headerPart = mainPart.AddNewPart<HeaderPart>();

                string headerMarkup = $@"<w:hdr xmlns:w='http://schemas.openxmlformats.org/wordprocessingml/2006/main'>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/> <!-- Center align -->
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='48'/> <!-- Enlarge font size -->
                                            <w:b/> <!-- Bold -->
                                        </w:rPr>
                                        <w:t>Baranggay Health Profiling</w:t>
                                    </w:r>
                                </w:p>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/>
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='36'/> <!-- Enlarge font size -->
                                            <w:b/>
                                        </w:rPr>
                                        <w:t>Municipality of Midsayap Barangay Central Bulanan</w:t>
                                    </w:r>
                                </w:p>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/>
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='36'/> <!-- Enlarge font size -->
                                            <w:b/>
                                        </w:rPr>
                                        <w:t>Purok {purok} Households Masterlist</w:t>
                                    </w:r>
                                </w:p>
                            </w:hdr>";

                using (StreamWriter streamWriter = new StreamWriter(headerPart.GetStream(FileMode.Create)))
                {
                    streamWriter.Write(headerMarkup);
                }

                // Set the reference to the header
                SectionProperties sectionProperties = new SectionProperties();
                HeaderReference headerReference = new HeaderReference() { Type = HeaderFooterValues.Default, Id = mainPart.GetIdOfPart(headerPart) };
                sectionProperties.Append(headerReference);
                if (mainPart.Document.Body != null)
                {
                    mainPart.Document.Body.Append(sectionProperties);
                }

                Table table = new Table();
                TableProperties props = new TableProperties(
                    new TableWidth { Width = "100%", Type = TableWidthUnitValues.Pct },
                    new TableBorders(
                        new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 }
                    )
                );
                table.AppendChild<TableProperties>(props);

                TableRow headerRow = new TableRow();
                headerRow.Append(
                    CreateHeaderCell("ID"),
                    CreateHeaderCell("Father's Name"),
                    CreateHeaderCell("Mother's Name"),
                    CreateHeaderCell("Member"),
                    CreateHeaderCell("Family Count")
                );
                table.AppendChild(headerRow);
                foreach (var household in households)
                {
                    TableRow dataRow = new TableRow();
                    dataRow.Append(
                        CreateTableCell(household.GetID().ToString()),
                        CreateTableCell(household.GetFathersName()),
                        CreateTableCell(household.GetMothersName()),
                        CreateTableCell(household.GetMember()),
                        CreateTableCell(household.GetFamilyCount().ToString())
                    );
                    table.AppendChild(dataRow);
                }

                body.Append(table);
                mainPart.Document.Save();

            }
                await DownloadFileFromStream(fileName, file, JS);
        }

        public async void GenerateRHS(List<ResidentHealthStatusModel> rhs, List<IllnessModel> illnesses, List<ResidentModel> residents, IJSRuntime JS)
        {
            string fileName = "ResidentHealthStatusList.docx"; 
            string file = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Files", fileName); 

            if (File.Exists(file))
            {
                File.Delete(file);
            }

            using (var document = WordprocessingDocument.Create(file, WordprocessingDocumentType.Document))
            {
                MainDocumentPart mainPart = document.AddMainDocumentPart();
                mainPart.Document = new Document();
                Body body = mainPart.Document.AppendChild(new Body());

                HeaderPart headerPart = mainPart.AddNewPart<HeaderPart>();

                string headerMarkup = @"<w:hdr xmlns:w='http://schemas.openxmlformats.org/wordprocessingml/2006/main'>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/> <!-- Center align -->
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='48'/> <!-- Enlarge font size -->
                                            <w:b/> <!-- Bold -->
                                        </w:rPr>
                                        <w:t>Baranggay Health Profiling</w:t>
                                    </w:r>
                                </w:p>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/>
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='36'/> <!-- Enlarge font size -->
                                            <w:b/>
                                        </w:rPr>
                                        <w:t>Municipality of Midsayap Barangay Central Bulanan</w:t>
                                    </w:r>
                                </w:p>
                                <w:p>
                                    <w:pPr>
                                        <w:jc w:val='center'/>
                                    </w:pPr>
                                    <w:r>
                                        <w:rPr>
                                            <w:sz w:val='36'/> <!-- Enlarge font size -->
                                            <w:b/>
                                        </w:rPr>
                                        <w:t>Resident Health Status list</w:t>
                                    </w:r>
                                </w:p>
                            </w:hdr>";

                using (StreamWriter streamWriter = new StreamWriter(headerPart.GetStream(FileMode.Create)))
                {
                    streamWriter.Write(headerMarkup);
                }

                // Set the reference to the header
                SectionProperties sectionProperties = new SectionProperties();
                HeaderReference headerReference = new HeaderReference() { Type = HeaderFooterValues.Default, Id = mainPart.GetIdOfPart(headerPart) };
                sectionProperties.Append(headerReference);
                if (mainPart.Document.Body != null)
                {
                    mainPart.Document.Body.Append(sectionProperties);
                }

                // Create the table and its properties
                Table table = new Table();
                TableProperties props = new TableProperties(
                    new TableWidth { Width = "100%", Type = TableWidthUnitValues.Pct },
                    new TableBorders(
                        new TopBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new BottomBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new LeftBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new RightBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new InsideHorizontalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 },
                        new InsideVerticalBorder { Val = new EnumValue<BorderValues>(BorderValues.Single), Size = 5 }
                    )
                );
                table.AppendChild<TableProperties>(props);

                // Table headers
                TableRow headerRow = new TableRow();
                headerRow.Append(
                    CreateHeaderCell("ID"),
                    CreateHeaderCell("Resident Name"),
                    CreateHeaderCell("Birthday"),
                    CreateHeaderCell("Age"),
                    CreateHeaderCell("Gender"),
                    CreateHeaderCell("Purok"),
                    CreateHeaderCell("Diagnosed Date")
                );
                table.AppendChild(headerRow);

                // Populate table with resident data
                Console.WriteLine(rhs.Count);
                foreach (ResidentHealthStatusModel rh in rhs)
                {
                    ResidentModel? resident = residents.FirstOrDefault(r => r.GetID() == rh.ResidentId);

                    if (resident != null)
                    {
                        Console.WriteLine("Illness and resident found");
                        Console.WriteLine($"Resident: {resident.GetResidentFirstName()} {resident.GetResidentLastName()}");

                        TableRow dataRow = new TableRow();
                        dataRow.Append(
                            CreateTableCell(rh.GetID().ToString()),
                            CreateTableCell($"{resident.GetResidentFirstName()} {resident.GetResidentMiddleName()} {resident.GetResidentLastName()} {resident.GetResidentSuffix()}"),
                            CreateTableCell(resident.GetResidentDOB()),
                            CreateTableCell(resident.GetResidentAge().ToString()),
                            CreateTableCell(resident.GetResidentGender()),
                            CreateTableCell(resident.GetPurok()),
                            CreateTableCell(rh.GetDiagnosedDate())
                        );
                        table.AppendChild(dataRow);
                    }
                    else
                    {
                        if (resident == null)
                        {
                            Console.WriteLine($"Resident with ID {rh.ResidentId} not found");
                        }
                    }
                }

                // Append the table to the document body and save the document
                body.Append(table);
                mainPart.Document.Save();

            }
                await DownloadFileFromStream(fileName, file, JS);
        }

        private TableCell CreateHeaderCell(string text)
        {
            return new TableCell(new Paragraph(new Run(new Text(text))))
            {
                TableCellProperties = new TableCellProperties(
                    new TableCellWidth { Type = TableWidthUnitValues.Auto })
            };
        }

        private TableCell CreateTableCell(string text)
        {
            return new TableCell(new Paragraph(new Run(new Text(text))));
        }


        public int GetPurokHouseholdCount(string purok)
        {
            using (var connection = _sqlConnector.GetConnection())
            {
                string query = @"
                    SELECT COUNT(*)
                    FROM household h
                    INNER JOIN resident r ON h.MemberID = r.ID
                    LEFT JOIN Archive a ON r.ID = a.ReferenceID AND a.Type = 'household'
                    WHERE r.Purok = @Purok
                    AND a.ReferenceID IS NULL"; 
                return connection.ExecuteScalar<int>(query, new { Purok = purok });
            }
        }

        public int GetPurokSeniorCitizenCount(string purok)
        {
            using (var connection = _sqlConnector.GetConnection())
            {
                string query = @"
                    SELECT COUNT(*)
                    FROM resident r
                    LEFT JOIN Archive a ON r.ID = a.ReferenceID AND a.Type = 'Resident'
                    WHERE r.Purok = @Purok 
                    AND r.Age >= 60
                    AND a.ReferenceID IS NULL"; 

                return connection.ExecuteScalar<int>(query, new { Purok = purok });
            }

        }

        public List<HouseholdModel> GetPurokHouseholds(string purok)
        {
            using (var connection = _sqlConnector.GetConnection())
            {
                string query = "SELECT * FROM household h INNER JOIN resident r ON h.MemberID = r.ID WHERE r.Purok = @Purok";
                return connection.Query<HouseholdModel>(query, new { Purok = purok }).ToList();
            }
        }

        public List<ResidentModel> GetPurokResidents(string purok)
        {
            using (var connection = _sqlConnector.GetConnection())
            {
                string query = "SELECT * FROM resident WHERE Purok = @Purok";
                return connection.Query<ResidentModel>(query, new { Purok = purok }).ToList();
            }
        }

        private Stream GetFileStream(string filePath)
        {
            return File.OpenRead($@"{filePath}");

        }

        private async Task DownloadFileFromStream(string fileName, string filePath, IJSRuntime JS)
        {
            var fileStream = GetFileStream(filePath);
            using var streamRef = new DotNetStreamReference(stream: fileStream);

            await JS.InvokeVoidAsync("downloadFileFromStream", fileName, streamRef);
            File.Delete(filePath);
        }


        public string GetIllnessNameByID(int ID)
        {
            string? name = "";
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    const string query = "SELECT IllnessName FROM illness WHERE ID = @ID";
                    name = connection.QuerySingleOrDefault<string>(query, new { ID = ID });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "ERROR";
            }
            string temp = (name != null) ? name : "";
            return temp;
        }

        public bool isIllnessNone(ResidentHealthStatusModel rhs)
        {
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    const string query = "SELECT COUNT(*) FROM rhs WHERE TypeofIllnessID = 0 AND ID = @ID";
                    int count = connection.ExecuteScalar<int>(query, new { ID = rhs.GetID() });
                    return (count > 0);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public List<MedicineReceiptModel> GetMedicineReceipt()
        {
            using (MySqlConnection connection = _sqlConnector.GetConnection())
            {
                var receipts = connection.Query<MedicineReceiptModel>("SELECT * FROM receipt").ToList();
                return receipts;
            }
        }

        public void AddReceipt(MedicineReceiptModel receipt)
        {
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    const string query = @"
                            INSERT INTO receipt (medicineId, residentId, quantity)
                            VALUES (@medicineId, @residentId, @quantity);

                            UPDATE medicine
                            SET Stock = Stock - @quantity
                            WHERE id = @medicineId;";

                    connection.Execute(query, new
                    {
                        medicineId = receipt.GetMedicineID(),
                        residentId = receipt.GetResidentID(),
                        quantity = receipt.GetQuantity()
                    });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public List<ResidentStatuses> GetStatuses()
        {
            using (MySqlConnection connection = _sqlConnector.GetConnection())
            {
                var statuses = connection.Query<ResidentStatuses>("SELECT * FROM resident_statuses").ToList();
                return statuses;
            }
        }

        public List<Status> GetAllStatuses()
        {
            using (MySqlConnection connection = _sqlConnector.GetConnection())
            {
                var statuses = connection.Query<Status>("SELECT * FROM status").ToList();
                return statuses;
            }
        }

        public List<ResidentModel> GetResidentsByStatus(int? id)
        {
            using (MySqlConnection connection = _sqlConnector.GetConnection())
            {
                var query = @"
                    SELECT r.* 
                    FROM resident r
                    JOIN resident_statuses ON r.ID = resident_statuses.residentId
                    WHERE resident_statuses.statusId = @ID";
                var res = connection.Query<ResidentModel>(query, new { id = id }).ToList();
                Console.WriteLine("Fetching Medicine Data");
                return res;
            }
        }

        public string GetStatusNameByID(int id)
        {
            string? name = "";
            try
            {
                using (var connection = _sqlConnector.GetConnection())
                {
                    const string query = "SELECT name FROM status WHERE ID = @ID";
                    name = connection.QuerySingleOrDefault<string>(query, new { ID = id });
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "ERROR";
            }
            string temp = (name != null) ? name : "";
            return temp;
        }
        
        public void AddStatus(String name)
        {

            using (MySqlConnection connection = _sqlConnector.GetConnection())
            {
                connection.Execute("INSERT INTO status (name) VALUES (@name);", new { name = name });
            }
        }

        public int GetPWDCount(string purok)
        {
            using (var connection = _sqlConnector.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM Resident WHERE isPWD = 1 AND purok = @purok";

                try
                {
                    int residentCount = connection.QuerySingle<int>(query, new { purok = purok });
                    return residentCount;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting pwd count: {ex.Message}");
                    return -1;
                }
            }
        }

        public int GetPrenateCount(string purok)
        {
            using (var connection = _sqlConnector.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM Resident WHERE isPrenate = 1 AND purok = @purok";

                try
                {
                    int residentCount = connection.QuerySingle<int>(query, new { purok = purok });
                    return residentCount;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting prenate count: {ex.Message}");
                    return -1;
                }
            }
        }

        public List<ResidentModel> GetPWDByPurok(string purok)
        {
            using (MySqlConnection connection = _sqlConnector.GetConnection())
            {
                var residents = connection.Query<ResidentModel>("SELECT * FROM resident WHERE isPWD = 1 AND purok = @purok", new { purok = purok }).ToList();
                Console.WriteLine("Fetching Resident Data");
                return residents;
            }
        }

        public List<ResidentModel> GetPrenateByPurok(string purok)
        {
            using (MySqlConnection connection = _sqlConnector.GetConnection())
            {
                var residents = connection.Query<ResidentModel>("SELECT * FROM resident WHERE isPrenate = 1 AND purok = @purok", new { purok = purok }).ToList();
                Console.WriteLine("Fetching Resident Data");
                return residents;
            }
        }

        public int GetIllnessCountByUserID(int id)
        {
            using(MySqlConnection connection = _sqlConnector.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM resident_illnesses WHERE residentId = @id";
                try
                {
                    int illnessCount = connection.QuerySingle<int>(query, new { id = id });
                    return illnessCount;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting illness count: {ex.Message}");
                    return -1;
                }
            }
        }

        public List<ResidentIllnesses> GetIllnessesByID(int id)
        {
            using(MySqlConnection connection = _sqlConnector.GetConnection())
            {
                string query = "SELECT * FROM resident_illnesses WHERE residentId = @id";
                try
                {
                    var illnesses = connection.Query<ResidentIllnesses>(query, new { id = id }).ToList();
                    return illnesses;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting illnesses: {ex.Message}");
                    return null;
                }
            }
        }

        public List<Status> GetResidentStatuses(int id)
        {
            using (MySqlConnection connection = _sqlConnector.GetConnection())
            {
                string query = "SELECT * FROM resident_statuses WHERE residentId = @id";
                try
                {
                    var statuses = connection.Query<Status>(query, new { id = id }).ToList();
                    return statuses;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error getting statuses: {ex.Message}");
                    return null;
                }
            }
        }

        public bool isIll(int residentId)
        {
            using(MySqlConnection connection = _sqlConnector.GetConnection())
            {
                string query = "SELECT COUNT(*) FROM resident_illnesses WHERE residentId = @residentId";
                try
                {
                    var count = connection.QuerySingle<int>(query, new { residentId = residentId });
                    return (count > 0);
                }
                catch(Exception ex)
                {
                    Console.WriteLine($"Error getting illnesses: {ex.Message}");
                    return false;
                }
            }
        }

    }

    
}


