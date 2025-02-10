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

namespace DVLD.Tests.WrittenTest
{
    public partial class frmWrittenTest : Form
    {

        int LDLAppID { get; set; }

        int TestType { get; set; }

        public frmWrittenTest(int LDLAppID)
        {
            InitializeComponent();


            this.LDLAppID = LDLAppID;
            ctrlLDLAppCard1.LDLAppID = LDLAppID;
            TestType = 2; // WrittenTest
        }

        private void frmWrittenTest_Load(object sender, EventArgs e)
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

                    frmScheduleWrittenTest frm = new frmScheduleWrittenTest(LDLAppID, -2);
                    frm.ShowDialog();
                    ListAppoinmentsForLocalDrivingLicenseApplicationID();
                }
            }
            else
            {
                // Mode Is Add // In Add Mode Send -1 
                frmScheduleWrittenTest frm = new frmScheduleWrittenTest(LDLAppID, -1);
                frm.ShowDialog();
                ListAppoinmentsForLocalDrivingLicenseApplicationID();
            }

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Mode Edit // In Edit Mode Send AppointmenetID
            int AppointmentID = (int)dataGridView1.CurrentRow.Cells[0].Value;


            frmScheduleWrittenTest frm = new frmScheduleWrittenTest(LDLAppID, AppointmentID);
            frm.ShowDialog();
            ListAppoinmentsForLocalDrivingLicenseApplicationID();
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DateTime AppointmentDate = (DateTime)dataGridView1.CurrentRow.Cells[1].Value;
            int AppointmentID = (int)dataGridView1.CurrentRow.Cells[0].Value;

            frmTakeWrittenTest frm = new frmTakeWrittenTest(LDLAppID, AppointmentID, AppointmentDate);
            frm.ShowDialog();
            ListAppoinmentsForLocalDrivingLicenseApplicationID();
        }
    }
}
