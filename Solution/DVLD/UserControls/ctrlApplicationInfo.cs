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

namespace DVLD.UserControls
{
    public partial class ctrlApplicationInfo : UserControl
    {

        public int LicenseID { get; set; }

        public int InternationalLicenseID { get; set; }

        public int InternationalLicenseApplicationID { get; set; }


        public ctrlApplicationInfo()
        {
            InitializeComponent();
        }





        public void FillLocalLicenseData()
        {
            lblLLID.Text = LicenseID.ToString();
            
            decimal Fees = clsManageApplicationTypesBusiness.GetApplicationFees(6);
            lblFees.Text = Fees.ToString();

        }

        public void FillInternationalLicenseData()
        {

            lblILAppID.Text = InternationalLicenseApplicationID.ToString();

            lblILID.Text = InternationalLicenseID.ToString();

            // ApplicationDate
            lblAppDate.Text = clsManageApplicationsBusiness.GetDateUsingApplicationID(InternationalLicenseApplicationID).ToString("dd/mm/yyyy");

            //Issue Date for InternationalLicenseID

            DataRow InternationalLicenseRecord = clsInternationalLicensesBusiness.GetInternaionalLicenseRecord(InternationalLicenseID);

            lblIssueDate.Text = ((DateTime)InternationalLicenseRecord["IssueDate"]).ToString("dd/mm/yyyy");

            // Expiration Date
            lblExpirationDate.Text = ((DateTime)InternationalLicenseRecord["ExpirationDate"]).ToString("dd/mm/yyyy");

            // user
            lblCreatedBy.Text = InternationalLicenseRecord["CreatedByUserID"].ToString();



        }

        public void ResetLocalLicenseData()
        {
            lblLLID.Text = "????";
        }




    }
}
