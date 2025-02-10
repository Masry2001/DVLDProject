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

namespace DVLD.Users
{
    public partial class frmAddUpdateUser : Form
    {

        clsUserBusiness User = new clsUserBusiness();


        public int UserID { get; private set; }

        public enum enMode
        {
            AddNew,
            Update
        }

        public enMode Mode;

        public frmAddUpdateUser(int UserID)
        {
            InitializeComponent();

            Mode = enMode.AddNew;

            this.UserID = UserID;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HanldeMovingToLoingInfoTabToMakePersonAUser()
        {

            if (Mode == enMode.AddNew)
            {

                // PersonLoded and person is not a user
                if (ctrlPersonCardWithFilter1.IsPersonLoded())
                {
                    int PersonID = ctrlPersonCardWithFilter1.PersonID;


                    if (!clsUserBusiness.IsPersonAUser(PersonID))
                    {
                        tabControl1.SelectedIndex = 1;

                    }
                    else
                    {
                        MessageBox.Show("This Person Is Already A User.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        tabControl1.SelectedIndex = 0;

                    }
                }
                else
                {

                    MessageBox.Show("You Have To Search For A Person Or Add A New One To Be Able To Make Him A User.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tabControl1.SelectedIndex = 0;


                }

            } else
            {
                tabControl1.SelectedIndex = 1;

            }


        }

        private void btnNext_Click_1(object sender, EventArgs e)
        {

            HanldeMovingToLoingInfoTabToMakePersonAUser();

        }


        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                HanldeMovingToLoingInfoTabToMakePersonAUser();

            }

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked)
            {
                lblActive.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\activeUser28px.png");
            }
            else
            {
                lblActive.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\notActiveUser28px.png");
            }
        }

        private void HandleValidating(object sender, CancelEventArgs e)
        {

            if (ActiveControl == btnClose || ActiveControl == btnBack || ActiveControl == LoginInfoPage)
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

        private void HandleKeyPress(object sender, KeyPressEventArgs e)
        {

            if (e.KeyChar == (char)Keys.Space)
            {
                // Suppress the key press event to prevent the space from being entered
                e.Handled = true;
            }
        }

        private void HandleMatchPassword(object sender, CancelEventArgs e)
        {
            System.Windows.Forms.TextBox textBox = sender as System.Windows.Forms.TextBox;

            if (txtPassword.Text != txtConfirmPassword.Text)
            {
                e.Cancel = false;
                errorProvider1.SetError(txtConfirmPassword, "Passwords do not match");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtConfirmPassword, "");
            }
        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            HandleValidating(sender, e);
        }

        private void txtUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleKeyPress(sender, e);
        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            HandleValidating(sender, e);
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleKeyPress(sender, e);
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            HandleValidating(sender, e);
            HandleMatchPassword(sender, e);
        }

        private void txtConfirmPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            HandleKeyPress(sender, e);
            
        }


        private void FillUserObject()
        {

            User.PersonID = ctrlPersonCardWithFilter1.PersonID;
            User.UserName = txtUserName.Text;
            User.Password = txtPassword.Text;
            User.IsActive = checkBox1.Checked ? true : false;
        }

        private void AddNewUser()
        {

            FillUserObject();

            if (User.SaveUserToDB())
            {
                MessageBox.Show("Done, User Saved To DB");

                lblUserID.Text  = User.UserID.ToString();
                lblHeader.Text = "Update User";
                Mode = enMode.Update;

            }
            else
            {
                MessageBox.Show("Failed To Add User, This UserName Exists");

            }
        }

        
        private void UpdateUser()
        {
            User.UserName = txtUserName.Text;
            User.Password = txtPassword.Text;
            User.IsActive = checkBox1.Checked;

            if (User.SaveUserToDB())
            {
                MessageBox.Show("Done, User Updated");

            } 
            else
            {
                MessageBox.Show("Failed To Update User (frmAddUpdateUser).");

            }
        }


        private void btnSave_Click(object sender, EventArgs e)
        {
            if (txtUserName.Text != "" && txtPassword.Text != "" && txtConfirmPassword.Text != "")
            {

                if (txtPassword.Text == txtConfirmPassword.Text)
                {

                    if (Mode == enMode.AddNew)
                    {
                        AddNewUser();

                    } 
                    else
                    {
                        UpdateUser();

                    }

                } 
                else
                {

                    MessageBox.Show("Passwords Are Not Mathced");

                }

            }
            else
            {
                MessageBox.Show("You Have To Fill All Fields Of Login Info Tab Before Saving");
            }

        }

        private void ctrlPersonCardWithFilter1_Load(object sender, EventArgs e)
        {

        }

        private void LoadPersonInfo()
        {
            ctrlPersonCardWithFilter1.PersonID = User.PersonID;
            ctrlPersonCardWithFilter1.LoadPersonData();
            lblHeader.Text = "Update User";

        }

        private void LoadLoginInfo()
        {

            lblUserID.Text = UserID.ToString();
            txtUserName.Text = User.UserName;
            txtPassword.Text = User.Password;
            txtConfirmPassword.Text = User.Password;
            checkBox1.Checked = User.IsActive;


        }

        private void LoadUserData()
        {
            // User Data Are 1- Person Info, 2- Login Info
            // Person Info
            LoadPersonInfo();
            LoadLoginInfo();

        }

        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            // if UserID != -1 , Load user Data
            // User Data Are 1- Person Info, 2- Login Info
            // Send The UserID to CtrlPersonCardWithFilter

            if (UserID != -1)
            {
                // update Mode
                Mode = enMode.Update;
                User = clsUserBusiness.FindUserByID(UserID);
                LoadUserData();

            }

        }

        private void PersonalInfoPage_Click(object sender, EventArgs e)
        {

        }

        private void txtPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtPassword.Text != "" && txtConfirmPassword.Text != "")
            {
                if (txtPassword.Text != txtConfirmPassword.Text)
                {

                    lblPassword.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\lock28px.png");
                    lblConfirmPassword.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\lock28px.png");

                }
                else
                {

                    lblPassword.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\passwordsMatched32px.png");
                    lblConfirmPassword.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\passwordsMatched32px.png");

                }
            }
        }

        private void txtConfirmPassword_TextChanged(object sender, EventArgs e)
        {
            if (txtPassword.Text != "" && txtConfirmPassword.Text!= "" )
            {
                if (txtPassword.Text != txtConfirmPassword.Text)
                {

                    lblPassword.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\lock28px.png");
                    lblConfirmPassword.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\lock28px.png");

                }
                else
                {

                    lblPassword.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\passwordsMatched32px.png");
                    lblConfirmPassword.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\passwordsMatched32px.png");

                }
            }

  
        }

        private void LoginInfoPage_Click(object sender, EventArgs e)
        {

        }
    }
}
