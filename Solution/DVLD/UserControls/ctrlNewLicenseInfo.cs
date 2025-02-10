using DVLD.Login;
using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.UserControls
{
    public partial class ctrlNewLicenseInfo : UserControl
    {

        public int ApplicationTypeID { get; set; }

        public int OldLicenseID { get; set; }

        public int RenewedLicenseID { get; set; }


        public ctrlNewLicenseInfo()
        {
            InitializeComponent();
        }

        private void ctrlNewLicenseInfo_Load(object sender, EventArgs e)
        {

        }

        public void FirstLodedData()
        {

            // Depend on ApplicationTypeID And Current User Loged on The System
            // 1- ApplicationFees
            // 2- Created By

            int UserID = clsUserBusiness.FindUserIDUsingPasswordAndUserName(clsGlobalSettings.UserName, clsGlobalSettings.Password);
            decimal ApplicationFees = clsManageApplicationTypesBusiness.GetApplicationFees(ApplicationTypeID);
            lblAppFees.Text = ApplicationFees.ToString();
            lblCreatedBy.Text = UserID.ToString();


        }

        public void SecondLodedData()
        {
            // Depend on Old Licene ID
            // 1- OldLicenseID
            // 2- LicenseFees => depend On License Class So We Need To Get License Class ID First
            // 3- TotalFees
            lblOldLID.Text = OldLicenseID.ToString();
            int LicenseClassID = clsLicensesBusiness.GetLicenseClassIDUsingLicenseID(OldLicenseID);

            decimal LicenseFees = clsLicenseClassesBusiness.GetLicenseClassFeesUsingLicenseClassID(LicenseClassID);

            lblLicenseFees.Text = LicenseFees.ToString();
            lblTotalFees.Text = (LicenseFees + decimal.Parse(lblAppFees.Text)).ToString();



        }

        public void ThirdLodedData()
        {

            // Depend on Renewed License ID
            // 1- R.L.AppID
            // 2- R.L.ID
            // 3- ApplicationDate
            // 4- Issue Date
            // 5- Expiration Date
            DataRow LicenseRecord = clsLicensesBusiness.GetLicenseRecordUsingLicenseID(RenewedLicenseID);

            lblRLAppID.Text = LicenseRecord["ApplicationID"].ToString();

            lblRLID.Text = RenewedLicenseID.ToString();

            int ApplicationID = (int)LicenseRecord["ApplicationID"];
            lblAppDate.Text = clsManageApplicationsBusiness.GetDateUsingApplicationID(ApplicationID).ToString("dd/MM/yyyy");

            lblIssueDate.Text = ((DateTime)LicenseRecord["IssueDate"]).ToString("dd/MM/yyyy");
            lblExpirationDate.Text = ((DateTime)LicenseRecord["ExpirationDate"]).ToString("dd/MM/yyyy");

        }

        public void ClearSecondAndThirdLodedData()
        {
            lblOldLID.Text = "????";
            lblLicenseFees.Text = "$$$$";
            lblTotalFees.Text = "$$$$";


            lblRLAppID.Text = "????";

            lblRLID.Text = "????";

            lblAppDate.Text = "????";

            lblIssueDate.Text = "????";
            lblExpirationDate.Text = "????";

        }


        public string TextBoxInput( )
        {
            string Notes = textBox1.Text;
            return Notes;
        }





    }
}
