using JC.PTIS.Main.Entities;
using JC.PTIS.Main.FlowInfo;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.SubViews;
using StdLib.PCBaseFrame.Extension.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.PTIS.Main.UserCtrls
{
    public partial class LotCellCtrl : UserControl
    {
        public LotCellCtrl()
        {
            InitializeComponent();
        }
        public int LotCount { get; set; } = 5;
        int RowNo = -1;
        int ColNo = -1;
        LotBoxType LotBoxType;
        LotCellBase FullLotCellModel = null;
        public void Init(LotCellBase lotCellModel, LotBoxType lotBoxType)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<LotCellBase, LotBoxType>(Init), lotCellModel, lotBoxType);
            }
            else
            {
                LotBoxType = lotBoxType;
                if (lotCellModel != null)
                {
                    RowNo = lotCellModel.RowNo;
                    ColNo = lotCellModel.ColNo;
                    if (LotBoxType== LotBoxType.EmptyTray)
                    {
                        label4.Text = "Tray";
                        if (lotCellModel.TrayList != null)
                        {
                            lblTrayCount.Text = lotCellModel.TrayList.Count.ToString();
                        }
                    }
                    else if (LotBoxType == LotBoxType.Bin135||LotBoxType == LotBoxType.Bin246)
                    {
                        lblTrayCount.Text = lotCellModel.LotList.Count.ToString();
                    }
                }
            }
        }
        public void UpdateLotCellInfo(LotCellBase lotCellModel)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<LotCellBase>(UpdateLotCellInfo), lotCellModel);
            }
            else
            {
                if (LotBoxType == LotBoxType.Bin135||LotBoxType == LotBoxType.Bin246)
                {
                    FullLotCellModel = lotCellModel;
                    label4.Text = "Lot";
                    lblBin.Text = lotCellModel.BinLevel.ToInt().ToString();
                    if (lotCellModel.LotList != null)
                    {
                        lblTrayCount.Text = lotCellModel.LotList.Count.ToString();
                    }
                }
                else
                {
                    label4.Text = "Tray";
                    if (lotCellModel.TrayList != null)
                    {
                        lblTrayCount.Text = lotCellModel.TrayList.Count.ToString();
                    }
                }
            }
        }
        private void LotCellCtrl_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode()==false)
            {
                //ProcInfoHelper.Instance.LotCellInfoUpdated += OnLotCellInfoUpdated;
            }
        }

        private void 详细信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (FullLotCellModel != null && FullLotCellModel.LotList != null)
            {
                FrmShowLotBox frmModelDetail = new FrmShowLotBox();
                frmModelDetail.ShowInfo(FullLotCellModel.LotList);
                frmModelDetail.ShowDialog();
            }
        }

        private void 清空信息ToolStripMenuItem1_Click(object sender, EventArgs e)
        {

        }
    }
}
