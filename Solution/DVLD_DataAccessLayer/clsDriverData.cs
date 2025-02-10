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
    public static class clsDriverData
    {


        public static bool IsPersonADriver(int PersonID)
        {

            bool IsPersonADriver = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select exist = 'yes' from Drivers
                            where PersonID = @PersonID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {
                    IsPersonADriver = true;
                }
                else
                {
                    IsPersonADriver = false;
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

            return IsPersonADriver;

        }



        public static int GetDriverID(int PersonID)
        {

            int DriverID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string query = @"select DriverID from Drivers
                            where PersonID = @PersonID;"
            ;

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    DriverID = (int)Result;
                }
                else
                {
                    Console.WriteLine($"No DriverID Found With This PersonID {PersonID}");
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

        public static int GetPersonID(int DriverID)
        {

            int PersonID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string query = @"select PersonID from Drivers
                            where DriverID = @DriverID;"
            ;

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@DriverID", DriverID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    PersonID = (int)Result;
                }
                else
                {
                    Console.WriteLine($"No PersonID Found With This DriverID {DriverID}");
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

            return PersonID;
        }



        public static int AddNewDriver(int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {

            int DriverID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"INSERT INTO [dbo].[Drivers]
                                   ([PersonID]
                                   ,[CreatedByUserID]
                                   ,[CreatedDate])
                            VALUES
                                   (@PersonID
                                   ,@CreatedByUserID
                                   ,@CreatedDate);
		                    SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@CreatedDate", CreatedDate);


            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    DriverID = Convert.ToInt32(Result);
                }
                else
                {
                    Console.WriteLine("Result Is Null Check DataBase (AddNewDriver Function)");
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


        public static DataTable ListDrivers()
        {

            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT 
                                Drivers.DriverID, 
                                Drivers.PersonID, 
                                People.NationalNo, 
                                CONCAT(People.FirstName, ' ', People.SecondName, ' ', People.ThirdName, ' ', People.LastName) AS FullName, 
                                Drivers.CreatedDate, 
                                COUNT(CASE WHEN Licenses.IsActive = 1 THEN 1 END) AS 'Active Licenses'
                            FROM 
                                Licenses
                            INNER JOIN 
                                Drivers ON Licenses.DriverID = Drivers.DriverID 
                            INNER JOIN 
                                People ON Drivers.PersonID = People.PersonID

                            GROUP BY 
                                Drivers.DriverID, 
                                Drivers.PersonID, 
                                People.NationalNo, 
                                People.FirstName, 
                                People.SecondName, 
                                People.ThirdName, 
                                People.LastName, 
                                Drivers.CreatedDate;";

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
                    Console.WriteLine("No rows found.");
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


        public static DataTable FilterDriversByDriverID(int DriverID)
        {
            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string Query = @"SELECT 
                                Drivers.DriverID, 
                                Drivers.PersonID, 
                                People.NationalNo, 
                                CONCAT(People.FirstName, ' ', People.SecondName, ' ', People.ThirdName, ' ', People.LastName) AS FullName, 
                                Drivers.CreatedDate, 
                                COUNT(CASE WHEN Licenses.IsActive = 1 THEN 1 END) AS 'Active Licenses'
                            FROM 
                                Licenses
                            INNER JOIN 
                                Drivers ON Licenses.DriverID = Drivers.DriverID 
                            INNER JOIN 
                                People ON Drivers.PersonID = People.PersonID
                            Where Drivers.DriverID = @DriverID

                            GROUP BY 
                                Drivers.DriverID, 
                                Drivers.PersonID, 
                                People.NationalNo, 
                                People.FirstName, 
                                People.SecondName, 
                                People.ThirdName, 
                                People.LastName, 
                                Drivers.CreatedDate;";


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
                    Console.WriteLine("No rows found (clsDriversData.FilterDriversByDriverID).");
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


        public static DataTable FilterDriversByPersonID(int PersonID)
        {
            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT 
                                Drivers.DriverID, 
                                Drivers.PersonID, 
                                People.NationalNo, 
                                CONCAT(People.FirstName, ' ', People.SecondName, ' ', People.ThirdName, ' ', People.LastName) AS FullName, 
                                Drivers.CreatedDate, 
                                COUNT(CASE WHEN Licenses.IsActive = 1 THEN 1 END) AS 'Active Licenses'
                            FROM 
                                Licenses
                            INNER JOIN 
                                Drivers ON Licenses.DriverID = Drivers.DriverID 
                            INNER JOIN 
                                People ON Drivers.PersonID = People.PersonID
                            Where Drivers.PersonID = @PersonID

                            GROUP BY 
                                Drivers.DriverID, 
                                Drivers.PersonID, 
                                People.NationalNo, 
                                People.FirstName, 
                                People.SecondName, 
                                People.ThirdName, 
                                People.LastName, 
                                Drivers.CreatedDate;";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);


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
                    Console.WriteLine("No rows found (clsDriversData.FilterDriversByPersonID).");
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


        public static DataTable FilterDriversByNationalNo(string NationalNo)
        {
            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string Query = @"SELECT 
                                Drivers.DriverID, 
                                Drivers.PersonID, 
                                People.NationalNo, 
                                CONCAT(People.FirstName, ' ', People.SecondName, ' ', People.ThirdName, ' ', People.LastName) AS FullName, 
                                Drivers.CreatedDate, 
                                COUNT(CASE WHEN Licenses.IsActive = 1 THEN 1 END) AS 'Active Licenses'
                            FROM 
                                Licenses
                            INNER JOIN 
                                Drivers ON Licenses.DriverID = Drivers.DriverID 
                            INNER JOIN 
                                People ON Drivers.PersonID = People.PersonID

                        	Where People.NationalNo like @NationalNo

                            GROUP BY 
                                Drivers.DriverID, 
                                Drivers.PersonID, 
                                People.NationalNo, 
                                People.FirstName, 
                                People.SecondName, 
                                People.ThirdName, 
                                People.LastName, 
                                Drivers.CreatedDate;";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@NationalNo", NationalNo + "%");

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
                    Console.WriteLine("No rows found (clsDriversData.FilterDriversByNationalNo).");
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


        public static DataTable FilterDriversByFullName(string FullName)
        {
            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT 
                                Drivers.DriverID, 
                                Drivers.PersonID, 
                                People.NationalNo, 
                                CONCAT(People.FirstName, ' ', People.SecondName, ' ', People.ThirdName, ' ', People.LastName) AS FullName, 
                                Drivers.CreatedDate, 
                                COUNT(CASE WHEN Licenses.IsActive = 1 THEN 1 END) AS 'Active Licenses'
                            FROM 
                                Licenses
                            INNER JOIN 
                                Drivers ON Licenses.DriverID = Drivers.DriverID 
                            INNER JOIN 
                                People ON Drivers.PersonID = People.PersonID
                            Where CONCAT(People.FirstName, ' ', People.SecondName, ' ', People.ThirdName, ' ', People.LastName) like @FullName

                            GROUP BY 
                                Drivers.DriverID, 
                                Drivers.PersonID, 
                                People.NationalNo, 
                                People.FirstName, 
                                People.SecondName, 
                                People.ThirdName, 
                                People.LastName, 
                                Drivers.CreatedDate;";



            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@FullName", "%" + FullName + "%");


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
                    Console.WriteLine("No rows found (clsDriversData.FilterDriversByFullName).");
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







    }
}
