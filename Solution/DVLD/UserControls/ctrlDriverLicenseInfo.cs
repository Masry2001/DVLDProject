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
using System.Runtime.CompilerServices;

namespace DVLD.UserControls
{
    public partial class ctrlDriverLicenseInfo : UserControl
    {


        public int LicenseID { get; set; }

        public bool LicenseExist { get; set; }

        public ctrlDriverLicenseInfo()
        {
            InitializeComponent();
            LicenseExist = false;
        }





        public void LoadDriverLicenseInformation()
        {
            // First Of All Get LicenseID

            if (clsLicensesBusiness.IsLicenseExist(LicenseID)) {

                LicenseExist = true;


                int ApplicationID = clsLicensesBusiness.GetApplicationIDUsingLicenseID(this.LicenseID);

                DataRow ApplicationRecord = clsManageApplicationsBusiness.GetApplicationRecordUsingID(ApplicationID);

                DataRow LicenseRecord = clsLicensesBusiness.GetLicenseRecordUsingLicenseID(LicenseID); 



                //int ApplicantPersonID = (int)ApplicationRecord["ApplicantPersonID"];
                // 1- LicenseClass
                int LicenseClassID = (int)LicenseRecord["LicenseClass"];
                lblLicenseClass.Text = clsLicenseClassesBusiness.FindLicenseClassNameUsingLicenceClassID(LicenseClassID);
                ChangeLblLicenceClassIcon();

                // 2- Name
                int PersonID = (int)ApplicationRecord["ApplicantPersonID"];
                lblName.Text = clsPersonBusiness.GetFullNameUsingPersonID(PersonID);

                // 3- LicenseID
                lblLicenseID.Text = LicenseID.ToString();

                // 4- National No
                DataTable dt = clsPersonBusiness.FilterPeopleByID(PersonID);
                DataRow PersonRecord = dt.Rows[0];
                lblNationalNo.Text = (string)PersonRecord["NationalNo"];

                // 5- Gendor
                lblGendor.Text = (bool)PersonRecord["Gendor"] == true ? "Female" : "Male";
                HanldeIcon((bool)PersonRecord["Gendor"]);

                // 6- IssueDate
                lblIssueDate.Text = ((DateTime)LicenseRecord["IssueDate"]).ToString("dd/MM/yyyy");

                // 7- IssueReason
                byte IssueReason = (byte)LicenseRecord["IssueReason"];
                // Issue Reason 1 => First Time
                // 2 => 
                // 3 => 
                // 4 => 
                if (IssueReason == 1)
                {
                    lblIssueReason.Text = "First Time";
                }
                else if (IssueReason == 2)
                {
                    lblIssueReason.Text = "Renew License";
                }
                else if (IssueReason == 3)
                {
                    lblIssueReason.Text = "Replacment For Lost License";

                }
                else if (IssueReason == 4)
                {

                    lblIssueReason.Text = "Replacment For Damaged License";
                }
                else if (IssueReason == 5)
                {
                    lblIssueReason.Text = "Release Detained License";

                }
                else if (IssueReason == 6)
                {
                    lblIssueReason.Text = "New International License";

                }


                // 8- Notes
                lblNotes.Text = LicenseRecord["Notes"] != DBNull.Value ? LicenseRecord["Notes"].ToString() : "No Notes";

                // 9- IsActive
                lblIsActive.Text = ((bool)LicenseRecord["IsActive"] == true) ? "Yes" : "No";

                // 10- Date Of Birth
                lblDateOfBirth.Text = ((DateTime)PersonRecord["DateOfBirth"]).ToString("dd/MM/yyyy");


                // 11- DriverID
                lblDriverID.Text = LicenseRecord["DriverID"].ToString();

                // 12- ExpirationDate
                lblExpirationDate.Text = ((DateTime)LicenseRecord["ExpirationDate"]).ToString("dd/MM/yyyy");

                // 13- Is Detained
                if (clsDetainedLicensesBusiness.IsLicenseExistInDetainedLicensesList(LicenseID))
                {
                    bool result = clsDetainedLicensesBusiness.GetReleasedField(LicenseID);
                    if (result)
                    {
                        lblIsDetained.Text = "No";
                    }
                    else
                    {
                        lblIsDetained.Text = "Yes";

                    }
                }
                else
                {
                        lblIsDetained.Text = "No";

                }


                // 14-Image
                string ImagePath = PersonRecord["ImagePath"] != DBNull.Value ? (string)PersonRecord["ImagePath"] : "";
                HandlePictureBox(ImagePath, (bool)PersonRecord["Gendor"]);
            } else
            {
                MessageBox.Show("License Not Exist In The DataBase", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
           
            


        }

        public void ResetDriverLicenseInformation ()
        {


            LicenseExist = false;







            lblLicenseClass.Text = "????";

            lblName.Text = "????";

            lblLicenseID.Text = "????";

            lblNationalNo.Text = "????";

            lblGendor.Text = "????";

            lblIssueDate.Text = "????";

            lblIssueReason.Text = "????";


            lblNotes.Text = "????";

            lblIsActive.Text = "????";

            lblDateOfBirth.Text = "????";


            lblDriverID.Text = "????";

            lblExpirationDate.Text = "????";

            lblIsDetained.Text = "????";

            pictureBox1.ImageLocation = "C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\male72px.png";



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


        private void ChangeLblLicenceClassIcon()
        {
            string ClassName = lblLicenseClass.Text;
            if (ClassName == "Class 1 - Small Motorcycle")
            {
                lblLicenceClassIcon.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\smallMotorcycle32px.png");
            }
            else if (ClassName == "Class 2 - Heavy Motorcycle License")
            {
                lblLicenceClassIcon.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\heavyMotorcycle32px.png");

            }
            else if (ClassName == "Class 3 - Ordinary driving license")
            {
                lblLicenceClassIcon.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\car32px.png");

            }
            else if (ClassName == "Class 4 - Commercial")
            {
                lblLicenceClassIcon.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\taxi32px.png");

            }
            else if (ClassName == "Class 5 - Agricultural")
            {

                lblLicenceClassIcon.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\tractor32px.png");
            }
            else if (ClassName == "Class 6 - Small and medium bus")
            {
                lblLicenceClassIcon.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\bus32px.png");

            }
            else if (ClassName == "Class 7 - Truck and heavy vehicle")
            {
                lblLicenceClassIcon.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\truck32px.png");

            }
            else
            {
                lblLicenceClassIcon.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\car32px.png");

            }
        }

        private void ctrlDriverLicenseInfo_Load(object sender, EventArgs e)
        {

        }
    }
}
