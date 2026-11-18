using JC.B17AutoMating.SubForms;
using JC.PTIS.Main.Devices.Vision;
using JC.PTIS.Main.Devices.VisionPC.MsgModel;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Stations;
using StdLib.PCBaseFrame.Station;
using StdLib.WindowsControls;
using StdLib.WindowsControls.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.PTIS.Main.UserCtrls.FuncTeach
{
    public partial class IsoPaperStnTeachCtrl : TeachCtrlBase
    {
        public IsoPaperStnTeachCtrl()
        {
            InitializeComponent();
            StationName = MotionInfo.StationName.IsolationPaperStn;
        }
        List<AxisName> Axes = null;
        List<PosName>PosNames = null;
        CancellationTokenSource Cts = null;
        Xyt PaperXyt = null;
        Xyt LotXyt = null;

        private void btnWaitPos_Click(object sender, EventArgs e)
        {
            ModuleMove(PosName.WaitPos);
        }

        private void btnPickPaperFromStagePos_Click(object sender, EventArgs e)
        {
            ModuleMove(PosName.PickPaperPos);
        }

        private void btnPutPaperToTrayPos_Click(object sender, EventArgs e)
        {
            ModuleMove(PosName.PutPaperToTrayPos);
        }


        private async void ModuleMove(PosName posName)
        {
            Axes = new List<AxisName>() { AxisName.IsolationPaper_ArmX, AxisName.IsolationPaper_ArmY, AxisName.IsolationPaper_ArmZ };
            if (IsCanMove(Axes) == false)
            {
                return;
            }
            PosNames = new List<PosName>() { PosName.WaitPos };
            Axes = new List<AxisName>() { AxisName.IsolationPaper_ArmZ };
            var ret = await Motion.MoveAbsMSync(Axes.ToArray(), PosNames.ToArray());
            if (ret)
            {
                PosNames = new List<PosName>() { posName };
                Axes = new List<AxisName>() { AxisName.IsolationPaper_ArmX, AxisName.IsolationPaper_ArmY };
                ret = await Motion.MoveAbsMSync(Axes.ToArray(), PosNames.ToArray());
                if (ret)
                {
                    Axes = new List<AxisName>() { AxisName.IsolationPaper_ArmZ };
                    ret = await Motion.MoveAbsMSync(Axes.ToArray(), PosNames.ToArray());
                    if (ret)
                    {
                        FrmTips.ShowTipsSuccess(null, $"Arm手臂轴移动到{posName}位置完成");
                    }
                    else
                    {
                        FrmTips.ShowTipsWarning(null, $"Arm手臂轴移动到{posName}位置失败");
                    }
                }
                else
                {
                    FrmTips.ShowTipsWarning(null, $"Arm手臂轴移动到{posName}位置失败");
                }
            }
            else
            {
                FrmTips.ShowTipsWarning(null, $"Arm手臂Z轴移动到待机位失败");
            }
        }

        private async void btnPickPaperFlow_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否执行从料仓取纸流程") == false)
            {
                return;
            }
            Axes = new List<AxisName>() { AxisName.IsolationPaper_ArmZ, AxisName.IsolationPaper_ArmX, AxisName.IsolationPaper_ArmY };
            if (IsCanMove(Axes) == false)
            {
                return;
            }
            btnPickPaperFlow.Enabled = false;
            Cts = new CancellationTokenSource();
            var station = StationManager.Instance.GetChild<IsolationPaperStn>(nameof(IsolationPaperStn));
            var ret = await TaskTool.RunStateMachine(station.PickPaperFromCabinetFlow, 1, Cts.Token, 100);
            if (ret == 0)
            {
                FrmTips.ShowTipsSuccess(null, $"从料仓取纸流程完成");
            }
            else if (ret == -1)
            {
                FrmTips.ShowTipsWarning(null, $"从料仓取纸流程失败");
            }
            btnPickPaperFlow.Enabled = true;
        }

        private void btnAutoPickStop_Click(object sender, EventArgs e)
        {
            Cts.Cancel();
            StationManager.Instance.Stop();
            EnableBtns(true);
        }
        private void EnableBtns(bool enable)
        {
            btnPickPaperFlow.Enabled = enable;
            btnAlignTray.Enabled = enable;
            btnAlignPaper.Enabled = enable;
            btnPutPaperToLotTray.Enabled= enable;
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            TaskTool.Pause();
        }

        private void btnResume_Click(object sender, EventArgs e)
        {
            TaskTool.Resume();
        }

        private async void btnAlignPaper_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否执行隔离纸对位流程") == false)
            {
                return;
            }
            Axes = new List<AxisName>() { AxisName.IsolationPaper_ArmZ, AxisName.IsolationPaper_ArmX, AxisName.IsolationPaper_ArmY };
            if (IsCanMove(Axes) == false)
            {
                return;
            }
            btnPickPaperFlow.Enabled = false;
            Cts = new CancellationTokenSource();
            var station = StationManager.Instance.GetChild<IsolationPaperStn>(nameof(IsolationPaperStn));
            var ret = await TaskTool.RunStateMachine(station.PaperAlignFlow, 1, Cts.Token, 100);
            if (ret == 0)
            {
                FrmTips.ShowTipsSuccess(null, $"隔离纸对位流程完成");
            }
            else if (ret == -1)
            {
                FrmTips.ShowTipsWarning(null, $"隔离纸对位流程失败");
            }
            btnPickPaperFlow.Enabled = true;
        }

        private async void btnAlignTray_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否执行Lot对位流程") == false)
            {
                return;
            }
            Axes = new List<AxisName>() { AxisName.IsolationPaper_ArmZ, AxisName.IsolationPaper_ArmX, AxisName.IsolationPaper_ArmY };
            if (IsCanMove(Axes) == false)
            {
                return;
            }
            btnAlignTray.Enabled = false;
            Cts = new CancellationTokenSource();
            var station = StationManager.Instance.GetChild<IsolationPaperStn>(nameof(IsolationPaperStn));
            var ret = await TaskTool.RunStateMachine(station.LotAlignFlow, 1, Cts.Token, 100);
            if (ret == 0)
            {
                FrmTips.ShowTipsSuccess(null, $"Lot对位流程完成");
            }
            else if (ret == -1)
            {
                FrmTips.ShowTipsWarning(null, $"Lot对位流程失败");
            }
            btnAlignTray.Enabled = true;
        }

        private async void btnPutPaperToLotTray_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否执行放隔离纸到Lot流程") == false)
            {
                return;
            }
            Axes = new List<AxisName>() { AxisName.IsolationPaper_ArmZ, AxisName.IsolationPaper_ArmX, AxisName.IsolationPaper_ArmY };
            if (IsCanMove(Axes) == false)
            {
                return;
            }
            btnPutPaperToLotTray.Enabled = false;
            Cts = new CancellationTokenSource();
            var station = StationManager.Instance.GetChild<IsolationPaperStn>(nameof(IsolationPaperStn));
            var ret = await TaskTool.RunStateMachine(station.PutPaperToLotFlow, 1, Cts.Token, 100);
            if (ret == 0)
            {
                FrmTips.ShowTipsSuccess(null, $"放隔离纸到Lot流程完成");
            }
            else if (ret == -1)
            {
                FrmTips.ShowTipsWarning(null, $"放隔离纸到Lot流程失败");
            }
            btnPutPaperToLotTray.Enabled = true;
        }

        private void btnTrayGrabPos_Click(object sender, EventArgs e)
        {
            ModuleMove(PosName.LotTrayGrabPos);
        }

        private void btnPaperGrabPos_Click(object sender, EventArgs e)
        {
            ModuleMove(PosName.PaperGrabPos);
        }

        private void btnTrayGrab_Click(object sender, EventArgs e)
        {
            var ret = AlignHelper.Instance.Send<AlignRspBody>(VisionCmd.Align, AlignStationEnum.Lot, 0);
            if (ret.Code == 0 && ret.Offset != null)
            {
                LotXyt = ret.Offset;
                tbxTrayXyt.Text = ret.Offset.ToString();
                FrmTips.ShowTipsSuccess(null, $"Tray盘拍照完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, $"Tray盘拍照失败");
            }
        }

        private void btnPaperGrab_Click(object sender, EventArgs e)
        {
            var ret = AlignHelper.Instance.Send<AlignRspBody>(VisionCmd.Align, AlignStationEnum.IsoPaper, 0);
            if (ret.Code == 0 && ret.Offset != null)
            {
                PaperXyt = ret.Offset;
                tbxPaperXyt.Text = ret.Offset.ToString();
                FrmTips.ShowTipsSuccess(null, $"隔离纸拍照完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, $"隔离纸拍照失败");
            }
        }

        private async void btnTrayAlign_Click(object sender, EventArgs e)
        {
            if (LotXyt != null)
            {
                var axes = new List<AxisName>() { AxisName.IsolationPaper_ArmT, AxisName.IsolationPaper_ArmX, AxisName.IsolationPaper_ArmY };
                var poses = LotXyt.ToList();
                var ret = await Motion.MoveRelMSync(axes.ToArray(), poses.ToArray());
                if (ret)
                {
                    FrmTips.ShowTipsSuccess(null, $"Lot补正完成");
                }
                else
                {
                    FrmTips.ShowTipsWarning(null, $"Lot补正失败");
                }
                LotXyt = null;
            }
        }

        private async void btnPaperAlign_Click(object sender, EventArgs e)
        {
            if (PaperXyt != null)
            {
                var axes = new List<AxisName>() { AxisName.IsolationPaper_ArmT, AxisName.IsolationPaper_ArmX, AxisName.IsolationPaper_ArmY };
                var poses = PaperXyt.ToList();
                var ret = await Motion.MoveRelMSync(axes.ToArray(), poses.ToArray());
                if (ret)
                {
                    FrmTips.ShowTipsSuccess(null, $"隔离纸补正完成");
                }
                else
                {
                    FrmTips.ShowTipsWarning(null, $"隔离纸补正失败");
                }
                PaperXyt = null;
            }
        }
    }
}
