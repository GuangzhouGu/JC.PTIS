using JC.PTIS.Main.Models;
using StdLib.PCBaseFrame.Extension.Views;
using StdLib.WindowsControls;
using StdLib.WindowsForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.PTIS.Main.SubViews
{
    public partial class FrmAxisTeach : FormPopBase
    {
        public FrmAxisTeach()
        {
            InitializeComponent();
            this.Title="单轴调试";
            this.TopLevel = true;
            this.Location = new Point((Screen.PrimaryScreen.Bounds.Width - this.Width) / 2, (Screen.PrimaryScreen.Bounds.Height - this.Height) / 2);
        }
        public static FrmAxisTeach Instance = new FrmAxisTeach();
        private void FrmAxisTeach_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
