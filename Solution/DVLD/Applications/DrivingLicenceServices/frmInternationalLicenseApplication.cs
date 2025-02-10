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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD.Applications.DrivingLicenceServices
{
    public partial class frmInternationalLicenseApplication : Form
    {


        int InternationalLicenseApplicationID {  get; set; }

        int InternationalLicenseID { get; set; }


        public frmInternationalLicenseApplication()
        {
            InitializeComponent();
        }


        private void btnSearch_Click(object sender, EventArgs e)
        {

            int LicenseID = int.Parse(maskedTextBox1.Text);
            HanldeDriverLicneseInfo(LicenseID);



        }


        private void HanldeDriverLicneseInfo(int LicenseID)
        {
            ctrlDriverLicenseInfo1.LicenseID = LicenseID;
            ctrlDriverLicenseInfo1.LoadDriverLicenseInformation();
            if (ctrlDriverLicenseInfo1.LicenseExist)
            {
                linkLabel1.Enabled = true;

            }
        }

        private void btnIssue_Click(object sender, EventArgs e)
        {
            int LicenseID = int.Parse(maskedTextBox1.Text);


            if (clsInternationalLicensesBusiness.IsThereInternationalLicenseIsuuedUsingLocalLicense(LicenseID))
            {

                MessageBox.Show("There Is InternationalLicense Isuued Using This Local License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            } else
            {

                HanldeApplicationInfo(LicenseID);



            }

        }


        private void HanldeApplicationInfo(int LicenseID)
        {
            ctrlApplicationInfo1.LicenseID = LicenseID;
            ctrlApplicationInfo1.FillLocalLicenseData();

            FillInternattionalLicenseData(LicenseID);
        }



        private void FillInternattionalLicenseData(int LicenseID)
        {

            // 1- Get License ClassID must be of class 3
            DataRow LicenseRecord = clsLicensesBusiness.GetLicenseRecordUsingLicenseID(LicenseID);

            int LicenseClassID = (int)LicenseRecord["LicenseClass"];

            // 2- Is Active
            bool IsLicenseActive = (bool)LicenseRecord["IsActive"];

            // 3- Not Expired
            DateTime ExpirationDate = (DateTime)LicenseRecord["ExpirationDate"];
            DateTime CurrentDate = DateTime.Now;
            bool IsLicenseExpired = CurrentDate > ExpirationDate;


            // 4- Not Detained
            bool IsLicenseDetained = clsDetainedLicensesBusiness.IsLicenseExistInDetainedLicensesList(LicenseID);

            if (LicenseClassID == 3)
            {

                if (IsLicenseActive) 
                {

                    if (!IsLicenseExpired)
                    {

                       if (!IsLicenseDetained )
                        {

                            CreateInternaionalLicense();

                        } 
                        else
                        {
                            MessageBox.Show("License Is Detained", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                        }

                    } 
                    else
                    {
                        MessageBox.Show("License Is Expired", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }

                } 
                else
                {
                    MessageBox.Show("License Must Be Active", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                }

            }
            else
            {
                MessageBox.Show("License Class Must Be Of Class 3", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }


        private void CreateInternaionalLicense()
        {



            // 1- Make Application of Type 6 International License

            MakeApplication();




            // 2- Make Internaional License
            MakeInternationalLicense();



            // 3- After Creating License Send InternationalLicenseID And Application ID of Type Internainal License ID To ctrlApplicationInfo

            ctrlApplicationInfo1.InternationalLicenseID = this.InternationalLicenseID;
            ctrlApplicationInfo1.InternationalLicenseApplicationID = this.InternationalLicenseApplicationID;
            ctrlApplicationInfo1.FillInternationalLicenseData();

            linkLabel2.Enabled = true;

            btnIssue.Enabled = false;
            FilterBox.Enabled = false;



        }


        private void MakeInternationalLicense()
        {
            // 1- ApplicationID
            int LicenseID = int.Parse(maskedTextBox1.Text);// 3- IssuedUsingLocalLicenseID

            int ApplicationID = clsLicensesBusiness.GetApplicationIDUsingLicenseID(LicenseID);

            // 2- DriverID
            int DriverID = clsLicensesBusiness.GetDriverIDUsingLicenseID(LicenseID);

            // 4- IssueDate
            DateTime IssueDate = DateTime.Now;

            // 5- ExpirationDate 
            int LicenseClassID = 3;// Ordinary Driving License
            byte DefaultValidityLength = clsLicenseClassesBusiness.GetLicenseDefaulltValidityLength(LicenseClassID);
            DateTime ExpirationDate = IssueDate.AddYears(DefaultValidityLength);

            // 6- IsActive
            bool IsActive = true;

            // 7- UserID
            int UserID = clsUserBusiness.FindUserIDUsingPasswordAndUserName(clsGlobalSettings.UserName, clsGlobalSettings.Password);

            // Make Internaional License

            int InternationalLicenseID = clsInternationalLicensesBusiness.MakeInternaionalLicenseAndReturnID(ApplicationID, DriverID, LicenseID, IssueDate, ExpirationDate, IsActive, UserID);

            if (InternationalLicenseApplicationID != -1)
            {
                MessageBox.Show($"International License Created Successfull With ID {InternationalLicenseID}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.InternationalLicenseID = InternationalLicenseID;
            }
            else
            {
                MessageBox.Show("Faliled To Create Internaional License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }



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
            int ApplicationTypeID = 6; // International License
            // (4) ApplicationStatus 
            byte ApplicationStatus = 3; // Completed
            // (5) LastStatusDate
            DateTime LastStatusDate = DateTime.Now;
            // (6) Fees depend on type
            decimal Fees = clsManageApplicationTypesBusiness.GetApplicationFees(ApplicationTypeID);

            // (7) User ID 
            int UserID = clsUserBusiness.FindUserIDUsingPasswordAndUserName(clsGlobalSettings.UserName, clsGlobalSettings.Password);

            // Make Application Of Type International License
            int InternationalLicenseApplicationID = clsManageApplicationsBusiness.MakeApplicationAndReturnApplicationID(PerosnID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate, Fees, UserID);


            if (InternationalLicenseApplicationID != -1)
            {
                MessageBox.Show($"Application Of Type International License Created Successfull With ID {InternationalLicenseApplicationID}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.InternationalLicenseApplicationID = InternationalLicenseApplicationID;
            }
            else
            {
                MessageBox.Show("Faliled To Create Application Of Type Internaional License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private void frmInternationalLicenseApplication_Load(object sender, EventArgs e)
        {
            linkLabel1.Enabled = false;
            linkLabel2.Enabled = false;
        }


        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int LicenseID = int.Parse(maskedTextBox1.Text); // Make Sure That textbox is not empty
            int ApplicationID = clsLicensesBusiness.GetApplicationIDUsingLicenseID(LicenseID);
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
            ctrlDriverLicenseInfo1.ResetDriverLicenseInformation();
            ctrlApplicationInfo1.ResetLocalLicenseData();

        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(InternationalLicenseID);
            frm.ShowDialog();


        }
    }
}
