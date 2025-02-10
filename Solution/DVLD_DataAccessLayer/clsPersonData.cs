using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DVLD_DataAccessLayer
{
    public static class clsPersonData
    {

        public static DataTable ListPeople ()
        {

            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = "Select * From People";
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

        public static DataTable FilterPeopleByID(int PersonID)
        {

            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = "Select * From People Where PersonID = @PersonID";

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

        public static DataTable FilterPeopleByNationalNo(string NationalNo)
        {

            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = "Select * From People Where NationalNo = @NationalNo";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@NationalNo", NationalNo);

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


        public static DataTable FilterPeopleByGendor(byte Gendor)
        {

            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = "Select * From People Where Gendor = @Gendor";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@Gendor", Gendor);

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


        public static DataTable FilterPeople(string ColumnName, string filter)
        {

            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = $"Select * From People Where {ColumnName} like @filter";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@filter", filter + "%");

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

        public static bool IsNationalNoUnique(string NationalNo) 
        {

            bool Unique = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = "Select UniqueNationalNo = 'Yes' From People Where NationalNo = @NationalNo";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@NationalNo", NationalNo);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {
                    Unique = false;
                }
                else
                {
                    Unique = true;
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

            return Unique;
        }


        public static bool DeletePerson(int PersonID)
        {
            byte RowsAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = "DELETE From People Where PersonID = @PersonID";
            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                Connection.Open();
                RowsAffected = (byte)Command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                Connection.Close();
            }

            return (RowsAffected > 0);

        }

        public static int AddNewPerson(string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName, DateTime DateOfBirth, byte Gendor, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {


            int PersonID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"INSERT INTO [dbo].[People]
                           ([NationalNo]
                           ,[FirstName]
                           ,[SecondName]
                           ,[ThirdName]
                           ,[LastName]
                           ,[DateOfBirth]
                           ,[Gendor]
                           ,[Address]
                           ,[Phone]
                           ,[Email]
                           ,[NationalityCountryID]
                           ,[ImagePath])
                     VALUES
                           (@NationalNo,
                           @FirstName, 
                           @SecondName, 
                           @ThirdName, 
                           @LastName, 
                           @DateOfBirth, 
                           @Gendor, 
                           @Address, 
                           @Phone, 
                           @Email, 
                           @NationalityCountryID, 
                           @ImagePath);
		                   SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@NationalNo", NationalNo);
            Command.Parameters.AddWithValue("@FirstName", FirstName);
            Command.Parameters.AddWithValue("@SecondName", SecondName);
            Command.Parameters.AddWithValue("@ThirdName", string.IsNullOrEmpty(ThirdName) ? (object)DBNull.Value : ThirdName);
            Command.Parameters.AddWithValue("@LastName", LastName);
            Command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            Command.Parameters.AddWithValue("@Gendor", Gendor);
            Command.Parameters.AddWithValue("@Address", Address);
            Command.Parameters.AddWithValue("@Phone", Phone);
            Command.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(Email) ? (object)DBNull.Value : Email);
            Command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            Command.Parameters.AddWithValue("@ImagePath", string.IsNullOrEmpty(ImagePath) ? (object)DBNull.Value : ImagePath);


            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    PersonID = Convert.ToInt32(Result);
                }
                else
                {
                    Console.WriteLine("Result Is Null Check DataBase (AddNewPerson Function)");
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


        public static bool UpdatePerson(int PersonID, string NationalNo, string FirstName, string SecondName, string ThirdName, string LastName, DateTime DateOfBirth, byte Gendor, string Address, string Phone, string Email, int NationalityCountryID, string ImagePath)
        {

            int RowsAffected = 0;
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"UPDATE [dbo].[People]
                           SET [NationalNo] = @NationalNo
                              ,[FirstName] = @FirstName
                              ,[SecondName] = @SecondName
                              ,[ThirdName] = @ThirdName
                              ,[LastName] = @LastName
                              ,[DateOfBirth] = @DateOfBirth
                              ,[Gendor] = @Gendor
                              ,[Address] = @Address
                              ,[Phone] = @Phone
                              ,[Email] = @Email
                              ,[NationalityCountryID] = @NationalityCountryID
                              ,[ImagePath] = @ImagePath
                         WHERE PersonID = @PersonID";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@NationalNo", NationalNo);
            Command.Parameters.AddWithValue("@FirstName", FirstName);
            Command.Parameters.AddWithValue("@SecondName", SecondName);
            Command.Parameters.AddWithValue("@ThirdName", string.IsNullOrEmpty(ThirdName) ? (object)DBNull.Value : ThirdName);
            Command.Parameters.AddWithValue("@LastName", LastName);
            Command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
            Command.Parameters.AddWithValue("@Gendor", Gendor);
            Command.Parameters.AddWithValue("@Address", Address);
            Command.Parameters.AddWithValue("@Phone", Phone);
            Command.Parameters.AddWithValue("@Email", string.IsNullOrEmpty(Email) ? (object)DBNull.Value : Email);
            Command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);
            Command.Parameters.AddWithValue("@ImagePath", string.IsNullOrEmpty(ImagePath) ? (object)DBNull.Value : ImagePath);

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

            return (RowsAffected > 0);

        }


        public static string GetFullNameUsingPersonID(int PersonID)
        {
            string FullName = "";

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string query = @"select CONCAT(MAX(People.FirstName), ' ', 
                                       MAX(People.SecondName), ' ', 
                                       MAX(People.ThirdName), ' ', 
                                       MAX(People.LastName)) AS 'Full Name' from People
                            where PersonID = @PersonID"
            ;

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    FullName = (string)Result;
                }
                else
                {
                    Console.WriteLine($"No FullName Found With This ID {PersonID}");
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

            return FullName;
        }


        public static int GetPersonIDUsingNationalNo(string NationalNo)
        {

            int PersonID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string query = @"select PersonID from People
                            where NationalNo = @NationalNo"
            ;

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@NationalNo", NationalNo);

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
                    Console.WriteLine($"No PersonID Found With This NationalNo {NationalNo}");
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

        public static string GetPersonNationalNoUsingPerosnID(int PersonID)
        {
            string NationalNo = "";

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string query = @"select NationalNo from People
                            where PersonID = @PersonID"
            ;

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    NationalNo = (string)Result;
                }
                else
                {
                    Console.WriteLine($"No NationalNo Found With This ID {PersonID}");
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

            return NationalNo;


        }


        public static bool GetPersonGendorUsingPersonID(int PersonID)
        {
            bool Gendor = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string query = @"select Gendor from People
                            where PersonID = @PersonID"
            ;

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    Gendor = (bool)Result;
                }
                else
                {
                    Console.WriteLine($"No Person Found With This ID {PersonID}");
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

            return Gendor;


        }





    }
}
