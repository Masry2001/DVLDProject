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
    public partial class ctrlUserCard : UserControl
    {


        public int UserID { get; set; }

        clsUserBusiness User = new clsUserBusiness();

        public ctrlUserCard()
        {
            InitializeComponent();
        }

        private void ctrlPersonCard1_Load(object sender, EventArgs e)
        {

        }

        private void FillLoginInfo()
        {
            lblUserID.Text = User.UserID.ToString();
            lblUserName.Text = User.UserName.ToString();    
            lblIsActive.Text = ( User.IsActive == true ? "Yes" : "No" );
            lblIsActive.ForeColor = User.IsActive ? Color.Green : Color.Red;
        }

        private void FillPersonInfo()
        {
            ctrlPersonCard1.PersonID = User.PersonID;
            ctrlPersonCard1.PersonCardLoadUsingID();
        }

        private void FillUserCard()
        {
            FillPersonInfo();
            FillLoginInfo();

        }

        public void UserCardLoadUsingID()
        {
            if (UserID != -1)
            {
                User = clsUserBusiness.FindUserByID(UserID);
                FillUserCard();
            }
        }

        private void ctrlUserCard_Load(object sender, EventArgs e)
        {

        }
    }
}
