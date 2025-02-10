using DVLD.Login;
using DVLD.UserControls;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace DVLD.Applications.DrivingLicenceServices
{
    public partial class frmReplacementForLostOrDamagedLicense : Form
    {


        int ApplicationTypeID { get; set; }


        int ReplacedLicenseApplicationID { get; set; }

        int ReplacedLicenseID { get; set; }



        public frmReplacementForLostOrDamagedLicense()
        {
            InitializeComponent();

            ApplicationTypeID = 4; //Damaged

            linkLabel1.Enabled = false;
            linkLabel2.Enabled = false;
            btnReplace.Enabled = false;


        }

        private void frmReplacementForLostOrDamagedLicense_Load(object sender, EventArgs e)
        {
            FirstLodedData();

        }


        private void FirstLodedData()
        {
            lblAppDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            int UserID = clsUserBusiness.FindUserIDUsingPasswordAndUserName(clsGlobalSettings.UserName, clsGlobalSettings.Password);
            HandleApplicationFees();
            lblCreatedBy.Text = UserID.ToString();
        }

        public void SecondLodedData()
        {
            int OldLicenseID = int.Parse(maskedTextBox1.Text);

            lblOldLID.Text = OldLicenseID.ToString();

        }

        public void ThirdLodedData()
        {

            // Depend on Replaced License ID
            // 1- R.L.AppID
            // 2- R.L.ID
            // 3- ApplicationDate

            lblRLAppID.Text = ReplacedLicenseApplicationID.ToString();

            lblRLID.Text = ReplacedLicenseID.ToString();

            lblAppDate.Text = clsManageApplicationsBusiness.GetDateUsingApplicationID(ReplacedLicenseApplicationID).ToString("dd/MM/yyyy");


        }

        private void ClearSecondAndThirdLodedData()
        {
            lblOldLID.Text = "????";

            lblRLAppID.Text = "????";

            lblRLID.Text = "????";

        }

        private void btnSearch_Click(object sender, EventArgs e)
        {
            int OldLicenseID = int.Parse(maskedTextBox1.Text);
            HanldeDriverLicneseInfo(OldLicenseID);
        }

        private void HanldeDriverLicneseInfo(int OldLicenseID)
        {
            ctrlDriverLicenseInfo1.LicenseID = OldLicenseID;
            ctrlDriverLicenseInfo1.LoadDriverLicenseInformation();
            if (ctrlDriverLicenseInfo1.LicenseExist)
            {
                linkLabel1.Enabled = true;
                btnReplace.Enabled = true;

                SecondLodedData();

            }
        }

        private void maskedTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only numbers and backspace; disallow spaces and letters
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void linkLabel1_LinkClicked(object sender, EventArgs e)
        {
            int OldLicenseID = int.Parse(maskedTextBox1.Text);
            int ApplicationID = clsLicensesBusiness.GetApplicationIDUsingLicenseID(OldLicenseID);
            int PersonID = clsManageApplicationsBusiness.GetPersonIDUsingApplicationID(ApplicationID);
            string NationalNo = clsPersonBusiness.GetPersonNationalNoUsingPerosnID(PersonID);

            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(NationalNo);
            frm.ShowDialog();
        }


        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Show New License Info
            // We Need LocalDrivingLicenseApplication you have License ID => RenewedLicenseID and ApplicationID => RenewLicenseApplicationID

            frmShowLicenseInfo frm = new frmShowLicenseInfo(ReplacedLicenseID);
            frm.ShowDialog();


        }

        private void maskedTextBox1_Validating(object sender, CancelEventArgs e)
        {

            // Check if the active control is the close button
            if (this.ActiveControl == btnClose)
            {
                e.Cancel = false; // Allow the form to close without validation
                return;
            }

            if (string.IsNullOrWhiteSpace(maskedTextBox1.Text))
            {
                errorProvider1.SetError(maskedTextBox1, "This field is required.");
                e.Cancel = true; // Prevent the user from leaving the TextBox
            }
            else
            {
                errorProvider1.SetError(maskedTextBox1, string.Empty);
            }
        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            linkLabel1.Enabled = false;
            ClearSecondAndThirdLodedData();


        }




        private void btnReplace_Click(object sender, EventArgs e)
        {
            int OldLicenseID = int.Parse(maskedTextBox1.Text);


            bool IsLicenseActive = clsLicensesBusiness.IsLicenseActive(OldLicenseID);

            if (IsLicenseActive)
            {


                DateTime LicenseExpirationDate = clsLicensesBusiness.GetExpirationDateUsingLicenseID(OldLicenseID);
                DateTime CurrentDate = DateTime.Now;

                if (CurrentDate > LicenseExpirationDate) // This Means That License Is Expired
                {

                    MessageBox.Show("You Can't Replace This License, It's Expired", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }
                else
                {
                    // Handle Replace License 
                    HandleReplaceLicenseProcess();

                    FilterBox.Enabled = false;
                    linkLabel2.Enabled = true;
                    btnReplace.Enabled = false;
                    groupBox1.Enabled = false;

                }

            }
            else
            {
                MessageBox.Show("You Can't Replace This License, It Is Not Active", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void HandleReplaceLicenseProcess()
        {
            // 1- First Make Application Of Type Damaged Of Lost

            MakeApplication();

            // 2- Make License For That Application 
            ReplaceLicense();



        }

        private void MakeApplication()
        {
            // (1) PersonID
            int LicenseID = int.Parse(maskedTextBox1.Text);
            int ApplicationID = clsLicensesBusiness.GetApplicationIDUsingLicenseID(LicenseID);
            int PerosnID = clsManageApplicationsBusiness.GetPersonIDUsingApplicationID(ApplicationID);
            // (2) Application Date
            DateTime ApplicationDate = DateTime.Now;
            // (3) ApplictationTypeID 
            int ApplicationTypeID = this.ApplicationTypeID; // Damaged Of Lost
            // (4) ApplicationStatus 
            byte ApplicationStatus = 3; // Completed
            // (5) LastStatusDate
            DateTime LastStatusDate = DateTime.Now;
            // (6) Fees depend on type
            decimal Fees = clsManageApplicationTypesBusiness.GetApplicationFees(ApplicationTypeID);

            // (7) User ID 
            int UserID = clsUserBusiness.FindUserIDUsingPasswordAndUserName(clsGlobalSettings.UserName, clsGlobalSettings.Password);

            // Make Application Of Type Damaged Or Lost 
            int ReplacedLicenseApplicationID = clsManageApplicationsBusiness.MakeApplicationAndReturnApplicationID(PerosnID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate, Fees, UserID);


            string ApplicationName = "";

            if (ReplacedLicenseApplicationID != -1)
            {
                if (ApplicationTypeID == 4)
                {
                    ApplicationName = "Damaged License";
                }
                else if (ApplicationTypeID == 3)
                {
                    ApplicationName = "Lost License";
                }

                MessageBox.Show($"Application Of Type {ApplicationName} Created Successfull With ID {ReplacedLicenseApplicationID}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.ReplacedLicenseApplicationID = ReplacedLicenseApplicationID;
            }
            else
            {
                MessageBox.Show($"Faliled To Create Application Of Type{ApplicationName} ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void ReplaceLicense()
        {
            int LicenseID = int.Parse(maskedTextBox1.Text);
            DataRow LicenseRecord = clsLicensesBusiness.GetLicenseRecordUsingLicenseID(LicenseID);

            // 1- ApplicationID
            int ApplicationID = ReplacedLicenseApplicationID;

            // 2- DriverID
            int DriverID = (int)LicenseRecord["DriverID"];

            // 3- LicenseClass
            int LicenseClass = (int)LicenseRecord["LicenseClass"];

            // 4- IssueDte
            DateTime IssueDate = (DateTime)LicenseRecord["IssueDate"];

            // 5- ExpirationDate 
            DateTime ExpirationDate = (DateTime)LicenseRecord["ExpirationDate"];

            // 6- Notes
            string Notes = LicenseRecord["Notes"] != DBNull.Value ? (string)LicenseRecord["Notes"] : string.Empty;

            // 7- Paid Fees
            decimal PaidFees = (decimal)LicenseRecord["PaidFees"];

            // 8- IsActive 
            bool IsActive = true;

            // 9- IssueReason
            byte IssueReason = (byte)ApplicationTypeID; // Replacement For Damaged => 3 Or Lost => 4

            // 10- userId
            int CreatedByUserID = clsUserBusiness.FindUserIDUsingPasswordAndUserName(clsGlobalSettings.UserName, clsGlobalSettings.Password);


            // Replace License
            int ReplacedLicenseID = clsLicensesBusiness.IssueDrivingLicense(ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID);


            if (ReplacedLicenseID != -1)
            {
                MessageBox.Show($"License Replaced Successfull With ID {ReplacedLicenseID}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.ReplacedLicenseID = ReplacedLicenseID;

                // Chage Old License Active
                ThirdLodedData();
                DeactivateOldLicense();

            }
            else
            {
                MessageBox.Show("Faliled To Replace License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }

        private void DeactivateOldLicense()
        {
            int LicenseID = int.Parse(maskedTextBox1.Text);
            if (clsLicensesBusiness.DeactivateLicense(LicenseID))
            {
                MessageBox.Show("Old License Deactivated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Failed To Deactivate Old License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }


        private void HandleApplicationFees()
        {
            decimal ApplicationFees = clsManageApplicationTypesBusiness.GetApplicationFees(ApplicationTypeID);
            lblAppFees.Text = ApplicationFees.ToString();
        }

        private void rbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {

            if (rbDamagedLicense.Checked)
            {

                ApplicationTypeID = 4; //Damaged

            }
            else
            {
                ApplicationTypeID = 3; // Lost
            }


            if (rbDamagedLicense.Checked)
            {
                HandleApplicationFees();

            }

        }    
        private void rbLostLicense_CheckedChanged(object sender, EventArgs e)
        {

            if (rbDamagedLicense.Checked)
            {

                ApplicationTypeID = 4; //Damaged

            }
            else
            {
                ApplicationTypeID = 3; // Lost
            }


            if (rbLostLicense.Checked)
            {
                HandleApplicationFees();

            }
        }




    }
}
