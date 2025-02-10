using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccessLayer
{
    static public class clsCountryData
    {

        static public DataTable ListCountries()
        {
            DataTable dataTable = new DataTable();
            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = "Select * From Countries";
            SqlCommand Command = new SqlCommand(Query, Connection);

            try
            {
                Connection.Open();
                SqlDataReader Reader = Command.ExecuteReader();

                if (Reader.HasRows)
                {
                    dataTable.Load(Reader);
                } else
                {
                    Console.WriteLine("Reader Dosen't Has Rows");
                }

                Reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
            finally
            {
                Connection.Close(); 
            }

            return dataTable;

        }

        static public string FindCountryNameByID(int CountryID)
        {

            string CountryName = "";

            SqlConnection Connection = new SqlConnection(clsDataAccessSettings.ConnectionString);
            string Query = "select CountryName From Countries where CountryID = @CountryID";

            SqlCommand Command = new SqlCommand(Query, Connection);
            Command.Parameters.AddWithValue("@CountryID", CountryID);

            try
            {
                Connection.Open();

                object result = Command.ExecuteScalar();

                if (result != null)
                {
                    CountryName = Convert.ToString(result);
                }
                else
                {
                    Console.WriteLine("Result is null (DataAccess Layer FindCountryNameByID Method) .");
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
            return CountryName;

        }



    }
}
