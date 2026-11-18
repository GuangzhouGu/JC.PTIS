using JC.B17AutoMating.SubForms;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Stations;
using StdLib.HTCard;
using StdLib.PCBaseFrame.Extension.Entities;
using StdLib.PCBaseFrame.Motion;
using StdLib.PCBaseFrame.Station;
using StdLib.WindowsControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.PTIS.Main.UserCtrls.FuncTeach
{
    public partial class MeasureHeightTeachCtrl : TeachCtrlBase
    {
        public MeasureHeightTeachCtrl()
        {
            InitializeComponent();
            StationName = StationName.MeasureHeightStn;
        }
        MeasureHeightStn MeasStn = null;
        CancellationTokenSource Cts = null;
        private async void btnExeMesHeight_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否执行测高流程"))
            {
                Cts = new CancellationTokenSource();
                var ret = await TaskTool.RunStateMachine(MeasStn.MeasureHeightFlow, 1, Cts.Token, 100);
                if (ret == 0)
                {
                    FrmTips.ShowTipsSuccess(null, "测高流程完成");
                }
                else if (ret == -1)
                {
                    FrmTips.ShowTipsWarning(null, "测高流程失败");
                }
            }
        }

        private void MeasureHeightTeachCtrl_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode()==false)
            {
                MeasStn = StationManager.Instance.GetChild<MeasureHeightStn>(nameof(MeasureHeightStn));
            }
        }
 
    }
}
