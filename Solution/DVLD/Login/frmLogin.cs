using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DVLD.Login
{
    public partial class frmLogin : Form
    {

        clsUserBusiness user1 = new clsUserBusiness();
        private bool isFormClosing = false;

        public frmLogin()
        {
            InitializeComponent();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            isFormClosing = true;
            this.Close();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            // ValidateUser is a method that validates the user credentials 
            if (user1.ValidateUser(txtUserName.Text, txtPassword.Text))
            {

                if (!user1.IsActive)
                {
                    MessageBox.Show("User Is Not Active.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;

                }

                if (chkRememberMe.Checked)
                {
                    SaveCredentials(txtUserName.Text, txtPassword.Text);
                }
                else
                {
                    SaveCredentials("", "");

                }

                // Store UserName and Password in clsGlobalSettings
                clsGlobalSettings.UserName = txtUserName.Text;
                clsGlobalSettings.Password = txtPassword.Text;


                // Hide the login form
                this.Hide();

                // Create and show the main form
                MainForm mainForm = new MainForm();
                mainForm.ShowDialog();

                // Close the login form
                this.Close();
            }
            else
            {

           
                    MessageBox.Show("Invalid username or password.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                
            }
        }

        private void txtUsername_Validating(object sender, CancelEventArgs e)
        {

            if (isFormClosing)
            {
                e.Cancel = false;
                errorProvider1.SetError(txtUserName, "");
                return;
            }


            if (string.IsNullOrEmpty(txtUserName.Text))
            {
                e.Cancel = true;
                txtUserName.Focus();
                errorProvider1.SetError(txtUserName, "UserName Should Have A Value");
            }
            else
            {

                e.Cancel = false;
                errorProvider1.SetError(txtUserName, "");
                
            }
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {

            if (isFormClosing)
            {
                e.Cancel = false;
                errorProvider1.SetError(txtPassword, "");
                return;
            }


            if (string.IsNullOrEmpty(txtPassword.Text))
            {
                e.Cancel = true;
                txtPassword.Focus();
                errorProvider1.SetError(txtPassword, "Password Should Have A Value");
            }
            else
            {

                e.Cancel = false;
                errorProvider1.SetError(txtPassword, "");
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void frmLogin_Load(object sender, EventArgs e)
        {
           
            LoadCredentials();

        }


        private void SaveCredentials(string UserName, string Password)
        {
            string filePath = "D:\\programming\\DVLD_Credentials\\credentials.txt";
            using (StreamWriter writer = new StreamWriter(filePath, false, Encoding.UTF8))
            {
                writer.WriteLine(UserName);
                writer.WriteLine(Password);
            }
        }


        private void LoadCredentials()
        {
            string filePath = "D:\\programming\\DVLD_Credentials\\credentials.txt";

            if (File.Exists(filePath))
            {
                using (StreamReader reader = new StreamReader(filePath, Encoding.UTF8))
                {
                    string UserName = reader.ReadLine();
                    string Password = reader.ReadLine();

                    txtUserName.Text = UserName;
                    txtPassword.Text = Password;
                }
            } 
            else
            {
                Console.WriteLine("File Not Exist (LoadCredentials)");
            }
        }




    }
}
