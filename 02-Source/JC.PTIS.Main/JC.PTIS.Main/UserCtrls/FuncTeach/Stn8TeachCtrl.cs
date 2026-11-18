using JC.B17AutoMating.SubForms;
using JC.PTIS.Main.AlarmInfo;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Params;
using JC.PTIS.Main.RotateInfo;
using JC.PTIS.Main.Service;
using JC.PTIS.Main.Stations;
using StdLib.HTCard;
using StdLib.PCBaseFrame.Alarm;
using StdLib.PCBaseFrame.Extension.Args;
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
    public partial class Stn8TeachCtrl : TeachCtrlBase
    {
        public Stn8TeachCtrl()
        {
            InitializeComponent();
            StationName = StationName.Stn8TurnTableStn;
        }
        List<AxisName> Axes = null;
        List<PosName> PosNames = null;
        List<double> Poses = null;
        const double RotateInterval = 45;
        CancellationTokenSource Cts = null;
        private async void btnHome_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, $"是否回零") == false)
            {
                return;
            }
            Axes = new List<AxisName>() { AxisName.Stn8TurnTable_DD };
            if (IsCanMove(Axes, true) == false)
            {
                return;
            }
            var ret = await Motion.HomeSync(AxisName.Stn8TurnTable_DD, 60);
            if (ret)
            {
                RotateInfoHelper.Instance.UpdateRotateInfo(RotateStnType.Stn8TurnTable, 0);
                FrmTips.ShowTipsSuccess(null, $"8工位DD马达回零完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, "8工位DD马达回零失败");
            }
        }

        private async void btnStn8DDPPTrayPos_Click(object sender, EventArgs e)
        {
            var btn = (HTButton)sender;
            var destStnNo = int.Parse(btn.Tag.ToString());
            var stageNo = cmxStageList.SelectedIndex;
            if (UIMessageDialog.ShowAskDialog(null, $"是否旋转DD马达到{(Stn8TurnTableStnEnum)destStnNo}位置") == false)
            {
                return;
            }
            Axes = new List<AxisName>() { AxisName.Stn8TurnTable_DD };
            if (IsCanMove(Axes) == false)
            {
                return;
            }
            var angle = RotateInfoHelper.Instance.GetActMoveAngle(destStnNo, stageNo, RotateInterval, RotateStnType.Stn8TurnTable);

            //var curStnNo = RotateInfoHelper.Instance.GetStnNo(stageNo, RotateStnType.Stn8TurnTable);
            //var diff = destStnNo - curStnNo;
            //var destAngle = diff * RotateInterval;
            //if (destAngle < 0)
            //{
            //    destAngle += 360;
            //}
            //else if (destAngle > 360)
            //{
            //    destAngle -= 360;
            //}
            var ret = await Motion.MoveAbsSync(AxisName.Stn8TurnTable_DD, angle);
            if (ret)
            {
                var actPos = Motion.GetAxisActPos(AxisName.Stn8TurnTable_DD);
                RotateInfoHelper.Instance.UpdateRotateInfo(RotateStnType.Stn8TurnTable, Math.Round(actPos, 1));
                FrmTips.ShowTipsSuccess(null, $"DD马达到{(Stn8TurnTableStnEnum)destStnNo}位置完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, $"DD马达到{(Stn8TurnTableStnEnum)destStnNo}位置失败");
            }
        }

        private void btnArmWaitPos_Click(object sender, EventArgs e)
        {
            ModuleMove(PosName.WaitPos);
        }



        private void btnPPTrayFromStn4Pos_Click(object sender, EventArgs e)
        {
            PosName posName = PosName.EmptyArmPPTrayFrom4StnDDPos;
            if (cmxArmList.SelectedIndex == 1)
            {
                posName = PosName.FullArmPPTrayFrom4StnDDPos;
            }
            ModuleMove(posName);
        }
        private async void ModuleMove(PosName posName)
        {
            Axes = new List<AxisName>() { AxisName.Stn8TurnTable_ArmZ, AxisName.Stn8TurnTable_ArmX, AxisName.Stn8TurnTable_ArmY };
            if (IsCanMove(Axes) == false)
            {
                return;
            }
            PosNames = new List<PosName>() { PosName.WaitPos };
            var cyls = new List<CylinderName>() { CylinderName.EmptyTrayArmLiftCyl, CylinderName.FullTrayArmLiftCyl };
            var ret = await Motion.CylinderMAsync(cyls, new List<bool>() { false });
            if (ret)
            {
                Axes = new List<AxisName>() { AxisName.Stn8TurnTable_ArmZ };
                ret = await Motion.MoveAbsMSync(Axes.ToArray(), PosNames.ToArray());
                if (ret)
                {
                    PosNames = new List<PosName>() { posName };
                    Axes = new List<AxisName>() { AxisName.Stn8TurnTable_ArmX, AxisName.Stn8TurnTable_ArmY };

                    //隔离纸工位X和Y轴不在待机位时，禁止8工位转盘ArmX和ArmY移动到8工位取放Tray位置
                    if (posName == PosName.EmptyArmPPTrayFrom8StnDDPos || posName == PosName.FullArmPPTrayFrom8StnDDPos)
                    {
                        if (Motion.IsArrive(AxisName.IsolationPaper_ArmX, PosName.WaitPos) == false ||
                            Motion.IsArrive(AxisName.IsolationPaper_ArmY, PosName.WaitPos) == false)
                        {
                            FrmTips.ShowTipsWarning(null, $"{AxisName.IsolationPaper_ArmX}轴或者{AxisName.IsolationPaper_ArmY}轴不在待机位，" +
                                $"禁止ArmX和ArmY轴移动到{posName}位");
                            return;
                        }
                    }

                    //调补料相机Y轴不在待机位，禁止轴移动到4工位
                    if (posName == PosName.EmptyArmPPTrayFrom4StnDDPos || posName == PosName.FullArmPPTrayFrom4StnDDPos)
                    {
                        if (Motion.IsArrive(AxisName.PickPatch_CamY, PosName.WaitPos) == false)
                        {
                            FrmTips.ShowTipsWarning(null, $"{AxisName.PickPatch_CamY}轴不在待机位，禁止ArmX和ArmY轴移动到{posName}");
                            return;
                        }
                    }

                    ret = await Motion.MoveAbsMSync(Axes.ToArray(), PosNames.ToArray(), 60);
                    if (ret)
                    {
                        //Axes = new List<AxisName>() { AxisName.Stn8TurnTable_ArmZ };
                        //ret = await Motion.MoveAbsMSync(Axes.ToArray(), PosNames.ToArray());
                        //if (ret)
                        {
                            FrmTips.ShowTipsSuccess(null, $"Arm手臂轴移动到{posName}位置完成");
                        }
                        //else
                        //{
                        //    FrmTips.ShowTipsWarning(null, $"Arm手臂轴移动到{posName}位置失败");
                        //}
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

        private void Stn8TeachCtrl_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode() == false)
            {
                cmxStageList.SelectedIndex = 0;
                cmxArmList.SelectedIndex = 0;
            }
        }

        private void htButton1_Click(object sender, EventArgs e)
        {
            PosName posName = PosName.EmptyArmPPTrayFrom8StnDDPos;
            if (cmxArmList.SelectedIndex == 1)
            {
                posName = PosName.FullArmPPTrayFrom8StnDDPos;
            }
            ModuleMove(posName);
        }

        private void htButton2_Click(object sender, EventArgs e)
        {
            PosName posName = PosName.FullArmPPTrayFromHalfTrayStagePos;
            ModuleMove(posName);
        }

        private void htButton3_Click(object sender, EventArgs e)
        {
            PosName posName = PosName.PPTrayFromEmptyTrayBufferPos;
            ModuleMove(posName);
        }

        private async void btnLoadEmptyLotReq_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否请求上空Lot") == false)
            {
                return;
            }
            Axes = new List<AxisName>() { AxisName.Stn8TurnTable_ArmZ, AxisName.Stn8TurnTable_ArmX, AxisName.Stn8TurnTable_ArmY };
            if (IsCanMove(Axes) == false)
            {
                return;
            }
            btnLoadEmptyLotReq.Enabled = false;
            Cts = new CancellationTokenSource();
            var station = StationManager.Instance.GetChild<Stn8TurnTableStn>(nameof(Stn8TurnTableStn));
            var ret = await TaskTool.RunStateMachine(station.LoadLotFlow, 1, Cts.Token, 100);
            if (ret == 0)
            {
                FrmTips.ShowTipsSuccess(null, $"上空Lot完成");
            }
            else if (ret == -1)
            {
                FrmTips.ShowTipsWarning(null, $"上空Lot失败");
            }
            btnLoadEmptyLotReq.Enabled = true;
        }

        private async void btnUnloadFullLotReq_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否请求下满Lot") == false)
            {
                return;
            }
            Axes = new List<AxisName>() { AxisName.Stn8TurnTable_ArmZ, AxisName.Stn8TurnTable_ArmX, AxisName.Stn8TurnTable_ArmY };
            if (IsCanMove(Axes) == false)
            {
                return;
            }
            btnUnloadFullLotReq.Enabled = false;
            Cts = new CancellationTokenSource();
            var station = StationManager.Instance.GetChild<Stn8TurnTableStn>(nameof(Stn8TurnTableStn));
            var ret = await TaskTool.RunStateMachine(station.UnloadLotFlow, 1, Cts.Token, 100);
            if (ret == 0)
            {
                FrmTips.ShowTipsSuccess(null, $"下满Lot完成");
            }
            else if (ret == -1)
            {
                FrmTips.ShowTipsWarning(null, $"下满Lot失败");
            }
            btnUnloadFullLotReq.Enabled = true;
        }

        private void btnAutoPickStop_Click(object sender, EventArgs e)
        {
            Cts.Cancel();
            StationManager.Instance.Stop();
            EnableBtns(true);
        }
        private void EnableBtns(bool enable)
        {
            btnLoadEmptyLotReq.Enabled = enable;
            btnUnloadFullLotReq.Enabled = enable;
        }

        private async void btnPlacePaperReq_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否请求放隔离纸") == false)
            {
                return;
            }
            Axes = new List<AxisName>() { AxisName.Stn8TurnTable_ArmZ, AxisName.Stn8TurnTable_ArmX, AxisName.Stn8TurnTable_ArmY };
            if (IsCanMove(Axes) == false)
            {
                return;
            }
            btnPlacePaperReq.Enabled = false;
            RegisterService.Instance.SetBoolReg(RegisterEnum.PlacePaperReq);
            Stopwatch sw = new Stopwatch();
            sw.Start();
            bool ret = await Task.Run(() =>
            {
                while (true)
                {
                    if (RegisterService.Instance.GetBoolReg(RegisterEnum.PlacePaperDone))
                    {
                        RegisterService.Instance.ResetBoolReg(RegisterEnum.PlacePaperReq);
                        RegisterService.Instance.ResetBoolReg(RegisterEnum.PlacePaperDone);
                        return true;
                    }
                    else if (sw.ElapsedMilliseconds > 30 * 1000)
                    {
                        return false;
                    }
                    Thread.Sleep(100);
                }
            });
            if (ret)
            {
                FrmTips.ShowTipsSuccess(null, $"放隔离纸完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, $"放隔离纸失败");
            }
            btnPlacePaperReq.Enabled = true;
        }

        private async void btnAxisZPickTray_Click(object sender, EventArgs e)
        {
            var count = cmxTrayCount.SelectedIndex;
            var pos = GetPPTrayHeight(count);
            await Motion.MoveAbsSync(AxisName.Stn8TurnTable_ArmZ, pos);
        }

        private async void btnAxisPutTrayFlow_Click(object sender, EventArgs e)
        {
            var count = cmxTrayCount.SelectedIndex;
            var pos = GetPPTrayHeight(count, true);
            await Motion.MoveAbsSync(AxisName.Stn8TurnTable_ArmZ, pos);
        }

        private double GetPPTrayHeight(int trayCount, bool isPut = true)
        {
            double ret = 0;
            var posInfo = AxisInfoHelper.GetPosInfoByName(AxisName.Stn8TurnTable_ArmZ, PosName.PPTrayBaseHeight);
            if (posInfo != null)
            {
                if (isPut)
                {
                    ret = posInfo.Pos + trayCount * ParaHelper.Prod.TrayThickness + (trayCount - 1) * ParaHelper.Global.PaperThickness;
                }
                else
                {
                    ret = posInfo.Pos + trayCount * ParaHelper.Prod.TrayThickness;
                }
            }
            else
            {
                UIMessageDialog.ShowErrorDialog(null, $"{StationName}->获取{AxisName.Stn8TurnTable_ArmZ}轴{PosName.PPTrayBaseHeight}点位信息失败");
            }
            return ret;
        }
    }
}
