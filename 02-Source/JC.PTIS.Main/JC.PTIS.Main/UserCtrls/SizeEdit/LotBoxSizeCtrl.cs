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
    public partial class LotBoxSizeCtrl : SizeEditBaseCtrl
    {
        public LotBoxSizeCtrl()
        {
            InitializeComponent();
        }
        public override void LoadPara()
        {
            try
            {
                tbxRow.Text = ParaHelper.Prod.RowCountPerLotBox.ToString();
                tbxCol.Text = ParaHelper.Prod.ColCountPerLotBox.ToString();
                tbxWidth.Text = ParaHelper.Prod.LotBoxWidth.ToString();
                tbxHeight.Text = ParaHelper.Prod.LotBoxHeight.ToString();
                tbxPitchX.Text = ParaHelper.Prod.LotCellPitchX.ToString();
                tbxPitchY.Text = ParaHelper.Prod.LotCellPitchY.ToString();
                tbxEdgeX.Text = ParaHelper.Prod.FirstLotCellOffsetX.ToString();
                tbxEdgeY.Text = ParaHelper.Prod.FirstLotCellOffsetY.ToString();
                tbxTrayCountPerCell.Text = ParaHelper.Prod.TrayCountPerEmptyLot.ToString();
                tbxPickTrayCountOnce.Text = ParaHelper.Prod.PickTrayCountFromLotOnce.ToString();
                tbxCellWidth.Text = ParaHelper.Prod.LotBoxWidth.ToString();
                tbxCellHeight.Text = ParaHelper.Prod.LotBoxHeight.ToString();
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
                ParaHelper.Prod.RowCountPerLotBox = int.Parse(tbxRow.Text);
                ParaHelper.Prod.ColCountPerLotBox = int.Parse(tbxCol.Text);
                ParaHelper.Prod.LotBoxWidth = double.Parse(tbxWidth.Text);
                ParaHelper.Prod.LotBoxHeight = double.Parse(tbxHeight.Text);
                ParaHelper.Prod.FirstLotCellOffsetX = double.Parse(tbxEdgeX.Text);
                ParaHelper.Prod.FirstLotCellOffsetY = double.Parse(tbxEdgeY.Text);
                ParaHelper.Prod.LotCellPitchX = double.Parse(tbxPitchX.Text);
                ParaHelper.Prod.LotCellPitchY = double.Parse(tbxPitchY.Text);
                ParaHelper.Prod.TrayCountPerEmptyLot = int.Parse(tbxTrayCountPerCell.Text);
                ParaHelper.Prod.PickTrayCountFromLotOnce = int.Parse(tbxPickTrayCountOnce.Text);
                ParaHelper.Prod.LotCellPitchX = double.Parse(tbxCellWidth.Text);
                ParaHelper.Prod.LotCellPitchY = double.Parse(tbxCellHeight.Text);
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
