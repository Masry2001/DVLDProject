using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD_DataAccessLayer
{
    public static class clsLocalDrivingLicenceApplicationData
    {

        public static bool IsPersonHasTheSpecifiedClassAndStatusIsNewOrCompleted(string NationalNo, string SpecifiedClassName)
        {
          

            bool IsPersonHasTheSpecifiedClassAndStatusIsNewOrCompleted = false;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT IsPersonHasTheSpecifiedClassAndStatusIsNewOrCompleted = 'Yes'
                            FROM   LocalDrivingLicenseApplications
                                   INNER JOIN LicenseClasses
                                           ON LocalDrivingLicenseApplications.LicenseClassID = LicenseClasses.LicenseClassID
                                   INNER JOIN Applications
                                           ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                                   INNER JOIN People
                                           ON Applications.ApplicantPersonID = People.PersonID
                            WHERE  People.NationalNo = @NationalNo -- paramater placeholder
                                   AND LicenseClasses.ClassName = @SpecifiedClassName -- parameter placeholder
                                   AND Applications.ApplicationStatus IN (1, 3)  -- 1 for 'new', 3 for 'completed'
                            GROUP  BY LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID;
";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@NationalNo", NationalNo);
            Command.Parameters.AddWithValue("@SpecifiedClassName", SpecifiedClassName);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {
                    IsPersonHasTheSpecifiedClassAndStatusIsNewOrCompleted = true;
                }
                else
                {
                    IsPersonHasTheSpecifiedClassAndStatusIsNewOrCompleted = false;
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

            return IsPersonHasTheSpecifiedClassAndStatusIsNewOrCompleted;


        }


        public static int MakeALocalDrivingLicenseApplication(int ApplicationID, int LicenseClassID)
        {

            int LocalDrivingLicenseApplicaionID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"INSERT INTO [dbo].[LocalDrivingLicenseApplications]
                                   ([ApplicationID]
                                   ,[LicenseClassID])
                             VALUES
                                   (@ApplicationID
                                   ,@LicenseClassID)
		                            select SCOPE_IDENTITY();";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
      

            try
            {

                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    LocalDrivingLicenseApplicaionID = Convert.ToInt32(Result);
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

            return LocalDrivingLicenseApplicaionID;


        }


        public static bool UpdateLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID, int LicenseClassID)
        {

            int RowsAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"UPDATE [dbo].[LocalDrivingLicenseApplications]
                           Set LicenseClassID = @LicenseClassID
                           WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue(@"LicenseClassID", LicenseClassID);
            Command.Parameters.AddWithValue(@"LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);


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

        public static bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {

            int RowsAffected = 0;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"DELETE FROM [dbo].[LocalDrivingLicenseApplications]
                            WHERE LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue(@"LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);


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


        public static DataTable ListLocalDrivingLicenseApplications()
        {
            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID AS 'L.D.L.AppID',
                                        MAX(LicenseClasses.ClassName) AS 'Driving Class',
                                        MAX(People.NationalNo) AS 'National No.',
                                        CONCAT(MAX(People.FirstName), ' ', 
                                                MAX(People.SecondName), ' ', 
                                                MAX(People.ThirdName), ' ', 
                                                MAX(People.LastName)) AS 'Full Name',
                                        MAX(Applications.ApplicationDate) AS 'Application Date',
                                        (SELECT COUNT(*)
                                        FROM   Tests t
                                        INNER JOIN TestAppointments ta 
                                                ON t.TestAppointmentID = ta.TestAppointmentID
                                        WHERE  ta.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID 
                                                AND t.TestResult = 1) AS 'Passed Tests',
                                        CASE MAX(Applications.ApplicationStatus)
                                            WHEN 1 THEN 'new'
                                            WHEN 2 THEN 'canceled'
                                            WHEN 3 THEN 'completed'
                                            ELSE 'unknown'
                                        END AS 'Status'
                                FROM   LocalDrivingLicenseApplications
                                        INNER JOIN LicenseClasses
                                                ON LocalDrivingLicenseApplications.LicenseClassID = LicenseClasses.LicenseClassID
                                        INNER JOIN Applications
                                                ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                                        INNER JOIN People
                                                ON Applications.ApplicantPersonID = People.PersonID

                                GROUP  BY LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID;";

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
                    Console.WriteLine("No rows found (ListLocalDrivingLicenseApplications.ListLocalDrivingLicenseApplications).");
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


        public static DataTable FilterLocalDrivingLicenseApplicationsByID(int LocalDrivingLicenseApplicationID)
        {
            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string Query = @"SELECT LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID AS 'L.D.L.AppID',
                                       MAX(LicenseClasses.ClassName) AS 'Driving Class',
                                       MAX(People.NationalNo) AS 'National No.',
                                       CONCAT(MAX(People.FirstName), ' ', 
                                              MAX(People.SecondName), ' ', 
                                              MAX(People.ThirdName), ' ', 
                                              MAX(People.LastName)) AS 'Full Name',
                                       MAX(Applications.ApplicationDate) AS 'Application Date',
                                       (SELECT COUNT(*)
                                        FROM   Tests t
                                        INNER JOIN TestAppointments ta 
                                                ON t.TestAppointmentID = ta.TestAppointmentID
                                        WHERE  ta.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID 
                                               AND t.TestResult = 1) AS 'Passed Tests',
                                       CASE MAX(Applications.ApplicationStatus)
                                           WHEN 1 THEN 'new'
                                           WHEN 2 THEN 'canceled'
                                           WHEN 3 THEN 'completed'
                                           ELSE 'unknown'
                                       END AS 'Status'
                                FROM   LocalDrivingLicenseApplications
                                       INNER JOIN LicenseClasses
                                               ON LocalDrivingLicenseApplications.LicenseClassID = LicenseClasses.LicenseClassID
                                       INNER JOIN Applications
                                               ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                                       INNER JOIN People
                                               ON Applications.ApplicantPersonID = People.PersonID
							    Where LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID


                                GROUP  BY LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID;";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

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
                    Console.WriteLine("No rows found (clsLocalDrivingLicenceApplicationData.FilterLocalDrivingLicenseApplicationsByID).");
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


        public static DataTable FilterLocalDrivingLicenseApplicationsByNationalNo(string NationalNo)
        {
            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string Query = @"SELECT LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID AS 'L.D.L.AppID',
                               MAX(LicenseClasses.ClassName) AS 'Driving Class',
                               MAX(People.NationalNo) AS 'National No.',
                               CONCAT(MAX(People.FirstName), ' ', 
                                      MAX(People.SecondName), ' ', 
                                      MAX(People.ThirdName), ' ', 
                                      MAX(People.LastName)) AS 'Full Name',
                               MAX(Applications.ApplicationDate) AS 'Application Date',
                               (SELECT COUNT(*)
                                FROM   Tests t
                                INNER JOIN TestAppointments ta 
                                        ON t.TestAppointmentID = ta.TestAppointmentID
                                WHERE  ta.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID 
                                       AND t.TestResult = 1) AS 'Passed Tests',
                               CASE MAX(Applications.ApplicationStatus)
                                   WHEN 1 THEN 'new'
                                   WHEN 2 THEN 'canceled'
                                   WHEN 3 THEN 'completed'
                                   ELSE 'unknown'
                               END AS 'Status'
                        FROM   LocalDrivingLicenseApplications
                               INNER JOIN LicenseClasses
                                       ON LocalDrivingLicenseApplications.LicenseClassID = LicenseClasses.LicenseClassID
                               INNER JOIN Applications
                                       ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                               INNER JOIN People
                                       ON Applications.ApplicantPersonID = People.PersonID
						WHERE People.NationalNo like @NationalNo


                        GROUP  BY LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID;";

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
                    Console.WriteLine("No rows found (clsLocalDrivingLicenceApplicationData.FilterLocalDrivingLicenseApplicationsByNationalNo).");
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


        public static DataTable FilterLocalDrivingLicenseApplicationsByFullName(string FullName)
        {
            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID AS 'L.D.L.AppID',
                                   MAX(LicenseClasses.ClassName) AS 'Driving Class',
                                   MAX(People.NationalNo) AS 'National No.',
                                   CONCAT(MAX(People.FirstName), ' ', 
                                          MAX(People.SecondName), ' ', 
                                          MAX(People.ThirdName), ' ', 
                                          MAX(People.LastName)) AS 'Full Name',
                                   MAX(Applications.ApplicationDate) AS 'Application Date',
                                   (SELECT COUNT(*)
                                    FROM   Tests t
                                    INNER JOIN TestAppointments ta 
                                            ON t.TestAppointmentID = ta.TestAppointmentID
                                    WHERE  ta.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID 
                                           AND t.TestResult = 1) AS 'Passed Tests',
                                   CASE MAX(Applications.ApplicationStatus)
                                       WHEN 1 THEN 'new'
                                       WHEN 2 THEN 'canceled'
                                       WHEN 3 THEN 'completed'
                                       ELSE 'unknown'
                                   END AS 'Status'
                            FROM   LocalDrivingLicenseApplications
                                   INNER JOIN LicenseClasses
                                           ON LocalDrivingLicenseApplications.LicenseClassID = LicenseClasses.LicenseClassID
                                   INNER JOIN Applications
                                           ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                                   INNER JOIN People
                                           ON Applications.ApplicantPersonID = People.PersonID
                                   
                            WHERE  CONCAT(People.FirstName, ' ', 
                                          People.SecondName, ' ', 
                                          People.ThirdName, ' ', 
                                          People.LastName) LIKE @FullName
                            GROUP  BY LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID;";



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
                    Console.WriteLine("No rows found (clsLocalDrivingLicenceApplicationData.FilterLocalDrivingLicenseApplicationsByFullName).");
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



        public static DataTable FilterLocalDrivingLicenseApplicationsByStatus(string Status)
        {
            DataTable dt = new DataTable();

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"SELECT LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID AS 'L.D.L.AppID',
                                       MAX(LicenseClasses.ClassName) AS 'Driving Class',
                                       MAX(People.NationalNo) AS 'National No.',
                                       CONCAT(MAX(People.FirstName), ' ', 
                                              MAX(People.SecondName), ' ', 
                                              MAX(People.ThirdName), ' ', 
                                              MAX(People.LastName)) AS 'Full Name',
                                       MAX(Applications.ApplicationDate) AS 'Application Date',
                                       (SELECT COUNT(*)
                                        FROM   Tests t
                                        INNER JOIN TestAppointments ta 
                                                ON t.TestAppointmentID = ta.TestAppointmentID
                                        WHERE  ta.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID 
                                               AND t.TestResult = 1) AS 'Passed Tests',
                                       CASE MAX(Applications.ApplicationStatus)
                                           WHEN 1 THEN 'new'
                                           WHEN 2 THEN 'canceled'
                                           WHEN 3 THEN 'completed'
                                           ELSE 'unknown'
                                       END AS 'Status'
                                FROM   LocalDrivingLicenseApplications
                                       INNER JOIN LicenseClasses
                                               ON LocalDrivingLicenseApplications.LicenseClassID = LicenseClasses.LicenseClassID
                                       INNER JOIN Applications
                                               ON LocalDrivingLicenseApplications.ApplicationID = Applications.ApplicationID
                                       INNER JOIN People
                                               ON Applications.ApplicantPersonID = People.PersonID
                                    
                                GROUP  BY LocalDrivingLicenseApplications.LocalDrivingLicenseApplicationID
                                HAVING CASE MAX(Applications.ApplicationStatus)
                                           WHEN 1 THEN 'new'
                                           WHEN 2 THEN 'canceled'
                                           WHEN 3 THEN 'completed'
                                           ELSE 'unknown'
                                       END LIKE @Status;";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@Status", Status + "%");


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
                    Console.WriteLine("No rows found (clsLocalDrivingLicenceApplicationData.FilterLocalDrivingLicenseApplicationsByStatus).");
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


        public static int GetApplicationIDUsingLDLAppID(int LocalDrivingLicenseApplicationID)
        {

            int ApplicationID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select ApplicationID from LocalDrivingLicenseApplications
                            where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);


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


        public static int GetLicenseClassIDUsingLDLAppID(int LocalDrivingLicenseApplicationID)
        {

            int LicenseClassID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select LicenseClassID from LocalDrivingLicenseApplications
                            where LocalDrivingLicenseApplicationID = @LocalDrivingLicenseApplicationID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);


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


        public static int GetLDLAppIDUsingAppID(int AppID)
        {

            int LocalDrivingLicenseApplicationID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string Query = @"select LocalDrivingLicenseApplicationID from LocalDrivingLicenseApplications
                            where ApplicationID = @AppID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@AppID", AppID);


            try
            {
                Connection.Open();
                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    LocalDrivingLicenseApplicationID = (int)Result;
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

            return LocalDrivingLicenseApplicationID;
        }





    }
}
