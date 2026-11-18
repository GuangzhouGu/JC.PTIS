using JC.PTIS.Main.AlarmInfo;
using JC.PTIS.Main.Devices.Vision;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Params;
using JC.PTIS.Main.RotateInfo;
using JC.PTIS.Main.Service;
using StdLib.LogHelper;
using StdLib.PCBaseFrame.Alarm;
using StdLib.PCBaseFrame.Entities;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.PCBaseFrame.Extension.Entities;
using StdLib.PCBaseFrame.Motion;
using StdLib.PCBaseFrame.Station;
using StdLib.WindowsControls.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Stations
{
    public class LotBoxStn : StationBaseEx, IStation
    {
        public int StationNum { get; set; } = 16;
        public string StationName { get; set; } = "LotBoxStn";

        int SubFlowStep = -1;

        List<AxisName> Axes = new List<AxisName>();
        List<PosName> PosNames = new List<PosName>();
        List<double> Poses = new List<double>();

        LotBoxType LotBoxType = LotBoxType.EmptyTray;
        LotBoxType Stn2LotBox = LotBoxType.Null;
        int LotBoxStnNo = -1;
        int ExBoxStnNo = -1;
        int NullStnNo = -1;

        Delay Delay = new Delay();
        public void Home()
        {
            switch (HomeStep)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->开始回零");
                        Motion.Cylinder(CylinderName.LotBoxTransferLiftCyl, false);
                        ShowMsg($"{StationName}->下料顶升气缸缩回");
                        Motion.Home(AxisName.LotBox_ArmZ);
                        ShowMsg($"{StationName}->LotBox_ArmZ轴回零");
                        HomeStep = 50;
                        break;
                    }
                case 50:
                    {
                        if (Motion.IsHomeDone(AxisName.LotBox_ArmZ) && Motion.IsCylinderArrive(CylinderName.LotBoxTransferLiftCyl, false))
                        {
                            ShowMsg($"{StationName}->下料顶升气缸缩回到位");
                            ShowMsg($"{StationName}->LotBox_ArmZ轴回零完成");
                            Motion.Home(AxisName.LotBox_ArmX);
                            ShowMsg($"{StationName}->LotBox_ArmX轴回零");
                            HomeStep = 100;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.LotBox_ArmZ))
                        {
                            ShowMsg($"{StationName}->LotBox_ArmZ轴回零超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        else if (Motion.IsCylinderTimeOut(CylinderName.LotBoxTransferLiftCyl))
                        {
                            ShowMsg($"{StationName}->下料顶升气缸缩回超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 100:
                    {
                        if (Motion.IsHomeDone(AxisName.LotBox_ArmX))
                        {
                            ShowMsg($"{StationName}->LotBox_ArmX轴回零完成");
                            Motion.Home(AxisName.LotBox_ArmY);
                            ShowMsg($"{StationName}->LotBox_ArmY轴回零");
                            HomeStep = 150;


                            //if (Global.RunMode == RunMode.空跑模式)
                            //{
                            //    IOInfoHelper.GetInputInfoByName(InputName.ExBoxStn2LoadDone.ToString(), out int _).IsShield = false;
                            //    IOInfoHelper.GetInputInfoByName(InputName.ExBoxStn1LoadDone.ToString(), out int _).IsShield = false;
                            //    for (int i = 1; i <= 4; i++)
                            //    {
                            //        IOInfoHelper.GetInputInfoByName($"LotBox{i}IsExist1", out int _).IsShield = false;
                            //        IOInfoHelper.GetInputInfoByName($"LotBox{i}IsExist2", out int _).IsShield = false;
                            //    }
                            //    IOInfoHelper.GetInputInfoByName(InputName.Stn8LulStageDetect.ToString(), out int _).IsShield = false;
                            //}
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.LotBox_ArmX))
                        {
                            ShowMsg($"{StationName}->LotBox_ArmX轴回零超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 150:
                    {
                        if (Motion.IsHomeDone(AxisName.LotBox_ArmY))
                        {
                            ShowMsg($"{StationName}->LotBox_ArmY轴回零完成");
                            Motion.MoveAbs(AxisName.LotBox_ArmX, PosName.WaitPos);
                            Motion.MoveAbs(AxisName.LotBox_ArmY, PosName.WaitPos);
                            ShowMsg($"{StationName}->{AxisName.LotBox_ArmX}轴和{AxisName.LotBox_ArmY}轴移动到{PosName.WaitPos}位");
                            HomeStep = 200;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.LotBox_ArmY))
                        {
                            ShowMsg($"{StationName}->LotBox_ArmY轴回零超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 200:
                    {
                        if (Motion.IsArrive(AxisName.LotBox_ArmX, PosName.WaitPos) && Motion.IsArrive(AxisName.LotBox_ArmY, PosName.WaitPos))
                        {
                            ShowMsg($"{StationName}->{AxisName.LotBox_ArmX}轴和{AxisName.LotBox_ArmY}轴到达{PosName.WaitPos}");
                            RotateInfoHelper.Instance.UpdateLotBoxStn(LotBoxPosEnum.Home);
                            UpdateHomeSts(StationName, StationNum, true);
                            HomeStep = 0;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.LotBox_ArmX))
                        {
                            HomeStep = 1000;
                            ShowMsg($"{StationName}->{AxisName.LotBox_ArmX}轴移动到{PosName.WaitPos}", MsgType.Error);
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.LotBox_ArmX))
                        {
                            HomeStep = 1000;
                            ShowMsg($"{StationName}->{AxisName.LotBox_ArmY}轴移动到{PosName.WaitPos}", MsgType.Error);
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
                            ShowMsg($"{StationName}->模组移动到待机位");
                            SubFlowStep = 1;
                            AutoRunStep = 50;
                            break;
                        }
                    case 50:
                        {
                            SubFlowStep = MoveToWaitPosFlow(SubFlowStep);
                            if (SubFlowStep == 0)
                            {
                                ShowMsg($"{StationName}->模组移动到待机位完成，判断空Tray平台中是否有内盒");
                                AutoRunStep = 100;
                            }
                            break;
                        }
                    case 100:
                        {
                            if (Motion.GetDigitalInput(InputName.LotBox2IsExist1) ||
                                Motion.GetDigitalInput(InputName.LotBox2IsExist2))
                            {
                                ShowMsg($"{StationName}->空Lot平台中有内盒，判断内盒中是否还有Lot");
                                var rcNo = ProcInfoHelper.Instance.GetAvlEmptyLotCellNo();
                                if (rcNo.r == -1 && rcNo.c == -1)
                                {
                                    ShowMsg($"{StationName}->空Tray内盒中没有可用空Tray，把空内盒暂存到外盒2平台");
                                    SubFlowStep = 1;
                                    AutoRunStep = 150;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->空Tray内盒中有可用空Tray，判断Bin135 Lot平台是否有内盒");
                                    AutoRunStep = 110;
                                }
                                break;
                            }
                            else
                            {
                                SubFlowStep = 1;
                                LotBoxType = LotBoxType.EmptyTray;
                                ShowMsg($"{StationName}->空Lot平台中没有内盒，请求天车上空Tray内盒。LotBoxType：{LotBoxType}");
                                AutoRunStep = 400;
                            }
                            break;
                        }
                    case 110:
                        {
                            if (Motion.GetDigitalInput(InputName.LotBox1IsExist1) ||
                                Motion.GetDigitalInput(InputName.LotBox1IsExist2))
                            {
                                ShowMsg($"{StationName}->Bin135 Lot平台中有内盒，判断内盒中是否全部放满Lot");
                                var rcNo2 = ProcInfoHelper.Instance.GetLotBoxInfo(LotBoxStnType.OKBinStn);
                                if (rcNo2.IsFull)
                                {
                                    ShowMsg($"{StationName}->Bin135 内盒已满，把满内盒暂存到外盒1平台");
                                    SubFlowStep = 1;
                                    AutoRunStep = 200;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->Bin135 内盒未满，判断Bin246 Lot平台是否有内盒");
                                    if (Motion.GetDigitalInput(InputName.LotBox3IsExist1) &&
                                        Motion.GetDigitalInput(InputName.LotBox3IsExist2))
                                    {
                                        ShowMsg($"{StationName}->Bin246 Lot平台中有内盒，判断内盒中是否全部放满Lot");
                                        var rcNo3 = ProcInfoHelper.Instance.GetLotBoxInfo(LotBoxStnType.NGBinStn);
                                        if (rcNo3.IsFull)
                                        {
                                            ShowMsg($"{StationName}->Bin246 内盒已满，把满内盒暂存到外盒1平台");
                                            SubFlowStep = 1;
                                            AutoRunStep = 300;
                                        }
                                        else
                                        {
                                            ShowMsg($"{StationName}->Bin246 内盒未满，等待PackingStn请求信号");
                                            AutoRunStep = 350;
                                        }
                                    }
                                    else
                                    {
                                        SubFlowStep = 1;
                                        LotBoxType = LotBoxType.Bin246;
                                        ShowMsg($"{StationName}->Bin246内盒平台中没有内盒，请求天车上空Tray内盒。LotBoxType：{LotBoxType}");
                                        AutoRunStep = 400;
                                    }
                                }
                            }
                            else
                            {
                                SubFlowStep = 1;
                                LotBoxType = LotBoxType.Bin135;
                                ShowMsg($"{StationName}->Bin135内盒平台中没有内盒，请求天车上空Tray内盒。LotBoxType：{LotBoxType}");
                                AutoRunStep = 400;
                            }
                            break;
                        }
                    case 150:
                        {
                            SubFlowStep = LotBoxFromInnerBoxStageToExBoxStageFlow(SubFlowStep, LotBoxType.EmptyTray, 1);
                            if (SubFlowStep == 0)
                            {
                                ShowMsg($"{StationName}->Lot内盒从内盒平台搬运到外盒平台流程结束");
                                AutoRunStep = 100;
                            }
                            break;
                        }
                    case 200:
                        {
                            SubFlowStep = LotBoxFromInnerBoxStageToExBoxStageFlow(SubFlowStep, LotBoxType.Bin135, 0);
                            if (SubFlowStep == 0)
                            {
                                ShowMsg($"{StationName}->Bin135满Lot已搬运到外盒1平台，请求天车取内盒");
                                SubFlowStep = 1;
                                AutoRunStep = 250;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.ExBox1IsExist1.ToString(), out int _).IsShield = true;
                                    IOInfoHelper.GetInputInfoByName(InputName.ExBox1IsExist2.ToString(), out int _).IsShield = true;
                                }
                            }
                            break;
                        }
                    case 250:
                        {
                            SubFlowStep = RequestOHTUnloadLotBoxFlow(SubFlowStep);
                            if (SubFlowStep == 0)
                            {
                                ShowMsg($"{StationName}->请求天车满Lot内盒流程结束");
                                AutoRunStep = 100;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.ExBox1IsExist1.ToString(), out int _).IsShield = false;
                                    IOInfoHelper.GetInputInfoByName(InputName.ExBox1IsExist2.ToString(), out int _).IsShield = false;
                                    IOInfoHelper.GetInputInfoByName(InputName.ExBox2IsExist1.ToString(), out int _).IsShield = false;
                                    IOInfoHelper.GetInputInfoByName(InputName.ExBox2IsExist2.ToString(), out int _).IsShield = false;
                                }
                            }
                            break;
                        }
                    case 300:
                        {
                            SubFlowStep = LotBoxFromInnerBoxStageToExBoxStageFlow(SubFlowStep, LotBoxType.Bin246, 0);
                            if (SubFlowStep == 0)
                            {
                                ShowMsg($"{StationName}->Bin246满Lot已搬运到外盒1平台，请求天车取内盒");
                                SubFlowStep = 1;
                                AutoRunStep = 250;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.ExBox2IsExist1.ToString(), out int _).IsShield = true;
                                    IOInfoHelper.GetInputInfoByName(InputName.ExBox2IsExist2.ToString(), out int _).IsShield = true;
                                }
                            }
                            break;
                        }
                    case 350:
                        {
                            if (GetBoolReg(RegisterEnum.PickLotFromLotBoxReq))
                            {
                                ShowMsg($"{StationName}->收到PackingStn工位上空Lot请求，把空Tray内盒搬运到工位2位置");
                                ResetBoolReg(RegisterEnum.PickLotFromLotBoxReq);
                                SubFlowStep = 1;
                                LotBoxType = LotBoxType.EmptyTray;
                                AutoRunStep = 500;
                            }
                            else if (GetBoolReg(RegisterEnum.PutFullLotToLotBoxReq))
                            {
                                var lotInfo = ProcInfoHelper.Instance.GetLotStageInfo(LotPosEnum.LotArmClaw);
                                if (lotInfo.LotModel != null)
                                {
                                    var binLevel = lotInfo.LotModel.BinLevel;
                                    ShowMsg($"{StationName}->收到PackingStn工位下Lot请求，Lot Bin等级:{binLevel}");
                                    ResetBoolReg(RegisterEnum.PutFullLotToLotBoxReq);
                                    SubFlowStep = 1;
                                    LotBoxType = LotBoxType.Bin135;
                                    if (binLevel.ToInt() % 2 == 0)
                                    {
                                        LotBoxType = LotBoxType.Bin246;
                                    }
                                    ShowMsg($"{StationName}->把{LotBoxType}内盒搬运到工位2位置");
                                    AutoRunStep = 500;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->收到PackingStn工位下Lot请求，但Lot信息为空，无法判断Bin等级", MsgType.Error);
                                    AlarmManager.CreateAlarm(AlarmCode.GetLotModelFail.ToInt(), StationNum);
                                }
                            }
                            break;
                        }
                    case 400:
                        {
                            SubFlowStep = RequestOHTLoadLotBoxFlow(SubFlowStep, LotBoxType == LotBoxType.EmptyTray);
                            if (SubFlowStep == 0)
                            {
                                ShowMsg($"{StationName}->请求天车上内盒完成，执行搬运内盒到内盒平台流程，LotBoxType：{LotBoxType}");
                                SubFlowStep = 1;
                                AutoRunStep = 450;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    if (LotBoxType == LotBoxType.EmptyTray)
                                    {
                                        IOInfoHelper.GetInputInfoByName(InputName.LotBox2IsExist1.ToString(), out int _).IsShield = true;
                                        IOInfoHelper.GetInputInfoByName(InputName.LotBox2IsExist2.ToString(), out int _).IsShield = true;
                                    }
                                    else if (LotBoxType == LotBoxType.Bin135)
                                    {
                                        IOInfoHelper.GetInputInfoByName(InputName.LotBox1IsExist1.ToString(), out int _).IsShield = true;
                                        IOInfoHelper.GetInputInfoByName(InputName.LotBox1IsExist2.ToString(), out int _).IsShield = true;
                                    }
                                    else if (LotBoxType == LotBoxType.Bin246)
                                    {
                                        IOInfoHelper.GetInputInfoByName(InputName.LotBox3IsExist1.ToString(), out int _).IsShield = true;
                                        IOInfoHelper.GetInputInfoByName(InputName.LotBox3IsExist2.ToString(), out int _).IsShield = true;
                                    }
                                }
                            }
                            break;
                        }
                    case 450:
                        {
                            SubFlowStep = LotBoxFromExBoxStageToInnerBoxStageFlow(SubFlowStep, LotBoxType, ExBoxStnNo);
                            if (SubFlowStep == 0)
                            {
                                ShowMsg($"{StationName}->上空Tray满Lot内盒到内盒平台流程结束");
                                AutoRunStep = 100;
                            }
                            break;
                        }
                    case 500:
                        {
                            SubFlowStep = MoveLotBoxToStn2Flow(SubFlowStep, LotBoxType);
                            if (SubFlowStep == 0)
                            {
                                AutoRunStep = 110;
                                if (LotBoxType == LotBoxType.EmptyTray)
                                {
                                    RegisterService.Instance.SetBoolReg(RegisterEnum.LotBoxStnAllowPickLot);
                                    ShowMsg($"{StationName}->把空Tray内盒搬运到平台2位置结束");
                                    RotateInfoHelper.Instance.UpdateLotBoxStn(LotBoxPosEnum.Home);
                                    AutoRunStep = 100;
                                }
                                else if (LotBoxType == LotBoxType.Bin246)
                                {
                                    ShowMsg($"{StationName}->把{LotBoxType}内盒搬运到平台2位置结束");
                                    RotateInfoHelper.Instance.UpdateLotBoxStn(LotBoxPosEnum.Stn2Null);
                                    RotateInfoHelper.Instance.UpdateLotBoxStn(LotBoxPosEnum.Bin246InStn2);
                                }
                                ShowMsg($"{StationName}->通知PackingStn可以放Lot到内盒中");
                                RegisterService.Instance.SetBoolReg(RegisterEnum.LotBoxStnAllowPutFullLot);
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
        /// 把指定内盒移动到工位2位置。工位3和工位4位置的内盒因为机构干涉不能取放Lot
        /// </summary>
        /// <param name="step"></param>
        /// <param name="lotBoxType"></param>
        /// <returns></returns>
        public int MoveLotBoxToStn2Flow(int step, LotBoxType lotBoxType)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->内盒移动到工位2位置流程开始。目标内盒：{lotBoxType}", MsgType.Notify);
                        if (lotBoxType == LotBoxType.Bin246 || lotBoxType == LotBoxType.EmptyTray)
                        {
                            Stn2LotBox = (LotBoxType)RotateInfoHelper.Instance.GetStageNo(LotBoxStnType.LulLotStn.ToInt(), RotateStnType.LotBox);
                            ShowMsg($"{StationName}->工位2内盒：{Stn2LotBox}");
                            if (Stn2LotBox == lotBoxType)
                            {
                                ShowMsg($"{StationName}->目标内盒：{lotBoxType}在已经在工位2，无需移动");
                                step = 0;
                            }
                            else
                            {
                                MoveAbs(AxisName.LotBox_ArmZ, PosName.WaitPos);
                                step = 50;
                            }
                        }
                        else
                        {
                            ShowMsg($"{StationName}->内盒：{lotBoxType}在固定位置，不用切换");
                            step = 0;
                        }
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArrive(AxisName.LotBox_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            if (Motion.GetDigitalInput(InputName.LotBox2IsExist1) &&
                                Motion.GetDigitalInput(InputName.LotBox2IsExist2))
                            {
                                ShowMsg($"{StationName}->工位2没有内盒，把目标内盒：{lotBoxType}移动到工位2");
                                ShowMsg($"{StationName}->ArmZ轴到达待机位，ArmX轴和ArmY轴移动到{lotBoxType}内盒所在位置");
                                LotBoxStnNo = RotateInfoHelper.Instance.GetStnNo(lotBoxType.ToInt(), RotateStnType.LotBox);
                                var posName = GetPosName(LotBoxStnNo, PosType.PPLotBoxFromInBoxPos, false);
                                Axes = new List<AxisName>() { AxisName.LotBox_ArmX, AxisName.LotBox_ArmY };
                                PosNames = new List<PosName>() { posName };
                                MoveAbsM(Axes, PosNames);
                                step = 100;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->工位2有内盒，把工位2的内盒移走");
                                Axes = new List<AxisName>() { AxisName.LotBox_ArmX, AxisName.LotBox_ArmY };
                                var posName = GetPosName(2, PosType.PPLotBoxFromInBoxPos, false);
                                PosNames = new List<PosName>() { posName };
                                MoveAbsM(Axes, PosNames);
                                step = 450;
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
                            ShowMsg($"{StationName}->ArmX轴和ArmY轴到达{lotBoxType}内盒所在位置，ArmZ轴上升到{lotBoxType}内盒下方");
                            MoveAbs(AxisName.LotBox_ArmZ, GetPosName(LotBoxStnNo, PosType.PPLotBoxFromInBoxPos, false));
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
                        var ret = IsAxisArrive(AxisName.LotBox_ArmZ, GetPosName(LotBoxStnNo, PosType.PPLotBoxFromInBoxPos, false));
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴到达{lotBoxType}内盒下方，固定气缸松开");
                            Cylinder(GetCyl(LotBoxStnNo), false);
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
                        var ret = IsCylinderArrive(GetCyl(LotBoxStnNo), false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->固定气缸松开完成，ArmZ轴下降到待机位");
                            MoveAbs(AxisName.LotBox_ArmZ, PosName.WaitPos);
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
                        var ret = IsAxisArrive(AxisName.LotBox_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmX轴和ArmY轴移动到工位2位置");
                            Axes = new List<AxisName>() { AxisName.LotBox_ArmX, AxisName.LotBox_ArmY };
                            var posName = GetPosName(2, PosType.PPLotBoxFromInBoxPos, false);
                            PosNames = new List<PosName>() { posName };
                            MoveAbsM(Axes, PosNames);
                            Cylinder(CylinderName.LotBox2FixLotCyl, false);
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
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmX轴和ArmY轴移动到工位2位置，ArmZ轴下降到工位2内盒上方");
                            Axes = new List<AxisName>() { AxisName.LotBox_ArmZ };
                            var posName = GetPosName(2, PosType.PPLotBoxFromInBoxPos, false);
                            PosNames = new List<PosName>() { posName };
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
                            ShowMsg($"{StationName}->ArmZ轴下降到工位2内盒上方，固定气缸伸出");
                            Cylinder(CylinderName.LotBox2FixLotCyl, true);
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
                        var ret = IsCylinderArrive(CylinderName.LotBox2FixLotCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->固定气缸伸出完成，ArmZ轴下降到待机位");
                            MoveAbs(AxisName.LotBox_ArmZ, PosName.WaitPos);
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
                        var ret = IsAxisArrive(AxisName.LotBox_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->内盒移动到工位2位置流程结束", MsgType.Notify);
                            step = 0;
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
                            ShowMsg($"{StationName}->ArmX轴和ArmY轴移动到工位2位置，ArmZ轴上升到工位2内盒下方");
                            var posName = GetPosName(2, PosType.PPLotBoxFromInBoxPos, false);
                            Axes = new List<AxisName>() { AxisName.LotBox_ArmZ };
                            PosNames = new List<PosName>() { posName };
                            MoveAbsM(Axes, PosNames);
                            step = 500;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 50;
                        }
                        break;
                    }
                case 500:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴到达工位2内盒下方，固定气缸缩回");
                            Cylinder(CylinderName.LotBox2FixLotCyl, false);
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
                        var ret = IsCylinderArrive(CylinderName.LotBox2FixLotCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->固定气缸缩回完成，ArmZ轴下降到待机位");
                            MoveAbs(AxisName.LotBox_ArmZ, PosName.WaitPos);
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
                        var ret = IsAxisArrive(AxisName.LotBox_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴到达待机位，获取空位工位号");
                            NullStnNo = RotateInfoHelper.Instance.GetStnNo(LotBoxType.Null.ToInt(), RotateStnType.LotBox);
                            var posName = GetPosName(NullStnNo, PosType.PPLotBoxFromInBoxPos, false);
                            Axes = new List<AxisName>() { AxisName.LotBox_ArmX, AxisName.LotBox_ArmY };
                            PosNames = new List<PosName>() { posName };
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
                            ShowMsg($"{StationName}->ArmX轴和ArmY轴移动到空位工位上方，ArmZ轴上升到空位工位下方");
                            MoveAbs(AxisName.LotBox_ArmZ, GetPosName(NullStnNo, PosType.PPLotBoxFromInBoxPos, false));
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
                        var ret = IsAxisArrive(AxisName.LotBox_ArmZ, GetPosName(NullStnNo, PosType.PPLotBoxFromInBoxPos, false));
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴下降到空位工位上方，固定气缸伸出");
                            Cylinder(GetCyl(NullStnNo), true);
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
                        var ret = IsCylinderArrive(GetCyl(NullStnNo), true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->固定气缸伸出完成，ArmZ轴下降到待机位");
                            MoveAbs(AxisName.LotBox_ArmZ, PosName.WaitPos);
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
                        var ret = IsAxisArrive(AxisName.LotBox_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴下降到待机位，模组移动到工位{LotBoxStnNo + 1}取内盒位置");
                            var posName = GetPosName(LotBoxStnNo, PosType.PPLotBoxFromInBoxPos, false);
                            Axes = new List<AxisName>() { AxisName.LotBox_ArmX, AxisName.LotBox_ArmY };
                            PosNames = new List<PosName>() { posName };
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
                            ShowMsg($"{StationName}->ArmZ轴上升到工位{LotBoxStnNo + 1}取内盒位置");
                            var posName = GetPosName(LotBoxStnNo, PosType.PPLotBoxFromInBoxPos, false);
                            Axes = new List<AxisName>() { AxisName.LotBox_ArmZ };
                            PosNames = new List<PosName>() { posName };
                            MoveAbsM(Axes, PosNames);
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
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴下降到工位{LotBoxStnNo + 1}取内盒位置，固定气缸松开");
                            Cylinder(GetCyl(LotBoxStnNo), false);
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
                        var ret = IsCylinderArrive(GetCyl(LotBoxStnNo), false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->固定气缸松开完成，ArmZ轴下降到待机位");
                            MoveAbs(AxisName.LotBox_ArmZ, PosName.WaitPos);
                            step = 250;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 900;
                        }
                        break;
                    }

                default:
                    break;
            }
            return step;
        }
        /// <summary>
        /// 请求天车上内盒流程
        /// </summary>
        /// <param name="step"></param>
        /// <param name="isFullTrayLot">true：满Tray Lot内盒。false：空Lot内盒</param>
        /// <returns></returns>
        public int RequestOHTLoadLotBoxFlow(int step, bool isFullTrayLot)
        {
            try
            {
                switch (step)
                {
                    case 1:
                        {
                            ShowMsg($"{StationName}->请求天车上内盒流程开始，判断外盒1平台是否有内盒", MsgType.Notify);
                            if (!Motion.GetDigitalInput(InputName.ExBox1IsExist1) &&
                                !Motion.GetDigitalInput(InputName.ExBox1IsExist2))
                            {
                                ShowMsg($"{StationName}->外盒1平台没有内盒，请求天车上外盒");
                                ExBoxStnNo = 0;
                                Motion.SetDigitalOutput(OutputName.ExBoxStn1LoadReq, true);

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.ExBoxStn1LoadDone.ToString(), out int _).IsShield = true;
                                }
                                step = 150;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->外盒1平台有内盒，判断外盒2平台是否有内盒");
                                if (!Motion.GetDigitalInput(InputName.ExBox2IsExist1) &&
                                !Motion.GetDigitalInput(InputName.ExBox2IsExist2))
                                {
                                    ShowMsg($"{StationName}->外盒2平台没有内盒，请求天车上外盒");
                                    ExBoxStnNo = 1;
                                    Motion.SetDigitalOutput(OutputName.ExBoxStn2LoadReq, true);
                                    step = 150;

                                    if (Global.RunMode == RunMode.空跑模式)
                                    {
                                        IOInfoHelper.GetInputInfoByName(InputName.ExBoxStn2LoadDone.ToString(), out int _).IsShield = true;
                                    }
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->外盒1和外盒2平台都有内盒，无法请求天车上内盒", MsgType.Error, true);
                                    AlarmManager.CreateAlarm(AlarmCode.BothExBoxStnExistBox.ToInt(), AllStationAlarm);
                                    ExBoxStnNo = -1;
                                }
                            }
                            break;
                        }
                    case 150:
                        {
                            if (Motion.GetDigitalInput(InputName.ExBoxStn1LoadDone))
                            {
                                ShowMsg($"{StationName}->天车上内盒到外盒1平台完成", MsgType.Notify);
                                Motion.SetDigitalOutput(OutputName.ExBoxStn1LoadReq, false);
                                //读取Rfid
                                var lotCode = DateTime.Now.ToString("yyyyMMddHHmmss");
                                ProcInfoHelper.Instance.LoadLotBox(lotCode, LotBoxStnType.ExBox1Stn);
                                step = 0;
                            }
                            else if (Motion.GetDigitalInput(InputName.ExBoxStn2LoadDone))
                            {
                                ShowMsg($"{StationName}->天车上内盒到外盒2平台完成", MsgType.Notify);
                                Motion.SetDigitalOutput(OutputName.ExBoxStn2LoadReq, false);
                                //读取Rfid
                                var lotCode = DateTime.Now.ToString("yyyyMMddHHmmss");
                                ProcInfoHelper.Instance.LoadLotBox(lotCode, LotBoxStnType.ExBox2Stn);
                                step = 0;
                            }
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger($"[LotBoxStn][RequestOHTLoadLotBoxFlow]方法执行异常，异常原因：{ex}");
                AlarmManager.CreateAlarm(AlarmCode.ProgramExceptionStart.ToInt(), AllStationAlarm);
                ExBoxStnNo = -1;
            }
            return step;
        }
        /// <summary>
        /// 请求天车上内盒流程
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int RequestOHTUnloadLotBoxFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->请求天车取内盒流程开始，判断外盒1平台是否有内盒", MsgType.Notify);
                        if (Motion.GetDigitalInput(InputName.ExBox1IsExist1) ||
                            Motion.GetDigitalInput(InputName.ExBox1IsExist2))
                        {
                            ShowMsg($"{StationName}->外盒1平台有内盒，请求天车从外盒1工位取内盒");
                            Motion.SetDigitalOutput(OutputName.ExBoxStn1UnloadReq, true);
                            Delay.Start(ParaHelper.Global.OHTLulTimeout);
                            step = 50;
                        }
                        else
                        {
                            ShowMsg($"{StationName}->外盒1平台没有内盒，判断外盒2平台是否有内盒");
                            if (Motion.GetDigitalInput(InputName.ExBox2IsExist1) ||
                                Motion.GetDigitalInput(InputName.ExBox2IsExist2))
                            {
                                ShowMsg($"{StationName}->外盒2平台有内盒，请求天车从外盒2工位取内盒");
                                Motion.SetDigitalOutput(OutputName.ExBoxStn2UnloadReq, true);
                                Delay.Start(ParaHelper.Global.OHTLulTimeout);
                                step = 100;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->外盒1和外盒2平台都没有内盒，无法请求天车下内盒", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.BothExBoxStnNoExistBox.ToInt(), AllStationAlarm);
                            }
                        }
                        break;
                    }
                case 50:
                    {
                        if (Motion.GetDigitalInput(InputName.ExBoxStn1UnloadDone))
                        {
                            Motion.SetDigitalOutput(OutputName.ExBoxStn1UnloadReq, false);
                            ShowMsg($"{StationName}->天车从外盒1工位取内盒完成");
                            ProcInfoHelper.Instance.PickLotBox(LotBoxStnType.ExBox1Stn);
                            ShowMsg($"{StationName}->天车取内盒流程结束", MsgType.Notify);
                            step = 0;
                        }
                        else if (Delay.TimeUp())
                        {
                            AlarmManager.CreateAlarm(AlarmCode.OHTLoadLotBoxTimeout.ToInt(), StationNum);
                        }
                        break;
                    }
                case 100:
                    {
                        if (Motion.GetDigitalInput(InputName.ExBoxStn2UnloadDone))
                        {
                            Motion.SetDigitalOutput(OutputName.ExBoxStn2UnloadReq, false);
                            ShowMsg($"{StationName}->天车从外盒2工位取内盒完成");
                            ProcInfoHelper.Instance.PickLotBox(LotBoxStnType.ExBox2Stn);
                            ShowMsg($"{StationName}->天车取内盒流程结束", MsgType.Notify);
                            step = 0;
                        }
                        else if (Delay.TimeUp())
                        {
                            AlarmManager.CreateAlarm(AlarmCode.OHTLoadLotBoxTimeout.ToInt(), StationNum);
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
                        ShowMsg($"{StationName}->移动到待机位流程开始");
                        MoveAbs(AxisName.LotBox_ArmZ, PosName.WaitPos);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArrive(AxisName.LotBox_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            Axes = new List<AxisName>() { AxisName.LotBox_ArmX, AxisName.LotBox_ArmY };
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
                            ShowMsg($"{StationName}->移动到待机位流程结束");
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
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
        /// 从内盒平台搬运LotBox到外盒平台流程
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int LotBoxFromInnerBoxStageToExBoxStageFlow(int step, LotBoxType lotBoxType, int exBoxNo)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->Lot内盒从内盒平台搬运到外盒平台流程开始，判断外盒{exBoxNo + 1}位置是否有内盒", MsgType.Notify);
                        if (!Motion.GetDigitalInput($"ExBox{exBoxNo + 1}IsExist1") &&
                            !Motion.GetDigitalInput($"ExBox{exBoxNo + 1}IsExist2"))
                        {
                            ShowMsg($"{StationName}->外盒{exBoxNo + 1}位置没有内盒，ArmZ轴移动到待机位", MsgType.Notify);
                            MoveAbs(AxisName.LotBox_ArmZ, PosName.WaitPos);
                            step = 50;
                        }
                        else
                        {
                            ShowMsg($"{StationName}->外盒{exBoxNo + 1}位置已经有内盒，无法把内盒放入", MsgType.Error, true);
                            AlarmManager.CreateAlarm(AlarmCode.ExBoxStn1ExistBox.ToInt() + exBoxNo, StationNum);
                        }
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArrive(AxisName.LotBox_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            LotBoxStnNo = RotateInfoHelper.Instance.GetStnNo(lotBoxType.ToInt(), RotateStnType.LotBox);
                            ShowMsg($"{StationName}->获取{lotBoxType}内盒所在工位号：{LotBoxStnNo}");
                            var posName = GetPosName(LotBoxStnNo, PosType.PPLotBoxFromInBoxPos, false);
                            Axes = new List<AxisName>() { AxisName.LotBox_ArmX, AxisName.LotBox_ArmY };
                            PosNames = new List<PosName>() { posName };
                            ShowMsg($"{StationName}->ArmX轴和ArmY轴移动到{lotBoxType}内盒所在位置");
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
                            ShowMsg($"{StationName}->ArmX轴和ArmY轴到达{lotBoxType}内盒所在位置，ArmZ轴上升到取放Lot位置");
                            Axes = new List<AxisName>() { AxisName.LotBox_ArmZ };
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
                            ShowMsg($"{StationName}->ArmZ轴到达取放Lot位置，固定气缸缩回");
                            Cylinder(GetCyl(LotBoxStnNo), false);
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
                        var ret = IsCylinderArrive(GetCyl(LotBoxStnNo), false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->固定气缸缩回完成，ArmZ轴下降到待机位");
                            MoveAbs(AxisName.LotBox_ArmZ, PosName.WaitPos);
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
                        var ret = IsAxisArrive(AxisName.LotBox_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴下降到待机位完成，ArmX轴和ArmY轴移动到预从外盒{exBoxNo + 1}取放LotBox位置");
                            Axes = new List<AxisName>() { AxisName.LotBox_ArmX, AxisName.LotBox_ArmY };
                            var posName = GetPosName(exBoxNo, PosType.PrePPLotBoxFromExBoxPos, true);
                            PosNames = new List<PosName>() { posName };
                            MoveAbsM(Axes, PosNames);
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
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmX轴和ArmY轴移动到外盒{exBoxNo + 1}待机位置完成，ArmZ轴上升到从外盒{exBoxNo + 1}取放LotBox位置");
                            Axes = new List<AxisName>() { AxisName.LotBox_ArmZ };
                            var posName = GetPosName(exBoxNo, PosType.PPLotBoxFromExBoxPos, true);
                            PosNames = new List<PosName>() { posName };
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
                            ShowMsg($"{StationName}->ArmZ轴上升到从外盒{exBoxNo + 1}取放LotBox位置，XY轴移动到取放LotBox位置");
                            Axes = new List<AxisName>() { AxisName.LotBox_ArmX, AxisName.LotBox_ArmY };
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
                            ShowMsg($"{StationName}->XY轴到达取放LotBox位置，ArmZ轴慢速下降到预取放LotBox位置");
                            Axes = new List<AxisName>() { AxisName.LotBox_ArmZ };
                            var posName = GetPosName(exBoxNo, PosType.PrePPLotBoxFromExBoxPos, true);
                            PosNames = new List<PosName>() { posName };
                            MoveAbsM(Axes, PosNames);
                            step = 430;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 350;
                        }
                        break;
                    }
                case 430:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴到达预取放LotBox位置，ArmZ轴下降到待机位置");
                            Axes = new List<AxisName>() { AxisName.LotBox_ArmZ };
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
                        var ret = IsAxisArrive(AxisName.LotBox_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴下降到待机位完成，ArmX轴和ArmY轴移动到待机位");
                            Axes = new List<AxisName>() { AxisName.LotBox_ArmX, AxisName.LotBox_ArmY };
                            PosNames = new List<PosName>() { PosName.WaitPos };
                            MoveAbsM(Axes, PosNames);
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
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmX轴和ArmY轴移动到待机位完成。Lot内盒从内盒平台搬运到外盒平台流程结束", MsgType.Notify);
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 500;
                        }
                        break;
                    }
                default:
                    break;
            }
            return step;
        }
        /// <summary>
        /// 从外盒平台搬运LotBox到内盒平台流程
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int LotBoxFromExBoxStageToInnerBoxStageFlow(int step, LotBoxType lotBoxType, int exBoxNo)
        {
            try
            {
                switch (step)
                {
                    case 1:
                        {
                            if (Global.RunMode == RunMode.空跑模式)
                            {
                                IOInfoHelper.GetInputInfoByName($"ExBox{exBoxNo + 1}IsExist1", out int _).IsShield = true;
                                IOInfoHelper.GetInputInfoByName($"ExBox{exBoxNo + 1}IsExist2", out int _).IsShield = true;
                            }
                            ShowMsg($"{StationName}->Lot内盒从外盒平台搬运到内盒平台流程开始，判断外盒{exBoxNo + 1}位置是否有内盒", MsgType.Notify);
                            if (Motion.GetDigitalInput($"ExBox{exBoxNo + 1}IsExist1") || Motion.GetDigitalInput($"ExBox{exBoxNo + 1}IsExist2"))
                            {
                                ShowMsg($"{StationName}->外盒{exBoxNo + 1}位置有内盒，ArmZ轴移动到待机位");
                                MoveAbs(AxisName.LotBox_ArmZ, PosName.WaitPos);
                                step = 50;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->外盒{exBoxNo + 1}位置没有内盒，无法执行上内盒流程", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.ExBoxStn1NoExistBox.ToInt() + exBoxNo, StationNum);
                            }
                            break;
                        }
                    case 50:
                        {
                            var ret = IsAxisArrive(AxisName.LotBox_ArmZ, PosName.WaitPos);
                            if (ret == MotionStatus.Arrived)
                            {
                                var posName = GetPosName(exBoxNo, PosType.PrePPLotBoxFromExBoxPos, true);
                                Axes = new List<AxisName>() { AxisName.LotBox_ArmX, AxisName.LotBox_ArmY };
                                PosNames = new List<PosName>() { posName };
                                ShowMsg($"{StationName}->ArmX轴和ArmY轴移动到外盒{exBoxNo + 1} PrePPLot位置");
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
                                ShowMsg($"{StationName}->ArmX轴和ArmY轴到达外盒{exBoxNo + 1} PrePPLot位置，继续移动到外盒{exBoxNo + 1} PPLot位置");
                                var posName = GetPosName(exBoxNo, PosType.PPLotBoxFromExBoxPos, true);
                                PosNames = new List<PosName>() { posName };
                                MoveAbsM(Axes, PosNames);
                                step = 110;
                            }
                            else if (ret == MotionStatus.Timeout)
                            {
                                step = 50;
                            }
                            break;
                        }
                    case 110:
                        {
                            var ret = IsAxisArriveM(Axes, PosNames);
                            if (ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->ArmX轴和ArmY轴到达外盒{exBoxNo + 1}位置，ArmZ轴快速上升到变速位置");
                                Axes = new List<AxisName>() { AxisName.LotBox_ArmZ };
                                var posName = GetPosName(exBoxNo, PosType.PrePPLotBoxFromExBoxPos, true);
                                PosNames = new List<PosName>() { posName };
                                MoveAbsM(Axes, PosNames);
                                step = 130;
                            }
                            else if (ret == MotionStatus.Timeout)
                            {
                                step = 50;
                            }
                            break;
                        }
                    case 130:
                        {
                            var ret = IsAxisArriveM(Axes, PosNames);
                            if (ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->ArmZ轴到达变速位置，ArmZ轴慢速上升到顶起LotBox位置");
                                Axes = new List<AxisName>() { AxisName.LotBox_ArmZ };
                                var posName = GetPosName(exBoxNo, PosType.PPLotBoxFromExBoxPos, true);
                                PosNames = new List<PosName>() { posName };
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
                                Axes = new List<AxisName>() { AxisName.LotBox_ArmX, AxisName.LotBox_ArmY };
                                var posName = GetPosName(exBoxNo, PosType.PrePPLotBoxFromExBoxPos, true);
                                PosNames = new List<PosName>() { posName };
                                MoveAbsM(Axes, PosNames);
                                ShowMsg($"{StationName}->ArmZ轴到达顶起外盒位置，XY轴移动到把LotBox取出外盒平台位置");
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
                                ShowMsg($"{StationName}->XY轴到达预取放LotBox位置，ArmZ轴下降到待机位");
                                MoveAbs(AxisName.LotBox_ArmZ, PosName.WaitPos);
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
                            var ret = IsAxisArrive(AxisName.LotBox_ArmZ, PosName.WaitPos);
                            if (ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->ArmZ轴下降到待机位完成，ArmX轴和ArmY轴移动到{lotBoxType}内盒所在位置");
                                LotBoxStnNo = RotateInfoHelper.Instance.GetStnNo(lotBoxType.ToInt(), RotateStnType.LotBox);
                                ShowMsg($"{StationName}->获取{lotBoxType}内盒所在工位号：{LotBoxStnNo}");
                                var posName = GetPosName(LotBoxStnNo, PosType.PPLotBoxFromInBoxPos, false);
                                Axes = new List<AxisName>() { AxisName.LotBox_ArmX, AxisName.LotBox_ArmY };
                                PosNames = new List<PosName>() { posName };
                                MoveAbsM(Axes, PosNames);
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
                            var ret = IsAxisArriveM(Axes, PosNames);
                            if (ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->ArmX轴和ArmY轴到达{lotBoxType}内盒所在位置，ArmZ轴上升到取放内盒位置");
                                Axes = new List<AxisName>() { AxisName.LotBox_ArmZ };
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
                                ShowMsg($"{StationName}->ArmZ轴到达取放内盒位置，固定气缸伸出");
                                Cylinder(GetCyl(LotBoxStnNo), true);
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
                            var ret = IsCylinderArrive(GetCyl(LotBoxStnNo), true);
                            if (ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->固定气缸伸出完成，ArmZ轴下降到待机位");
                                MoveAbs(AxisName.LotBox_ArmZ, PosName.WaitPos);
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
                            var ret = IsAxisArrive(AxisName.LotBox_ArmZ, PosName.WaitPos);
                            if (ret == MotionStatus.Arrived)
                            {
                                var model = ProcInfoHelper.Instance.PickLotBox(LotBoxStnType.ExBox1Stn + exBoxNo);
                                LotBoxModelInit(model, lotBoxType);
                                ShowMsg($"{StationName}->ArmZ轴下降到待机位完成，ArmX轴和ArmY轴移动到待机位");
                                Axes = new List<AxisName>() { AxisName.LotBox_ArmX, AxisName.LotBox_ArmY };
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
                                ShowMsg($"{StationName}->ArmX轴和ArmY轴移动到待机位完成。Lot内盒从外盒平台搬运到内盒平台流程结束", MsgType.Notify);
                                step = 0;

                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName($"ExBox{exBoxNo + 1}IsExist1", out int _).IsShield = false;
                                    IOInfoHelper.GetInputInfoByName($"ExBox{exBoxNo + 1}IsExist2", out int _).IsShield = false;
                                }
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
            }
            catch (Exception ex)
            {
                ShowMsg($"{StationName}->[LotBoxFromExBoxStageToInnerBoxStageFlow]方法执行异常，异常原因:{ex}", MsgType.Error);
                LogManager.GetLogger("Error").Error($"[LotBoxStn][LotBoxFromExBoxStageToInnerBoxStageFlow]方法执行异常，异常原因:{ex}");
            }
            return step;
        }

        /// <summary>
        /// 获取XY轴移动到内盒或外盒位置的PosName
        /// </summary>
        /// <param name="stnNo"></param>
        /// <param name="isExternalBox"></param>
        /// <returns></returns>
        public PosName GetPosName(int stnNo, PosType posType, bool isExternalBox)
        {
            PosName ret;
            var str = $"LotBoxStn{stnNo + 1}{posType}";
            if (isExternalBox)
            {
                str = $"ExBoxStn{stnNo + 1}{posType}";
            }
            return EnumHelper.GetEnumValue<PosName>(str);
        }
        private CylinderName GetCyl(int stnNo)
        {
            var str = $"LotBox{stnNo + 1}FixLotCyl";

            return EnumHelper.GetEnumValue<CylinderName>(str);
        }
        private void LotBoxModelInit(LotBoxModel lotBoxModel, LotBoxType lotBoxType)
        {
            var prodPara = ParaHelper.Prod;
            lotBoxModel.LotBoxType = lotBoxType;
            lotBoxModel.LotCellArray = new LotCellBase[prodPara.RowCountPerLotBox, prodPara.ColCountPerLotBox];
            for (int i = 0; i < prodPara.RowCountPerLotBox; i++)
            {
                for (int j = 0; j < prodPara.ColCountPerLotBox; j++)
                {
                    if (lotBoxType == LotBoxType.EmptyTray)
                    {
                        var model = new LotCellBase();
                        model.LotCellCode = $"{lotBoxModel.Rfid}_{i}_{j}";
                        model.TrayList = new List<TrayModel>();
                        model.TrayCount = prodPara.TrayCountPerEmptyLot;
                        model.LotBoxType = LotBoxType.EmptyTray;
                        for (int c = 0; c < prodPara.TrayCountPerEmptyLot; c++)
                        {
                            TrayModel trayModel = new TrayModel();
                            trayModel.TrayType = TrayTypeEnum.Empty;
                            trayModel.RowCount = prodPara.TrayRowCount;
                            trayModel.ColCount = prodPara.TrayColCount;
                            //trayModel.TrayCode = $"{model.LotCellCode}_{c}";
                            trayModel.DieArray = new DieModel[prodPara.TrayRowCount, prodPara.TrayColCount];
                            model.TrayList.Add(trayModel);
                        }
                        model.UpdateMap();
                        lotBoxModel.LotCellArray[i, j] = model;
                    }

                }
            }
            lotBoxModel.UpdateMap();
            ProcInfoHelper.Instance.PutLotBox((LotBoxStnType)LotBoxStnNo, lotBoxModel);
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
        }

        public void StopHandle()
        {
        }

        public void WaitFlowDone()
        {
        }
    }
    public enum PosType
    {
        /// <summary>
        /// 从外盒平台中取放内盒位置
        /// </summary>
        PPLotBoxFromExBoxPos,
        /// <summary>
        /// 从外盒平台中预取放内盒位置
        /// </summary>
        PrePPLotBoxFromExBoxPos,
        /// <summary>
        /// 内盒平台取放LotBox位置
        /// </summary>
        PPLotBoxFromInBoxPos,
        /// <summary>
        /// 内盒读码位置
        /// </summary>
        ScanPos,
        /// <summary>
        /// 外盒度Rfid位置
        /// </summary>
        ReadRfidPos
    }

}
