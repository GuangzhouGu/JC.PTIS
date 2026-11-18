using APS168_W32;
using JC.B17AutoMating.SubForms;
using JC.PTIS.Main.Devices.Vision;
using JC.PTIS.Main.Devices.VisionPC.MsgModel;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Params;
using JC.PTIS.Main.RotateInfo;
using JC.PTIS.Main.Stations;
using StdLib.HTCard;
using StdLib.PCBaseFrame.Config;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.PCBaseFrame.Extension.Entities;
using StdLib.PCBaseFrame.Extension.Recipe;
using StdLib.PCBaseFrame.Motion;
using StdLib.PCBaseFrame.MotionInfo;
using StdLib.PCBaseFrame.Station;
using StdLib.WindowsControls;
using StdLib.WindowsControls.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace JC.PTIS.Main.UserCtrls.FuncTeach
{
    public partial class Stn4TeachCtrl : TeachCtrlBase
    {
        public Stn4TeachCtrl()
        {
            InitializeComponent();
            StationName =MotionInfo.StationName.Stn4TurnTableStn;
        }
        const double RotateInterval = 90;
        List<AxisName> Axes = null;
        System.Timers.Timer Timer = new System.Timers.Timer();
        Stn4TurnTableStn Station = null;
        Xyt AlignXyt = null;
        Xyt FirstCellPos = new Xyt();
        CancellationTokenSource Cts = null;
        private void InitCtrl()
        {
            try
            {
                cmxStageList.Items.Clear();
                for (int i = 0; i < 4; i++)
                {
                    cmxStageList.Items.Add($"平台{i}");
                }
                for (int i = 0; i < ParaHelper.Prod.TrayRowCount; i++)
                {
                    cmxTrayRowList.Items.Add($"第{i}行");
                }
                for (int i = 0; i < ParaHelper.Prod.TrayColCount; i++)
                {
                    cmxTrayColList.Items.Add($"第{i}列");
                }
                cmxStageList.SelectedIndex = 0;
                cmxTrayRowList.SelectedIndex = 0;
                cmxTrayColList.SelectedIndex = 0;
                Timer.Elapsed += Timer_Tick;
            }
            catch (Exception ex)
            {
            }
        }

        private async void btnRotate_Click(object sender, EventArgs e)
        {
            var btn = (HTButton)sender;
            var destStnNo = int.Parse(btn.Tag.ToString());
            if (UIMessageDialog.ShowAskDialog(null, $"是否旋转DD马达到{(Stn4TurnTableStnEnum)destStnNo}位置") ==false)
            {
                return;
            }
            Axes= new List<AxisName>() { AxisName.Stn4TurnTable_DD };
            if (IsCanMove(Axes)==false)
            {
                return;
            }
            var stageNo= cmxStageList.SelectedIndex;
            var curStnNo = RotateInfoHelper.Instance.GetStnNo(stageNo, RotateStnType.Stn4TurnTable);
            var destAngle = RotateInfoHelper.Instance.GetActMoveAngle(destStnNo, stageNo, RotateInterval, RotateStnType.Stn4TurnTable);
            //var diff = destStnNo - curStnNo;
            //var destAngle = diff * RotateInterval;
            //if (destAngle < 0)
            //{
            //    destAngle += 360;
            //}
            //else if (destAngle>360)
            //{
            //    destAngle -= 360;
            //}
            var ret = await Motion.MoveAbsSync(AxisName.Stn4TurnTable_DD, destAngle);
            if (ret)
            {
                var actPos = Motion.GetAxisActPos(AxisName.Stn4TurnTable_DD);
                RotateInfoHelper.Instance.UpdateRotateInfo(RotateStnType.Stn4TurnTable, Math.Round(actPos,1));
                FrmTips.ShowTipsSuccess(null, $"DD马达到{(Stn4TurnTableStnEnum)destStnNo}位置完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, $"DD马达到{(Stn4TurnTableStnEnum)destStnNo}位置失败");
            }
        }

        private async void btnHome_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, $"是否回零") == false)
            {
                return;
            }
            Axes = new List<AxisName>() { AxisName.Stn4TurnTable_DD };
            if (IsCanMove(Axes,true) == false)
            {
                return;
            }
           var ret=  await Motion.HomeSync(AxisName.Stn4TurnTable_DD,60);
            if (ret)
            {
                RotateInfoHelper.Instance.UpdateRotateInfo(RotateStnType.Stn4TurnTable, 0);
                FrmTips.ShowTipsSuccess(null, $"4工位DD马达回零完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, "4工位DD马达回零失败");
            }
        }

        private void cmxNozzleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            vacuumCtrl1.VacuumName=$"TrayStage{cmxStageList.SelectedIndex + 1}Vac";
            vacuumCtrl1.VacuumName = $"平台{cmxStageList.SelectedIndex + 1}真空";
        }

   
        private double GetPickPos(int pickNo)
        {
            double ret = 0;
            var posInfo = AxisInfoHelper.GetPosInfoByName(AxisName.PickDie_Z, PosName.PickDiePos.ToString());
            return ret;
        }

     
        private void btnBackTriggerPosSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxBackStartPos.Text)&& !string.IsNullOrEmpty(tbxBackEndPos.Text))
            {
                var startPos = double.Parse(tbxBackStartPos.Text);
                var endPos = double.Parse(tbxBackEndPos.Text);
                StdLib.PCBaseFrame.MotionInfo.CompareInfo compareInfo = CompareInfoHelper.GetCompareInfoByName(CompareName.FrontDetectGrab);
                AxisName axisName = AxisName.PickPatch_CamY;
             
                AxisInfoHelper.GetPosInfoByName(axisName, PosName.GrabStartPos).Pos = startPos; ;
                AxisInfoHelper.GetPosInfoByName(axisName, PosName.GrabEndPos).Pos = endPos; ;
                compareInfo.Para.PulseTime = short.Parse(tbxPulseWidth.Text);
                compareInfo.Para.MaxErr = double.Parse(tbxMaxError.Text);
                var ret=MotionConfig.Instance.SaveStationInfo();
                if (ret)
                {
                    FrmTips.ShowTipsSuccess(null, $"保存飞拍参数成功");
                }
                else
                {
                    FrmTips.ShowTipsWarning(null, "保存飞拍参数失败");
                }
            }
        }

        private void ShowTriggerInfo( )
        {
            PosInfo posInfo = null;
            PosInfo posInfo2 = null;
            StdLib.PCBaseFrame.MotionInfo.CompareInfo compareInfo = null;
            posInfo = AxisInfoHelper.GetPosInfoByName(AxisName.PickPatch_CamY, PosName.GrabStartPos);
            posInfo2 = AxisInfoHelper.GetPosInfoByName(AxisName.PickPatch_CamY, PosName.GrabEndPos);
            compareInfo = CompareInfoHelper.GetCompareInfoByName(CompareName.FrontDetectGrab);
            tbxBackStartPos.Text = posInfo.Pos.ToString();
            tbxBackEndPos.Text = posInfo2.Pos.ToString();
            tbxPulseWidth.Text=compareInfo.Para.PulseTime.ToString();
            tbxMaxError.Text=compareInfo.Para.MaxErr.ToString();
        }

        private async void btnBackDetectTrigger_Click(object sender, EventArgs e)
        {
            Timer.Start();
            Motion.CompareStop(CompareName.FrontDetectGrab);
            await Motion.MoveAbsSync(AxisName.PickPatch_CamY, PosName.GrabStartPos);
            Motion.SetCompare(CompareName.FrontDetectGrab);
            await Motion.MoveAbsSync(AxisName.PickPatch_CamY, PosName.GrabEndPos);
        }

        private async void btnTriggerStartPos_Click(object sender, EventArgs e)
        {
            await Motion.MoveAbsSync(AxisName.PickPatch_CamY, PosName.GrabStartPos);
        }

        private async void btnTriggerEndPos_Click(object sender, EventArgs e)
        {
            await Motion.MoveAbsSync(AxisName.PickPatch_CamY, PosName.GrabEndPos);
        }

        private async void btnAxisZDetectPos_Click(object sender, EventArgs e)
        {
            await Motion.MoveAbsSync(AxisName.PickPatch_CamZ, PosName.DetectPos);
        }

        private void btnTriggerStop_Click(object sender, EventArgs e)
        {
            Motion.CompareStop(CompareName.FrontDetectGrab);
            Timer.Stop();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            CompareName compareName = CompareName.FrontDetectGrab;
            Motion.GetCompare(compareName, out int count, out short status);
            this.Invoke(new Action(() =>
            {
                UpdateUI(count, status);
            }));
         
        }
        private void UpdateUI(int count, short status)
        {
            tbxBackDetectTriggerCount.Text = count.ToString();
            if (status == 0)
            {
                Timer.Stop();
                FrmTips.ShowTipsSuccess(null, "飞拍完成");
            }
        }

        private void Stn4TeachCtrl_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode() == false)
            {
                InitCtrl();
                Station = StationManager.Instance.GetChild<Stn4TurnTableStn>(nameof(Stn4TurnTableStn));
                Station.OnShowMsg += Station_OnShowMsg;
                ShowTriggerInfo();
            }
        }

        private void Station_OnShowMsg(string arg1, string arg2, Brush arg3)
        {
            this.Invoke(new Action(() =>
            {
                if (MsgBox.Items.Count > 1000)
                {
                    MsgBox.Items.Clear();
                }
                MsgBox.AddListBoxMsg(arg2, arg3);
            }));
        }

        private async void btnMoveToDie_Click(object sender, EventArgs e)
        {
            var rowIndex = cmxTrayRowList.SelectedIndex;
            var colIndex = cmxTrayColList.SelectedIndex;
            if (UIMessageDialog.ShowAskDialog(null,$"是否移动Tray到第{rowIndex}行第{colIndex}列")==false)
            {
                return;
            }
            var stageNo = RotateInfoHelper.Instance.GetStageNo(Stn4TurnTableStnEnum.DieToTray.ToInt(), RotateStnType.Stn4TurnTable);
            var curStage = StageManager.Get(stageNo);
            var axes = new List<AxisName>() { curStage.X, curStage.Y, curStage.T };
            if (IsCanMove(axes) == false)
            {
                return;
            }
            var pos= GetCellPos(rowIndex, colIndex);
            var ret=  await Motion.MoveAbsMSync(axes.ToArray(), pos.ToArray());
            if (ret)
            {
                FrmTips.ShowTipsSuccess(null, $"移动Tray到第{rowIndex }行第{colIndex}列完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, $"移动Tray到第{rowIndex+1}行第{colIndex+1}列失败");
            }
        }

        private async void btnTrayGrabPos_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否移动相机到拍Tray位置") == false)
            {
                return;
            }
            var stageNo = RotateInfoHelper.Instance.GetStageNo(Stn4TurnTableStnEnum.DieToTray.ToInt(), RotateStnType.Stn4TurnTable);
            var curStage = StageManager.Get(stageNo);
            var axes = new List<AxisName>() { curStage.X, curStage.Y, curStage.T,AxisName.PickPatch_CamY, AxisName.PickPatch_CamZ };
            if (IsCanMove(axes) == false)
            {
                return;
            }
            var posNames = new List<PosName>() { PosName.GrabTrayPos };
            var ret = await Motion.MoveAbsMSync(axes.ToArray(), posNames.ToArray());
            if (ret)
            {
                FrmTips.ShowTipsSuccess(null, $"相机移动到拍Tray位置完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, "相机移动到拍Tray位置失败");
            }
        }

        private void btnTrayGrab_Click(object sender, EventArgs e)
        {
			var body = AlignHelper.Instance.Send<AlignRspBody>(VisionCmd.Align, AlignStationEnum.Tray_DieToTray);
			if (body != null && body.Offset != null)
			{
                AlignXyt = body.Offset;
                tbxTrayXyt.Text=$"{AlignXyt.ToString()}";
                FrmTips.ShowTipsSuccess(null, $"Tray拍照完成");
            }
            else
            {
                AlignXyt = null;
                FrmTips.ShowTipsWarning(null, "Tray拍照失败");
            }
        }

        private async void btnTrayAlign_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否移动对位补正值") == false)
            {
                return;
            }
            var stageNo = RotateInfoHelper.Instance.GetStageNo(Stn4TurnTableStnEnum.DieToTray.ToInt(), RotateStnType.Stn4TurnTable);
            var curStage = StageManager.Get(stageNo);
            var axes = new List<AxisName>() { curStage.X, curStage.Y, curStage.T };
            if (IsCanMove(axes) == false)
            {
                return;
            }
            if (AlignXyt != null)
            {
               var ret= await  Motion.MoveRelMSync(axes.ToArray(), AlignXyt.ToArray());
                if (ret)
                {
                    FirstCellPos.X = Motion.GetAxisActPos(curStage.X) + ParaHelper.Prod.FirstCellOffsetX;//计算Tray第一个穴位的X方向坐标
                    FirstCellPos.Y = Motion.GetAxisActPos(curStage.Y) + ParaHelper.Prod.FirstCellOffsetY;//计算Tray第一个穴位的Y方向坐标
                    FrmTips.ShowTipsSuccess(null, $"移动补正值完成");
                }
                else
                {
                    FrmTips.ShowTipsWarning(null, "移动补正值失败");
                }
                AlignXyt = null;
            }
        }
        private Xyt GetCellPos(int rowNo, int colNo)
        {
            var x = FirstCellPos.Y + colNo * ParaHelper.Prod.CellPitchX;
            var y = FirstCellPos.X + rowNo * ParaHelper.Prod.CellPitchY;
            return new Xyt() { X = x, Y = y };
        }

        private void btnDieGrab_Click(object sender, EventArgs e)
        {
			var rowNo = cmxTrayRowList.SelectedIndex;
			var colNo = cmxTrayColList.SelectedIndex;
			var dieIndex = rowNo * ParaHelper.Prod.TrayColCount + colNo;
			var body = AlignHelper.Instance.Send<AlignRspBody>(VisionCmd.Align, AlignStationEnum.Tray_DieToTray, dieIndex);
			if (body != null && body.Offset != null)
            {
                AlignXyt = body.Offset;
                tbxTrayXyt.Text = $"{AlignXyt.ToString()}";
                FrmTips.ShowTipsSuccess(null, $"Die拍照完成");
            }
            else
            {
                AlignXyt = null;
                FrmTips.ShowTipsWarning(null, "Die拍照失败");
            }
        }

        private async void btnDieAlign_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否移动对位补正值") == false)
            {
                return;
            }
            var stageNo = RotateInfoHelper.Instance.GetStageNo(Stn4TurnTableStnEnum.DieToTray.ToInt(), RotateStnType.Stn4TurnTable);
            var curStage = StageManager.Get(stageNo);
            var axes = new List<AxisName>() { curStage.X, curStage.Y, curStage.T };
            if (IsCanMove(axes) == false)
            {
                return;
            }
            if (AlignXyt != null)
            {
                var ret = await Motion.MoveRelMSync(axes.ToArray(), AlignXyt.ToArray());
                if (ret)
                {
                    FrmTips.ShowTipsSuccess(null, $"移动补正值完成");
                }
                else
                {
                    FrmTips.ShowTipsWarning(null, "移动补正值失败");
                }
                AlignXyt = null;
            }
        }

        private async void btnLulTrayPos_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否移动到取放Tray位置") == false)
            {
                return;
            }
            var stageNo = RotateInfoHelper.Instance.GetStageNo(Stn4TurnTableStnEnum.LuloadTrayStn.ToInt(), RotateStnType.Stn4TurnTable);
            var curStage = StageManager.Get(stageNo);
            var axes = new List<AxisName>() { curStage.X, curStage.Y, curStage.T };
            if (IsCanMove(axes) == false)
            {
                return;
            }
            var posNames = new List<PosName>() { PosName.LuloadTrayPos };
           var ret= await Motion.MoveAbsMSync(axes.ToArray(), posNames.ToArray());
            if (ret)
            {
                FrmTips.ShowTipsSuccess(null, $"移动到取放Tray位置完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, "移动到取放Tray位置失败");
            }
        }

        private async void btnDieGrabPos_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否移动相机到拍Die位置") == false)
            {
                return;
            }
            var axes = new List<AxisName>() { AxisName.PickPatch_CamY, AxisName.PickPatch_CamZ };
            if (IsCanMove(axes) == false)
            {
                return;
            }
            var posNames = new List<PosName>() { PosName.GrabDiePos };
            var ret = await Motion.MoveAbsMSync(axes.ToArray(), posNames.ToArray());
            if (ret)
            {
                FrmTips.ShowTipsSuccess(null, $"相机到拍Die位置完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, "相机到拍Die位置失败");
            }
        }

        private async void btnLoadTrayReq_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否请求上空Tray") == false)
            {
                return;
            }
            if (IsCanMove(null) == false)
            {
                return;
            }
            btnLoadTrayReq.Enabled = false;
            Cts = new CancellationTokenSource();
            var station = StationManager.Instance.GetChild<LuloadTrayStn>(nameof(LuloadTrayStn));
            var ret = await TaskTool.RunStateMachine(station.LoadEmptyTrayFromStn8Flow, 1, Cts.Token, 100);
            if (ret == 0)
            {
                FrmTips.ShowTipsSuccess(null, $"上空Tray完成");
            }
            else if (ret == -1)
            {
                FrmTips.ShowTipsWarning(null, $"上空Tray失败");
            }
            btnLoadTrayReq.Enabled = true;
        }

        private void btnAutoPickStop_Click(object sender, EventArgs e)
        {
            Cts.Cancel();
            StationManager.Instance.Stop();
            EnableBtns(true);
        }
        private void EnableBtns(bool enable)
        {
            btnLoadTrayReq.Enabled = enable;
            btnUnloadTrayReq.Enabled = enable;
        }
    }
}
