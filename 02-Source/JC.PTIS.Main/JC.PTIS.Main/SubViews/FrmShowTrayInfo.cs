using JC.PTIS.Main.Models;
using JC.PTIS.Main.UserCtrls;
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
    public partial class FrmShowTrayInfo : FormPopBase
    {
        public FrmShowTrayInfo()
        {
            InitializeComponent();
        }
        public void ShowInfo(TrayModel trayModel)
        {
            try
            {
                if (trayModel != null)
                {
                    TrayCtrl lotTrayCtrl = new TrayCtrl();
                    lotTrayCtrl.Dock = DockStyle.Fill;
                    lotTrayCtrl.Parent = panel1;
                    lotTrayCtrl.UpdateTrayModel(trayModel);
                }
            }
            catch (Exception ex)
            {
                UIMessageDialog.ShowErrorDialog(null, ex.ToString());
            }
        }
    }
}
