using DVLD.People;
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
    public partial class ctrlPersonCardWithFilter : UserControl
    {
       
        public int PersonID { get; set; }

        public string NationalNo { get; set; }

       

        public ctrlPersonCardWithFilter() // Constructor is the first Called Function when ctrlPersoncArdWithFilter Is Called
        {
            InitializeComponent(); 

            comboBox1.SelectedIndex = 0;
            maskedTextBox1.SelectionStart = 0;


        }


        public void LoadPersonData()
        {

            maskedTextBox1.Text = PersonID.ToString();
            ctrlPersonCard2.PersonID = PersonID;

            ctrlPersonCard2.PersonCardLoadUsingID();

            FilterBox.Enabled = false;
        }
   

        private void ctrlPersonCardWithFilter_Load(object sender, EventArgs e)
        {
    

        }

        // this is the Search Button
        private void button1_Click(object sender, EventArgs e)
        {

            string selectedItem = comboBox1.SelectedItem.ToString();

            if (maskedTextBox1.Text == "")
            {
                ctrlPersonCard2.ResetPersonData();
            }

            if (selectedItem == "PersonID")
            {
                int PersonID = -1;

                if (int.TryParse(maskedTextBox1.Text, out PersonID))
                {
                    // The conversion was successful, use personID as needed
                    // Send The ID from TextBox to ctrlPersonCard 
                    this.PersonID = PersonID;
                    ctrlPersonCard2.PersonID = PersonID;
                    ctrlPersonCard2.PersonCardLoadUsingID();


                    // after sending PersonID to ctrlPersonCard2 get the NationlNo back from ctrlPersonCard2 to here
                    NationalNo = ctrlPersonCard2.NationalNo;


                }
                else
                {
                    // The conversion failed, handle the error
                    MessageBox.Show("Invalid input, please enter a valid ID.");
                }
            }
            else
            {
                if (maskedTextBox1.Text != "")
                {
                    // Send National No to CtrlPersonCard 
                    ctrlPersonCard2.NationalNo = maskedTextBox1.Text;
                    this.NationalNo = maskedTextBox1.Text;
                    
                    ctrlPersonCard2.PersonCardLoadUsingNationalNo();
                    // after sending nationalNo to ctrlPersonCard2 get the id back from ctrlPersonCard2 to here
                    PersonID = ctrlPersonCard2.PersonID;


                } 
                else
                {
                    MessageBox.Show("Invalid input, please enter National No.");

                }
            }
                
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            string selectedItem = comboBox1.SelectedItem.ToString();

            if (selectedItem == "PersonID")
            {
                maskedTextBox1.Mask = "0000"; // Allows a 4-digit number

            }
            else
            {
                maskedTextBox1.Mask = ""; // Allow String or Numbers
            }

        }


        private bool comboBoxFocused = false;

        private void comboBox1_Click(object sender, EventArgs e)
        {
            comboBoxFocused = true;

        }

        private void maskedTextBox1_Validating(object sender, CancelEventArgs e)
        {

            if (comboBoxFocused)
            {
                // Skip validation if ComboBox is focused
                e.Cancel = false;
                errorProvider1.SetError(maskedTextBox1, "");
                return;
            }

            

            if (string.IsNullOrEmpty(maskedTextBox1.Text))
            {
                string selectedItem = comboBox1.SelectedItem.ToString();

                e.Cancel = true;
                maskedTextBox1.Focus();

                if (selectedItem == "PersonID")
                {
                    errorProvider1.SetError(maskedTextBox1, "Provide PersonID");
                } 
                else
                { 
                    errorProvider1.SetError(maskedTextBox1, "Provide NationalNo");

                }
            }
            else
            {

                e.Cancel = false;
                errorProvider1.SetError(maskedTextBox1, "");

            }
        }

        private void maskedTextBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // this will not allow spaces
            if (!char.IsLetterOrDigit(e.KeyChar) && !char.IsControl(e.KeyChar))
            {
                e.Handled = true;
            }
        }



        public bool IsPersonLoded()
        {
            return ctrlPersonCard2.PersonLoded;
        }

        private void maskedTextBox1_TextChanged(object sender, EventArgs e)
        {
            ctrlPersonCard2.ResetPersonData();

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            frmAddUpdatePerson frm = new frmAddUpdatePerson(-1);

            frm.ShowDialog();


            if (frm.PersonID != -1)
            {

                comboBox1.SelectedIndex = 0;
                maskedTextBox1.Text = frm.PersonID.ToString();

                // this is the Search Button
                button1_Click(sender, e);
            }

            // here Recive PersonID From frmAddUpdatePerson
            // then put the PeronID in maskedTextBox // maskedTextBox1.text = PersonID.toString();
            // then call the Search Button //button1_Click(sender, e)


        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
