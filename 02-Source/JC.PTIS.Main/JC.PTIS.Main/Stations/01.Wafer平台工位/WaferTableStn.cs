using APS168_W32;
using JC.AutoMating.ConfigInfo.StepTimeInfo;
using JC.PTIS.Main.AlarmInfo;
using JC.PTIS.Main.Devices.Scaner;
using JC.PTIS.Main.Devices.Vision;
using JC.PTIS.Main.Devices.VisionPC.MsgModel;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.Models.ModelHelper;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Params;
using JC.PTIS.Main.RotateInfo;
using JC.PTIS.Main.Service;
using StdLib.PCBaseFrame.Alarm;
using StdLib.PCBaseFrame.Args;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.PCBaseFrame.Motion;
using StdLib.PCBaseFrame.MotionInfo;
using StdLib.PCBaseFrame.Station;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Stations
{
    public class WaferTableStn : StationBaseEx, IStation
    {
        public static BinLevel CurBin = BinLevel.Bin1;
        public int StationNum { get; set; } = 1;
        public string StationName { get; set; } = "WaferTableStn";
        List<AxisName> Axes = new List<AxisName>();
        List<PosName> PosNames = new List<PosName>();//位置名称集合
        List<double> Poses = new List<double>();//实际位置集合
        List<CylinderName> Cylinders = new List<CylinderName>();
        #region 子流程状态机步骤
        int SubFlowStep = -1;
        int AxisZMoveStep = -1;
        #endregion

        int CurRowNo = 0;
        int CurColNo = 0;
        int CornerNo = 0;
        int CurDieNo = 0;
        int AlignCounter = 0;
        bool IsPickEnd = false;

        List<double[]> FourCornerDieGrabPos = new List<double[]>();//四个顶点Die的拍照位置
        List<double[]> FourCornerRealCrd = new List<double[]>();//四个顶点Die的实际坐标
        Xyt OffsetPerDie = new Xyt();
        Xyt BaseDieAbsPos = new Xyt();//首颗Die的实际绝对坐标

        DieModel CurDie = null;
        public bool IsManual { get; set; }
        public void Home()
        {
            switch (HomeStep)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->开始回零");
                        Motion.Home(AxisName.WaferTable_Thimble);
                        Motion.Home(AxisName.WaferTable_ThimbleCap);
                        ShowMsg($"{StationName}->{AxisName.WaferTable_Thimble}轴、{AxisName.WaferTable_Thimble}轴开始回零");
                        Cylinders = new List<CylinderName>() { CylinderName.WaferTableZPowerCyl, CylinderName.WaferTableZLimitCyl };
                        CylinderM(Cylinders, false);//Z轴动力气缸和限位气缸缩回，只有在Z轴需要运动时才伸出，并且T轴在特定位置才能伸出
                        ShowMsg($"{StationName}->WaferTableZ轴动力气缸、WaferTableZ轴正负极限气缸缩回");
                        HomeStep = 50;
                        break;
                    }
                case 50:
                    {
                        ShowMsg($"{StationName}->{AxisName.WaferTable_Thimble}轴、{AxisName.WaferTable_Thimble}轴回零完成");
                        ShowMsg($"{StationName}->WaferTableZ轴动力气缸、WaferTableZ轴正负极限气缸缩回到位");
                        var ret = IsCylinderArriveM(Cylinders, false);
                        if (Motion.IsHomeDone(AxisName.WaferTable_Thimble) &&
                            Motion.IsHomeDone(AxisName.WaferTable_ThimbleCap) &&
                            ret == MotionStatus.Arrived)
                        {

                            if (Motion.GetDigitalInput(InputName.WaferTableClawIsExistWafer))
                            {
                                ShowMsg($"{StationName}->WaferTable夹爪检测到Wafer，夹爪气缸夹紧");
                                Cylinder(CylinderName.WaferTableClawCyl, true);
                                HomeStep = 100;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->WaferTable夹爪未检测到Wafer，夹爪升降气缸上升");
                                Cylinder(CylinderName.WaferTableArmCyl, false);
                                HomeStep = 150;
                            }
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.WaferTable_Thimble))
                        {
                            ShowMsg($"{StationName}->{AxisName.WaferTable_Thimble}轴回零超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.WaferTable_ThimbleCap))
                        {
                            ShowMsg($"{StationName}->{AxisName.WaferTable_ThimbleCap}轴回零超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->Z轴动力气缸动作超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 100:
                    {
                        var ret = IsCylinderArrive(CylinderName.WaferTableClawCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->WaferTable夹爪夹紧到位");
                            Motion.Home(AxisName.WaferTable_ArmX);
                            ShowMsg($"{StationName}->WaferTable_ArmX轴开始回零");
                            HomeStep = 200;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->{CylinderName.WaferTableClawCyl}气缸夹紧超时", MsgType.Error);
                            HomeStep = 50;
                        }
                        break;
                    }
                case 150:
                    {
                        var ret = IsCylinderArrive(CylinderName.WaferTableArmCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->WaferTable夹爪升降气缸上升到位");
                            Motion.Home(AxisName.WaferTable_ArmX);
                            ShowMsg($"{StationName}->WaferTable_ArmX轴回零");
                            HomeStep = 200;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->{CylinderName.WaferTableClawCyl}气缸缩回超时", MsgType.Error);
                            HomeStep = 350;
                        }
                        break;
                    }
                case 200:
                    {
                        if (Motion.IsHomeDone(AxisName.WaferTable_ArmX))
                        {
                            ShowMsg($"{StationName}->WaferTable_ArmX轴回零完成");
                            if (Motion.GetDigitalInput(InputName.WaferTableClawIsExistWafer))
                            {
                                MoveAbs(AxisName.WaferTable_ArmX, PosName.LoadWaferPos);
                                ShowMsg($"{StationName}->WaferTable_ArmX轴移动到LoadWaferPos");
                                HomeStep = 250;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->WaferTable_ArmX轴移动到WaitPos");
                                MoveAbs(AxisName.WaferTable_ArmX, PosName.WaitPos);
                                HomeStep = 450;
                            }
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.WaferTable_ArmX))
                        {
                            ShowMsg($"{StationName}->{AxisName.WaferTable_ArmX}轴回零超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 250:
                    {
                        var ret = IsAxisArrive(AxisName.WaferTable_ArmX, PosName.LoadWaferPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->WaferTable_ArmX轴到达LoadWaferPos");
                            Cylinder(CylinderName.WaferTableClawCyl, false);
                            ShowMsg($"{StationName}->夹爪气缸松开");
                            HomeStep = 300;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->{AxisName.WaferTable_ArmX}轴移动到LoadWaferPos超时", MsgType.Error);
                            HomeStep = 200;
                        }
                        break;
                    }
                case 300:
                    {
                        var ret = IsCylinderArrive(CylinderName.WaferTableClawCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪气缸松开到位");
                            MoveRel(AxisName.WaferTable_ArmX, -10);
                            ShowMsg($"{StationName}->WaferTable_ArmX轴往后移动10毫米，夹爪脱离Wafer");
                            HomeStep = 350;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->{CylinderName.WaferTableClawCyl}气缸缩回超时", MsgType.Error);
                            HomeStep = 250;
                        }
                        break;
                    }
                case 350:
                    {
                        var ret = IsAxisArrive(AxisName.WaferTable_ArmX);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪升降气缸上升");
                            Cylinder(CylinderName.WaferTableArmCyl, false);
                            HomeStep = 400;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->{AxisName.WaferTable_X}轴往后移动10毫米超时", MsgType.Error);
                        }
                        break;
                    }
                case 400:
                    {
                        var ret = IsCylinderArrive(CylinderName.WaferTableArmCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪升降气缸上升到位");
                            MoveAbs(AxisName.WaferTable_ArmX, PosName.WaitPos);
                            ShowMsg($"{StationName}->WaferTable_ArmX轴移动到待机位置");
                            HomeStep = 450;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            HomeStep = 350;
                        }
                        break;
                    }
                case 450:
                    {
                        var ret = IsAxisArrive(AxisName.WaferTable_ArmX, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->WaferTable_X轴到达待机位置，等待上料转塔PickDieZ轴回零完成");
                            HomeStep = 500;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->{AxisName.WaferTable_ArmX}轴移动到待机位置超时", MsgType.Error);
                            HomeStep = 400;
                        }
                        break;
                    }
                case 500:
                    {
                        if (Motion.IsHomeDone(AxisName.PickDie_Z))
                        {
                            Motion.Home(AxisName.WaferTable_T);
                            ShowMsg($"{StationName}->{AxisName.WaferTable_T}轴开始回零");
                            HomeStep = 550;
                        }
                        break;
                    }
                case 550:
                    {
                        if (Motion.IsHomeDone(AxisName.WaferTable_T))
                        {
                            ShowMsg($"{StationName}->WaferTable_T回零完成");
                            MoveAbs(AxisName.WaferTable_T, PosName.WaferTableZCylOutPos);
                            ShowMsg($"{StationName}->WaferTable_T移动到动力气缸伸出位置");
                            HomeStep = 600;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.WaferTable_T))
                        {
                            ShowMsg($"{StationName}->{AxisName.WaferTable_T}轴回零超时", MsgType.Error);
                            HomeStep = 500;
                        }
                        break;
                    }
                case 600:
                    {
                        var ret = IsAxisArrive(AxisName.WaferTable_T, PosName.WaferTableZCylOutPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            Cylinders = new List<CylinderName>() { CylinderName.WaferTableZPowerCyl, CylinderName.WaferTableZLimitCyl };
                            CylinderM(Cylinders, true);//Z轴动力气缸和限位气缸只有在Z轴需要运动时才伸出，并且T轴在特定位置才能伸出
                            ShowMsg($"{StationName}->动力气缸和极限气缸伸出");
                            HomeStep = 650;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->{AxisName.PickDie_Z}轴移动到WaitPos超时", MsgType.Error);
                            HomeStep = 550;
                        }
                        break;
                    }
                case 650:
                    {
                        var ret = IsCylinderArriveM(Cylinders, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->动力气缸和极限气缸伸出到位");
                            Motion.Home(AxisName.WaferTable_Z);
                            ShowMsg($"{StationName}->WaferTable_X轴、WaferTable_Y轴回零");
                            HomeStep = 700;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->Z轴动力气缸和极限气缸伸出超时", MsgType.Error);
                            HomeStep = 600;
                        }
                        break;
                    }
                case 700:
                    {
                        if (Motion.IsHomeDone(AxisName.WaferTable_Z))
                        {
                            MoveAbs(AxisName.WaferTable_Z, PosName.WaitPos);
                            ShowMsg($"{StationName}->WaferTable_Z轴移动到待机位置");
                            HomeStep = 750;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.WaferTable_Z))
                        {
                            ShowMsg($"{StationName}->{AxisName.WaferTable_Z}轴回零超时", MsgType.Error);
                            HomeStep = 650;
                        }
                        break;
                    }
                case 750:
                    {
                        var ret = IsAxisArrive(AxisName.WaferTable_Z, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->动力气缸和极限气缸缩回");
                            CylinderM(Cylinders, false);
                            HomeStep = 800;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->{AxisName.WaferTable_Z}轴移动到待机位置超时", MsgType.Error);
                            HomeStep = 700;
                        }
                        break;
                    }
                case 800:
                    {
                        var ret = IsCylinderArriveM(Cylinders, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->WaferTable_X轴、WaferTable_Y轴回零");
                            Motion.Home(AxisName.WaferTable_X);
                            Motion.Home(AxisName.WaferTable_Y);
                            HomeStep = 850;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            HomeStep = 750;
                        }
                        break;
                    }
                case 850:
                    {
                        if (Motion.IsHomeDone(AxisName.WaferTable_X) &&
                            Motion.IsHomeDone(AxisName.WaferTable_Y))
                        {
                            ShowMsg($"{StationName}->WaferTable_X轴、WaferTable_Y轴回零完成，移动到{PosName.WaitPos}位置");
                            Motion.MoveAbs(AxisName.WaferTable_X, PosName.WaitPos);
                            Motion.MoveAbs(AxisName.WaferTable_Y, PosName.WaitPos);
                            HomeStep = 900;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.WaferTable_X))
                        {
                            ShowMsg($"{StationName}->{AxisName.WaferTable_X}轴回零超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.WaferTable_Y))
                        {
                            ShowMsg($"{StationName}->{AxisName.WaferTable_Y}轴回零超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 900:
                    {
                        if (Motion.IsArrive(AxisName.WaferTable_X, PosName.WaitPos) &&
                           Motion.IsArrive(AxisName.WaferTable_Y, PosName.WaitPos))
                        {
                            ShowMsg($"{StationName}->WaferTable_X轴、WaferTable_Y轴移动到{PosName.WaitPos}位置完成");
                            UpdateHomeSts(StationName, StationNum, true);
                            HomeStep = 0;
                            if (Global.RunMode == RunMode.空跑模式)
                            {
                                IOInfoHelper.GetInputInfoByName(InputName.WaferTableIsExistWafer.ToString(), out int _).IsShield = false;
                            }
                            break;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.WaferTable_X))
                        {
                            ShowMsg($"{StationName}->{AxisName.WaferTable_X}轴移动到{PosName.WaitPos}位置超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.WaferTable_Y))
                        {
                            ShowMsg($"{StationName}->{AxisName.WaferTable_Y}轴移动到{PosName.WaitPos}位置超时", MsgType.Error);
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
                            ShowMsg($"{StationName}->WaferTable模组移动到等待位");
                            SubFlowStep = 1;
                            AutoRunStep = 50;
                            break;
                        }
                    case 50:
                        {
                            SubFlowStep = MoveToWaitPosFlow(SubFlowStep);
                            if (SubFlowStep == 0)
                            {
                                IsManual = false;
                                ShowMsg($"{StationName}->判断WaferTable上是否有Wafer");
                                AutoRunStep = 100;
                            }
                            break;
                        }
                    case 100:
                        {
                            if (Motion.GetDigitalInput(InputName.WaferTableIsExistWafer))
                            {
                                ShowMsg($"{StationName}->WaferTable上有Wafer，判断是否已经取完Die");
                                var wafer = ProcInfoHelper.Instance.GetWaferModel(WaferPosEnum.WaferTable);
                                if (wafer != null && !wafer.IsPickEnd)
                                {
                                    ShowMsg($"{StationName}->Wafer没有取完Die，执行对位流程");
                                    SubFlowStep = 1;
                                    AutoRunStep = 200;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->Wafer取完Die，执行Wafer下料流程");
                                    SubFlowStep = 1;
                                    AutoRunStep = 300;
                                }
                            }
                            else
                            {
                                ShowMsg($"{StationName}->WaferTable上没有Wafer，等待WaferBuffer有Wafer信号");
                                AutoRunStep = 400;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    RegisterService.Instance.SetBoolReg(RegisterEnum.WaferBufferExistWafer);
                                }
                            }
                            break;
                        }
                    case 200:
                        {
                            SubFlowStep = WaferAlignFlow(SubFlowStep);
                            if (SubFlowStep == 0)
                            {
                                ShowMsg($"{StationName}->对位流程执行完成，执行AlignDie流程");
                                SubFlowStep = 1;
                                AutoRunStep = 250;
                            }
                            break;
                        }
                    case 250:
                        {
                            SubFlowStep = AlignDieFlow(SubFlowStep);
                            if (SubFlowStep == 0)
                            {
                                IsPickEnd = true;
                                ShowMsg($"{StationName}->AlignDie流程执行完成，执行Wafer下料流程");
                                SubFlowStep = 1;
                                AutoRunStep = 300;
                            }
                            break;
                        }
                    case 300:
                        {
                            SubFlowStep = UnloadWaferFlow(SubFlowStep);
                            if (SubFlowStep == 0)
                            {
                                ShowMsg($"{StationName}->下料流程执行完成，等待WaferBuffer有Wafer信号");
                                AutoRunStep = 400;
                                CurRowNo = 0;
                                CurColNo = 0;
                                CurDieNo = 0;
                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.WaferTableIsExistWafer.ToString(), out int _).IsShield = false;
                                    Thread.Sleep(2000);
                                }
                            }
                            break;
                        }
                    case 400:
                        {
                            if (GetBoolReg(RegisterEnum.WaferBufferExistWafer))
                            {
                                ResetBoolReg(RegisterEnum.WaferBufferExistWafer);
                                ShowMsg($"{StationName}->WaferBuffer平台上有Wafer，执行上料流程");
                                SubFlowStep = 1;
                                AutoRunStep = 450;
                            }
                            break;
                        }
                    case 450:
                        {
                            SubFlowStep = LoadWaferFlow(SubFlowStep);
                            if (SubFlowStep == 0)
                            {
                                ShowMsg($"{StationName}->上料流程执行完成，执行对位流程");
                                SubFlowStep = 1;
                                AutoRunStep = 200;
                                CurRowNo = 0;
                                CurColNo = 0;
                                CurDieNo = 0;
                                CurBin = BinLevel.Bin1;
                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.WaferTableIsExistWafer.ToString(), out int _).IsShield = true;
                                    Thread.Sleep(500);
                                }
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

        public int MoveToWaitPosFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->WaferTable模组Z轴移动到等待位");
                        Axes = new List<AxisName>() { AxisName.WaferTable_Thimble, AxisName.WaferTable_ThimbleCap };
                        PosNames = new List<PosName>() { PosName.WaitPos };
                        MoveAbsM(Axes, PosNames);
                        Cylinders = new List<CylinderName>() { CylinderName.WaferTableArmCyl, CylinderName.WaferTableZPowerCyl, CylinderName.WaferTableZLimitCyl };
                        CylinderM(Cylinders, false);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        var ret2 = IsCylinderArriveM(Cylinders, false);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                        {
                            if (Motion.IsArrive(AxisName.PickDie_Z))
                            {
                                ShowMsg($"{StationName}->WaferTable模组XY轴移动到等待位");
                                Axes = new List<AxisName>() { AxisName.WaferTable_X, AxisName.WaferTable_Y, AxisName.WaferTable_T };
                                PosNames = new List<PosName>() { PosName.WaitPos };
                                MoveAbsM(Axes, PosNames);
                                step = 100;
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
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->WaferTable模组移动到等待位完成");
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
        public int LoadWaferFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->上Wafer流程开始，Z轴移动到上Wafer位置", MsgType.Notify);
                        AxisZMoveStep = 1;
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        AxisZMoveStep = WaferTableZMove(AxisZMoveStep, PosName.LoadWaferPos);
                        if (AxisZMoveStep == 0)
                        {
                            ShowMsg($"{StationName}->XY轴移动到上Wafer位置");
                            Axes = new List<AxisName>() { AxisName.WaferTable_X, AxisName.WaferTable_Y, AxisName.WaferTable_T };
                            PosNames = new List<PosName>() { PosName.LoadWaferPos };
                            MoveAbsM(Axes, PosNames);
                            Cylinders = new List<CylinderName>() { CylinderName.WaferTableArmCyl, CylinderName.WaferTableClawCyl };
                            CylinderM(Cylinders, false);
                            RegisterService.Instance.SetBoolReg(RegisterEnum.WaferTableLoadWaferReq);
                            step = 100;
                        }
                        else if (AxisZMoveStep == -1)
                        {
                            ShowMsg($"{StationName}->Z轴移动到LoadWaferPos位置失败", MsgType.Error, true);
                            AlarmManager.CreateAlarm(AlarmCode.ProgramExceptionStart.ToInt() + StationNum, AllStationAlarm);
                        }
                        break;
                    }
                case 100:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        var ret2 = IsCylinderArriveM(Cylinders, false);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->XY轴到达上Wafer位置，等待WaferBuffer工位允许取放Wafer信号");
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
                        if (GetBoolReg(RegisterEnum.WaferBufferAllowPickPlaceWafer))
                        {
                            ResetBoolReg(RegisterEnum.WaferBufferAllowPickPlaceWafer);
                            ShowMsg($"{StationName}->收到WaferBuffer工位允许取放Wafer信号，夹爪升降气缸下降");
                            Cylinder(CylinderName.WaferTableArmCyl, true);
                            step = 200;
                        }
                        break;
                    }
                case 200:
                    {
                        var ret = IsCylinderArrive(CylinderName.WaferTableArmCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪升降气缸下降到位，夹爪X轴移动到取Wafer位置");
                            MoveAbs(AxisName.WaferTable_ArmX, PosName.ArmPickWaferFromBufferPos);
                            step = 250;
                        }
                        break;
                    }
                case 250:
                    {
                        var ret = IsAxisArrive(AxisName.WaferTable_ArmX, PosName.ArmPickWaferFromBufferPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪X轴到达取Wafer位置，夹爪夹紧Wafer");
                            Cylinder(CylinderName.WaferTableClawCyl, true);
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
                        var ret = IsCylinderArrive(CylinderName.WaferTableClawCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪夹紧Wafer，夹爪X轴移动到放Wafer位置");
                            MoveAbs(AxisName.WaferTable_ArmX, PosName.ArmPickWaferToTablePos);
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
                        var ret = IsAxisArrive(AxisName.WaferTable_ArmX, PosName.ArmPickWaferToTablePos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪X轴到达放Wafer位置，夹爪气缸松开");
                            Cylinder(CylinderName.WaferTableClawCyl, false);
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
                        var ret = IsCylinderArrive(CylinderName.WaferTableClawCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪X轴到达放Wafer位置，X轴后退10毫米，脱离Wafer");
                            MoveRel(AxisName.WaferTable_ArmX, -10);
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
                        var ret = IsAxisArrive(AxisName.WaferTable_ArmX);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪升降气缸缩回");
                            Cylinder(CylinderName.WaferTableArmCyl, false);
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
                        var ret = IsCylinderArrive(CylinderName.WaferTableArmCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪X轴移动到待机位");
                            MoveAbs(AxisName.WaferTable_ArmX, PosName.WaitPos);
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
                        var ret = IsAxisArrive(AxisName.WaferTable_ArmX, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            AxisZMoveStep = 1;
                            ShowMsg($"{StationName}->Z轴移动到Lift位");
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
                        AxisZMoveStep = WaferTableZMove(AxisZMoveStep, PosName.LiftPos);
                        if (AxisZMoveStep == 0)
                        {
                            ShowMsg($"{StationName}->Z轴到达Lift位，WaferTable模组上料完成", MsgType.Notify);
                            RegisterService.Instance.SetBoolReg(RegisterEnum.WaferTableLoadWaferDone);
                            var wafer = ProcInfoHelper.Instance.PickWafer(WaferPosEnum.WaferBufferUpLayer);
                            if (wafer != null)
                            {
                                //T08915-12-H1.txt
                                var tempWafer = WaferMapHelper.Instance.Parse(wafer.MapFilePath);
                                tempWafer.Code = wafer.Code;
                                tempWafer.FoupNo = wafer.FoupNo;
                                tempWafer.LayerNo = wafer.LayerNo;
                                tempWafer.MapFilePath = wafer.MapFilePath;
                                ProcInfoHelper.Instance.PutWafer(WaferPosEnum.WaferTable, tempWafer);
                            }
                            step = 0;
                        }
                        else if (AxisZMoveStep == -1)
                        {
                            ShowMsg($"{StationName}->Z轴移动到LiftPos位置失败", MsgType.Error, true);
                            AlarmManager.CreateAlarm(AlarmCode.ProgramExceptionStart.ToInt() + StationNum, AllStationAlarm);
                        }
                        break;
                    }
                default:
                    break;
            }
            return step;
        }
        public int UnloadWaferFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->下Wafer流程开始，Z轴移动到下Wafer位置", MsgType.Notify);
                        AxisZMoveStep = 1;
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        AxisZMoveStep = WaferTableZMove(AxisZMoveStep, PosName.LoadWaferPos);//和上Wafer位置相同
                        if (AxisZMoveStep == 0)
                        {
                            ShowMsg($"{StationName}->XYT轴移动到下Wafer位置");
                            Axes = new List<AxisName>() { AxisName.WaferTable_X, AxisName.WaferTable_Y, AxisName.WaferTable_T };
                            PosNames = new List<PosName>() { PosName.LoadWaferPos };//和上Wafer位置相同
                            MoveAbsM(Axes, PosNames);
                            Cylinders = new List<CylinderName>() { CylinderName.WaferTableArmCyl, CylinderName.WaferTableClawCyl };
                            CylinderM(Cylinders, false);
                            ResetBoolReg(RegisterEnum.WaferTableUnloadWaferDone);
                            step = 100;
                        }
                        else if (AxisZMoveStep == -1)
                        {
                            ShowMsg($"{StationName}->Z轴移动到LoadWaferPos位置失败", MsgType.Error, true);
                            AlarmManager.CreateAlarm(AlarmCode.ProgramExceptionStart.ToInt() + StationNum, AllStationAlarm);
                        }
                        break;
                    }
                case 100:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        var ret2 = IsCylinderArriveM(Cylinders, false);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                        {
                            RegisterService.Instance.SetBoolReg(RegisterEnum.WaferTableUnloadWaferReq);
                            ShowMsg($"{StationName}->XYT轴到达下Wafer位置，等待WaferBuffer工位允许取放Wafer信号");
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
                        if (GetBoolReg(RegisterEnum.WaferBufferAllowPickPlaceWafer))
                        {
                            ResetBoolReg(RegisterEnum.WaferBufferAllowPickPlaceWafer);
                            ShowMsg($"{StationName}->收到WaferBuffer工位允许取放Wafer信号");
                            MoveAbs(AxisName.WaferTable_ArmX, PosName.ArmPickWaferToTablePos);
                            step = 200;
                        }
                        break;
                    }
                case 200:
                    {
                        var ret = IsAxisArrive(AxisName.WaferTable_ArmX, PosName.ArmPickWaferToTablePos);
                        if (ret == MotionStatus.Arrived)
                        {
                            Cylinder(CylinderName.WaferTableArmCyl, true);
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
                        var ret = IsCylinderArrive(CylinderName.WaferTableArmCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            Cylinder(CylinderName.WaferTableClawCyl, true);
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
                        var ret = IsCylinderArrive(CylinderName.WaferTableClawCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            Cylinder(CylinderName.WaferTableArmCyl, false);
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
                        var ret = IsCylinderArrive(CylinderName.WaferTableArmCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            MoveAbs(AxisName.WaferTable_ArmX, PosName.ArmPickWaferFromBufferPos);
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
                        var ret = IsAxisArrive(AxisName.WaferTable_ArmX, PosName.ArmPickWaferFromBufferPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            Cylinder(CylinderName.WaferTableClawCyl, false);
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
                        var ret = IsCylinderArrive(CylinderName.WaferTableClawCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            MoveAbs(AxisName.WaferTable_ArmX, PosName.WaitPos);
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
                        var ret = IsAxisArrive(AxisName.WaferTable_ArmX, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->WaferTable模组下料完成");
                            RegisterService.Instance.SetBoolReg(RegisterEnum.WaferTableUnloadWaferDone);
                            var model = ProcInfoHelper.Instance.PickWafer(WaferPosEnum.WaferTable);
                            if (model != null)
                            {
                                ProcInfoHelper.Instance.PutWafer(WaferPosEnum.WaferBufferDownLayer, model);
                            }
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


        public int WaferAlignFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->Wafer对位流程开始", MsgType.Notify);
                        ShowMsg($"{StationName}->WaferTable模组移动到第一个MarkDie位置");
                        Axes = new List<AxisName>() { AxisName.WaferTable_X, AxisName.WaferTable_Y };
                        PosNames = new List<PosName>() { PosName.MarkDie1Pos };
                        MoveAbsM(Axes, PosNames);
                        AlignCounter = 0;
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->WaferTable模组移动到第一个MarkDie位置完成，请求对位PC拍第一个MarkDie");
                            var ret2 = AlignHelper.Instance.Send<BodyBase>(VisionCmd.Grab, AlignStationEnum.WaferMark, 0);
                            if (ret2.Code == 0)
                            {
                                ShowMsg($"{StationName}->对位PC拍第一个MarkDie完成，模组移动到第二个MarkDie位置");
                                Axes = new List<AxisName>() { AxisName.WaferTable_X, AxisName.WaferTable_Y };
                                PosNames = new List<PosName>() { PosName.MarkDie2Pos };
                                MoveAbsM(Axes, PosNames);
                                AlignCounter = 0;
                                step = 100;
                            }
                            else
                            {
                                AlignFailCounter("MarkDie1拍照超时", AlarmCode.AlignPCMarkDie1AlignFail);
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->WaferTable模组移动到第一个MarkDie位置超时", MsgType.Error);
                            step = 1;
                        }
                        break;
                    }
                case 100:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->WaferTable模组移动到第二个MarkDie位置完成，请求对位PC拍第二个MarkDie");
                            var ret2 = AlignHelper.Instance.Send<BodyBase>(VisionCmd.Grab, AlignStationEnum.WaferMark, 1);
                            if (ret2.Code == 0)
                            {
                                ShowMsg($"{StationName}->对位PC拍第二个MarkDie完成");
                                AlignCounter = 0;
                                step = 150;
                            }
                            else
                            {
                                AlignFailCounter("MarkDie2拍照超时", AlarmCode.AlignPCMarkDie2AlignFail);
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->WaferTable模组移动到第二个MarkDie位置超时", MsgType.Error);
                            step = 50;
                        }
                        break;
                    }
                case 150:
                    {
                        ShowMsg($"{StationName}->获取Wafer对位补正值");
                        var waferOffset = AlignHelper.Instance.Send<AlignRspBody>(VisionCmd.Align, AlignStationEnum.WaferMark);
                        if (waferOffset != null)
                        {
                            Axes = new List<AxisName>() { AxisName.WaferTable_X, AxisName.WaferTable_Y, AxisName.WaferTable_T };
                            Poses = waferOffset.Offset.ToList();
                            MoveRelM(Axes, Poses);
                            ShowMsg($"{StationName}->获取Wafer对位补正值完成：{waferOffset.Offset.ToString()}");
                            step = 200;
                        }
                        else
                        {
                            AlignFailCounter("获取Wafer对位补正值失败", AlarmCode.AlignPCCalcWaferOffsetFail);
                        }
                        break;
                    }
                case 200:
                    {
                        var retX = IsAxisArriveM(Axes);
                        if (retX == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->WaferTable模组补正完成");
                            //AlignHelper.Instance.Send<RspBaseBody>(VisionCmd.GrabMark, 1);
                            //ShowMsg($"{StationName}->MarkDie2拍照完成，WaferTable模组移动到左上角Die位置");
                            //Axes = new List<AxisName>() { AxisName.WaferTable_X, AxisName.WaferTable_Y };
                            //var die1XPos = (ParaHelper.Prod.MarkDie2ColNo - 0) * ParaHelper.Prod.DieWidth;//第一颗Die的X坐标位置
                            //var die1YPos = (ParaHelper.Prod.MarkDie2RowNo - 0) * ParaHelper.Prod.DieHeight;//第一颗Die的Y坐标位置
                            //Poses = new List<double>() { -die1XPos, -die1YPos };
                            //MoveRelM(Axes, Poses);
                            step = 220;
                        }
                        else if (retX == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->模组移动对位补正值超时", MsgType.Error);
                            step = 150;
                        }
                        break;
                    }
                case 220:
                    {
                        var retX = IsAxisArriveM(Axes);
                        if (retX == MotionStatus.Arrived)
                        {
                            BaseDieAbsPos.X = Motion.GetAxisActPos(AxisName.WaferTable_X);
                            BaseDieAbsPos.Y = Motion.GetAxisActPos(AxisName.WaferTable_Y);
                            //ShowMsg($"{StationName}->WaferTable模组移动到左上角Die位置完成，开始计算四个顶点的实际坐标");
                            step = 450;
                        }
                        else if (retX == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->WaferTable模组移动到左上角Die位置超时", MsgType.Error);
                            step = 200;
                        }
                        break;
                    }
                case 250:
                    {
                        CornerNo = 0;
                        FourCornerRealCrd = new List<double[]>();
                        var ret = GetFourCornerDieGrabPos();
                        if (ret && FourCornerDieGrabPos.Count > 0)
                        {
                            var crdStr = string.Empty;
                            FourCornerDieGrabPos.ForEach(p => { crdStr += $"X:{p[0]},Y:{p[1]},"; });
                            ShowMsg($"{StationName}->计算四个顶点的实际坐标完成，顶点坐标：{crdStr}");
                            step = 270;
                        }
                        else
                        {
                            AlarmManager.CreateAlarm(AlarmCode.CalcFourCornerDieGrabPosFail.ToInt(), StationNum);
                        }
                        break;
                    }
                case 270:
                    {
                        ShowMsg($"{StationName}->WaferTable模组移动到第{CornerNo}个Die位置，X:{FourCornerDieGrabPos[CornerNo][0]}，Y:{FourCornerDieGrabPos[CornerNo][1]}");
                        Axes = new List<AxisName>() { AxisName.WaferTable_X, AxisName.WaferTable_Y };
                        Poses = new List<double>() { FourCornerDieGrabPos[CornerNo][0], FourCornerDieGrabPos[CornerNo][1] };
                        MoveAbsM(Axes, Poses);
                        step = 300;
                        break;
                    }
                case 300:
                    {
                        var ret = IsAxisArriveM(Axes, Poses);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->WaferTable模组到达第{CornerNo}个Die位置，请求对位PC计算坐标");
                            var body = AlignHelper.Instance.Send<AlignRspBody>(VisionCmd.GetDieOffset, AlignStationEnum.None, CornerNo);
                            if (body == null || body.Offset == null)
                            {
                                ShowMsg($"{StationName}->对位PC计算第{CornerNo}个Die坐标失败，弹窗请求处理", MsgType.Error);
                                AlarmManager.CreateAlarm(AlarmCode.AlignPCCalcDieOffsetFail.ToInt(), StationNum);
                                var result = AskHowToDoRequest(StationNum, $"对位PC计算第{CornerNo}个Die坐标失败，弹窗请求处理");
                                switch (result)
                                {
                                    case AskResult.Skip:
                                        {
                                            ShowMsg($"{StationName}->处理结果：跳过");
                                            var offset2 = new Xyt() { X = 0, Y = 0, T = 0 };
                                            ShowMsg($"{StationName}->使用默认坐标：X={0}, Y={0}, T={0}");
                                            var realCrd = CalcRealCrd(CornerNo, offset2.ToArray());
                                            FourCornerRealCrd.Add(realCrd.ToArray());
                                            step = 320;
                                        }
                                        break;
                                    case AskResult.Retry:
                                        ShowMsg($"{StationName}->处理结果：重试");
                                        break;
                                    case AskResult.Stop:
                                        {
                                            ShowMsg($"{StationName}->处理结果：停机");
                                            step = -1;
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                            else
                            {
                                ShowMsg($"{StationName}->对位PC返回坐标：{body.Offset.ToString()}");
                                var realCrd = CalcRealCrd(CornerNo, body.Offset.ToArray());
                                FourCornerRealCrd.Add(realCrd.ToArray());
                                step = 320;
                            }
                            break;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->WaferTable模组移动到第{CornerNo}个位置超时", MsgType.Error);
                            step = 270;
                        }
                        break;
                    }
                case 320:
                    {
                        CornerNo++;
                        if (CornerNo >= FourCornerDieGrabPos.Count)
                        {
                            OffsetPerDie = null;
                            ShowMsg($"{StationName}->计算四个顶点的实际坐标完成，计算扩摸后每颗Die的偏移量");
                            var offset2 = CalcDieOffset();
                            if (offset2 != null)
                            {
                                ShowMsg($"{StationName}->计算每颗Die的偏移量完成，偏移量：X={offset2.X}, Y={offset2.Y}");
                                OffsetPerDie = offset2;
                                step = 350;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->计算每颗Die的偏移量失败", MsgType.Error);
                                AlarmManager.CreateAlarm(AlarmCode.CalcDieOffsetFail.ToInt(), AllStationAlarm);
                                step = -1;
                            }
                        }
                        else
                        {
                            step = 270;
                        }
                        break;
                    }
                case 350:
                    {
                        ShowMsg($"{StationName}->WaferTable模组移动到首颗Die位置");
                        Axes = new List<AxisName>() { AxisName.WaferTable_X, AxisName.WaferTable_Y };
                        Poses = new List<double>() { FourCornerDieGrabPos[0][0], FourCornerDieGrabPos[0][1] };
                        MoveAbsM(Axes, Poses);
                        step = 400;
                        break;
                    }
                case 400:
                    {
                        var ret = IsAxisArriveM(Axes, Poses);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->WaferTable模组移动到首颗Die位置完成，等待4工位转盘下料完成");
                            step = 450;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->WaferTable模组移动到首颗Die位置超时", MsgType.Error);
                            step = 400;
                        }
                        break;
                    }
                case 450:
                    {
                        var ret = IsExistTrayInStn4();
                        if (!ret || !IsPickEnd)//如果是最后一个Bin等级，要等到4工位转盘下完Tray后才能结束Wafer对位流程
                        {
                            if (!ret)
                            {
                                var waferModel = ProcInfoHelper.Instance.GetWaferModel(WaferPosEnum.WaferTable);
                                ProcInfoHelper.Instance.InitBinMapDic(waferModel);
                                IsPickEnd = false;
                            }
                            ShowMsg($"{StationName}->4工位转盘没有Tray，Wafer对位流程结束", MsgType.Notify);
                            step = 0;
                        }
                        break;
                    }
                default:
                    break;
            }
            return step;
        }
        public int AlignDieFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->Die对位流程开始，顶针气缸上升", MsgType.Notify);
                        var wafer = ProcInfoHelper.Instance.GetWaferModel(WaferPosEnum.WaferTable);
                        wafer.IsPickEnd = false;
                        wafer.StartTime = DateTime.Now;
                        //Cylinder(CylinderName.WaferTableThimbleCyl, true);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        //var ret = IsCylinderArrive(CylinderName.WaferTableThimbleCyl, true);
                        MotionStatus ret = MotionStatus.Arrived;
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->顶针气缸上升完成");
                            step = 100;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->顶针气缸上升超时", MsgType.Error);
                            step = 1;
                        }
                        break;
                    }
                case 100:
                    {
                        ShowMsg($"{StationName}->获取{CurBin}等级第{CurDieNo}颗Die信息", MsgType.Notify);
                        var temp = ProcInfoHelper.Instance.GetDie(CurDieNo, CurBin);
                        if (temp != null)
                        {
                            CurDie = temp.DeepCopy();
                            ShowMsg($"{StationName}->{CurBin}等级第{CurDieNo}颗Die信息获取成功，行号：{CurDie.InWaferRowNo}，列号：{CurDie.InWaferColNo}");
                            CurRowNo = CurDie.InWaferRowNo;
                            CurColNo = CurDie.InWaferColNo;
                            Axes = new List<AxisName>() { AxisName.WaferTable_X, AxisName.WaferTable_Y };
                            var pos = GetDieGrabPos(CurRowNo, CurColNo);
                            if (pos != null )
                            {
                                ShowMsg($"{StationName}->WaferTable模组移动到第{CurRowNo}行第{CurColNo}列颗Die位置：{pos?.ToString()}");
                                Poses = pos.ToList();
                                StepTimeInfoHelper.GetInfoByName(StepTimeEnum.DieMove.ToString())?.Start();
                                MoveAbsM(Axes, Poses);
                                step = 150;
                            }
                            else
                            {
                                if (pos == null)
                                {
                                    ShowMsg($"{StationName}->获取第{CurRowNo}行第{CurColNo}列颗Die位置失败", MsgType.Error);
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->获取第{CurRowNo}行第{CurColNo}列颗Die位置失败,Pos:{pos.ToString()}", MsgType.Error);
                                }
                                AlarmManager.CreateAlarm(AlarmCode.GetDieGrabPosFail.ToInt(), StationNum);
                            }
                        }
                        else
                        {
                            ShowMsg($"{StationName}->{CurBin}等级Die全部取完，开始取下一等级的Die");
                            CurBin++;
                            CurDieNo = 0;
                            if (CurBin > BinLevel.Bin6)
                            {
                                var wafer = ProcInfoHelper.Instance.GetWaferModel(WaferPosEnum.WaferTable);
                                wafer.IsPickEnd = true;
                                wafer.EndTime = DateTime.Now;
                                ShowMsg($"{StationName}->Wafer取Die完成，顶针气缸下降");
                                Cylinder(CylinderName.WaferTableThimbleCyl, false);
                                RegisterService.Instance.SetBoolReg(RegisterEnum.WaferEmpty);
                                step = 400;
                            }
                        }
                        break;
                    }

                case 150:
                    {
                        var ret = IsAxisArriveM(Axes, Poses);
                        if (ret == MotionStatus.Arrived)
                        {
                            StepTimeInfoHelper.GetInfoByName(StepTimeEnum.DieMove.ToString())?.End();
                            ShowMsg($"{StationName}->WaferTable模组移动到第{CurRowNo}行第{CurColNo}列颗Die位置完成，发送AlignDie请求给AlignPC，Station：{AlignStationEnum.Die_WaferTable}");
                            StepTimeInfoHelper.GetInfoByName(StepTimeEnum.DieAlign.ToString())?.Start();
                            var dieIndex = CurRowNo * ParaHelper.Prod.WaferColCount + CurColNo;
                            var body = AlignHelper.Instance.Send<AlignRspBody>(VisionCmd.Align, AlignStationEnum.Die_WaferTable, dieIndex);
                            if (body != null && body.Offset != null)
                            {
                                StepTimeInfoHelper.GetInfoByName(StepTimeEnum.DieAlign.ToString())?.End();
                                ShowMsg($"{StationName}->WaferTable模组对第{CurRowNo}行第{CurColNo}列颗Die对位完成，偏移量：{body.Offset.ToString()}");
                                Poses = body.Offset.ToList();
                                MoveRelM(Axes, Poses);
                                step = 200;
                            }
                            else
                            {
                                AlignFailCounter($"WaferTable模组对第{CurRowNo}行第{CurColNo}列颗Die对位失败", AlarmCode.DieAlignFail);
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->WaferTable模组移动到第{CurRowNo}行第{CurColNo}列颗Die位置超时", MsgType.Error);
                            step = 100;
                        }
                        break;
                    }
                case 200:
                    {
                        var ret = IsAxisArriveM(Axes);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->WaferTable模组对第{CurRowNo}行第{CurColNo}列颗Die对位完成，顶针帽顶起");
                            MoveAbs(AxisName.WaferTable_ThimbleCap, PosName.LiftPos);
                            step = 250;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->WaferTable模组对第{CurRowNo}行第{CurColNo}列颗Die对位超时", MsgType.Error);
                            step = 150;
                        }
                        break;
                    }
                case 250:
                    {
                        var ret = IsAxisArrive(AxisName.WaferTable_ThimbleCap, PosName.LiftPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            Vacuum(VacuumName.WaferTableThimbleCapVac, true);
                            ShowMsg($"{StationName}->顶针帽顶起完成，真空吸附，发送Die对位完成给PickDie工位，等待取Die完成");
                            RegisterService.Instance.SetBoolReg(RegisterEnum.AlignDieDone);
                            step = 300;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->顶针顶起超时", MsgType.Error);
                            step = 200;
                        }
                        break;
                    }
                case 300:
                    {
                        if (GetBoolReg(RegisterEnum.PickDieStnPickDone) || IsManual)
                        {
                            ShowMsg($"{StationName}->PickDie工位取Die完成，真空关闭，顶针帽和顶针下降");
                            Vacuum(VacuumName.WaferTableThimbleCapVac, false);
                            var model = ProcInfoHelper.Instance.PickDieFromWafer(CurRowNo, CurColNo);
                            if (model != null)
                            {
                                DieModelHelper.Insert(model);
                                ProcInfoHelper.Instance.PlaceDieToPicker(Stn16TurnTableStnEnum.PickDie, model, RotateStnType.LoadDie);
                                Axes = new List<AxisName>() { AxisName.WaferTable_ThimbleCap, AxisName.WaferTable_Thimble };
                                PosNames = new List<PosName>() { PosName.WaitPos };
                                MoveAbsM(Axes, PosNames);
                                step = 350;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->从Wafer第{CurRowNo}行第{CurColNo}列取DieModel信息失败", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.GetDieModelFromWaferFail.ToInt(), StationNum);
                            }
                        }
                        break;
                    }
                case 350:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ResetBoolReg(RegisterEnum.PickDieStnPickDone);
                            ShowMsg($"{StationName}->顶针下降完成，取下一颗Die");
                            CurDieNo++;
                            step = 100;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->顶针下降超时", MsgType.Error);
                            step = 300;
                        }
                        break;
                    }
                case 400:
                    {
                        var ret = IsCylinderArrive(CylinderName.WaferTableThimbleCyl, false);
                        if (ret == MotionStatus.Arrived)//如果是最后一个Bin等级，要等到4工位转盘下完Tray后才能结束Wafer对位流程
                        {
                            ShowMsg($"{StationName}->顶针气缸下降到位，取Die流程完成", MsgType.Notify);
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->顶针气缸下降超时", MsgType.Error);
                        }
                        break;
                    }
                default:
                    break;
            }
            return step;
        }

        public int WafterTableToLulPos(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->Wafer平台移动到取放Wafer位置开始", MsgType.Notify);
                        AxisZMoveStep = 1;
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        AxisZMoveStep = WaferTableZMove(AxisZMoveStep, PosName.LoadWaferPos);//和上Wafer位置相同
                        if (AxisZMoveStep == 0)
                        {
                            ShowMsg($"{StationName}->XYT轴移动到下Wafer位置");
                            Axes = new List<AxisName>() { AxisName.WaferTable_X, AxisName.WaferTable_Y, AxisName.WaferTable_T };
                            PosNames = new List<PosName>() { PosName.LoadWaferPos };//和上Wafer位置相同
                            MoveAbsM(Axes, PosNames);
                            Cylinders = new List<CylinderName>() { CylinderName.WaferTableArmCyl, CylinderName.WaferTableClawCyl };
                            CylinderM(Cylinders, false);
                            step = 100;
                        }
                        else if (AxisZMoveStep == -1)
                        {
                            ShowMsg($"{StationName}->Wafer平台Z轴移动失败", MsgType.Error, true);
                            step = -1;
                        }
                        break;
                    }
                case 100:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        var ret2 = IsCylinderArriveM(Cylinders, false);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->Wafer平台到达取放Wafer位置");
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
        public int WaferTableZMove(int step, PosName posName)
        {
            switch (step)
            {
                case 1:
                    {
                        if (Motion.GetDigitalInput(InputName.WaferTableClawIsExistWafer))
                        {
                            ShowMsg($"{StationName}->Wafer平台夹爪检测到Wafer，禁止移动Wafer平台", MsgType.Error, true);
                            step = -1;
                        }
                        else if (!Motion.IsCylinderArrive(CylinderName.WaferTableArmCyl, false))
                        {
                            ShowMsg($"{StationName}->Wafer平台夹爪升降气缸不在缩回位置，禁止移动Wafer平台", MsgType.Error, true);
                            step = -1;
                        }
                        else
                        {
                            ShowMsg($"{StationName}->动力气缸缩回");
                            Cylinders = new List<CylinderName>() { CylinderName.WaferTableZPowerCyl, CylinderName.WaferTableZLimitCyl };
                            CylinderM(Cylinders, false);
                            step = 50;
                        }
                        break;
                    }
                case 50:
                    {
                        var ret = IsCylinderArriveM(Cylinders, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->动力气缸缩回完成，WaferTableT轴移动到动力气缸伸出位置");
                            MoveAbs(AxisName.WaferTable_T, PosName.WaferTableZCylOutPos);
                            step = 100;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->动力气缸缩回超时", MsgType.Error);
                            step = 1;
                        }
                        break;
                    }
                case 100:
                    {
                        var ret = IsAxisArrive(AxisName.WaferTable_T, PosName.WaferTableZCylOutPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->WaferTableT轴移动到动力气缸伸出位置完成，动力气缸伸出");
                            Cylinder(CylinderName.WaferTableZPowerCyl, true);
                            step = 150;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->WaferTableT轴移动到动力气缸伸出位置超时", MsgType.Error);
                            step = 50;
                        }
                        break;
                    }
                case 150:
                    {
                        var ret = IsCylinderArrive(CylinderName.WaferTableZPowerCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->动力气缸伸出完成，WaferTableZ轴移动到{posName}位置");
                            MoveAbs(AxisName.WaferTable_Z, posName);
                            step = 200;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->动力气缸伸出超时", MsgType.Error);
                            step = 100;
                        }
                        break;
                    }
                case 200:
                    {
                        var ret = IsAxisArrive(AxisName.WaferTable_Z, posName);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->WaferTableZ轴移动到{posName}位置完成");
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->WaferTableZ轴移动到{posName}位置超时", MsgType.Error);
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
        /// 计算第rowNo行第colNo列颗Die的拍照位置，绝对坐标
        /// </summary>
        /// <param name="rowNo"></param>
        /// <param name="colNo"></param>
        /// <returns></returns>
        private Xyt GetDieGrabPos(int rowNo, int colNo)
        {
            Xyt ret = null;
            var waferModel = ProcInfoHelper.Instance.GetWaferModel(WaferPosEnum.WaferTable);
            if (waferModel != null)
            {
                var dieXPos = (colNo - ParaHelper.Prod.BaseDieColNo) * (waferModel.XDIES / 1000.0);
                var dieYPos = (rowNo - ParaHelper.Prod.BaseDieRowNo) * (waferModel.YDIES / 1000.0);
                //var dieXPos = (rowNo - ParaHelper.Prod.BaseDieRowNo) * (waferModel.YDIES / 1000.0);
                //var dieYPos = (colNo - ParaHelper.Prod.BaseDieColNo) * (waferModel.XDIES / 1000.0);
                ret = new Xyt() { X = BaseDieAbsPos.X + dieXPos, Y = BaseDieAbsPos.Y + dieYPos };
            }
            return ret;
        }
        private void AlignFailCounter(string msg, AlarmCode code)
        {
            AlignCounter++;
            ShowMsg($"{StationName}->{msg}", MsgType.Error);
            if (AlignCounter >= ParaHelper.Vision.RetrayCount)
            {
                ShowMsg($"{StationName}->{msg}超过{ParaHelper.Vision.RetrayCount}次", MsgType.Error);
                AlarmManager.CreateAlarm(code.ToInt(), StationNum);
            }
            else
            {
                Thread.Sleep(100);
            }
        }
        /// <summary>
        /// 计算Wafer扩摸完成后四个顶点的实际坐标
        /// </summary>
        /// <param name="cornerNo"></param>
        /// <param name="picCrd"></param>
        /// <returns></returns>
        private Xyt CalcRealCrd(int cornerNo, double[] picCrd)
        {
            Xyt ret = null;
            var xPos = Motion.GetAxisActPos(AxisName.WaferTable_X);
            var yPos = Motion.GetAxisActPos(AxisName.WaferTable_Y);
            ret = new Xyt() { X = picCrd[0] + xPos, Y = picCrd[1] + yPos };
            return ret;
        }
        /// <summary>
        /// 计算扩摸后每颗Die的偏移量
        /// </summary>
        /// <returns></returns>
        private Xyt CalcDieOffset()
        {
            Xyt ret = null;
            if (FourCornerRealCrd != null && FourCornerRealCrd.Count == 4)
            {
                var realWidth = FourCornerRealCrd[1][0] - FourCornerRealCrd[0][0];//实际宽度
                var realHeight = FourCornerRealCrd[2][1] - FourCornerRealCrd[0][1];//实际高度
                var waferModel = ProcInfoHelper.Instance.GetWaferModel(WaferPosEnum.WaferTable);
                if (waferModel != null)
                {
                    var mapWidth = waferModel.COLCT * waferModel.XDIES / 1000.0;
                    var mapHeight = waferModel.ROWCT * waferModel.YDIES / 1000.0;
                    ret = new Xyt();
                    var diffWidth = realWidth - mapWidth;
                    var diffHeight = realHeight - mapHeight;
                    if (diffWidth >= 0)
                    {
                        ret.X = Math.Round(diffWidth / waferModel.COLCT, 3);
                    }
                    if (diffHeight >= 0)
                    {
                        ret.Y = Math.Round(diffHeight / waferModel.ROWCT, 3);
                    }
                }
            }
            return ret;
        }
        public bool IsExistTrayInStn4()
        {
            for (int i = 0; i < 4; i++)
            {
                var stageNo = RotateInfoHelper.Instance.GetStageNo(i, RotateStnType.Stn4TurnTable);
                var stage = StageManager.Get(stageNo);
                var ret = Motion.GetDigitalInput(stage.IsExistTray);
                if (ret)
                {
                    return true;
                }
            }
            return false;
        }
        //□/////////////////////□//
        ////////////////////////////
        ////////////////////////////
        ////////////////////////////
        //△1///////////////////△2//
        ////////////////////////////
        ////////////////////////////
        ////////////////////////////
        //□/////////////////////□//

        /// <summary>
        /// 计算四个角的Die拍照位置，绝对坐标
        /// </summary>
        /// <returns></returns>
        private bool GetFourCornerDieGrabPos()
        {
            bool ret = false;
            try
            {
                FourCornerDieGrabPos = new List<double[]>();
                var waferModel = ProcInfoHelper.Instance.GetWaferModel(WaferPosEnum.WaferTable);
                if (waferModel != null)
                {
                    for (int i = 0; i < 4; i++)
                    {
                        double posX = 0;
                        double posY = 0;
                        if (i == 0)
                        {
                            posX = BaseDieAbsPos.X;
                            posY = BaseDieAbsPos.Y;
                        }
                        else if (i == 1)
                        {
                            posX = BaseDieAbsPos.X + waferModel.XDIES * (waferModel.COLCT - 1) / 1000.0;
                            posY = BaseDieAbsPos.Y;
                        }
                        else if (i == 2)
                        {
                            posX = BaseDieAbsPos.X + waferModel.XDIES * (waferModel.COLCT - 1) / 1000.0;
                            posY = BaseDieAbsPos.Y + waferModel.YDIES * (waferModel.ROWCT - 1) / 1000.0;
                        }
                        else if (i == 3)
                        {
                            posX = BaseDieAbsPos.X;
                            posY = BaseDieAbsPos.Y + waferModel.YDIES * (waferModel.ROWCT - 1) / 1000.0;
                        }
                        FourCornerDieGrabPos.Add(new double[] { posX, posY });
                    }
                    ret = true;
                }
            }
            catch (Exception ex)
            {

            }
            return ret;
        }
        public void Init()
        {
            // RegisterService.Instance.SetBoolReg(RegisterEnum.WaferEmpty);
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
            //ModuleMoveStep = -1;
            //LoadWaferStep = -1;
            //UnloadWaferStep = -1;
            //ScancodeStep = -1;
            //AutoPosStep = -1;
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
