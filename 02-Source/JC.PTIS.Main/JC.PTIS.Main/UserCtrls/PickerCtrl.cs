using JC.PTIS.Main.Entities;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.RotateInfo;
using JC.PTIS.Main.SubViews;
using StdLib.PCBaseFrame.Motion;
using StdLib.WindowsControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.PTIS.Main.UserCtrls
{
    public partial class PickerCtrl : UserControl
    {
        public PickerCtrl()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
        }
        private float _fontSize = 9;
        /// <summary>
        /// 不跟随转塔旋转改变
        /// </summary>
        [Description("字体大小")]
        public float FontSize
        {
            get { return _fontSize; }
            set
            {
                _fontSize = value;
                label1.Font = label2.Font = lblBinLevel.Font = lblPickNo.Font = new Font("微软雅黑", _fontSize);
            }
        }
        public RotateStnType RotateStnType { get; set; }
        PickerInfo PickerInfo = null;
        public void UpdatePickInfo(PickerInfo model)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() => UpdatePickInfo(model)));
            }
            else
            {
                PickerInfo = model;
                if (model.PickNo!=-1)
                {
                    lblPickNo.Text = model.PickNo.ToString();
                }
                if (model.DieModel==null)
                {
                    lblBinLevel.Text = "";
                    lblBinLevel.BackColor = Color.White;
                }
                else
                {
                    if (model.DieModel.BinLevel != BinLevel.None)
                    {
                        lblBinLevel.Text = model.DieModel.BinLevel.ToString().Substring(3);
                    }
                    else
                    {
                        lblBinLevel.Text = "";
                    }
                    if (model.DieModel.DieStatus==DieStatus.Picked)
                    {
                        lblBinLevel.BackColor = Global.GetBinColor(model.DieModel.BinLevel);
                    }
                    else
                    {
                        lblBinLevel.BackColor = Global.GetColor(model.DieModel.DieStatus);
                    }
                }
            }
        }

        private void 详细信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PickerInfo!=null&&PickerInfo.DieModel!=null)
            {
                FrmModelDetail frmModelDetail = new FrmModelDetail();
                frmModelDetail.ShowInfo(PickerInfo.DieModel);
                frmModelDetail.ShowDialog();
            }
        }

        private void 清除信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (PickerInfo != null)
            {
                var stnNo = RotateInfoHelper.Instance.GetStnNo(PickerInfo.PickNo, RotateStnType);
                ProcInfoHelper.Instance.PickDieFromPicker(stnNo, RotateStnType);
                if (Global.RunMode == RunMode.空跑模式)
                {
                    if (PickerInfo != null)
                    {
                        var ioInfo = IOInfoHelper.GetAnalogInputInfoByName($"{RotateStnType}Nozzle{PickerInfo.PickNo + 1}Value");
                        if (ioInfo != null)
                        {
                            ioInfo.AIValue = 0;
                        }
                    }
                }
            }
        }
    }
}
