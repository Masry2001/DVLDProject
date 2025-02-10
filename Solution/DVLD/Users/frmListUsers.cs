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

namespace DVLD.Users
{
    public partial class frmListUsers : Form
    {
        public frmListUsers()
        {
            InitializeComponent();
        }



        private void ListUsers()
        {
            dataGridView1.DataSource = clsUserBusiness.ListUsers();
            lblRecoredsNumber.Text = dataGridView1.RowCount.ToString();
        }

        private void frmListUsers_Load(object sender, EventArgs e)
        {
            ListUsers();
            comboBox1.SelectedIndex = 0;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            string selectedItem = comboBox1.SelectedItem.ToString();
            maskedTextBox1.Visible = true;
            maskedTextBox1.Focus();
            comboBox2.Visible = false;
            comboBox2.SelectedIndex = 0;

            if (selectedItem != "None")
            {

                maskedTextBox1.Enabled = true;

                if (selectedItem == "PersonID" || selectedItem == "UserID")
                {
                    maskedTextBox1.Mask = "0000"; // Allows a 4-digit number

                }
                else if (selectedItem == "IsActive")
                {
                    // Convert the masked text box to comboBox (drob Down List All, Yes, No)

                    maskedTextBox1.Visible = false;
                    comboBox2.Visible = true;
                    comboBox2.SelectedIndex = 0;

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


        private void FilterUsers(string ColumnName, string Filter)
        {
            dataGridView1.DataSource = clsUserBusiness.FilterUsers(ColumnName, Filter);
            lblRecoredsNumber.Text = dataGridView1.RowCount.ToString();
        }

        private void FilterUsers(string ColumnName, int ID)
        {
            dataGridView1.DataSource = clsUserBusiness.FilterUsers(ColumnName, ID);
            lblRecoredsNumber.Text = dataGridView1.RowCount.ToString();

        }


        private void FilterUsers(string ColumnName, bool IsActive)
        {
            dataGridView1.DataSource = clsUserBusiness.FilterUsers(ColumnName, IsActive);
            lblRecoredsNumber.Text = dataGridView1.RowCount.ToString();

        }


        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {

            string selectedItem = comboBox1.SelectedItem.ToString();
            // selectedItem is the ColumnName

            if (maskedTextBox1.Text == "")
            {

                ListUsers();
                return;
            }

            if (selectedItem == "PersonID" || selectedItem == "UserID")
            {
                int ID;
                bool isParsed = int.TryParse(maskedTextBox1.Text, out ID);

                if (isParsed)
                {
                    // call business layer => call dataAccess layer
                    // this will handle UserID and PersonID
                    FilterUsers(selectedItem, ID);
                }


            }
            else if (selectedItem == "IsActive")
            {
                // ComboBox2
                // IsActive => All
                //IsActive => Yes
                // IsActive => No
                // call ComboBox2_IndexChange
                comboBox2_SelectedIndexChanged(sender, e);


            }
            else
            {
                string filter = maskedTextBox1.Text;
                FilterUsers(selectedItem, filter);
            }


        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            // ComboBox2
            // IsActive => All
            // IsActive => Yes
            // IsActive => No
            // call ComboBox2_IndexChange
            string selectedItem = comboBox2.SelectedItem.ToString();
            string ColumnName = comboBox1.SelectedItem.ToString();
            bool IsActive = true;

            if (selectedItem == "All")
            {
                ListUsers();
                return;
            } 
            else if (selectedItem == "Yes")
            {
                IsActive = true;
                FilterUsers(ColumnName, IsActive);
            } 
            else // selectedItem == No
            {
                IsActive = false;
                FilterUsers(ColumnName, IsActive);
            }

        }


        // Add New User Button
        private void button1_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frmAddUpdateUser = new frmAddUpdateUser(-1);
            frmAddUpdateUser.ShowDialog();
            ListUsers();

        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void editPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = (int)dataGridView1.CurrentRow.Cells[0].Value;


            frmAddUpdateUser frm = new frmAddUpdateUser(UserID);
            frm.ShowDialog();

            ListUsers();

        }

        private void DeleteUser(int UserID)
        {

            if (MessageBox.Show($"Are You Sure You Want To Delete User {UserID}", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.OK)
            {
                if (clsUserBusiness.DeleteUser(UserID))
                {
                    MessageBox.Show("User Deleted");
                }
                else
                {
                    MessageBox.Show("Failed To Delte");

                }

                ListUsers();
            }
            else
            {
                    MessageBox.Show($"Cancled Deleting User {UserID}");

            }

      

        }

        private void deletePersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            
            DeleteUser(UserID);

        }

        private void addNewPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmAddUpdateUser frmAddUpdateUser = new frmAddUpdateUser(-1);
            frmAddUpdateUser.ShowDialog();
            ListUsers();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int UserID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            frmUserInfo frm = new frmUserInfo(UserID);
            frm.ShowDialog();
            ListUsers();

        }

        private void toolStripTextBox1_Click(object sender, EventArgs e)
        {
            int UserID = (int)dataGridView1.CurrentRow.Cells[0].Value;
            frmChangePassword frm = new frmChangePassword(UserID);
            frm.ShowDialog();
            ListUsers();
        }
    }
}
