using JC.PTIS.Main.AlarmInfo;
using JC.PTIS.Main.Devices.Printer;
using JC.PTIS.Main.Devices.Vision;
using JC.PTIS.Main.Devices.VisionPC.MsgModel;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.FlowInfo;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Params;
using JC.PTIS.Main.Service;
using Microsoft.Win32;
using SqlSugar.Extensions;
using StdLib.HTAlarm;
using StdLib.LogHelper;
using StdLib.PCBaseFrame.Alarm;
using StdLib.PCBaseFrame.Args;
using StdLib.PCBaseFrame.Entities;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.PCBaseFrame.Motion;
using StdLib.PCBaseFrame.Station;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Stations
{
    public class PackingStn : StationBaseEx, IStation
    {
        public int StationNum { get; set; } = 15;
        public string StationName { get; set; } = "PackingStn";


        List<AxisName> Axes = new List<AxisName>();
        List<PosName> PosNames = new List<PosName>();
        List<double> Poses = new List<double>();
        LotCellBase AvlFullLotCellModel = null;
        Xyt AlignOffset = null;
        LotStageInfo CurLotInfo = null;
        int SubFlowStep = -1;
        int PutLotToPackingStageStep = -1;

        List<CylinderName> CylinderNameList = new List<CylinderName>();


        int PackingNo = 0;
        int ActPickTrayCount = -1;

        (int r, int c) EmptySlotRCNo = (-1, -1);
        public void Home()
        {
            try
            {
                switch (HomeStep)
                {
                    case 1:
                        {
                            ShowMsg($"{StationName}->开始回零");
                            if (Motion.GetDigitalInput(InputName.LotTransferArmDetect))
                            {
                                ShowMsg($"{StationName}-> Lot夹爪上有Lot，禁止回零", MsgType.Error);
                                AlarmManager.CreateAlarm(AlarmCode.ClawExistLot.ToInt(), AllStationAlarm);
                                HomeStep = 1000;
                            }
                            else
                            {
                                CylinderNameList = new List<CylinderName>() { CylinderName.PickLabelCyl, CylinderName.PackerTransferCyl, CylinderName.LabelLiftCyl };
                                CylinderM(CylinderNameList, false);
                                ShowMsg($"{StationName}->{CylinderName.PickLabelCyl}气缸、{CylinderName.PackerTransferCyl}气缸、{CylinderName.LabelLiftCyl}气缸缩回");
                                HomeStep = 50;
                            }
                            break;
                        }
                    case 50:
                        {
                            var ret = IsCylinderArriveM(CylinderNameList, false);
                            if (ret == MotionStatus.Arrived)
                            {
                                Motion.Home(AxisName.Packing_Shift);
                                //MoveRel(AxisName.Packing_LabelZ, 5);//标签Z轴抬起5mm，防止碰撞
                                Motion.Home(AxisName.Packing_ArmZ);
                                ShowMsg($"{StationName}->Packing_Shift轴和Packing_ArmZ轴开始回零");
                                HomeStep = 100;
                            }
                            else if (ret == MotionStatus.Timeout)
                            {
                                ShowMsg($"{StationName}->{CylinderName.PickLabelCyl}气缸、{CylinderName.LabelLiftCyl}气缸{CylinderName.PackerTransferCyl}气缸、{CylinderName.LabelLiftCyl}气缸缩回超时", MsgType.Error);
                                HomeStep = 1000;
                            }
                            break;
                        }
                    case 100:
                        {
                            if (Motion.IsHomeDone(AxisName.Packing_ArmZ) &&
                                    Motion.IsHomeDone(AxisName.Packing_Shift))
                            {
                                ShowMsg($"{StationName}->Packing_ArmZ轴和Packing_Shift回零完成");
                                Motion.Home(AxisName.Packing_ArmY);
                                ShowMsg($"{StationName}->Packing_ArmY轴开始回零");
                                HomeStep = 150;
                            }
                            else if (Motion.IsMoveTimeOut(AxisName.Packing_Shift))
                            {
                                ShowMsg($"{StationName}->Packing_Shift轴回零超时", MsgType.Error, true);
                                HomeStep = 1000;
                            }
                            else if (Motion.IsMoveTimeOut(AxisName.Packing_ArmZ))
                            {
                                ShowMsg($"{StationName}->Packing_ArmZ轴回零超时", MsgType.Error, true);
                                HomeStep = 1000;
                            }
                            break;
                        }

                    case 150:
                        {
                            if (Motion.IsHomeDone(AxisName.Packing_ArmY))
                            {
                                ShowMsg($"{StationName}->Packing_ArmY轴回零完成");
                                Motion.Home(AxisName.Packing_ArmX);
                                Motion.Home(AxisName.Packing_ArmT);
                                ShowMsg($"{StationName}->Packing_ArmX轴和AxisName.Packing_ArmT轴开始回零");
                                HomeStep = 200;
                            }
                            else if (Motion.IsMoveTimeOut(AxisName.Packing_ArmY))
                            {
                                ShowMsg($"{StationName}->Packing_ArmY轴回零超时", MsgType.Error, true);
                                HomeStep = 1000;
                            }
                            break;
                        }
                    case 200:
                        {
                            if (Motion.IsHomeDone(AxisName.Packing_ArmX) &&
                                    Motion.IsHomeDone(AxisName.Packing_ArmT))
                            {
                                ShowMsg($"{StationName}->{AxisName.Packing_ArmX}轴{AxisName.Packing_ArmT}轴回零完成");
                                Motion.MoveAbs(AxisName.Packing_ArmX, PosName.WaitPos);
                                HomeStep = 250;
                            }
                            else if (Motion.IsMoveTimeOut(AxisName.Packing_ArmX))
                            {
                                ShowMsg($"{StationName}->Packing_ArmX轴回零超时", MsgType.Error, true);
                                HomeStep = 1000;
                            }
                            else if (Motion.IsMoveTimeOut(AxisName.Packing_ArmT))
                            {
                                ShowMsg($"{StationName}->Packing_ArmT轴回零超时", MsgType.Error, true);
                                HomeStep = 1000;
                            }
                            break;
                        }
                    case 250:
                        {
                            if (Motion.IsArrive(AxisName.Packing_ArmX, PosName.WaitPos))
                            {
                                ShowMsg($"{StationName}->Packing_ArmX轴到达{PosName.WaitPos}位置");
                                Motion.Home(AxisName.Packing_LabelZ);
                                ShowMsg($"{StationName}->Packing_LabelZ轴开始回零");
                                HomeStep = 300;
                            }
                            break;
                        }
                    case 300:
                        {
                            if (Motion.IsHomeDone(AxisName.Packing_LabelZ))
                            {
                                ShowMsg($"{StationName}->Packing_LabelZ轴回零完成");
                                UpdateHomeSts(StationName, StationNum, true);
                                HomeStep = 0;
                            }
                            else if (Motion.IsMoveTimeOut(AxisName.Packing_LabelZ))
                            {
                                ShowMsg($"{StationName}->Packing_LabelZ轴回零超时", MsgType.Error, true);
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
                LogManager.GetLogger("Error").Error($"[PachingStn][Home]方法执行异常，异常原因：{ex}");
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
                            ShowMsg($"{StationName}->执行模组回待机位流程");
                            SubFlowStep = 1;
                            AutoRunStep = 50;
                            break;
                        }
                    case 50:
                        {
                            SubFlowStep = MoveToWaitPosFlow(SubFlowStep);
                            if (SubFlowStep == 0)
                            {
                                ShowMsg($"{StationName}->模组回待机位流程结束，判断束带工位和夹爪上是否有Lot");
                                AutoRunStep = 100;
                            }
                            break;
                        }
                    case 100:
                        {
                            if (Motion.GetDigitalInput(InputName.PackingStageLotDetect))
                            {
                                ShowMsg($"{StationName}->束带工位有Lot，判断束带是否完成");
                                AutoRunStep = 150;
                            }
                            if (Motion.GetDigitalInput(InputName.LotTransferArmDetect))
                            {
                                ShowMsg($"{StationName}->夹爪上有Lot，判断Lot状态");
                                AutoRunStep = 500;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->束带工位没有Lot，等待8工位转盘信号请求");
                                AutoRunStep = 350;
                            }
                            break;
                        }
                    case 150:
                        {
                            CurLotInfo = ProcInfoHelper.Instance.GetLotStageInfo(LotPosEnum.PackingStage);
                            if (CurLotInfo != null && CurLotInfo.LotModel != null)
                            {
                                switch (CurLotInfo.LotModel.LotStatus)
                                {
                                    case LotStatusEnum.PasteLabelDone:
                                        {
                                            ShowMsg($"{StationName}->当前Lot束带和贴标签完成，执行从束带平台取Lot流程");
                                            SubFlowStep = 1;
                                            AutoRunStep = 170;
                                        }
                                        break;
                                    case LotStatusEnum.Belt1PackingDone:
                                    case LotStatusEnum.Belt2PackingDone:
                                    case LotStatusEnum.Belt1DetectDone:
                                        {
                                            ShowMsg($"{StationName}->当前Lot束带未完成，继续束带");
                                            SubFlowStep = 1;
                                            AutoRunStep = 300;
                                        }
                                        break;
                                    case LotStatusEnum.Belt2DetectDone:
                                        {
                                            ShowMsg($"{StationName}->当前Lot束带全部完成，执行贴标签流程");
                                            SubFlowStep = 1;
                                            AutoRunStep = 250;
                                        }
                                        break;
                                    case LotStatusEnum.FullTrayLot:
                                        {
                                            ShowMsg($"{StationName}->当前Lot还未束带，执行束带流程");
                                            SubFlowStep = 1;
                                            AutoRunStep = 300;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                ShowMsg($"{StationName}->获取PackingStage平台上的Lot信息失败");
                                AlarmManager.CreateAlarm(AlarmCode.GetLotModelFail.ToInt(), AllStationAlarm);
                            }
                            break;
                        }
                    case 170:
                        {
                            SubFlowStep = PickFullLotFromPackingStageFlow(SubFlowStep);
                            if (SubFlowStep == 0)
                            {
                                ShowMsg($"{StationName}->从束带平台取Lot完成，执行放满Lot到LotBox流程");
                                SubFlowStep = 1;
                                AutoRunStep = 200;
                            }
                            break;
                        }
                    case 200:
                        {
                            SubFlowStep = PutFullLotToLotBoxFlow(SubFlowStep);
                            if (SubFlowStep == 0)
                            {
                                ShowMsg($"{StationName}->放满Lot到LotBox流程结束");
                                AutoRunStep = 100;
                            }
                            break;
                        }
                    case 250:
                        {
                            SubFlowStep = PasteLabelFlow(SubFlowStep);
                            if (SubFlowStep == 0)
                            {
                                ShowMsg($"{StationName}->贴标签完成，执行标签检测流程");
                                SubFlowStep = 1;
                                AutoRunStep = 270;
                            }
                            break;
                        }
                    case 270:
                        {
                            SubFlowStep = LabelDetectFlow(SubFlowStep);
                            if (SubFlowStep == 0)
                            {
                                ShowMsg($"{StationName}->标签检测流程完成，执行从束带平台取Lot流程");
                                SubFlowStep = 1;
                                AutoRunStep = 170;
                            }
                            break;
                        }
                    case 300:
                        {
                            SubFlowStep = PackingLotFlow(SubFlowStep);
                            if (SubFlowStep == 0)
                            {
                                ShowMsg($"{StationName}->束带流程结束，执行贴标签流程");
                                SubFlowStep = 1;
                                AutoRunStep = 250;
                            }
                            break;
                        }
                    case 350:
                        {
                            if (GetBoolReg(RegisterEnum.Stn8TurnTableUnloadFullLotReq))
                            {
                                ShowMsg($"{StationName}->收到8工位转盘下Lot请求，执行从8工位转盘取Lot流程");
                                ResetBoolReg(RegisterEnum.Stn8TurnTableUnloadFullLotReq);
                                SubFlowStep = 1;
                                AutoRunStep = 400;
                            }
                            else if (GetBoolReg(RegisterEnum.Stn8TurnTableLoadEmptyLotReq))
                            {
                                ShowMsg($"{StationName}->收到8工位转盘上Lot请求");
                                ResetBoolReg(RegisterEnum.Stn8TurnTableLoadEmptyLotReq);
                                SubFlowStep = 1;
                                AutoRunStep = 450;
                            }
                            break;
                        }
                    case 400:
                        {
                            SubFlowStep = PickFullLotFromStn8Flow(SubFlowStep);
                            if (SubFlowStep == 0)
                            {
                                RegisterService.Instance.SetBoolReg(RegisterEnum.LotBoxStnUnloadFullLotDone);
                                ShowMsg($"{StationName}->从8工位转盘取Lot流程结束，执行束带流程");
                                SubFlowStep = 1;
                                AutoRunStep = 300;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    //AutoRunStep = 200;
                                }
                            }
                            break;
                        }
                    case 450:
                        {
                            SubFlowStep = PickEmptyLotFromLotBoxFlow(SubFlowStep);
                            if (SubFlowStep == 0)
                            {
                                ShowMsg($"{StationName}->取空Lot到8工位转盘流程结束");
                                AutoRunStep = 100;
                            }
                            break;
                        }
                    case 500:
                        {
                            CurLotInfo = ProcInfoHelper.Instance.GetLotStageInfo(LotPosEnum.LotArmClaw);
                            if (CurLotInfo != null && CurLotInfo.LotModel != null)
                            {
                                switch (CurLotInfo.LotModel.LotStatus)
                                {
                                    case LotStatusEnum.EmptyTrayLot:
                                        {
                                            ShowMsg($"{StationName}->夹爪上有空TrayLot，等待8工位转盘请求上空TrayLot");
                                            AutoRunStep = 350;
                                        }
                                        break;
                                    case LotStatusEnum.FullTrayLot:
                                        {
                                            ShowMsg($"{StationName}->夹爪上有满TrayLot，还未束带，执行放Lot到束带平台流程");
                                            SubFlowStep = 1;
                                            AutoRunStep = 550;
                                        }
                                        break;
                                    case LotStatusEnum.PasteLabelDone:
                                        {
                                            ShowMsg($"{StationName}->夹爪上有满TrayLot，束带和贴标签完成，执行放满Lot到LotBox流程");
                                            SubFlowStep = 1;
                                            AutoRunStep = 200;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                ShowMsg($"{StationName}->获取PackingStage平台上的Lot信息失败");
                                AlarmManager.CreateAlarm(AlarmCode.GetLotModelFail.ToInt(), AllStationAlarm);
                            }
                            break;
                        }
                    case 550:
                        {
                            SubFlowStep = PutLotToPackingStageFlow(SubFlowStep);
                            if (SubFlowStep == 0)
                            {
                                ShowMsg($"{StationName}->放Lot到束带平台流程结束，执行束带流程");
                                SubFlowStep = 1;
                                AutoRunStep = 300;
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
        /// 贴标签流程
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int PasteLabelFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->贴标签流程开始，标签Z轴抬起5mm", MsgType.Notify);
                        MoveAbs(AxisName.Packing_LabelZ, 5);//标签Z轴抬起5mm，防止碰撞
                        Thread.Sleep(20);
                        step = 20;
                        break;
                    }
                case 20:
                    {
                        var ret = IsAxisArrive(AxisName.Packing_LabelZ);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->标签Z轴抬升到位，取标签气缸缩回");
                            Cylinder(CylinderName.PickLabelCyl, false);//取标签气缸缩回
                            step = 50;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1;
                        }
                        break;
                    }
                case 50:
                    {
                        var ret = IsCylinderArrive(CylinderName.PickLabelCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->取标签气缸缩回完成，判断吸标签气缸是否有标签");
                            if (Motion.IsVacuumArrive(VacuumName.LabelCylVac, true))
                            {
                                ShowMsg($"{StationName}->吸标签气缸上有标签，标签Z轴上升到对位平台上方位置");
                                MoveAbs(AxisName.Packing_LabelZ, PosName.LabelPreAlignPos);
                                step = 250;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->吸标签气缸上没有标签，判断标签平台上有无标签");
                                if (Motion.GetDigitalInput(InputName.LabelExistDetect))
                                {
                                    ShowMsg($"{StationName}->标签平台上有标签，取标签气缸伸出");
                                    Cylinder(CylinderName.PickLabelCyl, true);//取标签气缸伸出
                                    step = 100;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->标签平台上没有标签");
                                    step = 60;

                                    if (Global.RunMode == RunMode.空跑模式)
                                    {
                                        IOInfoHelper.GetInputInfoByName(InputName.LabelExistDetect.ToString(), out int _).IsShield = true;
                                        Thread.Sleep(100);
                                    }
                                }
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1;
                        }
                        break;
                    }
                case 60:
                    {
                        ShowMsg($"{StationName}->请求标签机出标签");
                        var ret = PrinterHelper.Instance.Print();
                        if (ret)
                        {
                            ShowMsg($"{StationName}->标签机出标签完成");
                            step = 80;
                        }
                        else
                        {
                            ShowMsg($"{StationName}->标签机出标签失败", MsgType.Error);
                            AlarmManager.CreateAlarm(AlarmCode.PrintLabelFail.ToInt(), StationNum);
                        }
                        break;
                    }
                case 80:
                    {
                        ShowMsg($"{StationName}->标签机出标签完成，判断出标签是否成功");
                        if (Motion.GetDigitalInput(InputName.LabelExistDetect))
                        {
                            ShowMsg($"{StationName}->标签平台上有标签，取标签气缸伸出");
                            Cylinder(CylinderName.PickLabelCyl, true);//取标签气缸伸出
                            step = 100;
                        }
                        else
                        {
                            ShowMsg($"{StationName}->标签平台上没有标签，标签机出标签失败");
                            AlarmManager.CreateAlarm(AlarmCode.PrintLabelFail.ToInt(), StationNum);
                            step = 60;
                        }
                        break;
                    }
                case 100:
                    {
                        var ret = IsCylinderArrive(CylinderName.PickLabelCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->取标签气缸伸出完成，模组移动到取标签位置");
                            MoveAbs(AxisName.Packing_LabelZ, PosName.PickLabelFromStagePos);//Z轴下降到取标签位置
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
                        var ret = IsAxisArrive(AxisName.Packing_LabelZ, PosName.PickLabelFromStagePos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移动到取标签位置完成，取标签真空吸附");
                            Vacuum(VacuumName.LabelCylVac, true);//取标签真空吸附
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
                        var ret = IsVacuumArrive(VacuumName.LabelCylVac, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->取标签气缸缩回完成，Z轴上升5mm");
                            MoveRel(AxisName.Packing_LabelZ, 5);//Z轴上升5mm，防止碰撞
                            step = 220;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 150;
                        }
                        break;
                    }
                case 220:
                    {
                        var ret = IsAxisArrive(AxisName.Packing_LabelZ);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->Z轴上升5mm完成，取标签气缸缩回");
                            Cylinder(CylinderName.PickLabelCyl, false);//取标签气缸缩回
                            step = 230;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 150;
                        }
                        break;
                    }
                case 230:
                    {
                        var ret = IsCylinderArrive(CylinderName.PickLabelCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->取标签气缸缩回完成，模组移动到贴标签位置");
                            MoveAbs(AxisName.Packing_LabelZ, PosName.LabelPreAlignPos);
                            step = 250;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 200;
                        }
                        break;
                    }
                case 250:
                    {
                        var ret = IsAxisArrive(AxisName.Packing_LabelZ, PosName.LabelPreAlignPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移动到贴标签位置完成，贴标签气缸伸出");
                            Cylinder(CylinderName.PickLabelCyl, true);//贴标签气缸伸出
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
                        var ret = IsCylinderArrive(CylinderName.PickLabelCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->贴标签气缸伸出完成，标签Z轴下降到放标签位置");
                            MoveAbs(AxisName.Packing_LabelZ, PosName.PutLabelToAlignStagePos);
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
                        var ret = IsAxisArrive(AxisName.Packing_LabelZ, PosName.PutLabelToAlignStagePos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->标签Z轴下降到放标签位置完成，对位平台真空吸附");
                            Vacuum(VacuumName.LabelAlignStageVac, true);
                            Vacuum(VacuumName.LabelCylVac, false);
                            step = 380;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 300;
                        }
                        break;
                    }
                case 380:
                    {
                        var ret = IsVacuumArrive(VacuumName.LabelCylVac, false);
                        var ret2 = IsVacuumArrive(VacuumName.LabelAlignStageVac, true);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->对位平台真空吸附完成，标签Z轴上升");
                            MoveAbs(AxisName.Packing_LabelZ, PosName.LabelPreAlignPos);
                            step = 400;
                        }
                        else if (ret == MotionStatus.Timeout || ret2 == MotionStatus.Timeout)
                        {
                            step = 350;
                        }
                        break;
                    }
                case 400:
                    {
                        var ret = IsAxisArrive(AxisName.Packing_LabelZ, PosName.LabelPreAlignPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->标签Z轴上升完成，贴标签气缸缩回");
                            Cylinder(CylinderName.PickLabelCyl, false);//贴标签气缸缩回
                            step = 450;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 380;
                        }
                        break;
                    }
                case 450:
                    {
                        var ret = IsCylinderArrive(CylinderName.PickLabelCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->贴标签气缸缩回完成，标签Z轴下降到待机位置，相机模组移动到拍标签位置");
                            MoveAbs(AxisName.Packing_LabelZ, PosName.WaitPos);
                            Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY, AxisName.Packing_LabelZ };
                            PosNames = new List<PosName>() { PosName.GrabLabelPos, PosName.GrabLabelPos, PosName.WaitPos };
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
                            ShowMsg($"{StationName}->相机模组移动到拍标签位置完成，请求AlignPC对位标签");
							var body = AlignHelper.Instance.Send<AlignRspBody>(VisionCmd.Align, AlignStationEnum.Label);
							if (body != null && body.Offset != null)
                            {
                                AlignOffset = body.Offset;
                                Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY, AxisName.Packing_ArmT };
                                PosNames = new List<PosName>() { PosName.PickLabelFromStagePos };
                                MoveAbsM(Axes, PosNames);
                                ShowMsg($"{StationName}->标签对位完成，模组移动到取标签位置");
                                step = 550;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->标签位置检测失败，请人工处理", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.AlignLabelFail.ToInt(), AllStationAlarm);
                                var handleRes = AskHowToDoRequest(StationNum, $"{StationName}->标签位置检测失败，请人工处理");
                                switch (handleRes)
                                {
                                    case AskResult.Retry:
                                        {
                                            ShowMsg($"{StationName}->处理结果：重新对位");
                                        }
                                        break;
                                    case AskResult.Skip:
                                        {
                                            ShowMsg($"{StationName}->处理结果：不对位，跳转到下一步");
                                            step = 550;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
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
                            ShowMsg($"{StationName}->模组移动到取标签位置完成，ArmZ轴下降到取标签位置");
                            MoveAbs(AxisName.Packing_ArmZ, PosName.PickLabelFromStagePos);
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
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.PickLabelFromStagePos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴下降到取标签位置完成，取标签真空吸附");
                            Vacuum(VacuumName.PackingArmVac, true);
                            Vacuum(VacuumName.LabelAlignStageVac, false);
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
                        var ret = IsVacuumArrive(VacuumName.PackingArmVac, true);
                        var ret2 = IsVacuumArrive(VacuumName.LabelAlignStageVac, false);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->取标签真空吸附完成，ArmZ轴上升到待机位置");
                            MoveAbs(AxisName.Packing_ArmZ, PosName.WaitPos);
                            step = 700;
                        }
                        else if (ret == MotionStatus.Timeout || ret2 == MotionStatus.Timeout)
                        {
                            step = 600;
                        }
                        break;
                    }
                case 700:
                    {
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴上升到待机位置完成，模组移动对位补正值");
                            Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY, AxisName.Packing_ArmT };
                            Poses = AlignOffset.ToList();
                            MoveRelM(Axes, Poses);
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
                        var ret = IsAxisArriveM(Axes);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移动对位补正值完成，ArmZ轴移动到待机位置");
                            MoveAbs(AxisName.Packing_ArmZ, PosName.WaitPos);
                            step = 800;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 700;
                        }
                        break;
                    }
                case 800:
                    {
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴移动到待机位置完成，模组移动到贴标签位置");
                            Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY };
                            Poses = new List<double>() { ParaHelper.Offset.PastePabelOffset.X, ParaHelper.Offset.PastePabelOffset.Y };
                            MoveRelM(Axes, Poses);//贴标签位置是相对对位平台的偏移量
                            step = 850;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 750;
                        }
                        break;
                    }
                case 850:
                    {
                        var ret = IsAxisArriveM(Axes);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移动到贴标签位置完成，ArmZ轴下降");
                            MoveAbs(AxisName.Packing_ArmZ, PosName.PasteLabelPos);
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
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.PasteLabelPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴下降完成，取标签真空吸附关闭");
                            Vacuum(VacuumName.PackingArmVac, false);
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
                        var ret = IsVacuumArrive(VacuumName.PackingArmVac, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->取标签真空吸附关闭完成，ArmZ轴上升到待机位置");
                            MoveAbs(AxisName.Packing_ArmZ, PosName.WaitPos);
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
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            var lotInfo = ProcInfoHelper.Instance.GetLotStageInfo(LotPosEnum.PackingStage);
                            if (lotInfo != null && lotInfo.LotModel != null)
                            {
                                lotInfo.LotModel.LotStatus = LotStatusEnum.PasteLabelDone;
                                ShowMsg($"{StationName}->ArmZ轴上升到待机位置完成，贴标签流程结束", MsgType.Notify);
                                step = 0;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->从PackingStage获取LotModel信息失败", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.GetLotModelFail.ToInt(), StationNum);
                                step = -1;
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 950;
                        }
                        break;
                    }
                default:
                    break;
            }
            return step;
        }
        /// <summary>
        /// 标签检测流程
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int LabelDetectFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->标签检测流程开始，ArmZ轴移动到待机位置", MsgType.Notify);
                        MoveAbs(AxisName.Packing_ArmZ, PosName.WaitPos);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴移动到待机位置完成，模组移动到标签检测位置");
                            Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY };
                            PosNames = new List<PosName>() { PosName.LabelDetectPos };
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
                            ShowMsg($"{StationName}->模组移动到标签检测位置完成，ArmZ轴下降到检测位置");
                            MoveAbs(AxisName.Packing_ArmZ, PosName.LabelDetectPos);
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
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.LabelDetectPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴下降到标签检测位置完成，请求视觉检测标签");
							var detectResult = AlignHelper.Instance.Send<LabelDetectRspBody>(VisionCmd.DetectLabel, 0);
							if (detectResult != null && detectResult.DetectResult)
                            {
                                ShowMsg($"{StationName}->标签检测通过，ArmZ轴上升到待机位置");
                                MoveAbs(AxisName.Packing_ArmZ, PosName.WaitPos);
                                step = 200;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->标签检测不通过，弹窗请求人工处理", MsgType.Error, true);
                                var result = AskHowToDoRequest(StationNum, $"{StationName}->标签检测不通过，请人工处理");
                                switch (result)
                                {
                                    case AskResult.Retry:
                                        {
                                            ShowMsg($"{StationName}->处理结果：重新检测");
                                            break;
                                        }
                                    case AskResult.Skip:
                                        {
                                            ShowMsg($"{StationName}->处理结果：跳过标签检测，ArmZ轴上升到待机位置");
                                            MoveAbs(AxisName.Packing_ArmZ, PosName.WaitPos);
                                            step = 200;
                                            break;
                                        }
                                    case AskResult.Stop:
                                        {
                                            ShowMsg($"{StationName}->处理结果：停机");
                                            StopRequest();
                                            break;
                                        }
                                    default:
                                        break;
                                }
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 100;
                        }
                        break;
                    }
                case 200:
                    {
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴上升到待机位置完成，标签检测流程结束", MsgType.Notify);
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
        /// 从8工位转盘抓取满Lot流程
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int PickFullLotFromStn8Flow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->从8工位转盘抓取满Lot流程开始，ArmZ轴移动到待机位置", MsgType.Notify);
                        Axes = new List<AxisName>() { AxisName.Packing_ArmY, AxisName.Packing_ArmZ };
                        PosNames = new List<PosName>() { PosName.WaitPos };
                        MoveAbsM(Axes, PosNames);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴移动到待机位置完成，模组移动到8工位转盘上下Lot位置");
                            Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmT };
                            PosNames = new List<PosName>() { PosName.PPLotFrom8StnTurnTablePos, PosName.WaitPos };
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
                            ShowMsg($"{StationName}->模组移动到8工位转盘上下Lot位置完成，ArmZ轴下降到抓取Lot位置");
                            MoveAbs(AxisName.Packing_ArmZ, PosName.PPLotFrom8StnTurnTablePos);
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
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.PPLotFrom8StnTurnTablePos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴下降到抓取Lot位置完成，夹爪夹紧");
                            MoveAbs(AxisName.Packing_Claw, PosName.ClawLotPos);
                            step = 200;

                            if (Global.RunMode == RunMode.空跑模式)
                            {
                                IOInfoHelper.GetInputInfoByName(InputName.LotTransferArmDetect.ToString(), out int _).IsShield = true;
                                Thread.Sleep(100);
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 100;
                        }
                        break;
                    }
                case 200:
                    {
                        var ret = IsAxisArrive(AxisName.Packing_Claw, PosName.ClawLotPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            if (Motion.GetDigitalInput(InputName.LotTransferArmDetect))
                            {
                                var lotModel = ProcInfoHelper.Instance.PickLotFromLotStage(Stn8TurnTableStnEnum.LulLot);
                                if (lotModel != null)
                                {
                                    //lotModel.LotStatus = LotStatusEnum.FullTrayLot;
                                    ProcInfoHelper.Instance.PutLotToLotStage(LotPosEnum.LotArmClaw, lotModel);
                                    ShowMsg($"{StationName}->夹爪夹紧完成，ArmZ轴上升到待机位置");
                                    PutLotToPackingStageStep = 1;
                                    step = 250;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->从LulLot平台获取LotModel信息失败", MsgType.Error, true);
                                    AlarmManager.CreateAlarm(AlarmCode.GetLotModelFail.ToInt(), StationNum);
                                }
                            }
                            else
                            {
                                ShowMsg($"{StationName}->Lot夹爪Sensor未检测到Lot", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.LotSensorDetectLotFail.ToInt(), StationNum);
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
                        PutLotToPackingStageStep = PutLotToPackingStageFlow(PutLotToPackingStageStep);
                        if (PutLotToPackingStageStep == 0)
                        {
                            ShowMsg($"{StationName}->ArmZ轴上升到待机位置完成，从8工位转盘抓取满Lot流程结束", MsgType.Notify);
                            step = 0;
                        }
                        break;
                    }
                default:
                    break;
            }
            return step;
        }
        public int PutLotToPackingStageFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->放Lot到打包平台流程开始，ArmZ轴移动到待机位置", MsgType.Notify);
                        MoveAbs(AxisName.Packing_ArmZ, PosName.WaitPos);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴移动到待机位置完成，ArmY轴移动到待机位置");
                            Axes = new List<AxisName>() { AxisName.Packing_ArmY };
                            PosNames = new List<PosName>() { PosName.WaitPos };
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
                        var ret = IsAxisArrive(AxisName.Packing_ArmY, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmY轴移动到待机位置完成，模组移动到取放Lot位置");
                            Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY };
                            PosNames = new List<PosName>() { PosName.LotClawPPLotFromPackingStagePos };
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
                            ShowMsg($"{StationName}->模组移动到取放Lot位置完成，ArmZ轴下降到取放Lot位置");
                            MoveAbs(AxisName.Packing_ArmZ, PosName.LotClawPPLotFromPackingStagePos);
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
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.LotClawPPLotFromPackingStagePos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴下降到取放Lot位置完成，夹爪松开");
                            MoveAbs(AxisName.Packing_Claw, PosName.WaitPos);
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
                        var ret = IsAxisArrive(AxisName.Packing_Claw, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪松开完成，ArmZ轴上升到待机位置");
                            var lotModel = ProcInfoHelper.Instance.PickLotFromLotStage(LotPosEnum.LotArmClaw);
                            if (lotModel != null)
                            {
                                ProcInfoHelper.Instance.PutLotToLotStage(LotPosEnum.PackingStage, lotModel);
                                ShowMsg($"{StationName}->夹爪松开完成，ArmZ轴上升到待机位置");
                                MoveAbs(AxisName.Packing_ArmZ, PosName.WaitPos);
                                step = 300;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.LotTransferArmDetect.ToString(), out int _).IsShield = false;
                                    Thread.Sleep(100);
                                }
                            }
                            else
                            {
                                ShowMsg($"{StationName}->从LotArmClaw平台获取LotModel信息失败", MsgType.Error, true);
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
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴上升到待机位置完成，放Lot到打包平台流程结束", MsgType.Notify);
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 250;
                        }
                        break;
                    }
            }
            return step;
        }
        /// <summary>
        /// Lot束带流程
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int PackingLotFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        PackingNo = 0;
                        ShowMsg($"{StationName}->束带流程开始，判断束带流程", MsgType.Notify);
                        CurLotInfo = ProcInfoHelper.Instance.GetLotStageInfo(LotPosEnum.PackingStage);
                        if (CurLotInfo != null && CurLotInfo.LotModel != null)
                        {
                            switch (CurLotInfo.LotModel.LotStatus)
                            {
                                case LotStatusEnum.FullTrayLot:
                                    {
                                        ShowMsg($"{StationName}->Lot还未束带，ArmZ轴移动到待机位置", MsgType.Notify);
                                        MoveAbs(AxisName.Packing_ArmZ, PosName.WaitPos);
                                        step = 50;
                                    }
                                    break;
                                case LotStatusEnum.Belt1PackingDone:
                                    {
                                        ShowMsg($"{StationName}->第一条束带完成，执行束带检测流程", MsgType.Notify);
                                        Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY };
                                        PosNames = new List<PosName>() { PosName.LotClawPPLotFromPackingStagePos };
                                        MoveAbsM(Axes, PosNames);
                                        step = 200;
                                    }
                                    break;
                                case LotStatusEnum.Belt1DetectDone:
                                    {
                                        ShowMsg($"{StationName}->第一条束带检测完成，继续束带");
                                        step = 550;
                                    }
                                    break;
                                case LotStatusEnum.Belt2PackingDone:
                                    {
                                        ShowMsg($"{StationName}->第二条束带检测完成，执行束带检测流程");
                                        PosNames = new List<PosName>() { PosName.LotPickerPPLotFromPackingStagePos };
                                        Cylinder(CylinderName.PackerTransferCyl, false);
                                        MoveAbsM(Axes, PosNames);
                                        step = 1300;
                                    }
                                    break;
                                case LotStatusEnum.Belt2DetectDone:
                                    {
                                        ShowMsg($"{StationName}->束带完成，模组移动到吸取Lot位置");
                                        step = 1500;
                                    }
                                    break;
                                default:
                                    break;
                            }

                        }
                        else
                        {
                            ShowMsg($"{StationName}->从PackingStage获取LotModel信息失败", MsgType.Error, true);
                            AlarmManager.CreateAlarm(AlarmCode.GetLotModelFail.ToInt(), StationNum);
                        }
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴移动到待机位置完成，束带气缸伸出");
                            Cylinder(CylinderName.PackerTransferCyl, true);
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
                        var ret = IsCylinderArrive(CylinderName.PackerTransferCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->束带气缸伸出完成，第一次束带");
                            Motion.SetDigitalOutput(OutputName.PackingStart, true);
                            Delay.Start(10 * 1000);
                            step = 150;

                            if (Global.RunMode == RunMode.空跑模式)
                            {
                                IOInfoHelper.GetInputInfoByName(InputName.PackerComp.ToString(), out int _).IsShield = true;
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 50;
                        }
                        break;
                    }
                case 150:
                    {
                        if (Motion.GetDigitalInput(InputName.PackerComp))
                        {
                            Motion.SetDigitalOutput(OutputName.PackingStart, false);
                            ShowMsg($"{StationName}->第一次束带完成，执行束带检测流程");
                            ShowMsg($"{StationName}->模组移动到取放Lot位置");
                            CurLotInfo.LotModel.LotStatus = LotStatusEnum.Belt1PackingDone;
                            ProcInfoHelper.Instance.UpdateLotStatus(LotPosEnum.PackingStage, CurLotInfo.LotModel);
                            Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY };
                            PosNames = new List<PosName>() { PosName.LotClawPPLotFromPackingStagePos };
                            MoveAbsM(Axes, PosNames);
                            step = 200;
                        }
                        else if (Delay.TimeUp())
                        {
                            ShowMsg($"{StationName}->束带机超时，弹窗请求人工处理", MsgType.Error, true);
                            var ret = AskHowToDoRequest(StationNum, $"{StationName}->束带机超时，请人工处理");
                            switch (ret)
                            {
                                case AskResult.Retry:
                                    {
                                        ShowMsg($"{StationName}->处理结果：重新束带");
                                        step = 100;
                                    }
                                    break;
                                case AskResult.Skip:
                                    {
                                        ShowMsg($"{StationName}->处理结果：跳转到下一步");
                                        step = 200;
                                    }
                                    break;
                                case AskResult.Stop:
                                    {
                                        ShowMsg($"{StationName}->处理结果：停机");
                                        StopRequest();
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    }
                case 200:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移动到取放Lot位置完成，ArmZ轴下降到抓取Lot位置");
                            MoveAbs(AxisName.Packing_ArmZ, PosName.LotClawPPLotFromPackingStagePos);
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
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.LotClawPPLotFromPackingStagePos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴下降到抓取Lot位置完成，夹爪夹紧");
                            MoveAbs(AxisName.Packing_Claw, PosName.ClawLotPos);
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
                        var ret = IsAxisArrive(AxisName.Packing_Claw, PosName.ClawLotPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪夹紧完成，ArmZ轴上升到待机位置");
                            MoveAbs(AxisName.Packing_ArmZ, PosName.WaitPos);
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
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴上升到待机位置完成，束带气缸缩回");
                            Cylinder(CylinderName.PackerTransferCyl, false);
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
                        var ret = IsCylinderArrive(CylinderName.PackerTransferCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->束带气缸缩回完成，模组移动到束带检测位置");
                            Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY };
                            PosNames = new List<PosName>() { PosName.PackingBeltDetectPos };
                            MoveAbsM(Axes, PosNames);
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
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移动到束带检测位置完成，ArmZ轴下降到检测位置");
                            MoveAbs(AxisName.Packing_ArmZ, PosName.PackingBeltDetectPos);
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
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.PackingBeltDetectPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴下降到检测位置完成，执行束带检测流程");
							var detectResult = AlignHelper.Instance.Send<BeltDetectRspBody>(VisionCmd.DetectBelt, AlignStationEnum.None, PackingNo);
							if (detectResult.DetectResult)
                            {
                                CurLotInfo.LotModel.BeltDetectResult[PackingNo] = Result.OK;
                                PackingNo++;
                                if (PackingNo == 2)
                                {
                                    ShowMsg($"{StationName}->束带完成，模组移动到吸取Lot位置");
                                    Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY };
                                    PosNames = new List<PosName>() { PosName.LotPickerPPLotFromPackingStagePos };
                                    MoveAbsM(Axes, PosNames);
                                    CurLotInfo.LotModel.LotStatus = LotStatusEnum.Belt2DetectDone;
                                    ProcInfoHelper.Instance.UpdateLotStatus(LotPosEnum.PackingStage, CurLotInfo.LotModel);
                                    step = 1500;
                                }
                                else
                                {
                                    CurLotInfo.LotModel.LotStatus = LotStatusEnum.Belt1DetectDone;
                                    ProcInfoHelper.Instance.UpdateLotStatus(LotPosEnum.PackingStage, CurLotInfo.LotModel);
                                    ShowMsg($"{StationName}->束带检测通过，继续束带");
                                    step = 550;
                                }
                            }
                            else
                            {
                                CurLotInfo.LotModel.BeltDetectResult[PackingNo] = Result.NG;
                                ProcInfoHelper.Instance.UpdateLotStatus(LotPosEnum.PackingStage, CurLotInfo.LotModel);
                                ShowMsg($"{StationName}->束带检测不通过，弹窗请求如何处理", MsgType.Error, true);
                                var result = AskHowToDoRequest(StationNum, $"{StationName}->束带检测不通过，请人工处理");
                                switch (result)
                                {
                                    case AskResult.Retry:
                                        {
                                            ShowMsg($"{StationName}->处理结果：重新检测");
                                        }
                                        break;
                                    case AskResult.Skip:
                                        {
                                            PackingNo++;
                                            ShowMsg($"{StationName}->处理结果：不检测，跳转到下一步");
                                            step = 550;
                                        }
                                        break;
                                    case AskResult.Stop:
                                        {
                                            ShowMsg($"{StationName}->处理结果：停机");
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 450;
                        }
                        break;
                    }
                case 550:
                    {
                        ShowMsg($"{StationName}->ArmZ轴上升到待机");
                        MoveAbs(AxisName.Packing_ArmZ, PosName.WaitPos);
                        step = 600;
                        break;
                    }
                case 600:
                    {
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴上升到待机位置完成，模组移动到取放Lot位置");
                            Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY };
                            PosNames = new List<PosName>() { PosName.LotClawPPLotFromPackingStagePos };
                            MoveAbsM(Axes, PosNames);
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
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移动到取放Lot位置完成，ArmZ轴下降到取放Lot位置");
                            MoveAbs(AxisName.Packing_ArmZ, PosName.LotClawPPLotFromPackingStagePos);
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
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.LotClawPPLotFromPackingStagePos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴下降到取放Lot位置完成，夹爪松开");
                            MoveAbs(AxisName.Packing_Claw, PosName.WaitPos);
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
                        var ret = IsAxisArrive(AxisName.Packing_Claw, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪松开完成，ArmZ轴上升到待机位置");
                            MoveAbs(AxisName.Packing_ArmZ, PosName.WaitPos);
                            step = 800;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 700;
                        }
                        break;
                    }
                case 800:
                    {
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴上升到待机位置完成");
                            Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY, AxisName.Packing_ArmT };
                            PosNames = new List<PosName>() { PosName.LotPickerPPLotFromPackingStagePos };
                            MoveAbsM(Axes, PosNames);
                            step = 850;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 750;
                        }
                        break;
                    }
                case 850:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移动到取放Lot位置完成，ArmZ轴下降到取放Lot位置");
                            MoveAbs(AxisName.Packing_ArmZ, PosName.LotPickerPPLotFromPackingStagePos);
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
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.LotPickerPPLotFromPackingStagePos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴下降到取放Lot位置完成，真空吸附");
                            Vacuum(VacuumName.PackingArmVac, true);
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
                        var ret = IsVacuumArrive(VacuumName.PackingArmVac, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->真空吸附完成，ArmZ轴上升到待机位置");
                            MoveAbs(AxisName.Packing_ArmZ, PosName.WaitPos);
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
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴上升到待机位置完成，旋转90度");
                            MoveRel(AxisName.Packing_ArmT, 90);
                            step = 1050;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 950;
                        }
                        break;
                    }
                case 1050:
                    {
                        var ret = IsAxisArrive(AxisName.Packing_ArmT);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->旋转90度完成，ArmZ轴下降到取放Lot位置");
                            MoveAbs(AxisName.Packing_ArmZ, PosName.LotPickerPPLotFromPackingStagePos);
                            step = 1100;
                        }

                        break;
                    }
                case 1100:
                    {
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.LotPickerPPLotFromPackingStagePos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴下降到取放Lot位置完成，真空吸附关闭");
                            Vacuum(VacuumName.PackingArmVac, false);
                            step = 1150;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1050;
                        }
                        break;
                    }
                case 1150:
                    {
                        var ret = IsVacuumArrive(VacuumName.PackingArmVac, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->真空吸附关闭完成，ArmZ轴上升到待机位置");
                            MoveAbs(AxisName.Packing_ArmZ, PosName.WaitPos);
                            step = 1170;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1100;
                        }
                        break;
                    }
                case 1170:
                    {
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴上升到待机位置完成，束带气缸伸出");
                            Cylinder(CylinderName.PackerTransferCyl, true);
                            MoveRel(AxisName.Packing_ArmT, -90);
                            step = 1200;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1150;
                        }
                        break;
                    }
                case 1200:
                    {
                        var ret = IsCylinderArrive(CylinderName.PackerTransferCyl, true);
                        var ret2 = IsAxisArrive(AxisName.Packing_ArmT);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->束带气缸伸出完成，第二次束带");
                            Motion.SetDigitalOutput(OutputName.PackingStart, true);
                            Delay.Start(10 * 1000);
                            step = 1250;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1150;
                        }
                        break;
                    }
                case 1250:
                    {
                        if (Motion.GetDigitalInput(InputName.PackerComp))
                        {
                            Motion.SetDigitalOutput(OutputName.PackingStart, false);
                            ShowMsg($"{StationName}->第二次束带完成，模组移动到吸取Lot位置");
                            var lotInfo = ProcInfoHelper.Instance.GetLotStageInfo(LotPosEnum.PackingStage);
                            if (lotInfo != null && lotInfo.LotModel != null)
                            {
                                lotInfo.LotModel.LotStatus = LotStatusEnum.Belt2PackingDone;
                                ProcInfoHelper.Instance.UpdateLotStatus(LotPosEnum.PackingStage, lotInfo.LotModel);
                                PosNames = new List<PosName>() { PosName.LotPickerPPLotFromPackingStagePos };
                                Cylinder(CylinderName.PackerTransferCyl, false);
                                MoveAbsM(Axes, PosNames);
                                step = 1300;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->从PackingStage获取LotModel信息失败", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.GetLotModelFail.ToInt(), StationNum);
                            }
                        }
                        else if (Delay.TimeUp())
                        {
                            ShowMsg($"{StationName}->束带机超时，弹窗请求人工处理", MsgType.Error, true);
                            var ret = AskHowToDoRequest(StationNum, $"{StationName}->束带机超时，请人工处理");
                            switch (ret)
                            {
                                case AskResult.Retry:
                                    {
                                        ShowMsg($"{StationName}->处理结果：重新束带");
                                        step = 1200;
                                    }
                                    break;
                                case AskResult.Skip:
                                    {
                                        ShowMsg($"{StationName}->处理结果：跳转到下一步");
                                        PosNames = new List<PosName>() { PosName.LotPickerPPLotFromPackingStagePos };
                                        MoveAbsM(Axes, PosNames);
                                        Cylinder(CylinderName.PackerTransferCyl, false);
                                        step = 1300;
                                    }
                                    break;
                                case AskResult.Stop:
                                    {
                                        ShowMsg($"{StationName}->处理结果：停机");
                                        StopRequest();
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }
                        break;
                    }
                case 1300:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        var ret2 = IsCylinderArrive(CylinderName.PackerTransferCyl, false);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组到达吸取Lot位置，ArmZ轴下降到吸Lot位置");
                            MoveAbs(AxisName.Packing_ArmZ, PosName.LotPickerPPLotFromPackingStagePos);
                            step = 1350;
                        }
                        else if (ret == MotionStatus.Timeout || ret2 == MotionStatus.Timeout)
                        {
                            step = 1250;
                        }
                        break;
                    }
                case 1350:
                    {
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.LotPickerPPLotFromPackingStagePos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴下降到吸Lot位置完成，真空吸附");
                            Vacuum(VacuumName.PackingArmVac, true);
                            step = 1400;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1300;
                        }
                        break;
                    }
                case 1400:
                    {
                        var ret = IsVacuumArrive(VacuumName.PackingArmVac, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->真空吸附完成，ArmZ轴上升到待机位置");
                            MoveAbs(AxisName.Packing_ArmZ, PosName.WaitPos);
                            step = 1450;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1350;
                        }
                        break;
                    }
                case 1450:
                    {
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴上升到待机位置完成，模组移动到束带检测位置");
                            Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY };
                            PosNames = new List<PosName>() { PosName.PackingBeltDetectPos };
                            MoveAbsM(Axes, PosNames);
                            step = 450;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1400;
                        }
                        break;
                    }
                case 1500:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移动到吸取Lot位置完成，ArmT轴旋转90度");
                            MoveRel(AxisName.Packing_ArmT, -90);
                            step = 1520;
                        }
                        break;
                    }
                case 1520:
                    {
                        var ret = IsAxisArrive(AxisName.Packing_ArmT);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->旋转90度完成，ArmZ轴下降到吸取Lot位置");
                            MoveAbs(AxisName.Packing_ArmZ, PosName.LotPickerPPLotFromPackingStagePos);
                            step = 1550;
                        }
                        break;
                    }
                case 1550:
                    {
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.LotPickerPPLotFromPackingStagePos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->真空释放");
                            Vacuum(VacuumName.PackingArmVac, false);
                            step = 1600;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1500;
                        }
                        break;
                    }
                case 1600:
                    {
                        var ret = IsVacuumArrive(VacuumName.PackingArmVac, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->真空释放完成，ArmZ轴上升到待机位置");

                            MoveAbs(AxisName.Packing_ArmZ, PosName.WaitPos);
                            step = 1650;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1550;
                        }
                        break;
                    }
                case 1650:
                    {
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴上升到待机位置完成，束带流程结束", MsgType.Notify);
                            step = 0;

                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1600;
                        }
                        break;
                    }
                default:
                    break;
            }
            return step;
        }
        /// <summary>
        /// 从Lot料盒工位取空Lot放到8工位转盘流程  
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int PickEmptyLotFromLotBoxFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->取空Lot到8工位转盘流程开始", MsgType.Notify);
                        MoveAbs(AxisName.Packing_ArmZ, PosName.WaitPos);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            Axes = new List<AxisName>() { AxisName.Packing_ArmY };
                            PosNames = new List<PosName>() { PosName.WaitPos };
                            ShowMsg($"{StationName}->ArmZ轴上升到待机位，ArmY轴移动到待机位");
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
                            ShowMsg($"{StationName}->ArmY轴移动到待机位完成，模组移动到从LotBox取放Lot等待位置");
                            Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY };
                            PosNames = new List<PosName>() { PosName.PPLotFromLotBoxWaitPos };
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
                            ShowMsg($"{StationName}->模组移动到从LotBox取放Lot等待位置完成，发送取Lot请求给LotBox工位");
                            RegisterService.Instance.SetBoolReg(RegisterEnum.PickLotFromLotBoxReq);
                            step = 170;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 100;
                        }
                        break;
                    }
                case 170:
                    {
                        if (GetBoolReg(RegisterEnum.LotBoxStnAllowPickLot))
                        {
                            ResetBoolReg(RegisterEnum.LotBoxStnAllowPickLot);
                            ShowMsg($"{StationName}->LotBox工位回复可以取Lot，获取可用的空Lot穴位编号");
                            EmptySlotRCNo = ProcInfoHelper.Instance.GetAvlEmptyLotCellNo();
                            if (EmptySlotRCNo.r != -1 && EmptySlotRCNo.c != -1)
                            {
                                Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY };
                                var pos = GetPPTrayFromLotBoxStnPos(EmptySlotRCNo.r, EmptySlotRCNo.c, true);
                                Poses = pos.ToList();
                                MoveAbsM(Axes, Poses);
                                step = 200;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->获取可用的空Lot穴位编号失败", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.GetEmptyLotCellNoFail.ToInt(), AllStationAlarm);
                            }
                        }
                        break;
                    }
                case 200:
                    {
                        var ret = IsAxisArriveM(Axes, Poses);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移动到从LotBox取放Lot位置完成，ArmZ轴下降到取放Lot位置");
                            Axes = new List<AxisName>() { AxisName.Packing_ArmZ };
                            ActPickTrayCount = ParaHelper.Prod.PickTrayCountFromLotOnce;
                            var pos = GetPickLotHeight(ref ActPickTrayCount);
                            Poses = new List<double>() { pos };
                            MoveAbsM(Axes, Poses);
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
                        var ret = IsAxisArriveM(Axes, Poses);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴下降到取放Lot位置完成，夹爪夹紧");
                            MoveAbs(AxisName.Packing_Claw, PosName.ClawLotPos);
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
                        var ret = IsAxisArrive(AxisName.Packing_Claw, PosName.ClawLotPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪夹紧完成，ArmZ轴上升到待机位置");
                            MoveAbs(AxisName.Packing_ArmZ, PosName.WaitPos);
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
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            var trayList = ProcInfoHelper.Instance.PickTraysFromLotBox(EmptySlotRCNo.r, EmptySlotRCNo.c, ActPickTrayCount);
                            LotModel model = new LotModel();
                            model.LotCode = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                            model.TrayList = trayList;
                            model.TrayCount = trayList.Count;
                            model.LotStatus = LotStatusEnum.EmptyTrayLot;
                            model.UpdateMap();
                            ProcInfoHelper.Instance.PutLotToLotStage(LotPosEnum.LotArmClaw, model);

                            ShowMsg($"{StationName}->ArmZ轴上升到待机位置完成，ArmY轴运动到待机位");
                            Axes = new List<AxisName>() { AxisName.Packing_ArmY };
                            PosNames = new List<PosName>() { PosName.WaitPos };
                            MoveAbsM(Axes, PosNames);
                            step = 400;

                            if (Global.RunMode == RunMode.空跑模式)
                            {
                                IOInfoHelper.GetInputInfoByName(InputName.LotTransferArmDetect.ToString(), out int _).IsShield = true;
                                Thread.Sleep(100);
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
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmY轴到达待机位，ArmX轴移动到8工位转盘取放Lot位置");
                            Axes = new List<AxisName>() { AxisName.Packing_ArmX };
                            PosNames = new List<PosName>() { PosName.PPLotFrom8StnTurnTablePos };
                            MoveAbsM(Axes, PosNames);
                            step = 410;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 350;
                        }
                        break;
                    }
                case 410:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmX轴到达8工位转盘取放Lot位置，ArmY轴移动到8工位转盘取放Lot位置");
                            MoveAbs(AxisName.Packing_ArmY, PosName.PPLotFrom8StnTurnTablePos);
                            step = 450;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 400;
                        }
                        break;
                    }
                case 450:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmY轴到达8工位转盘取放Lot位置，ArmZ轴下降到取放Lot位置");
                            MoveAbs(AxisName.Packing_ArmZ, PosName.PPLotFrom8StnTurnTablePos);
                            step = 500;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 410;
                        }
                        break;
                    }
                case 500:
                    {
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.PPLotFrom8StnTurnTablePos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴到达取放Lot位置，夹爪松开");
                            MoveAbs(AxisName.Packing_Claw, PosName.WaitPos);
                            step = 550;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 450;
                        }
                        break;
                    }
                case 550:
                    {
                        var ret = IsAxisArrive(AxisName.Packing_Claw, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪松开完成，ArmZ轴上升到待机位");
                            MoveAbs(AxisName.Packing_ArmZ, PosName.WaitPos);
                            step = 600;

                            if (Global.RunMode == RunMode.空跑模式)
                            {
                                IOInfoHelper.GetInputInfoByName(InputName.LotTransferArmDetect.ToString(), out int _).IsShield = false;
                                Thread.Sleep(100);
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 500;
                        }
                        break;
                    }
                case 600:
                    {
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴到达待机位，ArmY轴移动到待机位");
                            MoveAbs(AxisName.Packing_ArmY, PosName.WaitPos);
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
                        var ret = IsAxisArrive(AxisName.Packing_ArmY, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmY轴到达待机位，ArmX轴移动到待机位");
                            MoveAbs(AxisName.Packing_ArmX, PosName.WaitPos);
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
                        var ret = IsAxisArrive(AxisName.Packing_ArmX, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            RegisterService.Instance.SetBoolReg(RegisterEnum.LotBoxStnLoadEmptyLotDone);
                            ShowMsg($"{StationName}->ArmX轴到达待机位，取空Lot到8工位转盘流程结束", MsgType.Notify);
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 650;
                        }
                        break;
                    }
                default:
                    break;
            }
            return step;
        }
        /// <summary>
        /// 放空Lot到8工位转盘流程
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int PutEmptyLotToStn8Flow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->放空Lot到8工位转盘流程开始，ArmZ轴上升到待机位", MsgType.Notify);
                        Axes = new List<AxisName>() { AxisName.Packing_ArmZ };
                        PosNames = new List<PosName>() { PosName.WaitPos };
                        MoveAbsM(Axes, PosNames);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴到达待机位，ArmY轴移动到待机位");
                            Axes = new List<AxisName>() { AxisName.Packing_ArmY };
                            PosNames = new List<PosName>() { PosName.WaitPos };
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
                            ShowMsg($"{StationName}->ArmY轴到达待机位，ArmX轴移动到8工位转盘取放Lot位置");
                            Axes = new List<AxisName>() { AxisName.Packing_ArmX };
                            PosNames = new List<PosName>() { PosName.PPLotFrom8StnTurnTablePos };
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
                            ShowMsg($"{StationName}->ArmX轴到达8工位转盘取放Lot位置，ArmY轴移动到8工位转盘取放Lot位置");
                            MoveAbs(AxisName.Packing_ArmY, PosName.PPLotFrom8StnTurnTablePos);
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
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmY轴到达8工位转盘取放Lot位置，ArmZ轴下降到取放Lot位置");
                            MoveAbs(AxisName.Packing_ArmZ, PosName.PPLotFrom8StnTurnTablePos);
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
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.PPLotFrom8StnTurnTablePos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴到达取放Lot位置，夹爪松开");
                            MoveAbs(AxisName.Packing_Claw, PosName.WaitPos);
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
                        var ret = IsAxisArrive(AxisName.Packing_Claw, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪松开完成，ArmZ轴上升到待机位");
                            MoveAbs(AxisName.Packing_ArmZ, PosName.WaitPos);
                            step = 350;

                            if (Global.RunMode == RunMode.空跑模式)
                            {
                                IOInfoHelper.GetInputInfoByName(InputName.LotTransferArmDetect.ToString(), out int _).IsShield = false;
                                Thread.Sleep(100);
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
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴到达待机位，ArmY轴移动到待机位");
                            MoveAbs(AxisName.Packing_ArmY, PosName.WaitPos);
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
                        var ret = IsAxisArrive(AxisName.Packing_ArmY, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmY轴到达待机位，ArmX轴移动到待机位");
                            MoveAbs(AxisName.Packing_ArmX, PosName.WaitPos);
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
                        var ret = IsAxisArrive(AxisName.Packing_ArmX, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            RegisterService.Instance.SetBoolReg(RegisterEnum.LotBoxStnLoadEmptyLotDone);
                            ShowMsg($"{StationName}->ArmX轴到达待机位，放空Lot到8工位转盘流程结束", MsgType.Notify);
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 400;
                        }
                        break;
                    }
                default:
                    break;
            }
            return step;
        }
        /// <summary>
        /// 放Lot到LotBox流程
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int PutFullLotToLotBoxFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->放Lot到LotBox流程开始", MsgType.Notify);
                        CurLotInfo = ProcInfoHelper.Instance.GetLotStageInfo(LotPosEnum.LotArmClaw);
                        if (CurLotInfo != null && CurLotInfo.LotModel != null)
                        {
                            ShowMsg($"{StationName}->等级：{CurLotInfo.LotModel.BinLevel}，模组Z轴移动到待机位置", MsgType.Notify);
                            Axes = new List<AxisName>() { AxisName.Packing_ArmZ };
                            PosNames = new List<PosName>() { PosName.WaitPos };
                            MoveAbsM(Axes, PosNames);
                            step = 50;
                        }
                        else
                        {
                            ShowMsg($"{StationName}->从LotArmClaw平台获取LotModel信息失败", MsgType.Error, true);
                            AlarmManager.CreateAlarm(AlarmCode.GetLotModelFail.ToInt(), StationNum);
                        }
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组Z轴到达待机位置，模组Y轴移动到待机位置");
                            Axes = new List<AxisName>() { AxisName.Packing_ArmY };
                            PosNames = new List<PosName>() { PosName.WaitPos };
                            MoveAbsM(Axes, PosNames);
                            step = 100;
                        }
                        break;
                    }
                case 100:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组Y轴到达待机位置，模组X轴移动到待机位置");
                            Axes = new List<AxisName>() { AxisName.Packing_ArmX };
                            PosNames = new List<PosName>() { PosName.WaitPos };
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
                            ShowMsg($"{StationName}->发送放满Lot请求给LotBoxStn");
                            RegisterService.Instance.SetBoolReg(RegisterEnum.PutFullLotToLotBoxReq);
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
                        if (GetBoolReg(RegisterEnum.LotBoxStnAllowPutFullLot))
                        {
                            int rowNo = -1;
                            int colNo = -1;
                            ShowMsg($"{StationName}->收到LotBoxStn允许放满Lot信号，获取可放Lot的穴位号");
                            ResetBoolReg(RegisterEnum.LotBoxStnAllowPutFullLot);
                            LotBoxStnType type = CurLotInfo.LotModel.BinLevel.ToInt() % 2 != 0 ? LotBoxStnType.OKBinStn : LotBoxStnType.NGBinStn;
                            var rcNo = ProcInfoHelper.Instance.GetAvlBinLotBoxCellNo(CurLotInfo.LotModel.BinLevel, type, ref AvlFullLotCellModel);
                            if (rcNo.r != -1 && rcNo.c != -1 && AvlFullLotCellModel != null)
                            {
                                rowNo = rcNo.r;
                                colNo = rcNo.c;
                            }
                            else
                            {
                                var emptyLotBoxNo = ProcInfoHelper.Instance.GetAvlNullCellNo(type, ref AvlFullLotCellModel);
                                if (emptyLotBoxNo.r != -1 && emptyLotBoxNo.c != -1)
                                {
                                    rowNo = emptyLotBoxNo.r;
                                    colNo = emptyLotBoxNo.c;
                                }
                            }
                            if (rowNo != -1 && colNo != -1)
                            {
                                var pos = GetPPTrayFromLotBoxStnPos(rowNo, colNo, false, CurLotInfo.LotModel.BinLevel);
                                ShowMsg($"{StationName}->获取可放Lot的穴位号：行号{rowNo}，列号{colNo}，等级：{CurLotInfo.LotModel.BinLevel}，穴位位置：{pos.ToString()}");
                                Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY };
                                Poses = pos.ToList();
                                MoveAbsM(Axes, Poses);
                                step = 250;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->获取可放Lot的穴位号失败，请检查代码逻辑", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.GetAvlBinLotBoxCellNoFail.ToInt(), AllStationAlarm);
                            }
                        }
                        break;
                    }
                case 250:
                    {
                        var ret = IsAxisArriveM(Axes, Poses);
                        if (ret == MotionStatus.Arrived)
                        {
                            var pos = GetPutLotHeight(AvlFullLotCellModel.LotCount);
                            ShowMsg($"{StationName}->模组移动到LotBox可放Lot位置完成，获取Z轴放Lot的高度：{pos}");
                            Poses = new List<double>() { pos };
                            Axes = new List<AxisName>() { AxisName.Packing_ArmZ };
                            MoveAbsM(Axes, Poses);
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
                        var ret = IsAxisArriveM(Axes, Poses);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->Z轴下降到放Lot位置完成，夹爪松开");
                            MoveAbs(AxisName.Packing_Claw, PosName.WaitPos);
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
                        var ret = IsAxisArrive(AxisName.Packing_Claw, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            var lotModel = ProcInfoHelper.Instance.PickLotFromLotStage(LotPosEnum.LotArmClaw);
                            if (lotModel != null)
                            {
                                var lotBoxType = lotModel.BinLevel.ToInt() % 2 != 0 ? LotBoxType.Bin135 : LotBoxType.Bin246;
                                //LotBoxStnType lotBoxStnType = CurLotInfo.LotModel.BinLevel.ToInt() % 2 != 0 ? LotBoxStnType.OKBinStn : LotBoxStnType.NGBinStn;
                                ProcInfoHelper.Instance.PutLotToLotBox(lotModel, lotBoxType, AvlFullLotCellModel.RowNo, AvlFullLotCellModel.ColNo);
                                ShowMsg($"{StationName}->夹爪松开完成，Z轴上升到待机位置");
                                MoveAbs(AxisName.Packing_ArmZ, PosName.WaitPos);
                                step = 400;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.LotTransferArmDetect.ToString(), out int _).IsShield = false;
                                    Thread.Sleep(100);
                                }
                            }
                            else
                            {
                                ShowMsg($"{StationName}->从LotArmClaw平台获取LotModel信息失败", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.GetLotModelFail.ToInt(), StationNum);
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
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->Z轴上升到待机位置完成，模组移动到待机位置");
                            Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY };
                            PosNames = new List<PosName>() { PosName.WaitPos };
                            MoveAbsM(Axes, PosNames);
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
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移动到待机位置完成，Lot从束带平台到LotBox流程结束", MsgType.Notify);
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 400;
                        }
                        break;
                    }
                default:
                    break;
            }
            return step;
        }
        /// <summary>
        /// 从束带平台取Lot流程
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int PickFullLotFromPackingStageFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->从束带平台取Lot流程开始", MsgType.Notify);
                        MoveAbs(AxisName.Packing_ArmZ, PosName.WaitPos);
                        Cylinder(CylinderName.PackerTransferCyl, false);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.WaitPos);
                        var ret2 = IsCylinderArrive(CylinderName.PackerTransferCyl, false);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                        {
                            Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY };
                            PosNames = new List<PosName>() { PosName.LotClawPPLotFromPackingStagePos };
                            ShowMsg($"{StationName}->ArmZ轴上升到待机位，模组移动到从束带平台取放Lot位置");
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
                            Axes = new List<AxisName>() { AxisName.Packing_ArmZ, AxisName.Packing_Claw };
                            PosNames = new List<PosName>() { PosName.LotClawPPLotFromPackingStagePos, PosName.WaitPos };
                            ShowMsg($"{StationName}->模组移动到从束带平台取放Lot位置完成，Z轴下降到抓取Lot位置");
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
                            ShowMsg($"{StationName}->Z轴下降到抓取Lot位置完成，夹爪夹紧");
                            MoveAbs(AxisName.Packing_Claw, PosName.ClawLotPos);
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
                        var ret = IsAxisArrive(AxisName.Packing_Claw, PosName.ClawLotPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            var model = ProcInfoHelper.Instance.PickLotFromLotStage(LotPosEnum.PackingStage);
                            if (model != null)
                            {
                                ProcInfoHelper.Instance.PutLotToLotStage(LotPosEnum.LotArmClaw, model);
                                ShowMsg($"{StationName}->Z轴上升到待机位置");
                                MoveAbs(AxisName.Packing_ArmZ, PosName.WaitPos);
                                step = 250;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->从束带平台获取Lot信息失败", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.GetLotModelFail.ToInt(), AllStationAlarm);
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
                        var ret = IsAxisArrive(AxisName.Packing_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组Z轴移动到待机位置，从束带平台取Lot流程结束", MsgType.Notify);
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 200;
                        }
                        break;
                    }

                default:
                    break;
            }
            return step;
        }
        public Xyt GetPPTrayFromLotBoxStnPos(int rowNo, int colNo, bool isPick, BinLevel binLevel = BinLevel.None)
        {
            Xyt ret = new Xyt();
            var posInfoX = AxisInfoHelper.GetPosInfoByName(AxisName.Packing_ArmX, PosName.PPTrayFromLotBoxStn1StartPos);//左上角第一个Cell
            var posInfoY = AxisInfoHelper.GetPosInfoByName(AxisName.Packing_ArmY, PosName.PPTrayFromLotBoxStn1StartPos);
            if (posInfoX != null && posInfoY != null)
            {
                if (binLevel == BinLevel.Bin2 || binLevel == BinLevel.Bin4 || binLevel == BinLevel.Bin6 || isPick)//放Bin246 Lot和取Lot都在工位2
                {
                    posInfoX = AxisInfoHelper.GetPosInfoByName(AxisName.Packing_ArmX, PosName.PPTrayFromLotBoxStn2StartPos);//左上角第一个Cell
                    posInfoY = AxisInfoHelper.GetPosInfoByName(AxisName.Packing_ArmY, PosName.PPTrayFromLotBoxStn2StartPos);
                }
                ret.X = posInfoX.Pos + ParaHelper.Prod.LotCellPitchX * rowNo;
                ret.Y = posInfoY.Pos + ParaHelper.Prod.LotCellPitchY * colNo;
            }
            else
            {
                ShowMsg($"{StationName}->获取{AxisName.Packing_ArmX}轴{PosName.PPTrayFromLotBoxStn1StartPos}点位信息失败", MsgType.Error, true);
                AlarmManager.CreateAlarm(AlarmCode.GetPosInfoFail.ToInt(), AllStationAlarm);
            }
            return ret;
        }

        private double GetPutLotHeight(int lotCount)
        {
            double ret = 0;
            var posInfo = AxisInfoHelper.GetPosInfoByName(AxisName.Packing_ArmZ, PosName.PutFullLotToLotBoxBaseHeight);
            if (posInfo != null)
            {
                ret = posInfo.Pos + lotCount * (6 * ParaHelper.Prod.TrayThickness + 5 * ParaHelper.Global.PaperThickness);
            }
            else
            {
                ShowMsg($"{StationName}->获取{AxisName.Packing_ArmZ}轴{PosName.PutFullLotToLotBoxBaseHeight}点位信息失败", MsgType.Error, true);
                AlarmManager.CreateAlarm(AlarmCode.GetPosInfoFail.ToInt(), AllStationAlarm);
            }
            return ret;
        }
        private double GetPickLotHeight(ref int trayCount)
        {
            double ret = 0;
            var posInfo = AxisInfoHelper.GetPosInfoByName(AxisName.Packing_ArmZ, PosName.PickTrayFromLotBoxBaseHeight);
            if (posInfo != null)
            {
                var model = ProcInfoHelper.Instance.GetLotCellModel(LotBoxStnType.LulLotStn, EmptySlotRCNo.r, EmptySlotRCNo.c);
                if (model != null)
                {
                    if (model.TrayCount >= trayCount)
                    {
                        ret = posInfo.Pos + (model.TrayCount - trayCount) * ParaHelper.Prod.TrayThickness;
                    }
                    else
                    {
                        ret = posInfo.Pos;
                        trayCount = model.TrayCount;//返回实际取的TrayCount数量
                    }
                }
                else
                {
                    ShowMsg($"{StationName}->从LulLotStn平台获取LotModel信息失败", MsgType.Error, true);
                    AlarmManager.CreateAlarm(AlarmCode.GetLotModelFail.ToInt(), StationNum);
                }
            }
            else
            {
                ShowMsg($"{StationName}->获取{AxisName.Packing_ArmZ}轴{PosName.PickTrayFromLotBoxBaseHeight}点位信息失败", MsgType.Error, true);
                AlarmManager.CreateAlarm(AlarmCode.GetPosInfoFail.ToInt(), AllStationAlarm);
            }
            return ret;
        }

        public int MoveToWaitPosFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->模组移动到待机位流程开始", MsgType.Notify);
                        MoveRel(AxisName.Packing_LabelZ, 5);
                        Axes = new List<AxisName>() { AxisName.Packing_ArmZ, AxisName.Packing_Shift };
                        PosNames = new List<PosName>() { PosName.WaitPos };
                        MoveAbsM(Axes, PosNames);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        var ret2 = IsAxisArrive(AxisName.Packing_LabelZ);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                        {
                            Cylinder(CylinderName.PickLabelCyl, false);
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
                        var ret = IsCylinderArrive(CylinderName.PickLabelCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            Axes = new List<AxisName>() { AxisName.Packing_ArmX, AxisName.Packing_ArmY };
                            MoveAbsM(Axes, PosNames);
                            step = 150;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1;
                        }
                        break;
                    }
                case 150:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移动到待机位流程完成", MsgType.Notify);
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1;
                        }
                        break;
                    }
                default:
                    break;
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
        }

        public void ResetBits()
        {
            SubFlowStep = -1;
        }

        public void StopHandle()
        {
        }

        public void WaitFlowDone()
        {
        }
    }
}
