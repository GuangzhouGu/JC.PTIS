using JC.B17AutoMating.SubForms;
using JC.PTIS.Main.Devices.Vision;
using JC.PTIS.Main.Devices.VisionPC.MsgModel;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Params;
using JC.PTIS.Main.RotateInfo;
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
    public partial class PackingTeachCtrl : TeachCtrlBase
    {
        public PackingTeachCtrl()
        {
            InitializeComponent();
            StationName = StationName.PackingStn;
        }
        PackingStn PackingStn = null;
        CancellationTokenSource Cts = null;
        List<AxisName> Axes = null;
        List<PosName> PosNames = null;
        Xyt LabelXyt = null;

        private void MeasureHeightTeachCtrl_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode()==false)
            {
                InitCtrl();
                PackingStn = StationManager.Instance.GetChild<PackingStn>(nameof(PackingStn));
            }
        }
        private void InitCtrl()
        {
            //初始化cmxLotBoxRowList和cmxLotBoxColList控件
            cmxLotBoxRowList.Items.Clear();
            cmxLotBoxColList.Items.Clear();
            for (int i = 0; i < ParaHelper.Prod.RowCountPerLotBox; i++)
            {
                cmxLotBoxRowList.Items.Add($"第{i}行");
            }
            for (int i = 0; i < ParaHelper.Prod.ColCountPerLotBox; i++)
            {
                cmxLotBoxColList.Items.Add($"第{i}列");
            }
            cmxInnerBoxStageList.SelectedIndex = 0;
            cmxLotBoxRowList.SelectedIndex = 0;
            cmxLotBoxColList.SelectedIndex = 0;
        }
        private void btnWaitPos_Click(object sender, EventArgs e)
        {
            ModuleMove(PosName.WaitPos);
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            TaskTool.Pause();
        }

        private void btnResume_Click(object sender, EventArgs e)
        {
            TaskTool.Resume();
        }

        private void btnAutoPickStop_Click(object sender, EventArgs e)
        {
            Cts.Cancel();
            StationManager.Instance.Stop();
            EnableBtns(true);
        }
        private void EnableBtns(bool enable)
        {
        }

        private  void btnLabelGrabPos_Click(object sender, EventArgs e)
        {
            ModuleMove(PosName.GrabLabelPos);
        }


        private async void ModuleMove(PosName posName)
        {
            Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY, AxisName.Packing_ArmZ, AxisName.Packing_ArmT };
            if (IsCanMove(Axes) == false)
            {
                return;
            }
            PosNames = new List<PosName>() { PosName.WaitPos };
            var cyls = new List<CylinderName>() { CylinderName.LotTransferArmLiftCyl};
            var ret = await Motion.CylinderMAsync(cyls, new List<bool>() { false });
            if (ret)
            {
                Axes = new List<AxisName>() { AxisName.Packing_ArmZ };
                ret = await Motion.MoveAbsMSync(Axes.ToArray(), PosNames.ToArray());
                if (ret)
                {
                    Axes = new List<AxisName>() { AxisName.Packing_ArmY };
                    ret = await Motion.MoveAbsMSync(Axes.ToArray(), PosNames.ToArray());

                    PosNames = new List<PosName>() { posName };
                    Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY };
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
                    FrmTips.ShowTipsWarning(null, $"Arm手臂Z轴移动到待机位失败");
                }
            }
            else
            {
                FrmTips.ShowTipsWarning(null, $"Arm手臂气缸抬起失败");
            }
        }
        private async void ModuleMove(Xyt pos)
        {
            Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY, AxisName.Packing_ArmZ };
            if (IsCanMove(Axes) == false)
            {
                return;
            }
            PosNames = new List<PosName>() { PosName.WaitPos };
            var cyls = new List<CylinderName>() { CylinderName.LotTransferArmLiftCyl };
            var ret = await Motion.CylinderMAsync(cyls, new List<bool>() { false });
            if (ret)
            {
                Axes = new List<AxisName>() { AxisName.Packing_ArmZ };
                ret = await Motion.MoveAbsMSync(Axes.ToArray(), PosNames.ToArray());
                if (ret)
                {
                    Axes = new List<AxisName>() { AxisName.Packing_ArmY };
                    ret = await Motion.MoveAbsMSync(Axes.ToArray(), PosNames.ToArray());

                    Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY };
                    ret = await Motion.MoveAbsMSync(Axes.ToArray(), pos.ToArray());
                    if (ret)
                    {
                        FrmTips.ShowTipsSuccess(null, $"Arm手臂轴移动完成");
                    }
                    else
                    {
                        FrmTips.ShowTipsWarning(null, $"Arm手臂轴移动失败");
                    }
                }
                else
                {
                    FrmTips.ShowTipsWarning(null, $"Arm手臂Z轴移动到待机位失败");
                }
            }
            else
            {
                FrmTips.ShowTipsWarning(null, $"Arm手臂气缸抬起失败");
            }
        }
        private void btnPickPaperFlow_Click(object sender, EventArgs e)
        {
            ModuleMove(PosName.PPLotFrom8StnTurnTablePos);
        }

        private void btnAlignPaper_Click(object sender, EventArgs e)
        {
            ModuleMove(PosName.LotClawPPLotFromPackingStagePos);
        }

        private void btnBeltDetectPos_Click(object sender, EventArgs e)
        {
            ModuleMove(PosName.DetectPackingBeltPos);
        }

        private void btnSuckerPPTrayFromPackingStagePos_Click(object sender, EventArgs e)
        {
            ModuleMove(PosName.LotPickerPPLotFromPackingStagePos);
        }

        private void htButton6_Click(object sender, EventArgs e)
        {
            var ret=AlignHelper.Instance.Send<BeltDetectRspBody>(VisionCmd.DetectBelt, 0);
            if (ret!=null&&ret.DetectResult)
            {
                tbxBeltDetectResult.Text = ret.DetectResult.ToString();
                FrmTips.ShowTipsSuccess(null, $"束带检测成功");
            }
            else
            {
                tbxBeltDetectResult.Text ="NG";
                FrmTips.ShowTipsWarning(null, $"束带检测失败");
            }
          
        }

        private void btnLabelGrab_Click(object sender, EventArgs e)
        {
            var ret = AlignHelper.Instance.Send<AlignRspBody>(VisionCmd.Align, AlignStationEnum.IsoPaper, 0);
            if (ret.Code == 0 && ret.Offset != null)
            {
                LabelXyt = ret.Offset;
                tbxBeltDetectResult.Text = ret.Offset.ToString();
                FrmTips.ShowTipsSuccess(null, $"隔离纸拍照完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, $"隔离纸拍照失败");
            }
        }

        private void htButton2_Click(object sender, EventArgs e)
        {
            var ret = AlignHelper.Instance.Send<BeltDetectRspBody>(VisionCmd.DetectLabel, 0);
            if (ret != null && ret.DetectResult)
            {
                tbxLabelDetectResult.Text = ret.DetectResult.ToString();
                FrmTips.ShowTipsSuccess(null, $"标签检测成功");
            }
            else
            {
                tbxLabelDetectResult.Text = "NG";
                FrmTips.ShowTipsWarning(null, $"标签检测失败");
            }
        }

        private async void btnLabelAlign_Click(object sender, EventArgs e)
        {
            if (LabelXyt != null)
            {
                var axes = new List<AxisName>() { AxisName.IsolationPaper_ArmT, AxisName.IsolationPaper_ArmX, AxisName.IsolationPaper_ArmY };
                var poses = LabelXyt.ToList();
                var ret = await Motion.MoveRelMSync(axes.ToArray(), poses.ToArray());
                if (ret)
                {
                    FrmTips.ShowTipsSuccess(null, $"标签补正完成");
                }
                else
                {
                    FrmTips.ShowTipsWarning(null, $"标签补正失败");
                }
                LabelXyt = null;
            }
        }

        private void btnLabelDetectPos_Click(object sender, EventArgs e)
        {
            ModuleMove(PosName.LabelDetectPos);
        }


        private async void btnPickLotFromStn8_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否从8工位转盘取Lot") == false)
            {
                return;
            }
            Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY, AxisName.Packing_ArmZ };
            if (IsCanMove(Axes) == false)
            {
                return;
            }
            btnPickLotFromStn8.Enabled = false;
            Cts = new CancellationTokenSource();
            var station = StationManager.Instance.GetChild<PackingStn>(nameof(PackingStn));
            var ret = await TaskTool.RunStateMachine(station.PickFullLotFromStn8Flow, 1, Cts.Token, 100);
            if (ret == 0)
            {
                FrmTips.ShowTipsSuccess(null, $"从8工位转盘取Lot流程完成");
            }
            else if (ret == -1)
            {
                FrmTips.ShowTipsWarning(null, $"从8工位转盘取Lot流程失败");
            }
            btnPickLotFromStn8.Enabled = true;
        }

        private async void btnPutLotToStn8_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否放Lot到8工位转盘") == false)
            {
                return;
            }
            Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY, AxisName.Packing_ArmZ };
            if (IsCanMove(Axes) == false)
            {
                return;
            }
            btnPutLotToStn8.Enabled = false;
            Cts = new CancellationTokenSource();
            var station = StationManager.Instance.GetChild<PackingStn>(nameof(PackingStn));
            var ret = await TaskTool.RunStateMachine(station.PutEmptyLotToStn8Flow, 1, Cts.Token, 100);
            if (ret == 0)
            {
                FrmTips.ShowTipsSuccess(null, $"放Lot到8工位转盘流程完成");
            }
            else if (ret == -1)
            {
                FrmTips.ShowTipsWarning(null, $"放Lot到8工位转盘流程失败");
            }
            btnPutLotToStn8.Enabled = true;
        }

     
        private async void btnPackingFlow_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否执行束带流程") == false)
            {
                return;
            }
            Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY, AxisName.Packing_ArmZ };
            if (IsCanMove(Axes) == false)
            {
                return;
            }
            btnPackingFlow.Enabled = false;
            Cts = new CancellationTokenSource();
            var station = StationManager.Instance.GetChild<PackingStn>(nameof(PackingStn));
            var ret = await TaskTool.RunStateMachine(station.PackingLotFlow, 1, Cts.Token, 100);
            if (ret == 0)
            {
                FrmTips.ShowTipsSuccess(null, $"束带完成");
            }
            else if (ret == -1)
            {
                FrmTips.ShowTipsWarning(null, $"束带失败");
            }
            btnPackingFlow.Enabled = true;
        }

        private async void btnPasteLabelFlow_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否执行贴标签流程") == false)
            {
                return;
            }
            Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY, AxisName.Packing_ArmZ };
            if (IsCanMove(Axes) == false)
            {
                return;
            }
            btnPasteLabelFlow.Enabled = false;
            Cts = new CancellationTokenSource();
            var station = StationManager.Instance.GetChild<PackingStn>(nameof(PackingStn));
            var ret = await TaskTool.RunStateMachine(station.PasteLabelFlow, 1, Cts.Token, 100);
            if (ret == 0)
            {
                FrmTips.ShowTipsSuccess(null, $"贴标签完成");
            }
            else if (ret == -1)
            {
                FrmTips.ShowTipsWarning(null, $"贴标签失败");
            }
            btnPasteLabelFlow.Enabled = true;
        }

        private async void btnLabelDetectFlow_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否执行标签检测流程") == false)
            {
                return;
            }
            Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY, AxisName.Packing_ArmZ };
            if (IsCanMove(Axes) == false)
            {
                return;
            }
            btnLabelDetectFlow.Enabled = false;
            Cts = new CancellationTokenSource();
            var station = StationManager.Instance.GetChild<PackingStn>(nameof(PackingStn));
            var ret = await TaskTool.RunStateMachine(station.LabelDetectFlow, 1, Cts.Token, 100);
            if (ret == 0)
            {
                FrmTips.ShowTipsSuccess(null, $"标签检测完成");
            }
            else if (ret == -1)
            {
                FrmTips.ShowTipsWarning(null, $"标签检测失败");
            }
            btnLabelDetectFlow.Enabled = true;
        }

        private void btnPickFromLotBox1Stn_Click(object sender, EventArgs e)
        {
            ModuleMove(PosName.PPTrayFromLotBoxStn1StartPos);
        }

        private void btnPickFromLotBox2Stn_Click(object sender, EventArgs e)
        {
            ModuleMove(PosName.PPTrayFromLotBoxStn2StartPos);
        }

        private void btnMoveToDestPos_Click(object sender, EventArgs e)
        {
            var stageNo= cmxInnerBoxStageList.SelectedIndex;
            var rowNo = cmxLotBoxRowList.SelectedIndex;
            var colNo = cmxLotBoxColList.SelectedIndex;
            var pos = PackingStn.GetPPTrayFromLotBoxStnPos(rowNo,colNo,false, stageNo==0?BinLevel.Bin1:BinLevel.Bin2);
            ModuleMove(pos);
        }
    }
}
