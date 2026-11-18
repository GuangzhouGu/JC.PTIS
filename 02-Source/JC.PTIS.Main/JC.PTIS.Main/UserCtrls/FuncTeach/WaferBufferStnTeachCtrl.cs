using APS168_W32;
using JC.B17AutoMating.SubForms;
using JC.PTIS.LoadPort.Devices.EFEM;
using JC.PTIS.Main.Devices.Scaner;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.Models.ModelHelper;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Params;
using JC.PTIS.Main.Stations;
using JC.PTIS.Main.SubViews;
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
using static System.Collections.Specialized.BitVector32;


namespace JC.PTIS.Main.UserCtrls.FuncTeach
{
    public partial class WaferBufferStnTeachCtrl : TeachCtrlBase
    {
        public WaferBufferStnTeachCtrl()
        {
            InitializeComponent();
            StationName = StationName.WaferBufferStn;
        }
        WaferBufferStn Station = null;
        CancellationTokenSource Cts = null;
        AxisName[] Axes = null;
        PosName[] Poses = null;
        private void InitCtrl()
        {
            cmxFoupLayerList.Items.Clear();
            for (int i = 0; i < ParaHelper.Prod.FoupSlotCount; i++)
            {
                cmxFoupLayerList.Items.Add($"第{i + 1}层");
            }
            cmxFoupLayerList.SelectedIndex = 0;
        }

        private void MeasureHeightTeachCtrl_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode()==false)
            {
                InitCtrl();
                Station = StationManager.Instance.GetChild<WaferBufferStn>(nameof(WaferBufferStn));
                var model = FoupModelHelper.GetFoup(0);
                if (model != null)
                {
                    foupCtrl1.UpdateFoupInfo(model);
                }
            }
        }

        private async void btnPickWaferFromFoup_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null,"是否执行从Foup中取Wafer流程")==false)
            {
                return;
            }
            if (IsCanMove(null) == false)
            {
                return;
            }
            btnPickWaferFromFoup.Enabled = false;
            Cts = new CancellationTokenSource();
            Station.IsManualMode = true;
            Station.WaferBuffer_ZPos = GetFoupLayerPos();
            Station.UseFoupNo= rbtnLeft.Checked ? Foup.Left : Foup.Right;
            Station.UseLayerNo = cmxFoupLayerList.SelectedIndex;
            var ret =await TaskTool.RunStateMachine(Station.PickWaferFromFoupFlow, 1, Cts.Token, 100);
            if (ret == 0)
            {
                FrmTips.ShowTipsSuccess(null, $"从Foup中取Wafer完成");
            }
            else if (ret == -1)
            {
                FrmTips.ShowTipsWarning(null, $"从Foup中取Wafer失败");
            }
            Station.IsManualMode = false;
            btnPickWaferFromFoup.Enabled = true;
        }

        private async void btnWaitPos_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否执行移动到待机位流程") == false)
            {
                return;
            }
            if (IsCanMove(null) == false)
            {
                return;
            }
            await Motion.MoveAbsSync(AxisName.WaferBuffer_X, PosName.WaitPos);
            var axes = new List<AxisName>() { AxisName.WaferBuffer_Y, AxisName.WaferBuffer_Z };
            var poses = new List<PosName>() { PosName.WaitPos , PosName.WaitPos };
            await Motion.MoveAbsMSync(axes.ToArray(), poses.ToArray(), 30);
        }

        private void btnAutoPickStop_Click(object sender, EventArgs e)
        {
            Cts.Cancel();
            StationManager.Instance.Stop();
            EnableBtns(true);
            Station.IsManualMode = false;
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            TaskTool.Pause();
        }

        private void btnResume_Click(object sender, EventArgs e)
        {
            TaskTool.Resume();
        }
        private void EnableBtns(bool isEnable)
        {
            btnPickWaferFromFoup.Enabled = isEnable;
            btnWaitPos.Enabled = isEnable;
            btnAutoPickStop.Enabled = isEnable;
        }

        private async void btnPlaceWafer_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否执行放Wafer到Foup流程") == false)
            {
                return;
            }
            if (IsCanMove(null) == false)
            {
                return;
            }
            btnPlaceWafer.Enabled = false;
            Cts = new CancellationTokenSource();
            Station.IsManualMode = true;
            Station.UseFoupNo = rbtnLeft.Checked ? Foup.Left : Foup.Right;
            Station.UseLayerNo = cmxFoupLayerList.SelectedIndex;
            Station.WaferBuffer_ZPos = GetFoupLayerPos();
            var ret = await TaskTool.RunStateMachine(Station.PutWaferToFoupFlow, 1, Cts.Token, 100);
            if (ret == 0)
            {
                FrmTips.ShowTipsSuccess(null, $"放Wafer到Foup完成");
            }
            else if (ret == -1)
            {
                FrmTips.ShowTipsWarning(null, $"放Wafer到Foup失败");
            }
            Station.IsManualMode = false;
            btnPlaceWafer.Enabled = true;
        }

        private async void btnMoveToDefineLayer_Click(object sender, EventArgs e)
        {
            var layerIndex = cmxFoupLayerList.SelectedIndex;
            var foupNo = rbtnLeft.Checked?0:1;
            if (UIMessageDialog.ShowAskDialog(null, $"是否移动模组到第{layerIndex + 1}层") == false)
            {
                return;
            }
            if (IsCanMove(null) == false)
            {
                return;
            }
            await Motion.MoveAbsSync(AxisName.WaferBuffer_X, PosName.WaitPos);
            var axes= new List<AxisName>() { AxisName.WaferBuffer_Y };
            var poses= new List<string>() { $"Foup{foupNo + 1}PickPlacePos"};
            await  Motion.MoveAbsMSync(axes.ToArray(), poses.ToArray(),30);
            var zPos = GetFoupLayerPos();
            var ret= await Motion.MoveAbsSync(AxisName.WaferBuffer_Z, zPos);
            if (ret)
            {
                FrmTips.ShowTipsSuccess(null,$"模组移动到第{layerIndex+1}层完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, $"模组移动到第{layerIndex + 1}层失败");
            }
        }
        private double GetFoupLayerPos()
        {
            var layerIndex = cmxFoupLayerList.SelectedIndex;
            var foupNo = rbtnLeft.Checked ? 0 : 1;
            var pos = 0.0;
            var posName = $"Foup{foupNo + 1}PickPlacePos";
            var posInfo = AxisInfoHelper.GetPosInfoByName(AxisName.WaferBuffer_Z, posName);
            pos = posInfo.Pos + ParaHelper.Prod.FoupPitch * layerIndex;
            return pos;
        }

        private async void btnScanCode_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, $"是否移动{AxisName.WaferBuffer_X}轴到扫码位置") == false)
            {
                return;
            }
            if (IsCanMove(null) == false)
            {
                return;
            }
            var ret=await Motion.MoveAbsSync(AxisName.WaferBuffer_X, PosName.ScancodePos);
            if (ret)
            {
                FrmTips.ShowTipsSuccess(null, $"{AxisName.WaferBuffer_X}轴移动到扫码位置完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, $"{AxisName.WaferBuffer_X}轴移动到扫码位置失败");
            }
        }

        private async void btnPPWaferFromFoup_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, $"是否移动{AxisName.WaferBuffer_X}轴到取放Wafer位置") == false)
            {
                return;
            }
            if (IsCanMove(null) == false)
            {
                return;
            }
            var ret = await Motion.MoveAbsSync(AxisName.WaferBuffer_X, PosName.ClampWaferFromFoup);
            if (ret)
            {
                FrmTips.ShowTipsSuccess(null, $"{AxisName.WaferBuffer_X}轴移动到取放Wafer位置完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, $"{AxisName.WaferBuffer_X}轴移动到取放Wafer位置失败");
            }
        }

        private void btnSubBarcode_Click(object sender, EventArgs e)
        {
            var code=ScanerHelper.Instance.Scan();
            if (string.IsNullOrEmpty(code))
            {
                FrmTips.ShowTipsWarning(null, $"扫码失败");
            }
            else
            {
                tbxBarcode.Text=code;
            }
        }

        private void htButton2_Click(object sender, EventArgs e)
        {
            var foupNo = rbtnLeft.Checked ? 0 : 1;
            var ret= RfidDeviceHelper.Instance.GetRfidDevice(foupNo).ReadCarrierId(out string rfid);
            if (string.IsNullOrEmpty(rfid))
            {
                FrmTips.ShowTipsWarning(null, $"Rfid读取失败");
            }
            else
            {
                tbxRFID.Text = rfid;
            }
        }

        private async void btnWaferLulPos_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, $"是否移动到Wafer平台上料位置") == false)
            {
                return;
            }
            if (IsCanMove(null) == false)
            {
                return;
            }
            var  axes = new List<AxisName>() { AxisName.WaferBuffer_Y, AxisName.WaferBuffer_Z };
            var poses = new List<PosName>() { PosName.WaferFromBufferToTablePos };
            var ret=await Motion.MoveAbsMSync(axes.ToArray(),poses.ToArray());
            if (ret)
            {
                FrmTips.ShowTipsSuccess(null, $"移动到Wafer平台上料位置完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, $"移动到Wafer平台上料位置失败");
            }
        }

        private async void btnWaferTableUnloadPos_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, $"是否移动到Wafer平台下料位置") == false)
            {
                return;
            }
            if (IsCanMove(null) == false)
            {
                return;
            }
            var axes = new List<AxisName>() { AxisName.WaferBuffer_Y, AxisName.WaferBuffer_Z };
            var poses = new List<PosName>() { PosName.WaferFromTableToBufferPos };
            var ret = await Motion.MoveAbsMSync(axes.ToArray(), poses.ToArray());
            if (ret)
            {
                FrmTips.ShowTipsSuccess(null, $"移动到Wafer平台下料位置完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, $"移动到Wafer平台下料位置失败");
            }
        }

        private void rbtnLeft_ValueChanged(object sender, bool value)
        {
            var model = FoupModelHelper.GetFoup(rbtnLeft.Checked?0:1);
            if (model != null)
            {
                foupCtrl1.UpdateFoupInfo(model);
            }
        }
    }
}
