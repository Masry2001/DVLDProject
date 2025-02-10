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
    public partial class frmShowLicenseInfo : Form
    {


        int LicenseID { get; set; }

        public frmShowLicenseInfo(int LicenseID)
        {
            InitializeComponent();

            this.LicenseID = LicenseID;
        }


        private void frmShowLicenseInfo_Load(object sender, EventArgs e)
        {


            LoadLicenseInformation();
        }


        private void LoadLicenseInformation()
        {

            ctrlDriverLicenseInfo1.LicenseID = LicenseID;
            ctrlDriverLicenseInfo1.LoadDriverLicenseInformation();


        }


        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
