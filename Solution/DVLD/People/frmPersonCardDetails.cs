﻿using DVLD_BusinessLayer;
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

namespace DVLD.People
{
    public partial class frmPersonCardDetails : Form
    {


        public frmPersonCardDetails(int PersonID)
        {
            InitializeComponent();
            ctrlPersonCard1.PersonID = PersonID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
  
        }

        private void ctrlPersonCard1_Load(object sender, EventArgs e)
        {

        }
    }
}
