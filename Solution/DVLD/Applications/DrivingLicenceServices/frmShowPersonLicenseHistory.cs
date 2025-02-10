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

namespace DVLD.Applications.DrivingLicenceServices
{
    public partial class frmShowPersonLicenseHistory : Form
    {



        string NationalNo { get; set; }

        int PersonID { get; set; }

        int DriverID { get; set; }
        public frmShowPersonLicenseHistory( string NationalNo)
        {
            InitializeComponent();

            this.NationalNo = NationalNo;
            

        }

        private void frmShowPersonLicenseHistory_Load(object sender, EventArgs e)
        {
            ctrlPersonCard1.NationalNo = NationalNo;
            ctrlPersonCard1.PersonCardLoadUsingNationalNo();
            ctrlPersonCard1.HideLinkLable();

            PersonID = clsPersonBusiness.GetPersonIDUsingNationalNo(NationalNo);
            DriverID = clsDriverBusiness.GetDriverID(PersonID);

            LoadLocalLicenses();


        }


        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab == tabPage1)
            {
                LoadLocalLicenses();
            }
            else if (tabControl1.SelectedTab == tabPage2) 
            {
                LoadInternationalLicenses();
            }
        }

        private void LoadLocalLicenses()
        {

            dgvLocalLicense.DataSource = clsLicensesBusiness.GetDriverLicenses(DriverID);
            lblRecords.Text = dgvLocalLicense.RowCount.ToString();

        }        
        
        private void LoadInternationalLicenses()
        {

            dgvInternationalLicense.DataSource = clsInternationalLicensesBusiness.GetDriverInternationalLicenses(DriverID);
            lblRecords.Text = dgvInternationalLicense.RowCount.ToString();

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (tabControl1.SelectedTab == tabPage1)
            {
                
                int AppID = (int)dgvLocalLicense.CurrentRow.Cells[1].Value;
                int LicenseID = clsLicensesBusiness.GetLicenseIDUsingAppID(AppID);

                frmShowLicenseInfo frm = new frmShowLicenseInfo(LicenseID);
                frm.ShowDialog();
            }
            else if (tabControl1.SelectedTab == tabPage2)
            {
                int InternationalLicenseID = (int)dgvInternationalLicense.CurrentRow.Cells[0].Value;

                frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(InternationalLicenseID);
                frm.ShowDialog();

            }



        }
    }
}
