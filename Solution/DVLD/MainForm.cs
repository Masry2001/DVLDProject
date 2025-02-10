using DVLD.Applications;
using DVLD.Applications.DetainLicense;
using DVLD.Applications.DrivingLicenceServices;
using DVLD.Applications.ManageApplications;
using DVLD.Login;
using DVLD.Tests;
using DVLD.Users;
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

namespace DVLD
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Maximized;
            this.FormBorderStyle = FormBorderStyle.None;
        }

        private void peopleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frmListPeople = new People.frmListPeople();
            frmListPeople.ShowDialog();
        }

        private void logOutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();

            frmLogin frmLogin = new frmLogin();
            frmLogin.ShowDialog();

            // Close the Main form
            this.Close();
        }

        private void usersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListUsers frmListUsers = new frmListUsers();
            frmListUsers.ShowDialog();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void currentUserInformationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsUserBusiness User = new clsUserBusiness();
            User = clsUserBusiness.FindUserUsingPasswordAndUserName(clsGlobalSettings.UserName, clsGlobalSettings.Password);
            frmUserInfo frm = new frmUserInfo(User.UserID);
            frm.ShowDialog();
        }

        private void changePasswordToolStripMenuItem_Click(object sender, EventArgs e)
        {
            clsUserBusiness User = new clsUserBusiness();
            User = clsUserBusiness.FindUserUsingPasswordAndUserName(clsGlobalSettings.UserName, clsGlobalSettings.Password);
            frmChangePassword frm = new frmChangePassword(User.UserID);
            frm.ShowDialog();
        }

        private void manageApplicationTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmManageApplicationTypes frm = new frmManageApplicationTypes();
            frm.ShowDialog();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void manageTestTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListTestTypes frm = new frmListTestTypes();
            frm.ShowDialog();
        }

        private void toolStripMenuItem12_Click(object sender, EventArgs e)
        {

        }

        private void localLicenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
    
            frmAddUpdateLocalDrivingLicenceApplication frm = new frmAddUpdateLocalDrivingLicenceApplication(-1);
            frm.ShowDialog();
        }

        private void localDrivingLicenceApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListLocalDrivingLicenseApplications frm = new frmListLocalDrivingLicenseApplications();
            frm.ShowDialog();
        }

        private void driversToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListDrivers frm = new frmListDrivers();
            frm.ShowDialog();
        }

        private void internationalLicenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmInternationalLicenseApplication frm = new frmInternationalLicenseApplication();
            frm.ShowDialog();
        }

        private void internationalDrivingLicenceApplicationsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListInternationalLicenseApplications frm = new frmListInternationalLicenseApplications();
            frm.ShowDialog();
        }

        private void renewDrivingLicenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmRenewLocalDrivingLicense frm = new frmRenewLocalDrivingLicense();
            frm.ShowDialog();
        }

        private void reToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReplacementForLostOrDamagedLicense frm = new frmReplacementForLostOrDamagedLicense();
            frm.ShowDialog();
        }

        private void manageDetainedLicensesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmListDetainedLicenses frm = new frmListDetainedLicenses();
            frm.ShowDialog();
        }

        private void detainLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {

            frmDetainLicense frm = new frmDetainLicense(); 
            frm.ShowDialog();

        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense(-1);
            frm.ShowDialog();
        }

        private void releaseDetainedDrivingLicenceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmReleaseDetainedLicense frm = new frmReleaseDetainedLicense(-1);
            frm.ShowDialog();
        }
    }
}
