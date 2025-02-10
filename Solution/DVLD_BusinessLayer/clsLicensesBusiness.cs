using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_BusinessLayer
{
    public static class clsLicensesBusiness
    {

        public static int IssueDrivingLicense(int ApplicationID, int DriverID, int LicenseClassID, DateTime IssueDate, DateTime ExpirationDate, string Notes, decimal PaidFees, bool IsActive, byte IssueReason, int CreatedByUserID)
        {

            return clsLicensesData.IssueDrivingLicense(ApplicationID, DriverID, LicenseClassID, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID);

        }


        public static DataRow GetLicenseRecordUsingApplicationID(int ApplicationID)
        {
            return clsLicensesData.GetLicenseRecordUsingID(ApplicationID);
        }

        public static DataRow GetLicenseRecordUsingLicenseID(int LicenseID)
        {
            return clsLicensesData.GetLicenseRecordUsingLicenseID(LicenseID);
        }



        public static DataTable GetDriverLicenses(int DriverID)
        {
            return clsLicensesData.GetDriverLicenses(DriverID);

        }


        public static int GetApplicationIDUsingLicenseID(int LicenseID)
        {
            return clsLicensesData.GetApplicationIDUsingLicenseID(LicenseID);

        }    
        
        public static int GetLicenseIDUsingAppID(int AppID)
        {
            return clsLicensesData.GetLicenseIDUsingAppID(AppID);

        }

        public static int GetUserIDUsingLicenseID(int LicenseID)
        {
            return clsLicensesData.GetUserIDUsingLicenseID(LicenseID);

        }

        public static int GetDriverIDUsingLicenseID(int LicenseID)
        {
            return clsLicensesData.GetDriverIDUsingLicenseID(LicenseID);

        }
        public static int GetLicenseClassIDUsingLicenseID(int LicenseID)
        {
            return clsLicensesData.GetLicenseClassIDUsingLicenseID(LicenseID);

        }
        public static DateTime GetExpirationDateUsingLicenseID(int LicenseID)
        {
            return clsLicensesData.GetExpirationDateUsingLicenseID(LicenseID);

        }


        public static bool IsLicenseExist(int LicenseID)
        {
            return clsLicensesData.IsLicenseExist(LicenseID);
        }        
        
        public static bool IsLicenseActive(int LicenseID)
        {
            return clsLicensesData.IsLicenseActive(LicenseID);
        }


        public static bool DeactivateLicense(int LicenseID)
        {
            return clsLicensesData.DeactivateLicense(LicenseID);
        }





    }
}
