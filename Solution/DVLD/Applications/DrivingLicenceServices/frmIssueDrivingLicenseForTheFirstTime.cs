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

namespace DVLD.Applications.DrivingLicenceServices
{
    public partial class frmIssueDrivingLicenseForTheFirstTime : Form
    {


        int LDLAppID { get; set; }


        public frmIssueDrivingLicenseForTheFirstTime(int LocalDrivingLicenceApplicationID)
        {
            InitializeComponent();

            this.LDLAppID = LocalDrivingLicenceApplicationID;


            ctrlLDLAppCard1.LDLAppID = LocalDrivingLicenceApplicationID;

        }

        private void frmIssueDrivingLicenseForTheFirstTime_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            IssueDrivingLicenseForTheFirstTime();
            DisableBtnIssue();
        }

        private void DisableBtnIssue()
        {
            btnIssue.Enabled = false;
            txtNotes.Enabled = false;
        }


        private void IssueDrivingLicenseForTheFirstTime()
        {
            // 1 - ApplicationID
            int ApplicationID = clsLocalDrivingLicenceApplicationBusiness.GetApplicationIDUsingLDLAppID(LDLAppID);


            // 2 - DriverID
            DataRow ApplicationRecord = clsManageApplicationsBusiness.GetApplicationRecordUsingID(ApplicationID);
            int ApplicantPersonID = (int)ApplicationRecord["ApplicantPersonID"];
            int DriverID = HandleGettingDriverID(ApplicantPersonID);



            // 3 - LicenseClassID
            int LicenseClassID = clsLocalDrivingLicenceApplicationBusiness.GetLicenseClassIDUsingLDLAppID(LDLAppID);

            // 4 - Issue Date
            DateTime IssueDate = DateTime.Now;

            // 5 - ExpirationDate
            //GetLicenseDefaulltValidityLength
            byte LicenseValidityLength = clsLicenseClassesBusiness.GetLicenseDefaulltValidityLength(LicenseClassID);
            DateTime ExpirationDate = IssueDate.AddYears(LicenseValidityLength);

            // 6 - Notes
            string Notes = txtNotes.Text; // Handle Null In DataAccess Layer

            // 7 - PaidFees
            decimal PaidFees = 20;

            // 8 - IsActive 
            bool IsActive = true;

            // 9 - IssueReasen
            byte IssueReason = 1; // 1 => New

            // 10 - UserID
            int CreatedByUserID = clsUserBusiness.FindUserIDUsingPasswordAndUserName(clsGlobalSettings.UserName, clsGlobalSettings.Password);


            // Issue License
            int LicenseID = clsLicensesBusiness.IssueDrivingLicense(ApplicationID, DriverID, LicenseClassID, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID);

            if (LicenseID != -1)
            {
                MessageBox.Show($"License Issued With ID: {LicenseID}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                UpdateApplicationStatus(ApplicationID);

            }
            else
            {
                MessageBox.Show($"Failed To Created License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void UpdateApplicationStatus(int ApplicationID)
        {
            // Update Application Status
            int ApplicationStatus = 3; // Completed
            if (clsManageApplicationsBusiness.UpdateApplicationStatus(ApplicationID, ApplicationStatus))
            {
                MessageBox.Show("Application Status Completed", "info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Failed To Update Application Status ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        private int HandleGettingDriverID(int ApplicantPersonID)
        {

            int DriverID = -1;

            bool Result = clsDriverBusiness.IsPersonADriver(ApplicantPersonID);
            if (Result)
            {
                DriverID  = clsDriverBusiness.GetDriverID(ApplicantPersonID);
            } 
            else
            {

                int CreatedByUserID = clsUserBusiness.FindUserIDUsingPasswordAndUserName(clsGlobalSettings.UserName, clsGlobalSettings.Password);
                DateTime CreatedDate = DateTime.Now;
                DriverID = clsDriverBusiness.AddNewDriver(ApplicantPersonID,  CreatedByUserID,  CreatedDate);
            }

            if (DriverID != -1)
            {
                MessageBox.Show($"DriverID Is {DriverID}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Failed To Created Driver", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


            return DriverID;
        }




    }
}
