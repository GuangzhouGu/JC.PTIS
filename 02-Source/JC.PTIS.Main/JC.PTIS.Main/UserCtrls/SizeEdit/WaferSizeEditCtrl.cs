using JC.PTIS.Main.Params;
using StdLib.WindowsControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.PTIS.Main.UserCtrls.SizeEdit
{
    public partial class WaferSizeEditCtrl : SizeEditBaseCtrl
    {
        public WaferSizeEditCtrl()
        {
            InitializeComponent();
        }
        public override void LoadPara()
        {
            try
            {
                tbxLayerCount.Text = ParaHelper.Prod.FoupSlotCount.ToString();
                tbxGap.Text = ParaHelper.Prod.FoupPitch.ToString();
            }
            catch (Exception ex)
            {
                UIMessageDialog.ShowErrorDialog(null, $"Boat参数加载异常，异常信息：{ex}");
            }
        }
        public override bool SavePara()
        {
            try
            {
                ParaHelper.Prod.FoupSlotCount = int.Parse(tbxLayerCount.Text);
                ParaHelper.Prod.FoupPitch = double.Parse(tbxGap.Text);
                return true;
            }
            catch (Exception ex)
            {
                UIMessageDialog.ShowErrorDialog(null, $"Boat参数保存异常，异常信息：{ex}");
                return false;
            }
        }
    }
}
