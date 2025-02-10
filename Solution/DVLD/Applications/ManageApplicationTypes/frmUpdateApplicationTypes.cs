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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DVLD.Applications.ManageApplicationTypes
{
    public partial class frmUpdateApplicationTypes : Form
    {

        int ApplicationID { get; set; }

        clsManageApplicationTypesBusiness Application = new clsManageApplicationTypesBusiness();
        public frmUpdateApplicationTypes(int ApplicationID)
        {
            InitializeComponent();
            this.ApplicationID = ApplicationID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) && string.IsNullOrWhiteSpace(txtFees.Text))
            {
                MessageBox.Show("All Fields Are Required");
            }
            else
            {
                Application.ApplicationTypeTitle = txtTitle.Text;
                Application.ApplicationTypeFees = Convert.ToDecimal(txtFees.Text);

                if (Application.Save() )
                {
                    MessageBox.Show("Saved");
                } else
                {
                    MessageBox.Show("Failed To Save");

                }

            }
        }



        private void FindApplicationType()
        {
            Application = clsManageApplicationTypesBusiness.FindApplicationType(ApplicationID);
        }

        private void FillUpdateApplicationData()
        {
            FindApplicationType();
            lblID.Text = ApplicationID.ToString();
            txtTitle.Text = Application.ApplicationTypeTitle;
            txtFees.Text = Application.ApplicationTypeFees.ToString();
        }

        private void frmUpdateApplicationTypes_Load(object sender, EventArgs e)
        {
            FillUpdateApplicationData();
        }


        private void HandleValidating(object sender, CancelEventArgs e)
        {


            if (ActiveControl == btnClose)
            {
                e.Cancel = false; // Allow the validation to pass without error
                return;
            }

            System.Windows.Forms.TextBox textBox = sender as System.Windows.Forms.TextBox;

            if (string.IsNullOrWhiteSpace(textBox.Text))
            {

                e.Cancel = true;
                errorProvider1.SetError(textBox, "This Field Is Required");
            }

            else
            {
                e.Cancel = false;
                errorProvider1.SetError(textBox, "");
                return;
            }


        }


        private void txtTitle_Validating(object sender, CancelEventArgs e)
        {
            HandleValidating(sender, e);
        }

        private void txtFees_Validating(object sender, CancelEventArgs e)
        {

            if (ActiveControl == btnClose)
            {
                e.Cancel = false; // Allow the validation to pass without error
                return;
            }

            if (!decimal.TryParse(txtFees.Text, out _))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFees, "Please enter a valid decimal number.");

           
            }
            else
            {
                errorProvider1.SetError(txtFees, "");

            }


        }



    }
}
