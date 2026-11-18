using JC.PTIS.Main.AlarmInfo;
using JC.PTIS.Main.Devices.Vision;
using JC.PTIS.Main.Devices.VisionPC.MsgModel;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Params;
using JC.PTIS.Main.RotateInfo;
using JC.PTIS.Main.Service;
using Newtonsoft.Json.Linq;
using SqlSugar;
using StdLib.PCBaseFrame.Alarm;
using StdLib.PCBaseFrame.Args;
using StdLib.PCBaseFrame.Entities;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.PCBaseFrame.Extension.Entities;
using StdLib.PCBaseFrame.Motion;
using StdLib.PCBaseFrame.MotionInfo;
using StdLib.PCBaseFrame.Param;
using StdLib.PCBaseFrame.Station;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static StdLib.WindowsControls.HTRoundMeter;

namespace JC.PTIS.Main.Stations
{
    public class HalfTrayStn : StationBaseEx, IStation
    {
        public int StationNum { get; set; } = 12;
        public string StationName { get; set; } = "HalfTrayStn";

        List<AxisName> Axes = new List<AxisName>();
        List<PosName> PosNames = new List<PosName>();//位置名称集合
        List<double> Poses = new List<double>();//实际位置集合
        List<CylinderName> Cylinders = new List<CylinderName>();

        #region 子流程状态机步骤
        int ModuleMoveStep = -1;
        int UnloadTrayStep = -1;
        int LoadTrayStep = -1;
        int PickStep = -1;
        int PatchStep = -1;
        int TrayInBufferStep = -1;
        int TrayOutBufferStep = -1;
        int StorageOkHalfTrayStep = -1;
        int FullTrayArmPickTrayFromBufferStep = -1;
        int HalfTrayStageMoveStep = -1;
        #endregion

        int CurPickNo = -1;
        int CurDieNo = -1;
        int RemainDieCount = 0;//剩余挑补Die数量
        int TotalNGCount = 0;//NG Die总数量
        int PPCounter = 0;
        (int R, int C) EmptyCellRCNo = (-1, -1);//Tray中空穴位的行列号
        (int R, int C) ExistCellRCNo = (-1, -1);//Tray中存在Die的行列号
        int DestLayerNo = -1;

        double AxisZDestPos = 0;

        BinLevel PickBinLevel = BinLevel.None;

        bool IsOkTrayAligned = false;//OkTray是否对位完成
        bool IsNgTrayAligned = false;//NgTray是否对位完成
        bool IsOkTrayEmpty = false;//OK半Tray是否为空
        Xyt[] BasePos = new Xyt[2] { new Xyt(), new Xyt() };//基准位置
        Xyt[] FirstCellPos = new Xyt[2] { new Xyt(), new Xyt() };//Tray第一个穴位位置
        TrayStageInfo OkTrayInfo = null;
        TrayStageInfo NgTrayInfo = null;
        TrayStageInfo Stn4TrayInfo = null;
        HalfTrayStnEnum HalfTrayStnEnum = HalfTrayStnEnum.None;
        bool IsHalfTray = false;
        Delay Delay = new Delay();
        public void Home()
        {
            switch (HomeStep)
            {
                case 1:
                    {
                        Axes = new List<AxisName>()
                        {
                            AxisName.Stn8TurnTable_ArmZ,
                        };
                        for (int i = AxisName.TrayStage1X.ToInt(); i <= AxisName.TrayStage4Y.ToInt(); i++)
                        {
                            Axes.Add((AxisName)i);
                        }
                        PosNames = new List<PosName>() { PosName.WaitPos };
                        ShowMsg($"{StationName}->等待{string.Join(",轴", Axes)}移动到待机位");
                        Delay.Start(120);
                        HomeStep = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->{string.Join(",轴", Axes)}到达待机位");
                            ShowMsg($"{StationName}->开始回零");
                            ShowMsg($"{StationName}->{CylinderName.HalfTrayBufferLiftCyl}气缸缩回");
                            Cylinder(CylinderName.HalfTrayBufferLiftCyl, false);
                            HomeStep = 60;
                        }
                        else if (Delay.TimeUp())
                        {
                            ShowMsg($"{StationName}->等待{AxisName.Stn8TurnTable_ArmZ}轴移动到待机位超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 60:
                    {
                        var ret = IsCylinderArrive(CylinderName.HalfTrayBufferLiftCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->{CylinderName.HalfTrayBufferLiftCyl}气缸缩回到位");
                            ShowMsg($"{StationName}->{AxisName.HalfTray_BufferX}轴开始回零");
                            Motion.Home(AxisName.HalfTray_BufferX);
                            HomeStep = 100;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->{CylinderName.HalfTrayBufferLiftCyl}气缸缩回超时", MsgType.Error);
                            HomeStep = 1;
                        }
                        break;
                    }
                case 100:
                    {
                        if (Motion.IsHomeDone(AxisName.HalfTray_BufferX) &&
                            Motion.IsHomeDone(AxisName.PickPatch_PickZ) &&
                            Motion.IsHomeDone(AxisName.PickPatch_PlaceZ))
                        {
                            ShowMsg($"{StationName}->{AxisName.HalfTray_BufferX}轴，{AxisName.PickPatch_PickZ}轴，{AxisName.PickPatch_PickZ}轴回零完成");
                            ShowMsg($"{StationName}->{AxisName.HalfTray_BufferX}轴移动到{PosName.WaitPos}位");
                            Motion.MoveAbs(AxisName.HalfTray_BufferX, PosName.WaitPos);
                            HomeStep = 150;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.HalfTray_BufferX))
                        {
                            ShowMsg($"{StationName}->{AxisName.HalfTray_BufferX}轴回原超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 150:
                    {
                        if (Motion.IsArrive(AxisName.HalfTray_BufferX, PosName.WaitPos))
                        {
                            ShowMsg($"{StationName}->{AxisName.HalfTray_BufferX}轴到达{PosName.WaitPos}位,HalfTrayBufferClawCyl气缸缩回");
                            Cylinder(CylinderName.HalfTrayBufferClawCyl, false);
                            HomeStep = 160;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.HalfTray_BufferX))
                        {
                            ShowMsg($"{StationName}->{AxisName.HalfTray_BufferX}轴移动到{PosName.WaitPos}位超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 160:
                    {
                        var ret = IsCylinderArrive(CylinderName.HalfTrayBufferClawCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->HalfTrayBufferClawCyl气缸缩回到位，平台开始回零");
                            Motion.Home(AxisName.HalfTray_BufferZ);
                            Motion.Home(AxisName.HalfTray_StageX);
                            Motion.Home(AxisName.HalfTray_StageY);
                            HomeStep = 200;
                        }
                        break;
                    }
                case 200:
                    {
                        if (Motion.IsHomeDone(AxisName.HalfTray_StageX) &&
                            Motion.IsHomeDone(AxisName.HalfTray_StageY) &&
                            Motion.IsHomeDone(AxisName.HalfTray_BufferZ))
                        {
                            Axes = new List<AxisName> { AxisName.HalfTray_StageX, AxisName.HalfTray_StageY };
                            PosNames = new List<PosName> { PosName.HalfTrayStageSafePos };
                            ShowMsg($"{StationName}->平台回零完成，移动到避让位置");
                            MoveAbsM(Axes, PosNames);
                            HomeStep = 250;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.HalfTray_BufferZ))
                        {
                            ShowMsg($"{StationName}->{AxisName.HalfTray_BufferZ}轴回零超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.HalfTray_StageX))
                        {
                            ShowMsg($"{StationName}->{AxisName.HalfTray_StageX}轴回零超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.HalfTray_StageY))
                        {
                            ShowMsg($"{StationName}->{AxisName.HalfTray_StageY}轴回零超时", MsgType.Error);
                            HomeStep = 1000;
                        }

                        break;
                    }
                case 250:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->{string.Join(",轴", Axes)}到达避让位");
                            Motion.Home(AxisName.HalfTray_StageT);
                            ShowMsg($"{StationName}->HalfTray_StageT轴开始回零");
                            HomeStep = 300;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->{string.Join(",轴", Axes)}移动到避让位超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        break;
                    }

                case 300:
                    {
                        if (Motion.IsHomeDone(AxisName.HalfTray_StageT))
                        {
                            ShowMsg($"{StationName}->HalfTray_StageT轴回零完成");
                            Motion.MoveAbs(AxisName.HalfTray_StageT, PosName.HalfTrayStageSafePos);
                            ShowMsg($"{StationName}->HalfTray_StageT轴移动到避让位");
                            HomeStep = 350;

                        }
                        else if (Motion.IsMoveTimeOut(AxisName.HalfTray_StageT))
                        {
                            ShowMsg($"{StationName}->{AxisName.HalfTray_StageT}轴回零超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 350:
                    {
                        if (Motion.IsArrive(AxisName.HalfTray_StageT, PosName.HalfTrayStageSafePos))
                        {
                            ShowMsg($"{StationName}->HalfTray_StageT轴到达避让位");
                            Motion.Home(AxisName.HalfTray_Y);
                            HomeStep = 400;
                            break;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.HalfTray_StageT))
                        {
                            ShowMsg($"{StationName}->{AxisName.HalfTray_StageT}轴移动到避让位超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 400:
                    {
                        if (Motion.IsHomeDone(AxisName.HalfTray_Y))
                        {
                            ShowMsg($"{StationName}->HalfTray_Y轴回零完成，移动到待机位");
                            Motion.MoveAbs(AxisName.HalfTray_Y, PosName.WaitPos);
                            HomeStep = 450;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.HalfTray_Y))
                        {
                            ShowMsg($"{StationName}->{AxisName.HalfTray_Y}轴回零超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 450:
                    {
                        if (Motion.IsArrive(AxisName.HalfTray_Y, PosName.WaitPos))
                        {
                            ShowMsg($"{StationName}->HalfTray_Y轴到达待机位，平台移动到待机位");
                            Axes = new List<AxisName> { AxisName.HalfTray_StageX, AxisName.HalfTray_StageY, AxisName.HalfTray_StageT };
                            PosNames = new List<PosName> { PosName.WaitPos };
                            MoveAbsM(Axes, PosNames);
                            HomeStep = 500;
                        }
                        break;
                    }
                case 500:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->平台到达待机位，回零完成");
                            UpdateHomeSts(StationName, StationNum, true);
                            HomeStep = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
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

        public void AutoRun()
        {
            try
            {
                switch (AutoRunStep)
                {
                    case 1:
                        {
                            ShowMsg($"{StationName}->等待4工位转盘转动到位");
                            AutoRunStep = 100;
                            break;
                        }
                    case 100:
                        {
                            if (GetBoolReg(RegisterEnum.Stn4DDRotateDone))
                            {
                                var stageNo = RotateInfoHelper.Instance.GetStageNo(Stn4TurnTableStnEnum.PickPatchStn.ToInt(), RotateStnType.Stn4TurnTable);
                                var stage = StageManager.Get(stageNo);
                                //判断正面检工位是否有Tray
                                if (Motion.GetDigitalInput(stage.IsExistTray))
                                {
                                    Stn4TrayInfo = ProcInfoHelper.Instance.GetTrayInfo(Stn4TurnTableStnEnum.PickPatchStn).DeepCopy();
                                    var wafer = ProcInfoHelper.Instance.GetWaferModel(WaferPosEnum.WaferTable);
                                    if (Stn4TrayInfo.TrayModel != null &&//信息不能为空
                                        Stn4TrayInfo.TrayModel.BinLevel % 2 == 1 &&//Bin135执行挑补流程
                                        Stn4TrayInfo.TrayModel.TrayStatus != TrayStatusEnum.Empty &&//不能是空Tray
                                        Stn4TrayInfo.TrayModel.TrayStatus != TrayStatusEnum.OkFull &&//不能是Ok满Tray
                                        Stn4TrayInfo.TrayModel.TrayStatus != TrayStatusEnum.OkHalf ||//不能是Ok半Tray
                                        (Stn4TrayInfo.TrayModel.TrayStatus == TrayStatusEnum.NoDetectHalf && wafer.IsLastWafer)) //最后一张Wafer的半Tray执行挑补流程
                                    {
                                        ShowMsg($"{StationName}->正面检工位有Tray，等级：Bin{Stn4TrayInfo.TrayModel.BinLevel}，状态：{Stn4TrayInfo.TrayModel.TrayStatus}，满足挑补条件，等待正面检测结果");
                                        IsOkTrayEmpty = false;
                                        PickBinLevel = (BinLevel)Stn4TrayInfo.TrayModel.BinLevel;
                                        AutoRunStep = 130;
                                    }
                                    else
                                    {
                                        if (Stn4TrayInfo.TrayModel != null &&
                                            Stn4TrayInfo.TrayModel.TrayType == TrayTypeEnum.Half)
                                        {
                                            ShowMsg($"{StationName}->正面检工位有半Tray，等级：Bin{Stn4TrayInfo.TrayModel.BinLevel}，状态：{Stn4TrayInfo.TrayModel.TrayStatus}，等待4工位转盘暂存半Tray请求");
                                            AutoRunStep = 950;
                                        }
                                    }
                                }
                                else
                                {
                                    AutoRunStep = 950;
                                }
                            }
                            break;
                        }
                    case 130:
                        {
                            if (GetBoolReg(RegisterEnum.FrontDetectExistNgDie))
                            {
                                ResetBoolReg(RegisterEnum.FrontDetectExistNgDie);
                                var ngCount = RegisterService.Instance.GetIntValue(IntValueEnum.TotalNGDieCount);//获取要挑补Die数量
                                if (ngCount != 0)
                                {
                                    RemainDieCount = TotalNGCount = ngCount;
                                    RegisterService.Instance.ResetIntValue(IntValueEnum.TotalNGDieCount);
                                }
                                ShowMsg($"{StationName}->正面检有NgDie，数量：{TotalNGCount}，判断OkTray平台是否有Tray");
                                AutoRunStep = 150;
                            }
                            else if (GetBoolReg(RegisterEnum.FrontDetectNoExistNgDie))
                            {
                                ShowMsg($"{StationName}->正面检工位没有NgDie，不执行挑补流程");
                                ResetBoolReg(RegisterEnum.FrontDetectNoExistNgDie);
                                AutoRunStep = 100;
                            }
                            break;
                        }
                    case 150:
                        {
                            if (IsVacuumArriveEx(VacuumName.OKHalfTrayVac, true))
                            {
                                ShowMsg($"{StationName}->OkTray平台有Tray，判断OkTray中是否为空Tray");
                                OkTrayInfo = ProcInfoHelper.Instance.GetTrayInfo(TrayPosEnum.OkHalfTrayStage);
                                if (OkTrayInfo != null && OkTrayInfo.TrayModel != null)
                                {
                                    if (OkTrayInfo.TrayModel.TrayType == TrayTypeEnum.Empty)
                                    {
                                        ShowMsg($"{StationName}->OkTray是空Tray，执行下空Tray流程");
                                        UnloadTrayStep = 1;
                                        AutoRunStep = 250;
                                    }
                                    else
                                    {
                                        ShowMsg($"{StationName}->OkTray有半Tray，等级：Bin{OkTrayInfo.TrayModel.BinLevel}，判断和4工位转盘中的Tray等级是否一致");
                                        if (OkTrayInfo.TrayModel.BinLevel == Stn4TrayInfo.TrayModel.BinLevel)
                                        {
                                            ShowMsg($"{StationName}->Ok半Tray等级和4工位转盘中的Tray等级一致，判断NgTray平台是否有Tray");
                                            AutoRunStep = 200;
                                        }
                                        else
                                        {
                                            ShowMsg($"{StationName}->Ok半Tray等级和4工位转盘中的Tray等级不一致，执行缓存Ok半Tray流程");
                                            UnloadTrayStep = 1;
                                            AutoRunStep = 300;
                                        }
                                    }
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->OkTray平台有Tray但是获取Tray信息失败", MsgType.Error, true);
                                    AlarmManager.CreateAlarm(AlarmCode.GetTrayInfoFail.ToInt(), AllStationAlarm);
                                }
                            }
                            else
                            {
                                ShowMsg($"{StationName}->OkTray平台没有Tray，判断半Tray缓存中有等级一致的半Tray");
                                var ret = ProcInfoHelper.Instance.IsHalfTrayBufferExistTray((BinLevel)Stn4TrayInfo.TrayModel.BinLevel, true, HalfTraySource.HalfTrayStage);
                                if (ret)
                                {
                                    ShowMsg($"{StationName}->半Tray缓存中有{PickBinLevel}等级的Ok半Tray，执行从半Tray缓存取Tray流程");
                                    LoadTrayStep = 1;
                                    AutoRunStep = 350;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->半Tray缓存中没有{PickBinLevel}等级的Ok半Tray，判断NgTray平台是否有Tray");
                                    IsOkTrayEmpty = true;
                                    AutoRunStep = 200;

                                }
                            }
                            break;
                        }
                    case 200:
                        {
                            if (IsVacuumArriveEx(VacuumName.NGHalfTrayVac, true))
                            {
                                ShowMsg($"{StationName}->NgTray平台有Tray，判断是否是满Tray");
                                NgTrayInfo = ProcInfoHelper.Instance.GetTrayInfo(TrayPosEnum.NgHalfTrayStage);
                                if (NgTrayInfo.TrayModel != null)
                                {
                                    if (NgTrayInfo.TrayModel.TrayType == TrayTypeEnum.Full)
                                    {
                                        ShowMsg($"{StationName}->NgTray是满Tray，等级：Bin{NgTrayInfo.TrayModel.BinLevel}，执行满Tray下料流程");
                                        UnloadTrayStep = 1;
                                        AutoRunStep = 400;
                                    }
                                    else
                                    {
                                        ShowMsg($"{StationName}->NgTray不是满Tray，等级：Bin{NgTrayInfo.TrayModel.BinLevel}，判断和4工位转盘中的Tray等级是否一致");
                                        if (NgTrayInfo.TrayModel.BinLevel == Stn4TrayInfo.TrayModel.BinLevel)
                                        {
                                            ShowMsg($"{StationName}->Ng半Tray等级和4工位转盘中的Tray等级一致，不用缓存Ng半Tray，执行挑Die流程");
                                            PickStep = 1;
                                            AutoRunStep = 700;
                                        }
                                        else
                                        {
                                            ShowMsg($"{StationName}->Ng半Tray等级和4工位转盘中的Tray等级不一致，执行缓存Ng半Tray流程");
                                            UnloadTrayStep = 1;
                                            AutoRunStep = 500;
                                        }
                                    }
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->NgTray平台有Tray但是获取Tray信息失败", MsgType.Error, true);
                                    AlarmManager.CreateAlarm(AlarmCode.GetTrayInfoFail.ToInt(), AllStationAlarm);
                                }
                            }
                            else
                            {
                                ShowMsg($"{StationName}->NgTray平台没有Tray，判断半Tray缓存中有等级一致的半Tray");
                                var ret = ProcInfoHelper.Instance.IsHalfTrayBufferExistTray((BinLevel)Stn4TrayInfo.TrayModel.BinLevel, false, HalfTraySource.HalfTrayStage);
                                if (ret)
                                {
                                    ShowMsg($"{StationName}->半Tray缓存中有{PickBinLevel}等级的Ng半Tray，执行从半Tray缓存取Tray流程");
                                    LoadTrayStep = 1;
                                    AutoRunStep = 550;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->半Tray缓存中没有{PickBinLevel}等级的Ng半Tray，请求8工位转盘上空Tray");
                                    LoadTrayStep = 1;
                                    AutoRunStep = 600;
                                }
                            }
                            break;
                        }
                    case 250:
                        {
                            UnloadTrayStep = HalfTrayStageUnloadTrayFlow(UnloadTrayStep, HalfTrayStnEnum.OkTray, false);
                            if (UnloadTrayStep == 0)
                            {
                                if (IsOkTrayEmpty)
                                {
                                    ShowMsg($"{StationName}->Ok半Tray平台没有Tray，请求上Ok满Tray");
                                    LoadTrayStep = 1;
                                    AutoRunStep = 650;
                                }
                                else
                                {
                                    AutoRunStep = 150;
                                }
                            }
                            break;
                        }
                    case 300:
                        {
                            UnloadTrayStep = HalfTrayStageUnloadTrayFlow(UnloadTrayStep, HalfTrayStnEnum.OkTray, true);
                            if (UnloadTrayStep == 0)
                            {
                                ShowMsg($"{StationName}->暂存Ok半Tray结束");
                                AutoRunStep = 150;
                            }
                            break;
                        }
                    case 350:
                        {
                            LoadTrayStep = HalfTrayStageLoadTrayFlow(LoadTrayStep, HalfTrayStnEnum.OkTray, true);
                            if (LoadTrayStep == 0)
                            {
                                IsOkTrayEmpty = false;
                                ShowMsg($"{StationName}->Ok半Tray上料流程结束");
                                AutoRunStep = 150;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.OKHalfTrayVacDetect.ToString(), out int _).IsShield = true;
                                    Thread.Sleep(500);
                                }
                            }
                            break;
                        }
                    case 400:
                        {
                            UnloadTrayStep = HalfTrayStageUnloadTrayFlow(UnloadTrayStep, HalfTrayStnEnum.NgTray, false);
                            if (UnloadTrayStep == 0)
                            {
                                AutoRunStep = 200;
                            }
                            break;
                        }
                    case 500:
                        {
                            UnloadTrayStep = HalfTrayStageUnloadTrayFlow(UnloadTrayStep, HalfTrayStnEnum.NgTray, true);
                            if (UnloadTrayStep == 0)
                            {
                                ShowMsg($"{StationName}->暂存Ng半Tray结束");
                                AutoRunStep = 200;
                            }
                            break;
                        }
                    case 550:
                        {
                            LoadTrayStep = HalfTrayStageLoadTrayFlow(LoadTrayStep, HalfTrayStnEnum.NgTray, true);
                            if (LoadTrayStep == 0)
                            {
                                ShowMsg($"{StationName}->Ng半Tray上料流程结束");
                                AutoRunStep = 200;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.NGHalfTrayVacDetect.ToString(), out int _).IsShield = true;
                                    Thread.Sleep(500);
                                }
                            }
                            break;
                        }
                    case 600:
                        {
                            LoadTrayStep = HalfTrayStageLoadTrayFlow(LoadTrayStep, HalfTrayStnEnum.NgTray, false);
                            if (LoadTrayStep == 0)
                            {
                                AutoRunStep = 200;
                                NgTrayInfo = ProcInfoHelper.Instance.GetTrayInfo(TrayPosEnum.NgHalfTrayStage);
                                if (NgTrayInfo != null && NgTrayInfo.TrayModel != null)
                                {
                                    NgTrayInfo.TrayModel.BinLevel = Stn4TrayInfo.TrayModel.BinLevel;
                                }
                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.NGHalfTrayVacDetect.ToString(), out int _).IsShield = true;
                                    Thread.Sleep(500);
                                }
                            }
                            break;
                        }

                    case 650:
                        {
                            LoadTrayStep = HalfTrayStageLoadTrayFlow(LoadTrayStep, HalfTrayStnEnum.OkTray, false);
                            if (LoadTrayStep == 0)
                            {
                                IsOkTrayEmpty = false;
                                ShowMsg($"{StationName}->Ok半Tray上料流程结束");
                                AutoRunStep = 100;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.OKHalfTrayVacDetect.ToString(), out int _).IsShield = true;
                                    Thread.Sleep(500);
                                }
                            }
                            break;
                        }

                    case 700:
                        {
                            PickStep = PickFlow(PickStep, PickBinLevel);
                            if (PickStep == 0)
                            {
                                ShowMsg($"{StationName}->挑Die流程结束。等待挑补工位发送补Die信号");
                                PatchStep = 1;
                                AutoRunStep = 750;
                            }
                            break;
                        }
                    case 750:
                        {
                            PatchStep = PatchFlow(PatchStep);
                            if (PatchStep == 0)
                            {
                                ShowMsg($"{StationName}->补Die流程结束，判断当前Tray挑补流程是否结束");
                                if (RemainDieCount == 0)
                                {
                                    //ResetBoolReg(RegisterEnum.FrontDetectExistNgDie);
                                    CurDieNo = TotalNGCount = 0;
                                    ShowMsg($"{StationName}->正面检工位Tray中NgDie全部挑补完，判断是否需要暂存半Tray");
                                    var isStorageTray = true;
                                    //获取测高工位平台号
                                    var stageNo = RotateInfoHelper.Instance.GetStageNo(Stn4TurnTableStnEnum.MeasureHeight.ToInt(), RotateStnType.Stn4TurnTable);
                                    var measStage = StageManager.Get(stageNo);
                                    //判断测高工位是否有Tray
                                    if (Motion.GetDigitalInput(measStage.IsExistTray))
                                    {
                                        var measStageTray = ProcInfoHelper.Instance.GetTrayInfo(Stn4TurnTableStnEnum.MeasureHeight);
                                        if (measStageTray.TrayModel != null)
                                        {
                                            var wafer = ProcInfoHelper.Instance.GetWaferModel(WaferPosEnum.WaferTable);
                                            ShowMsg($"{StationName}->正面检工位Tray等级：{PickBinLevel}，测高工位Tray等级：Bin{measStageTray.TrayModel.BinLevel}");
                                            //对比正面检工位和测高工位Tray等级是否一致，如果不一致，通知HalfTray工位暂存半Tray
                                            if (measStageTray.TrayModel.BinLevel == PickBinLevel.ToInt() &&
                                                measStageTray.TrayModel.BinLevel % 2 == 1 &&
                                                measStageTray.TrayModel.TrayStatus == TrayStatusEnum.NoDetectFull ||
                                                (measStageTray.TrayModel.TrayStatus == TrayStatusEnum.NoDetectHalf && wafer.IsLastWafer))
                                            {
                                                ShowMsg($"{StationName}->测高工位Tray等级和正面检工位Tray等级一致，并且是Bin135等级，不需要暂存半Tray");
                                                isStorageTray = false;
                                            }
                                        }
                                        else
                                        {
                                            ShowMsg($"{StationName}->测高平台有Tray，但是获取Tray信息失败", MsgType.Error);
                                            AlarmManager.CreateAlarm(AlarmCode.GetTrayInfoFail.ToInt(), StationNum);
                                        }
                                    }

                                    RegisterService.Instance.SetBoolReg(RegisterEnum.HalfTrayConfirmDone);
                                    if (isStorageTray)
                                    {
                                        ShowMsg($"{StationName}->执行暂存半Tray流程");
                                        ResetBoolReg(RegisterEnum.TrayArmNoExistHalfTrayToStorage);
                                        AutoRunStep = 850;
                                    }
                                    else if (IsOkTrayEmpty)//如果OkTray为空，请求上Ok满Tray
                                    {
                                        if (IsVacuumArriveEx(VacuumName.OKHalfTrayVac, true))
                                        {
                                            ShowMsg($"{StationName}->Ok半Tray平台有空Tray，请求下Ok空Tray");
                                            //先把空Tary盘下料
                                            UnloadTrayStep = 1;
                                            AutoRunStep = 250;
                                        }
                                        else
                                        {
                                            ShowMsg($"{StationName}->Ok半Tray平台没有Tray，请求上Ok满Tray");
                                            LoadTrayStep = 1;
                                            AutoRunStep = 650;
                                        }
                                    }
                                    else
                                    {
                                        AutoRunStep = 100;
                                    }
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->正面检工位Tray中NgDie还没挑补完，继续执行挑补流程");
                                    PickStep = 1;
                                    AutoRunStep = 700;
                                }
                            }
                            break;
                        }

                    case 850:
                        {
                            if (IsVacuumArriveEx(VacuumName.OKHalfTrayVac, true))
                            {
                                HalfTrayStnEnum = HalfTrayStnEnum.OkTray;
                                UnloadTrayStep = 1;
                                var okTrayInfo = ProcInfoHelper.Instance.GetTrayInfo(TrayPosEnum.OkHalfTrayStage);
                                if (okTrayInfo.TrayModel != null)
                                {
                                    if (okTrayInfo.TrayModel.TrayType == TrayTypeEnum.Empty)
                                    {
                                        ShowMsg($"{StationName}->OkTray是空Tray，等级：Bin{okTrayInfo.TrayModel.BinLevel}，执行空Tray下料流程");
                                        IsHalfTray = false;
                                    }
                                    else if (okTrayInfo.TrayModel.TrayType == TrayTypeEnum.Half)
                                    {
                                        IsHalfTray = true;
                                        ShowMsg($"{StationName}->OkTray是半Tray，等级：Bin{okTrayInfo.TrayModel.BinLevel}，执行暂存Ok半Tray流程");
                                    }
                                    AutoRunStep = 900;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->从OkTray平台上获取TrayModel信息失败", MsgType.Error, true);
                                    AlarmManager.CreateAlarm(AlarmCode.GetTrayInfoFail.ToInt(), StationNum);
                                }
                            }
                            else if (IsVacuumArriveEx(VacuumName.NGHalfTrayVac, true))
                            {
                                HalfTrayStnEnum = HalfTrayStnEnum.NgTray;
                                UnloadTrayStep = 1;
                                var ngTrayInfo = ProcInfoHelper.Instance.GetTrayInfo(TrayPosEnum.NgHalfTrayStage);
                                if (ngTrayInfo.TrayModel != null)
                                {
                                    if (ngTrayInfo.TrayModel.TrayType == TrayTypeEnum.Full)
                                    {
                                        IsHalfTray = false;
                                        ShowMsg($"{StationName}->NgTray是满Tray，等级：Bin{ngTrayInfo.TrayModel.BinLevel}，执行满Tray下料流程");
                                    }
                                    else if (ngTrayInfo.TrayModel.TrayType == TrayTypeEnum.Half)
                                    {
                                        IsHalfTray = true;
                                        ShowMsg($"{StationName}->NgTray是半Tray，等级：Bin{ngTrayInfo.TrayModel.BinLevel}，执行暂存Ng半Tray流程");
                                    }
                                    AutoRunStep = 900;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->从NgTray平台上获取TrayModel信息失败", MsgType.Error, true);
                                    AlarmManager.CreateAlarm(AlarmCode.GetTrayInfoFail.ToInt(), StationNum);
                                }
                            }
                            else
                            {
                                AutoRunStep = 100;
                            }

                            break;
                        }
                    case 900:
                        {
                            UnloadTrayStep = HalfTrayStageUnloadTrayFlow(UnloadTrayStep, HalfTrayStnEnum, IsHalfTray);
                            if (UnloadTrayStep == 0)
                            {
                                AutoRunStep = 850;
                            }
                            break;
                        }
                    case 950:
                        {
                            if (GetBoolReg(RegisterEnum.FullTrayArmPickHalfTrayFromBufferReq))//收到4工位转盘取半Tray请求
                            {
                                ResetBoolReg(RegisterEnum.FullTrayArmPickHalfTrayFromBufferReq);
                                PickBinLevel = (BinLevel)RegisterService.Instance.GetIntValue(IntValueEnum.ShiftBinLevel);
                                ShowMsg($"{StationName}->收到4工位转盘取半Tray请求，执行从半Tray缓存中取Tray流程，等级：{PickBinLevel}");
                                FullTrayArmPickTrayFromBufferStep = 1;
                                AutoRunStep = 1050;
                            }
                            else if (GetBoolReg(RegisterEnum.FullTrayArmStorageHalfTrayReq))//满TrayArm上有Ok半Tray需要暂存
                            {
                                ResetBoolReg(RegisterEnum.FullTrayArmStorageHalfTrayReq);
                                ShowMsg($"{StationName}->满TrayArm上有Ok半Tray需要暂存，执行暂存Ok半Tray流程");
                                StorageOkHalfTrayStep = 1;
                                AutoRunStep = 1000;
                            }
                            else if (GetBoolReg(RegisterEnum.TrayArmNoExistHalfTrayToStorage) ||//TrayArm上没有半Tray需要暂存
                                GetBoolReg(RegisterEnum.Stn4DDStnLoadEmptyReq) ||//不从半Tray缓存取Tray
                                GetBoolReg(RegisterEnum.WaferEmpty))//Wafer为空
                            {
                                ResetBoolReg(RegisterEnum.TrayArmNoExistHalfTrayToStorage);
                                AutoRunStep = 100;
                            }
                            break;
                        }
                    case 1000:
                        {
                            StorageOkHalfTrayStep = StorageOkHalfTrayFlow(StorageOkHalfTrayStep);
                            if (StorageOkHalfTrayStep == 0)
                            {
                                if (!Motion.GetDigitalInput(InputName.FullTrayArmIsExistTray))
                                {
                                    RegisterService.Instance.SetBoolReg(RegisterEnum.StorageHalfTrayDone);
                                }
                                ShowMsg($"{StationName}->暂存Ok半Tray到缓存流程结束");
                                AutoRunStep = 950;
                            }
                            break;
                        }
                    case 1050:
                        {
                            FullTrayArmPickTrayFromBufferStep = FullTrayArmPickTrayFromBufferFlow(FullTrayArmPickTrayFromBufferStep);
                            if (FullTrayArmPickTrayFromBufferStep == 0)
                            {
                                ShowMsg($"{StationName}->满TrayArm从半Tray缓存取Tray完成");
                                AutoRunStep = 100;
                            }
                            else if (FullTrayArmPickTrayFromBufferStep == -1)
                            {
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
        /// 半Tray平台下Tray流程
        /// </summary>
        /// <param name="step"></param>
        /// <param name="type"></param>
        /// <param name="isHalfTray">true：NG或者OK半Tray。false：空Tray或者NG满Tray</param>
        /// <returns></returns>
        public int HalfTrayStageUnloadTrayFlow(int step, HalfTrayStnEnum type, bool isHalfTray)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->UnloadTray流程开始，Tray平台：{type}，是否是半Tray：{isHalfTray}", MsgType.Notify);
                        if (!isHalfTray && type == HalfTrayStnEnum.OkTray)
                        {
                            ShowMsg($"{StationName}->OkTray平台上是空Tray，判断8工位转盘取放空Tray平台的Tray数量是否小于最大限制：{ParaHelper.Prod.TrayCountPerStn8}");
                            var lotInfo = ProcInfoHelper.Instance.GetLotStageInfo(LotStageType.LulLotStage);
                            if (lotInfo.LotModel != null && lotInfo.LotModel.TrayList.Count < ParaHelper.Prod.TrayCountPerStn8)
                            {
                                ShowMsg($"{StationName}->8工位转盘取放空Tray平台的Tray数量：{lotInfo.LotModel.TrayList.Count}小于最大限制：{ParaHelper.Prod.TrayCountPerStn8}");
                            }
                            else
                            {
                                return 0;
                            }
                        }
                        else
                        {
                            if (type == HalfTrayStnEnum.OkTray)
                            {
                                if (IsVacuumArriveEx(VacuumName.OKHalfTrayVac, true) == false)
                                {
                                    ShowMsg($"{StationName}->OkTray平台上没有Tray盘，无法执行UnloadTray流程", MsgType.Error, true);
                                    AlarmManager.CreateAlarm(AlarmCode.OkTrayStageNoExistTrayCantUnloadTrayFlow.ToInt(), StationNum);
                                    return -1;
                                }
                            }
                            else
                            {
                                if (IsVacuumArriveEx(VacuumName.NGHalfTrayVac, true) == false)
                                {
                                    ShowMsg($"{StationName}->NgTray平台上没有Tray盘，无法执行UnloadTray流程", MsgType.Error, true);
                                    AlarmManager.CreateAlarm(AlarmCode.NgTrayStageNoExistTrayCantUnloadTrayFlow.ToInt(), StationNum);
                                    return -1;
                                }
                            }
                        }
                        ShowMsg($"{StationName}->半Tray平台移动到取放Tray位置");
                        HalfTrayStageMoveStep = 1;
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        HalfTrayStageMoveStep = HalfTrayToLulPosFlow(HalfTrayStageMoveStep, type == HalfTrayStnEnum.OkTray);
                        if (HalfTrayStageMoveStep == 0)
                        {
                            ShowMsg($"{StationName}->半Tray平台到达取放Tray位置");
                            if (isHalfTray)
                            {
                                ShowMsg($"{StationName}->当前Tray是半Tray，执行HalfTray入缓存流程");
                                TrayInBufferStep = 1;
                                step = 200;
                            }
                            else
                            {
                                if (type == HalfTrayStnEnum.OkTray)
                                {
                                    Vacuum(VacuumName.OKHalfTrayVac, false);
                                    RegisterService.Instance.SetBoolReg(RegisterEnum.HalfTrayStnUnloadEmptyTrayReq);
                                    ShowMsg($"{StationName}->发送下空Tray请求给Stn8TurnTable工位，等待TrayArm取走Tray");
                                }
                                else
                                {
                                    Vacuum(VacuumName.NGHalfTrayVac, false);
                                    RegisterService.Instance.SetBoolReg(RegisterEnum.HalfTrayStnUnloadFullTrayReq);
                                    ShowMsg($"{StationName}->发送下满Tray请求给Stn8TurnTable工位，等待TrayArm取走Tray");
                                }
                                step = 250;
                            }
                        }
                        break;
                    }

                case 200:
                    {
                        TrayInBufferStep = TrayInBufferFlow(TrayInBufferStep, type == HalfTrayStnEnum.OkTray);
                        if (TrayInBufferStep == 0)
                        {
                            ShowMsg($"{StationName}->HalfTray入缓存流程结束，UnloadTray流程结束", MsgType.Notify);
                            step = 0;
                        }
                        break;
                    }
                case 250:
                    {
                        if (GetBoolReg(RegisterEnum.FullArmPickEmptyTrayFromHalfTrayDone))
                        {
                            ResetBoolReg(RegisterEnum.FullArmPickEmptyTrayFromHalfTrayDone);
                            ShowMsg($"{StationName}->满TrayArm从挑补平台取空Tray完成，UnloadTray流程结束", MsgType.Notify);
                            step = 0;

                            if (Global.RunMode == RunMode.空跑模式)
                            {
                                IOInfoHelper.GetInputInfoByName(InputName.OKHalfTrayVacDetect.ToString(), out int _).IsShield = false;
                                Thread.Sleep(100);
                            }
                        }
                        else if (GetBoolReg(RegisterEnum.FullTrayArmPickFullTrayFromHalfTrayDone))
                        {
                            ResetBoolReg(RegisterEnum.FullTrayArmPickFullTrayFromHalfTrayDone);
                            ShowMsg($"{StationName}->满TrayArm从挑补平台取满Tray完成，UnloadTray流程结束", MsgType.Notify);
                            step = 0;

                            if (Global.RunMode == RunMode.空跑模式)
                            {
                                IOInfoHelper.GetInputInfoByName(InputName.NGHalfTrayVacDetect.ToString(), out int _).IsShield = false;
                                Thread.Sleep(100);
                            }
                        }
                        break;
                    }
                default:
                    break;
            }
            return step;
        }
        /// <summary>
        /// 半Tray平台上Tray流程
        /// </summary>
        /// <param name="step"></param>
        /// <param name="type"></param>
        /// <param name="isHalfTray">true：NG或者OK半Tray。false：空Tray或者NG满Tray</param>
        /// <returns></returns>
        public int HalfTrayStageLoadTrayFlow(int step, HalfTrayStnEnum type, bool isHalfTray)
        {
            try
            {
                switch (step)
                {
                    case 1:
                        {
                            ShowMsg($"{StationName}->LoadTray流程开始，Tray平台：{type}，是否是半Tray：{isHalfTray}", MsgType.Notify);
                            if (type == HalfTrayStnEnum.OkTray)
                            {
                                if (IsVacuumArriveEx(VacuumName.OKHalfTrayVac, true))
                                {
                                    ShowMsg($"{StationName}->OkTray平台上有Tray盘，无法执行LoadTray流程", MsgType.Error, true);
                                    AlarmManager.CreateAlarm(AlarmCode.OkTrayStageExistTrayCantLoadTrayFlow.ToInt(), StationNum);
                                    return -1;
                                }
                            }
                            else
                            {
                                if (IsVacuumArriveEx(VacuumName.NGHalfTrayVac, true))
                                {
                                    ShowMsg($"{StationName}->NgTray平台上有Tray盘，无法执行LoadTray流程", MsgType.Error, true);
                                    AlarmManager.CreateAlarm(AlarmCode.OkTrayStageExistTrayCantLoadTrayFlow.ToInt(), StationNum);
                                    return -1;
                                }
                            }
                            ShowMsg($"{StationName}->半Tray平台移动到取放Tray位置");
                            HalfTrayStageMoveStep = 1;
                            step = 50;
                            break;
                        }
                    case 50:
                        {
                            HalfTrayStageMoveStep = HalfTrayToLulPosFlow(HalfTrayStageMoveStep, type == HalfTrayStnEnum.OkTray);
                            if (HalfTrayStageMoveStep == 0)
                            {
                                ShowMsg($"{StationName}->半Tray平台到达取放Tray位置");
                                if (isHalfTray)
                                {
                                    ShowMsg($"{StationName}->当前是半Tray上料请求，执行HalfTray出缓存流程");
                                    TrayOutBufferStep = 1;
                                    step = 100;
                                }
                                else
                                {
                                    if (type == HalfTrayStnEnum.OkTray)
                                    {
                                        RegisterService.Instance.SetBoolReg(RegisterEnum.HalfTrayStnLoadFullTrayReq);
                                        ShowMsg($"{StationName}->发送上满Tray请求给Stn8TurnTable工位，等待TrayArm放OK满Tray完成");
                                    }
                                    else
                                    {
                                        RegisterService.Instance.SetBoolReg(RegisterEnum.HalfTrayStnLoadEmptyTrayReq);
                                        ShowMsg($"{StationName}->发送上空Tray请求给Stn8TurnTable工位，等待TrayArm放空Tray完成");
                                    }
                                    step = 150;
                                }
                            }
                            break;
                        }

                    case 100:
                        {
                            TrayOutBufferStep = TrayOutBufferFlow(TrayOutBufferStep, type == HalfTrayStnEnum.OkTray);
                            if (TrayOutBufferStep == 0)
                            {
                                ShowMsg($"{StationName}->HalfTray出缓存流程结束，LoadTray流程结束");
                                step = 0;
                            }
                            break;
                        }
                    case 150:
                        {
                            if (GetBoolReg(RegisterEnum.TrayArmPutEmptyTrayDone))
                            {
                                ShowMsg($"{StationName}->TrayArm放空Tray到HalfTray平台完成，LoadTray流程结束", MsgType.Notify);
                                ResetBoolReg(RegisterEnum.TrayArmPutEmptyTrayDone);
                                step = 0;
                            }
                            else if (GetBoolReg(RegisterEnum.TrayArmPutOKFullTrayDone))
                            {
                                ShowMsg($"{StationName}->TrayArm放满Tray到HalfTray平台完成，LoadTray流程结束", MsgType.Notify);
                                ResetBoolReg(RegisterEnum.TrayArmPutOKFullTrayDone);
                                step = 0;
                            }
                            break;
                        }
                    default:
                        break;
                }
                return step;
            }
            catch (Exception ex)
            {
                ShowMsg($"{StationName}->HalfTrayStageLoadTrayFlow方法执行异常：{ex}", MsgType.Error);
                return -1;
            }
        }
        /// <summary>
        /// 暂存4工位平台上的Ok半Tray流程
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int StorageOkHalfTrayFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->暂存4工位Ok半Tray流程开始，OkTray平台移动到取放Tray位置", MsgType.Notify);
                        HalfTrayStageMoveStep = 1;
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        HalfTrayStageMoveStep = HalfTrayToLulPosFlow(HalfTrayStageMoveStep, true);
                        if (HalfTrayStageMoveStep == 0)
                        {
                            ShowMsg($"{StationName}->OkTray平台到达取放Tray位置，等待满TrayArm放Tray完成", MsgType.Notify);
                            RegisterService.Instance.SetBoolReg(RegisterEnum.HalfTrayStageArrivePutTrayPos);
                            step = 100;
                        }
                        break;
                    }
                case 100:
                    {
                        if (GetBoolReg(RegisterEnum.TrayArmPutOKFullTrayDone))
                        {
                            ShowMsg($"{StationName}->满TrayArm放Tray到Ok半Tray平台完成，执行Ok半Tray进缓存流程", MsgType.Notify);
                            ResetBoolReg(RegisterEnum.TrayArmPutOKFullTrayDone);
                            TrayInBufferStep = 1;
                            step = 150;

                            if (Global.RunMode == RunMode.空跑模式)
                            {
                                IOInfoHelper.GetInputInfoByName(InputName.OKHalfTrayVacDetect.ToString(), out int _).IsShield = true;
                                Thread.Sleep(100);
                            }
                        }
                        break;
                    }
                case 150:
                    {
                        TrayInBufferStep = TrayInBufferFlow(TrayInBufferStep, true, HalfTraySource.Stn4);
                        if (TrayInBufferStep == 0)
                        {
                            ShowMsg($"{StationName}->暂存4工位Ok半Tray流程结束", MsgType.Notify);
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
        /// 半Tray进缓存流程
        /// </summary>
        /// <param name="step"></param>
        /// <param name="isOk"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        public int TrayInBufferFlow(int step, bool isOk, HalfTraySource source = HalfTraySource.HalfTrayStage)
        {
            switch (step)
            {
                case 1:
                    {
                        TrayStageInfo trayStageInfo = null;
                        if (isOk)
                        {
                            Vacuum(VacuumName.OKHalfTrayVac, false);
                            trayStageInfo = ProcInfoHelper.Instance.GetTrayInfo(TrayPosEnum.OkHalfTrayStage);
                        }
                        else
                        {
                            Vacuum(VacuumName.NGHalfTrayVac, false);
                            trayStageInfo = ProcInfoHelper.Instance.GetTrayInfo(TrayPosEnum.NgHalfTrayStage);
                        }
                        if (trayStageInfo == null)
                        {
                            ShowMsg($"{StationName}->从{(isOk ? "Ok" : "Ng")}半Tray平台中获取TrayModel信息失败", MsgType.Error, true);
                            AlarmManager.CreateAlarm(AlarmCode.GetTrayInfoFail.ToInt(), StationNum);
                        }
                        BinLevel binLevel = (BinLevel)trayStageInfo.TrayModel.BinLevel;
                        ShowMsg($"{StationName}->{(isOk ? "Ok" : "Ng")}Tray平台上有Tray，等级：{binLevel}");
                        //如果是Bin135等级，OK半Tray放在1到6层。未检测半Tray放在7到9层。其他等级或者是Ng半Tray，直接获取对应等级的层号
                        if (trayStageInfo.TrayModel.BinLevel % 2 == 1 &&
                            source == HalfTraySource.Stn4 && isOk)
                        {
                            if (trayStageInfo.TrayModel.TrayStatus == TrayStatusEnum.OkHalf)//已经检测的半Tray放在1到6层
                            {
                                DestLayerNo = ProcInfoHelper.Instance.GetTrayBufferLayerNo(binLevel, isOk, HalfTraySource.HalfTrayStage);
                            }
                            else if (trayStageInfo.TrayModel.TrayStatus == TrayStatusEnum.NoDetectHalf)//未检测的半Tray放在7到9层
                            {
                                DestLayerNo = ProcInfoHelper.Instance.GetTrayBufferLayerNo(binLevel, isOk, HalfTraySource.Stn4);
                            }
                        }
                        else//其他等级或者是Ng半Tray，直接获取对应等级的层号
                        {
                            DestLayerNo = ProcInfoHelper.Instance.GetTrayBufferLayerNo(binLevel, isOk, source);
                        }
                        ShowMsg($"{StationName}->获取{binLevel}等级在HalfTrayBuffer中的层号:{DestLayerNo}");
                        AxisZDestPos = GetBufferPos(DestLayerNo);
                        ShowMsg($"{StationName}->TrayBuffer第{DestLayerNo}层的位置:{AxisZDestPos}");
                        var vac = isOk ? VacuumName.OKHalfTrayVac : VacuumName.NGHalfTrayVac;
                        ShowMsg($"{StationName}->半Tray入Buffer流程开始，判断{(isOk ? "Ok" : "Ng")}Tray平台上是否有Tray", MsgType.Notify);
                        if (IsVacuumArriveEx(vac, true))
                        {
                            ShowMsg($"{StationName}->Tray平台上有Tray，X轴移动到从挑补平台取放Tray位置，Z轴移动到对应层号位置");
                            Cylinder(CylinderName.HalfTrayBufferClawCyl, false);//取料牙插气缸缩回
                            MoveAbs(AxisName.HalfTray_BufferZ, AxisZDestPos);  //Z轴移动到对应层号位置
                            MoveAbs(AxisName.HalfTray_BufferX, PosName.BufferArmPPTrayFromHalfTrayStagePos);  //X轴移动到从挑补平台取放Tray位置
                            step = 50;
                        }
                        else
                        {
                            ShowMsg($"{StationName}->Tray平台上没有Tray，判断半Tray缓存手臂上是否有Tray");
                            if (Motion.GetDigitalInput(InputName.HalfTrayBufferArmSensor))
                            {
                                ShowMsg($"{StationName}->半Tray缓存手臂上有Tray，X轴移动到缓存取放Tray位置");
                                MoveAbs(AxisName.HalfTray_BufferX, PosName.BufferArmPPTrayFromBufferPos);//X轴移动到缓存取放Tray位置
                                step = 250;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->半Tray缓存手臂上没有Tray，取料牙插气缸缩回");
                                Cylinder(CylinderName.HalfTrayBufferClawCyl, false);//取料牙插气缸缩回
                                step = 350;
                            }
                        }
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArrive(AxisName.HalfTray_BufferX, PosName.BufferArmPPTrayFromHalfTrayStagePos);
                        var ret2 = IsAxisArrive(AxisName.HalfTray_BufferZ, AxisZDestPos);
                        var ret3 = IsCylinderArrive(CylinderName.HalfTrayBufferClawCyl, false);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived && ret3 == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->X轴到达从挑补平台取放Tray位置，Z轴到达对应层号位置，分离气缸伸出，取料升降气缸下降");
                            Cylinder(CylinderName.HalfTrayBufferSplitCyl, true);//分离气缸伸出
                            Cylinder(CylinderName.HalfTrayBufferLiftCyl, true);//取料升降气缸下降
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
                        var ret = IsCylinderArrive(CylinderName.HalfTrayBufferSplitCyl, true);
                        var ret2 = IsCylinderArrive(CylinderName.HalfTrayBufferLiftCyl, true);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->取料牙插气缸伸出，Z轴下降，分离Tray盘");
                            Cylinder(CylinderName.HalfTrayBufferClawCyl, true);//取料牙插气缸伸出
                            MoveAbs(AxisName.HalfTray_BufferZ, PosName.WaitPos);//Z轴下降，分离Tray盘
                            step = 150;
                        }
                        else if (ret == MotionStatus.Timeout || ret2 == MotionStatus.Timeout)
                        {
                            step = 50;
                        }
                        break;
                    }
                case 150:
                    {
                        var ret = IsAxisArrive(AxisName.HalfTray_BufferZ, PosName.WaitPos);
                        var ret2 = IsCylinderArrive(CylinderName.HalfTrayBufferClawCyl, true);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->升降气缸缩回");
                            Cylinder(CylinderName.HalfTrayBufferLiftCyl, false);//升降气缸缩回
                            step = 200;
                        }
                        else if (ret == MotionStatus.Timeout || ret2 == MotionStatus.Timeout)
                        {
                            step = 100;
                        }
                        break;
                    }
                case 200:
                    {
                        var ret = IsCylinderArrive(CylinderName.HalfTrayBufferLiftCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            var trayModel = ProcInfoHelper.Instance.PickTrayFromTrayStage(isOk ? TrayPosEnum.OkHalfTrayStage : TrayPosEnum.NgHalfTrayStage);
                            if (trayModel != null)
                            {
                                ShowMsg($"{StationName}->X轴移动到缓存取放Tray位置");
                                ProcInfoHelper.Instance.PutTrayToTrayStage(TrayPosEnum.HalfTrayBufferArm, trayModel);
                                MoveAbs(AxisName.HalfTray_BufferX, PosName.BufferArmPPTrayFromBufferPos);//X轴移动到缓存取放Tray位置
                                step = 250;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    if (isOk)
                                    {
                                        IOInfoHelper.GetInputInfoByName(InputName.OKHalfTrayVacDetect.ToString(), out int _).IsShield = false;
                                    }
                                    else
                                    {
                                        IOInfoHelper.GetInputInfoByName(InputName.NGHalfTrayVacDetect.ToString(), out int _).IsShield = false;
                                    }
                                    IOInfoHelper.GetInputInfoByName(InputName.HalfTrayBufferArmSensor.ToString(), out int _).IsShield = true;
                                    Thread.Sleep(100);
                                }
                            }
                            else
                            {
                                ShowMsg($"{StationName}->从半Tray平台中获取TrayModel信息失败", MsgType.Error, true);
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
                        var ret = IsAxisArrive(AxisName.HalfTray_BufferX, PosName.BufferArmPPTrayFromBufferPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->升降气缸下降");
                            Cylinder(CylinderName.HalfTrayBufferLiftCyl, true);//升降气缸下降
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
                        var ret = IsCylinderArrive(CylinderName.HalfTrayBufferLiftCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            var model = ProcInfoHelper.Instance.PickTrayFromTrayStage(TrayPosEnum.HalfTrayBufferArm);
                            if (model != null)
                            {
                                ShowMsg($"{StationName}->取料牙插气缸缩回");
                                Cylinder(CylinderName.HalfTrayBufferClawCyl, false);//取料牙插气缸缩回
                                ProcInfoHelper.Instance.PutTrayToHalfTrayBuffer(model, DestLayerNo);
                                step = 350;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.HalfTrayBufferArmSensor.ToString(), out int _).IsShield = false;
                                    Thread.Sleep(100);
                                }
                            }
                            else
                            {
                                ShowMsg($"{StationName}->从半Tray缓存Arm中获取TrayModel信息失败", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.GetTrayInfoFail.ToInt(), StationNum);
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 250;
                        }
                        break;
                    }
                case 350:
                    {
                        var ret = IsCylinderArrive(CylinderName.HalfTrayBufferClawCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->X轴移动到等待位置");
                            MoveAbs(AxisName.HalfTray_BufferX, PosName.WaitPos);//X轴移动到等待位置
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
                        var ret = IsAxisArrive(AxisName.HalfTray_BufferX, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->升降气缸缩回");
                            Cylinder(CylinderName.HalfTrayBufferLiftCyl, false);//升降气缸缩回
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
                        var ret = IsCylinderArrive(CylinderName.HalfTrayBufferLiftCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->缓存Z轴移动到{AxisZDestPos}位置");
                            MoveAbs(AxisName.HalfTray_BufferZ, AxisZDestPos);
                            step = 550;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 400;
                        }
                        break;
                    }

                case 550:
                    {
                        var ret = IsAxisArrive(AxisName.HalfTray_BufferZ, AxisZDestPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->缓存Z轴到达{AxisZDestPos}位置，分离气缸缩回");
                            Cylinder(CylinderName.HalfTrayBufferSplitCyl, false);
                            step = 600;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 500;
                        }
                        break;
                    }
                case 600:
                    {
                        var ret = IsCylinderArrive(CylinderName.HalfTrayBufferSplitCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->分离气缸到达，缓存Z轴移动到等待位置");
                            MoveAbs(AxisName.HalfTray_BufferZ, PosName.WaitPos);
                            step = 650;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 550;
                        }
                        break;
                    }
                case 650:
                    {
                        var ret = IsAxisArrive(AxisName.HalfTray_BufferZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->半Tray入缓存流程结束", MsgType.Notify);
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 600;
                        }
                        break;
                    }
                default:
                    break;
            }
            return step;
        }
        /// <summary>
        /// 半Tray出缓存流程
        /// </summary>
        /// <param name="step"></param>
        /// <param name="isOk"></param>
        /// <returns></returns>
        public int TrayOutBufferFlow(int step, bool isOk, HalfTraySource source = HalfTraySource.HalfTrayStage)
        {
            switch (step)
            {
                case 1:
                    {
                        var vac = isOk ? VacuumName.OKHalfTrayVac : VacuumName.NGHalfTrayVac;
                        ShowMsg($"{StationName}->半Tray出Buffer流程开始，判断{(isOk ? "Ok" : "Ng")}Tray平台上是否有Tray", MsgType.Notify);
                        if (IsVacuumArriveEx(vac, true) == false)
                        {
                            ShowMsg($"{StationName}->Tray平台上没有Tray，判断缓存手臂上是否有Tray");
                            if (Motion.GetDigitalInput(InputName.HalfTrayBufferArmSensor) == false)
                            {
                                DestLayerNo = ProcInfoHelper.Instance.GetTrayBufferLayerNo(PickBinLevel, isOk, source);
                                ShowMsg($"{StationName}->缓存手臂上没有Tray，获取{PickBinLevel}等级在HalfTrayBuffer中的层号:{DestLayerNo}");
                                AxisZDestPos = GetBufferPos(DestLayerNo);
                                MoveAbs(AxisName.HalfTray_BufferZ, AxisZDestPos);//Z轴移动到对应层号位置
                                MoveAbs(AxisName.HalfTray_BufferX, PosName.BufferArmPPTrayFromHalfTrayStagePos);  //X轴移动到从挑补平台取放Tray位置
                                step = 50;
                            }
                            else
                            {
                                var trayInfo = ProcInfoHelper.Instance.GetTrayInfo(TrayPosEnum.HalfTrayBufferArm);
                                if (trayInfo.TrayModel != null)
                                {
                                    ShowMsg($"{StationName}->缓存手臂上有Tray，Tray等级：Bin{trayInfo.TrayModel.BinLevel}，X轴移动到从挑补平台取放Tray位置");
                                    MoveAbs(AxisName.HalfTray_BufferX, PosName.BufferArmPPTrayFromHalfTrayStagePos);//X轴移动到从挑补平台取放Tray位置
                                    step = 300;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->缓存手臂上有Tray，但是获取TrayModel失败", MsgType.Error, true);
                                    AlarmManager.CreateAlarm(AlarmCode.GetTrayInfoFail.ToInt(), StationNum);
                                }
                            }
                        }
                        else
                        {
                            ShowMsg($"{StationName}->OkTray平台上有Tray，半Tray出缓存流程结束", MsgType.Notify);
                            step = 0;
                        }
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArrive(AxisName.HalfTray_BufferZ, AxisZDestPos);
                        var ret2 = IsAxisArrive(AxisName.HalfTray_BufferX, PosName.BufferArmPPTrayFromHalfTrayStagePos);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                        {
                            Cylinders = new List<CylinderName>() {
                                CylinderName.HalfTrayBufferLiftCyl, //升降气缸缩回
                                CylinderName.HalfTrayBufferClawCyl,//取料牙插气缸伸出
                                CylinderName.HalfTrayBufferSplitCyl };//分离气缸伸出
                            CylinderM(Cylinders, true);
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
                        var ret = IsCylinderArriveM(Cylinders, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            MoveAbs(AxisName.HalfTray_BufferZ, PosName.WaitPos);//Z轴下降，分离Tray盘
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
                        var ret = IsAxisArrive(AxisName.HalfTray_BufferZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            MoveAbs(AxisName.HalfTray_BufferX, PosName.BufferArmPPTrayFromBufferPos);//X轴移动到从Buffer取放Tray位置
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
                        var ret = IsAxisArrive(AxisName.HalfTray_BufferX, PosName.BufferArmPPTrayFromBufferPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            Cylinder(CylinderName.HalfTrayBufferLiftCyl, false);//升降气缸上升
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
                        var ret = IsCylinderArrive(CylinderName.HalfTrayBufferLiftCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            var trayModel = ProcInfoHelper.Instance.PickTrayFromHalfTrayBuffer(DestLayerNo);
                            if (trayModel != null)
                            {
                                ProcInfoHelper.Instance.PutTrayToTrayStage(TrayPosEnum.HalfTrayBufferArm, trayModel);
                                MoveAbs(AxisName.HalfTray_BufferX, PosName.BufferArmPPTrayFromHalfTrayStagePos);//X轴移动到从挑补平台取放Tray位置
                                step = 300;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.HalfTrayBufferArmSensor.ToString(), out int _).IsShield = true;
                                    Thread.Sleep(100);
                                }
                            }
                            else
                            {
                                ShowMsg($"{StationName}->从半Tray缓存中获取TrayModel信息失败，等级：{PickBinLevel}", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.GetTrayInfoFromBufferFail.ToInt(), StationNum);
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
                        var ret = IsAxisArrive(AxisName.HalfTray_BufferX, PosName.BufferArmPPTrayFromHalfTrayStagePos);
                        if (ret == MotionStatus.Arrived)
                        {
                            MoveAbs(AxisName.HalfTray_BufferZ, AxisZDestPos);//Z轴上升到取放Tray位置
                            Cylinder(CylinderName.HalfTrayBufferLiftCyl, true);//升降气缸下降
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
                        var ret = IsAxisArrive(AxisName.HalfTray_BufferZ, AxisZDestPos);
                        var ret2 = IsCylinderArrive(CylinderName.HalfTrayBufferLiftCyl, true);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                        {
                            var trayModel = ProcInfoHelper.Instance.PickTrayFromTrayStage(TrayPosEnum.HalfTrayBufferArm);
                            if (trayModel != null)
                            {
                                ProcInfoHelper.Instance.PutTrayToTrayStage(isOk ? TrayPosEnum.OkHalfTrayStage : TrayPosEnum.NgHalfTrayStage, trayModel);
                                Cylinders = new List<CylinderName>() {
                                CylinderName.HalfTrayBufferClawCyl,//取料牙插气缸缩回
                                CylinderName.HalfTrayBufferSplitCyl };//分离气缸缩回
                                CylinderM(Cylinders, false);
                                step = 400;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.HalfTrayBufferArmSensor.ToString(), out int _).IsShield = false;
                                    var inputName = isOk ? InputName.OKHalfTrayVacDetect : InputName.NGHalfTrayVacDetect;
                                    IOInfoHelper.GetInputInfoByName(inputName.ToString(), out int _).IsShield = true;
                                    Thread.Sleep(100);
                                }
                            }
                            else
                            {
                                ShowMsg($"{StationName}->从半Tray缓存Arm中获取TrayModel信息失败", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.GetTrayInfoFail.ToInt(), StationNum);
                            }
                        }
                        else if (ret == MotionStatus.Timeout || ret2 == MotionStatus.Timeout)
                        {
                            step = 300;
                        }
                        break;
                    }
                case 400:
                    {
                        var ret = IsCylinderArriveM(Cylinders, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            Cylinder(CylinderName.HalfTrayBufferLiftCyl, false);//升降气缸上升
                            MoveAbs(AxisName.HalfTray_BufferZ, PosName.WaitPos);//Z轴下降，分离Tray盘
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
                        var ret = IsCylinderArrive(CylinderName.HalfTrayBufferLiftCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            MoveAbs(AxisName.HalfTray_BufferX, PosName.WaitPos);
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
                        var ret = IsAxisArrive(AxisName.HalfTray_BufferX, PosName.WaitPos);
                        var ret2 = IsAxisArrive(AxisName.HalfTray_BufferZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->半Tray出缓存流程结束", MsgType.Notify);
                            step = 0;

                        }
                        else if (ret == MotionStatus.Timeout || ret2 == MotionStatus.Timeout)
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
        /// 取Die流程
        /// </summary>
        /// <param name="step"></param>
        /// <param name="binLevel"></param>
        /// <returns></returns>
        public int PickFlow(int step, BinLevel binLevel)
        {
            switch (step)
            {
                case 1:
                    {
                        if (GetBoolReg(RegisterEnum.PickFlowStart))
                        {
                            ResetBoolReg(RegisterEnum.PickFlowStart);
                            PPCounter = 0;
                            ShowMsg($"{StationName}->挑Die流程开始", MsgType.Notify);
                            ResetBoolReg(RegisterEnum.PickFlowEnd);
                            if (IsOkTrayEmpty)//如果OkTray是空的，直接跳过挑Die流程
                            {
                                ShowMsg($"{StationName}->Ok半Tray为空，不执行挑Die，等待挑补DD马达转动到位");
                                RegisterService.Instance.SetBoolReg(RegisterEnum.ShiftHalfTrayDone);
                                RegisterService.Instance.SetBoolReg(RegisterEnum.OkHalfTrayEmpty);
                                step = 200;
                            }
                            else
                            {
                                RegisterService.Instance.SetBoolReg(RegisterEnum.ShiftHalfTrayDone);
                                if (IsOkTrayAligned)
                                {
                                    ShowMsg($"{StationName}->HalfTray平台已对位过，移动到OkTray基准位置");
                                    Axes = new List<AxisName>() { AxisName.HalfTray_StageX, AxisName.HalfTray_StageY, AxisName.HalfTray_StageT };
                                    Poses = BasePos[0].ToList();
                                    MoveAbsM(Axes, Poses);
                                    step = 20;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->HalfTray平台移动到OkTray拍照位置");
                                    HalfTrayStageMoveStep = 1;
                                    step = 50;
                                }
                            }
                        }
                        break;
                    }
                case 20:
                    {
                        var ret = IsAxisArriveM(Axes, Poses);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->HalfTray平台到达OkTray基准位置，等待挑补DD马达转动到位");
                            step = 200;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1;
                        }
                        break;
                    }
                case 50:
                    {
                        HalfTrayStageMoveStep = HalfTrayToGrabTrayPosFlow(HalfTrayStageMoveStep, true);
                        if (HalfTrayStageMoveStep == 0)
                        {
							ShowMsg($"{StationName}->HalfTray平台到达OkTray拍照位置，发送AlignTray请求给AlignPC，Station：Tray_HalfNgTray");
							var body = AlignHelper.Instance.Send<AlignRspBody>(VisionCmd.Align, AlignStationEnum.Tray_HalfNgTray);
							if (body != null && body.Offset != null)
                            {
                                IsOkTrayAligned = true;
                                ShowMsg($"{StationName}->对位PC返回坐标： {body.Offset.ToString()}");
                                Axes = new List<AxisName>() { AxisName.HalfTray_StageX, AxisName.HalfTray_StageY, AxisName.HalfTray_StageT };
                                Poses = body.Offset.ToList();
                                ShowMsg($"{StationName}->HalfTray平台移动补正值");
                                MoveRelM(Axes, Poses);
                                step = 150;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->AlignPC Tray对位失败", MsgType.Error);
                                AlarmManager.CreateAlarm(AlarmCode.TrayAlignFail.ToInt(), StationNum);
                                var result = AskHowToDoRequest(StationNum, $"{StationName}->AlignPC Tray对位失败，请选择如何处理");
                                switch (result)
                                {
                                    case AskResult.Retry:
                                        {
                                            ShowMsg($"{StationName}->处理结果：重试");
                                            break;
                                        }
                                    case AskResult.Skip:
                                        {
                                            ShowMsg($"{StationName}->处理结果：不对位Tray，继续流程");
                                            step = 150;
                                            break;
                                        }
                                    default:
                                        break;
                                }
                            }
                        }
                        break;
                    }

                case 150:
                    {
                        var ret = IsAxisArriveM(Axes);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->HalfTray平台移动补正值完成，等待挑补DD马达转动到位");
                            CalcFirstDieBasePos(true);
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
                        if (GetBoolReg(RegisterEnum.PickPatchDDRotateDone))
                        {
                            if (IsOkTrayEmpty)
                            {
                                RegisterService.Instance.SetBoolReg(RegisterEnum.OkHalfTrayEmpty);
                                step = 400;
                            }
                            else
                            {
                                CurPickNo = RotateInfoHelper.Instance.GetStageNo(PickPatchStnEnum.HalfTrayStn.ToInt(), RotateStnType.PickPatch);
                                var isExist = false;
                                var pickInfo = ProcInfoHelper.Instance.GetPickerInfo(PickPatchStnEnum.HalfTrayStn, RotateStnType.PickPatch);
                                isExist = IsNozzleVacArrive(CurPickNo) && pickInfo.DieModel != null;
                                if (isExist == false)
                                {
                                    NozzleVacuum(CurPickNo, false);
                                    ShowMsg($"{StationName}->挑补DD马达转动到位，吸嘴：{CurPickNo}无料，获取可挑行列号");
                                    ExistCellRCNo = ProcInfoHelper.Instance.GetAvlDieNoFromTray(TrayPosEnum.OkHalfTrayStage);
                                    if (ExistCellRCNo.R != -1 && ExistCellRCNo.C != -1)
                                    {
                                        ShowMsg($"{StationName}->HalfTray平台移动到第{ExistCellRCNo.R}行，第{ExistCellRCNo.C}列位置");
                                        Axes = new List<AxisName>() { AxisName.HalfTray_StageX, AxisName.HalfTray_StageY, AxisName.HalfTray_StageT };
                                        Poses = GetPerCellPos(ExistCellRCNo.R, ExistCellRCNo.C, true).ToList();
                                        MoveAbsM(Axes, Poses);
                                        step = 250;
                                    }
                                    else
                                    {
                                        ShowMsg($"{StationName}->当前OkTray盘已挑完，通知挑补工位OkTray空Tray");
                                        IsOkTrayEmpty = true;
                                        RegisterService.Instance.SetBoolReg(RegisterEnum.OkHalfTrayEmpty);
                                        step = 400;
                                    }
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->挑补DD马达转动到位，吸嘴：{CurPickNo}有料，Z轴移动到待机位");
                                    MoveAbs(AxisName.PickPatch_PlaceZ, PosName.WaitPos);
                                    PPCounter++;
                                    step = 350;
                                }
                            }
                            ResetBoolReg(RegisterEnum.PickPatchDDRotateDone);
                        }
                        else if (GetBoolReg(RegisterEnum.PickFlowEnd))
                        {
                            ResetBoolReg(RegisterEnum.PickFlowEnd);
                            ShowMsg($"{StationName}->收到正面检工位挑Die结束信号");
                            step = 400;
                        }
                        break;
                    }
                case 250:
                    {
                        var ret = IsAxisArriveM(Axes, Poses);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->HalfTray平台到达目标行列位置");
                            MoveAbs(AxisName.PickPatch_PlaceZ, PosName.PlaceDiePos);
                            NozzleVacuum(CurPickNo, true);
                            Delay.Start(1000);
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
                        var ret = IsAxisArrive(AxisName.PickPatch_PlaceZ, PosName.PlaceDiePos);
                        if (ret == MotionStatus.Arrived)
                        {
                            if (IsNozzleVacArrive(CurPickNo))
                            {
                                ShowMsg($"{StationName}->>吸嘴：{CurPickNo}吸真空完成，更新物流信息，Z轴移动到待机位");
                                var dieModel = ProcInfoHelper.Instance.PickDieFromTray(TrayPosEnum.OkHalfTrayStage, ExistCellRCNo.R, ExistCellRCNo.C);
                                if (dieModel != null)
                                {
                                    CurDieNo++;
                                    PPCounter++;
                                    ProcInfoHelper.Instance.PlaceDieToPicker(PickPatchStnEnum.HalfTrayStn, dieModel, RotateStnType.PickPatch);
                                    MoveAbs(AxisName.PickPatch_PlaceZ, PosName.WaitPos);
                                    step = 350;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->从OkHalfTrayStage吸嘴获取DieModel信息失败", MsgType.Error, true);
                                    AlarmManager.CreateAlarm(AlarmCode.GetDieModelFail.ToInt(), StationNum);
                                }
                            }
                            else if (Delay.TimeUp())
                            {
                                ShowMsg($"{StationName}->>吸嘴：{CurPickNo}吸真空失败", MsgType.Error);
                                var ret2 = AskHowToDoRequest(StationNum, $"{StationName}->弹窗选择如何处理");
                                switch (ret2)
                                {
                                    case AskResult.Retry:
                                        {
                                            ShowMsg($"{StationName}->处理结果：重试");
                                            NozzleVacuum(CurPickNo, true);
                                            Delay.Start(1000);
                                            break;
                                        }
                                    case AskResult.Skip:
                                        {
                                            ShowMsg($"{StationName}->处理结果：跳过");
                                            step = 350;
                                            break;
                                        }
                                    default:
                                        break;
                                }
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 250;
                        }
                        break;
                    }
                case 350:
                    {
                        var ret = IsAxisArrive(AxisName.PickPatch_PlaceZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->Z轴到达待机位，发送HalfTray工位PickDie完成信号，等待挑补DD马达转动到位");
                            RegisterService.Instance.SetBoolReg(RegisterEnum.HalfTrayPickOneDieDone);
                            ShowMsg($"{StationName}->TotalNGCount:{TotalNGCount}，CurDieNo:{CurDieNo}，PPCounter:{PPCounter}");
                            if (CurDieNo < TotalNGCount && PPCounter < 4)//HalfTray工位挑Die，吸嘴索引8到11
                            {
                                step = 200;
                            }
                            else if (CurDieNo == TotalNGCount || PPCounter == 4)
                            {
                                if (CurDieNo == TotalNGCount)
                                {
                                    IsOkTrayAligned = false;
                                    ShowMsg($"{StationName}->正面检工位Tray盘NgDie全部挑补完成");
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->挑补工位4个吸嘴全部满料");
                                }
                                ShowMsg($"{StationName}->收到正面检工位挑Die结束信号");
                                step = 400;
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 300;
                        }
                        break;
                    }
                case 400:
                    {
                        ShowMsg($"{StationName}->挑Die流程结束", MsgType.Notify);
                        PPCounter = 0;
                        step = 0;
                        break;
                    }
                default:
                    break;
            }
            return step;
        }

        /// <summary>
        /// 补Die流程
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int PatchFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        if (GetBoolReg(RegisterEnum.PatchFlowStart))
                        {
                            ResetBoolReg(RegisterEnum.PatchFlowStart);
                            RegisterService.Instance.SetBoolReg(RegisterEnum.PatchFlowReady);
                            ShowMsg($"{StationName}->补Die流程开始", MsgType.Notify);
                            Axes = new List<AxisName>() { AxisName.HalfTray_StageX, AxisName.HalfTray_StageY, AxisName.HalfTray_StageT };
                            if (IsNgTrayAligned)
                            {
                                ShowMsg($"{StationName}->HalfTray平台已对位过，移动到NgTray基准位置");
                                Poses = BasePos[1].ToList();
                                MoveAbsM(Axes, Poses);
                                step = 20;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->HalfTray平台移动到NgTray拍照位置");
                                PosNames = new List<PosName>() { PosName.GrabNgTrayPos };
                                MoveAbsM(Axes, PosNames);
                                step = 50;
                            }
                        }
                        break;
                    }
                case 20:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->HalfTray平台到达NgTray基准位置，等待挑补DD马达转动到位");
                            step = 150;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1;
                        }
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
							ShowMsg($"{StationName}->HalfTray平台移动补正值，发送AlignTray请求给AlignPC，Station：Tray_HalfOkTray");
							var body = AlignHelper.Instance.Send<AlignRspBody>(VisionCmd.Align, AlignStationEnum.Tray_HalfOkTray);
							if (body != null && body.Offset != null)
                            {
                                ShowMsg($"{StationName}->对位PC返回坐标： {body.Offset.ToString()}");
                                ShowMsg($"{StationName}->HalfTray平台移动补正值");
                                Poses = body.Offset.ToList();
                                MoveRelM(Axes, Poses);
                                step = 100;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->AlignPC Tray对位失败", MsgType.Error);
                                AlarmManager.CreateAlarm(AlarmCode.TrayAlignFail.ToInt(), StationNum);
                                var result = AskHowToDoRequest(StationNum, $"{StationName}->AlignPC Tray对位失败，请选择如何处理");
                                switch (result)
                                {
                                    case AskResult.Retry:
                                        {
                                            ShowMsg($"{StationName}->处理结果：重试");
                                            break;
                                        }
                                    case AskResult.Skip:
                                        {
                                            ShowMsg($"{StationName}->处理结果：不对位Tray，继续流程");
                                            step = 100;
                                            break;
                                        }
                                    default:
                                        break;
                                }
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1;
                        }
                        break;
                    }
                case 100:
                    {
                        var ret = IsAxisArriveM(Axes);
                        if (ret == MotionStatus.Arrived)
                        {
                            CalcFirstDieBasePos(false);
                            ShowMsg($"{StationName}->HalfTray平台移动补正值完成，等待挑补DD马达转动到位");
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
                        if (GetBoolReg(RegisterEnum.PickPatchDDRotateDone))
                        {
                            CurPickNo = RotateInfoHelper.Instance.GetStageNo(PickPatchStnEnum.HalfTrayStn.ToInt(), RotateStnType.PickPatch);
                            var pickInfo = ProcInfoHelper.Instance.GetPickerInfo(PickPatchStnEnum.HalfTrayStn, RotateStnType.PickPatch);
                            var isExist = false;
                            isExist = IsNozzleVacArrive(CurPickNo) && pickInfo.DieModel != null;
                            if (isExist)
                            {
                                ShowMsg($"{StationName}->挑补DD马达转动到位，当前吸嘴：{CurPickNo}有料");
                                EmptyCellRCNo = ProcInfoHelper.Instance.GetAvlEmptyCellNoFromTray(TrayPosEnum.NgHalfTrayStage);
                                if (EmptyCellRCNo.R != -1 && EmptyCellRCNo.C != -1)
                                {
                                    ShowMsg($"{StationName}->获取可放Die的空穴位行列号，行：{EmptyCellRCNo.R}，列：{EmptyCellRCNo.C}");
                                    var xyt = GetPerCellPos(EmptyCellRCNo.R, EmptyCellRCNo.C, false);
                                    Poses = xyt.ToList();
                                    ShowMsg($"{StationName}->HalfTray平台移动到空穴位位置");
                                    MoveAbsM(Axes, Poses);
                                    step = 200;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->NgTray满Tray，执行UnloadTray流程");
                                    UnloadTrayStep = 1;
                                    step = 500;
                                }
                            }
                            else
                            {
                                ShowMsg($"{StationName}->挑补DD马达转动到位，当前吸嘴：{CurPickNo}无料，Z轴移动到待机位");
                                NozzleVacuum(CurPickNo, false);
                                MoveAbs(AxisName.PickPatch_PlaceZ, PosName.WaitPos);
                                step = 400;
                            }
                        }
                        break;
                    }
                case 200:
                    {
                        var ret = IsAxisArriveM(Axes, Poses);
                        if (ret == MotionStatus.Arrived)
                        {
                            ResetBoolReg(RegisterEnum.PickPatchDDRotateDone);
							ShowMsg($"{StationName}->HalfTray平台到达空穴位位置，发送AlignDie请求给AlignPC，Station：Die_HalfOkTray");
							if (ParaHelper.Vision.IsAlignBeforeDieToTray)
							{
								var dieIndex = EmptyCellRCNo.R * ParaHelper.Prod.TrayColCount + EmptyCellRCNo.C;
								var body = AlignHelper.Instance.Send<AlignRspBody>(VisionCmd.Align, AlignStationEnum.Die_HalfOkTray, dieIndex);
								if (body != null && body.Offset != null)
                                {
                                    ShowMsg($"{StationName}->对位PC返回坐标： {body.Offset.ToString()}");
                                    Poses = body.Offset.ToList();
                                    ShowMsg($"{StationName}->HalfTray平台移动补正值");
                                    MoveRelM(Axes, Poses);
                                    step = 250;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->AlignPC Die对位失败", MsgType.Error);
                                    AlarmManager.CreateAlarm(AlarmCode.DieAlignFail.ToInt(), StationNum);
                                    var result = AskHowToDoRequest(StationNum, $"{StationName}->AlignPC Die对位失败，请选择如何处理");
                                    switch (result)
                                    {
                                        case AskResult.Retry:
                                            {
                                                ShowMsg($"{StationName}->处理结果：重试");
                                                break;
                                            }
                                        case AskResult.Skip:
                                            {
                                                ShowMsg($"{StationName}->处理结果：不对位Die，继续流程");
                                                step = 250;
                                                break;
                                            }
                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                ShowMsg($"{StationName}->不对位Die，放DieZ轴下降到放Die位置");
                                MoveAbs(AxisName.PickPatch_PlaceZ, PosName.PlaceDiePos);
                                step = 150;
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1;
                        }
                        break;
                    }
                case 250:
                    {
                        var ret = IsAxisArriveM(Axes);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->HalfTray平台移动补正值完成，Z轴下降到放Die位置");
                            MoveAbs(AxisName.PickPatch_PlaceZ, PosName.PlaceDiePos);
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
                        var ret = IsAxisArrive(AxisName.PickPatch_PlaceZ, PosName.PlaceDiePos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->吸嘴：{CurDieNo}破真空");
                            NozzleVacuum(CurPickNo, false);
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
                        if (IsNozzleVacArrive(CurPickNo) == false)
                        {
                            var die = ProcInfoHelper.Instance.PickDieFromPicker(PickPatchStnEnum.HalfTrayStn, RotateStnType.PickPatch);
                            if (die != null)
                            {
                                ShowMsg($"{StationName}->吸嘴：{CurPickNo}破真空完成，更新物流信息，Z轴移动到待机位");
                                ProcInfoHelper.Instance.PlaceDieToTray(TrayPosEnum.NgHalfTrayStage, EmptyCellRCNo.R, EmptyCellRCNo.C, die);
                                MoveAbs(AxisName.PickPatch_PlaceZ, PosName.WaitPos);
                                step = 400;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->从吸嘴：{CurPickNo}获取DieModel失败", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.GetDieModelFromPickPatchDDPickerFail.ToInt(), StationNum);
                            }
                        }
                        break;
                    }
                case 400:
                    {
                        var ret = IsAxisArrive(AxisName.PickPatch_PlaceZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            RegisterService.Instance.SetBoolReg(RegisterEnum.HalfTrayPlaceOneDieDone);
                            ShowMsg($"{StationName}->Z轴到达待机位，等待挑补DD马达转动到位");
                            RemainDieCount--;
                            PPCounter++;
                            ShowMsg($"{StationName}->剩余NG Die数量：{RemainDieCount}");
                            if (PPCounter < 4 && RemainDieCount > 0)
                            {
                                step = 150;
                            }
                            else
                            {
                                PPCounter = 0;
                                if (RemainDieCount == 0)
                                {
                                    IsNgTrayAligned = false;
                                    ShowMsg($"{StationName}->剩余NG Die数量为0，HalfTray工位移动到OkTray基准位");
                                    Axes = new List<AxisName>() { AxisName.HalfTray_StageX, AxisName.HalfTray_StageY, AxisName.HalfTray_StageT };
                                    Poses = BasePos[0].ToList();
                                    MoveAbsM(Axes, Poses);
                                    step = 450;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->补Die结束，挑补平台旋转到OkTray取Die位置", MsgType.Notify);
                                    step = 0;
                                }
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
                        var ret = IsAxisArriveM(Axes, Poses);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->Tray移动到基准位完成，补Die流程结束", MsgType.Notify);
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 400;
                        }
                        break;
                    }
                case 500:
                    {
                        UnloadTrayStep = HalfTrayStageUnloadTrayFlow(UnloadTrayStep, HalfTrayStnEnum.NgTray, false);
                        if (UnloadTrayStep == 0)
                        {
                            ShowMsg($"{StationName}->Ng满Tray下料流程执行结束，执行上空Tray流程");
                            IsNgTrayAligned = false;
                            LoadTrayStep = 1;
                            step = 550;
                        }
                        break;
                    }
                case 550:
                    {
                        LoadTrayStep = HalfTrayStageLoadTrayFlow(LoadTrayStep, HalfTrayStnEnum.NgTray, false);
                        if (LoadTrayStep == 0)
                        {
                            ShowMsg($"{StationName}->LoadTray流程执行结束，HalfTray平台移动到拍Tray位置");
                            HalfTrayStageMoveStep = 1;
                            step = 600;
                        }
                        if (Global.RunMode == RunMode.空跑模式)
                        {
                            IOInfoHelper.GetInputInfoByName(InputName.NGHalfTrayVacDetect.ToString(), out int _).IsShield = true;
                            Thread.Sleep(500);
                        }
                        break;
                    }
                case 600:
                    {
                        HalfTrayStageMoveStep = HalfTrayToGrabTrayPosFlow(HalfTrayStageMoveStep, false);
                        if (HalfTrayStageMoveStep == 0)
                        {
                            ShowMsg($"{StationName}->HalfTray平台移动到NgTray拍照位置完成");
                            step = 50;
                        }
                        break;
                    }

                default:
                    break;
            }
            return step;
        }


        /// <summary>
        /// FullTrayArm从半Tray缓存取Tray流程
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int FullTrayArmPickTrayFromBufferFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        var value = true;
                        if (IsVacuumArriveEx(VacuumName.OKHalfTrayVac, true))
                        {
                            var binLevel = RegisterService.Instance.GetIntValue(IntValueEnum.ShiftBinLevel);
                            var trayInfo = ProcInfoHelper.Instance.GetTrayInfo(TrayPosEnum.OkHalfTrayStage);
                            if (trayInfo.TrayModel != null && trayInfo.TrayModel.BinLevel != binLevel)
                            {
                                ShowMsg($"{StationName}->OkTray平台有Tray，但BinLevel与当前BinLevel不一致，不允许从半Tray缓存取Tray");
                                value = false;
                            }
                        }
                        if (value)
                        {
                            ShowMsg($"{StationName}->FullTrayArm从半Tray缓存取Tray流程开始，OkTray平台移动到取放Tray位置", MsgType.Notify);
                            HalfTrayStageMoveStep = 1;
                            step = 50;
                        }
                        else
                        {
                            step = -1;
                        }
                        break;
                    }
                case 50:
                    {
                        HalfTrayStageMoveStep = HalfTrayToLulPosFlow(HalfTrayStageMoveStep, true);
                        if (HalfTrayStageMoveStep == 0)
                        {
                            ShowMsg($"{StationName}->OkTray平台到达取放Tray位置，执行从半Tray缓存取Tray流程");
                            TrayOutBufferStep = 1;
                            step = 100;
                        }
                        break;
                    }
                case 100:
                    {
                        TrayOutBufferStep = TrayOutBufferFlow(TrayOutBufferStep, true, HalfTraySource.Stn4);
                        if (TrayOutBufferStep == 0)
                        {
                            RegisterService.Instance.SetBoolReg(RegisterEnum.OkHalfTrayStageAllowPickTray);
                            ShowMsg($"{StationName}->半Tray出缓存流程结束，通知满Tray手臂取Tray");
                            step = 150;

                            if (Global.RunMode == RunMode.空跑模式)
                            {
                                IOInfoHelper.GetInputInfoByName(InputName.OKHalfTrayVacDetect.ToString(), out int _).IsShield = true;
                                Thread.Sleep(500);
                            }
                        }
                        break;
                    }
                case 150:
                    {
                        if (GetBoolReg(RegisterEnum.FullTrayArmPickTrayFromOkHalfTrayStageDone))
                        {
                            ShowMsg($"{StationName}->满Tray手臂从Ok半Tray平台取Tray流程结束");
                            ResetBoolReg(RegisterEnum.FullTrayArmPickTrayFromOkHalfTrayStageDone);
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
        /// 半Tray平台移动到取放半Tray位置流程
        /// </summary>
        /// <param name="step"></param>
        /// <param name="isOkTray">true：OK半Tray。false：NG半Tray</param>
        /// <returns></returns>
        public int HalfTrayToLulPosFlow(int step, bool isOkTray)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->半Tray平台移动到取放半Tray位置流程开始", MsgType.Notify);
                        if (Motion.IsArrive(AxisName.HalfTray_Y, PosName.HalfTrayYLulTrayPos))
                        {
                            ShowMsg($"{StationName}->大Y轴在取放半Tray位置，XYT轴移动到取放{(isOkTray ? "Ok" : "Ng")}半Tray位置");
                            Axes = new List<AxisName>() { AxisName.HalfTray_StageX, AxisName.HalfTray_StageY, AxisName.HalfTray_StageT };
                            PosNames = new List<PosName>() { isOkTray ? PosName.OkTrayLuloadPos : PosName.NgTrayLuloadPos };//OkTray下料位
                            MoveAbsM(Axes, PosNames);
                            step = 150;
                        }
                        else
                        {
                            ShowMsg($"{StationName}->大Y轴不在取放半Tray位置，HalfTray平台XY移动到避让位置");
                            Axes = new List<AxisName>() { AxisName.HalfTray_StageX, AxisName.HalfTray_StageY };
                            PosNames = new List<PosName>() { PosName.HalfTrayStageSafePos };//OkTray下料位
                            MoveAbsM(Axes, PosNames);
                            step = 50;
                        }
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->HalfTray平台XY到达避让位置，大Y轴移动到取放半Tray位置");
                            Axes = new List<AxisName>() { AxisName.HalfTray_Y };
                            PosNames = new List<PosName>() { PosName.HalfTrayYLulTrayPos };//OkTray下料位
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
                            ShowMsg($"{StationName}->大Y轴到达取放半Tray位置，XYT轴移动到取放{(isOkTray ? "Ok" : "Ng")}半Tray位置");
                            Axes = new List<AxisName>() { AxisName.HalfTray_StageX, AxisName.HalfTray_StageY, AxisName.HalfTray_StageT };
                            PosNames = new List<PosName>() { isOkTray ? PosName.OkTrayLuloadPos : PosName.NgTrayLuloadPos };//OkTray下料位
                            MoveAbsM(Axes, PosNames);
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
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->半Tray平台移动到取放半Tray位置流程结束", MsgType.Notify);
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 150;
                        }
                        break;
                    }
                default:
                    break;
            }
            return step;
        }
        /// <summary>
        /// 半Tray平台移动到拍照位置流程
        /// </summary>
        /// <param name="step"></param>
        /// <param name="isOkTray">true：OK半Tray。false：NG半Tray</param>
        /// <returns></returns>
        public int HalfTrayToGrabTrayPosFlow(int step, bool isOkTray)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->半Tray平台移动到拍Tray位置流程开始", MsgType.Notify);
                        if (Motion.IsArrive(AxisName.HalfTray_Y, PosName.HalfTrayYGrabTrayPos))
                        {
                            ShowMsg($"{StationName}->大Y轴在取放Die位置，XYT轴移动到取放{(isOkTray ? "Ok" : "Ng")}半Tray位置");
                            Axes = new List<AxisName>() { AxisName.HalfTray_StageX, AxisName.HalfTray_StageY, AxisName.HalfTray_StageT };
                            PosNames = new List<PosName>() { isOkTray ? PosName.GrabOkTrayPos : PosName.GrabNgTrayPos };
                            MoveAbsM(Axes, PosNames);
                            step = 150;
                        }
                        else
                        {
                            ShowMsg($"{StationName}->大Y轴不在取放Die位置，HalfTray平台XY移动到避让位置");
                            Axes = new List<AxisName>() { AxisName.HalfTray_StageX, AxisName.HalfTray_StageY };
                            PosNames = new List<PosName>() { PosName.HalfTrayStageSafePos };
                            MoveAbsM(Axes, PosNames);
                            step = 50;
                        }
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->HalfTray平台XY到达避让位置，大Y轴移动到取放半Tray位置");
                            Axes = new List<AxisName>() { AxisName.HalfTray_Y };
                            PosNames = new List<PosName>() { PosName.HalfTrayYGrabTrayPos };
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
                            ShowMsg($"{StationName}->大Y轴到达取放半Tray位置，XYT轴移动到取放{(isOkTray ? "Ok" : "Ng")}半Tray位置");
                            Axes = new List<AxisName>() { AxisName.HalfTray_StageX, AxisName.HalfTray_StageY, AxisName.HalfTray_StageT };
                            PosNames = new List<PosName>() { isOkTray ? PosName.GrabOkTrayPos : PosName.GrabNgTrayPos };
                            MoveAbsM(Axes, PosNames);
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
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->半Tray平台移动到拍Tray位置流程结束", MsgType.Notify);
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 150;
                        }
                        break;
                    }
                default:
                    break;
            }
            return step;
        }
        /// <summary>
        /// 计算首颗Die的基准位置
        ///   _______________
        ///   |                     |
        ///   |                     |
        ///   |                     |
        ///   |                     |
        ///   |                     |
        ///   |                 □ /
        ///   ¯¯¯¯¯¯¯¯¯¯¯¯¯¯
        /// </summary>
        private void CalcFirstDieBasePos(bool isOkTray)
        {
            var trayNo = isOkTray ? 0 : 1;
            BasePos[trayNo].X = Motion.GetAxisActPos(AxisName.HalfTray_StageX);
            BasePos[trayNo].Y = Motion.GetAxisActPos(AxisName.HalfTray_StageY);
            BasePos[trayNo].T = Motion.GetAxisActPos(AxisName.HalfTray_StageT);
            //FirstCellPos[trayNo].X = BasePos[trayNo].X - ParaHelper.Prod.FirstCellOffsetX;//计算Tray第一个穴位的X方向坐标
            //FirstCellPos[trayNo].Y = BasePos[trayNo].Y - ParaHelper.Prod.FirstCellOffsetY;//计算Tray第一个穴位的Y方向坐标
            FirstCellPos[trayNo].X = BasePos[trayNo].X ;//计算Tray第一个穴位的X方向坐标
            FirstCellPos[trayNo].Y = BasePos[trayNo].Y ;//计算Tray第一个穴位的Y方向坐标
        }
        /// <summary>
        /// 获取指定行列号穴位的坐标位置
        /// </summary>
        /// <param name="rowNo"></param>
        /// <param name="colNo"></param>
        /// <param name="isOkTray"></param>
        /// <returns></returns>
        private Xyt GetPerCellPos(int rowNo, int colNo, bool isOkTray)
        {
            Xyt ret = new Xyt();
            //var x = FirstCellPos[isOkTray ? 0 : 1].X - colNo * ParaHelper.Prod.CellPitchX;//挑补平台首颗Die在Tray的右下角
            //var y = FirstCellPos[isOkTray ? 0 : 1].Y - rowNo * ParaHelper.Prod.CellPitchY;
            var x = FirstCellPos[isOkTray ? 0 : 1].Y + colNo * ParaHelper.Prod.CellPitchX;//挑补平台首颗Die在Tray的坐上角
            var y = FirstCellPos[isOkTray ? 0 : 1].X + rowNo * ParaHelper.Prod.CellPitchY;
            return ret;
        }
        /// <summary>
        /// 获取半Tray缓存Z轴位置
        /// </summary>
        /// <param name="layerNo"></param>
        /// <returns></returns>
        public double GetBufferPos(int layerNo)
        {
            var pos = AxisInfoHelper.GetPosInfoByName(AxisName.HalfTray_BufferZ, PosName.FirstLayerPos).Pos;
            return pos + layerNo * ParaHelper.Prod.TrayBufferPitch;
        }
        public void Init()
        {
            Init(StationNum, StationName);
        }
        /// <summary>
        /// 吸嘴真空控制
        /// </summary>
        /// <param name="nozzleNo"></param>
        /// <param name="value"></param>
        private void NozzleVacuum(int nozzleNo, bool value)
        {
            nozzleNo = 1;
            var ran = new Random();
            VacuumName vacuumName = EnumHelper.GetEnumValue<VacuumName>($"PickPatchNozzle{nozzleNo + 1}Vac");
            if (Global.RunMode == RunMode.空跑模式)
            {
                if (value)
                {
                    IOInfoHelper.GetAnalogInputInfoByName($"PickPatchNozzle{nozzleNo + 1}Value").AIValue = ran.Next(1100, 1500);
                }
                else
                {
                    IOInfoHelper.GetAnalogInputInfoByName($"PickPatchNozzle{nozzleNo + 1}Value").AIValue = ran.Next(100, 200);
                }
                Thread.Sleep(20);
            }
            Vacuum(vacuumName, value);
        }
        /// <summary>
        /// 吸嘴真空到位检测
        /// </summary>
        /// <param name="nozzleNo"></param>
        /// <returns></returns>
        private bool IsNozzleVacArrive(int nozzleNo)
        {
            nozzleNo = 1;
            var inputName = $"PickPatchNozzle{nozzleNo + 1}Value";
            //var value = Motion.GetAnalogInput(inputName);
            var value = IOInfoHelper.GetAnalogInputInfoByName(inputName).AIValue;
            return value > 1000;
        }
        /// <summary>
        /// 判断半Tray平台是否到达取放半Tray位置
        /// </summary>
        /// <param name="isOkTray"></param>
        /// <returns></returns>
        public static bool IsHalfTrayStageArriveLulPos(bool isOkTray)
        {
            MotionManager motionManager = MotionManager.GetInstance();
            //判断半Tray平台是否到达取放半Tray位置
            if (motionManager.IsArrive(AxisName.HalfTray_Y, PosName.HalfTrayYLulTrayPos))
            {
                var posName = isOkTray ? PosName.OkTrayLuloadPos : PosName.NgTrayLuloadPos;
                if (motionManager.IsArrive(AxisName.HalfTray_StageX, posName) &&
                    motionManager.IsArrive(AxisName.HalfTray_StageY, posName) &&
                    motionManager.IsArrive(AxisName.HalfTray_StageT, posName))
                {
                    return true;
                }
            }
            return false;
        }
        public void OnButtonTrigger(IOInfo io, bool sts)
        {
        }

        public void PauseHandle()
        {
        }

        public void RegisterEvent()
        {
            var stn4TurnTableStn = StationManager.Instance.GetChild<Stn4TurnTableStn>(nameof(Stn4TurnTableStn));
            stn4TurnTableStn.ResetRegisterEvent += Stn_ResetRegisterEvent;
            var pickPatchStn = StationManager.Instance.GetChild<PickPatchStn>(nameof(PickPatchStn));
            pickPatchStn.ResetRegisterEvent += Stn_ResetRegisterEvent;
        }

        private void Stn_ResetRegisterEvent(RegisterEnum obj)
        {
            if (obj == RegisterEnum.Stn4DDRotateDone)
            {
                LastRegisterValue[obj.ToInt()] = false;
            }
        }
        public void ResetBits()
        {
            ModuleMoveStep = -1;
            UnloadTrayStep = -1;
            LoadTrayStep = -1;
            PickStep = -1;
            PatchStep = -1;
            TrayInBufferStep = -1;
            TrayOutBufferStep = -1;
        }

        public void StopHandle()
        {
        }

        public void WaitFlowDone()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                if (PatchStep < 350 || PatchStep > 400)
                {
                    return;
                }
                else if (sw.ElapsedMilliseconds > 2000)
                {
                    return;
                }
                Thread.Sleep(5);
            }
        }
    }
}
