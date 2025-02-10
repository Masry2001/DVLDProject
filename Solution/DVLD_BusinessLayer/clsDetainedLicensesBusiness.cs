using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public static class clsDetainedLicensesBusiness
    {


        public static bool IsLicenseExistInDetainedLicensesList(int LicenseID)
        {
            return clsDetainedLicensesData.IsLicenseExistInDetainedLicensesList(LicenseID);
        }
        public static bool IsLicenseDetainedAndNotReleased(int LicenseID)
        {
            return clsDetainedLicensesData.IsLicenseDetainedAndNotReleased(LicenseID);
        }

        public static bool GetReleasedField(int LicenseID)
        {
            return clsDetainedLicensesData.GetReleasedField(LicenseID);
        }


        public static int DetainLicense(int LicenseID, DateTime DetainDate, decimal FineFees, int CreatedByUserID)
        {
            return clsDetainedLicensesData.DetainLicense(LicenseID, DetainDate, FineFees, CreatedByUserID);

        }

        public static bool ReleaseDetainedLicense(int LicenseID, DateTime ReleaseDate, int ReleasedByUserID, int ReleaseApplicationID)
        {
            return clsDetainedLicensesData.ReleaseDetainedLicense(LicenseID, ReleaseDate, ReleasedByUserID, ReleaseApplicationID);

        }


        public static DataRow GetDetainedLicenseRecordUsingLicenseID(int LicenseID)
        {

            return clsDetainedLicensesData.GetDetainedLicenseRecordUsingLicenseID(LicenseID);

        }


        public static DataTable ListDetainedLicenses()
        {

            return clsDetainedLicensesData.ListDetainedLicenses();

        }

        public static DataTable FilterDetainedLicenseByDetainID(int DetainID)
        {
            return clsDetainedLicensesData.FilterDetainedLicenseByDetainID(DetainID);
        }
        public static DataTable FilterDetainedLicenseByReleaseApplicationID(int ReleaseApplicationID)
        {
            return clsDetainedLicensesData.FilterDetainedLicenseByReleaseApplicationID(ReleaseApplicationID);
        }
        public static DataTable FilterDetainedLicenseByNationalNo(string NationalNo)
        {
            return clsDetainedLicensesData.FilterDetainedLicenseByNationalNo(NationalNo);
        }
        public static DataTable FilterDetainedLicenseByFullName(string FullName)
        {
            return clsDetainedLicensesData.FilterDetainedLicenseByFullName(FullName);
        }
        public static DataTable FilterDetainedLicenseByIsReleased(bool IsReleased)
        {
            return clsDetainedLicensesData.FilterDetainedLicenseByIsReleased(IsReleased);
        }



    }
}
