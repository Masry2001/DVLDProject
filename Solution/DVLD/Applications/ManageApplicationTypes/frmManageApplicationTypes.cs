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

namespace DVLD.Applications
{
    public partial class frmManageApplicationTypes : Form
    {
        public frmManageApplicationTypes()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void ListApplicationTypes()
        {
            dataGridView1.DataSource = clsManageApplicationTypesBusiness.ListApplicationTypes();
            lblApplicationsNumber.Text = dataGridView1.RowCount.ToString();
        }

        private void frmManageApplicationTypes_Load(object sender, EventArgs e)
        {
            ListApplicationTypes();
        }

        private void editPersonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int SelectedApplicationID = (int)dataGridView1.CurrentRow.Cells[0].Value;

            frmUpdateApplicationTypes frm = new frmUpdateApplicationTypes(SelectedApplicationID);
            frm.ShowDialog();
            ListApplicationTypes();

        }
    }
}
