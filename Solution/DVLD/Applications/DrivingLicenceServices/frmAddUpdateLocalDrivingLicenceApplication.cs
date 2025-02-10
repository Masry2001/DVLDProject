using DVLD.Login;
using DVLD_BusinessLayer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications.DrivingLicenceServices
{
    public partial class frmAddUpdateLocalDrivingLicenceApplication : Form
    {

        private int ApplicationID = -1;

        private int LocalDrivingLicenceApplicationID = -1;  

        private string ApplicationTypeTitle = "New Local Driving License Service";

        private int ApplicationTypeID = -1;

        public enum enMode
        {
            AddNew,
            Update
        }


        public enMode Mode;

        public frmAddUpdateLocalDrivingLicenceApplication(int LocalDrivingLicenceApplicationID)
        {
            InitializeComponent();

            this.LocalDrivingLicenceApplicationID = LocalDrivingLicenceApplicationID;

            if (this.LocalDrivingLicenceApplicationID == -1 ) 
            {
                Mode = enMode.AddNew;
            }
            else
            {
                 Mode = enMode.Update;
            }


        }


        private void FillLocalDrivingLicenseApplicationData()
        {

            

            // You Have Local Driving License ApplicationID 
            // From Local Driving License ApplicationID You Can Get ApplicationID And LicenseID
            // From ApplicationID You Can Get PersonID

            int ApplicationID = clsLocalDrivingLicenceApplicationBusiness.GetApplicationIDUsingLDLAppID(LocalDrivingLicenceApplicationID);
            this.ApplicationID = ApplicationID;

            int LicenseClassID = clsLocalDrivingLicenceApplicationBusiness.GetLicenseClassIDUsingLDLAppID(LocalDrivingLicenceApplicationID);

            int PersonID = clsManageApplicationsBusiness.GetPersonIDUsingApplicationID(ApplicationID);

            ctrlPersonCardWithFilter1.PersonID = PersonID;
            ctrlPersonCardWithFilter1.LoadPersonData();

            string ClassName = clsLicenseClassesBusiness.FindLicenseClassNameUsingLicenceClassID(LicenseClassID);

            lblHeader.Text = "Update Local Driving License Application";
            lblLocalDrivingLicenceApplicationID.Text = LocalDrivingLicenceApplicationID.ToString();
            lblDate.Text = clsManageApplicationsBusiness.GetDateUsingApplicationID(ApplicationID).ToString();
            comboBox1.SelectedItem = ClassName;



        }


        private void frmAddUpdateLocalDrivingLicenceApplication_Load(object sender, EventArgs e)
        {

            int ApplicationTypeID = clsManageApplicationTypesBusiness.FindApplicationTypeIDUsingApplicationTypeTitle(ApplicationTypeTitle);
            this.ApplicationTypeID = ApplicationTypeID;

            comboBox1.SelectedIndex = 2;
            lblDate.Text = DateTime.Now.ToString("yyyy-M-d");

            lblFees.Text = clsManageApplicationTypesBusiness.GetApplicationFees(ApplicationTypeID).ToString();

            lblUser.Text = clsGlobalSettings.UserName;

            if (Mode == enMode.Update)
            {
                FillLocalDrivingLicenseApplicationData();
                ctrlPersonCardWithFilter1.FilterBox.Enabled = true;

            }
        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void HanldeMovingToApplicationInfoTab()
        {

            if (Mode == enMode.AddNew)
            {

                // PersonLoded
                if (ctrlPersonCardWithFilter1.IsPersonLoded())
                {
                    //int PersonID = ctrlPersonCardWithFilter1.PersonID;


                    tabControl1.SelectedIndex = 1;

                }
                else
                {

                    MessageBox.Show("You Have To Search For A Person Or Add A New One To Be Able To Create Driving Licence.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    tabControl1.SelectedIndex = 0;


                }

            }
            else
            {
                tabControl1.SelectedIndex = 1;

            }


        }


        private void btnNext_Click(object sender, EventArgs e)
        {
            HanldeMovingToApplicationInfoTab();

        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (tabControl1.SelectedIndex == 1)
            {
                HanldeMovingToApplicationInfoTab();

            }
        }



        private void btnBack_Click(object sender, EventArgs e)
        {
            tabControl1.SelectedIndex = 0;

        }




        private void MakeApplication()
        {

            int ApplicantPersonID = ctrlPersonCardWithFilter1.PersonID;

            DateTime ApplicationDate = DateTime.Now;

       
            int ApplicationStatus = 1; // New

            DateTime LastStatusDate = DateTime.Now; // check this, need modification After Changing the Status


            // Using ApplicationTypeID Get ApplicationFees
            decimal PaidFees = clsManageApplicationTypesBusiness.GetApplicationFees(ApplicationTypeID);

            int CreatedByUserID = clsUserBusiness.FindUserIDUsingPasswordAndUserName(clsGlobalSettings.UserName, clsGlobalSettings.Password);


            ApplicationID = clsManageApplicationsBusiness.MakeApplicationAndReturnApplicationID(ApplicantPersonID, ApplicationDate, ApplicationTypeID, ApplicationStatus, LastStatusDate, PaidFees, CreatedByUserID);

        }

        private void MakeALocalDrivingLicenceApplication()
        {
            // To make a LocalDrivingLicenceApplicaitonID You need:
            // 1- ApplicationID
            // 2- LicenseClassID

            MakeApplication();

            string ClassName = comboBox1.SelectedItem.ToString();
            int LicenseClassID = clsLicenseClassesBusiness.FindLicenseClassIDUsingLicenceClassName(ClassName);

            LocalDrivingLicenceApplicationID = clsLocalDrivingLicenceApplicationBusiness.MakeALocalDrivingLicenseApplication(ApplicationID, LicenseClassID);

            lblLocalDrivingLicenceApplicationID.Text = LocalDrivingLicenceApplicationID.ToString();
           
            lblHeader.Text = "Update Local Driving License Application";
            MessageBox.Show($"Done, Local Driving License Application Adeded With ID: {LocalDrivingLicenceApplicationID}");

            Mode = enMode.Update;
        
        }

        private void UpdateApplication()
        {
            // Update Person ID
            int ApplicantPersonID = ctrlPersonCardWithFilter1.PersonID;

            MessageBox.Show(ApplicationID.ToString());// Propblem in ApplicationID
            if (clsManageApplicationsBusiness.UpdateApplication(ApplicationID, ApplicantPersonID))
            {
                MessageBox.Show("PersonID Updated, Application Updated");
            } 
            else
            {
                MessageBox.Show("PersonID Not Updated, Application Not Updated");
            }


        }

        private void UpdateLocalDrivingLicenceApplication()
        {
            // Update PeronID And LicenseClassID
            UpdateApplication();

            string ClassName = comboBox1.SelectedItem.ToString();
            int LicenseClassID = clsLicenseClassesBusiness.FindLicenseClassIDUsingLicenceClassName(ClassName);

            if ( clsLocalDrivingLicenceApplicationBusiness.UpdateLocalDrivingLicenseApplication(LocalDrivingLicenceApplicationID, LicenseClassID))
            {
                MessageBox.Show("LicenseClassID Updated, LocalDrivngLicenseApplication Updated");
            } 
            else
            {
                MessageBox.Show("LicenseClassID Not Updated, LocalDrivngLicenseApplication Not Updated");

            }

                
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            // Before Saving
            // If Person Try To make an another applicatioin for the same Class Refuse it
            string NationalNo = ctrlPersonCardWithFilter1.NationalNo;
            string SpecifiedClassName = comboBox1.SelectedItem.ToString();

            if (ctrlPersonCardWithFilter1.IsPersonLoded())
            {
                if (clsLocalDrivingLicenceApplicationBusiness.IsPersonHasTheSpecifiedClassAndStatusIsNewOrCompleted(NationalNo, SpecifiedClassName))
                {
                    MessageBox.Show("Not Saved, The Person Has The Specified Class And Status Is New Or Completed, Choose Another Class.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    if (Mode == enMode.AddNew)
                    {

                        MakeALocalDrivingLicenceApplication();

                    }
                    else
                    {
                        UpdateLocalDrivingLicenceApplication();
                    }

                }

            } else
            {
                MessageBox.Show("Fill Out The Reqired Fields Before Saving");
            }


        }


        private void ChangeLblLicenceClassIcon()
        {
            string selectedItem = comboBox1.SelectedItem.ToString();
            if (selectedItem == "Class 1 - Small Motorcycle")
            {
                lblLicenceClassIcon.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\smallMotorcycle32px.png");
            }
            else if (selectedItem == "Class 2 - Heavy Motorcycle License")
            {
                lblLicenceClassIcon.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\heavyMotorcycle32px.png");

            }
            else if (selectedItem == "Class 3 - Ordinary driving license")
            {
                lblLicenceClassIcon.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\car32px.png");

            }
            else if (selectedItem == "Class 4 - Commercial")
            {
                lblLicenceClassIcon.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\taxi32px.png");

            }
            else if (selectedItem == "Class 5 - Agricultural")
            {

                lblLicenceClassIcon.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\tractor32px.png");
            }
            else if (selectedItem == "Class 6 - Small and medium bus")
            {
                lblLicenceClassIcon.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\bus32px.png");

            }
            else if (selectedItem == "Class 7 - Truck and heavy vehicle")
            {
                lblLicenceClassIcon.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\truck32px.png");

            }
            else
            {
                lblLicenceClassIcon.Image = Image.FromFile("C:\\Users\\mohamed el masry\\source\\repos\\DVLD\\Resources\\car32px.png");

            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ChangeLblLicenceClassIcon();
        }



    }
}
