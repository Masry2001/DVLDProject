using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_BusinessLayer
{
    public static class clsInternationalLicensesBusiness
    {

        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {
            return clsInternationalLicensesData.GetDriverInternationalLicenses(DriverID);
        }

        public static DataTable ListInternationalLicenses()
        {
            return clsInternationalLicensesData.ListInternationalLicenses();
        }




        public static int MakeInternaionalLicenseAndReturnID(int ApplicationID, int DriverID, int LicenseID, DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int UserID)
        {
            return clsInternationalLicensesData.MakeInternaionalLicenseAndReturnID(ApplicationID, DriverID, LicenseID, IssueDate, ExpirationDate, IsActive, UserID);

        }


        public static DataRow GetInternaionalLicenseRecord(int InternationalLicenseID)
        {
            return clsInternationalLicensesData.GetInternaionalLicenseRecord(InternationalLicenseID);

        }

        public static bool IsThereInternationalLicenseIsuuedUsingLocalLicense(int LicenseID)
        {
            return clsInternationalLicensesData.IsThereInternationalLicenseIsuuedUsingLocalLicense(LicenseID);

        }

        public static DataTable FilterInternationalDrivingLicenseApplicationsByIntLicID(int IntLicID)
        {
            return clsInternationalLicensesData.FilterInternationalDrivingLicenseApplicationsByIntLicID(IntLicID);

        }  
        
        public static DataTable FilterInternationalDrivingLicenseApplicationsByIntAppID(int IntAppID)
        {
            return clsInternationalLicensesData.FilterInternationalDrivingLicenseApplicationsByIntAppID(IntAppID);

        }

        public static DataTable FilterInternationalDrivingLicenseApplicationsByLLicID(int LLicID)
        {
            return clsInternationalLicensesData.FilterInternationalDrivingLicenseApplicationsByLLicID(LLicID);

        }

        public static int GetDriverIDUsingIntLiceID(int InternationalLicenseID)
        {
            return clsInternationalLicensesData.GetDriverIDUsingIntLiceID(InternationalLicenseID);

        }





    }
}
