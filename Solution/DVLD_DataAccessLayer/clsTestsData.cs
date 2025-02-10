using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DVLD_DataAccessLayer
{
    public class clsTestsData
    {

        public static DataTable ListTestTypes()
        {
            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT * FROM [dbo].[TestTypes]";


            // COALESCE handle the case when ThirdName = "" or = null

            SqlCommand Command = new SqlCommand(Query, Connection);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {
                    dt.Load(Reader);
                }
                else
                {
                    Console.WriteLine("No rows found (clsTestsData.ListTestsTypes).");
                }

                Reader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

                Connection.Close();
            }

            return dt;
        }


        public static DataTable ListAppoinmentsForLocalDrivingLicenseApplicationID(int LDLAppID, int TestType)
        {
            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select TestAppointmentID, AppointmentDate, PaidFees, IsLocked from TestAppointments
                            where LocalDrivingLicenseApplicationID = @LDLAppID And TestTypeID = @TestType ";


            // COALESCE handle the case when ThirdName = "" or = null

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LDLAppID", LDLAppID);
            Command.Parameters.AddWithValue("@TestType", TestType);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {
                    dt.Load(Reader);
                }
                else
                {
                    Console.WriteLine("No rows found (clsTestsData.ListTestsTypes).");
                }

                Reader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

                Connection.Close();
            }

            return dt;
        }





        public static DataRow FindTestType(int TestTypeID)
        {
            DataTable dt = new DataTable();
            DataRow dr = dt.NewRow();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string query = @"SELECT * FROM [dbo].[TestTypes]
                            Where TestTypeID = @TestTypeID";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                Connection.Open();

                SqlDataReader reader = Command.ExecuteReader();

                if (reader.HasRows)
                {
                    dt.Load(reader);

                    if (dt.Rows.Count > 0)
                    {
                        dr = dt.Rows[0];
                    }
                    else
                    {
                        dr = null;
                        Console.WriteLine("dt.Rows.Count > 0 Is False (clsTestsData.FindTestType )");
                    }
                }
                else
                {
                    dr = null;
                    Console.WriteLine("Reader.HasRows Is False (clsTestsData.FindTestType  )");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return dr;
        }


        public static bool UpdateTestType(int TestTypeID, string TestTypeTitle, string TestTypeDescription, decimal TestTypeFees)
        {

            int RowsAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string Query = @"UPDATE [dbo].[TestTypes]
                               SET [TestTypeTitle] = @TestTypeTitle
                                  ,[TestTypeDescription] = @TestTypeDescription
                                  , [TestTypeFees] = @TestTypeFees
                             WHERE TestTypeID = @TestTypeID ";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue(@"TestTypeID", TestTypeID);
            Command.Parameters.AddWithValue(@"TestTypeTitle", TestTypeTitle);
            Command.Parameters.AddWithValue(@"TestTypeDescription", TestTypeDescription);
            Command.Parameters.AddWithValue(@"TestTypeFees", TestTypeFees);

            try
            {
                Connection.Open();
                RowsAffected = Command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

                Connection.Close();
            }

            return RowsAffected > 0;

        }


        public static int NumberOfPassedTests(int LocalDrivingLicenseApplicationID)
        {
            int PassedTests = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string query = @"SELECT 
                                   (SELECT COUNT(*)
                                    FROM   Tests t
                                    INNER JOIN TestAppointments ta 
                                            ON t.TestAppointmentID = ta.TestAppointmentID
                                    WHERE  ta.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID 
                                           AND t.TestResult = 1) AS 'Passed Tests'
                            FROM   LocalDrivingLicenseApplications
                                   INNER JOIN Applications
                                           ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                            WHERE  LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                            GROUP  BY LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID;";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    PassedTests = (int)Result;
                }
                else
                {
                    Console.WriteLine($"No Tests Found With For LocalDrivingLicenseApplicationID {LocalDrivingLicenseApplicationID}");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return PassedTests;
        }



        public static int TrialsOfTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            int Trials = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string query = @"SELECT 
                                COUNT(*) AS Trial 
                            FROM 
                                TestAppointments
                            WHERE 
                                LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID
                                AND TestTypeID = @TestTypeID;";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    Trials = (int)Result;
                }
                else
                {
                    Console.WriteLine($"No Trials For LocalDrivingLicenseApplicationID {LocalDrivingLicenseApplicationID}");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return Trials;
        }

        public static int AddTestAppoinment(int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, decimal PaidFees, int CreatedByUserID, bool IsLocked)
        {

            int AppointmentID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"INSERT INTO [dbo].[TestAppointments]
                                   ([TestTypeID]
                                   ,[LocalDrivingLicenseApplicationID]
                                   ,[AppointmentDate]
                                   ,[PaidFees]
                                   ,[CreatedByUserID]
                                   ,[IsLocked])
                             VALUES
                                   (@TestTypeID,
                                   @LocalDrivingLicenseApplicationID, 
                                   @AppointmentDate, 
                                   @PaidFees,
                                   @CreatedByUserID,
                                   @IsLocked);
		                           SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
            Command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@IsLocked", IsLocked);


            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    AppointmentID = Convert.ToInt32(Result);
                }
                else
                {
                    Console.WriteLine("Result Is Null Check DataBase (AddTestAppoinment Function)");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

                Connection.Close();
            }

            return AppointmentID;
        }


        public static bool IsThereIsAnActiveAppointmentForLocalDrivingLicenseApplicationIDAndTestType(int LDLAppID, int TestType)
        {
            bool IsThereIsAnActiveAppoinment = false;


            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select IsThereIsAnActiveAppoinment = 'Yes' from TestAppointments
                            where LocalDrivingLicenseApplicationID = @LDLAppID And TestTypeID = @TestType And IsLocked = 0;";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LDLAppID", LDLAppID);
            Command.Parameters.AddWithValue("@TestType", TestType);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {
                    IsThereIsAnActiveAppoinment = true;
                }
                else
                {
                    IsThereIsAnActiveAppoinment = false;
                }

                Reader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return IsThereIsAnActiveAppoinment;
        }


        public static bool IsLDLAppIDHasLockedTestAppointment(int LDLAppID, int TestType)
        {
            bool IsLDLAppIDHasLockedTestAppointment = false;


            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select IsLDLAppIDHasLockedTestAppointment = 'Yes' from TestAppointments
                            where LocalDrivingLicenseApplicationID = @LDLAppID And TestTypeID = @TestType And IsLocked = @IsLocked;";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LDLAppID", LDLAppID);
            Command.Parameters.AddWithValue("@TestType", TestType);
            Command.Parameters.AddWithValue("@IsLocked", 1);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {
                    IsLDLAppIDHasLockedTestAppointment = true;
                }
                else
                {
                    IsLDLAppIDHasLockedTestAppointment = false;
                }

                Reader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return IsLDLAppIDHasLockedTestAppointment;
        }





        public static bool EditTestAppointment(int AppointmentID, DateTime AppointmentDate)
        {
            int RowsAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"UPDATE [dbo].[TestAppointments]
                               SET 
                                  [AppointmentDate] = @AppointmentDate

                             WHERE TestAppointmentID = @AppointmentID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@AppointmentID", AppointmentID);
            Command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);

            try
            {
                Connection.Open();
                RowsAffected = Command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

                Connection.Close();
            }

            return RowsAffected > 0;

        }


        public static int TakeTest(int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            int TestID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"INSERT INTO [dbo].[Tests]
                                   ([TestAppointmentID]
                                   ,[TestResult]
                                   ,[Notes]
                                   ,[CreatedByUserID])
                             VALUES
                                   (@TestAppointmentID
                                   ,@TestResult
                                   ,@Notes
                                   ,@CreatedByUserID);
		                           SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            Command.Parameters.AddWithValue("@TestResult", TestResult);
            Command.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(Notes) ? (object)DBNull.Value : Notes);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);


            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    TestID = Convert.ToInt32(Result);
                }
                else
                {
                    Console.WriteLine("Result Is Null Check DataBase (TakeTest Function)");
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

                Connection.Close();
            }

            return TestID;
        }



        public static bool LockAppointment(int AppointmentID)
        {
            int RowsAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"UPDATE [dbo].[TestAppointments]
                               SET 
                                  [IsLocked] = @IsLocked

                             WHERE TestAppointmentID = @AppointmentID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@AppointmentID", AppointmentID);
            Command.Parameters.AddWithValue("@IsLocked", 1);

            try
            {
                Connection.Open();
                RowsAffected = Command.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

                Connection.Close();
            }

            return RowsAffected > 0;

        }


        public static bool CheckIfAppointmentIsLocked(int AppointmentID)
        {
            bool IsLocked = false;


            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select IsLocked from TestAppointments
                            where TestAppointmentID = @AppointmentID And IsLocked = @IsLocked";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@AppointmentID", AppointmentID);
            Command.Parameters.AddWithValue("@IsLocked", 1);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {
                    IsLocked = true;
                }
                else
                {
                    IsLocked = false;
                }

                Reader.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return IsLocked;
        }


        public static DateTime GetAppointmentDate(int AppointmentID)
        {

            DateTime dt = DateTime.Now;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select AppointmentDate from TestAppointments 
                            where TestAppointmentID = @AppointmentID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@AppointmentID", AppointmentID);

            try
            {
                Connection.Open();
                object result = Command.ExecuteScalar();

                if (result != null)
                {
                    dt = (DateTime)result;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return dt;

        }



        public static bool IsTestResultPass(int TestID)
        {
            bool IsPass = false;


            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select TestResult from Tests
                            Where TestID = @TestID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@TestID", TestID);

            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    IsPass = (bool)Result;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return IsPass;
        }


        public static string GetTestNotes(int TestID)
        {

            string Notes = "";

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select Notes from Tests
                            Where TestID = @TestID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@TestID", TestID);

            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();

                if (Result != null && Result != DBNull.Value)
                {
                    Notes = (string)Result;
                }
                // If Result is null or DBNull.Value, Notes remains ""


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return Notes;
        }


        public static int GetTestIDUsingTestAppointmentID(int AppointmentID)
        {
            int TestID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select TestID from Tests
                            Where TestAppointmentID = @AppointmentID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@AppointmentID", AppointmentID);

            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    TestID = (int)Result;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return TestID;
        }


        public static int GetTestTypeIDUsingTestAppointmentID(int AppointmentID)
        {
            int TestTypeID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select TestTypeID from TestAppointments 
                            where TestAppointmentID = @AppointmentID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@AppointmentID", AppointmentID);

            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    TestTypeID = (int)Result;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return TestTypeID;
        }


        public static int GetLastLockedTestAppointmentID(int LDLAppID, int TestTypeID)
        {
            int TestAppointmentID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT TOP 1 TestAppointmentID 
                            FROM TestAppointments
                            WHERE LocalDrivingLicenseApplicationID = @LDLAppID
                              AND IsLocked = @IsLocked
                              AND TestTypeID = @TestTypeID
                            ORDER BY TestAppointmentID DESC;";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LDLAppID", LDLAppID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
            Command.Parameters.AddWithValue("@IsLocked", 1);

            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    TestAppointmentID = (int)Result;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return TestAppointmentID;
        }


        public static bool GetTestResultForTestAppointmentID(int TestAppointmentID)
        {
            bool TestResult = false;


            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select TestResult from Tests
                            where TestAppointmentID = @TestAppointmentID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    TestResult = (bool)Result;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return TestResult;
        }


        public static int GetFirstTestAppointmentID(int LDLAppID, int TestTypeID)
        {
            int TestAppointmentID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select top 1 * from TestAppointments
                            where LocalDrivingLicenseApplicationID = @LDLAppID And TestTypeID = @TestTypeID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LDLAppID", LDLAppID);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    TestAppointmentID = (int)Result;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return TestAppointmentID;
        }



        public static decimal GetTestTypeFees(int TestTypeID)
        {


            decimal TestTypeFees = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select TestTypeFees from TestTypes
                            where TestTypeID = @TestTypeID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    TestTypeFees = (decimal)Result;
                }


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return TestTypeFees;

        }


    }
}
