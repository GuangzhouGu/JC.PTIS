using JC.PTIS.Main.Entities;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.RotateInfo;
using StdLib.PCBaseFrame.Motion;
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
    public partial class PickPatchStnMonitorCtrl : UserControl
    {
        MotionManager Motion => MotionManager.GetInstance();
        public PickPatchStnMonitorCtrl()
        {
            InitializeComponent();
            for (int i = 0; i <16; i++)
            {
                var name = $"pickerCtrl{i + 1}";
                var ctrl = this.Controls.Find(name, true).FirstOrDefault() as PickerCtrl;
                PickerMonitorCtrls[i] = ctrl;
            }
        }
        [Description("标题栏背景色")]
        public Color TitleBackColor { get { return htTitlePanel1.TitleColor; } set { htTitlePanel1.TitleColor = value; } }
        PickerCtrl[] PickerMonitorCtrls = new PickerCtrl[16];

        private void PickPatchCtrl_Load(object sender, EventArgs e)
        {
            ProcInfoHelper.Instance.PickerInfoUpdated += OnPickPatchUpdated;
            RotateInfoHelper.Instance.RotateInfoChanged += OnRotateInfoChanged;
        }
        private void OnRotateInfoChanged(RotateStnType type, int[] stnNoList)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<RotateStnType, int[]>(OnRotateInfoChanged), type, stnNoList);
            }
            else
            {
                if (type == RotateStnType.PickPatch)
                {
                    foreach (var stnNo in stnNoList)
                    {
                        var model = ProcInfoHelper.Instance.GetPickerInfo((PickPatchStnEnum)stnNo, type);
                        if (model != null && PickerMonitorCtrls[stnNo] != null)
                        {
                            PickerMonitorCtrls[stnNo].UpdatePickInfo(model);
                        }
                    }
                    lblRealPos.Text = Motion.GetAxisActPos(AxisName.PickPatch_DD).ToString("F3");
                }
            }
        }
        private void OnPickPatchUpdated(int arg1, RotateInfo.PickerInfo arg2, RotateStnType type)
        {
            if (type==RotateStnType.PickPatch)
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action<int, RotateInfo.PickerInfo, RotateStnType>(OnPickPatchUpdated), arg1, arg2, type);
                }
                else
                {
                    if (arg1 >= 0 && arg1 < PickerMonitorCtrls.Length)
                    {
                        var ctrl = PickerMonitorCtrls[arg1];
                        if (ctrl != null)
                        {
                            ctrl.UpdatePickInfo(arg2);
                        }
                    }
                }
            }
           
        }
      
    }
}
