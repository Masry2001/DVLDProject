using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DVLD_DataAccessLayer
{
    public static class clsManageApplicationTypesData
    {


        public static DataTable ListApplicationTypes()
        {
            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT * FROM [dbo].[ApplicationTypes]";


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
                    Console.WriteLine("No rows found (clsManageApplicationTypesData.ListApplicationTypes).");
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

        public static DataRow FindApplicationType(int ApplicationID)
        {
            DataTable dt = new DataTable();
            DataRow dr = dt.NewRow();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string query = @"SELECT * FROM [dbo].[ApplicationTypes]
                            Where ApplicationTypeID = @ApplicationID";

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
                            Console.WriteLine("dt.Rows.Count > 0 Is False (clsManageApplicationTypesData.FindApplicationType )");
                        }
                    }
                    else
                    {
                        dr = null;
                        Console.WriteLine("Reader.HasRows Is False (tionTypesData.FindApplicationType )");
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


        public static int FindApplicationTypeIDUsingApplicationTypeTitle(string ApplicationTypeTitle)
        {
            
            int ApplicationTypeID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string query = @"select ApplicationTypeID from ApplicationTypes
                            where ApplicationTypeTitle = @ApplicationTypeTitle;";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                  ApplicationTypeID = (int)Result;
                }
                else
                {
                    Console.WriteLine($"No ApplicationID Found With This Title {ApplicationTypeTitle}");
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

        public static decimal GetApplicationFees(int ApplicationTypeID)
        {

            decimal ApplicationFees = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string query = @"select ApplicationFees from ApplicationTypes
                            where ApplicationTypeID = @ApplicationTypeID;";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    ApplicationFees = (decimal)Result;
                }
                else
                {
                    Console.WriteLine($"No ApplicationID Found With This ID {ApplicationTypeID}");
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

            return ApplicationFees;
        }

        public static decimal GetApplicationFees(string ApplicationTypeTitle)
        {

            decimal ApplicationFees = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string query = @"select ApplicationFees from ApplicationTypes
                            where ApplicationTypeTitle = @ApplicationTypeTitle;";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@ApplicationTypeTitle", ApplicationTypeTitle);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    ApplicationFees = (decimal)Result;
                }
                else
                {
                    Console.WriteLine($"No ApplicationTypeTitle Found With This Name {ApplicationTypeTitle}");
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

            return ApplicationFees;
        }




        public static bool UpdateApplication(int ApplicationID, string ApplicationName, decimal ApplicationFees)
        {

            int RowsAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string Query = @"UPDATE [dbo].[ApplicationTypes]
                               SET [ApplicationTypeTitle] = @ApplicationName
                                  ,[ApplicationFees] = @ApplicationFees
                             WHERE ApplicationTypeID = @ApplicationID ";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue(@"ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue(@"ApplicationName", ApplicationName);
            Command.Parameters.AddWithValue(@"ApplicationFees", ApplicationFees);

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
