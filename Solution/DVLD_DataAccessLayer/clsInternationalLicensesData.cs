using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public static class clsInternationalLicensesData
    {

        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {

            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"select InternationalLicenseID as 'Int.Lic.ID', ApplicationID as 'App.ID', IssuedUsingLocalLicenseID as 'L.Lic.ID',                     IssueDate, ExpirationDate, IsActive from InternationalLicenses
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
                    Console.WriteLine("No International Licenses found.");
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


        public static DataTable ListInternationalLicenses()
        {
            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"select InternationalLicenseID as 'Int.Lic.ID', ApplicationID as 'App.ID', IssuedUsingLocalLicenseID as 'L.Lic.ID',                     IssueDate, ExpirationDate, IsActive from InternationalLicenses";

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
                    Console.WriteLine("No International Licenses found.");
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


        public static DataTable FilterInternationalDrivingLicenseApplicationsByIntLicID(int IntLicID)
        {

            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"select InternationalLicenseID as 'Int.Lic.ID', ApplicationID as 'App.ID', IssuedUsingLocalLicenseID as 'L.Lic.ID',                     IssueDate, ExpirationDate, IsActive from InternationalLicenses
                            Where InternationalLicenseID = @IntLicID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@IntLicID", IntLicID);


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
                    Console.WriteLine("No International Licenses found.");
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


        public static DataTable FilterInternationalDrivingLicenseApplicationsByIntAppID(int IntAppID)
        {
            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"select InternationalLicenseID as 'Int.Lic.ID', ApplicationID as 'App.ID', IssuedUsingLocalLicenseID as 'L.Lic.ID',                     IssueDate, ExpirationDate, IsActive from InternationalLicenses
                            Where ApplicationID = @IntAppID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@IntAppID", IntAppID);

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
                    Console.WriteLine("No International Licenses found.");
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


        public static DataTable FilterInternationalDrivingLicenseApplicationsByLLicID(int LLicID)
        {
            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"select InternationalLicenseID as 'Int.Lic.ID', ApplicationID as 'App.ID', IssuedUsingLocalLicenseID as 'L.Lic.ID',                     IssueDate, ExpirationDate, IsActive from InternationalLicenses
                            Where IssuedUsingLocalLicenseID = @LLicID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LLicID", LLicID);

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
                    Console.WriteLine("No International Licenses found.");
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



        public static int MakeInternaionalLicenseAndReturnID(int ApplicationID, int DriverID, int LicenseID, DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int UserID)
        {

            int InternationalLicenseID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"INSERT INTO [dbo].[InternationalLicenses]
                           ([ApplicationID]
                           ,[DriverID]
                           ,[IssuedUsingLocalLicenseID]
                           ,[IssueDate]
                           ,[ExpirationDate]
                           ,[IsActive]
                           ,[CreatedByUserID])
                     VALUES
                           (@ApplicationID
                           ,@DriverID
                           ,@LicenseID
                           ,@IssueDate
                           ,@ExpirationDate
                           ,@IsActive
                           ,@CreatedByUserID)
		                    select SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@DriverID", DriverID);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);
            Command.Parameters.AddWithValue("@IssueDate", IssueDate);
            Command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
            Command.Parameters.AddWithValue("@IsActive", IsActive);
            Command.Parameters.AddWithValue("@CreatedByUserID", UserID);


            try
            {

                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    InternationalLicenseID = Convert.ToInt32(Result);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {

                Connection.Close();
            }

            return InternationalLicenseID;


        }

        public static DataRow GetInternaionalLicenseRecord(int InternationalLicenseID)
        {

            DataTable dt = new DataTable();
            DataRow dr = dt.NewRow();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string query = @"select * from InternationalLicenses
                            Where InternationalLicenseID = @InternationalLicenseID";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

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
                        Console.WriteLine("No Rows Found (clsInternationalLicensesData.GetInternaionalLicenseRecord )");
                    }
                }
                else
                {
                    dr = null;
                    Console.WriteLine("Reader.HasRows Is False (clsInternationalLicensesData.GetInternaionalLicenseRecord  )");
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


        public static bool IsThereInternationalLicenseIsuuedUsingLocalLicense(int LicenseID)
        {

            bool Exist = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select exist = 'yes'from InternationalLicenses
                            where IssuedUsingLocalLicenseID = @LicenseID";

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

        public static int GetDriverIDUsingIntLiceID(int InternationalLicenseID)
        {

            int DriverID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select DriverID from InternationalLicenses
                            where InternationalLicenseID = @InternationalLicenseID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();

                if (Result != null ) 
                { 
                    DriverID = (int)Result;
                } else
                {
                    Console.WriteLine($"DriverID Of This InternationalLicensesID {InternationalLicenseID} Not Found (clsInternationalLicensesData.GetDriverIDUsingIntLiceID)");
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


    }






}
