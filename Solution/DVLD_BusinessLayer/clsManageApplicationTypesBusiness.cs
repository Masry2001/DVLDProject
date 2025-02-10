using DVLD_DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_BusinessLayer
{
    public class clsManageApplicationTypesBusiness
    {

        public int ApplicationTypeID { get; set; }

        public string ApplicationTypeTitle { get; set; }

        public decimal ApplicationTypeFees { get; set; }



        public static DataTable ListApplicationTypes()
        {
            return clsManageApplicationTypesData.ListApplicationTypes();
        }


        public static clsManageApplicationTypesBusiness FindApplicationType(int ApplicationTypeID)
        {
            DataRow dr = clsManageApplicationTypesData.FindApplicationType(ApplicationTypeID);

            clsManageApplicationTypesBusiness ApplicationType = new clsManageApplicationTypesBusiness();

            ApplicationType.ApplicationTypeID = ApplicationTypeID;
            ApplicationType.ApplicationTypeTitle = (string) dr["ApplicationTypeTitle"];
            ApplicationType.ApplicationTypeFees = (decimal)dr["ApplicationFees"];

            return ApplicationType;
        }

        public static int FindApplicationTypeIDUsingApplicationTypeTitle(string ApplicationTypeTitle)
        {
            return clsManageApplicationTypesData.FindApplicationTypeIDUsingApplicationTypeTitle(ApplicationTypeTitle);
        }

        public static decimal GetApplicationFees(int ApplicationTypeID)
        {
            return clsManageApplicationTypesData.GetApplicationFees(ApplicationTypeID);
        }

        public static decimal GetApplicationFees(string ApplicationTypeTitle)
        {
            return clsManageApplicationTypesData.GetApplicationFees(ApplicationTypeTitle);
        }




        private bool _UpdateApplication()
        {
            return clsManageApplicationTypesData.UpdateApplication(ApplicationTypeID, ApplicationTypeTitle, ApplicationTypeFees);
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



    }
}
