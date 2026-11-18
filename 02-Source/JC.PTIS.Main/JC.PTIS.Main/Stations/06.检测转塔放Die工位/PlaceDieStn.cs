using JC.AutoMating.ConfigInfo.StepTimeInfo;
using JC.PTIS.Main.AlarmInfo;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Params;
using JC.PTIS.Main.RotateInfo;
using JC.PTIS.Main.Service;
using StdLib.PCBaseFrame.Alarm;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.PCBaseFrame.Extension.Entities;
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
    public class PlaceDieStn : StationBaseEx, IStation
    {
        public int StationNum { get; set; } = 6;
        public string StationName { get; set; } = "PlaceDieStn";

        List<AxisName> Axes = new List<AxisName>();
        List<PosName> PosNames = new List<PosName>();//位置名称集合
        List<double> Poses = new List<double>();//实际位置集合
        double PlaceZPos = 0;
        #region 子流程状态机步骤
        int SubFlowStep = -1;
        #endregion
        int CurPickNo = -1;
        public void Home()
        {
            switch (HomeStep)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->开始回零");
                        ShowMsg($"{StationName}->{AxisName.PlaceDie_Z}轴开始回零");
                        Motion.Home(AxisName.PlaceDie_Z);
                        HomeStep = 50;
                        break;
                    }
                case 50:
                    {
                        if (Motion.IsHomeDone(AxisName.PlaceDie_Z))
                        {
                            ShowMsg($"{StationName}->回零完成");
                            UpdateHomeSts(StationName, StationNum, true);
                            HomeStep = 0;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.PlaceDie_Z))
                        {
                            ShowMsg($"{StationName}->PlaceDie_Z轴回零超时", MsgType.Error);
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
                                ShowMsg($"{StationName}->模组到达待机位，执行放Die流程", MsgType.Notify);
                                SubFlowStep = 1;
                                AutoRunStep = 100;
                            }
                            break;
                        }
                    case 100:
                        {
                            SubFlowStep = PlaceDieFlow(SubFlowStep);
                            if (SubFlowStep == -1)
                            {
                                ShowMsg($"{StationName}->放Die流程错误", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.PlaceDieFlowError.ToInt(), StationNum);
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
                        Axes = new List<AxisName>() { AxisName.PlaceDie_Z };
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

        public int PlaceDieFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        RegisterService.Instance.SetBoolReg(RegisterEnum.PlaceDieStnSafe);
                        ShowMsg($"{StationName}->通知LoadDie转盘可以转动，等待LoadDie转盘转动到位");
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        if (GetBoolReg(RegisterEnum.LoadDDRotateDone, true))
                        {
                            ShowMsg($"{StationName}->LoadDie转盘转动到位，判断当前吸嘴上是否有Die");
                            CurPickNo = RotateInfoHelper.Instance.GetStageNo(Stn16TurnTableStnEnum.PlaceDie.ToInt(), RotateStnType.LoadDie);
                            if (IsNozzleVacArrive(CurPickNo) == false)
                            {
                                ShowMsg($"{StationName}->吸嘴：{CurPickNo}没有Die");
                                step = 1;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->吸嘴：{CurPickNo}有Die，判断IR检测结果");
                                var pickDieInfo = ProcInfoHelper.Instance.GetPickerInfo(Stn16TurnTableStnEnum.PlaceDie, RotateStnType.LoadDie);
                                if (pickDieInfo != null && pickDieInfo.DieModel != null)
                                {
                                    if (pickDieInfo.DieModel.DieStatus != DieStatus.IRNG)
                                    {
                                        ShowMsg($"{StationName}->吸嘴：{CurPickNo}上Die IR检测结果：OK，等待DieToTray工位允许放Die");
                                        RegisterService.Instance.SetBoolReg(RegisterEnum.PlaceDieStnPlaceDieReq);
                                        StepTimeInfoHelper.GetInfoByName(StepTimeEnum.PlaceDie.ToString())?.Start();
                                        step = 100;
                                    }
                                    else
                                    {
                                        ShowMsg($"{StationName}->吸嘴：{CurPickNo}上Die IR检测结果：NG，不执行放Die", MsgType.Error);
                                        step = 1;
                                    }
                                }
                                else
                                {
                                    //AlarmManager.CreateAlarm(AlarmCode.GetDieModelFromLoadDDPickerFail.ToInt(), StationNum);
                                    ShowMsg($"{StationName}->吸嘴：{CurPickNo}上的Die信息为空,跳过", MsgType.Error);
                                    step = 1;
                                }
                            }
                        }
                        break;
                    }
                case 100:
                    {
                        if (GetBoolReg(RegisterEnum.DieToTrayStnAllowPlaceDie))
                        {
                            var trayInfo = ProcInfoHelper.Instance.GetTrayInfo(Stn4TurnTableStnEnum.DieToTray)?.DeepCopy();
                            var pickInfo = ProcInfoHelper.Instance.GetPickerInfo(Stn16TurnTableStnEnum.PlaceDie, RotateStnType.LoadDie)?.DeepCopy();
                            if (trayInfo != null && trayInfo.TrayModel != null &&
                                pickInfo != null && pickInfo.DieModel != null)
                            {
                                ShowMsg($"{StationName}->DieToTray工位Bin等级：Bin{trayInfo.TrayModel.BinLevel}，当前吸嘴Bin等级：{pickInfo.DieModel.BinLevel}");
                                if (trayInfo.TrayModel.BinLevel == pickInfo.DieModel.BinLevel.ToInt() || trayInfo.TrayModel.BinLevel == 0)
                                {
                                    PlaceZPos = GetPlacePos(CurPickNo);
                                    ShowMsg($"{StationName}->DieToTray工位的Bin等级和当前吸嘴Bin等级一致，吸嘴Z轴下降到放Die位置：{PlaceZPos}");
                                    MoveAbs(AxisName.PlaceDie_Z, PlaceZPos);
                                    step = 150;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->DieToTray工位的Bin等级和当前吸嘴Bin等级不一致，等待下一个Tray盘到位");
                                    RegisterService.Instance.SetBoolReg(RegisterEnum.CurrentBinLevelPickOver);
                                }
                            }
                            else
                            {
                                ShowMsg($"{StationName}->吸嘴：{CurPickNo}获取信息失败");
                                step = 1;
                            }
                        }
                        break;
                    }
                case 150:
                    {
                        var ret = IsAxisArrive(AxisName.PlaceDie_Z, PlaceZPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->通知DieToTray工位放Die完成，Z轴移动到待机位");
                            ResetBoolReg(RegisterEnum.DieToTrayStnAllowPlaceDie);
                            RegisterService.Instance.SetBoolReg(RegisterEnum.PlaceDieCom);
                            NozzleVacuum(CurPickNo, false);
                            MoveAbs(AxisName.PlaceDie_Z, PosName.WaitPos);
                            StepTimeInfoHelper.GetInfoByName(StepTimeEnum.PlaceDie.ToString())?.End();
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
                        var ret = IsAxisArrive(AxisName.PlaceDie_Z, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->放Die完成", MsgType.Notify);
                            step = 1;
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
        private void NozzleVacuum(int nozzleNo, bool value)
        {
            var ran = new Random();
            VacuumName vacuumName = EnumHelper.GetEnumValue<VacuumName>($"LoadDieNozzle{nozzleNo + 1}Vac");
            if (Global.RunMode == RunMode.空跑模式)
            {
                if (value)
                {
                    IOInfoHelper.GetAnalogInputInfoByName($"LoadDieNozzle{nozzleNo + 1}Value").AIValue = ran.Next(1100, 1500);
                }
                else
                {
                    IOInfoHelper.GetAnalogInputInfoByName($"LoadDieNozzle{nozzleNo + 1}Value").AIValue = ran.Next(100, 200);
                }
                Thread.Sleep(20);
            }
            Vacuum(vacuumName, value);
        }
        private bool IsNozzleVacArrive(int nozzleNo)
        {
            var inputName = $"LoadDieNozzle{nozzleNo + 1}Value";
            //var value = Motion.GetAnalogInput(inputName);
            var value = IOInfoHelper.GetAnalogInputInfoByName($"LoadDieNozzle{nozzleNo + 1}Value").AIValue;
            return value > ParaHelper.Motion.VacuumThreshold;
        }
        private double GetPlacePos(int pickNo)
        {
            double ret = 0;
            var posInfo = AxisInfoHelper.GetPosInfoByName(AxisName.PlaceDie_Z, PosName.PlaceDiePos.ToString());
            ret = posInfo.Pos + ParaHelper.Offset.LoadDieStnPlaceDieZOffset[pickNo];
            return ret;
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
            var stn = StationManager.Instance.GetChild<LoadDieDDStn>(nameof(LoadDieDDStn));
            stn.ResetRegisterEvent += Stn_ResetRegisterEvent;
        }

        private void Stn_ResetRegisterEvent(RegisterEnum obj)
        {
            if (obj == RegisterEnum.LoadDDRotateDone)
            {
                LastRegisterValue[obj.ToInt()] = false;
            }
        }

        public void ResetBits()
        {
            SubFlowStep = -1;
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
