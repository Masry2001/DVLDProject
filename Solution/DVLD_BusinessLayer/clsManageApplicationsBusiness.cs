using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_BusinessLayer
{
    public class clsManageApplicationsBusiness
    {

        public static int MakeApplicationAndReturnApplicationID(int ApplicantPersonID, DateTime ApplicationDate,
            int ApplicationTypeID, int ApplicationStatus, DateTime LastStatusDate, decimal PaidFees, int CreatedByUserID)
        {
            return clsManageApplicationsData.MakeApplicationAndReturnApplicationID(ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID);
        }

        public static bool UpdateApplication(int ApplicationID, int ApplicantPersonID)
        {
            return clsManageApplicationsData.UpdateApplication(ApplicationID, ApplicantPersonID);
        }


        public static bool DeleteApplication(int ApplicationID)
        {
            return clsManageApplicationsData.DeleteApplication(ApplicationID);
        }

        public static int GetPersonIDUsingApplicationID(int ApplicationID)
        {
            return clsManageApplicationsData.GetPersonIDUsingApplicationID(ApplicationID);
        }
        public static int GetApplicationTypeIDUsingApplicationID(int ApplicationID)
        {
            return clsManageApplicationsData.GetApplicationTypeIDUsingApplicationID(ApplicationID);
        }

        public static DateTime GetDateUsingApplicationID(int ApplicationID)
        {
            return clsManageApplicationsData.GetDateUsingApplicationID(ApplicationID);
        }

        public static DataRow GetApplicationRecordUsingID(int ApplicationID)
        {
            return clsManageApplicationsData.GetApplicationRecordUsingID(ApplicationID);
        }

        public static bool SaveTestAppointmentIDAndRetakeTestApplicationID(int TestAppointmentID, int RetakeTestApplicationID)
        {
            return clsManageApplicationsData.SaveTestAppointmentIDAndRetakeTestApplicationID(TestAppointmentID, RetakeTestApplicationID);
        }

        public static int GetRetakeTestApplicationID(int TestAppointmentID)
        {
            return clsManageApplicationsData.GetRetakeTestApplicationID(TestAppointmentID);
        }

        public static bool UpdateApplicationStatus(int ApplicationID, int ApplicationStatus)
        {
            return clsManageApplicationsData.UpdateApplicationStatus(ApplicationID, ApplicationStatus);
        }

        public static bool UpdateApplicationLastStatusDate(int ApplicationID, DateTime Date)
        {
            return clsManageApplicationsData.UpdateApplicationLastStatusDate(ApplicationID, Date);
        }


    }
}
