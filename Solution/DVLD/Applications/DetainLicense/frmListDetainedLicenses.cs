using DVLD.Applications.DrivingLicenceServices;
using DVLD.People;
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
    public partial class frmListDetainedLicenses : Form
    {
        public frmListDetainedLicenses()
        {
            InitializeComponent();
        }

        private void frmListDetainedLicenses_Load(object sender, EventArgs e)
        {
            ListDetainedLicenses();
            comboBox1.SelectedIndex = 0;

        }

        private void ListDetainedLicenses()
        {

            dataGridView1.DataSource = clsDetainedLicensesBusiness.ListDetainedLicenses();
            lblRecordsCount.Text = dataGridView1.RowCount.ToString();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // PersonID
            string NationalNo = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            int PersonID = clsPersonBusiness.GetPersonIDUsingNationalNo(NationalNo);
            frmPersonCardDetails frm = new frmPersonCardDetails(PersonID);
            frm.ShowDialog();
        }

        private void showLicenseMenuItem_Click(object sender, EventArgs e)
        {
            // LicenseID
            int LicenseID = (int)dataGridView1.CurrentRow.Cells[1].Value;

            frmShowLicenseInfo frm = new frmShowLicenseInfo(LicenseID);
            frm.ShowDialog();

        }

        private void showLicensesHistoryMenuItem_Click(object sender, EventArgs e)
        {
            string NationalNo = dataGridView1.CurrentRow.Cells[6].Value.ToString();
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(NationalNo);
            frm.ShowDialog();
        }

        private void releaseLicenseMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = (int)dataGridView1.CurrentRow.Cells[1].Value;
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense(LicenseID);
            frm.ShowDialog();
            ListDetainedLicenses();


        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            bool IsReleased = (bool)dataGridView1.CurrentRow.Cells[3].Value;

            if (IsReleased)
            {
                releaseLicenseMenuItem.Enabled = false;
            }
            else
            {
                releaseLicenseMenuItem.Enabled = true;
            }
        }




        // Filter 


        private void FilterDetainedLicenseByDetainID(int DetainID)
        {
            dataGridView1.DataSource = clsDetainedLicensesBusiness.FilterDetainedLicenseByDetainID(DetainID);
            lblRecordsCount.Text = dataGridView1.RowCount.ToString();

        }


        private void FilterDetainedLicenseByReleaseApplicationID(int ApplicationID)
        {
            dataGridView1.DataSource = clsDetainedLicensesBusiness.FilterDetainedLicenseByReleaseApplicationID(ApplicationID);

            lblRecordsCount.Text = dataGridView1.RowCount.ToString();
        }


        private void FilterDetainedLicenseByNationalNo(string NationalNo)
        {
            dataGridView1.DataSource = clsDetainedLicensesBusiness.FilterDetainedLicenseByNationalNo(NationalNo);

            lblRecordsCount.Text = dataGridView1.RowCount.ToString();
        }
        private void FilterDetainedLicenseByFullName(string FullName)
        {
            dataGridView1.DataSource = clsDetainedLicensesBusiness.FilterDetainedLicenseByFullName(FullName);

            lblRecordsCount.Text = dataGridView1.RowCount.ToString();
        }

        private void FilterDetainedLicenseByIsReleased(bool IsReleased)
        {
            dataGridView1.DataSource = clsDetainedLicensesBusiness.FilterDetainedLicenseByIsReleased(IsReleased);

            lblRecordsCount.Text = dataGridView1.RowCount.ToString();
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem = comboBox1.SelectedItem.ToString();

            if (selectedItem != "None")
            {
                maskedTextBox1.Enabled = true;

                if (selectedItem == "ReleaseApplicationID" || selectedItem == "DetainID")
                {
                    maskedTextBox1.Mask = "0000"; // Allows a 4-digit number
                    maskedTextBox1.KeyPress -= maskedTextBox1_KeyPress; // Unsubscribe from KeyPress event
                }
                else if (selectedItem == "NationalNo")
                {
                    maskedTextBox1.Mask = ""; // Allow String or Numbers
                    maskedTextBox1.KeyPress -= maskedTextBox1_KeyPress; // Unsubscribe from KeyPress event
                }
                else if (selectedItem == "FullName")
                {
                    maskedTextBox1.Mask = ""; // No specific mask, but restrict to letters
                    maskedTextBox1.KeyPress += maskedTextBox1_KeyPress; // Subscribe to KeyPress event
                } 
                else if (selectedItem == "IsReleased")
                {
                    maskedTextBox1.Mask = "0"; // 

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

                ListDetainedLicenses();
                return;
            }

            if (selectedItem == "DetainID")
            {

                int DetainID = int.Parse(maskedTextBox1.Text);
                FilterDetainedLicenseByDetainID(DetainID);

            }
            else if (selectedItem == "ReleaseApplicationID")
            {

                int ReleaseApplicationID = int.Parse(maskedTextBox1.Text);
                FilterDetainedLicenseByReleaseApplicationID(ReleaseApplicationID);

            }
            else if (selectedItem == "NationalNo")
            {

                string NationalNo = maskedTextBox1.Text;
                FilterDetainedLicenseByNationalNo(NationalNo);

            }
            else if (selectedItem == "FullName")
            {

                string FullName = maskedTextBox1.Text;
                FilterDetainedLicenseByFullName(FullName);

            }
            else if (selectedItem == "IsReleased")
            {
                    bool IsReleased = maskedTextBox1.Text == "1";
                    FilterDetainedLicenseByIsReleased(IsReleased);

            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmDetainLicense frm = new frmDetainLicense();
            frm.ShowDialog();
            ListDetainedLicenses();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense(-1);
            frm.ShowDialog();
            ListDetainedLicenses();
        }
    }
}
