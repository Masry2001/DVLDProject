using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DVLD_DataAccessLayer
{
    public static class clsLicensesData
    {


        public static int IssueDrivingLicense(int ApplicationID, int DriverID, int LicenseClassID, DateTime IssueDate, DateTime ExpirationDate, string Notes, decimal PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        {

            int LicenseID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"INSERT INTO [dbo].[Licenses]
                           ([ApplicationID]
                           ,[DriverID]
                           ,[LicenseClass]
                           ,[IssueDate]
                           ,[ExpirationDate]
                           ,[Notes]
                           ,[PaidFees]
                           ,[IsActive]
                           ,[IssueReason]
                           ,[CreatedByUserID])
                     VALUES
                           (@ApplicationID,
                           @DriverID, 
                           @LicenseClassID, 
                           @IssueDate, 
                           @ExpirationDate, 
                           @Notes, 
                           @PaidFees, 
                           @IsActive, 
                           @IssueReason, 
                           @CreatedByUserID);
		                   SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
            Command.Parameters.AddWithValue("@IssueDate", IssueDate);
            Command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            Command.Parameters.AddWithValue("@Notes", string.IsNullOrEmpty(Notes) ? (object)DBNull.Value : Notes);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@IsActive", IsActive);
            Command.Parameters.AddWithValue("@IssueReason", IssueReason);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);



            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    LicenseID = Convert.ToInt32(Result);
                }
                else
                {
                    Console.WriteLine("Result Is Null Check DataBase (IssueDrivingLicense Function)");
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

            return LicenseID;

        }


        public static DataRow GetLicenseRecordUsingID(int ApplicationID)
        {
            DataTable dt = new DataTable();
            DataRow dr = dt.NewRow();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string query = @"select * from Licenses
                            Where ApplicationID = @ApplicationID";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

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
                        Console.WriteLine("No Rows Found (clsLicensesData.GetApplicationRecordUsingID )");
                    }
                }
                else
                {
                    dr = null;
                    Console.WriteLine("Reader.HasRows Is False (clsLicensesData.GetApplicationRecordUsingID  )");
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


        public static DataRow GetLicenseRecordUsingLicenseID(int LicenseID)
        {
            DataTable dt = new DataTable();
            DataRow dr = dt.NewRow();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string query = @"select * from Licenses
                            Where LicenseID = @LicenseID";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@LicenseID", LicenseID);

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
                        Console.WriteLine("No Rows Found (clsLicensesData.GetLicenseRecordUsingLicenseID )");
                    }
                }
                else
                {
                    dr = null;
                    Console.WriteLine("Reader.HasRows Is False (clsLicensesData.GetLicenseRecordUsingLicenseID  )");
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




        public static DataTable GetDriverLicenses(int DriverID)
        {

            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"SELECT Licenses.LicenseID As 'Lic.ID', Licenses.ApplicationID as 'App.ID', LicenseClasses.ClassName, Licenses.IssueDate,               Licenses.ExpirationDate, Licenses.IsActive
                            FROM Licenses INNER JOIN
                            LicenseClasses ON Licenses.LicenseClass = LicenseClasses.LicenseClassID
				            where DriverID = @DriverID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@DriverID", DriverID);

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
                    Console.WriteLine("No Licenses found.");
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


        public static int GetApplicationIDUsingLicenseID(int LicenseID)
        {

            int ApplicationID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select ApplicationID from Licenses
                            where LicenseID = @LicenseID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);


            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null)
                {
                    ApplicationID = (int)Result;
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

            return ApplicationID;

        }
        public static int GetLicenseIDUsingAppID(int AppID)
        {

            int LicenseID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select LicenseID from Licenses
                            where ApplicationID = @AppID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@AppID", AppID);


            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null)
                {
                    LicenseID = (int)Result;
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

            return LicenseID;

        }

        public static int GetUserIDUsingLicenseID(int LicenseID)
        {

            int UserID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select CreatedByUserID from Licenses
                            where LicenseID = @LicenseID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);


            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null)
                {
                    UserID = (int)Result;
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

            return UserID;

        }


        public static int GetDriverIDUsingLicenseID(int LicenseID)
        {

            int DriverID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select DriverID from Licenses
                            where LicenseID = @LicenseID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);


            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null)
                {
                    DriverID = (int)Result;
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

            return DriverID;

        }


        public static int GetLicenseClassIDUsingLicenseID(int LicenseID)
        {
            int LicenseClassID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select LicenseClass from Licenses
                            where LicenseID = @LicenseID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);


            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();
                if (Result != null)
                {
                    LicenseClassID = (int)Result;
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

            return LicenseClassID;
        }


        public static bool IsLicenseExist(int LicenseID)
        {
            bool Exist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT exist = 'exist'
                            FROM Licenses
                            WHERE LicenseID = @LicenseID;";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {
                    Exist = true;
                }
                else
                {
                    Exist = false;
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

            return Exist;

        }


        public static bool IsLicenseActive(int LicenseID)
        {
            bool IsLicenseActive = false;

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string Query = @"SELECT IsActive
                         FROM Licenses
                         WHERE LicenseID = @LicenseID;";

                using (SqlCommand Command = new SqlCommand(Query, Connection))
                {
                    Command.Parameters.AddWithValue("@LicenseID", LicenseID);

                    try
                    {
                        Connection.Open();
                        object Result = Command.ExecuteScalar();

                        if (Result != null)
                        {
                            IsLicenseActive = (bool)Result; // Cast the result directly to bool
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
                }
            }

            return IsLicenseActive;
        }



        public static bool DeactivateLicense(int LicenseID)
        {
            int RowsAffected = 0;

            using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string Query = @"UPDATE [dbo].[Licenses]
                         SET [IsActive] = 0
                         WHERE LicenseID = @LicenseID";

                using (SqlCommand Command = new SqlCommand(Query, Connection))
                {
                    Command.Parameters.AddWithValue("@LicenseID", LicenseID);

                    try
                    {
                        Connection.Open();
                        RowsAffected = Command.ExecuteNonQuery(); // Executes the update command
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    finally
                    {
                        Connection.Close();
                    }
                }
            }

            return RowsAffected > 0;
        }



        public static DateTime GetExpirationDateUsingLicenseID(int LicenseID)
        {

                DateTime ExpirationDate = DateTime.Now;

                SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                string Query = @"select ExpirationDate from Licenses
                            where LicenseID = @LicenseID";

                SqlCommand Command = new SqlCommand(Query, Connection);
                Command.Parameters.AddWithValue("@LicenseID", LicenseID);


                try
                {
                    Connection.Open();
                    object Result = Command.ExecuteScalar();

                    if (Result != null)
                    {
                        ExpirationDate = (DateTime)Result;
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

                return ExpirationDate;
            


        }




    }
}
