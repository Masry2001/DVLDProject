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

namespace DVLD
{
    public partial class frmListDrivers : Form
    {
        public frmListDrivers()
        {
            InitializeComponent();
        }

        private void frmListDrivers_Load(object sender, EventArgs e)
        {
            ListDrivers();
            comboBox1.SelectedIndex = 0;
        }


        private void ListDrivers()
        {
            dataGridView1.DataSource = clsDriverBusiness.ListDrivers();
            lblRecoredsNumber.Text = dataGridView1.RowCount.ToString();
        }



        // Filter 


        private void FilterDriversByDriverID(int DriverID)
        {
            dataGridView1.DataSource = clsDriverBusiness.FilterDriversByDriverID(DriverID);
            lblRecoredsNumber.Text = dataGridView1.RowCount.ToString();

        }

        private void FilterDriversByPersonID(int PersonID)
        {
            dataGridView1.DataSource = clsDriverBusiness.FilterDriversByPersonID(PersonID);

            lblRecoredsNumber.Text = dataGridView1.RowCount.ToString();
        }

        private void FilterDriversByNationalNo(string NationalNo)
        {
            dataGridView1.DataSource = clsDriverBusiness.FilterDriversByNationalNo(NationalNo);

            lblRecoredsNumber.Text = dataGridView1.RowCount.ToString();
        }

        private void FilterDriversByFullName(string FullName)
        {
            dataGridView1.DataSource = clsDriverBusiness.FilterDriversByFullName(FullName);

            lblRecoredsNumber.Text = dataGridView1.RowCount.ToString();
        }



        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem = comboBox1.SelectedItem.ToString();

            if (selectedItem != "None")
            {
                maskedTextBox1.Enabled = true;

                if (selectedItem == "DriverID" || selectedItem == "PersonID")
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

                ListDrivers();
                return;
            }

            if (selectedItem == "DriverID")
            {

                int DriverID = int.Parse(maskedTextBox1.Text);
                FilterDriversByDriverID(DriverID);

            }
            else if (selectedItem == "PersonID")
            {

                int PersonID = int.Parse(maskedTextBox1.Text);
                FilterDriversByPersonID(PersonID);

            }
            else if (selectedItem == "NationalNo")
            {

                string NationalNo = maskedTextBox1.Text;
                FilterDriversByNationalNo(NationalNo);

            }
            else if (selectedItem == "FullName")
            {

                string FullName = maskedTextBox1.Text;
                FilterDriversByFullName(FullName);

            }


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        // End Filter







    }
}
