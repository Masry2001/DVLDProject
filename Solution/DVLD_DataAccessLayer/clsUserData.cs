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
    static public class clsUserData
    {

        static public DataRow FindUserUsingPasswordAndUserName(string UserName, string Password)
        {
            DataTable dt = new DataTable();
            DataRow dr = dt.NewRow();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            
            string query = "SELECT * FROM Users WHERE UserName = @UserName AND Password = @Password";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@UserName", UserName);
            Command.Parameters.AddWithValue("@Password", Password);

            try
            {
                Connection.Open();

                SqlDataReader reader = Command.ExecuteReader();

                {
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
                            Console.WriteLine("dt.Rows.Count > 0 Is False (There Is No User Matched (FindUserUsingPasswordAndUserName) clsUserData )");
                        }
                    }
                    else
                    {
                        dr = null;
                        Console.WriteLine("Reader.HasRows Is False (There Is No User Matched (FindUserUsingPasswordAndUserName) clsUserData )");
                    }
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


        static public DataTable ListUsers()
        {


            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT 
                                Users.UserID, 
                                Users.PersonID,
                                FullName = FirstName + ' ' + SecondName + 
                                           COALESCE(' ' + NULLIF(ThirdName, ''), '') + ' ' + LastName, 
                                UserName, 
                                IsActive
                            FROM 
                                Users 
                            INNER JOIN 
                                People ON Users.PersonID = People.PersonID;";
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
                    Console.WriteLine("No rows found (clsUserData.ListUsers).");
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


        public static DataTable FilterUsers(string ColumnName, string Filter)
        {
            // Filter May Be The UserName Or the FullName

            string FullNameExpression = "FirstName + ' ' + SecondName + COALESCE(' ' + NULLIF(ThirdName, ''), '') + ' ' + LastName";
            ColumnName = (ColumnName == "FullName") ? FullNameExpression : "UserName";

            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = $@"SELECT Users.UserID, Users.PersonID, 
                         FullName = {FullNameExpression}, 
                         UserName, IsActive
                  FROM Users INNER JOIN
                       People ON Users.PersonID = People.PersonID
                  WHERE {ColumnName} LIKE @Filter;";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@Filter", Filter + "%");

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

        public static DataTable FilterUsers(string ColumnName, int ID)
        {


            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = $@"SELECT Users.UserID, Users.PersonID, FullName = FirstName + ' ' + SecondName + 
                           COALESCE(' ' + NULLIF(ThirdName, ''), '') + ' ' + LastName, 
                           UserName, IsActive
                           FROM Users INNER JOIN
                           People ON Users.PersonID = People.PersonID
				           where Users.{ColumnName} = @ID;";

    

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ID", ID);

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

        public static DataTable FilterUsers(string ColumnName, bool IsActive)
        {


            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = $@"SELECT Users.UserID, Users.PersonID,
                           FullName = FirstName + ' ' + SecondName + 
                           COALESCE(' ' + NULLIF(ThirdName, ''), '') + ' ' + LastName, 
                           UserName, IsActive
                           FROM Users INNER JOIN
                           People ON Users.PersonID = People.PersonID
				           where Users.{ColumnName} = @IsActive;";





            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@IsActive", IsActive);

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


        public static int AddNewUser(int PersonID, string UserName, string Password, bool IsActive)
        {

            int UserID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = @"INSERT INTO [dbo].[Users]
                           ([PersonID]
                           ,[UserName]
                           ,[Password]
                           ,[IsActive])
                     VALUES
                           (@PersonID,
                           @UserName, 
                           @Password, 
                           @IsActive);
		                   SELECT SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, Connection);

            Command.Parameters.AddWithValue("@PersonID", PersonID);
            Command.Parameters.AddWithValue("@UserName", UserName);
            Command.Parameters.AddWithValue("@Password", Password);
            Command.Parameters.AddWithValue("@IsActive", IsActive);


            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    UserID = Convert.ToInt32(Result);
                }
                else
                {
                    Console.WriteLine("Result Is Null Check DataBase (AddNewUser Function)");
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


        public static bool UpdateUser(int UserID, string UserName, string Password, bool IsActive)
        {

            int RowsAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString); 

            string Query = @"UPDATE [dbo].[Users]
                           SET 
                           [UserName] = @UserName
                           ,[Password] = @Password
                           ,[IsActive] = @IsActive
                           WHERE UserID = @UserID;";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue(@"UserId", UserID); 
            Command.Parameters.AddWithValue(@"UserName", UserName); 
            Command.Parameters.AddWithValue(@"Password", Password); 
            Command.Parameters.AddWithValue(@"IsActive", IsActive);

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


        public static bool DeleteUser(int UserID)
        {

            int RowsAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"DELETE FROM [dbo].[Users]
                            WHERE UserID = @UserID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue(@"UserId", UserID);

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


        static public bool IsPersonAUser(int PersonID)
        {

            bool IsPersonAUser = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT exist = 'exist'
                            FROM Users
                            WHERE PersonID = @PersonID;";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@PersonID", PersonID);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {
                    IsPersonAUser = true;
                }
                else
                {
                    IsPersonAUser = false;
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

            return IsPersonAUser;
        }

        public static DataTable FindUserByID(int ID)
        {


            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = $@"SELECT Users.UserID, Users.PersonID, FullName = FirstName + ' ' + SecondName + ' ' +ThirdName 
                           + ' ' + LastName, UserName, Password, IsActive
                           FROM Users INNER JOIN
                           People ON Users.PersonID = People.PersonID
				           where Users.UserID = @ID;";



            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ID", ID);

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


        public static int FindUserIDUsingPasswordAndUserName(string UserName, string Password)
        {

            int UserID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string query = @"select UserID from Users
                            where UserName = @UserName And Password = @Password;";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@UserName", UserName);
            Command.Parameters.AddWithValue("@Password", Password);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    UserID = (int)Result;
                }
                else
                {
                    Console.WriteLine($"No UserID Found With This UserName {UserName}");
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




        public static string FindUserNameUsingUserID(int UserID)
        {

            string UserName = "";

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string query = @"select UserName from Users
                            where UserID = @UserID";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@UserID", UserID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    UserName = (string)Result;
                }
                else
                {
                    Console.WriteLine($"No UserName Found With This UserID {UserID}");
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

            return UserName;
        }
 



    }
}
