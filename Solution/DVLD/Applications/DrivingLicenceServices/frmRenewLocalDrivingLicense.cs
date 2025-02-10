using DVLD.Login;
using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications.DrivingLicenceServices
{
    public partial class frmRenewLocalDrivingLicense : Form
    {

        int ApplicationTypeID {  get; set; }


        int RenewLicenseApplicationID {  get; set; }

        int RenewedLicenseID { get; set; }  




        public frmRenewLocalDrivingLicense()
        {
            InitializeComponent();
        }


        private void frmRenewLocalDrivingLicense_Load(object sender, EventArgs e)
        {

            ApplicationTypeID = 2; // Renew
            

            linkLabel1.Enabled = false;
            linkLabel2.Enabled = false;
            btnRenew.Enabled = false;

            ctrlNewLicenseInfo1.ApplicationTypeID = ApplicationTypeID;
            ctrlNewLicenseInfo1.FirstLodedData();


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
                btnRenew.Enabled = true;
                ctrlNewLicenseInfo1.OldLicenseID = OldLicenseID;
                ctrlNewLicenseInfo1.SecondLodedData();

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



        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int OldLicenseID = int.Parse(maskedTextBox1.Text); 
            int ApplicationID = clsLicensesBusiness.GetApplicationIDUsingLicenseID(OldLicenseID);
            int PersonID = clsManageApplicationsBusiness.GetPersonIDUsingApplicationID(ApplicationID);
            string NationalNo = clsPersonBusiness.GetPersonNationalNoUsingPerosnID(PersonID);

            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(NationalNo);
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
            ctrlNewLicenseInfo1.ClearSecondAndThirdLodedData();


        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            // Show New License Info
            // We Need LocalDrivingLicenseApplication you have License ID => RenewedLicenseID and ApplicationID => RenewLicenseApplicationID

            frmShowLicenseInfo frm = new frmShowLicenseInfo(RenewedLicenseID);
            frm.ShowDialog();


        }

        private void btnRenew_Click(object sender, EventArgs e)
        {
            int OldLicenseID = int.Parse(maskedTextBox1.Text);


            bool IsLicenseActive = clsLicensesBusiness.IsLicenseActive(OldLicenseID);

            if (IsLicenseActive)
            {


                DateTime LicenseExpirationDate = clsLicensesBusiness.GetExpirationDateUsingLicenseID(OldLicenseID);
                DateTime CurrentDate = DateTime.Now;

                if (CurrentDate > LicenseExpirationDate)
                {
                    // Handle Renew License 
                    HandleRenewLicenseProcess();
                    FilterBox.Enabled = false;
                    linkLabel2.Enabled = true;
                    btnRenew.Enabled= false;
                    


                }
                else
                {
                    MessageBox.Show("You Can't Renew This License, It Has Not Expired Yet", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

            } 
            else
            {
                    MessageBox.Show("You Can't Renew This License, It Is Not Active", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }



        }


        private void HandleRenewLicenseProcess()
        {
            // 1- First Make Application Of Type Renew

            MakeApplication();

            // 2- Make License For That Application 
            RenewLicense();

          

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
            int ApplicationTypeID = this.ApplicationTypeID; // Renew
            // (4) ApplicationStatus 
            byte ApplicationStatus = 3; // Completed
            // (5) LastStatusDate
            DateTime LastStatusDate = DateTime.Now;
            // (6) Fees depend on type
            decimal Fees = clsManageApplicationTypesBusiness.GetApplicationFees(ApplicationTypeID);

            // (7) User ID 
            int UserID = clsUserBusiness.FindUserIDUsingPasswordAndUserName(clsGlobalSettings.UserName, clsGlobalSettings.Password);

            // Make Application Of Type Renew
            int RenewLicenseApplicationID = clsManageApplicationsBusiness.MakeApplicationAndReturnApplicationID(PerosnID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate, Fees, UserID);


            if (RenewLicenseApplicationID != -1)
            {
                MessageBox.Show($"Application Of Type Renew License Created Successfull With ID {RenewLicenseApplicationID}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.RenewLicenseApplicationID = RenewLicenseApplicationID;
            }
            else
            {
                MessageBox.Show("Faliled To Create Application Of Type Renew License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RenewLicense()
        {

            // 1- ApplicationID
            int ApplicationID = RenewLicenseApplicationID;

            // 2- DriverID
            int LicenseID = int.Parse(maskedTextBox1.Text);
            int DriverID = clsLicensesBusiness.GetDriverIDUsingLicenseID(LicenseID);

            // 3- LicenseClass
            int LicenseClass = clsLicensesBusiness.GetLicenseClassIDUsingLicenseID(LicenseID);

            // 4- IssueDte
            DateTime IssueDate = DateTime.Now;

            // 5- ExpirationDate 
            byte ValidityLength = clsLicenseClassesBusiness.GetLicenseDefaulltValidityLength(LicenseClass);
            DateTime ExpirationDate = IssueDate.AddYears(ValidityLength);

            // 6- Notes
            string Notes = ctrlNewLicenseInfo1.TextBoxInput();

            // 7- Paid Fees
            decimal PaidFees = clsLicenseClassesBusiness.GetLicenseClassFeesUsingLicenseClassID(LicenseClass);

            // 8- IsActive 
            bool IsActive = true;

            // 9- IssueReason
            byte IssueReason = 2; // Renew

            // 10- userId
            int CreatedByUserID = clsUserBusiness.FindUserIDUsingPasswordAndUserName(clsGlobalSettings.UserName, clsGlobalSettings.Password);


            // Renew License
            int RenewedLicenseID = clsLicensesBusiness.IssueDrivingLicense(ApplicationID, DriverID, LicenseClass, IssueDate, ExpirationDate, Notes, PaidFees, IsActive, IssueReason, CreatedByUserID);


            if (RenewedLicenseID != -1)
            {
                MessageBox.Show($"License Renewed Successfull With ID {RenewedLicenseID}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                this.RenewedLicenseID = RenewedLicenseID;

                // Chage Old License Active
                ctrlNewLicenseInfo1.ApplicationTypeID = this.ApplicationTypeID;
                ctrlNewLicenseInfo1.RenewedLicenseID = this.RenewedLicenseID;
                ctrlNewLicenseInfo1.ThirdLodedData();
                DeactivateOldLicense();

            }
            else
            {
                MessageBox.Show("Faliled To Renew License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }


        }




        private void DeactivateOldLicense()
        {
            int LicenseID = int.Parse(maskedTextBox1.Text);
            if (clsLicensesBusiness.DeactivateLicense(LicenseID))
            {
                MessageBox.Show("Old License Deactivated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            } else
            {
                MessageBox.Show("Failed To Deactivate Old License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }






    }
}
