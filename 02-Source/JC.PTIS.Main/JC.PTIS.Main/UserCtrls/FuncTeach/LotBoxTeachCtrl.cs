using JC.B17AutoMating.SubForms;
using JC.PTIS.LoadPort.Devices.EFEM;
using JC.PTIS.Main.Devices.Scaner;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Stations;
using StdLib.HTCard;
using StdLib.PCBaseFrame.Extension.Entities;
using StdLib.PCBaseFrame.Motion;
using StdLib.PCBaseFrame.Station;
using StdLib.WindowsControls;
using StdLib.WindowsControls.Class;
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
using static System.Collections.Specialized.BitVector32;

namespace JC.PTIS.Main.UserCtrls.FuncTeach
{
    public partial class LotBoxTeachCtrl : TeachCtrlBase
    {
        public LotBoxTeachCtrl()
        {
            InitializeComponent();
            StationName = StationName.PackingStn;
        }
        PackingStn PackingStn = null;
        CancellationTokenSource Cts = null;
        List<AxisName> Axes = null;
        List<PosName> PosNames = null;
        private void MeasureHeightTeachCtrl_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode()==false)
            {
                Station=StationManager.Instance.GetChild<LotBoxStn>(nameof(LotBoxStn));
                cmxInnerBoxStageList.SelectedIndex = 0;
                cmxExBoxStageList.SelectedIndex = 0;
                PackingStn = StationManager.Instance.GetChild<PackingStn>(nameof(PackingStn));
            }
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

        }

        private void cmxStageList_SelectedIndexChanged(object sender, EventArgs e)
        {
            cylinderCtrl1.CylinderName = $"LotBox{cmxInnerBoxStageList.SelectedIndex+1}FixLotCyl";
            cylinderCtrl1.CylinderNameChn = $"内盒{cmxInnerBoxStageList.SelectedIndex + 1}固定气缸";
        }


        private async void ModuleMove(PosName posName)
        {
            Axes = new List<AxisName>() { AxisName.LotBox_ArmX, AxisName.LotBox_ArmY, AxisName.LotBox_ArmZ };
            if (IsCanMove(Axes) == false)
            {
                return;
            }
            PosNames = new List<PosName>() { PosName.WaitPos };
            Axes = new List<AxisName>() { AxisName.LotBox_ArmZ };
            var ret = await Motion.MoveAbsMSync(Axes.ToArray(), PosNames.ToArray());
            if (ret)
            {
                PosNames = new List<PosName>() { posName };
                Axes = new List<AxisName>() { AxisName.LotBox_ArmX, AxisName.LotBox_ArmY };
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
        LotBoxStn Station = null;
        private void btnMoveToDestStage_Click(object sender, EventArgs e)
        {
            var stageNo = cmxInnerBoxStageList.SelectedIndex;
            var pos = Station.GetPosName(stageNo, PosType.PPLotBoxFromInBoxPos, false);
            ModuleMove(pos);
        }

        private void htButton4_Click(object sender, EventArgs e)
        {
            var stageNo = cmxInnerBoxStageList.SelectedIndex;
            var pos = Station.GetPosName(stageNo, PosType.ScanPos, false);
            ModuleMove(pos);
        }

        private void htButton5_Click(object sender, EventArgs e)
        {
            var stageNo = cmxExBoxStageList.SelectedIndex;
            var pos = Station.GetPosName(stageNo, PosType.PPLotBoxFromExBoxPos, true);
            ModuleMove(pos);
        }

        private void htButton6_Click(object sender, EventArgs e)
        {
            var stageNo = cmxExBoxStageList.SelectedIndex;
            var pos = Station.GetPosName(stageNo, PosType.PrePPLotBoxFromExBoxPos, true);
            ModuleMove(pos);
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            tbxCode.Text = "";
            var code= ScanerHelper.Instance.Scan(1);
            if (string.IsNullOrEmpty(code)==false)
            {
                tbxCode.Text = code;
            }
            else
            {
                UIMessageDialog.ShowErrorDialog(null, "扫码失败");
            }
        }

        private void btnReadRfid_Click(object sender, EventArgs e)
        {
            var stageNo = cmxExBoxStageList.SelectedIndex;
            tbxRfid.Text = "";
            var ret = RfidDeviceHelper.Instance.GetRfidDevice(stageNo).ReadCarrierId(out string rfid);
            if (string.IsNullOrEmpty(rfid) == false)
            {
                tbxRfid.Text = rfid;
            }
            else
            {
                UIMessageDialog.ShowErrorDialog(null, "读取失败");
            }
        }

        private async void btnEmptyLotMoveToStage2_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否移动空TrayLot到内盒平台2") == false)
            {
                return;
            }
            Axes = new List<AxisName>() { AxisName.LotBox_ArmX, AxisName.LotBox_ArmY, AxisName.LotBox_ArmZ };
            if (IsCanMove(Axes) == false)
            {
                return;
            }
            btnEmptyLotMoveToStage2.Enabled = false;
            Cts = new CancellationTokenSource();
            var ret = await TaskTool.RunStateMachine(Station.MoveLotBoxToStn2Flow,1,LotBoxType.EmptyTray, Cts.Token, 100);
            if (ret == 0)
            {
                FrmTips.ShowTipsSuccess(null, $"移动空TrayLot到内盒平台2完成");
            }
            else if (ret == -1)
            {
                FrmTips.ShowTipsWarning(null, $"移动空TrayLot到内盒平台2失败");
            }
            btnEmptyLotMoveToStage2.Enabled = true;
        }

        private async void btnNgLotMoveToStage2_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否移动NgTrayLot到内盒平台2") == false)
            {
                return;
            }
            Axes = new List<AxisName>() { AxisName.LotBox_ArmX, AxisName.LotBox_ArmY, AxisName.LotBox_ArmZ };
            if (IsCanMove(Axes) == false)
            {
                return;
            }
            btnNgLotMoveToStage2.Enabled = false;
            Cts = new CancellationTokenSource();
            var ret = await TaskTool.RunStateMachine(Station.MoveLotBoxToStn2Flow, 1, LotBoxType.Bin246, Cts.Token, 100);
            if (ret == 0)
            {
                FrmTips.ShowTipsSuccess(null, $"移动NgTrayLot到内盒平台2完成");
            }
            else if (ret == -1)
            {
                FrmTips.ShowTipsWarning(null, $"移动NgTrayLot到内盒平台2失败");
            }
            btnNgLotMoveToStage2.Enabled = true;
        }
    }
}
