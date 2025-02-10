using DVLD.Login;
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

namespace DVLD.Tests.StreetTest
{
    public partial class frmTakeStreetTest : Form
    {


        int LDLAppID { get; set; }

        DateTime AppointmentDate { get; set; }

        int TestAppointmentID { get; set; }


        public frmTakeStreetTest(int LDLAppID, int TestAppointmentID, DateTime AppointmentDate)
        {
            InitializeComponent();
            this.LDLAppID = LDLAppID;
            this.AppointmentDate = AppointmentDate;
            this.TestAppointmentID = TestAppointmentID;

        }

        private void frmTakeStreetTest_Load(object sender, EventArgs e)
        {
            FillTakeStreetTest();
            PreventModifyingTakeTestIfAppointmentIsLocked();
        }

        private void LockTakeTest()
        {
            btnSave.Enabled = false;
            rbFail.Enabled = false;
            rbPass.Enabled = false;
            txtNotes.Enabled = false;
        }

        private void PreventModifyingTakeTestIfAppointmentIsLocked()
        {
            if (clsTestsBusiness.CheckIfAppointmentIsLocked(TestAppointmentID))
            {
                LockTakeTest();

                int TestID = clsTestsBusiness.GetTestIDUsingTestAppointmentID(TestAppointmentID);

                int TestTypeID = clsTestsBusiness.GetTestTypeIDUsingTestAppointmentID(TestAppointmentID);

                lblTrial.Text = clsTestsBusiness.TrialsOfTestType(LDLAppID, TestTypeID).ToString();

                lblTestID.Text = TestID.ToString();
                txtNotes.Text = clsTestsBusiness.GetTestNotes(TestID);

                bool Result = clsTestsBusiness.IsTestResultPass(TestID);
                if (Result)
                {
                    rbPass.Checked = true;
                    lblTestResult.Text = "Test Has Finished, Result Is Pass";
                    lblTestResult.ForeColor = Color.Green;

                }
                else
                {
                    rbFail.Checked = true;
                    lblTestResult.Text = "Test Has Finished, Result Is Fail";
                    lblTestResult.ForeColor = Color.Red;

                }

            }
        }


        private void FillTakeStreetTest()
        {
            lblLDLAppID.Text = LDLAppID.ToString();

            int LicenseClassID = clsLocalDrivingLicenceApplicationBusiness.GetLicenseClassIDUsingLDLAppID(LDLAppID);
            lblLicenseClass.Text = clsLicenseClassesBusiness.FindLicenseClassNameUsingLicenceClassID(LicenseClassID);
            ChangeLblLicenceClassIcon();

            int ApplicationID = clsLocalDrivingLicenceApplicationBusiness.GetApplicationIDUsingLDLAppID(LDLAppID);
            DataRow ApplicationRecord = clsManageApplicationsBusiness.GetApplicationRecordUsingID(ApplicationID);
            int ApplicationTypeID = (int)ApplicationRecord["ApplicationTypeID"];

            clsManageApplicationTypesBusiness ApplicationType = clsManageApplicationTypesBusiness.FindApplicationType(ApplicationTypeID);

            int ApplicantPersonID = (int)ApplicationRecord["ApplicantPersonID"];
            lblFullName.Text = clsPersonBusiness.GetFullNameUsingPersonID(ApplicantPersonID);

            int TestTypeID = 3; // StreetTest
            lblTrial.Text = clsTestsBusiness.TrialsOfTestType(LDLAppID, TestTypeID).ToString();

            lblFees.Text = clsTestsBusiness.GetTestTypeFees(TestTypeID).ToString();

            lblDate.Text = AppointmentDate.ToString();

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

        private void ChangeResultIcon()
        {
            if (rbPass.Checked)
            {
                lblResultIcon.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\passed32px.png");

            }
            else
            {
                lblResultIcon.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\fail32px.png");

            }
        }


        private void rbPass_CheckedChanged(object sender, EventArgs e)
        {
            ChangeResultIcon();
        }

        private void rbFail_CheckedChanged(object sender, EventArgs e)
        {
            ChangeResultIcon();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are You Sure You Want To Save", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
            {
                TakeTest();
                LockTakeTest();

            }
        }

        private void TakeTest()
        {
            //After Taking The Test Lock The Appointment

            bool TestResult = rbPass.Checked;

            string Notes = txtNotes.Text;

            int UserID = clsUserBusiness.FindUserIDUsingPasswordAndUserName(clsGlobalSettings.UserName, clsGlobalSettings.Password);


            int TestID = clsTestsBusiness.TakeTest(TestAppointmentID, TestResult, Notes, UserID);

            if (TestID != -1)
            {
                MessageBox.Show($"Data Saved, TestID: {TestID}", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblTestID.Text = TestID.ToString();

                LockAppointment();

            }
            else
            {
                MessageBox.Show("Failed To Save");
            }
        }

        private void LockAppointment()
        {
            if (clsTestsBusiness.LockAppointment(TestAppointmentID))
            {
                MessageBox.Show("Success, Appointment Locked");
            }
            else
            {
                MessageBox.Show("Failed, Appointment Not Locked");

            }
        }




    }
}
