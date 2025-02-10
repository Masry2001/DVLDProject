using DVLD.Applications.DrivingLicenceServices;
using DVLD.Tests;
using DVLD.Tests.WrittenTest;
using DVLD.Tests.StreetTest;
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
using static System.Net.Mime.MediaTypeNames;


namespace DVLD.Applications.ManageApplications
{
    public partial class frmListLocalDrivingLicenseApplications : Form
    {





        public frmListLocalDrivingLicenseApplications()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }






        private void ListLocalDrivingLicenseApplications()
        {
            dataGridView1.DataSource = clsLocalDrivingLicenceApplicationBusiness.ListLocalDrivingLicenseApplications();
            lblRecoredsNumber.Text = dataGridView1.RowCount.ToString();
        }


        private void frmListLocalDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            ListLocalDrivingLicenseApplications();
            comboBox1.SelectedIndex = 0;


        }




        // Filter 


        private void FilterLocalDrivingLicenseApplicationsByID(int LocalDrivingLicenseApplicationID)
        {
            dataGridView1.DataSource = clsLocalDrivingLicenceApplicationBusiness.FilterLocalDrivingLicenseApplicationsByID(LocalDrivingLicenseApplicationID);
            lblRecoredsNumber.Text = dataGridView1.RowCount.ToString();

        }

        private void FilterLocalDrivingLicenseApplicationsByNationalNo(string NationalNo)
        {
            dataGridView1.DataSource = clsLocalDrivingLicenceApplicationBusiness.FilterLocalDrivingLicenseApplicationsByNationalNo(NationalNo);

            lblRecoredsNumber.Text = dataGridView1.RowCount.ToString();
        }

        private void FilterLocalDrivingLicenseApplicationsByFullName(string FullName)
        {
            dataGridView1.DataSource = clsLocalDrivingLicenceApplicationBusiness.FilterLocalDrivingLicenseApplicationsByFullName(FullName);

            lblRecoredsNumber.Text = dataGridView1.RowCount.ToString();
        }

        private void FilterLocalDrivingLicenseApplicationsByStatus(string Status)
        {
            dataGridView1.DataSource = clsLocalDrivingLicenceApplicationBusiness.FilterLocalDrivingLicenseApplicationsByStatus(Status);

            lblRecoredsNumber.Text = dataGridView1.RowCount.ToString();
        }



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem = comboBox1.SelectedItem.ToString();

            if (selectedItem != "None")
            {
                maskedTextBox1.Enabled = true;

                if (selectedItem == "L.D.L.AppID")
                {
                    maskedTextBox1.Mask = "0000"; // Allows a 4-digit number
                    maskedTextBox1.KeyPress -= maskedTextBox1_KeyPress; // Unsubscribe from KeyPress event
                }
                else if (selectedItem == "NationalNo")
                {
                    maskedTextBox1.Mask = ""; // Allow String or Numbers
                    maskedTextBox1.KeyPress -= maskedTextBox1_KeyPress; // Unsubscribe from KeyPress event
                }
                else if (selectedItem == "FullName" || selectedItem == "Status")
                {
                    maskedTextBox1.Mask = ""; // No specific mask, but restrict to letters
                    maskedTextBox1.KeyPress += maskedTextBox1_KeyPress; // Subscribe to KeyPress event
                }
            }
            else
            {
                maskedTextBox1.Enabled = false;
                maskedTextBox1.KeyPress -= maskedTextBox1_KeyPress; // Unsubscribe from KeyPress event
            }
        }

        // Event handler to allow only alphabetic characters
        private void maskedTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only letters, spaces, and control characters (like Backspace)
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != ' ')
            {
                e.Handled = true; // Cancel the input if it's not a letter, space, or control character
            }
        }



        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {

            string selectedItem = comboBox1.SelectedItem.ToString();

            if (maskedTextBox1.Text == "")
            {

                ListLocalDrivingLicenseApplications();
                return;
            }

            if (selectedItem == "L.D.L.AppID")
            {

                int LocalDrivingLicenseApplicationID = int.Parse(maskedTextBox1.Text);
                FilterLocalDrivingLicenseApplicationsByID(LocalDrivingLicenseApplicationID);

            }
            else if (selectedItem == "NationalNo")
            {

                string NationalNo = maskedTextBox1.Text;
                FilterLocalDrivingLicenseApplicationsByNationalNo(NationalNo);

            }
            else if (selectedItem == "FullName")
            {

                string FullName = maskedTextBox1.Text;
                FilterLocalDrivingLicenseApplicationsByFullName(FullName);

            }
            else if (selectedItem == "Status")
            {

                string Status = maskedTextBox1.Text;
                FilterLocalDrivingLicenseApplicationsByStatus(Status);

            }


        }


        // End Filter
        private void cancelApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Cancel This Application", "Cancel", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {

                int LocalDrivingLicenseApplicationID = (int)dataGridView1.CurrentRow.Cells[0].Value;

                int ApplicationID = clsLocalDrivingLicenceApplicationBusiness.GetApplicationIDUsingLDLAppID(LocalDrivingLicenseApplicationID);

                int ApplicationStatus = 2; // 2 for Canceled

                if (clsManageApplicationsBusiness.UpdateApplicationStatus(ApplicationID, ApplicationStatus))
                {
                    MessageBox.Show("Application Canceled", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Here You Should Update Last Status Date
                    if (clsManageApplicationsBusiness.UpdateApplicationLastStatusDate(ApplicationID, DateTime.Now))
                    {
                        MessageBox.Show("Last Status Date Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    }


                    ListLocalDrivingLicenseApplications();
                }
            }

        }


        private bool Delete(int LocalDrivingLicenseApplicationID, int ApplicationID)
        {
            bool ApplicationAndLDLApplictionDeleted = false;

            if (clsLocalDrivingLicenceApplicationBusiness.DeleteLocalDrivingLicenseApplication(LocalDrivingLicenseApplicationID))
            {
                MessageBox.Show("DeleteLocalDrivingLicenseApplication");

                if (clsManageApplicationsBusiness.DeleteApplication(ApplicationID))
                {
                    MessageBox.Show("DeleteApplication");
                    ApplicationAndLDLApplictionDeleted = true;

                }
            }

            return ApplicationAndLDLApplictionDeleted;

        }


        private void deleteApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Delete This Application", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {

                int LocalDrivingLicenseApplicationID = (int)dataGridView1.CurrentRow.Cells[0].Value;

                int ApplicationID = clsLocalDrivingLicenceApplicationBusiness.GetApplicationIDUsingLDLAppID(LocalDrivingLicenseApplicationID);

                if (Delete(LocalDrivingLicenseApplicationID, ApplicationID))
                {
                    MessageBox.Show("Application Deleted");


                    ListLocalDrivingLicenseApplications();
                }
            }
        }


        private void editApplicationToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int LocalDrivingLicenseApplicationID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            frmAddUpdateLocalDrivingLicenceApplication frm = new frmAddUpdateLocalDrivingLicenceApplication(LocalDrivingLicenseApplicationID);
            frm.ShowDialog();

            int ApplicationID = clsLocalDrivingLicenceApplicationBusiness.GetApplicationIDUsingLDLAppID(LocalDrivingLicenseApplicationID);


            // Here You Should Update Last Status Date
            if (clsManageApplicationsBusiness.UpdateApplicationLastStatusDate(ApplicationID, DateTime.Now))
            {
                MessageBox.Show("Last Status Date Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }

            ListLocalDrivingLicenseApplications();
        }


        private void dataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            HandleEnabaledTests();
            HandleEnabledMenues();
        }



        private void HandleEnabaledTests()
        {
            int PassedTests = (int)dataGridView1.CurrentRow.Cells[5].Value;



            scheduleVisionTestToolStripMenuItem.Enabled = false;
            scheduleWrittenTestToolStripMenuItem.Enabled = false;
            scheduleStreetTestToolStripMenuItem.Enabled = false;

            if (PassedTests == 0)
            {
                scheduleVisionTestToolStripMenuItem.Enabled = true;

            }
            else if (PassedTests == 1)
            {
                scheduleWrittenTestToolStripMenuItem.Enabled = true;

            }
            else if (PassedTests == 2)
            {
                scheduleStreetTestToolStripMenuItem.Enabled = true;

            }


        }




        private void scheduleVisionTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenceApplicationID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            frmVisionTest frm = new frmVisionTest(LocalDrivingLicenceApplicationID);
            frm.ShowDialog();

            ListLocalDrivingLicenseApplications();
        }

        private void scheduleWrittenTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LocalDrivingLicenceApplicationID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            frmWrittenTest frm = new frmWrittenTest(LocalDrivingLicenceApplicationID);

            frm.ShowDialog();

            ListLocalDrivingLicenseApplications();
        }

        private void scheduleStreetTestToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int LocalDrivingLicenceApplicationID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            frmStreetTest frm = new frmStreetTest(LocalDrivingLicenceApplicationID);

            frm.ShowDialog();

            ListLocalDrivingLicenseApplications();

        }


        private void HandleEnabledMenues()
        {
            IssueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
            showLicenseToolStripMenuItem.Enabled = false;

            int PassedTests = (int)dataGridView1.CurrentRow.Cells[5].Value;

            if (PassedTests == 3)
            {
                IssueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = true;

            }


            string Status = (string)dataGridView1.CurrentRow.Cells[6].Value;

            if (Status == "completed")
            {
                IssueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;

                editApplicationToolStripMenuItem.Enabled = false;
                deleteApplicationToolStripMenuItem.Enabled = false;
                cancelApplicationToolStripMenuItem.Enabled = false;
                scheduleTestsToolStripMenuItem.Enabled = false;

                showLicenseToolStripMenuItem.Enabled = true;


            }

            if (Status == "new")
            {
                editApplicationToolStripMenuItem.Enabled = true;
                deleteApplicationToolStripMenuItem.Enabled = true;
                cancelApplicationToolStripMenuItem.Enabled = true;
                scheduleTestsToolStripMenuItem.Enabled = true;
            }

            if (Status == "canceled")
            {
                editApplicationToolStripMenuItem.Enabled = false;
                cancelApplicationToolStripMenuItem.Enabled = false;
                scheduleTestsToolStripMenuItem.Enabled = false;
                IssueDrivingLicenseFirstTimeToolStripMenuItem.Enabled = false;
                showLicenseToolStripMenuItem.Enabled = false;


            }



        }

        private void IssueDrivingLicenseFirstTimeToolStripMenuItem_Click(object sender, EventArgs e)
        {

            int LocalDrivingLicenceApplicationID = (int)dataGridView1.CurrentRow.Cells[0].Value;

            frmIssueDrivingLicenseForTheFirstTime frm = new frmIssueDrivingLicenseForTheFirstTime(LocalDrivingLicenceApplicationID);
            frm.ShowDialog();


            ListLocalDrivingLicenseApplications();

        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // Use LDLAppID to Get AppID then use AppID To Get LicenseID
            int LocalDrivingLicenseApplicationID = (int)dataGridView1.CurrentRow.Cells[0].Value;

            int AppID = clsLocalDrivingLicenceApplicationBusiness.GetApplicationIDUsingLDLAppID(LocalDrivingLicenseApplicationID);

            int LicenseID = clsLicensesBusiness.GetLicenseIDUsingAppID(AppID);


            frmShowLicenseInfo frm = new frmShowLicenseInfo(LicenseID);
            frm.ShowDialog();

        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {

            string NationalNo = (string)dataGridView1.CurrentRow.Cells[2].Value;

            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory( NationalNo);
            frm.ShowDialog();

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            frmAddUpdateLocalDrivingLicenceApplication frm = new frmAddUpdateLocalDrivingLicenceApplication(-1);
            frm.ShowDialog();
            ListLocalDrivingLicenseApplications();
        }

    }
}
