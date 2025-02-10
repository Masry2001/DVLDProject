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
    public partial class frmShowInternationalLicenseInfo : Form
    {

        int InternationalLicenseID {  get; set; }
        public frmShowInternationalLicenseInfo(int InternationalLicenseID)
        {
            InitializeComponent();
            this.InternationalLicenseID = InternationalLicenseID;
        }

        private void frmShowInternationalLicenseInfo_Load(object sender, EventArgs e)
        {
            // send  InternationalLicenseID to ctrlInternationalDriverLicenseInfo
            ctrlInternationalDriverLicenseInfo1.InternationalLicenseID = this.InternationalLicenseID;
            ctrlInternationalDriverLicenseInfo1.FillInternationalLicenseInfo();


        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
