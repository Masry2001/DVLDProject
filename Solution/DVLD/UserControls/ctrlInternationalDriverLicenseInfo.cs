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
using System.IO;

namespace DVLD.UserControls
{
    public partial class ctrlInternationalDriverLicenseInfo : UserControl
    {

        public int InternationalLicenseID { get; set; }
        public ctrlInternationalDriverLicenseInfo()
        {
            InitializeComponent();
        }



        public void FillInternationalLicenseInfo()
        {

            DataRow InternationalLicenseRecord = clsInternationalLicensesBusiness.GetInternaionalLicenseRecord(InternationalLicenseID);

            int DriverID = (int)InternationalLicenseRecord["DriverID"];

            int PersonID = clsDriverBusiness.GetPersonID(DriverID);

            DataTable db = clsPersonBusiness.FilterPeopleByID(PersonID);
            DataRow PersonRecord = db.Rows[0];

            // 1- Name
            lblName.Text = clsPersonBusiness.GetFullNameUsingPersonID(PersonID);

            // 2- International LicenseID
            lblILID.Text = InternationalLicenseID.ToString();

            // 3- LicenseID
            lblLID.Text = InternationalLicenseRecord["IssuedUsingLocalLicenseID"].ToString();

            // 4- NationalNO
            lblNationalNo.Text = PersonRecord["NationalNo"].ToString();

            // 5- Gendor
            lblGendor.Text = (bool)PersonRecord["Gendor"] == true ? "Female" : "Male";
            HanldeIcon((bool)PersonRecord["Gendor"]);

            // 6- Issue Date
            lblIssueDate.Text = ((DateTime)InternationalLicenseRecord["IssueDate"]).ToString("dd/MM/yyyy");

            // 7- I.App.ID
            lblAppID.Text = InternationalLicenseRecord["ApplicationID"].ToString();

            // 8- IsActive
            lblIsActive.Text = (bool)InternationalLicenseRecord["IsActive"] == true ? "Yes": "No";

            // 9- DateOfBirth
            lblDateOfBirth.Text = ((DateTime) PersonRecord["DateOfBirth"]).ToString("dd/MM/yyyy");

            // 10- DriverID
            lblDriverID.Text = InternationalLicenseRecord["DriverID"].ToString();

            // 11- ExpirationDate
            lblExpirationDate.Text = ((DateTime)InternationalLicenseRecord["ExpirationDate"]).ToString("dd/MM/yyyy");

            // 12- pictureBox

            string ImagePath = PersonRecord["ImagePath"] != DBNull.Value ? (string)PersonRecord["ImagePath"] : "";
            HandlePictureBox(ImagePath, (bool)PersonRecord["Gendor"]);

        }

        private void HandlePictureBox(string ImagePath, bool Gendor)
        {



            if (File.Exists(ImagePath) && ImagePath != null)
            {
                pictureBox1.ImageLocation = ImagePath;

            }
            else
            {
                if (Gendor == false)
                {
                    pictureBox1.ImageLocation = "C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\male72px.png";
                }
                else
                {
                    pictureBox1.ImageLocation = "C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\female72px.png";
                }


            }
        }

        private void HanldeIcon(bool Gendor)
        {
            if (Gendor == false)
            {
                lblGendor.Text = "Male";
                lblGendorIcon.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\male32px.png");
                lblNameIcon.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\male32px.png");
            }
            else
            {
                lblGendor.Text = "Female";

                lblGendorIcon.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\female32px.png");
                lblNameIcon.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\female32px.png");

            }
        }

        private void ctrlInternationalDriverLicenseInfo_Load(object sender, EventArgs e)
        {

        }
    }
}
