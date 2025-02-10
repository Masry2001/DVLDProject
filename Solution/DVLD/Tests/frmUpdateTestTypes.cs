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
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.Tests
{
    public partial class frmUpdateTestTypes : Form
    {

        int TestTypeID {  get; set; }

        clsTestsBusiness TestType = new clsTestsBusiness();

        public frmUpdateTestTypes(int TestTypeID)
        {
            InitializeComponent();
            this.TestTypeID = TestTypeID;
        }


        private void FindApplicationType()
        {
            TestType = clsTestsBusiness.FindTestType(TestTypeID);
        }

        private void FillUpdateApplicationData()
        {
            FindApplicationType();
            lblID.Text = TestTypeID.ToString();
            txtTitle.Text = TestType.TestTypeTitle;
            txtDescription.Text = TestType.TestTypeDescription;
            txtFees.Text = TestType.TestTypeFees.ToString();
        }

        private void frmUpdateTestTypes_Load(object sender, EventArgs e)
        {
            FillUpdateApplicationData();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void txtDescription_Validating(object sender, CancelEventArgs e)
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


        private void btnSave_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtTitle.Text) && string.IsNullOrWhiteSpace(txtDescription.Text) && string.IsNullOrWhiteSpace(txtFees.Text))
            {
                MessageBox.Show("All Fields Are Required");
            }
            else
            {
                TestType.TestTypeTitle = txtTitle.Text;
                TestType.TestTypeDescription = txtDescription.Text;
                TestType.TestTypeFees = Convert.ToDecimal(txtFees.Text);

                if (TestType.Save())
                {
                    MessageBox.Show("Saved");
                }
                else
                {
                    MessageBox.Show("Failed To Save");

                }

            }
        }
    }
}
