using JC.PTIS.Main.Entities;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.RotateInfo;
using StdLib.PCBaseFrame.Extension.Entities;
using StdLib.PCBaseFrame.Motion;
using System;
using System.Collections;
using System.Collections.Concurrent;
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
    public partial class LoadDieStnMonitorCtrl : UserControl
    {
        MotionManager Motion => MotionManager.GetInstance();
        [Description("标题栏背景色")]
        public Color TitleBackColor { get { return htTitlePanel1.TitleColor; } set { htTitlePanel1.TitleColor = value; } }
        public LoadDieStnMonitorCtrl()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            for (int i = 0; i < circleCtrl1.StnCount; i++)
            {
                var name = $"pickerMonitorCtrl{i + 1}";
                var ctrl = this.Controls.Find(name, true).FirstOrDefault() as PickerCtrl;
                PickerMonitorCtrls[i] = ctrl;
            }
        }

        PickerCtrl[] PickerMonitorCtrls = new PickerCtrl[16];
    

        private void LoadDieStnMonitorCtrl_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode()==false)
            {
                RotateInfoHelper.Instance.RotateInfoChanged += OnRotateInfoChanged;
                ProcInfoHelper.Instance.PickerInfoUpdated += OnPickerInfoUpdated;
            }
        }

        private void OnPickerInfoUpdated(int stnNo,PickerInfo pickerInfo, RotateStnType type)
        {
            try
            {
                if (type == RotateStnType.LoadDie)
                {
                    if (InvokeRequired)
                    {
                        this.Invoke(new Action<int, PickerInfo, RotateStnType>(OnPickerInfoUpdated), stnNo, pickerInfo, type);
                    }
                    else
                    {
                        PickerMonitorCtrls[stnNo].UpdatePickInfo(pickerInfo);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void OnRotateInfoChanged(RotateStnType type, int[] stnNoList)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<RotateStnType, int[]>(OnRotateInfoChanged), type, stnNoList);
            }
            else
            {
                if (type == RotateStnType.LoadDie)
                {
                    foreach (var stnNo in stnNoList)
                    {
                        var model = ProcInfoHelper.Instance.GetPickerInfo((Stn16TurnTableStnEnum)stnNo, type);
                        PickerMonitorCtrls[stnNo].UpdatePickInfo(model);
                    }
                    lblRealPos.Text = Motion.GetAxisActPos(AxisName.LoadDie_DD).ToString("F3");
                }
            }
        }

    
    }
}
