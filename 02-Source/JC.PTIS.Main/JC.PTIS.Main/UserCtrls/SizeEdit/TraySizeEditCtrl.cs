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
    public partial class TraySizeEditCtrl : SizeEditBaseCtrl
    {
        public TraySizeEditCtrl()
        {
            InitializeComponent();
        }
        public override void LoadPara()
        {
            try
            {
                tbxRow.Text = ParaHelper.Prod.TrayRowCount.ToString();
                tbxCol.Text = ParaHelper.Prod.TrayColCount.ToString();
                tbxBoatWidth.Text = ParaHelper.Prod.TrayWidth.ToString();
                tbxBoatHeight.Text = ParaHelper.Prod.TrayHeight.ToString();
                tbxThickness.Text = ParaHelper.Prod.TrayThickness.ToString();
                tbxEdgeX.Text = ParaHelper.Prod.FirstCellOffsetX.ToString();
                tbxEdgeY.Text = ParaHelper.Prod.FirstCellOffsetY.ToString();
                tbxPitchX.Text = ParaHelper.Prod.CellPitchX.ToString();
                tbxPitchY.Text = ParaHelper.Prod.CellPitchY.ToString();
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
                ParaHelper.Prod.TrayRowCount = int.Parse(tbxRow.Text);
                ParaHelper.Prod.TrayColCount = int.Parse(tbxCol.Text);
                ParaHelper.Prod.TrayWidth = float.Parse(tbxBoatWidth.Text);
                ParaHelper.Prod.TrayHeight = float.Parse(tbxBoatHeight.Text);
                ParaHelper.Prod.TrayThickness = float.Parse(tbxThickness.Text);
                ParaHelper.Prod.FirstCellOffsetX = float.Parse(tbxEdgeX.Text);
                ParaHelper.Prod.FirstCellOffsetY = float.Parse(tbxEdgeY.Text);
                ParaHelper.Prod.CellPitchX = float.Parse(tbxPitchX.Text);
                ParaHelper.Prod.CellPitchY = float.Parse(tbxPitchY.Text);
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
