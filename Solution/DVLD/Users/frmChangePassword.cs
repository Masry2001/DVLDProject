using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD.Users
{
    public partial class frmChangePassword : Form
    {

        public int UserID { get; set; } 

        clsUserBusiness User = new clsUserBusiness();
        public frmChangePassword(int UserID)
        {
            InitializeComponent();
            this.UserID = UserID;
            User = clsUserBusiness.FindUserByID(UserID);

        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            ctrlUserCard1.UserID = UserID;
            ctrlUserCard1.UserCardLoadUsingID();
            
        }

        private void HandleValidating(object sender, CancelEventArgs e)
        {


            if (ActiveControl == btnClose)
            {
                e.Cancel = false; // Allow the validation to pass without error
                return;
            }

            System.Windows.Forms.TextBox textBox = sender as System.Windows.Forms.TextBox;

            if (textBox.Text == "")
            {
                e.Cancel = true;
                errorProvider1.SetError(textBox, "This Field Is Required");

            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(textBox, "");
                return;
            }


        }


        private void ValidateUserCurrentPassword(object sender, CancelEventArgs e)
        {
            string CurrentPassword = User.Password;
            System.Windows.Forms.TextBox textBox = sender as System.Windows.Forms.TextBox;


            if (ActiveControl == btnClose)
            {
                e.Cancel = false; // Allow the validation to pass without error
                return;
            }

            if (textBox.Text != "" )
            {
                if (CurrentPassword == txtCurrentPassword.Text)
                {
                    e.Cancel = false;
                    errorProvider1.SetError(textBox, "");
                    return;

                }
                else
                {
                    e.Cancel = true;
                    errorProvider1.SetError(textBox, "Wrong Password");

                }
            }

 
        }

        private void HandleMatchPassword(object sender, CancelEventArgs e)
        {
            System.Windows.Forms.TextBox textBox = sender as System.Windows.Forms.TextBox;

            if (txtNewPassword.Text != txtConfirmPassword.Text)
            {
                e.Cancel = false;
                errorProvider1.SetError(txtConfirmPassword, "Passwords do not match");
                lblNewPassword.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\lock28px.png");
                lblConfirmPassword.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\lock28px.png");

            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtConfirmPassword, "");
                lblNewPassword.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\passwordsMatched32px.png");
                lblConfirmPassword.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\passwordsMatched32px.png");

            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {


            this.Close();
        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {
            HandleValidating(sender, e);
            ValidateUserCurrentPassword(sender, e);
        }

        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            HandleValidating(sender, e);

        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            HandleValidating(sender, e);
            HandleMatchPassword(sender, e);

        }


        private void UpdateUserPassword()
        {
            string CurrentPassword = txtCurrentPassword.Text;
            string NewPassword = txtNewPassword.Text;

            if (CurrentPassword != NewPassword) 
            {

                User.Password = NewPassword;
                User.SaveUserToDB();
                MessageBox.Show("Done Password Changed.");

            }
            else
            {
                MessageBox.Show("The New Password Is The Same As Current Password.");

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtCurrentPassword.Text != "" &&  txtConfirmPassword.Text != "" && txtNewPassword.Text != "")
            {
                if (txtNewPassword.Text == txtConfirmPassword.Text)
                {

                    UpdateUserPassword();

                } 
                else
                {
                    MessageBox.Show("Passwords Are Not Matched");
                }

            }
            else
            {
                MessageBox.Show("All Fields Are Required");
            }
        }

        private void txtCurrentPassword_TextChanged(object sender, EventArgs e)
        {
            string CurrentPassword = User.Password;

            if (CurrentPassword == txtCurrentPassword.Text)
            {
 
                lblCurrentPassword.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\openLockk28px.png");

            }
            else
            {

                lblCurrentPassword.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\lock28px.png");

            }
        }


        private void txtConfirmPassword_TextChanged(object sender, EventArgs e)
        {

            if (txtNewPassword.Text != "" && txtConfirmPassword.Text != "")
            {
                if (txtNewPassword.Text != txtConfirmPassword.Text)
                {

                    lblNewPassword.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\lock28px.png");
                    lblConfirmPassword.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\lock28px.png");

                }
                else
                {

                    lblNewPassword.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\passwordsMatched32px.png");
                    lblConfirmPassword.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\passwordsMatched32px.png");

                }
            }

        }

        private void txtNewPassword_TextChanged(object sender, EventArgs e)
        {

            if (txtNewPassword.Text != "" && txtConfirmPassword.Text != "")
            {
                if (txtNewPassword.Text != txtConfirmPassword.Text)
                {

                    lblNewPassword.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\lock28px.png");
                    lblConfirmPassword.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\lock28px.png");

                }
                else
                {

                    lblNewPassword.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\passwordsMatched32px.png");
                    lblConfirmPassword.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\passwordsMatched32px.png");

                }
            }
        }
    }
}
