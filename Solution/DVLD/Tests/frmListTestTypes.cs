using DVLD.Applications.ManageApplicationTypes;
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
    public partial class frmListTestTypes : Form
    {
        public frmListTestTypes()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ListTestTypes()
        {
            dataGridView1.DataSource = clsTestsBusiness.ListTestTypes();
            lblTestsNumber.Text = dataGridView1.RowCount.ToString();
        }


        private void frmListTestTypes_Load(object sender, EventArgs e)
        {
            ListTestTypes();
        }

        private void editPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int SelectedTestID = (int)dataGridView1.CurrentRow.Cells[0].Value;

            frmUpdateTestTypes frm = new frmUpdateTestTypes(SelectedTestID);
            frm.ShowDialog();
            ListTestTypes();
        }
    }
}
