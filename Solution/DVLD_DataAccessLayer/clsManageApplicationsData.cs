using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DVLD_DataAccessLayer
{
    public static class clsManageApplicationsData
    {


        public static int MakeApplicationAndReturnApplicationID(int ApplicantPersonID, DateTime ApplicationDate,
            int ApplicationTypeID, int ApplicationStatus, DateTime LastStatusDate, decimal PaidFees, int CreatedByUserID)
        {

            int ApplicationID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"INSERT INTO [dbo].[Applications]
                           ([ApplicantPersonID]
                           ,[ApplicationDate]
                           ,[ApplicationTypeID]
                           ,[ApplicationStatus]
                           ,[LastStatusDate]
                           ,[PaidFees]
                           ,[CreatedByUserID])
                     VALUES
                           (@ApplicantPersonID
                           ,@ApplicationDate
                           ,@ApplicationTypeID
                           ,@ApplicationStatus
                           ,@LastStatusDate
                           ,@PaidFees
                           ,@CreatedByUserID)
		                    select SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
            Command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
            Command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
            Command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
            Command.Parameters.AddWithValue("@PaidFees", PaidFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);


            try
            {

                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    ApplicationID =  Convert.ToInt32(Result);
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
            
            return ApplicationID;
            
        }


        public static bool UpdateApplication(int ApplicationID, int ApplicantPersonID)
        {
            int RowsAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"UPDATE [dbo].[Applications]
                           SET [ApplicantPersonID] = @ApplicantPersonID
                           WHERE ApplicationID = @ApplicationID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);


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

        public static bool DeleteApplication(int ApplicationID)
        {
            int RowsAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"Delete From Applications
                           WHERE ApplicationID = @ApplicationID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

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


        public static bool CancelApplication(int ApplicationID)
        {
            int RowsAffected = 0;
            byte ApplicationStatus = 2; // Cancel

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"UPDATE [dbo].[Applications]
                           SET [ApplicationStatus] = @ApplicationStatus
                           WHERE ApplicationID = @ApplicationID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);


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


        public static int GetPersonIDUsingApplicationID (int ApplicationID) 
        {
            int ApplicantPersonID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select ApplicantPersonID from Applications
                            where ApplicationID = @ApplicationID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);


            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    ApplicantPersonID = (int)Result;
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

            return ApplicantPersonID;
        }

        public static int GetApplicationTypeIDUsingApplicationID(int ApplicationID) 
        {
            int ApplicationTypeID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select ApplicationTypeID from Applications
                            where ApplicationID = @ApplicationID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);


            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    ApplicationTypeID = (int)Result;
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

            return ApplicationTypeID;
        }


        public static DateTime GetDateUsingApplicationID(int ApplicationID) 
        {
            DateTime ApplicationDate = DateTime.Now;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select ApplicationDate from Applications
                            where ApplicationID = @ApplicationID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);


            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    ApplicationDate = (DateTime)Result;
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

            return ApplicationDate;
        }


        public static DataRow GetApplicationRecordUsingID(int ApplicationID)
        {
            DataTable dt = new DataTable();
            DataRow dr = dt.NewRow();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string query = @"select * from Applications
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


        public static bool SaveTestAppointmentIDAndRetakeTestApplicationID(int TestAppointmentID, int RetakeTestApplicationID)
        {

            int RowsAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"INSERT INTO [dbo].[TestAppointmentIDWithRetakeTestApplicationID]
                                   ([TestAppointmentID]
                                   ,[RetakeTestApplicationID])
                             VALUES
                                   (@TestAppointmentID
                                   ,@RetakeTestApplicationID)";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
            Command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);


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


        public static int GetRetakeTestApplicationID(int TestAppointmentID)
        {
            int AppointmentID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select RetakeTestApplicationID from TestAppointmentIDWithRetakeTestApplicationID
                            where TestAppointmentID = @TestAppointmentID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);


            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    AppointmentID = (int)Result;
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


        public static bool UpdateApplicationStatus(int ApplicationID, int ApplicationStatus)
        {
            // Application Status => 1 = new, 2 = Canceled, 3 = Completed
            int RowsAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"UPDATE [dbo].[Applications]
                           SET [ApplicationStatus] = @ApplicationStatus
                           WHERE ApplicationID = @ApplicationID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);


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


        public static bool UpdateApplicationLastStatusDate(int ApplicationID, DateTime Date)
        {

            int RowsAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"UPDATE [dbo].[Applications]
                           SET [LastStatusDate] = @LastStatusDate
                           WHERE ApplicationID = @ApplicationID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@LastStatusDate", Date);


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


    }
}
