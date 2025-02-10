using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public static class clsLicenseClassesBusiness
    {

        public static int FindLicenseClassIDUsingLicenceClassName(string ClassName)
        {
            return clsLicenseClassesData.FindLicenseClassIDUsingLicenceClassName(ClassName);
        }

        public static decimal GetLicenseClassFeesUsingLicenseClassID(int LicenseClassID)
        {
            return clsLicenseClassesData.GetLicenseClassFeesUsingLicenseClassID(LicenseClassID);
        }



        public static string FindLicenseClassNameUsingLicenceClassID(int LicenceClassID)
        {
            return clsLicenseClassesData.FindLicenseClassNameUsingLicenceClassID(LicenceClassID);
        }


        public static byte GetLicenseDefaulltValidityLength(int LicenseClassID)
        {
            return clsLicenseClassesData.GetLicenseDefaulltValidityLength(LicenseClassID);

        }

    }
}
