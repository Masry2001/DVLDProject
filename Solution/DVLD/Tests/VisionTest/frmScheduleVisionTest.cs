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

namespace DVLD.Tests
{
    public partial class frmScheduleVisionTest : Form
    {

        int LDLAppID { get; set; }

        string LicenseClass { get; set; }

        string FullName { get; set; }

        int Trial { get; set; }

        DateTime AppointmentDate { get; set; }

        decimal PaidFees { get; set; }

        int TestTypeID { get; set; }

        int CreatedByUserID { get; set; }

        int AppointmentID { get; set; }

        int ApplicantPersonID { get; set; }

        int LicenseClassID { get; set; }

        int RetakeTestApplicationID { get; set; }



        public enum enMode
        {
            AddNewTestAppoinment,
            EditTestAppoinment,
            RetakeTest
        }

        public enMode _Mode;


        public frmScheduleVisionTest(int LDLAppID, int AppointmentID)
        {
            InitializeComponent();

            this.LDLAppID = LDLAppID;
            TestTypeID = 1; // VisionTest
            this.AppointmentID = AppointmentID;

            if (AppointmentID == -1)
            {
                _Mode = enMode.AddNewTestAppoinment;
            }
            else if (AppointmentID == -2)
            {
                _Mode = enMode.RetakeTest;

            } else
            {
                _Mode = enMode.EditTestAppoinment;
            }

        }

        private void frmScheduleVisionTest_Load(object sender, EventArgs e)
        {
            FillFrmScheduleVisionTest();
            DisableEditIfAppointmentIsLocked();

        }

        private void DisableEditIfAppointmentIsLocked()
        {

            if (clsTestsBusiness.CheckIfAppointmentIsLocked(AppointmentID))
            {

                dateTimePicker1.Value = clsTestsBusiness.GetAppointmentDate(AppointmentID);
                dateTimePicker1.Enabled = false;
                btnSave.Enabled = false;
                lblAppointmentIsLocked.Text = "Appointment Is Locked";
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



        private void FillFrmScheduleVisionTest()
        {
            lblLDLAppID.Text = LDLAppID.ToString();

            LicenseClassID = clsLocalDrivingLicenceApplicationBusiness.GetLicenseClassIDUsingLDLAppID(LDLAppID);
            lblLicenseClass.Text = clsLicenseClassesBusiness.FindLicenseClassNameUsingLicenceClassID(LicenseClassID);
            this.LicenseClass = lblLicenseClass.Text;
            ChangeLblLicenceClassIcon();


            int ApplicationID = clsLocalDrivingLicenceApplicationBusiness.GetApplicationIDUsingLDLAppID(LDLAppID);
            DataRow ApplicationRecord = clsManageApplicationsBusiness.GetApplicationRecordUsingID(ApplicationID);
            int ApplicationTypeID = (int)ApplicationRecord["ApplicationTypeID"];

            clsManageApplicationTypesBusiness ApplicationType = clsManageApplicationTypesBusiness.FindApplicationType(ApplicationTypeID);

            ApplicantPersonID = (int)ApplicationRecord["ApplicantPersonID"];
            lblFullName.Text = clsPersonBusiness.GetFullNameUsingPersonID(ApplicantPersonID);
            this.FullName = lblFullName.Text;

            int CreatedByUserID = (int)ApplicationRecord["CreatedByUserID"];
            this.CreatedByUserID = CreatedByUserID;

            // Trial
            int Trial = clsTestsBusiness.TrialsOfTestType(LDLAppID, TestTypeID);
            lblTrial.Text = Trial.ToString();
            this.Trial = Trial;

            this.AppointmentDate = dateTimePicker1.Value;

            // Problem With Fees, You Need To Get Fees Of TestType Not ApplicationType
            lblFees.Text = clsTestsBusiness.GetTestTypeFees(TestTypeID).ToString();


            HandleRetakeTestGroubBox();


        }

        private void HandleRetakeTestGroubBox()
        {

            int FirstTestAppointmentID = clsTestsBusiness.GetFirstTestAppointmentID(LDLAppID, TestTypeID);

            if ( _Mode == enMode.AddNewTestAppoinment || this.AppointmentID == FirstTestAppointmentID) 
            {

                gbRetakeVisionTest.Enabled = false;
                lblRetakeTestAppID.Text = "N/A";
                lblRetakeTestFees.Text = "0";

            }
            else if (_Mode == enMode.EditTestAppoinment)
            {
                lblHeader.Text = "Edit Schedule Test";
                gbRetakeVisionTest.Enabled = true;


                lblRetakeTestAppID.Text = clsManageApplicationsBusiness.GetRetakeTestApplicationID(AppointmentID).ToString();

                lblRetakeTestFees.Text = clsManageApplicationTypesBusiness.GetApplicationFees("Retake Test").ToString();

            } 
            else if (_Mode == enMode.RetakeTest)
            {

                lblHeader.Text = "Schedule Retake Test";
                gbRetakeVisionTest.Enabled = true;
                lblRetakeTestAppID.Text = "Not Added Yet"; 
                lblRetakeTestFees.Text = clsManageApplicationTypesBusiness.GetApplicationFees("Retake Test").ToString();
            }

            

            decimal Fees = decimal.Parse(lblFees.Text);
            decimal RetakeTestFees = decimal.Parse(lblRetakeTestFees.Text);

            decimal TotalFees = Fees + RetakeTestFees;

            lblTotalFees.Text = TotalFees.ToString();

            this.PaidFees = Fees; 


        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.AddNewTestAppoinment)
            {
                // Add Mode
                AddTestAppoinment();
            }
            else if (_Mode == enMode.EditTestAppoinment)
            {
                // Edit Mode
                EditTestAppointment();
            }
            else if (_Mode == enMode.RetakeTest)
            {
                RetakeTest();
            }

            btnSave.Enabled = false;
            dateTimePicker1.Enabled = false;


        }

        private void AddTestAppoinment()
        {
            bool IsLocked = false;
            int TestAppointmentID = clsTestsBusiness.AddTestAppoinment(TestTypeID, LDLAppID, AppointmentDate, PaidFees, CreatedByUserID, IsLocked);


            if (TestAppointmentID != -1)
            {
                MessageBox.Show($"Data Saved, AppointmentID: {TestAppointmentID}", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);


                if (RetakeTestApplicationID != -1 && RetakeTestApplicationID != 0)
                {
                    // Add AppointmentID and RetakeTestApplicationID to TestAppointmentIDWithRetakeTestApplicationID Table
                    if (clsManageApplicationsBusiness.SaveTestAppointmentIDAndRetakeTestApplicationID(TestAppointmentID, RetakeTestApplicationID))
                    {
                        MessageBox.Show("Test Appointment ID Linked With Retake Test Application ID", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        MessageBox.Show("Test Appointment ID Not Linked With Retake Test Application ID", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                }


            }
            else
            {
                MessageBox.Show($"Faild To Save", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }




        }


        private void EditTestAppointment()
        {
            AppointmentDate = dateTimePicker1.Value;
            if (clsTestsBusiness.EditTestAppointment(AppointmentID, AppointmentDate))
            {
                MessageBox.Show("Updated", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
            } 
            else
            {
                MessageBox.Show("Failed To Update", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

            }
        }


        private void RetakeTest()
        {
            // 1- Make Application Of Type Retake 

            int ApplicationTypeID = 8;  // Retake Test
            int ApplicationStatus = 1; // New

            RetakeTestApplicationID = clsManageApplicationsBusiness.MakeApplicationAndReturnApplicationID(ApplicantPersonID, DateTime.Now, ApplicationTypeID, ApplicationStatus, DateTime.Now, PaidFees, CreatedByUserID);

            if (RetakeTestApplicationID != -1) 
            {
                MessageBox.Show($"Application Of Type Retake Test Estabilished With ID: {RetakeTestApplicationID}", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                lblRetakeTestAppID.Text = RetakeTestApplicationID.ToString();

                // 2- Make Test Appointment;
                AddTestAppoinment();

            } else
            {
                MessageBox.Show($"Faild To Make Application Of Type Retake Test", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }





    }
}
