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
    public class clsTestsBusiness
    {
        public int TestTypeID { get; set; }

        public string TestTypeTitle { get; set; }

        public string TestTypeDescription { get; set; }

        public decimal TestTypeFees { get; set; }

        public static DataTable ListTestTypes()
        {
            return clsTestsData.ListTestTypes();

        }

        public static DataTable ListAppoinmentsForLocalDrivingLicenseApplicationID(int LDLAppID, int TestType)
        {
            return clsTestsData.ListAppoinmentsForLocalDrivingLicenseApplicationID(LDLAppID, TestType);

        }



        public static clsTestsBusiness FindTestType(int TestTypeID)
        {
            DataRow dr = clsTestsData.FindTestType(TestTypeID);

            clsTestsBusiness TestType = new clsTestsBusiness();

            TestType.TestTypeID = TestTypeID;
            TestType.TestTypeTitle = (string)dr["TestTypeTitle"];
            TestType.TestTypeDescription = (string)dr["TestTypeDescription"];
            TestType.TestTypeFees = (decimal)dr["TestTypeFees"];

            return TestType;
        }


        private bool _UpdateApplication()
        {
            return clsTestsData.UpdateTestType(TestTypeID, TestTypeTitle, TestTypeDescription, TestTypeFees);
        }
        public bool Save()
        {
            if (_UpdateApplication())
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        public static int NumberOfPassedTests(int LocalDrivingLicenseApplicationID)
        {
            return clsTestsData.NumberOfPassedTests(LocalDrivingLicenseApplicationID);  
        }
        public static int TrialsOfTestType(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {
            return clsTestsData.TrialsOfTestType(LocalDrivingLicenseApplicationID, TestTypeID);  
        }
        public static int AddTestAppoinment(int TestTypeID, int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, decimal PaidFees, int CreatedByUserID, bool IsLocked)
        {
           return clsTestsData.AddTestAppoinment(TestTypeID, LocalDrivingLicenseApplicationID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked);  
        }

        

        public static bool IsThereIsAnActiveAppointmentForLocalDrivingLicenseApplicationIDAndTestType(int LDLAppID, int TestType)
        {
            return clsTestsData.IsThereIsAnActiveAppointmentForLocalDrivingLicenseApplicationIDAndTestType(LDLAppID, TestType);
        }

        public static bool IsLDLAppIDHasLockedTestAppointment(int LDLAppID, int TestType)
        {
            return clsTestsData.IsLDLAppIDHasLockedTestAppointment(LDLAppID, TestType);
        }



        public static bool EditTestAppointment(int AppointmentID, DateTime AppointmentDate)
        {
            return clsTestsData.EditTestAppointment(AppointmentID, AppointmentDate);
        }


        public static int TakeTest(int TestAppointmentID, bool TestResult, string Notes, int CreatedByUserID)
        {
            return clsTestsData.TakeTest(TestAppointmentID, TestResult, Notes, CreatedByUserID);
        }


        public static bool LockAppointment(int AppointmentID)
        {
            return clsTestsData.LockAppointment(AppointmentID);
        }

        public static bool CheckIfAppointmentIsLocked(int AppointmentID)
        {
            return clsTestsData.CheckIfAppointmentIsLocked(AppointmentID);
        }

        public static DateTime GetAppointmentDate(int AppointmentID) 
        { 
            return clsTestsData.GetAppointmentDate(AppointmentID);
        }

        public static bool IsTestResultPass(int TestID)
        {
            return clsTestsData.IsTestResultPass(TestID);
        }


        public static string GetTestNotes(int TestID)
        {
            return clsTestsData.GetTestNotes(TestID);
        }

        public static int GetTestIDUsingTestAppointmentID(int AppointmentID)
        {
            return clsTestsData.GetTestIDUsingTestAppointmentID(AppointmentID);
        }
        public static int GetTestTypeIDUsingTestAppointmentID(int AppointmentID)
        {
            return clsTestsData.GetTestTypeIDUsingTestAppointmentID(AppointmentID);
        }

        public static int GetLastLockedTestAppointmentID(int LDLAppID, int TestTypeID)
        {
            return clsTestsData.GetLastLockedTestAppointmentID(LDLAppID, TestTypeID);
        }

        public static int GetFirstTestAppointmentID(int LDLAppID, int TestTypeID)
        {
            return clsTestsData.GetFirstTestAppointmentID(LDLAppID, TestTypeID);
        }


        public static bool GetTestResultForTestAppointmentID(int TestAppointmentID)
        {
            return clsTestsData.GetTestResultForTestAppointmentID(TestAppointmentID);
        }


        public static decimal GetTestTypeFees(int TestTypeID)
        {
            return clsTestsData.GetTestTypeFees(TestTypeID);
        }




    }
}
