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
    public partial class frmVisionTest : Form
    {

        int LDLAppID {  get; set; }

        int TestType { get; set; }



        public frmVisionTest(int LDLAppID)
        {
            InitializeComponent();
            this.LDLAppID = LDLAppID;
            ctrlLDLAppCard1.LDLAppID = LDLAppID;
            TestType = 1; // VisionTest

        }

        private void frmVisionTest_Load(object sender, EventArgs e)
        {
            ListAppoinmentsForLocalDrivingLicenseApplicationID();
        }

        private void ListAppoinmentsForLocalDrivingLicenseApplicationID()
        {
            dataGridView1.DataSource = clsTestsBusiness.ListAppoinmentsForLocalDrivingLicenseApplicationID(LDLAppID, TestType);
            lblRecordsNumber.Text = dataGridView1.RowCount.ToString();
            ctrlLDLAppCard1.HandlePassedTests();


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            // Prevent Add If There is an Active appoinment

            bool IsThereIsAnActiveAppoinment = clsTestsBusiness.IsThereIsAnActiveAppointmentForLocalDrivingLicenseApplicationIDAndTestType(LDLAppID, TestType);


            if (IsThereIsAnActiveAppoinment)
            {

                MessageBox.Show("There Is An Active Appointment For This Test", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);

            } 
            else if (clsTestsBusiness.IsLDLAppIDHasLockedTestAppointment(LDLAppID, TestType))
            {

                int TestAppointmentID = clsTestsBusiness.GetLastLockedTestAppointmentID(LDLAppID, TestType);
                bool TestResult = clsTestsBusiness.GetTestResultForTestAppointmentID(TestAppointmentID);


                if (TestResult == true)
                {
                    // Passed The Test
                    MessageBox.Show("You Have Passed This Test", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Information);

                }
                else if (TestResult == false)
                {
                    // Failed In Test // In Failed Mode Send -2
                    // -2 => Retake Test

                    frmScheduleVisionTest frm = new frmScheduleVisionTest(LDLAppID, -2);
                    frm.ShowDialog();
                    ListAppoinmentsForLocalDrivingLicenseApplicationID();
                }
            }
            else
            {
                // Mode Is Add // In Add Mode Send -1 
                frmScheduleVisionTest frm = new frmScheduleVisionTest(LDLAppID, -1);
                frm.ShowDialog();
                ListAppoinmentsForLocalDrivingLicenseApplicationID();
            }


        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Mode Edit // In Edit Mode Send AppointmenetID
            int AppointmentID = (int) dataGridView1.CurrentRow.Cells[0].Value;


            frmScheduleVisionTest frm = new frmScheduleVisionTest(LDLAppID, AppointmentID);
            frm.ShowDialog();
            ListAppoinmentsForLocalDrivingLicenseApplicationID();
        }



        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime AppointmentDate = (DateTime)dataGridView1.CurrentRow.Cells[1].Value;
            int AppointmentID = (int)dataGridView1.CurrentRow.Cells[0].Value;

            frmTakeVisionTest frm = new frmTakeVisionTest(LDLAppID, AppointmentID, AppointmentDate);
            frm.ShowDialog();
            ListAppoinmentsForLocalDrivingLicenseApplicationID();

        }
    }
}
