using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    public static class clsLicenseClassesData
    {

        public static int FindLicenseClassIDUsingLicenceClassName(string ClassName)
        {


            int LicenceClassID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string query = @"select LicenseClassID from LicenseClasses
                            where ClassName = @ClassName";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@ClassName", ClassName);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    LicenceClassID = (int)Result;
                }
                else
                {
                    Console.WriteLine($"No LicenceClassID Found With This Title {LicenceClassID}");
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

            return LicenceClassID;

        }

        public static decimal GetLicenseClassFeesUsingLicenseClassID(int LicenseClassID)
        {
            decimal LicenceClassID = -1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string query = @"select ClassFees from LicenseClasses
                            where LicenseClassID = @LicenseClassID";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    LicenceClassID = (decimal)Result;
                }
                else
                {
                    Console.WriteLine($"No LicenceClassID Found With This Title {LicenceClassID}");
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

            return LicenceClassID;
        }


        public static string FindLicenseClassNameUsingLicenceClassID(int LicenseClassID)
        {


            string LicenceClassName = "";

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string query = @"select ClassName from LicenseClasses
                            where LicenseClassID = @LicenseClassID";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    LicenceClassName = (string)Result;
                }
                else
                {
                    Console.WriteLine($"No LicenceClassID Found With This Title {LicenseClassID}");
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

            return LicenceClassName;

        }



        public static byte GetLicenseDefaulltValidityLength(int LicenseClassID)
        {

            byte DefaulltValidityLength = 1;

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);


            string query = @"select DefaultValidityLength from LicenseClasses
                            where LicenseClassID = @LicenseClassID";

            SqlCommand Command = new SqlCommand(query, Connection);

            Command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

            try
            {
                Connection.Open();

                object Result = Command.ExecuteScalar();

                if (Result != null)
                {
                    DefaulltValidityLength = (byte)Result;
                }
                else
                {
                    Console.WriteLine($"No DefaulltValidityLength Found For This LicenseClassID {LicenseClassID}");
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

            return DefaulltValidityLength;


        }



    }
}
