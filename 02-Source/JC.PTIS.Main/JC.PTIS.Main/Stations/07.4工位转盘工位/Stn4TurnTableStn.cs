using APS168_W32;
using JC.PTIS.Main.AlarmInfo;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Params;
using JC.PTIS.Main.RotateInfo;
using JC.PTIS.Main.Service;
using StdLib.PCBaseFrame.Alarm;
using StdLib.PCBaseFrame.Entities;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.PCBaseFrame.Motion;
using StdLib.PCBaseFrame.Station;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Stations
{
    public class Stn4TurnTableStn : StationBaseEx, IStation
    {
        public int StationNum { get; set; } = 7;
        public string StationName { get; set; } = "Stn4TurnTableStn";

        List<AxisName> Axes = new List<AxisName>();
        List<PosName> PosNames = new List<PosName>();//位置名称集合
        List<double> Poses = new List<double>();//位置值集合
        #region 子流程状态机步骤
        int ModuleMoveStep = -1;
        int StageMoveStep = -1;
        #endregion
        double[] UnloadTrayDDPos = new double[4];

        int RotateNo = 0;
        int LastRotateNo = 0;
        List<StageInfo> StageList = new List<StageInfo>();
        Delay Delay = new Delay();
        public void Home()
        {
            switch (HomeStep)
            {
                case 1:
                    {
                        LastRotateNo = 0;
                        RotateNo = 0;
                        ShowMsg($"{StationName}->开始回零，等待Z轴回零完成");
                        HomeStep = 50;
                        break;
                    }
                case 50:
                    {
                        if (Motion.IsHomeDone(AxisName.PickPatch_PickZ) &&
                            Motion.IsHomeDone(AxisName.PickPatch_PlaceZ) &&
                            Motion.IsHomeDone(AxisName.PlaceDie_Z))
                        {
                            ShowMsg($"{StationName}->{AxisName.Stn4TurnTable_DD}轴回零");
                            Motion.Home(AxisName.Stn4TurnTable_DD);
                            HomeStep = 100;
                        }
                        break;
                    }
                case 100:
                    {
                        if (Motion.IsHomeDone(AxisName.Stn4TurnTable_DD))
                        {
                            ShowMsg($"{StationName}->{AxisName.Stn4TurnTable_DD}轴回零完成");
                            Motion.HomeOffset(AxisName.Stn4TurnTable_DD);
                            Delay.Start(ParaHelper.Motion.WaitAxisMoveTimeOut);
                            ShowMsg($"{StationName}->{AxisName.Stn4TurnTable_DD}轴移动到回零偏移位置");
                            HomeStep = 150;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.Stn4TurnTable_DD))
                        {
                            ShowMsg($"{StationName}->Stn4TurnTable_DD轴回零超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 150:
                    {
                        if (Motion.IsHomeOffsetDone(AxisName.Stn4TurnTable_DD))
                        {
                            ShowMsg($"{StationName}->{AxisName.Stn4TurnTable_DD}轴到达回零偏移位置");
                            ShowMsg($"{StationName}->Tray平台回零");
                            RotateInfoHelper.Instance.UpdateRotateInfo(RotateStnType.Stn4TurnTable, 0);
                            StageMoveStep = 1;
                            HomeStep = 200;
                        }
                        else if (Delay.TimeUp())
                        {
                            ShowMsg($"{StationName}->{AxisName.Stn4TurnTable_DD}轴移动到回零偏移位置超时");
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 200:
                    {
                        StageMoveStep = StageHome(StageMoveStep);
                        if (StageMoveStep == 0)
                        {
                            UpdateHomeSts(StationName, StationNum, true);
                            var temp = RedisHelper.Instance.StringGet(RotateStnType.Stn4TurnTable.ToString());
                            LastRotateNo = string.IsNullOrEmpty(temp) ? 0 : int.Parse(temp);
                            HomeStep = 0;
                        }
                        else if (HomeStep == -1)
                        {
                            ShowMsg($"{StationName}->Tray平台回零超时", MsgType.Error);
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
                            ShowMsg($"{StationName}->所有模组移动到等待位");
                            ModuleMoveStep = 1;
                            AutoRunStep = 50;
                            break;
                        }
                    case 50:
                        {
                            ModuleMoveStep = MoveToWaitPosFlow(ModuleMoveStep);
                            if (ModuleMoveStep == 0)
                            {
                                ShowMsg($"{StationName}->等待PickPatch工位、放Die工位和上下Tray工位安全信号");
                                AutoRunStep = 100;
                            }
                            break;
                        }
                    case 100:
                        {
                            if (GetBoolReg(RegisterEnum.PickPatchStnSafe) &&
                                GetBoolReg(RegisterEnum.TrayLuloadDone) &&
                                GetBoolReg(RegisterEnum.DieToTrayDone) &&
                                Motion.IsArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos))
                            {
                                ResetBoolReg(RegisterEnum.PickPatchStnSafe);
                                ResetBoolReg(RegisterEnum.TrayLuloadDone);
                                ResetBoolReg(RegisterEnum.DieToTrayDone);
                                ResetBoolReg(RegisterEnum.Stn4DDRotateDone);
                                ShowMsg($"{StationName}->收到PickPatch工位和放Die工位安全信号，判断转盘是否可以转动");
                                AutoRunStep = 150;
                            }
                            break;
                        }
                    case 150:
                        {
                            if (IsDDCanRotate())
                            {
                                ShowMsg($"{StationName}->转盘转动到{UnloadTrayDDPos[RotateNo]}度位置");
                                MoveAbs(AxisName.Stn4TurnTable_DD, UnloadTrayDDPos[RotateNo]);
                                Axes = new List<AxisName>() { AxisName.TrayStage1T, AxisName.TrayStage2T, AxisName.TrayStage3T, AxisName.TrayStage4T };
                                Poses = new List<double>() { -90 };
                                MoveRelM(Axes, Poses);
                                AutoRunStep = 200;
                            }
                            break;
                        }
                    case 200:
                        {
                            var ret = IsDDArrive();
                            var ret2 = IsAxisArriveM(Axes);
                            if (ret == MotionStatus.Arrived &&
                                ret2 == MotionStatus.Arrived)
                            {
                                Axes.ForEach(p =>
                                {
                                    if (Motion.IsArrive(p, 360.0, 0.2))
                                    {
                                        Motion.ZeroPos(p);
                                    }
                                });
                                ShowMsg($"{StationName}->转盘到达{UnloadTrayDDPos[RotateNo]}度位置，通知取Die工位和放Die工位转盘转动完成");
                                RegisterService.Instance.SetBoolReg(RegisterEnum.Stn4DDRotateDone);
                                LastRotateNo = RotateNo;
                                RedisHelper.Instance.StringSet(RotateStnType.Stn4TurnTable.ToString(), LastRotateNo.ToString());
                                RotateNo++;
                                if (RotateNo == 4)
                                {
                                    RotateNo = 0;
                                }
                                AutoRunStep = 100;
                            }
                            else if (ret == MotionStatus.Timeout)
                            {
                                AutoRunStep = 150;
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
                        RotateNo = LastRotateNo;
                        Axes = new List<AxisName>() { AxisName.PickPatch_PickZ, AxisName.PickPatch_PlaceZ, AxisName.PlaceDie_Z, AxisName.Stn8TurnTable_ArmZ };
                        PosNames = new List<PosName>() { PosName.WaitPos };
                        ShowMsg($"{StationName}->等待Z轴回待机位完成");
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->Z轴到达待机位");
                            MoveAbs(AxisName.Stn4TurnTable_DD, UnloadTrayDDPos[RotateNo]);
                            step = 100;
                        }
                        break;
                    }
                case 100:
                    {
                        var ret = IsDDArrive();
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->Tray平台回待机位");
                            StageList.ForEach(item =>
                            {
                                Motion.MoveAbs(item.X, PosName.WaitPos);
                                Motion.MoveAbs(item.Y, PosName.WaitPos);
                                Motion.MoveAbs(item.T, PosName.WaitPos);
                            });
                            Delay.Start(20);
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
                        bool isAllArrive = true;
                        foreach (var item in StageList)
                        {
                            isAllArrive &= Motion.IsArrive(item.X, PosName.WaitPos) &&
                                Motion.IsArrive(item.Y, PosName.WaitPos) &&
                                Motion.IsArrive(item.T, PosName.WaitPos);

                            if (Motion.IsMoveTimeOut(item.X) ||
                                Motion.IsMoveTimeOut(item.Y) ||
                                Motion.IsMoveTimeOut(item.T))
                            {
                                isAllArrive = false;
                                break;
                            }
                        }
                        if (isAllArrive)
                        {
                            ShowMsg($"{StationName}->Tray平台回待机位完成");
                            step = 0;
                        }
                        else if (Delay.TimeUp())
                        {
                            ShowMsg($"{StationName}->Tray平台回待机位超时", MsgType.Error, true);
                            PauseRequest(StationNum);
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
        /// XYT平台回零
        /// </summary>
        /// <param name="step">回零流程步</param>
        /// <returns></returns>
        public int StageHome(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->所有Tray平台XY轴开始回零");
                        StageList.ForEach(item =>
                        {
                            Motion.Home(item.X);
                            Motion.Home(item.Y);
                            ShowMsg($"{StationName}->{item.X}轴、{item.Y}轴开始回零");
                        });
                        step = 50;
                        Delay.Start(30);
                        break;
                    }
                case 50:
                    {
                        bool isAllHomeDone = true;
                        foreach (var item in StageList)
                        {
                            isAllHomeDone &= Motion.IsHomeDone(item.X) && Motion.IsHomeDone(item.Y);
                            if (Motion.IsMoveTimeOut(item.X) ||
                                Motion.IsMoveTimeOut(item.Y))
                            {
                                isAllHomeDone = false;
                                break;
                            }
                        }
                        if (isAllHomeDone)
                        {
                            ShowMsg($"{StationName}->所有Tray平台XY轴回零完成");
                            StageList.ForEach(item =>
                            {
                                MoveAbs(item.X, PosName.WaitPos);
                                MoveAbs(item.Y, PosName.WaitPos);
                                ShowMsg($"{StationName}->{item.X}轴、{item.Y}轴移动到待机位");
                            });
                            Delay.Start(30);
                            step = 100;
                        }
                        else if (Delay.TimeUp())
                        {
                            step = -1;
                            ShowMsg($"{StationName}->Tray平台回零超时", MsgType.Error);
                        }
                        break;
                    }
                case 100:
                    {
                        bool isAllMoveDone = true;
                        foreach (var item in StageList)
                        {
                            isAllMoveDone &= Motion.IsArrive(item.X, PosName.WaitPos) && Motion.IsArrive(item.Y, PosName.WaitPos);
                            if (Motion.IsMoveTimeOut(item.X) ||
                                    Motion.IsMoveTimeOut(item.Y))
                            {
                                isAllMoveDone = false;
                                break;
                            }
                        }

                        if (isAllMoveDone)
                        {
                            ShowMsg($"{StationName}->所有平台XY轴到达待机位");
                            step = 150;
                        }
                        else if (Delay.TimeUp())
                        {
                            step = -1;
                            ShowMsg($"{StationName}->Tray平台XY轴移动到待机位超时", MsgType.Error);
                        }
                        break;
                    }
                case 150:
                    {
                        ShowMsg($"{StationName}->所有Tray平台T轴开始回零");
                        StageList.ForEach(item =>
                        {
                            if (Global.RunMode == RunMode.空跑模式)
                            {
                                AxisInfoHelper.GetAxisInfoByName(item.T).IsHomeOK = true;
                                Motion.ZeroPos(item.T);
                            }
                            else
                            {
                                Motion.Home(item.T);
                                ShowMsg($"{StationName}->{item.T}轴开始回零");
                            }
                        });
                        step = 200;
                        Delay.Start(30);
                        break;
                    }
                case 200:
                    {
                        bool isAllHomeDone = true;
                        foreach (var item in StageList)
                        {
                            isAllHomeDone &= Motion.IsHomeDone(item.T);
                            if (Motion.IsMoveTimeOut(item.T))
                            {
                                isAllHomeDone = false;
                                break;
                            }
                        }
                        if (isAllHomeDone)
                        {
                            ShowMsg($"{StationName}->所有Tray平台T轴回零完成");
                            StageList.ForEach(item =>
                            {
                                MoveAbs(item.T, PosName.WaitPos);
                                ShowMsg($"{StationName}->{item.T}轴移动到待机位");
                            });
                            Delay.Start(30);
                            step = 250;
                        }
                        else if (Delay.TimeUp())
                        {
                            step = -1;
                            ShowMsg($"{StationName}->Tray平台T轴回零超时", MsgType.Error);
                        }
                        break;
                    }
                case 250:
                    {
                        bool isAllMoveDone = true;
                        foreach (var item in StageList)
                        {
                            isAllMoveDone &= Motion.IsArrive(item.T, PosName.WaitPos);
                            if (Motion.IsMoveTimeOut(item.T))
                            {
                                isAllMoveDone = false;
                                break;
                            }
                        }

                        if (isAllMoveDone)
                        {
                            ShowMsg($"{StationName}->所有平台T轴到达待机位");
                            step = 0;
                        }
                        else if (Delay.TimeUp())
                        {
                            step = -1;
                            ShowMsg($"{StationName}->Tray平台T轴移动到待机位超时", MsgType.Error);
                        }
                        break;
                    }
                default:
                    break;
            }
            return step;
        }
        private bool IsDDCanRotate()
        {
            if (Motion.IsArrive(AxisName.PickDie_Z, PosName.WaitPos) &&
                Motion.IsArrive(AxisName.PickPatch_PickZ, PosName.WaitPos) &&
                Motion.IsArrive(AxisName.PickPatch_PlaceZ, PosName.WaitPos) &&
                Motion.IsArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos))
            {
                return true;
            }

            return false;
        }
        private MotionStatus IsDDArrive()
        {
            var ret = IsAxisArrive(AxisName.Stn4TurnTable_DD, UnloadTrayDDPos[RotateNo]);
            if (ret == MotionStatus.Arrived)
            {
                if (UnloadTrayDDPos[RotateNo] == 360)
                {
                    Motion.ZeroPos(AxisName.Stn4TurnTable_DD);
                }
                RotateInfoHelper.Instance.UpdateRotateInfo(RotateStnType.Stn4TurnTable, UnloadTrayDDPos[RotateNo]);
            }
            return ret;
        }
        public void Init()
        {
            for (int i = 0; i < 4; i++)
            {
                UnloadTrayDDPos[i] = 90 * i;
            }
            StageList = StageManager.GetList();
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
            ModuleMoveStep = -1;

        }

        public void StopHandle()
        {
        }

        public void WaitFlowDone()
        {
        }
    }
}
