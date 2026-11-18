using JC.PTIS.Main.AlarmInfo;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.FlowInfo;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Params;
using JC.PTIS.Main.RotateInfo;
using JC.PTIS.Main.Service;
using StdLib.LogHelper;
using StdLib.PCBaseFrame.Alarm;
using StdLib.PCBaseFrame.Entities;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.PCBaseFrame.Motion;
using StdLib.PCBaseFrame.Station;
using StdLib.WindowsControls;
using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Stations
{
    /// <summary>
    /// 8工位转盘
    /// </summary>
    public class Stn8TurnTableStn : StationBaseEx, IStation
    {
        public int StationNum { get; set; } = 13;
        public string StationName { get; set; } = "Stn8TurnTableStn";
        const double StepAngle = 45d;
        List<AxisName> Axes = new List<AxisName>();
        List<PosName> PosNames = new List<PosName>();//位置名称集合
        List<CylinderName> CylinderNames = new List<CylinderName>();
        BinLevel TrayBinLevel = BinLevel.None;
        #region 子流程状态机步骤
        int LulLotStep = -1;
        int MoveToWaitPosStep = -1;
        int PickTrayFrom8StnStep = -1;
        int PutTrayToEmptyTrayBufferStep = -1;
        int PutTrayToHalfTrayStageStep = -1;
        int PickTrayFromHalfTrayStep = -1;
        int PutEmptyTrayTo4StnStep = -1;
        int PickFullTrayFrom4StnStep = -1;
        int PutFullTrayTo8StnStep = -1;
        int PutEmptyTrayTo8StnStep = -1;
        int PickTrayFromHalfTrayBufferStep = -1;
        int PutFullTrayToStn4Step = -1;
        #endregion

        double LotTrayDDPos = 0;
        double ArmZDestPos = 0;

        bool IsLulTrayStageEmpty = false;
        TrayModel OkTrayModel = new TrayModel();
        Delay Delay = new Delay();

        public void Home()
        {
            try
            {
                switch (HomeStep)
                {
                    case 1:
                        {
                            ShowMsg($"{StationName}->开始回零");
                            Motion.Home(AxisName.Stn8TurnTable_ArmZ);
                            ShowMsg($"{StationName}->Stn8TurnTable_ArmZ轴回零");
                            Cylinder(CylinderName.EmptyTrayArmLiftCyl, false);
                            Cylinder(CylinderName.FullTrayArmLiftCyl, false);
                            ShowMsg($"{StationName}->Stn8TurnTable_{CylinderName.EmptyTrayArmLiftCyl}气缸和{CylinderName.FullTrayArmLiftCyl}气缸缩回");

                            HomeStep = 50;
                            break;
                        }
                    case 50:
                        {
                            var ret = IsCylinderArrive(CylinderName.EmptyTrayArmLiftCyl, false);
                            var ret2 = IsCylinderArrive(CylinderName.FullTrayArmLiftCyl, false);
                            if (Motion.IsHomeDone(AxisName.Stn8TurnTable_ArmZ) &&
                            ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->Stn8TurnTable_{CylinderName.EmptyTrayArmLiftCyl}气缸和" +
                                    $"{CylinderName.FullTrayArmLiftCyl}气缸缩回到位");
                                ShowMsg($"{StationName}->Stn8TurnTable_ArmZ轴回零完成");
                                Motion.Home(AxisName.Stn8TurnTable_ArmX);
                                ShowMsg($"{StationName}->Stn8TurnTable_ArmX轴开始回零");
                                HomeStep = 100;
                            }
                            else if (Motion.IsMoveTimeOut(AxisName.Stn8TurnTable_ArmZ))
                            {
                                ShowMsg($"{StationName}->Stn8TurnTable_ArmZ轴回零超时", MsgType.Error);
                                HomeStep = 1000;
                            }
                            else if (ret == MotionStatus.Timeout)
                            {
                                ShowMsg($"{StationName}->Stn8TurnTable_{CylinderName.EmptyTrayArmLiftCyl}气缸缩回超时", MsgType.Error);
                                HomeStep = 1000;
                            }
                            else if (ret2 == MotionStatus.Timeout)
                            {
                                ShowMsg($"{StationName}->Stn8TurnTable_{CylinderName.FullTrayArmLiftCyl}气缸缩回超时", MsgType.Error);
                                HomeStep = 1000;
                            }
                            break;
                        }
                    case 100:
                        {
                            if (Motion.IsHomeDone(AxisName.Stn8TurnTable_ArmX))
                            {
                                ShowMsg($"{StationName}->Stn8TurnTable_ArmX轴回零完成");
                                Motion.MoveAbs(AxisName.Stn8TurnTable_ArmX, PosName.WaitPos);
                                ShowMsg($"{StationName}->Stn8TurnTable_ArmX轴移动到{PosName.WaitPos}位");
                                HomeStep = 150;
                            }
                            else if (Motion.IsMoveTimeOut(AxisName.Stn8TurnTable_ArmX))
                            {
                                ShowMsg($"{StationName}->Stn8TurnTable_ArmX轴回零超时", MsgType.Error);
                                HomeStep = 1000;
                            }
                            break;
                        }
                    case 150:
                        {
                            if (Motion.IsArrive(AxisName.Stn8TurnTable_ArmX, PosName.WaitPos))
                            {
                                ShowMsg($"{StationName}->Stn8TurnTable_ArmX轴到达{PosName.WaitPos}位");
                                ShowMsg($"{StationName}->等待{AxisName.IsolationPaper_ArmX}轴和" +
                                    $"{AxisName.IsolationPaper_ArmY}轴运动到{PosName.WaitPos}位置");
                                Delay.Start(30);
                                HomeStep = 200;
                            }
                            else if (Motion.IsMoveTimeOut(AxisName.Stn8TurnTable_ArmX))
                            {
                                ShowMsg($"{StationName}->Stn8TurnTable_ArmX轴移动到{PosName.WaitPos}位超时", MsgType.Error);
                                HomeStep = 1000;
                            }
                            break;
                        }
                    case 200:
                        {
                            if (Motion.IsArrive(AxisName.IsolationPaper_ArmX, PosName.WaitPos) &&
                                Motion.IsArrive(AxisName.IsolationPaper_ArmY, PosName.WaitPos))
                            {
                                ShowMsg($"{StationName}->{AxisName.IsolationPaper_ArmX}轴" +
                                    $"和{AxisName.IsolationPaper_ArmY}轴到达{PosName.WaitPos}位置");
                                Motion.Home(AxisName.Stn8TurnTable_ArmY);
                                Motion.Home(AxisName.Stn8TurnTable_Shift);
                                ShowMsg($"{StationName}->Stn8TurnTable_{AxisName.Stn8TurnTable_ArmY}轴、{AxisName.Stn8TurnTable_Shift}轴开始回零");
                                HomeStep = 250;
                            }
                            else if (Delay.TimeUp())
                            {
                                ShowMsg($"{StationName}->等待{AxisName.IsolationPaper_ArmX}轴运动到{PosName.WaitPos}超时", MsgType.Error);
                                HomeStep = 1000;
                            }
                            break;
                        }
                    case 250:
                        {
                            if (Motion.IsHomeDone(AxisName.Stn8TurnTable_ArmY) && Motion.IsHomeDone(AxisName.Stn8TurnTable_Shift))
                            {
                                ShowMsg($"{StationName}->{AxisName.Stn8TurnTable_ArmY}轴和{AxisName.Stn8TurnTable_Shift}回零完成");
                                Motion.Home(AxisName.Stn8TurnTable_DD);
                                ShowMsg($"{StationName}->Stn8TurnTable_DD轴开始回零");
                                HomeStep = 300;
                            }
                            else if (Motion.IsMoveTimeOut(AxisName.Stn8TurnTable_ArmY))
                            {
                                ShowMsg($"{StationName}->Stn8TurnTable_ArmY轴回零超时", MsgType.Error);
                                HomeStep = 1000;
                            }
                            else if (Motion.IsMoveTimeOut(AxisName.Stn8TurnTable_Shift))
                            {
                                ShowMsg($"{StationName}->Stn8TurnTable_Shift轴回零超时", MsgType.Error);
                                HomeStep = 1000;
                            }
                            break;
                        }
                    case 300:
                        {
                            if (Motion.IsHomeDone(AxisName.Stn8TurnTable_DD))
                            {
                                ShowMsg($"{StationName}->Stn8TurnTable_DD轴回零完成");
                                Motion.HomeOffset(AxisName.Stn8TurnTable_DD);
                                Delay.Start(ParaHelper.Motion.WaitAxisMoveTimeOut);
                                ShowMsg($"{StationName}->Stn8TurnTable_DD轴移动到回零偏移位置");
                                HomeStep = 350;
                                //if (Global.RunMode==RunMode.空跑模式)
                                //{
                                //    for (int i = 1; i <= 6; i++)
                                //    {
                                //        IOInfoHelper.GetInputInfoByName($"Stn8Bin{i}StageDetect",out int _).IsShield=false;
                                //    }
                                //}
                            }
                            else if (Motion.IsMoveTimeOut(AxisName.Stn8TurnTable_DD))
                            {
                                ShowMsg($"{StationName}->Stn8TurnTable_DD轴回零超时", MsgType.Error);
                                HomeStep = 1000;
                            }
                            break;
                        }
                    case 350:
                        {
                            if (Motion.IsHomeOffsetDone(AxisName.Stn8TurnTable_DD))
                            {
                                ShowMsg($"{StationName}->Stn8TurnTable_DD轴到达回零偏移位置");
                                Motion.MoveAbs(AxisName.Stn8TurnTable_ArmX, PosName.WaitPos);
                                Motion.MoveAbs(AxisName.Stn8TurnTable_ArmY, PosName.WaitPos);
                                ShowMsg($"{StationName}->{AxisName.Stn8TurnTable_ArmX}轴和{AxisName.Stn8TurnTable_ArmY}轴移动到{PosName.WaitPos}位");
                                HomeStep = 400;
                            }
                            else if (Delay.TimeUp())
                            {
                                ShowMsg($"{StationName}->{AxisName.Stn8TurnTable_DD}轴移动到回零偏移位置超时", MsgType.Error);
                                HomeStep = 1000;
                            }
                            break;
                        }
                    case 400:
                        {
                            if (Motion.IsArrive(AxisName.Stn8TurnTable_ArmX, PosName.WaitPos) &&
                                Motion.IsArrive(AxisName.Stn8TurnTable_ArmY, PosName.WaitPos))
                            {
                                ShowMsg($"{StationName}->{AxisName.Stn8TurnTable_ArmX}轴和{AxisName.Stn8TurnTable_ArmY}轴到达{PosName.WaitPos}位");
                                RotateInfoHelper.Instance.UpdateRotateInfo(RotateStnType.Stn8TurnTable, 0);
                                UpdateHomeSts(StationName, StationNum, true);
                                HomeStep = 0;
                            }
                            else if (Motion.IsMoveTimeOut(AxisName.Stn8TurnTable_ArmX))
                            {
                                ShowMsg($"{StationName}->{AxisName.Stn8TurnTable_ArmX}轴移动到{PosName.WaitPos}位超时", MsgType.Error);
                                HomeStep = 1000;
                            }
                            else if (Motion.IsMoveTimeOut(AxisName.Stn8TurnTable_ArmY))
                            {
                                ShowMsg($"{StationName}->{AxisName.Stn8TurnTable_ArmY}轴移动到{PosName.WaitPos}位超时", MsgType.Error);
                                HomeStep = 1000;
                            }
                            break;
                        }
                    case 1000:
                        {
                            UpdateHomeSts(StationName, StationNum, false);
                            HomeStep = 0;
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("Error").Error($"[Stn8TurnTableStn][Home]方法执行异常，异常原因:{ex}");
                HomeStep = -1;
            }
        }

        public void AutoRun()
        {
            try
            {
                switch (AutoRunStep)
                {
                    case 1:
                        {
                            ShowMsg($"{StationName}->执行轴回待机位流程");
                            MoveToWaitPosStep = 1;
                            AutoRunStep = 50;
                            break;
                        }
                    case 50:
                        {
                            MoveToWaitPosStep = MoveToWaitPosFlow(MoveToWaitPosStep);
                            if (MoveToWaitPosStep == 0)
                            {
                                ShowMsg($"{StationName}->回待机位流程结束，判断取放Tray位置是否有Tray");
                                AutoRunStep = 100;
                            }
                            break;
                        }
                    case 100:
                        {
                            if (Motion.GetDigitalInput(InputName.Stn8LulStageDetect) == false)
                            {
                                ShowMsg($"{StationName}->取放Lot位置没有检测到Tray，执行LoadLot流程");
                                LulLotStep = 1;
                                AutoRunStep = 150;
                            }
                            else
                            {
                                if (Motion.GetDigitalInput(InputName.EmptyTrayArmIsExistTray))
                                {
                                    if (Motion.GetDigitalInput(InputName.EmptyTrayBufferVacDetect))
                                    {
                                        if (GetBoolReg(RegisterEnum.Stn4DDStnLoadEmptyReq))
                                        {
                                            ShowMsg($"{StationName}->收到4工位转盘上空Tray请求，执行放空Tray到4工位转盘流程");
                                            PutEmptyTrayTo4StnStep = 1;
                                            AutoRunStep = 200;
                                        }
                                        else if (Motion.GetDigitalInput(InputName.FullTrayArmIsExistTray))
                                        {
                                            var trayInfo = ProcInfoHelper.Instance.GetTrayInfo(TrayPosEnum.FullTrayArm);
                                            if (trayInfo != null && trayInfo.TrayModel != null)
                                            {
                                                if (GetBoolReg(RegisterEnum.LoadTrayFromHalfTrayBufferToStn4Req))
                                                {
                                                    ResetBoolReg(RegisterEnum.LoadTrayFromHalfTrayBufferToStn4Req);
                                                    var value = false;
                                                    var binLevel = RegisterService.Instance.GetIntValue(IntValueEnum.ShiftBinLevel);
                                                    if (trayInfo.TrayModel.BinLevel == binLevel)//满Tray手臂上有Tray，但Bin等级与4工位转盘请求的Bin等级不一致
                                                    {
                                                        value = true;
                                                    }
                                                    if (value)
                                                    {
                                                        ShowMsg($"{StationName}->收到4工位转盘从半Tray缓存取Tray请求，等级：Bin{binLevel}，满Tray手臂上有Tray，执行满Tray手臂放半Tray到4工位转盘流程");
                                                        PutFullTrayToStn4Step = 1;
                                                        AutoRunStep = 900;
                                                    }
                                                }
                                                else if (GetBoolReg(RegisterEnum.HalfTrayStnLoadFullTrayReq) &&
                                                    HalfTrayStn.IsHalfTrayStageArriveLulPos(true))
                                                {
                                                    RegisterService.Instance.SetBoolReg(RegisterEnum.TrayArmNoExistHalfTrayToStorage);
                                                    ShowMsg($"{StationName}->收到挑补平台上OkTray请求，执行满Tray手臂放Tray到挑补平台流程");
                                                    PutTrayToHalfTrayStageStep = 1;
                                                    AutoRunStep = 250;
                                                }
                                                else if (GetBoolReg(RegisterEnum.StorageHalfTrayReq) && trayInfo.TrayModel.TrayType == TrayTypeEnum.Half)
                                                {
                                                    ShowMsg($"{StationName}->收到4工位转盘暂存半Tray请求，发送满Tray手臂暂存半Tray信号给HalfTray工位");
                                                    RegisterService.Instance.SetBoolReg(RegisterEnum.FullTrayArmStorageHalfTrayReq);
                                                    AutoRunStep = 750;
                                                }
                                                else if (trayInfo.TrayModel.TrayType == TrayTypeEnum.Full)
                                                {
                                                    RegisterService.Instance.SetBoolReg(RegisterEnum.TrayArmNoExistHalfTrayToStorage);
                                                    ShowMsg($"{StationName}->未收到挑补平台上OkTray请求，执行满Tray手臂放Tray到8工位转盘流程");
                                                    PutFullTrayTo8StnStep = 1;
                                                    AutoRunStep = 260;
                                                }
                                            }
                                            else
                                            {
                                                ShowMsg($"{StationName}->从满Tray手臂获取TrayModel信息失败", MsgType.Error, true);
                                                AlarmManager.CreateAlarm(AlarmCode.GetTrayInfoFail.ToInt(), StationNum);
                                            }
                                        }
                                        else if (GetBoolReg(RegisterEnum.HalfTrayStnUnloadEmptyTrayReq))
                                        {
                                            ShowMsg($"{StationName}->收到挑补平台空Tray下料请求，执行从挑补平台取空Tray流程");
                                            PickTrayFromHalfTrayStep = 1;
                                            AutoRunStep = 650;
                                        }
                                        else if (GetBoolReg(RegisterEnum.HalfTrayStnLoadEmptyTrayReq) &&
                                            HalfTrayStn.IsHalfTrayStageArriveLulPos(false))
                                        {
                                            ShowMsg($"{StationName}->收到挑补平台空Tray上料请求，执行从8工位转盘取空Tray流程");
                                            PickTrayFrom8StnStep = 1;
                                            AutoRunStep = 350;//使用满TrayArm从转盘上取空Tray
                                        }
                                        else if (GetBoolReg(RegisterEnum.LoadTrayFromHalfTrayBufferToStn4Req) &&
                                            !GetBoolReg(RegisterEnum.HalfTrayStnLoadEmptyTrayReq))
                                        {
                                            ResetBoolReg(RegisterEnum.LoadTrayFromHalfTrayBufferToStn4Req);
                                            var binLevel = RegisterService.Instance.GetIntValue(IntValueEnum.ShiftBinLevel);
                                            ShowMsg($"{StationName}->收到4工位转盘从半Tray缓存取Tray请求，等级：{binLevel}，执行从OkTray平台取Tray流程");
                                            PickTrayFromHalfTrayBufferStep = 1;
                                            AutoRunStep = 850;
                                        }
                                        else if (GetBoolReg(RegisterEnum.HalfTrayStnUnloadFullTrayReq))
                                        {
                                            ShowMsg($"{StationName}->收到挑补平台满Tray下料请求，执行下满Tray流程");
                                            PickTrayFromHalfTrayStep = 1;
                                            AutoRunStep = 550;
                                        }
                                        else if (GetBoolReg(RegisterEnum.Stn4DDUnloadFullTrayReq))
                                        {
                                            ShowMsg($"{StationName}->收到4工位转盘满Tray下料请求，执行下满Tray流程");
                                            PickFullTrayFrom4StnStep = 1;
                                            AutoRunStep = 600;
                                        }
                                    }
                                    else
                                    {
                                        ShowMsg($"{StationName}->空Tray缓存没有Tray，执行放空Tray到缓存平台流程");
                                        PutTrayToEmptyTrayBufferStep = 1;
                                        AutoRunStep = 300;
                                    }
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->空Tray夹爪没有Tray，执行从8工位转盘取空Tray流程");
                                    PickTrayFrom8StnStep = 1;
                                    AutoRunStep = 450;//使用空TrayArm从转盘上取空Tray
                                }
                            }
                            break;
                        }
                    case 150:
                        {
                            LulLotStep = LoadLotFlow(LulLotStep);
                            if (LulLotStep == 0)
                            {
                                ShowMsg($"{StationName}->LoadLot流程结束");
                                AutoRunStep = 100;
                            }
                            break;
                        }
                    case 200:
                        {
                            PutEmptyTrayTo4StnStep = PutEmptyTrayTo4StnFlow(PutEmptyTrayTo4StnStep);
                            if (PutEmptyTrayTo4StnStep == 0)
                            {
                                ResetBoolReg(RegisterEnum.Stn4DDStnLoadEmptyReq);//复位请求
                                RegisterService.Instance.SetBoolReg(RegisterEnum.TrayArmLoadEmptyTrayDone);
                                ShowMsg($"{StationName}->放空Tray到4工位转盘流程结束");
                                AutoRunStep = 100;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.EmptyTrayArmIsExistTray.ToString(), out int _).IsShield = false;
                                    Thread.Sleep(500);
                                }
                            }
                            break;
                        }
                    case 250:
                        {
                            PutTrayToHalfTrayStageStep = PutTrayToHalfTrayStageFlow(PutTrayToHalfTrayStageStep, false);
                            if (PutTrayToHalfTrayStageStep == 0)
                            {
                                ResetBoolReg(RegisterEnum.HalfTrayStnLoadFullTrayReq);
                                RegisterService.Instance.SetBoolReg(RegisterEnum.TrayArmPutOKFullTrayDone);
                                ShowMsg($"{StationName}->把满Tray放到HalfTray平台流程结束，发送放满Tray完成信号给HalfTray工位");
                                AutoRunStep = 100;
                            }
                            break;
                        }
                    case 260:
                        {
                            PutFullTrayTo8StnStep = PutFullTrayTo8StnFlow(PutFullTrayTo8StnStep);
                            if (PutFullTrayTo8StnStep == 0)
                            {
                                ShowMsg($"{StationName}->放满Tray到8工位转盘流程结束");
                                AutoRunStep = 100;
                            }
                            break;
                        }
                    case 300:
                        {
                            PutTrayToEmptyTrayBufferStep = PutTrayToEmptyTrayBufferFlow(PutTrayToEmptyTrayBufferStep);
                            if (PutTrayToEmptyTrayBufferStep == 0)
                            {
                                AutoRunStep = 100;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.EmptyTrayBufferVacDetect.ToString(), out int _).IsShield = true;
                                    IOInfoHelper.GetInputInfoByName(InputName.EmptyTrayArmIsExistTray.ToString(), out int _).IsShield = false;
                                    Thread.Sleep(500);
                                }
                            }
                            break;
                        }
                    case 350:
                        {
                            PickTrayFrom8StnStep = PickEmptyTrayFrom8StnFlow(PickTrayFrom8StnStep, true);
                            if (PickTrayFrom8StnStep == 0)
                            {
                                ResetBoolReg(RegisterEnum.HalfTrayStnLoadEmptyTrayReq);
                                PutTrayToHalfTrayStageStep = 1;
                                ShowMsg($"{StationName}->满TrayArm取Tray完成，执行把空Tray放到HalfTray平台流程");
                                AutoRunStep = 400;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.FullTrayArmIsExistTray.ToString(), out int _).IsShield = true;
                                    Thread.Sleep(500);
                                }
                            }
                            break;
                        }
                    case 400:
                        {
                            PutTrayToHalfTrayStageStep = PutTrayToHalfTrayStageFlow(PutTrayToHalfTrayStageStep, true);
                            if (PutTrayToHalfTrayStageStep == 0)
                            {
                                RegisterService.Instance.SetBoolReg(RegisterEnum.TrayArmPutEmptyTrayDone);
                                ShowMsg($"{StationName}->把空Tray放到HalfTray平台流程结束，发送放空Tray完成信号给HalfTray工位");
                                AutoRunStep = 100;
                            }
                            break;
                        }
                    case 450:
                        {
                            PickTrayFrom8StnStep = PickEmptyTrayFrom8StnFlow(PickTrayFrom8StnStep, false);
                            if (PickTrayFrom8StnStep == 0)
                            {
                                ShowMsg($"{StationName}->空TrayArm取Tray完成");
                                AutoRunStep = 100;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.EmptyTrayArmIsExistTray.ToString(), out int _).IsShield = true;
                                    Thread.Sleep(500);
                                }
                            }
                            break;
                        }
                    case 550:
                        {
                            PickTrayFromHalfTrayStep = PickTrayFromHalfTrayStageFlow(PickTrayFromHalfTrayStep, true);
                            if (PickTrayFromHalfTrayStep == 0)
                            {
                                ResetBoolReg(RegisterEnum.HalfTrayStnUnloadFullTrayReq);//复位请求
                                RegisterService.Instance.SetBoolReg(RegisterEnum.FullTrayArmPickFullTrayFromHalfTrayDone);//从挑补平台取放Tray都用满TrayArm
                                ShowMsg($"{StationName}->从挑补平台取满Tray流程结束");
                                AutoRunStep = 100;

                            }
                            break;
                        }
                    case 600:
                        {
                            PickFullTrayFrom4StnStep = PickFullTrayFrom4StnFlow(PickFullTrayFrom4StnStep);
                            if (PickFullTrayFrom4StnStep == 0)
                            {
                                ResetBoolReg(RegisterEnum.Stn4DDUnloadFullTrayReq);//复位请求
                                RegisterService.Instance.SetBoolReg(RegisterEnum.TrayArmUnloadFullTrayDone);
                                ShowMsg($"{StationName}->从4工位转盘取满Tray流程结束");
                                AutoRunStep = 100;
                            }
                            break;
                        }
                    case 650:
                        {
                            PickTrayFromHalfTrayStep = PickTrayFromHalfTrayStageFlow(PickTrayFromHalfTrayStep, false);
                            if (PickTrayFromHalfTrayStep == 0)
                            {
                                ResetBoolReg(RegisterEnum.HalfTrayStnUnloadEmptyTrayReq);
                                RegisterService.Instance.SetBoolReg(RegisterEnum.FullArmPickEmptyTrayFromHalfTrayDone);//从挑补平台取放Tray都用满TrayArm
                                ShowMsg($"{StationName}->从挑补平台取空Tray流程结束，执行放空Tray到8工位转盘流程");
                                PutEmptyTrayTo8StnStep = 1;
                                AutoRunStep = 700;
                            }

                            break;
                        }
                    case 700:
                        {
                            PutEmptyTrayTo8StnStep = PutEmptyTrayTo8StnFlow(PutEmptyTrayTo8StnStep);//放空Tray到8工位转盘
                            if (PutEmptyTrayTo8StnStep == 0)
                            {
                                AutoRunStep = 100;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.FullTrayArmIsExistTray.ToString(), out int _).IsShield = false;
                                    Thread.Sleep(500);
                                }
                            }
                            break;
                        }
                    case 750:
                        {
                            if (GetBoolReg(RegisterEnum.HalfTrayStageArrivePutTrayPos))
                            {
                                ShowMsg($"{StationName}->收到HalfTray工位OkTray平台到达取放Tray位置，执行满Tray手臂放半Tray到HalfTray平台流程");
                                PutTrayToHalfTrayStageStep = 1;
                                AutoRunStep = 800;
                            }
                            break;
                        }
                    case 800:
                        {
                            PutTrayToHalfTrayStageStep = PutTrayToHalfTrayStageFlow(PutTrayToHalfTrayStageStep, false);
                            if (PutTrayToHalfTrayStageStep == 0)
                            {
                                ResetBoolReg(RegisterEnum.StorageHalfTrayReq);
                                ResetBoolReg(RegisterEnum.HalfTrayStageArrivePutTrayPos);
                                RegisterService.Instance.SetBoolReg(RegisterEnum.TrayArmPutOKFullTrayDone);
                                ShowMsg($"{StationName}->把满Tray放到HalfTray平台流程结束，发送放满Tray完成信号给HalfTray工位");
                                AutoRunStep = 950;
                            }
                            break;
                        }
                    case 850:
                        {
                            PickTrayFromHalfTrayBufferStep = PickTrayFromHalfTrayBufferFlow(PickTrayFromHalfTrayBufferStep);
                            if (PickTrayFromHalfTrayBufferStep == 0)
                            {
                                ShowMsg($"{StationName}->满Tray手臂从半Tray缓存取Tray流程结束，执行满Tray手臂放半Tray到4工位转盘流程");
                                PutFullTrayToStn4Step = 1;
                                AutoRunStep = 900;
                            }
                            break;
                        }
                    case 900:
                        {
                            PutFullTrayToStn4Step = PutFullTrayToStn4Flow(PutFullTrayToStn4Step);
                            if (PutFullTrayToStn4Step == 0)
                            {
                                ShowMsg($"{StationName}->满Tray手臂放半Tray到4工位转盘流程结束");
                                AutoRunStep = 100;
                            }
                            break;
                        }
                    case 950:
                        {
                            if (GetBoolReg(RegisterEnum.StorageHalfTrayDone))
                            {
                                ResetBoolReg(RegisterEnum.StorageHalfTrayDone);
                                ShowMsg($"{StationName}->HalfTray工位暂存半Tray完成");
                                AutoRunStep = 100;
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                ShowMsg($"{StationName}->AutoRun方法异常：{ex}", MsgType.Error, true);
                AlarmManager.CreateAlarm(AlarmCode.ProgramExceptionStart.ToInt() + StationNum, AllStationAlarm);
                StopRequest();
            }
        }
        /// <summary>
        /// 从半Tray缓存取Tray放到4工位转盘取放Tray流程
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int PickTrayFromHalfTrayBufferFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        if (Motion.GetDigitalInput(InputName.FullTrayArmIsExistTray) == false)
                        {
                            ShowMsg($"{StationName}->满Tray手臂从半Tray缓存取半Tray流程开始，满Tray手臂移动到半Tray缓存取Tray位置", MsgType.Notify);
                            Axes = new List<AxisName>() { AxisName.Stn8TurnTable_ArmX, AxisName.Stn8TurnTable_ArmY };
                            PosNames = new List<PosName>() { PosName.FullArmPPTrayFromHalfTrayStagePos };
                            MoveAbsM(Axes, PosNames);
                            Cylinder(CylinderName.FullTrayArmClawCyl, false);
                            step = 50;
                        }
                        else
                        {
                            ShowMsg($"{StationName}->满TrayArm存在Tray，无法执行从半Tray缓存取半Tray流程", MsgType.Error, true);
                            AlarmManager.CreateAlarm(AlarmCode.FullTrayArmExistTray.ToInt(), StationNum);
                        }
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        var ret2 = IsCylinderArrive(CylinderName.FullTrayArmClawCyl, false);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->满Tray手臂到达半Tray缓存取Tray位置，发送满Tray手臂从半Tray缓存取半Tray请求");
                            RegisterService.Instance.SetBoolReg(RegisterEnum.FullTrayArmPickHalfTrayFromBufferReq);
                            step = 100;
                        }
                        else if (ret == MotionStatus.Timeout || ret2 == MotionStatus.Timeout)
                        {
                            step = 1;
                        }
                        break;
                    }
                case 100:
                    {
                        if (GetBoolReg(RegisterEnum.OkHalfTrayStageAllowPickTray))
                        {
                            ResetBoolReg(RegisterEnum.OkHalfTrayStageAllowPickTray);
                            ShowMsg($"{StationName}->Ok半Tray平台允许取Tray，ArmZ轴下降");
                            MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.FullArmPPTrayFromHalfTrayStagePos);
                            step = 150;
                        }
                        break;
                    }
                case 150:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.FullArmPPTrayFromHalfTrayStagePos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->满Tray手臂ArmZ轴下降到位，抓取半Tray");
                            Cylinder(CylinderName.FullTrayArmClawCyl, true);
                            step = 200;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 100;
                        }
                        break;
                    }
                case 200:
                    {
                        var ret = IsCylinderArrive(CylinderName.FullTrayArmClawCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->满Tray手臂抓取半Tray到位，ArmZ轴上升");
                            MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                            var trayModel = ProcInfoHelper.Instance.PickTrayFromTrayStage(TrayPosEnum.OkHalfTrayStage);
                            if (trayModel != null)
                            {
                                ProcInfoHelper.Instance.PutTrayToTrayStage(TrayPosEnum.FullTrayArm, trayModel);
                                step = 250;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.OKHalfTrayVacDetect.ToString(), out int _).IsShield = false;
                                    IOInfoHelper.GetInputInfoByName(InputName.FullTrayArmIsExistTray.ToString(), out int _).IsShield = true;
                                    Thread.Sleep(500);
                                }
                            }
                            else
                            {
                                ShowMsg($"{StationName}->从Ok半Tray平台获取Tray信息失败", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.GetTrayInfoFail.ToInt(), StationNum);
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 150;
                        }
                        break;
                    }
                case 250:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->满Tray手臂ArmZ轴上升到位，移动到待机位");
                            RegisterService.Instance.SetBoolReg(RegisterEnum.FullTrayArmPickTrayFromOkHalfTrayStageDone);
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 200;
                        }
                        break;
                    }

            }
            return step;
        }
        /// <summary>
        /// 满Tray手臂放半Tray到4工位转盘流程
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int PutFullTrayToStn4Flow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->满Tray手臂放半Tray到4工位转盘流程开始，ArmZ轴移动到待机位置", MsgType.Notify);
                        MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->满Tray手臂ArmZ轴到达待机位");
                            Delay.Start(ParaHelper.Motion.WaitAxisMoveTimeOut);
                            ShowMsg($"{StationName}->等待{AxisName.PickPatch_CamY}轴移动到{PosName.WaitPos}位");
                            step = 100;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1;
                        }
                        break;
                    }
                case 100:
                    {
                        if (Motion.IsArrive(AxisName.PickPatch_CamY, PosName.WaitPos))
                        {
                            ShowMsg($"{StationName}->等待{AxisName.PickPatch_CamY}轴到达{PosName.WaitPos}位");
                            Axes = new List<AxisName>() { AxisName.Stn8TurnTable_ArmX, AxisName.Stn8TurnTable_ArmY };
                            PosNames = new List<PosName>() { PosName.FullArmPPTrayFrom4StnDDPos };
                            MoveAbsM(Axes, PosNames);
                            ShowMsg($"{StationName}->{AxisName.Stn8TurnTable_ArmX}轴和{AxisName.Stn8TurnTable_ArmY}轴移动到{PosName.FullArmPPTrayFrom4StnDDPos}位");
                            step = 150;
                        }
                        else if (Delay.TimeUp())
                        {
                            ShowMsg($"{StationName}->等待{AxisName.PickPatch_CamY}轴到达{PosName.WaitPos}位超时");
                            step = -1;
                        }
                        break;
                    }
                case 150:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->满Tray手臂移动到4工位转盘取放Tray位置，ArmZ轴下降");
                            MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.FullArmPPTrayFrom4StnDDPos);
                            step = 200;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 100;
                        }
                        break;
                    }
                case 200:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.FullArmPPTrayFrom4StnDDPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->满Tray手臂ArmZ轴下降到位，放置半Tray到4工位转盘");
                            Cylinder(CylinderName.FullTrayArmClawCyl, false);
                            step = 250;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 150;
                        }
                        break;
                    }
                case 250:
                    {
                        var ret = IsCylinderArrive(CylinderName.FullTrayArmClawCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->满Tray手臂放置半Tray到位，ArmZ轴上升");
                            MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                            var trayModel = ProcInfoHelper.Instance.PickTrayFromTrayStage(TrayPosEnum.FullTrayArm);
                            if (trayModel != null)
                            {
                                ProcInfoHelper.Instance.PutTrayToTrayStage(Stn4TurnTableStnEnum.LuloadTrayStn, trayModel);
                                step = 300;


                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.FullTrayArmIsExistTray.ToString(), out int _).IsShield = false;
                                    Thread.Sleep(500);
                                }
                            }
                            else
                            {
                                ShowMsg($"{StationName}->从满Tray手臂获取Tray信息失败", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.GetTrayInfoFail.ToInt(), StationNum);
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 200;
                        }
                        break;
                    }
                case 300:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->满Tray手臂ArmZ轴上升到位，移动到待机位");
                            Axes = new List<AxisName>() { AxisName.Stn8TurnTable_ArmX, AxisName.Stn8TurnTable_ArmY };
                            PosNames = new List<PosName>() { PosName.WaitPos };
                            MoveAbsM(Axes, PosNames);
                            step = 350;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 250;
                        }
                        break;
                    }
                case 350:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->满Tray手臂移动到待机位，满Tray手臂从半Tray缓存取半Tray流程结束", MsgType.Notify);
                            RegisterService.Instance.SetBoolReg(RegisterEnum.LoadTrayFromHalfTrayBufferToStn4Done);
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 450;
                        }
                        break;
                    }
                default:
                    break;
            }
            return step;
        }
        /// <summary>
        /// 放满Tray到8工位转盘流程
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int PutFullTrayTo8StnFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        var trayInfo = ProcInfoHelper.Instance.GetTrayInfo(TrayPosEnum.FullTrayArm);
                        if (trayInfo.TrayModel != null)
                        {
                            ShowMsg($"{StationName}->放满Tray到8工位转盘流程开始，等级：Bin{trayInfo.TrayModel.BinLevel}，结果：{(trayInfo.TrayModel.IsOkTray ? "OK" : "NG")}", MsgType.Notify);
                            MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                            Cylinder(CylinderName.FullTrayArmLiftCyl, false);
                            step = 50;
                        }
                        else
                        {
                            ShowMsg($"{StationName}->从满Tray手臂获取TrayModel信息失败", MsgType.Error, true);
                            AlarmManager.CreateAlarm(AlarmCode.GetTrayInfoFail.ToInt(), StationNum);
                        }
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        var ret2 = IsCylinderArrive(CylinderName.FullTrayArmLiftCyl, false);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->满TrayArm移动到取放Tray位置");
                            Axes = new List<AxisName>() { AxisName.Stn8TurnTable_ArmX, AxisName.Stn8TurnTable_ArmY };
                            PosNames = new List<PosName>() { PosName.FullArmPPTrayFrom8StnDDPos };
                            MoveAbsM(Axes, PosNames);
                            step = 100;
                        }
                        else if (ret == MotionStatus.Timeout || ret2 == MotionStatus.Timeout)
                        {
                            step = 1;
                        }
                        break;
                    }
                case 100:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->判断DD马达是否可以转动");
                            step = 150;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 100;
                        }
                        break;
                    }
                case 150:
                    {
                        if (IsDDCanRotate())
                        {
                            var trayInfo = ProcInfoHelper.Instance.GetTrayInfo(TrayPosEnum.FullTrayArm);
                            TrayBinLevel = (BinLevel)trayInfo.TrayModel.BinLevel;
                            if (trayInfo.TrayModel.TrayStatus == TrayStatusEnum.NgFull &&
                                trayInfo.TrayModel.BinLevel % 2 == 1)//Bin135 Ng的Tray放到Bin246位置
                            {
                                TrayBinLevel += 1;
                            }
                            ShowMsg($"{StationName}->{TrayBinLevel}平台转到放满Tray位置");
                            RotateDD(Stn8TurnTableStnEnum.PlaceFullTrayStn, (LotStageType)(TrayBinLevel.ToInt() - 1));
                            step = 200;
                        }
                        break;
                    }
                case 200:
                    {
                        var ret = IsDDArrive();
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->判断空Tray手臂或者空Tray缓存上是否有Tray");
                            if (Motion.GetDigitalInput(InputName.EmptyTrayArmIsExistTray) ||
                                Motion.GetDigitalInput(InputName.EmptyTrayBufferVacDetect))
                            {
                                ShowMsg($"{StationName}->空Tray手臂或者空Tray缓存上有Tray，判断{TrayBinLevel}平台是否有Tray");
                                ProcInfoHelper.Instance.UpdateStn8Status(Stn8TurnTableStnEnum.PlaceFullTrayStn, true);
                                if (Motion.GetDigitalInput(GetInputName(TrayBinLevel.ToInt())))
                                {
                                    ShowMsg($"{StationName}->{TrayBinLevel}平台有Tray，请求从隔离纸工位放置隔离纸");
                                    RegisterService.Instance.SetBoolReg(RegisterEnum.PlacePaperReq);
                                    step = 250;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->{TrayBinLevel}平台没有Tray，Arm气缸下降");
                                    Cylinder(CylinderName.FullTrayArmLiftCyl, true);
                                    step = 300;
                                }
                            }
                            else
                            {
                                ShowMsg($"{StationName}->空Tray手臂或者空Tray缓存上没有Tray，转盘转动到取空Tray位置");
                                PickTrayFrom8StnStep = 1;
                                step = 1200;
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 150;
                        }
                        break;
                    }
                case 250:
                    {
                        if (GetBoolReg(RegisterEnum.PlacePaperDone))
                        {
                            ShowMsg($"{StationName}->隔离纸工位放置隔离纸完成，Arm气缸下降");
                            ResetBoolReg(RegisterEnum.PlacePaperDone);
                            Cylinder(CylinderName.FullTrayArmLiftCyl, true);
                            step = 300;
                        }
                        break;
                    }
                case 300:
                    {
                        var ret = IsCylinderArrive(CylinderName.FullTrayArmLiftCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->Arm气缸下降到位，获取ArmZ轴放Tray高度");
                            var lotStageInfo = ProcInfoHelper.Instance.GetLotStageInfo(Stn8TurnTableStnEnum.PlaceFullTrayStn);
                            ArmZDestPos = GetPPTrayHeight(lotStageInfo.LotModel.TrayCount);
                            ShowMsg($"{StationName}->ArmZ轴移动到放Tray位置：{ArmZDestPos}");
                            MoveAbs(AxisName.Stn8TurnTable_ArmZ, ArmZDestPos);
                            step = 350;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 300;
                        }
                        break;
                    }
                case 350:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, ArmZDestPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴到达放Tray位置，满Tray夹爪气缸松开");
                            Cylinder(CylinderName.FullTrayArmClawCyl, false);
                            step = 400;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 250;
                        }
                        break;
                    }
                case 400:
                    {
                        var ret = IsCylinderArrive(CylinderName.FullTrayArmClawCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            OkTrayModel = ProcInfoHelper.Instance.PickTrayFromTrayStage(TrayPosEnum.FullTrayArm);
                            if (OkTrayModel != null)
                            {
                                ProcInfoHelper.Instance.PutTrayToLot(Stn8TurnTableStnEnum.PlaceFullTrayStn, OkTrayModel);
                                ShowMsg($"{StationName}->满Tray夹爪气缸松开完成，ArmZ轴移动到待机位");
                                MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                                step = 450;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.FullTrayArmIsExistTray.ToString(), out int _).IsShield = false;
                                    IOInfoHelper.GetInputInfoByName(GetInputName(TrayBinLevel.ToInt()), out int _).IsShield = true;
                                    Thread.Sleep(500);
                                }
                            }
                            else
                            {
                                ShowMsg($"{StationName}->从满Tray手臂获取TrayModel信息失败", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.GetTrayInfoFail.ToInt(), StationNum);
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 350;
                        }
                        break;
                    }
                case 450:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴移动到待机位完成，判断当前Tray平台Tray数量是否达到5个");
                            var lotStageInfo = ProcInfoHelper.Instance.GetLotStageInfo(Stn8TurnTableStnEnum.PlaceFullTrayStn);
                            if (lotStageInfo != null && lotStageInfo.LotModel != null)
                            {
                                if (lotStageInfo.LotModel.TrayCount == 5)//5个满Tray+一个空Tray组Lot
                                {
                                    ShowMsg($"{StationName}->当前Tray平台Tray数量：5个，再放一个空Tray组Lot，判断空TrayArm上是否有空Tray");
                                    if (Motion.GetDigitalInput(InputName.EmptyTrayArmIsExistTray))
                                    {
                                        ShowMsg($"{StationName}->空TrayArm上有空Tray，空TrayArm移动到取放Tray位置");
                                        Axes = new List<AxisName>() { AxisName.Stn8TurnTable_ArmX, AxisName.Stn8TurnTable_ArmY };
                                        PosNames = new List<PosName>() { PosName.EmptyArmPPTrayFrom8StnDDPos };
                                        MoveAbsM(Axes, PosNames);
                                        step = 550;
                                    }
                                    else if (Motion.GetDigitalInput(InputName.EmptyTrayBufferVacDetect))
                                    {
                                        ShowMsg($"{StationName}->空Tray缓存上有空Tray，空TrayArm移动到空Tray缓存位置");
                                        Axes = new List<AxisName>() { AxisName.Stn8TurnTable_ArmX, AxisName.Stn8TurnTable_ArmY };
                                        PosNames = new List<PosName>() { PosName.PPTrayFromEmptyTrayBufferPos };
                                        Cylinder(CylinderName.EmptyTrayArmClawCyl, false);
                                        MoveAbsM(Axes, PosNames);
                                        step = 800;
                                    }
                                    else
                                    {
                                        ShowMsg($"{StationName}->转盘转动到取空Tray位置");
                                        AlarmManager.CreateAlarm(AlarmCode.EmptyTrayArmNoExistTray.ToInt(), StationNum);
                                    }
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->当前Tray平台Tray数量：{lotStageInfo.LotModel.TrayCount}个，模组移动到待机位置");
                                    Axes = new List<AxisName>() { AxisName.Stn8TurnTable_ArmX, AxisName.Stn8TurnTable_ArmY };
                                    PosNames = new List<PosName>() { PosName.WaitPos };
                                    MoveAbsM(Axes, PosNames);
                                    step = 500;
                                }
                            }
                            else
                            {
                                ShowMsg($"{StationName}->获取Tray平台信息失败", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.GetLotModelFail.ToInt(), StationNum);
                                step = -1;
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 400;
                        }
                        break;
                    }
                case 500:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移动到待机位置，放满Tray到8工位转盘流程结束", MsgType.Notify);
                            ProcInfoHelper.Instance.UpdateStn8Status(Stn8TurnTableStnEnum.PlaceFullTrayStn, false);
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 450;
                        }
                        break;
                    }
                case 550:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->TrayArm到达叠Tray位置，空TrayArm气缸下降");
                            Cylinder(CylinderName.EmptyTrayArmLiftCyl, true);
                            step = 600;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 450;
                        }
                        break;
                    }
                case 600:
                    {
                        var ret = IsCylinderArrive(CylinderName.EmptyTrayArmLiftCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->空TrayArm气缸下降完成，获取TrayArm下降高度");
                            var lotStageInfo = ProcInfoHelper.Instance.GetLotStageInfo(Stn8TurnTableStnEnum.PlaceFullTrayStn);
                            if (lotStageInfo != null && lotStageInfo.LotModel != null)
                            {
                                ArmZDestPos = GetPPTrayHeight(lotStageInfo.LotModel.TrayCount);
                                ShowMsg($"{StationName}->ArmZ轴移动到放Tray位置：{ArmZDestPos}");
                                MoveAbs(AxisName.Stn8TurnTable_ArmZ, ArmZDestPos);
                                step = 650;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->获取Tray平台信息失败", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.GetLotModelFail.ToInt(), StationNum);
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 550;
                        }
                        break;
                    }
                case 650:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, ArmZDestPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴到达放Tray位置，空Tray夹爪气缸松开");
                            Cylinder(CylinderName.EmptyTrayArmClawCyl, false);
                            step = 700;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 600;
                        }
                        break;
                    }
                case 700:
                    {
                        var ret = IsCylinderArrive(CylinderName.EmptyTrayArmClawCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            var emptyModel = ProcInfoHelper.Instance.PickTrayFromTrayStage(TrayPosEnum.EmptyTrayArm);
                            ProcInfoHelper.Instance.PutTrayToLot(Stn8TurnTableStnEnum.PlaceFullTrayStn, emptyModel);
                            ShowMsg($"{StationName}->空Tray夹爪气缸松开完成，ArmZ轴移动到待机位");
                            MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                            Cylinder(CylinderName.EmptyTrayArmLiftCyl, false);
                            if (Global.RunMode == RunMode.空跑模式)
                            {
                                IOInfoHelper.GetInputInfoByName(InputName.EmptyTrayArmIsExistTray.ToString(), out int _).IsShield = false;
                                Thread.Sleep(500);
                            }
                            step = 750;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 650;
                        }
                        break;
                    }
                case 750:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        var ret2 = IsCylinderArrive(CylinderName.EmptyTrayArmLiftCyl, false);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴移动到待机位完成，模组移动到待机位置");
                            Axes = new List<AxisName>() { AxisName.Stn8TurnTable_ArmX, AxisName.Stn8TurnTable_ArmY };
                            PosNames = new List<PosName>() { PosName.WaitPos };
                            MoveAbsM(Axes, PosNames);
                            step = 1650;
                        }
                        else if (ret == MotionStatus.Timeout || ret2 == MotionStatus.Timeout)
                        {
                            step = 700;
                        }
                        break;
                    }
                case 800:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        var ret2 = IsCylinderArrive(CylinderName.EmptyTrayArmClawCyl, false);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->TrayArm到达空Tray缓存位置，空TrayArm气缸下降");
                            Cylinder(CylinderName.EmptyTrayArmLiftCyl, true);
                            step = 850;
                        }
                        else if (ret == MotionStatus.Timeout || ret2 == MotionStatus.Timeout)
                        {
                            step = 450;
                        }
                        break;
                    }
                case 850:
                    {
                        var ret = IsCylinderArrive(CylinderName.EmptyTrayArmLiftCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->空TrayArm气缸下降完成，ArmZ轴下降到取Tray位置");
                            MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.PPTrayFromEmptyTrayBufferPos);
                            step = 900;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 800;
                        }
                        break;
                    }
                case 900:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.PPTrayFromEmptyTrayBufferPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴下降到取Tray位置，空Tray夹爪气缸夹紧");
                            Cylinder(CylinderName.EmptyTrayArmClawCyl, true);
                            step = 950;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 850;
                        }
                        break;
                    }
                case 950:
                    {
                        var ret = IsCylinderArrive(CylinderName.EmptyTrayArmClawCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->空Tray夹爪气缸夹紧完成，空TrayArm气缸抬起");
                            Cylinder(CylinderName.EmptyTrayArmLiftCyl, false);
                            step = 1000;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 900;
                        }
                        break;
                    }
                case 1000:
                    {
                        var ret = IsCylinderArrive(CylinderName.EmptyTrayArmLiftCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            var trayModel = ProcInfoHelper.Instance.PickTrayFromTrayStage(TrayPosEnum.EmptyTrayBufferStage);
                            ProcInfoHelper.Instance.PutTrayToTrayStage(TrayPosEnum.EmptyTrayArm, trayModel);
                            ShowMsg($"{StationName}->空TrayArm气缸抬起完成，空TrayArm移动到取放Tray位置");
                            Axes = new List<AxisName>() { AxisName.Stn8TurnTable_ArmX, AxisName.Stn8TurnTable_ArmY };
                            PosNames = new List<PosName>() { PosName.EmptyArmPPTrayFrom8StnDDPos };
                            MoveAbsM(Axes, PosNames);
                            step = 550;

                            if (Global.RunMode == RunMode.空跑模式)
                            {
                                IOInfoHelper.GetInputInfoByName(InputName.EmptyTrayBufferVacDetect.ToString(), out int _).IsShield = false;
                                Thread.Sleep(500);
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 950;
                        }
                        break;
                    }
                case 1200:
                    {
                        PickTrayFrom8StnStep = PickEmptyTrayFrom8StnFlow(PickTrayFrom8StnStep, false);
                        if (PickTrayFrom8StnStep == 0)
                        {
                            step = 200;
                        }
                        break;
                    }

                case 1650:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移动到待机位置，组Lot完成，判断转盘是否可以转动");
                            step = 1700;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 450;
                        }
                        break;
                    }
                case 1700:
                    {
                        if (IsDDCanRotate())
                        {
                            ShowMsg($"{StationName}->转盘转动到下Lot位置，等级：{TrayBinLevel}");
                            RotateDD(Stn8TurnTableStnEnum.LulLot, (LotStageType)(TrayBinLevel.ToInt() - 1));
                            step = 1750;
                        }
                        break;
                    }
                case 1750:
                    {
                        var ret = IsDDArrive();
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->转盘到达下Lot位置，发送从从8工位转盘取Lot请求给LotBox手臂");
                            RegisterService.Instance.SetBoolReg(RegisterEnum.Stn8TurnTableUnloadFullLotReq);
                            step = 1800;

                            if (Global.RunMode == RunMode.空跑模式)
                            {
                                IOInfoHelper.GetInputInfoByName(GetInputName(OkTrayModel.BinLevel), out int _).IsShield = false;
                                Thread.Sleep(500);
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1700;
                        }
                        break;
                    }
                case 1800:
                    {
                        if (GetBoolReg(RegisterEnum.LotBoxStnUnloadFullLotDone))
                        {
                            ResetBoolReg(RegisterEnum.LotBoxStnUnloadFullLotDone);
                            ShowMsg($"{StationName}->LotBox手臂从Stn8TurnTable工位取Lot完成", MsgType.Notify);
                            ProcInfoHelper.Instance.UpdateStn8Status(Stn8TurnTableStnEnum.LulLot, false);
                            step = 0;
                        }
                        break;
                    }
                default:
                    break;
            }
            return step;
        }
        /// <summary>
        /// 放空Tray到8工位转盘流程
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int PutEmptyTrayTo8StnFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->放空Tray到8工位转盘流程开始", MsgType.Notify);
                        MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        Cylinder(CylinderName.FullTrayArmLiftCyl, false);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        var ret2 = IsCylinderArrive(CylinderName.FullTrayArmLiftCyl, false);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->满TrayArm移动到取放Tray位置");
                            Axes = new List<AxisName>() { AxisName.Stn8TurnTable_ArmX, AxisName.Stn8TurnTable_ArmY };
                            PosNames = new List<PosName>() { PosName.FullArmPPTrayFrom8StnDDPos };
                            MoveAbsM(Axes, PosNames);
                            step = 100;
                        }
                        else if (ret == MotionStatus.Timeout || ret2 == MotionStatus.Timeout)
                        {
                            step = 1;
                        }
                        break;
                    }
                case 100:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->判断DD马达是否可以转动");
                            step = 150;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 100;
                        }
                        break;
                    }
                case 150:
                    {
                        if (IsDDCanRotate())
                        {
                            ShowMsg($"{StationName}->平台转到取放空Tray位置");
                            RotateDD(Stn8TurnTableStnEnum.PickEmptyTrayStn, LotStageType.LulLotStage);
                            step = 200;
                        }
                        break;
                    }
                case 200:
                    {
                        var ret = IsDDArrive();
                        if (ret == MotionStatus.Arrived)
                        {
                            ProcInfoHelper.Instance.UpdateStn8Status(Stn8TurnTableStnEnum.PickEmptyTrayStn, true);
                            ShowMsg($"{StationName}->转盘转动到取放空Tray位置，Arm气缸下降");
                            Cylinder(CylinderName.FullTrayArmLiftCyl, true);
                            step = 300;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 150;
                        }
                        break;
                    }
                case 300:
                    {
                        var ret = IsCylinderArrive(CylinderName.FullTrayArmLiftCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->Arm气缸下降到位，获取ArmZ轴放Tray高度");
                            var lotStageInfo = ProcInfoHelper.Instance.GetLotStageInfo(Stn8TurnTableStnEnum.PickEmptyTrayStn);
                            ArmZDestPos = GetPPTrayHeight(lotStageInfo.LotModel.TrayCount);
                            ShowMsg($"{StationName}->ArmZ轴移动到放Tray位置");
                            MoveAbs(AxisName.Stn8TurnTable_ArmZ, ArmZDestPos);
                            step = 350;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 300;
                        }
                        break;
                    }
                case 350:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, ArmZDestPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴到达放Tray位置，满Tray夹爪气缸松开");
                            Cylinder(CylinderName.FullTrayArmClawCyl, false);
                            step = 400;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 250;
                        }
                        break;
                    }
                case 400:
                    {
                        var ret = IsCylinderArrive(CylinderName.FullTrayArmClawCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            var trayModel = ProcInfoHelper.Instance.PickTrayFromTrayStage(TrayPosEnum.FullTrayArm);
                            ProcInfoHelper.Instance.PutTrayToLot(Stn8TurnTableStnEnum.PickEmptyTrayStn, trayModel);
                            ShowMsg($"{StationName}->满Tray夹爪气缸松开完成，ArmZ轴移动到待机位");
                            MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                            step = 450;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 350;
                        }
                        break;
                    }
                case 450:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴移动到待机位完成");
                            Axes = new List<AxisName>() { AxisName.Stn8TurnTable_ArmX, AxisName.Stn8TurnTable_ArmY };
                            PosNames = new List<PosName>() { PosName.WaitPos };
                            MoveAbsM(Axes, PosNames);
                            step = 500;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 400;
                        }
                        break;
                    }
                case 500:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移动到待机位置，放空Tray到8工位转盘流程结束", MsgType.Notify);
                            ProcInfoHelper.Instance.UpdateStn8Status(Stn8TurnTableStnEnum.PickEmptyTrayStn, false);
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 450;
                        }
                        break;
                    }

                default:
                    break;
            }
            return step;
        }
        /// <summary>
        /// 放Tray到4工位转盘
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int PutEmptyTrayTo4StnFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->把空Tray放到4工位转盘流程开始", MsgType.Notify);
                        MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->{AxisName.Stn8TurnTable_ArmZ}轴到达待机位");
                            Delay.Start(ParaHelper.Motion.WaitAxisMoveTimeOut);
                            ShowMsg($"{StationName}->等待{AxisName.PickPatch_CamY}轴移动{PosName.WaitPos}位");
                            step = 100;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1;
                        }
                        break;
                    }
                case 100:
                    {
                        if (Motion.IsArrive(AxisName.PickPatch_CamY, PosName.WaitPos))
                        {
                            ShowMsg($"{StationName}->{AxisName.PickPatch_CamY}轴到达{PosName.WaitPos}位");
                            Axes = new List<AxisName>() { AxisName.Stn8TurnTable_ArmX, AxisName.Stn8TurnTable_ArmY };
                            PosNames = new List<PosName>() { PosName.EmptyArmPPTrayFrom4StnDDPos };
                            MoveAbsM(Axes, PosNames);
                            ShowMsg($"{StationName}->{AxisName.Stn8TurnTable_ArmX}轴和{AxisName.Stn8TurnTable_ArmY}轴移动到4工位转盘取放Tray位置");
                            step = 150;
                        }
                        else if (Delay.TimeUp())
                        {
                            ShowMsg($"{StationName}->等待{AxisName.PickPatch_CamY}轴移动{PosName.WaitPos}位超时");
                            step = -1;
                        }
                        break;
                    }
                case 150:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移到达4工位转盘取放Tray位置，夹爪下降到放Tray位置");
                            Cylinder(CylinderName.EmptyTrayArmLiftCyl, true);
                            step = 200;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 100;
                        }
                        break;
                    }
                case 200:
                    {
                        var ret = IsCylinderArrive(CylinderName.EmptyTrayArmLiftCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪下降到放Tray位置，ArmZ轴");
                            MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.EmptyArmPPTrayFrom4StnDDPos);
                            step = 250;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 150;
                        }
                        break;
                    }
                case 250:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.EmptyArmPPTrayFrom4StnDDPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴到达放Tray位置，夹爪松开Tray");
                            Cylinder(CylinderName.EmptyTrayArmClawCyl, false);
                            step = 300;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 200;
                        }
                        break;
                    }
                case 300:
                    {
                        var ret = IsCylinderArrive(CylinderName.EmptyTrayArmClawCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪松开Tray，抬起Tray");
                            Cylinder(CylinderName.EmptyTrayArmLiftCyl, false);
                            step = 350;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 300;
                        }
                        break;
                    }
                case 350:
                    {
                        var ret = IsCylinderArrive(CylinderName.EmptyTrayArmLiftCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪抬起Tray，模组移动到待机位");
                            MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                            step = 400;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 300;
                        }
                        break;
                    }
                case 400:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->把空Tray放到4工位转盘取放Tray平台流程结束", MsgType.Notify);
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 250;
                        }
                        break;
                    }
                default:
                    break;
            }
            return step;
        }
        /// <summary>
        /// 从4工位转盘取满Tray流程
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int PickFullTrayFrom4StnFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        if (Motion.GetDigitalInput(InputName.FullTrayArmIsExistTray) == false)
                        {
                            var trayInfo = ProcInfoHelper.Instance.GetTrayInfo(Stn4TurnTableStnEnum.LuloadTrayStn);
                            if (trayInfo != null && trayInfo.TrayModel != null)
                            {
                                ShowMsg($"{StationName}->满Tray手臂从4工位转盘取Tray流程开始，等级：Bin{trayInfo.TrayModel.BinLevel}", MsgType.Notify);
                                MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                                CylinderNames = new List<CylinderName>() { CylinderName.FullTrayArmLiftCyl, CylinderName.FullTrayArmClawCyl };
                                CylinderM(CylinderNames, false);
                                step = 50;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->从4工位转盘上下Tray位置获取TrayModel信息失败", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.GetTrayInfoFail.ToInt(), StationNum);
                            }
                        }
                        else
                        {
                            ShowMsg($"{StationName}->满TrayArm存在Tray，无法执行从4工位转盘取Tray流程", MsgType.Error, true);
                            AlarmManager.CreateAlarm(AlarmCode.FullTrayArmExistTray.ToInt(), StationNum);
                        }
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        var ret2 = IsCylinderArriveM(CylinderNames, false);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移动到取放Tray位置");
                            Axes = new List<AxisName>() { AxisName.Stn8TurnTable_ArmX, AxisName.Stn8TurnTable_ArmY };
                            PosNames = new List<PosName>() { PosName.FullArmPPTrayFrom4StnDDPos };
                            MoveAbsM(Axes, PosNames);
                            step = 100;
                        }
                        else if (ret == MotionStatus.Timeout || ret2 == MotionStatus.Timeout)
                        {
                            step = 1;
                        }
                        break;
                    }
                case 100:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            CylinderNames = new List<CylinderName>() { CylinderName.FullTrayArmLiftCyl };
                            CylinderM(CylinderNames, true);
                            step = 150;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 100;
                        }
                        break;
                    }
                case 150:
                    {
                        var ret = IsCylinderArriveM(CylinderNames, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->空Tray夹爪下降到取Tray位置");
                            MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.FullArmPPTrayFrom4StnDDPos);
                            step = 200;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 100;
                        }
                        break;
                    }
                case 200:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.FullArmPPTrayFrom4StnDDPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->空Tray夹爪下降到取Tray位置，夹爪夹紧Tray");
                            CylinderNames = new List<CylinderName>() { CylinderName.FullTrayArmClawCyl };
                            CylinderM(CylinderNames, true);
                            step = 250;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 150;
                        }
                        break;
                    }
                case 250:
                    {
                        var ret = IsCylinderArriveM(CylinderNames, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->空Tray夹爪夹紧Tray，抬起Tray");
                            CylinderNames = new List<CylinderName>() { CylinderName.FullTrayArmLiftCyl };
                            CylinderM(CylinderNames, false);
                            step = 300;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 200;
                        }
                        break;
                    }
                case 300:
                    {
                        var ret = IsCylinderArriveM(CylinderNames, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->空Tray夹爪抬起Tray，模组移动到待机位");
                            MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                            step = 350;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 250;
                        }
                        break;
                    }
                case 350:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            var trayModel = ProcInfoHelper.Instance.PickTrayFromTrayStage(Stn4TurnTableStnEnum.LuloadTrayStn);
                            if (trayModel != null)
                            {
                                ShowMsg($"{StationName}->满Tray手臂从4工位转盘取Tray流程结束，等级：Bin{trayModel.BinLevel}", MsgType.Notify);
                                step = 0;
                                ProcInfoHelper.Instance.PutTrayToTrayStage(TrayPosEnum.FullTrayArm, trayModel);
                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.FullTrayArmIsExistTray.ToString(), out int _).IsShield = true;
                                    Thread.Sleep(500);
                                }
                            }
                            else
                            {
                                ShowMsg($"{StationName}->从4工位转盘上下Tray位置获取TrayModel信息失败", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.GetTrayInfoFail.ToInt(), StationNum);
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 300;
                        }
                        break;
                    }
            }
            return step;
        }
        /// <summary>
        /// 满Tray手臂从挑补平台取Tray流程。无论是取空Tray还是满Tray都使用满TrayArm
        /// </summary>
        /// <param name="step"></param>
        /// <param name="isFullTray">true：Ng满Tray。false：空Tray</param>
        /// <returns></returns>
        public int PickTrayFromHalfTrayStageFlow(int step, bool isFullTray)
        {
            switch (step)
            {
                case 1:
                    {
                        if (Motion.GetDigitalInput(InputName.FullTrayArmIsExistTray) == false)
                        {
                            ShowMsg($"{StationName}->满Tray手臂从挑补平台取{(isFullTray ? "满" : "空")}Tray流程开始", MsgType.Notify);
                            MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                            CylinderNames = new List<CylinderName>() { CylinderName.FullTrayArmLiftCyl, CylinderName.FullTrayArmClawCyl };
                            CylinderM(CylinderNames, false);
                            step = 50;
                        }
                        else
                        {
                            //  step = -1;
                            ShowMsg($"{StationName}->满TrayArm存在Tray，无法执行从挑补平台取{(isFullTray ? "满" : "空")}Tray流程", MsgType.Error, true);
                            AlarmManager.CreateAlarm(AlarmCode.FullTrayArmExistTray.ToInt(), StationNum);
                        }

                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        var ret2 = IsCylinderArriveM(CylinderNames, false);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移动到取放Tray位置");
                            Axes = new List<AxisName>() { AxisName.Stn8TurnTable_ArmX, AxisName.Stn8TurnTable_ArmY };
                            PosNames = new List<PosName>() { PosName.FullArmPPTrayFromHalfTrayStagePos };
                            MoveAbsM(Axes, PosNames);
                            step = 100;
                        }
                        else if (ret == MotionStatus.Timeout || ret2 == MotionStatus.Timeout)
                        {
                            step = 1;
                        }
                        break;
                    }
                case 100:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->满Tray夹爪下降到取Tray位置");
                            CylinderNames = new List<CylinderName>() { CylinderName.FullTrayArmLiftCyl };
                            CylinderM(CylinderNames, true);
                            step = 150;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 100;
                        }
                        break;
                    }
                case 150:
                    {
                        var ret = IsCylinderArriveM(CylinderNames, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴下降到取Tray位置");
                            MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.FullArmPPTrayFromHalfTrayStagePos);
                            step = 200;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 100;
                        }
                        break;
                    }
                case 200:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.FullArmPPTrayFromHalfTrayStagePos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴到达取Tray位置，夹爪夹紧Tray");
                            CylinderNames = new List<CylinderName>() { CylinderName.FullTrayArmClawCyl };
                            CylinderM(CylinderNames, true);
                            step = 250;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 150;
                        }
                        break;
                    }
                case 250:
                    {
                        var ret = IsCylinderArriveM(CylinderNames, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->满Tray夹爪夹紧Tray，抬起Tray");
                            CylinderNames = new List<CylinderName>() { CylinderName.FullTrayArmLiftCyl };
                            CylinderM(CylinderNames, false);
                            step = 300;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 200;
                        }
                        break;
                    }
                case 300:
                    {
                        var ret = IsCylinderArriveM(CylinderNames, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            var trayModel = ProcInfoHelper.Instance.PickTrayFromTrayStage(isFullTray ? TrayPosEnum.NgHalfTrayStage : TrayPosEnum.OkHalfTrayStage);
                            if (trayModel != null)
                            {
                                if (isFullTray == false)//如果是下空Tray，把IsOkTray改为false
                                {
                                    trayModel.IsOkTray = false;
                                }
                                ProcInfoHelper.Instance.PutTrayToTrayStage(TrayPosEnum.FullTrayArm, trayModel);
                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.FullTrayArmIsExistTray.ToString(), out int _).IsShield = true;
                                    Thread.Sleep(500);
                                }
                            }
                            ShowMsg($"{StationName}->满Tray夹爪抬起Tray，模组移动到待机位");
                            MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                            step = 350;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 250;
                        }
                        break;
                    }
                case 350:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->满Tray手臂从挑补平台取{(isFullTray ? "满" : "空")}Tray流程结束", MsgType.Notify);
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 300;
                        }
                        break;
                    }
            }
            return step;
        }

        /// <summary>
        /// 满Tray手臂放Tray放到HalfTray平台流程，无论是放空Tray还是满Tray都使用满TrayArm
        /// </summary>
        /// <param name="step"></param>
        /// <param name="isEmptyTray">true：空Tray，false：满Tray或者半Tray</param>
        /// <returns></returns>
        public int PutTrayToHalfTrayStageFlow(int step, bool isEmptyTray)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->满Tray手臂放{(isEmptyTray ? "空" : "满")}Tray到HalfTray平台流程开始", MsgType.Notify);
                        MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移动到HalfTray平台位置");
                            Axes = new List<AxisName>() { AxisName.Stn8TurnTable_ArmX, AxisName.Stn8TurnTable_ArmY };
                            PosNames = new List<PosName>() { PosName.FullArmPPTrayFromHalfTrayStagePos };
                            MoveAbsM(Axes, PosNames);
                            step = 100;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1;
                        }
                        break;
                    }
                case 100:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移到达HalfTray平台位置，夹爪下降到放Tray位置");
                            Cylinder(CylinderName.FullTrayArmLiftCyl, true);
                            step = 150;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 50;
                        }
                        break;
                    }
                case 150:
                    {
                        var ret = IsCylinderArrive(CylinderName.FullTrayArmLiftCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪下降到放Tray位置，夹爪松开Tray");
                            Cylinder(CylinderName.FullTrayArmClawCyl, false);
                            step = 200;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 100;
                        }
                        break;
                    }
                case 200:
                    {
                        var ret = IsCylinderArrive(CylinderName.FullTrayArmClawCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪松开Tray，抬起Tray");
                            Cylinder(CylinderName.FullTrayArmLiftCyl, false);
                            step = 250;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 150;
                        }
                        break;
                    }
                case 250:
                    {
                        var ret = IsCylinderArrive(CylinderName.FullTrayArmLiftCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            var trayModel = ProcInfoHelper.Instance.PickTrayFromTrayStage(TrayPosEnum.FullTrayArm);
                            if (trayModel != null)
                            {
                                ProcInfoHelper.Instance.PutTrayToTrayStage(isEmptyTray ? TrayPosEnum.NgHalfTrayStage : TrayPosEnum.OkHalfTrayStage, trayModel);
                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.FullTrayArmIsExistTray.ToString(), out int _).IsShield = false;
                                    Thread.Sleep(500);
                                }
                            }
                            ShowMsg($"{StationName}->夹爪抬起Tray，模组移动到待机位");
                            MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                            step = 300;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 200;
                        }
                        break;
                    }
                case 300:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->满Tray手臂放{(isEmptyTray ? "空" : "满")}Tray到HalfTray平台流程结束", MsgType.Notify);
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 250;
                        }
                        break;
                    }
                default:
                    break;
            }
            return step;
        }
        /// <summary>
        /// 放空Tray到缓存平台流程
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int PutTrayToEmptyTrayBufferFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->放空Tray到缓存平台流程开始", MsgType.Notify);
                        MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        Cylinder(CylinderName.EmptyTrayArmLiftCyl, false);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        var ret2 = IsCylinderArrive(CylinderName.EmptyTrayArmLiftCyl, false);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移动到放空Tray位置");
                            Axes = new List<AxisName>() { AxisName.Stn8TurnTable_ArmX, AxisName.Stn8TurnTable_ArmY };
                            PosNames = new List<PosName>() { PosName.PPTrayFromEmptyTrayBufferPos };
                            MoveAbsM(Axes, PosNames);
                            step = 100;
                        }
                        else if (ret == MotionStatus.Timeout || ret2 == MotionStatus.Timeout)
                        {
                            step = 1;
                        }
                        break;
                    }
                case 100:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移到达放空Tray位置");
                            Cylinder(CylinderName.EmptyTrayArmLiftCyl, true);
                            step = 150;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 50;
                        }
                        break;
                    }
                case 150:
                    {
                        var ret = IsCylinderArrive(CylinderName.EmptyTrayArmLiftCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->空Tray夹爪下降到放Tray位置");
                            MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.PPTrayFromEmptyTrayBufferPos);
                            step = 200;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 100;
                        }
                        break;
                    }
                case 200:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.PPTrayFromEmptyTrayBufferPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->空Tray夹爪下降到放Tray位置，夹爪松开Tray");
                            Cylinder(CylinderName.EmptyTrayArmClawCyl, false);
                            step = 250;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 150;
                        }
                        break;
                    }
                case 250:
                    {
                        var ret = IsCylinderArrive(CylinderName.EmptyTrayArmClawCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->空Tray夹爪松开Tray，抬起Tray");
                            Cylinder(CylinderName.EmptyTrayArmLiftCyl, false);
                            step = 300;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 200;
                        }
                        break;
                    }
                case 300:
                    {
                        var ret = IsCylinderArrive(CylinderName.EmptyTrayArmLiftCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            var trayModel = ProcInfoHelper.Instance.PickTrayFromTrayStage(TrayPosEnum.EmptyTrayArm);
                            if (trayModel != null)
                            {
                                ProcInfoHelper.Instance.PutTrayToTrayStage(TrayPosEnum.EmptyTrayBufferStage, trayModel);
                            }
                            ShowMsg($"{StationName}->空Tray夹爪抬起Tray，模组移动到待机位");
                            MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                            step = 350;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 250;
                        }
                        break;
                    }
                case 350:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            Axes = new List<AxisName>() { AxisName.Stn8TurnTable_ArmX, AxisName.Stn8TurnTable_ArmY };
                            PosNames = new List<PosName>() { PosName.WaitPos };
                            MoveAbsM(Axes, PosNames);
                            step = 400;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 300;
                        }
                        break;
                    }
                case 400:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移动到待机位，放空Tray到缓存平台流程结束", MsgType.Notify);
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 350;
                        }
                        break;
                    }
                default:
                    break;
            }
            return step;
        }
        /// <summary>
        /// 轴移动到待机位
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int MoveToWaitPosFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->回待机位流程开始", MsgType.Notify);
                        MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        Cylinder(CylinderName.EmptyTrayArmLiftCyl, false);
                        Cylinder(CylinderName.FullTrayArmLiftCyl, false);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        var ret2 = IsCylinderArrive(CylinderName.EmptyTrayArmLiftCyl, false);
                        var ret3 = IsCylinderArrive(CylinderName.FullTrayArmLiftCyl, false);
                        if (ret == MotionStatus.Arrived &&
                            ret2 == MotionStatus.Arrived &&
                            ret3 == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->Stn8TurnTable_ArmX和Y轴移动到待机位");
                            Axes = new List<AxisName>() { AxisName.Stn8TurnTable_ArmX, AxisName.Stn8TurnTable_ArmY, AxisName.Stn8TurnTable_Shift };
                            PosNames = new List<PosName>() { PosName.WaitPos };
                            MoveAbsM(Axes, PosNames);
                            step = 100;
                        }
                        else if (ret == MotionStatus.Timeout || ret2 == MotionStatus.Timeout || ret3 == MotionStatus.Timeout)
                        {
                            step = 1;
                        }
                        break;
                    }
                case 100:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->Stn8TurnTable_DD轴移动到{LotTrayDDPos}");
                            MoveAbs(AxisName.Stn8TurnTable_DD, LotTrayDDPos);
                            step = 150;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 50;
                        }
                        break;
                    }
                case 150:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_DD, LotTrayDDPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->回待机位流程结束", MsgType.Notify);
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 100;
                        }
                        break;
                    }
                default:
                    break;
            }
            return step;
        }

        /// <summary>
        /// 从8工位转盘取空Tray流程
        /// </summary>
        /// <param name="step"></param>
        /// <param name="isUseFullTrayArm">是否使用满Tray夹爪</param>
        /// <returns></returns>
        public int PickEmptyTrayFrom8StnFlow(int step, bool isUseFullTrayArm)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->从8工位转盘取空Tray流程开始", MsgType.Notify);
                        if (isUseFullTrayArm)
                        {
                            if (Motion.GetDigitalInput(InputName.FullTrayArmIsExistTray))
                            {
                                ShowMsg($"{StationName}->满TrayArm存在Tray，无法执行从8工位转盘取空Tray流程", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.FullTrayArmExistTray.ToInt(), StationNum);
                                return 0;
                            }
                            CylinderNames = new List<CylinderName>() { CylinderName.FullTrayArmLiftCyl, CylinderName.FullTrayArmClawCyl };
                        }
                        else
                        {
                            if (Motion.GetDigitalInput(InputName.EmptyTrayArmIsExistTray))
                            {
                                ShowMsg($"{StationName}->空TrayArm存在Tray，无法执行从8工位转盘取空Tray流程", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.EmptyTrayArmExistTray.ToInt(), StationNum);
                                return 0;
                            }
                            CylinderNames = new List<CylinderName>() { CylinderName.EmptyTrayArmLiftCyl, CylinderName.EmptyTrayArmClawCyl };
                        }
                        MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        CylinderM(CylinderNames, false);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        var ret2 = IsCylinderArriveM(CylinderNames, false);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移动到取放Tray位置");
                            Axes = new List<AxisName>() { AxisName.Stn8TurnTable_ArmX, AxisName.Stn8TurnTable_ArmY };
                            if (isUseFullTrayArm)
                            {
                                PosNames = new List<PosName>() { PosName.FullArmPPTrayFrom8StnDDPos };
                            }
                            else
                            {
                                PosNames = new List<PosName>() { PosName.EmptyArmPPTrayFrom8StnDDPos };
                            }
                            MoveAbsM(Axes, PosNames);
                            step = 100;
                        }
                        else if (ret == MotionStatus.Timeout || ret2 == MotionStatus.Timeout)
                        {
                            step = 1;
                        }
                        break;
                    }
                case 100:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->判断是否可以转动转盘");
                            step = 150;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 100;
                        }
                        break;
                    }
                case 150:
                    {
                        if (IsDDCanRotate())
                        {
                            RotateDD(Stn8TurnTableStnEnum.PickEmptyTrayStn, LotStageType.LulLotStage);
                            step = 200;
                        }
                        break;
                    }
                case 200:
                    {
                        var ret = IsDDArrive();
                        if (ret == MotionStatus.Arrived)
                        {
                            ProcInfoHelper.Instance.UpdateStn8Status(Stn8TurnTableStnEnum.PickEmptyTrayStn, true);
                            CylinderNames = new List<CylinderName>() { CylinderName.EmptyTrayArmLiftCyl };
                            if (isUseFullTrayArm)
                            {
                                CylinderNames = new List<CylinderName>() { CylinderName.FullTrayArmLiftCyl };
                            }
                            CylinderM(CylinderNames, true);
                            step = 250;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 150;
                        }
                        break;
                    }
                case 250:
                    {
                        var ret = IsCylinderArriveM(CylinderNames, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->{(isUseFullTrayArm ? "满" : "空")}Tray夹爪下降到取Tray位置");
                            var lotStageInfo = ProcInfoHelper.Instance.GetLotStageInfo(Stn8TurnTableStnEnum.PickEmptyTrayStn);
                            if (lotStageInfo.LotModel != null)
                            {
                                ArmZDestPos = GetPPTrayHeight(lotStageInfo.LotModel.TrayCount);
                                ShowMsg($"{StationName}->ArmZ轴移动到放Tray位置：{ArmZDestPos}");
                                MoveAbs(AxisName.Stn8TurnTable_ArmZ, ArmZDestPos);
                                step = 300;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->从8工位转盘取Tray失败，LotModel信息为空", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.GetLotModelFail.ToInt(), StationNum);
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 200;
                        }
                        break;
                    }
                case 300:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, ArmZDestPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->{(isUseFullTrayArm ? "满" : "空")}Tray夹爪下降到取Tray位置，夹爪夹紧Tray");
                            CylinderNames = new List<CylinderName>() { CylinderName.EmptyTrayArmClawCyl };
                            if (isUseFullTrayArm)
                            {
                                CylinderNames = new List<CylinderName>() { CylinderName.FullTrayArmClawCyl };
                            }
                            CylinderM(CylinderNames, true);
                            step = 350;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 250;
                        }
                        break;
                    }
                case 350:
                    {
                        var ret = IsCylinderArriveM(CylinderNames, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->{(isUseFullTrayArm ? "满" : "空")}Tray夹爪夹紧Tray，抬起Tray");
                            CylinderNames = new List<CylinderName>() { CylinderName.EmptyTrayArmLiftCyl };
                            if (isUseFullTrayArm)
                            {
                                CylinderNames = new List<CylinderName>() { CylinderName.FullTrayArmLiftCyl };
                            }
                            CylinderM(CylinderNames, false);
                            step = 400;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 300;
                        }
                        break;
                    }
                case 400:
                    {
                        var ret = IsCylinderArriveM(CylinderNames, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            IsLulTrayStageEmpty = false;
                            var trayModel = ProcInfoHelper.Instance.PickTrayFromLot(Stn8TurnTableStnEnum.PickEmptyTrayStn, ref IsLulTrayStageEmpty);
                            if (trayModel != null)
                            {
                                ProcInfoHelper.Instance.PutTrayToTrayStage(isUseFullTrayArm ? TrayPosEnum.FullTrayArm : TrayPosEnum.EmptyTrayArm, trayModel);
                                ShowMsg($"{StationName}->{(isUseFullTrayArm ? "满" : "空")}Tray手臂移动到待机位");
                            }
                            else
                            {
                                ShowMsg($"{StationName}->从8工位转盘取Tray失败", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.GetTrayInfoFail.ToInt(), StationNum);
                            }
                            if (IsLulTrayStageEmpty && Global.RunMode == RunMode.空跑模式)
                            {
                                IOInfoHelper.GetInputInfoByName(InputName.Stn8LulStageDetect.ToString(), out int _).IsShield = false;
                                Thread.Sleep(500);
                            }
                            MoveAbs(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                            step = 450;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 350;
                        }
                        break;
                    }

                case 450:
                    {
                        var ret = IsAxisArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->{AxisName.Stn8TurnTable_ArmZ}轴到达待机位");
                            Axes = new List<AxisName>() { AxisName.Stn8TurnTable_ArmX, AxisName.Stn8TurnTable_ArmY };
                            PosNames = new List<PosName>() { PosName.WaitPos };
                            MoveAbsM(Axes, PosNames);
                            ShowMsg($"{StationName}->{AxisName.Stn8TurnTable_ArmX}轴和{AxisName.Stn8TurnTable_ArmY}轴移动到待机位");
                            step = 500;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 400;
                        }
                        break;
                    }
                case 500:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->{AxisName.Stn8TurnTable_ArmX}轴和{AxisName.Stn8TurnTable_ArmY}轴到达待机位");
                            ProcInfoHelper.Instance.UpdateStn8Status(Stn8TurnTableStnEnum.PickEmptyTrayStn, false);
                            ShowMsg($"{StationName}->从8工位转盘取空Tray流程结束", MsgType.Notify);
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 450;
                        }
                        break;
                    }
            }
            return step;
        }
        /// <summary>
        /// 上空Lot流程
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int LoadLotFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->上空Lot流程开始，判断是否可以转动转盘", MsgType.Notify);
                        step = 20;
                        break;
                    }
                case 20:
                    {
                        if (IsDDCanRotate())
                        {
                            RotateDD(Stn8TurnTableStnEnum.LulLot, LotStageType.LulLotStage);
                            step = 50;
                        }
                        break;
                    }
                case 50:
                    {
                        var ret = IsDDArrive();
                        if (ret == MotionStatus.Arrived)
                        {
                            ProcInfoHelper.Instance.UpdateStn8Status(Stn8TurnTableStnEnum.LulLot, true);
                            ShowMsg($"{StationName}->转盘转动到上空Lot位置，发送上空Lot请求给LotBox工位");
                            RegisterService.Instance.SetBoolReg(RegisterEnum.Stn8TurnTableLoadEmptyLotReq);
                            step = 100;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 50;
                        }
                        break;
                    }
                case 100:
                    {
                        if (GetBoolReg(RegisterEnum.LotBoxStnLoadEmptyLotDone))
                        {
                            ResetBoolReg(RegisterEnum.LotBoxStnLoadEmptyLotDone);
                            var lotModel = ProcInfoHelper.Instance.PickLotFromLotStage(LotPosEnum.LotArmClaw);
                            if (lotModel != null)
                            {
                                ProcInfoHelper.Instance.LoadEmptyLot(Stn8TurnTableStnEnum.LulLot, lotModel.TrayList);
                                ShowMsg($"{StationName}->LotBox工位上空Lot完成，判断是否可以转动转盘");
                                step = 150;
                                ProcInfoHelper.Instance.UpdateStn8Status(Stn8TurnTableStnEnum.LulLot, false);
                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.Stn8LulStageDetect.ToString(), out int _).IsShield = true;
                                    Thread.Sleep(100);
                                }
                            }
                            else
                            {
                                ShowMsg($"{StationName}->从LotArmClaw获取LotModel信息失败", MsgType.Error);
                                AlarmManager.CreateAlarm(AlarmCode.GetLotModelFail.ToInt(), StationNum);
                            }
                        }
                        break;
                    }
                case 150:
                    {
                        if (IsDDCanRotate())
                        {
                            RotateDD(Stn8TurnTableStnEnum.PickEmptyTrayStn, LotStageType.LulLotStage);//转盘转动到取空Tray位置
                            step = 200;
                        }
                        break;
                    }
                case 200:
                    {
                        var ret = IsDDArrive();
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->转盘到达取空Tray位置，上空Lot流程结束", MsgType.Notify);
                            ResetBoolReg(RegisterEnum.LotBoxStnLoadEmptyLotDone);
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 150;
                        }
                        break;
                    }
            }
            return step;
        }
        /// <summary>
        /// 下满Lot流程
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int UnloadLotFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->下满Lot流程开始，判断是否可以转动转盘", MsgType.Notify);
                        step = 20;
                        break;
                    }
                case 20:
                    {
                        if (IsDDCanRotate())
                        {
                            RotateDD(Stn8TurnTableStnEnum.LulLot, LotStageType.LulLotStage);
                            step = 50;
                        }
                        break;
                    }
                case 50:
                    {
                        var ret = IsDDArrive();
                        if (ret == MotionStatus.Arrived)
                        {
                            ProcInfoHelper.Instance.UpdateStn8Status(Stn8TurnTableStnEnum.LulLot, true);
                            ShowMsg($"{StationName}->转盘转动到下满Lot位置，发送下满Lot请求给LotBox工位");
                            RegisterService.Instance.SetBoolReg(RegisterEnum.Stn8TurnTableUnloadFullLotReq);
                            step = 100;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 50;
                        }
                        break;
                    }
                case 100:
                    {
                        if (GetBoolReg(RegisterEnum.LotBoxStnUnloadFullLotDone))
                        {
                            ResetBoolReg(RegisterEnum.LotBoxStnUnloadFullLotDone);
                            ShowMsg($"{StationName}->下满Lot流程完成", MsgType.Notify);
                            ProcInfoHelper.Instance.UpdateStn8Status(Stn8TurnTableStnEnum.LulLot, false);
                            step = 0;
                        }
                        break;
                    }

            }
            return step;
        }
        /// <summary>
        /// 8工位转盘DD马达是否可以转动
        /// </summary>
        /// <returns></returns>
        private bool IsDDCanRotate()
        {
            if (Motion.IsArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos) &&
               Motion.IsArrive(AxisName.LotBox_ArmZ, PosName.WaitPos) &&
                Motion.IsArrive(AxisName.IsolationPaper_ArmZ, PosName.WaitPos))
            {
                return true;
            }
            else
            {
                ShowMsg($"{StationName}->条件不满足，禁止转盘转动", MsgType.Error);
            }
            return false;
        }

        private string GetInputName(int trayStageNo)
        {
            return $"Stn8Bin{trayStageNo}StageDetect";
        }
        /// <summary>
        /// 获取取放Tray时ArmZ轴的高度
        /// </summary>
        /// <returns></returns>
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
                ShowMsg($"{StationName}->获取{AxisName.Stn8TurnTable_ArmZ}轴{PosName.PPTrayBaseHeight}点位信息失败", MsgType.Error, true);
                AlarmManager.CreateAlarm(AlarmCode.GetPosInfoFail.ToInt(), AllStationAlarm);
            }
            return ret;
        }


        /// <summary>
        /// 移动XY轴到指定位置
        /// </summary>
        /// <param name="step"></param>
        /// <param name="posName"></param>
        /// <returns></returns>
        private int MoveAxisToPos(int step, PosName posName)
        {
            try
            {
                switch (step)
                {
                    case 1:
                        {
                            if (posName == PosName.WaitPos)
                            {
                                step = 50;
                                ShowMsg($"{StationName}->等待{AxisName.IsolationPaper_ArmX}轴和{AxisName.IsolationPaper_ArmY}轴移动到{PosName.WaitPos}位");
                                Delay.Start(ParaHelper.Motion.WaitAxisMoveTimeOut);
                                step = 100;
                            }
                            else
                            {
                                step = 500;
                            }
                            break;
                        }
                    case 100:
                        {
                            if (Motion.IsArrive(AxisName.IsolationPaper_ArmX, PosName.WaitPos) && Motion.IsArrive(AxisName.IsolationPaper_ArmY, PosName.WaitPos))
                            {
                                ShowMsg($"{StationName}->{AxisName.IsolationPaper_ArmX}轴和{AxisName.IsolationPaper_ArmY}轴到达{PosName.WaitPos}位");
                                step = 500;
                            }
                            else if (Delay.TimeUp())
                            {
                                ShowMsg($"{StationName}->等待{AxisName.IsolationPaper_ArmX}轴和{AxisName.IsolationPaper_ArmY}轴移动到{PosName.WaitPos}位超时");
                                step = 10000;
                            }
                            break;
                        }
                    case 500:
                        {
                            Motion.MoveAbs(AxisName.Stn8TurnTable_ArmX, PosName.WaitPos);
                            Motion.MoveAbs(AxisName.Stn8TurnTable_ArmY, PosName.WaitPos);
                            ShowMsg($"{StationName}->{AxisName.Stn8TurnTable_ArmX}轴和{AxisName.Stn8TurnTable_ArmY}轴移动到{PosName.WaitPos}位");
                            step = 550;
                            break;
                        }
                    case 550:
                        {
                            if (Motion.IsArrive(AxisName.Stn8TurnTable_ArmX, PosName.WaitPos) &&
                                Motion.IsArrive(AxisName.Stn8TurnTable_ArmY, PosName.WaitPos))
                            {
                                ShowMsg($"{StationName}->{AxisName.Stn8TurnTable_ArmX}轴和{AxisName.Stn8TurnTable_ArmY}轴到达{PosName.WaitPos}位");
                            }
                            break;
                        }



                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                step = -1;
                LogManager.GetLogger("Error").Error($"[Stn8TurnTableStn][MoveAxisToPos]方法执行异常，异常原因：{ex}");
            }
            return step;
        }
        public void Init()
        {
            Init(StationNum, StationName);
        }

        public void OnButtonTrigger(IOInfo io, bool sts)
        {
        }

        public void PauseHandle()
        {
        }

        public void RegisterEvent()
        {
            var pickPatchStn = StationManager.Instance.GetChild<PickPatchStn>(nameof(PickPatchStn));
            pickPatchStn.ResetRegisterEvent += Stn_ResetRegisterEvent;
        }

        private void Stn_ResetRegisterEvent(RegisterEnum obj)
        {
            //if (obj == RegisterEnum.StorageHalfTray)
            //{
            //    LastRegisterValue[obj.ToInt()] = false;
            //}
        }
        public void ResetBits()
        {
            LulLotStep = -1;
            MoveToWaitPosStep = -1;
            PickTrayFrom8StnStep = -1;
            PutTrayToEmptyTrayBufferStep = -1;
            PutTrayToHalfTrayStageStep = -1;
            PickTrayFromHalfTrayStep = -1;
            PutEmptyTrayTo4StnStep = -1;
            PickFullTrayFrom4StnStep = -1;
            PutFullTrayTo8StnStep = -1;
        }
        /// <summary>
        /// 是否转盘转动到位
        /// </summary>
        /// <returns></returns>
        private MotionStatus IsDDArrive()
        {
            var ret = IsAxisArrive(AxisName.Stn8TurnTable_DD, LotTrayDDPos);
            if (ret == MotionStatus.Arrived)
            {
                RotateInfoHelper.Instance.UpdateRotateInfo(RotateStnType.Stn8TurnTable, LotTrayDDPos);
            }
            return ret;
        }
        public void StopHandle()
        {
        }

        public void WaitFlowDone()
        {
        }
        /// <summary>
        /// DD转盘旋转到指定位置
        /// </summary>
        /// <param name="trayStationNo">工位号，固定不变，不跟随转达转动发生改变</param>
        /// <param name="stageTypeNo">Tray平台号，跟随转塔转动实时变化</param>
        private void RotateDD(Stn8TurnTableStnEnum trayStationNo, LotStageType stageTypeNo, bool isNg = false)
        {
            var stnNo = trayStationNo.ToInt();
            var rotateIndex = RotateStnType.Stn8TurnTable.ToInt();
            var currentStageNo = RotateInfoHelper.Instance.GetStageNo(stnNo, RotateStnType.Stn8TurnTable);
            var currentAngle = NormalizeAngle((stnNo - currentStageNo) * StepAngle);
            var currentStationNo = RotateInfoHelper.Instance.GetStnNo(stageTypeNo.ToInt(), RotateStnType.Stn8TurnTable);
            var targetOffsetAngle = NormalizeOffsetAngle((stnNo - currentStationNo) * StepAngle);
            LotTrayDDPos = NormalizeAngle(currentAngle + targetOffsetAngle);
            MoveAbs(AxisName.Stn8TurnTable_DD, LotTrayDDPos);
        }
        /// <summary>
        /// 归一化角度到0-360范围内
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        private double NormalizeAngle(double angle)
        {
            while (angle < 0)
            {
                angle += 360;
            }

            while (angle >= 360)
            {
                angle -= 360;
            }

            return angle;
        }
        /// <summary>
        /// 归一化角度到-180到180范围内
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        private double NormalizeOffsetAngle(double angle)
        {
            while (angle > 180)
            {
                angle -= 360;
            }

            while (angle < -180)
            {
                angle += 360;
            }

            return angle;
        }
    }

}
