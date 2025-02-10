using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    static public class clsCountryBusiness
    {

        static public DataTable ListCountries()
        {
            return clsCountryData.ListCountries();
        }


        static public string FindCountryNameByID(int CountryID)
        {
            return clsCountryData.FindCountryNameByID(CountryID);
        }


    }
}
