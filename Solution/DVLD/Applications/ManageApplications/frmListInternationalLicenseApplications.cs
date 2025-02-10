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

namespace DVLD.Applications.ManageApplications
{
    public partial class frmListInternationalLicenseApplications : Form
    {
        public frmListInternationalLicenseApplications()
        {
            InitializeComponent();
        }

        private void contextMenuStrip1_Opening(object sender, CancelEventArgs e)
        {

        }

        private void frmListInternationalLicenseApplications_Load(object sender, EventArgs e)
        {
            ListLocalDrivingLicenseApplications();
            comboBox1.SelectedIndex = 0;
        }

        private void ListLocalDrivingLicenseApplications()
        {
            dataGridView1.DataSource = clsInternationalLicensesBusiness.ListInternationalLicenses();
            lblRecoredsNumber.Text = dataGridView1.RowCount.ToString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmInternationalLicenseApplication frm = new frmInternationalLicenseApplication();
            frm.ShowDialog();


            ListLocalDrivingLicenseApplications();
            comboBox1.SelectedIndex = 0;
        }

        // Filter 


        private void FilterInternationalDrivingLicenseApplicationsByIntLicID(int IntLicID)
        {
            dataGridView1.DataSource = clsInternationalLicensesBusiness.FilterInternationalDrivingLicenseApplicationsByIntLicID(IntLicID);
            lblRecoredsNumber.Text = dataGridView1.RowCount.ToString();

        }

        private void FilterInternationalDrivingLicenseApplicationsByIntAppID(int IntAppID)
        {
            dataGridView1.DataSource = clsInternationalLicensesBusiness.FilterInternationalDrivingLicenseApplicationsByIntAppID(IntAppID);

            lblRecoredsNumber.Text = dataGridView1.RowCount.ToString();
        }

        private void FilterInternationalDrivingLicenseApplicationsByLLicID(int LLicID)
        {
            dataGridView1.DataSource = clsInternationalLicensesBusiness.FilterInternationalDrivingLicenseApplicationsByLLicID(LLicID);

            lblRecoredsNumber.Text = dataGridView1.RowCount.ToString();
        }




        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem = comboBox1.SelectedItem.ToString();

            if (selectedItem != "None")
            {
                maskedTextBox1.Enabled = true;

                if (selectedItem == "Int.Lic.ID" || selectedItem == "App.ID" || selectedItem == "L.Lic.ID")
                {
                    maskedTextBox1.Mask = "0000"; // Allows a 4-digit number
                }

            }
            else
            {
                maskedTextBox1.Enabled = false;
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

            if (selectedItem == "Int.Lic.ID")
            {

                int IntLicID = int.Parse(maskedTextBox1.Text);
                FilterInternationalDrivingLicenseApplicationsByIntLicID(IntLicID);

            }
            else if (selectedItem == "App.ID")
            {

                int IntAppID = int.Parse(maskedTextBox1.Text);
                FilterInternationalDrivingLicenseApplicationsByIntAppID(IntAppID);

            }
            else if (selectedItem == "L.Lic.ID")
            {

                int LLicID = int.Parse(maskedTextBox1.Text);
                FilterInternationalDrivingLicenseApplicationsByLLicID(LLicID);

            }



        }
        // End Filter

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {

            // PersonID
            int InternationalLicenseID = (int)dataGridView1.CurrentRow.Cells[0].Value;

            int DriverID = clsInternationalLicensesBusiness.GetDriverIDUsingIntLiceID(InternationalLicenseID);

            int PersonID = clsDriverBusiness.GetPersonID(DriverID);

            frmPersonCardDetails frm = new frmPersonCardDetails(PersonID);
            frm.ShowDialog();

        }

        private void showLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int InternationalLicenseID = (int)dataGridView1.CurrentRow.Cells[2].Value;

            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(InternationalLicenseID);
            frm.ShowDialog();
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // National No
            int InternationalLicenseID = (int)dataGridView1.CurrentRow.Cells[0].Value;

            int DriverID = clsInternationalLicensesBusiness.GetDriverIDUsingIntLiceID(InternationalLicenseID);

            int PersonID = clsDriverBusiness.GetPersonID(DriverID);

            string NationalNo = clsPersonBusiness.GetPersonNationalNoUsingPerosnID(PersonID);

            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(NationalNo);
            frm.ShowDialog();
        }


    }
}
