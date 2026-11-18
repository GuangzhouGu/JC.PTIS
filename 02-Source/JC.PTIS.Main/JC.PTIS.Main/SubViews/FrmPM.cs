using StdLib.PCBaseFrame.Extension.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace HYC.SaturnMain.SubForms
{
    public partial class FrmPM : Form
    {
        public FrmPM()
        {
            InitializeComponent();
         
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            FormLogin frm = new FormLogin();
            frm.ShowDialog();
        }

        private void FrmPM_Load(object sender, EventArgs e)
        {
            //this.label1.Text = ParaHelper.Global.PMFormText;
            this.FormBorderStyle = FormBorderStyle.None;
        }
    }
}
