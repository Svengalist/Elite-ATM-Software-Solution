﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Elite_ATM_Class_Library;
using System.Data.SqlClient;

namespace Elite_ATM_Software
{
    public partial class FrmCreateAccount : Form
    {
        public FrmCreateAccount()
        {
            InitializeComponent();
        }
        private void btnOpenAccount_Click(object sender, EventArgs e)
        {
            
            try
            {
                if (rbBusiness.Checked || rbPersonal.Checked)
                {
                    if (rbBusiness.Checked)
                    {
                        UserTracker.currentUser.UserAccount = new Business();
                    }
                    else if (rbPersonal.Checked)
                    {
                        if (cbPersonalAccountType.Text == "Savings")
                        {
                            UserTracker.currentUser.UserAccount = new Savings();
                        }
                        else if (cbPersonalAccountType.Text == "") { throw new Exception("Select a Chequing or Savings Account"); }
                        else { UserTracker.currentUser.UserAccount = new Chequing(); }
                    }
                }
                else
                {
                    throw new Exception("Select an Account Type");
                }
                if (cbCurrency.Text == "")
                {
                    throw new Exception("Select a Currency");
                }
                else
                {
                    UserTracker.currentUser.UserAccount.Currency = cbCurrency.Text;
                    UserTracker.currentUser.UserAccount.Balance = 0.0f;
                }
                SqlConnection connection = new SqlConnection(Utilities.GetConnectionString());
                connection.Open();
                SqlCommand command = new SqlCommand($"INSERT INTO [Account] (Type, Balance, Currency, UserID) VALUES ('{UserTracker.currentUser.UserAccount.GetType().Name}',{0.0},'{UserTracker.currentUser.UserAccount.Currency}','{UserTracker.currentUser.Id}')", connection);
                int i = command.ExecuteNonQuery();
                if (i != 0)
                {
                    MessageBox.Show("Account Created Successfully!");
                    connection.Close();
                    FrmHome frmHome = new FrmHome();
                    frmHome.MdiParent = this.MdiParent;
                    frmHome.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Something Went Wrong, Please Contact Administrator");
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            
        }

        private void rbPersonal_CheckedChanged(object sender, EventArgs e)
        {
            lblChoosePersonalAccountType.Visible = !lblChoosePersonalAccountType.Visible;
            cbPersonalAccountType.Visible = !cbPersonalAccountType.Visible;
        }
    }
}
