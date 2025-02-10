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

namespace DVLD.UserControls
{
    public partial class ctrlLDLAppCard : UserControl
    {


        public int LDLAppID { get; set; }

        public int PersonID { get; set; }


        public ctrlLDLAppCard()
        {
            InitializeComponent();
        }

        private void ctrlLDLAppCard_Load(object sender, EventArgs e)
        {

            LoadLDLAppCardInfo();
            ChangeLblLicenceClassIcon();
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

        private void LoadDrivingLicenseApplicationInfo()
        {
            lblLDLAppID.Text = LDLAppID.ToString();
            int LicenseClassID = clsLocalDrivingLicenceApplicationBusiness.GetLicenseClassIDUsingLDLAppID(LDLAppID);
            lblLicenseClass.Text = clsLicenseClassesBusiness.FindLicenseClassNameUsingLicenceClassID(LicenseClassID);



            HandlePassedTests();

            lnkLicenseInfo.Enabled = false;
        }

        public void HandlePassedTests()
        {
            // Passed Tests
            int PassedTests = clsTestsBusiness.NumberOfPassedTests(LDLAppID);
            if (PassedTests == 0)
            {
                lblPassedTests.Text = "0/3";
            }
            else if (PassedTests == 1)
            {
                lblPassedTests.Text = "1/3";
            }
            else if (PassedTests == 2)
            {
                lblPassedTests.Text = "2/3";
            }
            else if (PassedTests == 3)
            {
                lblPassedTests.Text = "3/3";
            }
        }

        private void LoadApplicationBasicInfo()
        {
            int ApplicationID = clsLocalDrivingLicenceApplicationBusiness.GetApplicationIDUsingLDLAppID(LDLAppID);
            lblApplicationID.Text = ApplicationID.ToString();

            DataRow ApplicationRecord = clsManageApplicationsBusiness.GetApplicationRecordUsingID(ApplicationID);

            if (ApplicationRecord == null)
            {
                return;
            }

            byte Status = (byte)ApplicationRecord["ApplicationStatus"];

            if (Status == 1)
            {
                lblStatus.Text = "New";
            }
            else if (Status == 2)
            {
                lblStatus.Text = "Canceled";

            }
            else if (Status == 3)
            {

                lblStatus.Text = "Completed";
            }

            int ApplicationTypeID = (int) ApplicationRecord["ApplicationTypeID"];

            clsManageApplicationTypesBusiness ApplicationType = clsManageApplicationTypesBusiness.FindApplicationType(ApplicationTypeID);

            lblFees.Text = ApplicationType.ApplicationTypeFees.ToString();

            lblApplicationTypeTitle.Text = ApplicationType.ApplicationTypeTitle.ToString();

            int ApplicantPersonID = (int)ApplicationRecord["ApplicantPersonID"];

            this.PersonID = ApplicantPersonID;

            lblFullName.Text = clsPersonBusiness.GetFullNameUsingPersonID(ApplicantPersonID);

            lblDate.Text = Convert.ToDateTime(ApplicationRecord["ApplicationDate"]).ToString("dd/MM/yyyy");

            lblStatusDate.Text = Convert.ToDateTime(ApplicationRecord["LastStatusDate"]).ToString("dd/MM/yyyy");


            int UserID = (int) ApplicationRecord["CreatedByUserID"];

            lblUserName.Text = clsUserBusiness.FindUserNameUsingUserID(UserID);


        }

        public void LoadLDLAppCardInfo()
        {
            LoadDrivingLicenseApplicationInfo();
            LoadApplicationBasicInfo();
        }


        private void lnkPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmPersonCardDetails frm = new frmPersonCardDetails(PersonID);
            frm.ShowDialog();
            LoadApplicationBasicInfo();
        }
    }
}
