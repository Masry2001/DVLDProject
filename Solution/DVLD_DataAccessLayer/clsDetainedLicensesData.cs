using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public static class clsDetainedLicensesData
    {

        public static bool IsLicenseExistInDetainedLicensesList(int LicenseID)
        {


            bool IsLicenseDetained = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select DetainedLicense = 'Yes' from DetainedLicenses
                            where LicenseID = @LicenseID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {
                    IsLicenseDetained = true;
                }
                else
                {
                    IsLicenseDetained = false;
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

            return IsLicenseDetained;


        }


        public static bool GetReleasedField(int LicenseID)
        {
            bool IsLicenseDetained = false;

            // Use 'using' to ensure the connection and command are properly disposed of
            using (SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                string Query = @"SELECT TOP 1 IsReleased FROM DetainedLicenses
                         WHERE LicenseID = @LicenseID
                         ORDER BY DetainID DESC";

                using (SqlCommand Command = new SqlCommand(Query, Connection))
                {
                    Command.Parameters.AddWithValue("@licenseID", LicenseID);

                    try
                    {
                        Connection.Open();
                        object Result = Command.ExecuteScalar();

                        if (Result != null && Result != DBNull.Value)
                        {
                            IsLicenseDetained = (bool)Result;
                        }
                        // else case is unnecessary as IsLicenseDetained is already set to false by default
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        // Optionally, rethrow the exception or handle it as needed
                    }
                }
            }

            return IsLicenseDetained;
        }


        public static bool IsLicenseDetainedAndNotReleased(int LicenseID)
        {

            bool IsLicenseDetainedAndNotReleased = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select top 1 IsReleased from DetainedLicenses
                            where IsReleased = 0 and LicenseID = @LicenseID
                            order by DetainID desc";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {
                    IsLicenseDetainedAndNotReleased = true;
                }
                else
                {
                    IsLicenseDetainedAndNotReleased = false;
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

            return IsLicenseDetainedAndNotReleased;
        }



        public static int DetainLicense(int LicenseID, DateTime DetainDate, decimal FineFees, int CreatedByUserID)
        {

            int DetainID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"INSERT INTO [dbo].[DetainedLicenses]
                                   ([LicenseID]
                                   ,[DetainDate]
                                   ,[FineFees]
                                   ,[CreatedByUserID]
                                   ,[IsReleased]
                                   ,[ReleaseDate]
                                   ,[ReleasedByUserID]
                                   ,[ReleaseApplicationID])
                             VALUES
                                   (@LicenseID
                                   ,@DetainDate
                                   ,@FineFees
                                   ,@CreatedByUserID
                                   ,@IsReleased
                                   ,@ReleaseDate
                                   ,@ReleasedByUserID
                                   ,@ReleaseApplicationID)
		                    Select SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);
            Command.Parameters.AddWithValue("@DetainDate", DetainDate);
            Command.Parameters.AddWithValue("@FineFees", FineFees);
            Command.Parameters.AddWithValue("@CreatedByUserID", CreatedByUserID);
            Command.Parameters.AddWithValue("@IsReleased", 0);
            Command.Parameters.AddWithValue("@ReleaseDate", DBNull.Value);
            Command.Parameters.AddWithValue("@ReleasedByUserID",  DBNull.Value);
            Command.Parameters.AddWithValue("@ReleaseApplicationID",  DBNull.Value);


            try
            {

                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    DetainID = Convert.ToInt32(Result);
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

            return DetainID;

        }


        public static bool ReleaseDetainedLicense(int LicenseID, DateTime ReleaseDate, int ReleasedByUserID, int ReleaseApplicationID)
        {

            int RowsAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"UPDATE [dbo].[DetainedLicenses]
                            SET
                              [IsReleased] = @IsReleased
                              ,[ReleaseDate] = @ReleaseDate
                              ,[ReleasedByUserID] = @ReleasedByUserID
                              ,[ReleaseApplicationID] = @ReleaseApplicationID
                            WHERE LicenseID = @LicenseID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LicenseID", LicenseID);
            Command.Parameters.AddWithValue("@IsReleased", 1);
            Command.Parameters.AddWithValue("@ReleaseDate", ReleaseDate);
            Command.Parameters.AddWithValue("@ReleasedByUserID", ReleasedByUserID);
            Command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);


            try
            {

                Connection.Open();

                RowsAffected = Command.ExecuteNonQuery();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {

                Connection.Close();
            }

            return RowsAffected > 0;

        }


        public static DataRow GetDetainedLicenseRecordUsingLicenseID(int LicenseID)
        {

            DataTable dt = new DataTable();
            DataRow dr = dt.NewRow();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string query = @"select top 1 * from DetainedLicenses
                            Where LicenseID = @LicenseID
							order by DetainID desc";

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
                        Console.WriteLine("dt.Rows.Count > 0 Is False (GetDetainedLicenseRecordUsingLicenseID )");
                    }
                }
                else
                {
                    dr = null;
                    Console.WriteLine("Reader.HasRows Is False (GetDetainedLicenseRecordUsingLicenseID  )");
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


        public static DataTable ListDetainedLicenses()
        {

            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT 
                                DetainedLicenses.DetainID AS 'D.ID', 
                                DetainedLicenses.LicenseID AS 'L.ID',
                                DetainedLicenses.DetainDate AS 'D.Date', 
                                DetainedLicenses.IsReleased, 
                                DetainedLicenses.FineFees,
                                DetainedLicenses.ReleaseDate, 
                                People.NationalNo, 
                                FullName = FirstName + ' ' + SecondName + COALESCE(' ' + NULLIF(ThirdName, ''), '') + ' ' + LastName,  
                                DetainedLicenses.ReleaseApplicationID AS 'Release App.ID' 
                            FROM 
                                DetainedLicenses 
                            LEFT JOIN 
                                Applications ON DetainedLicenses.ReleaseApplicationID = Applications.ApplicationID 
                            LEFT JOIN 
                                People ON Applications.ApplicantPersonID = People.PersonID;

";
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
                    Console.WriteLine("No rows found (ListDetainedLicenses).");
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


        public static DataTable FilterDetainedLicenseByDetainID(int DetainID)
        {

            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT 
                                DetainedLicenses.DetainID AS 'D.ID', 
                                DetainedLicenses.LicenseID AS 'L.ID',
                                DetainedLicenses.DetainDate AS 'D.Date', 
                                DetainedLicenses.IsReleased, 
                                DetainedLicenses.FineFees,
                                DetainedLicenses.ReleaseDate, 
                                People.NationalNo, 
                                FullName = FirstName + ' ' + SecondName + COALESCE(' ' + NULLIF(ThirdName, ''), '') + ' ' + LastName,  
                                DetainedLicenses.ReleaseApplicationID AS 'Release App.ID' 
                            FROM 
                                DetainedLicenses 
                            LEFT JOIN 
                                Applications ON DetainedLicenses.ReleaseApplicationID = Applications.ApplicationID 
                            LEFT JOIN 
                                People ON Applications.ApplicantPersonID = People.PersonID
                                								where DetainID = @DetainID;";
            // COALESCE handle the case when ThirdName = "" or = null

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@DetainID", DetainID);

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
                    Console.WriteLine("No rows found (FilterDetainedLicenseByDetainID).");
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


        public static DataTable FilterDetainedLicenseByReleaseApplicationID(int ReleaseApplicationID)
        {

            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT 
                                DetainedLicenses.DetainID AS 'D.ID', 
                                DetainedLicenses.LicenseID AS 'L.ID',
                                DetainedLicenses.DetainDate AS 'D.Date', 
                                DetainedLicenses.IsReleased, 
                                DetainedLicenses.FineFees,
                                DetainedLicenses.ReleaseDate, 
                                People.NationalNo, 
                                FullName = FirstName + ' ' + SecondName + COALESCE(' ' + NULLIF(ThirdName, ''), '') + ' ' + LastName,  
                                DetainedLicenses.ReleaseApplicationID AS 'Release App.ID' 
                            FROM 
                                DetainedLicenses 
                            LEFT JOIN 
                                Applications ON DetainedLicenses.ReleaseApplicationID = Applications.ApplicationID 
                            LEFT JOIN 
                                People ON Applications.ApplicantPersonID = People.PersonID
                            where ReleaseApplicationID = @ReleaseApplicationID;";
            // COALESCE handle the case when ThirdName = "" or = null

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);

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
                    Console.WriteLine("No rows found (FilterDetainedLicenseByDetainID).");
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


        public static DataTable FilterDetainedLicenseByNationalNo(string NationalNo)
        {

            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT 
                                DetainedLicenses.DetainID AS 'D.ID', 
                                DetainedLicenses.LicenseID AS 'L.ID',
                                DetainedLicenses.DetainDate AS 'D.Date', 
                                DetainedLicenses.IsReleased, 
                                DetainedLicenses.FineFees,
                                DetainedLicenses.ReleaseDate, 
                                People.NationalNo, 
                                FullName = FirstName + ' ' + SecondName + COALESCE(' ' + NULLIF(ThirdName, ''), '') + ' ' + LastName,  
                                DetainedLicenses.ReleaseApplicationID AS 'Release App.ID' 
                            FROM 
                                DetainedLicenses 
                            LEFT JOIN 
                                Applications ON DetainedLicenses.ReleaseApplicationID = Applications.ApplicationID 
                            LEFT JOIN 
                                People ON Applications.ApplicantPersonID = People.PersonID
                            where People.NationalNo like @NationalNo;";
            // COALESCE handle the case when ThirdName = "" or = null

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@NationalNo", "%" + NationalNo + "%");

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
                    Console.WriteLine("No rows found (FilterDetainedLicenseByNationalNo).");
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


        public static DataTable FilterDetainedLicenseByFullName(string FullName)
        {

            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT 
                                DetainedLicenses.DetainID AS 'D.ID', 
                                DetainedLicenses.LicenseID AS 'L.ID',
                                DetainedLicenses.DetainDate AS 'D.Date', 
                                DetainedLicenses.IsReleased, 
                                DetainedLicenses.FineFees,
                                DetainedLicenses.ReleaseDate, 
                                People.NationalNo, 
                                FullName = FirstName + ' ' + SecondName + COALESCE(' ' + NULLIF(ThirdName, ''), '') + ' ' + LastName,  
                                DetainedLicenses.ReleaseApplicationID AS 'Release App.ID' 
                            FROM 
                                DetainedLicenses 
                            LEFT JOIN 
                                Applications ON DetainedLicenses.ReleaseApplicationID = Applications.ApplicationID 
                            LEFT JOIN 
                                People ON Applications.ApplicantPersonID = People.PersonID
                            where  FirstName + ' ' + SecondName + COALESCE(' ' + NULLIF(ThirdName, ''), '') + ' ' + LastName like @FullName;";
            // COALESCE handle the case when ThirdName = "" or = null

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
                    Console.WriteLine("No rows found (FilterDetainedLicenseByFullName).");
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


        public static DataTable FilterDetainedLicenseByIsReleased(bool IsReleased)
        {

            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT 
                                DetainedLicenses.DetainID AS 'D.ID', 
                                DetainedLicenses.LicenseID AS 'L.ID',
                                DetainedLicenses.DetainDate AS 'D.Date', 
                                DetainedLicenses.IsReleased, 
                                DetainedLicenses.FineFees,
                                DetainedLicenses.ReleaseDate, 
                                People.NationalNo, 
                                FullName = FirstName + ' ' + SecondName + COALESCE(' ' + NULLIF(ThirdName, ''), '') + ' ' + LastName,  
                                DetainedLicenses.ReleaseApplicationID AS 'Release App.ID' 
                            FROM 
                                DetainedLicenses 
                            LEFT JOIN 
                                Applications ON DetainedLicenses.ReleaseApplicationID = Applications.ApplicationID 
                            LEFT JOIN 
                                People ON Applications.ApplicantPersonID = People.PersonID
                            where  IsReleased = @IsReleased";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@IsReleased", IsReleased);

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
                    Console.WriteLine("No rows found (FilterDetainedLicenseByIsReleased).");
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
