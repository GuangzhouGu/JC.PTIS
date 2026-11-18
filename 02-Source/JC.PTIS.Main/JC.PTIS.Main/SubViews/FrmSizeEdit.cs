using JC.PTIS.Main.Params;
using JC.PTIS.Main.UserCtrls.SizeEdit;
using StdLib.PCBaseFrame.Extension.Recipe;
using StdLib.PCBaseFrame.Extension.Views;
using StdLib.WindowsControls.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.PTIS.Main.SubForms
{
    public partial class FrmSizeEdit : ParaViewBase
    {
        public FrmSizeEdit()
        {
            InitializeComponent();
        }
        public static FrmSizeEdit Instance = new FrmSizeEdit();
        public override bool Save()
        {
            bool ret = false;
            SizeEditBaseCtrl ctrl = null;
            if (htTabControl1.SelectedTab.Controls.Count>0)
            {
                foreach (var item in htTabControl1.SelectedTab.Controls)
                {
                    if (item.GetType().BaseType.Name == nameof(SizeEditBaseCtrl))
                    {
                        ctrl= item as SizeEditBaseCtrl;
                       
                        break;
                    }
                }
                if (ctrl!=null)
                {
                    ret = ctrl.SavePara();
                }
            }
            if (ret)
            {
                return RecipeManager.SavePara(nameof(ProdPara));
            }
            else
            {
                return ret;
            }
        }

        private void htTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            SizeEditBaseCtrl ctrl = null;
            if (htTabControl1.SelectedTab.Controls.Count > 0)
            {
                foreach (var item in htTabControl1.SelectedTab.Controls)
                {
                    if (item.GetType().BaseType.Name == nameof(SizeEditBaseCtrl))
                    {
                        ctrl = item as SizeEditBaseCtrl;
                        break;
                    }
                }
                if (ctrl != null)
                {
                     ctrl.LoadPara();
                }
            }
        }

        private void FrmSizeEdit_Load(object sender, EventArgs e)
        {
        }
    }
}
