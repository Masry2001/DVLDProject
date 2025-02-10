using DVLD.UserControls;
using DVLD.Users;
using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD.People
{
    public partial class frmAddUpdatePerson : Form
    {


        // By Default Add New Mode
        clsPersonBusiness Person1 = new clsPersonBusiness();

        public int PersonID { get; set; }

        public frmAddUpdatePerson(int PersonID)
        {
            InitializeComponent();


            this.PersonID = PersonID;

            if (PersonID != -1 )
            {
                // update Mode
                Person1.FindPersonByID(PersonID);

            }


        }

  


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
            Form parentForm = this.FindForm();
           
        }

        private void HandleDateTimePicker()
        {
            dateTimePicker1.MinDate = DateTime.Now.AddYears(-100);
            dateTimePicker1.MaxDate = DateTime.Now.AddYears(-18);
            dateTimePicker1.Value = DateTime.Now.AddYears(-21);
        }

        private void ListCountriesInComboBox()
        {

            DataTable table = clsCountryBusiness.ListCountries();

            foreach (DataRow row in table.Rows)
            {
                comboBox1.Items.Add($"{row["CountryName"]}");
            }

            comboBox1.SelectedIndex = comboBox1.FindString("Egypt");
        }

        private void HandlePictureBox()
        {
            // if Image not exist and file Directory not Resources
            if (!File.Exists(pictureBox1.ImageLocation)) { 

                if (rbMale.Checked)
                {
                    pictureBox1.ImageLocation = "C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\male72px.png";
                } 
                else
                {
                    pictureBox1.ImageLocation = "C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\female72px.png";

                }
                lnkRemoveImage.Enabled = false;
            }

            string DirectoryPath = Path.GetDirectoryName(pictureBox1.ImageLocation);

            if (DirectoryPath == "C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources")
            {
                if (rbMale.Checked)
                {
                    pictureBox1.ImageLocation = "C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\male72px.png";
                }
                else
                {
                    pictureBox1.ImageLocation = "C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\female72px.png";

                }
                lnkRemoveImage.Enabled = false;
            }




        }

        private void frmAddUpdatePerson_Load(object sender, EventArgs e)
        {
            HandleDateTimePicker();
            ListCountriesInComboBox();

            if (PersonID != -1)
            {
                LoadPersonDataInEditMode();

            }

            HandlePictureBox();

        }

        private void rbMale_CheckedChanged(object sender, EventArgs e)
        {
            HandlePictureBox();
        }

        private void rbFemale_CheckedChanged(object sender, EventArgs e)
        {
            HandlePictureBox() ;
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                pictureBox1.ImageLocation = openFileDialog1.FileName;
                lnkRemoveImage.Enabled = true;
            }
        }

        private void lnkRemoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            pictureBox1.ImageLocation = null;
            lnkRemoveImage.Enabled = false;
            HandlePictureBox();

        }


        private void txtValidateNullOrEmpty(object sender, CancelEventArgs e)
        {
            System.Windows.Forms.TextBox textBox = sender as System.Windows.Forms.TextBox;

            if (string.IsNullOrEmpty(textBox.Text))
            {
                e.Cancel = true;
                textBox.Focus();
                errorProvider1.SetError(textBox, "This Field Should Have a Value");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(textBox, "");
            }
        }

        private void txtValidateKeyPress(object sender, KeyPressEventArgs e)
        {
            // Allow only letters and control characters (like backspace)
            if (!char.IsLetter(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Discard the character
            }
        }

        private void txtValidateKeyPressPhoneNumber(object sender, KeyPressEventArgs e)
        {
            // Allow only digits and control characters (like backspace)
            if (!char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true; // Discard the character
            }
        }

        
        private void txtValidateEmail(object sender, CancelEventArgs e)
        {
            System.Windows.Forms.TextBox textBox = sender as System.Windows.Forms.TextBox;

            if (textBox != null)
            {
                string email = textBox.Text;

                // Define the email regex pattern
                string pattern = @"^[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[a-zA-Z]{2,}$";

                // Validate the email
                if (!Regex.IsMatch(email, pattern))
                {
                    e.Cancel = true;
                    textBox.Focus();
                    errorProvider1.SetError(textBox, "Invalid email format. Please use the format textWithOrWithoutNumbers@gmail.com");
                }
                else
                {
                    e.Cancel = false;
                    errorProvider1.SetError(textBox, "");
                }
            }
        }

        


        private void HandleUniqueNationalNo(object sender, CancelEventArgs e)
        {
            System.Windows.Forms.TextBox textBox = sender as System.Windows.Forms.TextBox;
            if (!clsPersonBusiness.IsNationalNoUnique(textBox.Text))
            {

                e.Cancel = true;
                textBox.Focus();
                errorProvider1.SetError(textBox, "This Value Should Be Unique");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(textBox, "");
            }
        }


        private void txtFirstName_Validating(object sender, CancelEventArgs e)
        {
            txtValidateNullOrEmpty(sender, e);
        }

        private void txtFirstName_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtValidateKeyPress(sender, e);
        }



        private void txtSecondName_Validating(object sender, CancelEventArgs e)
        {
            txtValidateNullOrEmpty(sender , e);
        }

        private void txtSecondName_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtValidateKeyPress(sender , e);
        }

        private void txtThirdName_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtValidateKeyPress(sender, e);
        }

        private void txtLastName_Validating(object sender, CancelEventArgs e)
        {
            txtValidateNullOrEmpty(sender, e);
        }

        private void txtLastName_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtValidateKeyPress(sender, e);
        }

        private void txtNationalNo_Validating(object sender, CancelEventArgs e)
        {

            txtValidateNullOrEmpty(sender, e);

            if (txtNationalNo.Text != "")
            {
                HandleUniqueNationalNo(sender, e);
            }

        }


        private void txtPhone_Validating(object sender, CancelEventArgs e)
        {
            txtValidateNullOrEmpty(sender, e);
        }

        private void txtPhone_KeyPress(object sender, KeyPressEventArgs e)
        {
            txtValidateKeyPressPhoneNumber(sender, e);
        }

        private void txtEmail_Validating(object sender, CancelEventArgs e)
        {
            if (txtEmail.Text == "")
            {
                // no problem 
            } 
            else
            {
                // Check if the Email is Valid Fomat
                txtValidateEmail(sender, e);
            }
        }

        private void txtAddress_Validating(object sender, CancelEventArgs e)
        {
            txtValidateNullOrEmpty(sender, e);
        }


        private void AddNewPerson()
        {
            Person1.NationalNo  = txtNationalNo.Text;
            Person1.FirstName = txtFirstName.Text;
            Person1.SecondName = txtSecondName.Text;
            Person1.ThirdName = txtThirdName.Text;
            Person1.LastName = txtLastName.Text;
            Person1.DateOfBirth = dateTimePicker1.Value;

            Person1.Gendor = (byte)(rbMale.Checked ? 0 : 1);

            Person1.Address = txtAddress.Text;
            Person1.Phone = txtPhone.Text;
            Person1.Email = txtEmail.Text;

            Person1.NationalityCountryID = comboBox1.SelectedIndex + 1; // cehck this 

            Person1.ImagePath = pictureBox1.ImageLocation;

            if (Person1.SavePersonToDB())
            {
                MessageBox.Show("Done, Person Saved To DB");

            }
            else
            {
                MessageBox.Show("Failed To Save Person To DB");

            }
        }

        private void UpdatePerson()
        {
            AddNewPerson();

        }

        public void LoadPersonDataInEditMode()
        {
            lblHeader.Text = "Edit Person";

            PersonID = Person1.PersonID;
            lblPersonID.Text = PersonID.ToString();

            txtNationalNo.Text = Person1.NationalNo;
            txtFirstName.Text = Person1.FirstName;
            txtSecondName.Text = Person1.SecondName;
            txtThirdName.Text = Person1.ThirdName;
            txtLastName.Text = Person1.LastName;
            dateTimePicker1.Value = Person1.DateOfBirth;

            if (Person1.Gendor == 0)
            {
                rbMale.Checked = true;
            } else
            {
                rbFemale.Checked = true;
            }

            txtAddress.Text = Person1.Address;
            txtPhone.Text = Person1.Phone;
            txtEmail.Text = Person1.Email;

            int index = comboBox1.FindString(clsCountryBusiness.FindCountryNameByID(Person1.NationalityCountryID));
            comboBox1.SelectedIndex = index;


            // if there is an image other than the one in the Director Resources Enable Remove Image link
            pictureBox1.ImageLocation = Person1.ImagePath; // check 

            string DirectoryPath = Path.GetDirectoryName(pictureBox1.ImageLocation);
                

            if (File.Exists(pictureBox1.ImageLocation) && DirectoryPath != "C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources")
            {
                lnkRemoveImage.Enabled = true;
            }





        }

        private bool CheckThatAllFieldsHaveData()
        {
            if (txtNationalNo.Text != "" && txtFirstName.Text != "" && txtSecondName.Text != "" && txtLastName.Text != "" && txtAddress.Text != "" && txtPhone.Text != "" )
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

            if (CheckThatAllFieldsHaveData() )
            {
                if (PersonID == -1)
                {
                    AddNewPerson();
                    LoadPersonDataInEditMode();

                    SendPersonIDToctrlPersonCardWithFilter();

                }
                else
                {
                    UpdatePerson();

                }
            }
            else
            {
                    MessageBox.Show("You Must Fill All Required Fields");
            }

            




        }


        public void SendPersonIDToctrlPersonCardWithFilter()
        {
            // using Delegate
            // Send ID back To ctrlPersonCardWithFilter
            // then to ctrlPersonCard then call PersonCardLoadusingID
        }


    }
}
