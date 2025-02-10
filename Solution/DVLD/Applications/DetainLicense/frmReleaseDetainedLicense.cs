using DVLD.Applications.DrivingLicenceServices;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DVLD.Applications.DetainLicense
{
    public partial class frmReleaseDetainedLicense : Form
    {

        int ApplicationTypeID { get; set; }

        int ApplicationIDOfTypeReleaseLicense { get; set; }

        int LicenseID { get; set; }

        public frmReleaseDetainedLicense(int LicenseID)
        {
            InitializeComponent();

            this.LicenseID = LicenseID; 

            ApplicationTypeID = 5; //Detain Licensea

            linkLabel1.Enabled = false;
            linkLabel2.Enabled = false;
            btnRelease.Enabled = false;

        }

        private void frmReleaseDetainedLicense_Load(object sender, EventArgs e)
        {
            FirstLodedData();

            if (LicenseID != -1)
            {
                maskedTextBox1.Text = LicenseID.ToString();
                FilterBox.Enabled = false;
                linkLabel1.Enabled = true;
                btnRelease.Enabled = true;
                HanldeDriverLicneseInfo(LicenseID);
                SecondLodedData();
            }

        }


        private void FirstLodedData()
        {
            // NO Data To Load in This Form
        }

        public void SecondLodedData()
        {
            int LicenseID = int.Parse(maskedTextBox1.Text);

            lblLicenseID.Text = LicenseID.ToString();

            DataRow DetaiendLicenseRecord = clsDetainedLicensesBusiness.GetDetainedLicenseRecordUsingLicenseID(LicenseID);

            int DetainID = (int)DetaiendLicenseRecord["DetainID"];
            lblDetainID.Text = DetainID.ToString();

            DateTime DetainDate = (DateTime)DetaiendLicenseRecord["DetainDate"];
            lblDetainDate.Text = DetainDate.ToString();

            int UserID = (int)DetaiendLicenseRecord["CreatedByUserID"];
            string UserName = clsUserBusiness.FindUserNameUsingUserID(UserID);
            lblCreatedBy.Text = UserName;

            decimal FineFees = (decimal)DetaiendLicenseRecord["FineFees"];
            lblFineFees.Text = FineFees.ToString();

            decimal ApplicationFees = clsManageApplicationTypesBusiness.GetApplicationFees(ApplicationTypeID);
            lblAppFees.Text = ApplicationFees.ToString();

            decimal TotalFees = FineFees + ApplicationFees;
            lblTotalFees.Text = TotalFees.ToString();

        }


        public void ThirdLodedData()
        {

            lblAppID.Text = ApplicationIDOfTypeReleaseLicense.ToString();
        }

        private void ClearSecondAndThirdLodedData()
        {

            lblLicenseID.Text = "????";

            lblDetainID.Text = "????";

            lblDetainDate.Text = "????";

            lblCreatedBy.Text = "????";

            lblFineFees.Text = "$$$$";

            lblAppFees.Text = "$$$$";

            lblTotalFees.Text = "$$$$";

            lblAppID.Text = "$$$$";

        }

        private void btnSearch_Click_1(object sender, EventArgs e)
        {
            // check if licese is Detained

            int LicenseID = int.Parse(maskedTextBox1.Text);

            HanldeDriverLicneseInfo(LicenseID);


        }

        private void HanldeDriverLicneseInfo(int LicenseID)
        {
            ctrlDriverLicenseInfo1.LicenseID = LicenseID;
            ctrlDriverLicenseInfo1.LoadDriverLicenseInformation();

            if (ctrlDriverLicenseInfo1.LicenseExist)
            {
                bool IsLicenseDetained = clsDetainedLicensesBusiness.IsLicenseExistInDetainedLicensesList(LicenseID);

                if (IsLicenseDetained)
                {
                    linkLabel1.Enabled = true;
                    btnRelease.Enabled = true;

                    SecondLodedData();
                }
                else
                {
                    MessageBox.Show("You Can't Release This License, Cause It's Not Detained", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

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
            int LicenseID = int.Parse(maskedTextBox1.Text);
            int ApplicationID = clsLicensesBusiness.GetApplicationIDUsingLicenseID(LicenseID);
            int PersonID = clsManageApplicationsBusiness.GetPersonIDUsingApplicationID(ApplicationID);
            string NationalNo = clsPersonBusiness.GetPersonNationalNoUsingPerosnID(PersonID);

            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(NationalNo);
            frm.ShowDialog();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int LicenseID = int.Parse(maskedTextBox1.Text);

            frmShowLicenseInfo frm = new frmShowLicenseInfo(LicenseID);
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
            ctrlDriverLicenseInfo1.ResetDriverLicenseInformation();

        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            int LicenseID = int.Parse(maskedTextBox1.Text);

            bool result = clsDetainedLicensesBusiness.GetReleasedField(LicenseID);
            // result = 1 this Mens That License Released 
            // result = 0 License Is Not Released
            if (result)
            {
                MessageBox.Show("This License Released Once Before", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else
            {
                if (MessageBox.Show("Are You Sure You Want To Release This License", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
                {


                    HandleReleaseLicenseProcess();
                    ThirdLodedData();
                    FilterBox.Enabled = false;
                    linkLabel2.Enabled = true;
                    btnRelease.Enabled = false;

                }


            }


        }


        private void HandleReleaseLicenseProcess()
        {

            MakeApplicationOfTypeReleaseLicense();

            ReleaseLicenseProcess();
        }

        private void MakeApplicationOfTypeReleaseLicense()
        {
            // (1) PersonID
            int LicenseID = int.Parse(maskedTextBox1.Text);
            int ApplicationID = clsLicensesBusiness.GetApplicationIDUsingLicenseID(LicenseID);
            int PerosnID = clsManageApplicationsBusiness.GetPersonIDUsingApplicationID(ApplicationID);
            // (2) Application Date
            DateTime ApplicationDate = DateTime.Now;
            // (3) ApplictationTypeID 
            int ApplicationTypeID = this.ApplicationTypeID; // Release Detained License
            // (4) ApplicationStatus 
            byte ApplicationStatus = 3; // Completed
            // (5) LastStatusDate
            DateTime LastStatusDate = DateTime.Now;
            // (6) Fees depend on type
            decimal Fees = clsManageApplicationTypesBusiness.GetApplicationFees(ApplicationTypeID);

            // (7) User ID 
            int UserID = clsUserBusiness.FindUserIDUsingPasswordAndUserName(clsGlobalSettings.UserName, clsGlobalSettings.Password);

            // Make Application Of Type Damaged Or Lost 
            ApplicationIDOfTypeReleaseLicense = clsManageApplicationsBusiness.MakeApplicationAndReturnApplicationID(PerosnID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate, Fees, UserID);



            if (ApplicationIDOfTypeReleaseLicense != -1)
            {
 

                MessageBox.Show($"Application Of Type Release Detained License Created Successfull With ID {ApplicationIDOfTypeReleaseLicense}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Faliled To Create Application Of Type Release Detained License ", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ReleaseLicenseProcess()
        {



            // 1- License ID
            int LicenseID = int.Parse(maskedTextBox1.Text);

            // 2- DetainDate

            // 3- FineFees

            // 4- UserID

            // 5- IsReleased => 1

            // 6- Realease Date 
            DateTime ReleaseDate = DateTime.Now;

            // 7-Released By UserID
            int ReleasedByUserID = clsUserBusiness.FindUserIDUsingPasswordAndUserName(clsGlobalSettings.UserName, clsGlobalSettings.Password);



            // 8- ReleaseApplicationID 
            int ReleaseApplicationID = this.ApplicationIDOfTypeReleaseLicense;

            // Detain License
            bool Result = clsDetainedLicensesBusiness.ReleaseDetainedLicense(LicenseID, ReleaseDate, ReleasedByUserID, ReleaseApplicationID);

            if (Result)
            {
                MessageBox.Show($"License Released", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show($"Failed To Release License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }



    }
}
