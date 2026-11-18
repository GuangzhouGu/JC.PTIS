using JC.AutoMating.UserCtrl;
using StdLib.PCBaseFrame.Extension.Entities;
using StdLib.PCBaseFrame.Extension.Views;
using StdLib.WindowsForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.AutoMating.SubForms
{
    public partial class FrmInfoConfig : ParaViewBase
    {
        public FrmInfoConfig()
        {
            InitializeComponent();
            IsHaveSaveBtn = true;
        }
        static FrmInfoConfig _instance = null;
        public static FrmInfoConfig Instance
        {
            get
            {
                if (_instance == null || _instance.IsDisposed)
                {
                    _instance = new FrmInfoConfig();
                }
                return _instance;
            }
        }
        private void FrmInfoConfig_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode()==false)
            {
                StepTimeConfigCtrl stepTimeConfigCtrl = new StepTimeConfigCtrl();
                stepTimeConfigCtrl.Dock = DockStyle.Fill;
                stepTimeConfigCtrl.Margin = new Padding(1);
                stepTimeConfigCtrl.Parent = this;
            }
        }
    }
}
