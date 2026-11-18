using JC.PTIS.LoadPort.Devices.EFEM;
using JC.PTIS.Main.AlarmInfo;
using JC.PTIS.Main.Devices.EFEM;
using JC.PTIS.Main.Devices.Scaner;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Params;
using JC.PTIS.Main.Service;
using StdLib.PCBaseFrame.Alarm;
using StdLib.PCBaseFrame.Args;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.PCBaseFrame.Motion;
using StdLib.PCBaseFrame.MotionInfo;
using StdLib.PCBaseFrame.Param;
using StdLib.PCBaseFrame.Station;
using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Stations
{
    public class WaferBufferStn : StationBaseEx, IStation
    {
        public static event Func<string> OnScancodeFailConfirm;
        public int StationNum { get; set; } = 0;
        public string StationName { get; set; } = "WaferBufferStn";
        int SubFlowStep = -1;
        int PutWaferToBufferStep = -1;
        int LulFoupStep = -1;
        // int LoopCounter = 0;
        PosName ClawZPos = PosName.None;
        List<AxisName> Axes = new List<AxisName>();
        List<CylinderName> Cylinders = new List<CylinderName>();
        List<PosName> Poses = new List<PosName>();
        public double WaferBuffer_ZPos = 0;//Wafer缓存模组Z轴目标位置
        public Foup UseFoupNo = Foup.Left;//在用的Foup编号
        public int UseLayerNo = 0;//在用的Foup层号
        string Barcode = string.Empty;//Wafer条码
        public bool IsManualMode { get; set; } = false;//是否手动调试模式
        bool[]LulFoupSts=new bool[2] { false, false };//LulFoup流程是否完成
        public void Home()
        {
            switch (HomeStep)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->开始回零，等待WaferTableWaferBufferX轴回零完成");
                        HomeStep = 20;
                        break;
                    }
                case 20:
                    {
                        if (Motion.IsArrive(AxisName.WaferTable_ArmX, PosName.WaitPos))
                        {
                            ShowMsg($"{StationName}->WaferTableWaferBufferX轴回零完成，判断WaferBuffer夹爪上有无Wafer");
                            if (Motion.GetDigitalInput(InputName.WaferBufferClawIsExistWafer))
                            {
                                ShowMsg($"{StationName}->WaferBuffer夹爪检测到Wafer，夹爪气缸夹紧");
                                Cylinder(CylinderName.WaferBufferClawCyl, true);
                                HomeStep = 50;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->WaferBuffer夹爪未检测到Wafer，夹爪气缸松开");
                                Cylinder(CylinderName.WaferBufferClawCyl, false);
                                Motion.Home(AxisName.WaferBuffer_X);
                                HomeStep = 100;
                            }
                        }
                        break;
                    }
                case 50:
                    {
                        var ret = IsCylinderArrive(CylinderName.WaferBufferClawCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->WaferBuffer夹爪气缸夹紧到位，{AxisName.WaferBuffer_X}轴开始回零");
                            Motion.Home(AxisName.WaferBuffer_X);
                            HomeStep = 100;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 100:
                    {
                        if (Motion.IsHomeDone(AxisName.WaferBuffer_X))
                        {
                            ShowMsg($"{StationName}->{AxisName.WaferBuffer_X}轴回零完成");
                            if (Motion.GetDigitalInput(InputName.WaferBufferClawIsExistWafer))//如果夹爪上检测到Wafer，WaferBufferX轴要先移动到待机位置
                            {
                                ShowMsg($"{StationName}->{AxisName.WaferBuffer_X}轴移动到待机位");
                                MoveAbs(AxisName.WaferBuffer_X, PosName.WaitPos);
                                HomeStep = 150;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->{AxisName.WaferBuffer_ClawZ}轴回零");
                                Motion.Home(AxisName.WaferBuffer_ClawZ);
                                HomeStep = 600;
                            }
                        }
                        else if (Motion.IsHomeDone(AxisName.WaferBuffer_X))
                        {
                            ShowMsg($"{StationName}->{AxisName.WaferBuffer_X}轴回零超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 150:
                    {
                        var ret = IsAxisArrive(AxisName.WaferBuffer_X, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->{AxisName.WaferBuffer_X}轴到达待机位，夹爪气缸开始松开");
                            Cylinder(CylinderName.WaferBufferClawCyl, false);
                            HomeStep = 180;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 180:
                    {
                        var ret = IsCylinderArrive(CylinderName.WaferBufferClawCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪气缸开始松开完成，{AxisName.WaferBuffer_X}轴后退10mm");
                            MoveRel(AxisName.WaferBuffer_X, -10);//WaferBufferX到达释放位置后回退10mm
                            HomeStep = 200;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 200:
                    {
                        var ret = IsAxisArrive(AxisName.WaferBuffer_X);
                        if (ret == MotionStatus.Arrived)
                        {
                            Motion.Home(AxisName.WaferBuffer_ClawZ);//夹爪脱离开Wafer后，夹爪Z轴回零
                            ShowMsg($"{StationName}->{AxisName.WaferBuffer_X}轴后退到位，{AxisName.WaferBuffer_ClawZ}轴回零");
                            HomeStep = 250;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 250:
                    {
                        if (Motion.IsHomeDone(AxisName.WaferBuffer_ClawZ))
                        {
                            ShowMsg($"{StationName}->{AxisName.WaferBuffer_ClawZ}轴回零完成");
                            MoveAbs(AxisName.WaferBuffer_ClawZ, PosName.WaitPos);
                            ClawZPos = PosName.None;
                            HomeStep = 300;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.WaferBuffer_ClawZ))
                        {
                            ShowMsg($"{StationName}->{AxisName.WaferBuffer_ClawZ}轴回零超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 300:
                    {
                        var ret = IsAxisArrive(AxisName.WaferBuffer_ClawZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            //LoopCounter++;
                            //if (LoopCounter > 2)//判断吸盘吸Wafer后退动作是否执行2次
                            //{
                            //    ShowMsg($"{StationName}->Wafer放回平台完成，轴：{AxisName.WaferBuffer_ClawZ}回零");
                            //    Motion.Home(AxisName.WaferBuffer_ClawZ);
                            //    HomeStep = 600;
                            //}
                            //else
                            //{
                            //    MoveAbs(AxisName.WaferBuffer_ClawX, PosName.WaitPos);
                            //    HomeStep = 320;
                            //}
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 320:
                    {
                        var ret = IsAxisArrive(AxisName.WaferBuffer_X, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            Cylinder(CylinderName.WaferBufferStretchCyl, true);//吸盘气缸伸出
                            HomeStep = 350;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 350:
                    {
                        var ret = IsCylinderArrive(CylinderName.WaferBufferStretchCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            if (ClawZPos == PosName.None)
                            {
                                if (Motion.GetDigitalInput(InputName.UpLayerWaferDetect2))
                                {
                                    ClawZPos = PosName.UpLayerSuckWaferPos;
                                }
                                else
                                {
                                    ClawZPos = PosName.DownLayerSuckWaferPos;
                                }
                            }
                            MoveAbs(AxisName.WaferBuffer_ClawZ, ClawZPos);//吸盘伸出后，夹爪Z轴上升到吸Wafer位置
                            HomeStep = 400;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 400:
                    {
                        var ret = IsAxisArrive(AxisName.WaferBuffer_ClawZ, ClawZPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            Vacuum(VacuumName.WaferBufferClawVac, true);//吸盘吸真空
                            HomeStep = 450;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 450:
                    {
                        var ret = IsVacuumArrive(VacuumName.WaferBufferClawVac, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            Cylinder(CylinderName.WaferBufferStretchCyl, false);//吸盘气缸缩回
                            HomeStep = 500;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->{VacuumName.WaferBufferClawVac}破真空超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 500:
                    {
                        var ret = IsCylinderArrive(CylinderName.WaferBufferStretchCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            Vacuum(VacuumName.WaferBufferClawVac, false);//吸盘破真空
                            HomeStep = 550;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->{CylinderName.WaferBufferStretchCyl}气缸缩回超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 550:
                    {
                        var ret = IsVacuumArrive(VacuumName.WaferBufferClawVac, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->{AxisName.WaferBuffer_ClawZ}轴移动到待机位");
                            MoveAbs(AxisName.WaferBuffer_ClawZ, PosName.WaitPos);//吸盘破真空后，夹爪Z轴下降到待机位置
                            HomeStep = 300;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->{VacuumName.WaferBufferClawVac}破真空超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 600:
                    {
                        if (Motion.IsHomeDone(AxisName.WaferBuffer_ClawZ))
                        {
                            ShowMsg($"{StationName}->{AxisName.WaferBuffer_ClawZ}轴回零完成");
                            Motion.Home(AxisName.WaferBuffer_Y);
                            Motion.Home(AxisName.WaferBuffer_Z);
                            HomeStep = 650;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.WaferBuffer_ClawZ))
                        {
                            ShowMsg($"{StationName}->{AxisName.WaferBuffer_ClawZ}轴回零超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 650:
                    {
                        if (Motion.IsHomeDone(AxisName.WaferBuffer_Y) &&
                            Motion.IsHomeDone(AxisName.WaferBuffer_Z))
                        {
                            ShowMsg($"{StationName}->{AxisName.WaferBuffer_Y}轴和{AxisName.WaferBuffer_Z}轴回零完成");
                            Motion.MoveAbs(AxisName.WaferBuffer_Y, PosName.WaitPos);
                            Motion.MoveAbs(AxisName.WaferBuffer_Z, PosName.WaitPos);
                            ShowMsg($"{StationName}->{AxisName.WaferBuffer_Y}轴和{AxisName.WaferBuffer_Z}轴移动到{PosName.WaitPos}位");
                            HomeStep = 700;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.WaferBuffer_Y) ||
                            Motion.IsMoveTimeOut(AxisName.WaferBuffer_Z))
                        {
                            ShowMsg($"{StationName}->Wafer缓存模组回零超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 700:
                    {
                        if (Motion.IsArrive(AxisName.WaferBuffer_Y, PosName.WaitPos) &&
                            Motion.IsArrive(AxisName.WaferBuffer_Z, PosName.WaitPos))
                        {
                            ShowMsg($"{StationName}->{AxisName.WaferBuffer_Y}轴和{AxisName.WaferBuffer_Z}轴到达{PosName.WaitPos}位");
                            ShowMsg($"{StationName}->Wafer缓存模组回零完成");
                            UpdateHomeSts(StationName, StationNum = StationNum, true);
                            HomeStep = 0;


                            if (Global.RunMode == RunMode.空跑模式)
                            {
                                IOInfoHelper.GetInputInfoByName(InputName.UpLayerWaferDetect1.ToString(), out int _).IsShield = false;
                                IOInfoHelper.GetInputInfoByName(InputName.DownLayerWaferDetect1.ToString(), out int _).IsShield = false;
                                Thread.Sleep(500);
                            }
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.WaferBuffer_Y))
                        {
                            ShowMsg($"{StationName}->{AxisName.WaferBuffer_Y}轴移动到{PosName.WaitPos}位超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.WaferBuffer_Z))
                        {
                            ShowMsg($"{StationName}->{AxisName.WaferBuffer_Z}轴移动到{PosName.WaitPos}位超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 1000:
                    {
                        UpdateHomeSts(StationName, StationNum = StationNum, false);
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
                            ShowMsg($"{StationName}->设备启动，执行回待机位流程");
                            SubFlowStep = 1;
                            AutoRunStep = 50;
                            break;
                        }
                    case 50:
                        {
                            SubFlowStep = MoveToWaitPosFlow(SubFlowStep);
                            if (SubFlowStep == 0)
                            {
                                IsManualMode = false;
                                ShowMsg($"{StationName}->各轴回待机位完成，判断缓存平台下层有无空Wafer");
                                AutoRunStep = 100;
                            }
                            break;
                        }
                    case 100:
                        {
                            if (Motion.GetDigitalInput(InputName.DownLayerWaferDetect1))
                            {
                                ShowMsg($"{StationName}->缓存平台下层有空Wafer，执行Wafer进Foup流程");
                                SubFlowStep = 1;
                                AutoRunStep = 150;
                            }
                            else if (Motion.GetDigitalInput(InputName.UpLayerWaferDetect1))
                            {
                                RegisterService.Instance.SetBoolReg(RegisterEnum.WaferBufferExistWafer);
                                ShowMsg($"{StationName}->缓存平台上层有满Wafer，等待WaferTable工位请求");
                                AutoRunStep = 200;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->缓存平台上层没有满Wafer，执行从Foup取Wafer流程");
                                SubFlowStep = 1;
                                AutoRunStep = 250;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.WaferBufferClawIsExistWafer.ToString(), out int _).IsShield = true;
                                }
                            }
                            break;
                        }
                    case 150:
                        {
                            SubFlowStep = PutWaferToFoupFlow(SubFlowStep);
                            if (SubFlowStep == 0)
                            {
                                ShowMsg($"{StationName}->Wafer下料流程结束，判断Wafer缓存平台上是否有Wafer");
                                AutoRunStep = 100;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.DownLayerWaferDetect1.ToString(), out int _).IsShield = false;
                                    Thread.Sleep(500);
                                }
                            }
                            break;
                        }
                    case 200:
                        {
                            if (GetBoolReg(RegisterEnum.WaferTableLoadWaferReq))
                            {
                                ResetBoolReg(RegisterEnum.WaferTableLoadWaferReq);
                                ShowMsg($"{StationName}->收到WaferTable工位上满Wafer请求");
                                SubFlowStep = 1;
                                AutoRunStep = 300;
                            }
                            else if (GetBoolReg(RegisterEnum.WaferTableUnloadWaferReq))
                            {
                                ResetBoolReg(RegisterEnum.WaferTableUnloadWaferReq);
                                ShowMsg($"{StationName}->收到WaferTable工位下空Wafer请求");
                                SubFlowStep = 1;
                                AutoRunStep = 350;
                            }
                            break;
                        }
                    case 250:
                        {
                            SubFlowStep = PickWaferFromFoupFlow(SubFlowStep);
                            if (SubFlowStep == 0)
                            {
                                ShowMsg($"{StationName}->从Foup取Wafer流程结束");
                                AutoRunStep = 100;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.WaferBufferClawIsExistWafer.ToString(), out int _).IsShield = false;
                                    IOInfoHelper.GetInputInfoByName(InputName.UpLayerWaferDetect1.ToString(), out int _).IsShield = true;
                                    Thread.Sleep(500);
                                }
                            }
                            break;
                        }
                    case 300:
                        {
                            SubFlowStep = WaferTablePickWaferFlow(SubFlowStep);
                            if (SubFlowStep == 0)
                            {
                                ShowMsg($"{StationName}->WaferTable工位取Wafer流程结束");
                                AutoRunStep = 100;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.UpLayerWaferDetect1.ToString(), out int _).IsShield = false;
                                    Thread.Sleep(500);
                                }
                            }
                            break;
                        }
                    case 350:
                        {
                            SubFlowStep = WaferTablePlaceWaferFlow(SubFlowStep);
                            if (SubFlowStep == 0)
                            {
                                ShowMsg($"{StationName}->WaferTable工位放Wafer流程结束");
                                AutoRunStep = 100;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.DownLayerWaferDetect1.ToString(), out int _).IsShield = true;
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
        /// <summary>
        /// 把Wafer放到Foup中
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int PutWaferToFoupFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->往Foup中放Wafer流程开始", MsgType.Notify);
                        ShowMsg($"{StationName}->夹爪Z轴移动到待机位，夹爪气缸松开，伸缩轴伸出");
                        MoveAbs(AxisName.WaferBuffer_ClawZ, PosName.WaitPos);//夹爪Z轴移动到夹紧Wafer位置
                        Cylinder(CylinderName.WaferBufferClawCyl, false);//夹爪气缸松开
                        Cylinder(CylinderName.WaferBufferStretchCyl, true);//夹爪气缸缩回
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArrive(AxisName.WaferBuffer_ClawZ, PosName.WaitPos);
                        var ret2 = IsCylinderArrive(CylinderName.WaferBufferClawCyl, false);
                        var ret3 = IsCylinderArrive(CylinderName.WaferBufferStretchCyl, true);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived && ret3 == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->WaferBuffer_X轴移动到ClawXReleaseWaferPos3位置");
                            MoveAbs(AxisName.WaferBuffer_X, PosName.ClawXReleaseWaferPos3);
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
                        var ret = IsAxisArrive(AxisName.WaferBuffer_X, PosName.ClawXReleaseWaferPos3);
                        if (ret == MotionStatus.Arrived)
                        {
                            var layerNoLeft = ProcInfoHelper.Instance.GetAvailableEmptySlotNo(Foup.Left.ToInt());
                            var layerNoRight = ProcInfoHelper.Instance.GetAvailableEmptySlotNo(Foup.Right.ToInt());
                            ShowMsg($"获取左右Foup空层层号，左侧Foup：{layerNoLeft}，右侧Foup：{layerNoRight}");
                            if (layerNoLeft >= layerNoRight)
                            {
                                UseFoupNo = Foup.Left;
                                UseLayerNo = layerNoLeft;
                                var posInfo = AxisInfoHelper.GetPosInfoByName(AxisName.WaferBuffer_Z, PosName.Foup1PickPlacePos);
                                WaferBuffer_ZPos = posInfo.Pos + ParaHelper.Prod.FoupPitch * layerNoLeft;
                                ShowMsg($"往左侧Foup第{layerNoLeft}层中放Wafer，WaferBuffer_Z轴目标位置：{WaferBuffer_ZPos}");
                            }
                            else
                            {
                                UseFoupNo = Foup.Right;
                                UseLayerNo = layerNoRight;
                                var posInfo = AxisInfoHelper.GetPosInfoByName(AxisName.WaferBuffer_Z, PosName.Foup2PickPlacePos);
                                WaferBuffer_ZPos = posInfo.Pos + ParaHelper.Prod.FoupPitch * layerNoRight;
                                ShowMsg($"往右侧Foup第{layerNoRight}层中放Wafer，WaferBuffer_Z轴目标位置：{WaferBuffer_ZPos}");
                            }
                            MoveAbs(AxisName.WaferBuffer_Z, WaferBuffer_ZPos);
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
                        var ret = IsAxisArrive(AxisName.WaferBuffer_Z, WaferBuffer_ZPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->Wafer缓存模组移动到待机位置");
                            Axes = new List<AxisName>() { AxisName.WaferBuffer_Y };
                            if (UseFoupNo == Foup.Left)
                            {
                                Poses = new List<PosName>() { PosName.Foup1PickPlacePos };
                            }
                            else
                            {
                                Poses = new List<PosName>() { PosName.Foup2PickPlacePos };
                            }
                            MoveAbsM(Axes, Poses);
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
                        var ret = IsAxisArriveM(Axes, Poses);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪Z轴移动到吸Wafer位置");
                            MoveAbs(AxisName.WaferBuffer_ClawZ, PosName.DownLayerSuckWaferPos);//夹爪Z轴移动到吸Wafer位置
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
                        var ret = IsAxisArrive(AxisName.WaferBuffer_ClawZ, PosName.DownLayerSuckWaferPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->吸盘吸真空");
                            Vacuum(VacuumName.WaferBufferClawVac, true);//吸盘吸真空
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
                        var ret = IsVacuumArrive(VacuumName.WaferBufferClawVac, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->WaferBufferX轴移动到ClawXSuckWaferPos2位置");
                            MoveAbs(AxisName.WaferBuffer_X, PosName.ClawXSuckWaferPos2);
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
                        var ret = IsAxisArrive(AxisName.WaferBuffer_X, PosName.ClawXSuckWaferPos2);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->吸盘破真空");
                            Vacuum(VacuumName.WaferBufferClawVac, false);//吸盘破真空
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
                        var ret = IsVacuumArrive(VacuumName.WaferBufferClawVac, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪Z轴下降到待机位置");
                            MoveAbs(AxisName.WaferBuffer_ClawZ, PosName.WaitPos);//夹爪Z轴下降到待机位置
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
                        var ret = IsAxisArrive(AxisName.WaferBuffer_ClawZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪气缸缩回");
                            Cylinder(CylinderName.WaferBufferStretchCyl, false);//夹爪气缸缩回
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
                        var ret = IsCylinderArrive(CylinderName.WaferBufferStretchCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->WaferBufferX轴移动到ClawXReleaseWaferPos2位置");
                            MoveAbs(AxisName.WaferBuffer_X, PosName.ClawXReleaseWaferPos2);
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
                        var ret = IsAxisArrive(AxisName.WaferBuffer_X, PosName.ClawXReleaseWaferPos2);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪Z轴移动到吸Wafer位置");
                            MoveAbs(AxisName.WaferBuffer_ClawZ, PosName.DownLayerSuckWaferPos);//夹爪Z轴移动到夹紧Wafer位置
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
                        var ret = IsAxisArrive(AxisName.WaferBuffer_ClawZ, PosName.DownLayerSuckWaferPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->吸盘吸真空");
                            Vacuum(VacuumName.WaferBufferClawVac, true);//吸盘吸真空
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
                        var ret = IsVacuumArrive(VacuumName.WaferBufferClawVac, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->WaferBufferX轴移动到ClawXSuckWaferPos1位置");
                            MoveAbs(AxisName.WaferBuffer_X, PosName.ClawXSuckWaferPos1);
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
                        var ret = IsAxisArrive(AxisName.WaferBuffer_X, PosName.ClawXSuckWaferPos1);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->吸盘破真空");
                            Vacuum(VacuumName.WaferBufferClawVac, false);//吸盘破真空
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
                        var ret = IsVacuumArrive(VacuumName.WaferBufferClawVac, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪Z轴移动到待机位置");
                            MoveAbs(AxisName.WaferBuffer_ClawZ, PosName.WaitPos);//夹爪Z轴移动到待机位置
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
                        var ret = IsAxisArrive(AxisName.WaferBuffer_ClawZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->WaferBufferX轴移动到ClawXReleaseWaferPos1位置");
                            MoveAbs(AxisName.WaferBuffer_X, PosName.ClawXReleaseWaferPos1);
                            step = 850;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 750;
                        }
                        break;
                    }
                case 810:
                    {
                        var ret = IsAxisArrive(AxisName.WaferBuffer_X, PosName.ClawXReleaseWaferPos1);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->WaferBufferX轴后退10毫米");
                            MoveRel(AxisName.WaferBuffer_X, -10);//WaferBufferX轴后退10毫米
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
                        var ret = IsAxisArrive(AxisName.WaferBuffer_X);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪Z轴上升到夹Wafer位置");
                            MoveAbs(AxisName.WaferBuffer_ClawZ, PosName.DownLayerClawWaferPos);
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
                        var ret = IsAxisArrive(AxisName.WaferBuffer_ClawZ, PosName.DownLayerClawWaferPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->WaferBufferX轴移动到ClawXReleaseWaferPos1位置");
                            MoveAbs(AxisName.WaferBuffer_X, PosName.ClawXReleaseWaferPos1);//WaferBufferX轴移动到ClawXReleaseWaferPos1位置
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
                        var ret = IsAxisArrive(AxisName.WaferBuffer_X, PosName.ClawXReleaseWaferPos1);
                        if (ret == MotionStatus.Arrived)
                        {
                            Cylinder(CylinderName.WaferBufferClawCyl, true);
                            ShowMsg($"{StationName}->气缸夹紧Wafer");
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
                        var ret = IsCylinderArrive(CylinderName.WaferBufferClawCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->WaferBufferX轴移动到ClampWaferFromFoup位置");
                            MoveAbs(AxisName.WaferBuffer_X, PosName.ClampWaferFromFoup);//WaferBufferX轴移动到夹紧Wafer位置
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
                        var ret = IsAxisArrive(AxisName.WaferBuffer_X, PosName.ClampWaferFromFoup);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->BufferZ轴下降5mm");
                            MoveRel(AxisName.WaferBuffer_Z, -5);//BufferZ轴下降5mm，让Wafer贴近Foup
                            step = 1100;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1000;
                        }
                        break;
                    }
                case 1100:
                    {
                        var ret = IsAxisArrive(AxisName.WaferBuffer_Z);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪气缸松开");
                            Cylinder(CylinderName.WaferBufferClawCyl, false);//夹爪气缸松开
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
                        var ret = IsCylinderArrive(CylinderName.WaferBufferClawCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->BufferZ轴上升5mm，脱离Wafer");
                            MoveRel(AxisName.WaferBuffer_Z, 5);//BufferZ轴上升5mm，脱离Wafer
                            step = 1200;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1100;
                        }
                        break;
                    }
                case 1200:
                    {
                        var ret = IsAxisArrive(AxisName.WaferBuffer_Z);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->WaferBufferX轴移动到待机位置");
                            MoveAbs(AxisName.WaferBuffer_X, PosName.WaitPos);//夹爪Z轴下降到待机位置
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
                        var ret = IsAxisArrive(AxisName.WaferBuffer_X, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->往Foup中放Wafer流程完成，Wafer模组移动到待机位");
                            ProcInfoHelper.Instance.PlaceWaferToFoup(UseFoupNo.ToInt(), UseLayerNo);
                            Axes = new List<AxisName>() { AxisName.WaferBuffer_Y, AxisName.WaferBuffer_Z };
                            Poses = new List<PosName>() { PosName.WaitPos };
                            MoveAbsM(Axes, Poses);
                            step = 1300;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1200;
                        }
                        break;
                    }
                case 1300:
                    {
                        var ret = IsAxisArriveM(Axes, Poses);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->Wafer模组到达待机位，判断Foup中是否还有满Wafer");
                            var layerNoLeft = ProcInfoHelper.Instance.GetAvailableFullWaferSlotNo(Foup.Left.ToInt());
                            var layerNoRight = ProcInfoHelper.Instance.GetAvailableFullWaferSlotNo(Foup.Right.ToInt());
                            if (layerNoLeft==-1&& layerNoRight == -1)
                            {
                                ShowMsg($"{StationName}->两侧Foup中没有满Wafer，执行下Foup流程");
                                LulFoupStep = 1;
                                step = 1350;
                            }
                            else
                            {
                                step = 0;
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1250;
                        }
                        break;
                    }
                case 1350:
                    {
                        LulFoupStep = UnloadFoupFlow(LulFoupStep);
                        if (LulFoupStep == 0)
                        {
                            ShowMsg($"{StationName}->下Foup流程完成");
                            step = 0;
                        }
                        break;
                    }
            }
            return step;
        }
        /// <summary>
        /// 从Foup中取Wafer
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int PickWaferFromFoupFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->从Foup中取Wafer流程开始", MsgType.Notify);
                        var codeLeft=LoadPortHelper.Instance.GetLoadPortDevice(Foup.Left.ToInt()).HasFoup(out bool isExistLeft);
                        var codeRight = LoadPortHelper.Instance.GetLoadPortDevice(Foup.Right.ToInt()).HasFoup(out bool isExistRight);
                        if (codeLeft==LPCodes.SUCCESS&& codeRight == LPCodes.SUCCESS)
                        {
                            //没有Foup，请求上Foup
                            if (!isExistLeft&& !isExistRight)
                            {
                                ShowMsg($"{StationName}->两侧LoadPort中都没有Foup，执行上Foup流程");
                                LulFoupStep = 1;
                                LulFoupSts[Foup.Left.ToInt()] = false;
                                step = 20;
                            }
                            else
                            {
                                MoveAbs(AxisName.WaferBuffer_ClawZ, PosName.UpLayerClawWaferPos);//夹爪Z轴移动到夹紧Wafer位置
                                Cylinders = new List<CylinderName>() { CylinderName.WaferBufferClawCyl, CylinderName.WaferBufferStretchCyl };
                                CylinderM(Cylinders, false);//夹爪气缸缩回
                                step = 50;
                            }
                        }
                        else
                        {
                            ShowMsg($"{StationName}->获取LoadPort有无Foup失败", MsgType.Error);
                            AlarmManager.CreateAlarm(AlarmCode.GetLoadPortStatusFail.ToInt(),StationNum);
                        }
                        break;
                    }
                case 20:
                    {
                        LulFoupStep = LoadFoupFlow(LulFoupStep);
                        if (LulFoupStep == 0)
                        {
                            ShowMsg($"{StationName}->上Foup流程完成，从Foup中取Wafer");
                            MoveAbs(AxisName.WaferBuffer_ClawZ, PosName.UpLayerClawWaferPos);//夹爪Z轴移动到夹紧Wafer位置
                            Cylinders = new List<CylinderName>() { CylinderName.WaferBufferClawCyl, CylinderName.WaferBufferStretchCyl };
                            CylinderM(Cylinders, false);//夹爪气缸缩回
                            step = 50;
                        }
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArrive(AxisName.WaferBuffer_ClawZ, PosName.UpLayerClawWaferPos);
                        var ret2 = IsCylinderArriveM(Cylinders, false);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                        {
                            MoveAbs(AxisName.WaferBuffer_X, PosName.WaitPos);
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
                        var ret = IsAxisArrive(AxisName.WaferBuffer_X, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            if (IsManualMode == false)
                            {
                                var layerNoLeft = ProcInfoHelper.Instance.GetAvailableFullWaferSlotNo(Foup.Left.ToInt());
                                var layerNoRight = ProcInfoHelper.Instance.GetAvailableFullWaferSlotNo(Foup.Right.ToInt());
                                if (layerNoLeft==-1&& layerNoRight==-1)//两个Foup都没有Wafer
                                {
                                    ShowMsg($"{StationName}->两侧Foup中都没有Wafer，执行下Foup流程");
                                    LulFoupStep = 1;
                                    LulFoupSts[Foup.Left.ToInt()] = false;
                                    step = 110;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->获取左右Foup有Wafer的层号，左侧Foup：{layerNoLeft}，右侧Foup：{layerNoRight}");
                                    if (layerNoLeft >= layerNoRight)
                                    {
                                        UseFoupNo = Foup.Right;
                                        UseLayerNo = layerNoRight;
                                        var posInfo = AxisInfoHelper.GetPosInfoByName(AxisName.WaferBuffer_Z, PosName.Foup2PickPlacePos);
                                        WaferBuffer_ZPos = posInfo.Pos + ParaHelper.Prod.FoupPitch * layerNoRight;
                                        ShowMsg($"{StationName}->从右侧Foup第{layerNoRight}层中取Wafer，WaferBuffer_Z轴目标位置：{WaferBuffer_ZPos}");
                                    }
                                    else
                                    {
                                        UseFoupNo = Foup.Left;
                                        UseLayerNo = layerNoLeft;
                                        var posInfo = AxisInfoHelper.GetPosInfoByName(AxisName.WaferBuffer_Z, PosName.Foup1PickPlacePos);
                                        WaferBuffer_ZPos = posInfo.Pos + ParaHelper.Prod.FoupPitch * layerNoLeft;
                                        ShowMsg($"{StationName}->从左侧Foup第{layerNoLeft}层中取Wafer，WaferBuffer_Z轴目标位置：{WaferBuffer_ZPos}");
                                    }
                                    MoveAbs(AxisName.WaferBuffer_Z, WaferBuffer_ZPos);
                                    step = 150;
                                }
                            }
                            else
                            {
                                MoveAbs(AxisName.WaferBuffer_Z, WaferBuffer_ZPos);
                                step = 150;
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 50;
                        }
                        break;
                    }
                case 110:
                    {
                        LulFoupStep = UnloadFoupFlow(LulFoupStep);
                        if (LulFoupStep == 0)
                        {
                            ShowMsg($"{StationName}->下Foup流程完成，执行上Foup流程");
                            LulFoupStep = 1;
                            step = 20;
                        }
                        break;
                    }
                case 150:
                    {
                        var ret = IsAxisArrive(AxisName.WaferBuffer_Z, WaferBuffer_ZPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->Wafer缓存模组移动到待机位置");
                            Axes = new List<AxisName>() { AxisName.WaferBuffer_Y };
                            if (UseFoupNo == Foup.Left)
                            {
                                Poses = new List<PosName>() { PosName.Foup1PickPlacePos };
                            }
                            else
                            {
                                Poses = new List<PosName>() { PosName.Foup2PickPlacePos };
                            }
                            MoveAbsM(Axes, Poses);
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
                        var ret = IsAxisArriveM(Axes, Poses);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->WaferBufferX轴移动到从Foup中取Wafer位置");
                            MoveAbs(AxisName.WaferBuffer_X, PosName.ClampWaferFromFoup);
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
                        var ret = IsAxisArrive(AxisName.WaferBuffer_X, PosName.ClampWaferFromFoup);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->夹爪Z轴下降5毫米，贴近Wafer");
                            MoveRel(AxisName.WaferBuffer_Z, -5);//BufferZ轴下降5mm，贴近Wafer
                            step = 300;
                            if (Global.RunMode == RunMode.空跑模式)
                            {
                                IOInfoHelper.GetInputInfoByName(InputName.WaferBufferClawIsExistWafer.ToString(), out int _).IsShield = true;
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
                        var ret = IsAxisArrive(AxisName.WaferBuffer_Z);
                        if (ret == MotionStatus.Arrived)
                        {
                            if (Motion.GetDigitalInput(InputName.WaferBufferClawIsExistWafer))
                            {
                                ShowMsg($"{StationName}->夹爪Sensor检测到Wafer，夹爪气缸夹紧Wafer");
                                PutWaferToBufferStep = 1;
                                step = 350;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->夹爪Sensor未检测到Wafer，请选择如何处理", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.WaferClawNoDetectWafer.ToInt(), StationNum);
                                var result = AskHowToDoRequest(StationNum, $"第{UseLayerNo}层未检测到Wafer，请选择如何处理", "取下一片");
                                switch (result)
                                {
                                    case StdLib.PCBaseFrame.Args.AskResult.Skip:
                                        {
                                            ShowMsg($"{StationName}->处理结果：跳过", MsgType.Error, true);
                                            PutWaferToBufferStep = 1;
                                            step = 350;
                                        }
                                        break;
                                    case StdLib.PCBaseFrame.Args.AskResult.Retry:
                                        {
                                            ShowMsg($"{StationName}->处理结果：重试", MsgType.Error, true);
                                            break;
                                        }
                                    case StdLib.PCBaseFrame.Args.AskResult.Other:
                                        {
                                            ShowMsg($"{StationName}->处理结果：取下一片", MsgType.Error, true);
                                            step = 100;
                                        }
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
                        PutWaferToBufferStep = PutWaferToBufferFlow(PutWaferToBufferStep, true);
                        if (PutWaferToBufferStep == 0)
                        {
                            step = 0;
                            if (Global.RunMode == RunMode.空跑模式)
                            {
                                IOInfoHelper.GetInputInfoByName(InputName.WaferBufferClawIsExistWafer.ToString(), out int _).IsShield = false;
                            }
                        }
                        break;
                    }
            }
            return step;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="step"></param>
        /// <param name="isNewWafer"></param>
        /// <param name="isUpLayer"></param>
        /// <returns></returns>
        private int PutWaferToBufferFlow(int step, bool isNewWafer, bool isUpLayer = true)
        {
            try
            {
                switch (step)
                {
                    case 1:
                        {
                            ShowMsg($"{StationName}->放Wafer到缓存平台流程开始，夹爪气缸夹紧Wafer", MsgType.Notify);
                            Cylinder(CylinderName.WaferBufferClawCyl, true);//夹爪气缸夹紧Wafer
                            step = 350;
                            break;
                        }
                    case 350:
                        {
                            var ret = IsCylinderArrive(CylinderName.WaferBufferClawCyl, true);
                            if (ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->Z轴上升5mm，带动Wafer离开Foup");
                                MoveRel(AxisName.WaferBuffer_Z, 5);//BufferZ轴上升5mm，带动Wafer离开Foup
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
                            var ret = IsAxisArrive(AxisName.WaferBuffer_Z);
                            if (ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->WaferBufferX轴移动到释放Wafer位置1");
                                MoveAbs(AxisName.WaferBuffer_X, PosName.ClawXReleaseWaferPos1);//WaferBufferX轴移动到释放Wafer位置1
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
                            var ret = IsAxisArrive(AxisName.WaferBuffer_X, PosName.ClawXReleaseWaferPos1);
                            if (ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->夹爪气缸松开");
                                Cylinder(CylinderName.WaferBufferClawCyl, false);//夹爪气缸松开
                                step = 500;
                            }
                            else if (ret == MotionStatus.Timeout)
                            {
                                step = 450;
                            }
                            break;
                        }
                    case 500:
                        {
                            var ret = IsCylinderArrive(CylinderName.WaferBufferClawCyl, false);
                            if (ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->WaferBufferX轴回退10mm，离开Wafer");
                                MoveRel(AxisName.WaferBuffer_X, -10);//WaferBufferX轴回退10mm，离开Wafer
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
                            var ret = IsAxisArrive(AxisName.WaferBuffer_X);
                            if (ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->夹爪Z轴下降到待机位置");
                                MoveAbs(AxisName.WaferBuffer_ClawZ, PosName.WaitPos);//BufferZ轴下降到待机位置
                                step = 580;
                            }
                            else if (ret == MotionStatus.Timeout)
                            {
                                step = 500;
                            }
                            break;
                        }
                    case 580:
                        {
                            var ret = IsAxisArrive(AxisName.WaferBuffer_ClawZ, PosName.WaitPos);
                            if (ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->WaferBufferX轴移动到吸盘第一次吸Wafer位置");
                                MoveAbs(AxisName.WaferBuffer_X, PosName.ClawXSuckWaferPos1);//WaferBufferX轴移动到吸盘第一次吸Wafer位置
                                step = 600;
                            }
                            else if (ret == MotionStatus.Timeout)
                            {
                                step = 550;
                            }
                            break;
                        }
                    case 600:
                        {
                            var ret = IsAxisArrive(AxisName.WaferBuffer_X, PosName.ClawXSuckWaferPos1);
                            if (ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->夹爪Z轴上升到吸盘吸Wafer位置");
                                MoveAbs(AxisName.WaferBuffer_ClawZ, isUpLayer ? PosName.UpLayerSuckWaferPos : PosName.DownLayerSuckWaferPos);//夹爪Z轴上升到吸盘吸Wafer位置
                                step = 700;
                            }
                            else if (ret == MotionStatus.Timeout)
                            {
                                step = 550;
                            }
                            break;
                        }
                    case 700:
                        {
                            var ret = IsAxisArrive(AxisName.WaferBuffer_ClawZ, isUpLayer ? PosName.UpLayerSuckWaferPos : PosName.DownLayerSuckWaferPos);
                            if (ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->吸盘吸真空");
                                Vacuum(VacuumName.WaferBufferClawVac, true);//吸盘吸真空
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
                            var ret = IsVacuumArrive(VacuumName.WaferBufferClawVac, true);
                            if (ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->WaferBufferX轴移动到释放Wafer位置2");
                                MoveAbs(AxisName.WaferBuffer_X, PosName.ClawXReleaseWaferPos2);//WaferBufferX轴移动到吸盘第一次吸Wafer位置
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
                            var ret = IsAxisArrive(AxisName.WaferBuffer_X, PosName.ClawXReleaseWaferPos2);
                            if (ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->吸盘破真空");
                                Vacuum(VacuumName.WaferBufferClawVac, false);//吸盘破真空
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
                            var ret = IsVacuumArrive(VacuumName.WaferBufferClawVac, false);
                            if (ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->夹爪Z轴下降到待机位置");
                                MoveAbs(AxisName.WaferBuffer_ClawZ, PosName.WaitPos);//夹爪Z轴下降到待机位置
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
                            var ret = IsAxisArrive(AxisName.WaferBuffer_ClawZ, PosName.WaitPos);
                            if (ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->WaferBufferX轴移动到吸盘第二次吸Wafer位置，夹爪伸缩气缸伸出");
                                MoveAbs(AxisName.WaferBuffer_X, PosName.ClawXSuckWaferPos2);//WaferBufferX轴移动到吸盘第二次吸Wafer位置
                                Cylinders = new List<CylinderName>() { CylinderName.WaferBufferStretchCyl };
                                CylinderM(Cylinders, true);//夹爪伸缩气缸伸出
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
                            var ret = IsAxisArrive(AxisName.WaferBuffer_X, PosName.ClawXSuckWaferPos2);
                            var ret2 = IsCylinderArriveM(Cylinders, true);
                            if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->夹爪Z轴上升到吸盘吸Wafer位置");
                                MoveAbs(AxisName.WaferBuffer_ClawZ, isUpLayer ? PosName.UpLayerSuckWaferPos : PosName.DownLayerSuckWaferPos);//夹爪Z轴上升到吸盘吸Wafer位置
                                step = 1000;
                            }
                            else if (ret == MotionStatus.Timeout || ret2 == MotionStatus.Timeout)
                            {
                                step = 900;
                            }
                            break;
                        }
                    case 1000:
                        {
                            var ret = IsAxisArrive(AxisName.WaferBuffer_ClawZ, isUpLayer ? PosName.UpLayerSuckWaferPos : PosName.DownLayerSuckWaferPos);
                            if (ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->吸盘吸真空");
                                Vacuum(VacuumName.WaferBufferClawVac, true);//吸盘吸真空
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
                            var ret = IsVacuumArrive(VacuumName.WaferBufferClawVac, true);
                            if (ret == MotionStatus.Arrived)
                            {
                                if (isNewWafer)
                                {
                                    ShowMsg($"{StationName}->WaferBufferX轴移动到扫码位置");
                                    MoveAbs(AxisName.WaferBuffer_X, PosName.ScancodePos);//WaferBufferX轴移动到扫码位置
                                    step = 1100;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->WaferBufferX轴移动到释放Wafer位置3");
                                    MoveAbs(AxisName.WaferBuffer_X, PosName.ClawXReleaseWaferPos3);//WaferBufferX轴移动到释放Wafer位置3
                                    step = 1200;
                                }
                            }
                            else if (ret == MotionStatus.Timeout)
                            {
                                step = 1000;
                            }
                            break;
                        }
                    case 1100:
                        {
                            var ret = IsAxisArrive(AxisName.WaferBuffer_X, PosName.ScancodePos);
                            if (ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->WaferBufferX轴到达扫码位置，开始扫码");
                                Barcode = ScanerHelper.Instance.Scan();
                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    Barcode = "T08915-12-H1";
                                }
                                if (string.IsNullOrEmpty(Barcode))
                                {
                                    ShowMsg($"{StationName}->扫码失败，请选择如何处理", MsgType.Error);
                                    AlarmManager.CreateAlarm(AlarmCode.ScancodeFail.ToInt(), StationNum);
                                    var result = AskHowToDoRequest(StationNum, "Wafer扫码失败，请选择如何处理", "手动输入");
                                    switch (result)
                                    {
                                        case AskResult.Skip:
                                            {
                                                Barcode = DateTime.Now.ToString("yyyyMMddHHmmss");
                                                ShowMsg($"{StationName}->处理结果：跳过，使用日期码：{Barcode}");
                                                step = 1150;
                                            }
                                            break;
                                        case AskResult.Retry:
                                            {
                                                ShowMsg($"{StationName}->处理结果：重试");
                                            }
                                            break;
                                        case AskResult.Other:
                                            {
                                                Barcode = OnScancodeFailConfirm?.Invoke();
                                                ShowMsg($"{StationName}->处理结果：手动输入条码：{Barcode}");
                                                step = 1150;
                                            }
                                            break;
                                    }
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->扫码完成，结果：{Barcode}");
                                    step = 1150;
                                }
                            }
                            else if (ret == MotionStatus.Timeout)
                            {
                                step = 1050;
                            }
                            break;
                        }
                    case 1150:
                        {
                            if (isNewWafer == false)
                            {
                                ShowMsg($"{StationName}->从Eap获取Map信息");
                                var wafer = ProcInfoHelper.Instance.GetWaferModel(WaferPosEnum.WaferBufferUpLayer);
                                wafer.MapFilePath = $"WaferMap\\{Barcode}.txt";
                                wafer.Code = Barcode;
                                ProcInfoHelper.Instance.UpdateWafer(WaferPosEnum.WaferBufferUpLayer, wafer);
                            }
                            ShowMsg($"{StationName}->WaferBufferX轴移动到释放Wafer位置3");
                            MoveAbs(AxisName.WaferBuffer_X, PosName.ClawXReleaseWaferPos3);//WaferBufferX轴移动到释放Wafer位置3
                            step = 1200;
                            break;
                        }
                    case 1200:
                        {
                            var ret = IsAxisArrive(AxisName.WaferBuffer_X, PosName.ClawXReleaseWaferPos3);
                            if (ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->吸盘破真空");
                                Vacuum(VacuumName.WaferBufferClawVac, false);//吸盘破真空
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
                            var ret = IsVacuumArrive(VacuumName.WaferBufferClawVac, false);
                            if (ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->夹爪Z轴下降到待机位置");
                                MoveAbs(AxisName.WaferBuffer_ClawZ, PosName.WaitPos);//夹爪Z轴下降到待机位置
                                step = 1300;
                            }
                            else if (ret == MotionStatus.Timeout)
                            {
                                step = 1200;
                            }
                            break;
                        }
                    case 1300:
                        {
                            var ret = IsAxisArrive(AxisName.WaferBuffer_ClawZ, PosName.WaitPos);
                            if (ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->从Foup中取Wafer完成，Wafer模组移动到待机位");
                                if (isNewWafer)
                                {
                                    ProcInfoHelper.Instance.PickWaferFromFoup(UseFoupNo.ToInt(), UseLayerNo);
                                    var wafer = new WaferModel()
                                    {
                                        FoupNo = UseFoupNo.ToInt(),
                                        LayerNo = UseLayerNo
                                    };
                                    wafer.MapFilePath = $"WaferMap\\{Barcode}.txt";
                                    wafer.Code = Barcode;
                                    ProcInfoHelper.Instance.PutWafer(WaferPosEnum.WaferBufferUpLayer, wafer);
                                }
                                Axes = new List<AxisName>() { AxisName.WaferBuffer_Y, AxisName.WaferBuffer_Z, AxisName.WaferBuffer_ClawZ };
                                Poses = new List<PosName>() { PosName.WaitPos };
                                Cylinder(CylinderName.WaferBufferStretchCyl, false);
                                MoveAbsM(Axes, Poses);
                                step = 1350;
                            }
                            else if (ret == MotionStatus.Timeout)
                            {
                                step = 1250;
                            }
                            break;
                        }
                    case 1350:
                        {
                            var ret = IsAxisArriveM(Axes, Poses);
                            var ret2 = IsCylinderArrive(CylinderName.WaferBufferStretchCyl, false);
                            if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->Wafer模组到达待机位");
                                step = 0;
                            }
                            else if (ret == MotionStatus.Timeout || ret2 == MotionStatus.Timeout)
                            {
                                step = 1300;
                            }
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                ShowMsg($"{StationName}->PutWaferToBufferFlow流程异常，异常原因：{ex}", MsgType.Error);
            }
            return step;
        }
        /// <summary>
        /// WaferTable从缓存平台取Wafer流程
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int WaferTablePickWaferFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"Wafer缓存平台模组移动到{PosName.WaferFromBufferToTablePos}位置");
                        Axes = new List<AxisName>() { AxisName.WaferBuffer_Y, AxisName.WaferBuffer_Z };
                        Poses = new List<PosName>() { PosName.WaferFromBufferToTablePos };
                        MoveAbsM(Axes, Poses);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArriveM(Axes, Poses);
                        if (ret == MotionStatus.Arrived)
                        {
                            RegisterService.Instance.SetBoolReg(RegisterEnum.WaferBufferAllowPickPlaceWafer);
                            ShowMsg($"Wafer缓存平台模组到达{PosName.WaferFromBufferToTablePos}位置，等待WaferTable取Wafer完成");
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
                        if (GetBoolReg(RegisterEnum.WaferTableLoadWaferDone))
                        {
                            ResetBoolReg(RegisterEnum.WaferTableLoadWaferDone);
                            ShowMsg("WaferTable取Wafer完成");
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
        /// WaferTable放Wafer到缓存平台流程
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int WaferTablePlaceWaferFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"Wafer缓存平台模组移动到{PosName.WaferFromTableToBufferPos}位置");
                        Axes = new List<AxisName>() { AxisName.WaferBuffer_Y, AxisName.WaferBuffer_Z };
                        Poses = new List<PosName>() { PosName.WaferFromTableToBufferPos };
                        MoveAbsM(Axes, Poses);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArriveM(Axes, Poses);
                        if (ret == MotionStatus.Arrived)
                        {
                            RegisterService.Instance.SetBoolReg(RegisterEnum.WaferBufferAllowPickPlaceWafer);
                            ShowMsg($"Wafer缓存平台模组到达{PosName.WaferFromTableToBufferPos}位置，等待WaferTable放Wafer完成");
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
                        if (GetBoolReg(RegisterEnum.WaferTableUnloadWaferDone))
                        {
                            ResetBoolReg(RegisterEnum.WaferTableUnloadWaferDone);
                            ShowMsg("WaferTable放Wafer完成");
                            step = 0;
                        }
                        break;
                    }
                default:
                    break;
            }
            return step;
        }
        public int MoveToWaitPosFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        if (Motion.GetDigitalInput(InputName.WaferBufferClawIsExistWafer))
                        {
                            ShowMsg($"{StationName}-夹爪Sensor检测到Wafer，把Wafer放回缓存平台上");
                            MoveAbs(AxisName.WaferBuffer_X, PosName.JudgeLayerPos);
                            step = 50;
                        }
                        else
                        {
                            ShowMsg($"{StationName}-夹爪Z轴移动到等待位置");
                            MoveAbs(AxisName.WaferBuffer_ClawZ, PosName.WaitPos);
                            step = 200;
                        }
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArrive(AxisName.WaferBuffer_X, PosName.JudgeLayerPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}-WaferBufferX轴移动到判断层位置，判断Wafer是哪一层");
                            if (Motion.GetDigitalInput(InputName.UpLayerWaferDetect2))
                            {
                                ShowMsg($"{StationName}-Wafer是上层，执行把Wafer放回缓存平台流程");
                                step = 100;
                            }
                            else
                            {
                                ShowMsg($"{StationName}-Wafer是下层，执行把Wafer放回缓存平台流程");
                                step = 150;
                            }
                            PutWaferToBufferStep = 1;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1;
                        }
                        break;
                    }
                case 100:
                    {
                        PutWaferToBufferStep = PutWaferToBufferFlow(PutWaferToBufferStep, false);
                        if (PutWaferToBufferStep == 0)
                        {
                            step = 0;
                        }
                        break;
                    }
                case 150:
                    {
                        PutWaferToBufferStep = PutWaferToBufferFlow(PutWaferToBufferStep, false, false);
                        if (PutWaferToBufferStep == 0)
                        {
                            step = 0;
                        }
                        break;
                    }
                case 200:
                    {
                        var ret = IsAxisArrive(AxisName.WaferBuffer_ClawZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}-WaferBufferX轴移动到等待位置");
                            MoveAbs(AxisName.WaferBuffer_X, PosName.WaitPos);
                            step = 250;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1;
                        }
                        break;
                    }
                case 250:
                    {
                        var ret = IsAxisArrive(AxisName.WaferBuffer_X, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1;
                        }
                        break;
                    }
            }
            return step;
        }
        private int LoadFoupFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        LulFoupSts[0] = false;
                        LulFoupSts[1] = false;
                        ShowMsg($"{StationName}->LoadFoup流程开始，发送上Foup请求给LoadPort",MsgType.Notify);
                        LoadPortHelper.Instance.GetLoadPortDevice(Foup.Left.ToInt()).LpLoadAsync();
                        LoadPortHelper.Instance.GetLoadPortDevice(Foup.Right.ToInt()).LpLoadAsync();
                        Delay.Start(60);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        if (LulFoupSts[0]&& LulFoupSts[1])
                        {
                            ShowMsg($"{StationName}->LoadPort上Foup完成", MsgType.Notify);
                            step = 0;
                        }
                        else if (Delay.TimeUp())
                        {
                            if (LulFoupSts[0]==false)
                            {
                                ShowMsg($"{StationName}->LoadPort1上Foup超时", MsgType.Error);
                                AlarmManager.CreateAlarm(AlarmCode.LoadPort1LoadFoupFail.ToInt(),StationNum);
                            }
                            else if (LulFoupSts[1]==false)
                            {
                                ShowMsg($"{StationName}->LoadPort2上Foup超时", MsgType.Error);
                                AlarmManager.CreateAlarm(AlarmCode.LoadPort2LoadFoupFail.ToInt(), StationNum);
                            }
                        }
                        break;
                    }
            }
            return step;
        }
        private int UnloadFoupFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->LoadFoup流程开始，发送上Foup请求给LoadPort", MsgType.Notify);
                        LoadPortHelper.Instance.GetLoadPortDevice(Foup.Left.ToInt()).LpUnloadAsync();
                        LoadPortHelper.Instance.GetLoadPortDevice(Foup.Right.ToInt()).LpUnloadAsync();
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        if (LulFoupSts[0] && LulFoupSts[1])
                        {
                            ShowMsg($"{StationName}->LoadPort下Foup完成", MsgType.Notify);
                            step = 0;
                        }
                        break;
                    }
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
            LoadPortHelper.Instance.GetLoadPortDevice(0).LoadStatusChanged += WaferBufferStn_LoadStatusChanged;
            LoadPortHelper.Instance.GetLoadPortDevice(1).LoadStatusChanged += WaferBufferStn_LoadStatusChanged;
        }

        private void WaferBufferStn_LoadStatusChanged(int num,Map.Devices.EFEM.LoadTypes arg1, LPCodes arg2, List<SlotStates> arg3)
        {
            try
            {
                if (arg2 == LPCodes.SUCCESS)
                {
                    if (arg1 == Map.Devices.EFEM.LoadTypes.LpLoad)
                    {
                        LulFoupSts[num] = true;
                        if (arg3!=null&& arg3.Count>0)
                        {
                            FoupModel model = new FoupModel();
                            model.InEqpTime=DateTime.Now;
                            model.FoupNo= num;
                            model.TotalSlot= arg3.Count;
                            model.RemainWaferCount = arg3.Count;
                            var arr = Enumerable.Repeat(1, arg3.Count).ToArray();
                            for (int i = 0; i < arg3.Count; i++)
                            {
                                if (arg3[i]==SlotStates.HasWafter)
                                {
                                    arr[i] = 1;
                                }
                                else
                                {
                                    arr[i] = 0;
                                }
                            }
                            model.WaferMap = string.Join(",", arr);
                            var device = RfidDeviceHelper.Instance.GetRfidDevice(num);
                            if (device!=null)
                            {
                                var ret = device.ReadCarrierId(out string rfid);
                                if (ret && !string.IsNullOrEmpty(rfid))
                                {
                                    model.Rfid = rfid;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->Rfid读取失败，序号：{num}", MsgType.Error);
                                }
                            }
                            else
                            {
                                ShowMsg($"{StationName}->获取Rfid对象失败，序号：{num}", MsgType.Error);
                            }
                            ProcInfoHelper.Instance.LoadFoup(num, model);
                        }
                    }
                    else if (arg1 == Map.Devices.EFEM.LoadTypes.OHTUnload)
                    {
                        LulFoupSts[num] = true;
                        ProcInfoHelper.Instance.UnloadFoup(num);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMsg($"{StationName}->LoadPortDevice1 LoadStatusChanged处理方法执行异常：{ex.ToString()}", MsgType.Error);
            }
        }
        public void ResetBits()
        {
            // SubFlowStep = -1;
            PutWaferToBufferStep = 1;
        }

        public void StopHandle()
        {
        }

        public void WaitFlowDone()
        {
        }
    }
}
