using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD.People
{
    public partial class frmListPeople : Form
    {
        public frmListPeople()
        {
            InitializeComponent();
        }

        private void ListPeople()
        {
            dataGridView1.DataSource = clsPersonBusiness.ListPeople();
            lblRecordsCount.Text = dataGridView1.RowCount.ToString();
        }

        private void FilterPeopleByID(int PersonID)
        {
            dataGridView1.DataSource = clsPersonBusiness.FilterPeopleByID(PersonID);
            lblRecordsCount.Text = dataGridView1.RowCount.ToString();

        }

        private void FilterPeopleByGendor(byte Gendor)
        {
            dataGridView1.DataSource = clsPersonBusiness.FilterPeopleByGendor(Gendor);
            lblRecordsCount.Text = dataGridView1.RowCount.ToString();
        }

        private void FilterPeople(string ColumnName, string Filter)
        {
            dataGridView1.DataSource = clsPersonBusiness.FilterPeople(ColumnName, Filter);
            lblRecordsCount.Text = dataGridView1.RowCount.ToString();
        }

        private void frmListPeople_Load(object sender, EventArgs e)
        {
            ListPeople();
            comboBox1.SelectedIndex = 0;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            string selectedItem = comboBox1.SelectedItem.ToString();

            if (selectedItem != "None")
            {

                maskedTextBox1.Enabled = true;

                if (selectedItem == "PersonID")
                {
                    maskedTextBox1.Mask = "0000"; // Allows a 4-digit number

                }
                else if (selectedItem == "Gendor")
                {
                    // Gendor 0 (man) or 1 (woman)
                    maskedTextBox1.Mask = "0"; // Allow a 1-digit number 
                }
                else
                {
                    maskedTextBox1.Mask = ""; // Allow String or Numbers
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

                ListPeople();
                return;
            }

            if (selectedItem == "PersonID")
            {
                int PersonID;
                bool isParsed = int.TryParse(maskedTextBox1.Text, out PersonID);
                
                if (isParsed)
                {
                    // call business layer => call dataAccess layer
                    FilterPeopleByID(PersonID);
                }


            } 
            else if (selectedItem == "Gendor") 
            {
                // Gendor
                byte Gendor;
                bool isParsed = byte.TryParse(maskedTextBox1.Text, out Gendor);

                if (isParsed)
                {
                    // Conversion succeeded, you can use the Gendor value
                    FilterPeopleByGendor(Gendor);
                }
                else
                {
                    Gendor = 0; // Default value, assuming 0 is for Male
                    FilterPeopleByGendor(Gendor);

                }
            } 
            else
            {
                string filter = maskedTextBox1.Text;
                FilterPeople(selectedItem, filter);
            }


        }


        private void deletePersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PersonID = Convert.ToInt32(dataGridView1.CurrentRow.Cells[0].Value);

            if (MessageBox.Show($"Are You Sure You Want To Delete Person {PersonID}", "Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {

                if (clsPersonBusiness.DeletePerson(PersonID))
                {
                    MessageBox.Show($"Person {PersonID} Deleted");
                    ListPeople();
                }
                else
                {
                    MessageBox.Show($"Failed To Delete Person {PersonID} It Has Data Linked To It", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);

                }

            } else
            {
                MessageBox.Show("Ceneled Deleting");
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Form frmAddUpdate = new frmAddUpdatePerson(-1);
            frmAddUpdate.ShowDialog();
            ListPeople();
        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {

            Form frmAddUpdate = new frmAddUpdatePerson(-1);
            frmAddUpdate.ShowDialog();
            ListPeople();

        }

        private void editPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int SelectedPersonID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            Form frmAddUpdate = new frmAddUpdatePerson(SelectedPersonID);
            frmAddUpdate.ShowDialog();
            ListPeople();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int SelectedPersonID = (int)dataGridView1.CurrentRow.Cells[0].Value;

            frmPersonCardDetails frmCardDetails = new frmPersonCardDetails(SelectedPersonID);   
            frmCardDetails.ShowDialog();
            ListPeople();

        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Will Be Available Soon");
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Will Be Available Soon");
        }





    }
}
