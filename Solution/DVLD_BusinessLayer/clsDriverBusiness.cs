using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public static class clsDriverBusiness
    {

        public static bool IsPersonADriver(int PersonID)
        {
            return clsDriverData.IsPersonADriver(PersonID);
        }


        public static int GetDriverID(int PersonID)
        {
            return clsDriverData.GetDriverID(PersonID);
        }
        public static int GetPersonID(int DriverID)
        {
            return clsDriverData.GetPersonID(DriverID);
        }


        public static int AddNewDriver(int PersonID, int CreatedByUserID, DateTime CreatedDate)
        {
            return clsDriverData.AddNewDriver(PersonID,  CreatedByUserID, CreatedDate);
        }

        static public DataTable ListDrivers()
        {
            return clsDriverData.ListDrivers();
        }


        public static DataTable FilterDriversByDriverID(int DriverID)
        {
            return clsDriverData.FilterDriversByDriverID(DriverID);
        }

        public static DataTable FilterDriversByPersonID(int PersonID)
        {
            return clsDriverData.FilterDriversByPersonID(PersonID);
        }

        public static DataTable FilterDriversByNationalNo(string NationalNo)
        {
            return clsDriverData.FilterDriversByNationalNo(NationalNo);
        }

        public static DataTable FilterDriversByFullName(string FullName)
        {
            return clsDriverData.FilterDriversByFullName(FullName);
        }



    }
}
