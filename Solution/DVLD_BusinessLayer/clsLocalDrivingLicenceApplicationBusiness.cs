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
    public class clsLocalDrivingLicenceApplicationBusiness
    {

        public static bool IsPersonHasTheSpecifiedClassAndStatusIsNewOrCompleted(string NationalNo, string SpecifiedClassName)
        {
            return clsLocalDrivingLicenceApplicationData.IsPersonHasTheSpecifiedClassAndStatusIsNewOrCompleted (NationalNo, SpecifiedClassName);
        }

        public static int MakeALocalDrivingLicenseApplication(int ApplicationID, int LicenseClassID)
        {
            return clsLocalDrivingLicenceApplicationData.MakeALocalDrivingLicenseApplication(ApplicationID, LicenseClassID);
        }

        public static bool UpdateLocalDrivingLicenseApplication(int LocalDrivingLicenceApplicationID, int LicenseClassID)
        {
            return clsLocalDrivingLicenceApplicationData.UpdateLocalDrivingLicenseApplication(LocalDrivingLicenceApplicationID, LicenseClassID);
        }

        public static DataTable ListLocalDrivingLicenseApplications()
        {
            return clsLocalDrivingLicenceApplicationData.ListLocalDrivingLicenseApplications();
        }



        public static DataTable FilterLocalDrivingLicenseApplicationsByID(int LocalDrivingLicenseApplicationID)
        {
            return clsLocalDrivingLicenceApplicationData.FilterLocalDrivingLicenseApplicationsByID(LocalDrivingLicenseApplicationID);
        }

        public static DataTable FilterLocalDrivingLicenseApplicationsByNationalNo(string NationalNo)
        {
            return clsLocalDrivingLicenceApplicationData.FilterLocalDrivingLicenseApplicationsByNationalNo(NationalNo);
        }

        public static DataTable FilterLocalDrivingLicenseApplicationsByFullName(string FullName)
        {
            return clsLocalDrivingLicenceApplicationData.FilterLocalDrivingLicenseApplicationsByFullName(FullName);
        }

        public static DataTable FilterLocalDrivingLicenseApplicationsByStatus(string Status)
        {
            return clsLocalDrivingLicenceApplicationData.FilterLocalDrivingLicenseApplicationsByStatus(Status);
        }




        public static int GetApplicationIDUsingLDLAppID(int LocalDrivingLicenseApplicationID)
        {
            return clsLocalDrivingLicenceApplicationData.GetApplicationIDUsingLDLAppID(LocalDrivingLicenseApplicationID);
        }      

        public static int GetLicenseClassIDUsingLDLAppID(int LocalDrivingLicenseApplicationID)
        {
            return clsLocalDrivingLicenceApplicationData.GetLicenseClassIDUsingLDLAppID(LocalDrivingLicenseApplicationID);
        }      
        public static int GetLDLAppIDUsingAppID(int AppID)
        {
            return clsLocalDrivingLicenceApplicationData.GetLDLAppIDUsingAppID(AppID);
        }      
        
        public static bool DeleteLocalDrivingLicenseApplication(int LocalDrivingLicenseApplicationID)
        {
            return clsLocalDrivingLicenceApplicationData.DeleteLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID);
        }





    }
}
