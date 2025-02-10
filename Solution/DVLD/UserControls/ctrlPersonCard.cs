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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO;

namespace DVLD.UserControls
{
    public partial class ctrlPersonCard : UserControl
    {

        // we send the personID and national no here
        public int PersonID { get; set; }

        public string NationalNo { get; set; }

        public bool PersonLoded { get; set; }

        clsPersonBusiness Person1 = new clsPersonBusiness();

        public ctrlPersonCard()
        {
            InitializeComponent();

        }

        public void LoadPersonData()
        {

            linkLabel1.Enabled = true;
            lblPersonID.Text = Person1.PersonID.ToString();

            lblNationalNo.Text = Person1.NationalNo;
            lblName.Text = Person1.FirstName + " " + Person1.SecondName + " " + Person1.ThirdName + " " + Person1.LastName;
            lblDateOfBirth.Text = Person1.DateOfBirth.ToString("dd/MM/yyyy");


            lblAddress.Text = Person1.Address;
            lblPhone.Text = Person1.Phone;
            lblEmail.Text = Person1.Email;

            if (Person1.Gendor == 0)
            {
                lblGendor.Text = "Male";
                lblGendorIcon.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\male32px.png");
            }
            else
            {
                lblGendor.Text = "Female";

                lblGendorIcon.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\female32px.png");
            }

            lblCountryName.Text = clsCountryBusiness.FindCountryNameByID(Person1.NationalityCountryID);

            if (File.Exists(Person1.ImagePath) && Person1.ImagePath != null)
            {
                pictureBox1.ImageLocation = Person1.ImagePath;

            }
            else
            {
                if (Person1.Gendor == 0)
                {
                    pictureBox1.ImageLocation = "C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\male72px.png";
                }
                else
                {
                    pictureBox1.ImageLocation = "C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\female72px.png";
                }


            }

            this.PersonID = Person1.PersonID; // added
            this.NationalNo = Person1.NationalNo; // added
            PersonLoded = true;

        }


        public void ResetPersonData()
        {
            lblPersonID.Text = "????";

            lblNationalNo.Text = "????";
            lblName.Text = "????";
            lblDateOfBirth.Text = "????";


            lblAddress.Text = "????";
            lblPhone.Text = "????";
            lblEmail.Text = "????";

            if (Person1.Gendor == 0)
            {
                lblGendor.Text = "????";
                lblGendorIcon.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\male32px.png");
            }
            else
            {
                lblGendor.Text = "????";

                lblGendorIcon.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\female32px.png");
            }

            lblCountryName.Text = "????";



            pictureBox1.ImageLocation = "C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\male72px.png";


            linkLabel1.Enabled = false;
            this.PersonID = -1; // added
            this.NationalNo = ""; // added
            PersonLoded = false;

        }



        public void PersonCardLoadUsingID()
        {

            if (Person1.FindPersonByID(PersonID))
            {

                LoadPersonData();
            }
            else
            {
                MessageBox.Show($"Person With ID: {PersonID} Not Found");
                // Make a Reset Function for all labels
                ResetPersonData();

            }
        }

        public void PersonCardLoadUsingNationalNo()
        {

            if (Person1.FindPersonByNationalNo(NationalNo))
            {

                LoadPersonData();
            }
            else
            {
                MessageBox.Show($"Person With NationalNo: {NationalNo} Not Found");
                // Make a Reset Function for all labels
                ResetPersonData();

            }
        }

        public void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson(PersonID);
            frm.ShowDialog();

            // you can use Delegate instead of this way
            Form parentForm = this.FindForm();
            if (parentForm.Name == "frmAddUpdateUser" || parentForm.Name == "frmUserInfo" || parentForm.Name == "frmChangePassword" || parentForm.Name == "frmVisionTest" || parentForm.Name == "frmPersonCardDetails")
            {
                PersonCardLoadUsingID();

            } 

        }


        private void ctrlPersonCard_Load(object sender, EventArgs e)
        {
            linkLabel1.Enabled = false;
            if (PersonID != -1 && PersonID != 0)
            {
                PersonCardLoadUsingID();

            }
        }

        public void DisableLinkLable()
        {
            linkLabel1.Enabled = false;
        }        
        
        public void HideLinkLable()
        {
            linkLabel1.Visible = false;
        }

    }
}
