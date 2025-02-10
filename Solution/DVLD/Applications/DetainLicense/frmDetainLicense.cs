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

namespace DVLD.Applications.DetainLicense
{
    public partial class frmDetainLicense : Form
    {


        int ApplicationTypeID { get; set; }
        int DetainID { get; set; }



        public frmDetainLicense()
        {
            InitializeComponent();


            ApplicationTypeID = 5; //Detain Licensea

            linkLabel1.Enabled = false;
            linkLabel2.Enabled = false;
            btnDetain.Enabled = false;

        }

        private void frmDetainLicense_Load(object sender, EventArgs e)
        {
            FirstLodedData();

        }

        private void FirstLodedData()
        {
            lblDetainDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
            int UserID = clsUserBusiness.FindUserIDUsingPasswordAndUserName(clsGlobalSettings.UserName, clsGlobalSettings.Password);
            string UserName = clsUserBusiness.FindUserNameUsingUserID(UserID);
            lblCreatedBy.Text = UserName;
        }


        public void SecondLodedData()
        {
            int LicenseID = int.Parse(maskedTextBox1.Text);

            lblLicenseID.Text = LicenseID.ToString();

        }

        public void ThirdLodedData()
        {

            lblDetainID.Text = DetainID.ToString();

        }

        private void ClearSecondAndThirdLodedData()
        {
            lblLicenseID.Text = "????";

            lblDetainID.Text = "????";

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

        private void txtFineFees_Validating(object sender, CancelEventArgs e)
        {

            // Check if the active control is the close button
            if (this.ActiveControl == btnClose)
            {
                e.Cancel = false; // Allow the form to close without validation
                return;
            }

            if (string.IsNullOrWhiteSpace(txtFineFees.Text))
            {
                errorProvider1.SetError(txtFineFees, "This field is required.");
                e.Cancel = true; // Prevent the user from leaving the TextBox
            }
            else
            {
                errorProvider1.SetError(txtFineFees, string.Empty);
            }

        }

        private void txtFineFees_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only numbers and backspace; disallow spaces and letters
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            linkLabel1.Enabled = false;
            ClearSecondAndThirdLodedData();
            ctrlDriverLicenseInfo1.ResetDriverLicenseInformation();


        }


        private void btnDetain_Click(object sender, EventArgs e)
        {



            if (MessageBox.Show("Are You Sure You Want To Detain This License", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                int LicenseID = int.Parse(maskedTextBox1.Text);
                bool IsLicenseInDetainList = clsDetainedLicensesBusiness.IsLicenseExistInDetainedLicensesList(LicenseID);
                if (IsLicenseInDetainList)
                {
                    bool IsLicenseDetainedAndNotReleased = clsDetainedLicensesBusiness.IsLicenseDetainedAndNotReleased(LicenseID);
                    if (IsLicenseDetainedAndNotReleased)
                    {
       
                        MessageBox.Show("You Can't Detain This License, Cause It's Detained", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    }
                    else
                    {
                        DetainLicenseProcess();

                        FilterBox.Enabled = false;
                        linkLabel2.Enabled = true;
                        btnDetain.Enabled = false;
                        txtFineFees.Enabled = false;
                    }
                }
                else
                {
                    DetainLicenseProcess();

                    FilterBox.Enabled = false;
                    linkLabel2.Enabled = true;
                    btnDetain.Enabled = false;
                    txtFineFees.Enabled = false;
                }

                // 1 means that license is Released and you can Detain it 
 
   
               
            }


        }

        private void DetainLicenseProcess()
        {
            // 1- License ID
            int LicenseID = int.Parse(maskedTextBox1.Text);

            // 2- DetainDate
            DateTime DetainDate = DateTime.Now;

            // 3- FineFees
            decimal FineFees = decimal.Parse(txtFineFees.Text);

            // 4- UserID
            int CreatedByUserID = clsUserBusiness.FindUserIDUsingPasswordAndUserName(clsGlobalSettings.UserName, clsGlobalSettings.Password);

            // 5- IsReleased => 0

            // 6- Realease Date => null

            // 7-Released By UserID => null


            // 8- ReleaseApplicationID => null

            // Detain License
            DetainID = clsDetainedLicensesBusiness.DetainLicense(LicenseID, DetainDate, FineFees, CreatedByUserID);
            
            if (DetainID != -1)
            {
                MessageBox.Show($"License Is Detained, DetainID {DetainID}", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }else
            {
                MessageBox.Show($"Failed To Detain License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }


        }

        private void txtFineFees_TextChanged(object sender, EventArgs e)
        {
            btnDetain.Enabled = true;
        }
    }
}
