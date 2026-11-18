using JC.PTIS.Main.AlarmInfo;
using JC.PTIS.Main.Devices.Vision;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Params;
using JC.PTIS.Main.RotateInfo;
using JC.PTIS.Main.Service;
using StdLib.PCBaseFrame.Alarm;
using StdLib.PCBaseFrame.Entities;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.PCBaseFrame.Extension.Entities;
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
    public class ThrowNgDieStn : StationBaseEx, IStation
    {
        public int StationNum { get; set; } = 17;
        public string StationName { get; set; } = "ThrowNgDieStn";

        int SubFlowStep = -1;

        int CurPickNo = -1;

        Delay Delay = new Delay();

        double DestPos = 0;
        public static bool ChangeIRNgTrayDone { get; set; }
        public void Home()
        {
            switch (HomeStep)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->开始回零");
                        ShowMsg($"{StationName}->IRNG_Y轴回零");
                        Motion.Home(AxisName.IRNG_Y);
                        HomeStep = 50;
                        break;
                    }
                case 50:
                    {
                        if (Motion.IsHomeDone(AxisName.IRNG_Y))
                        {
                            ShowMsg($"{StationName}->IRNG_Y轴回零完成");
                            UpdateHomeSts(StationName, StationNum, true);
                            HomeStep = 0;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.IRNG_Y))
                        {
                            ShowMsg($"{StationName}->IRNG_Y轴回零超时", MsgType.Error);
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
                                SubFlowStep = 1;
                                ShowMsg($"{StationName}->等待LoadDie转盘转动到位");
                                AutoRunStep = 100;
                            }
                            break;
                        }
                    case 100:
                        {
                            SubFlowStep = IRNgThrowFlow(SubFlowStep);
                            if (SubFlowStep == -1)
                            {
                                ShowMsg($"{StationName}->抛料流程执行失败", MsgType.Error);
                                StopRequest();
                            }
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                ShowMsg($"{StationName}->AutoRun方法异常：{ex.ToString()}", MsgType.Error, true);
                AlarmManager.CreateAlarm(AlarmCode.ProgramExceptionStart.ToInt() + StationNum, AllStationAlarm);
                StopRequest();
            }
        }
        /// <summary>
        /// IRNG抛料流程
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int IRNgThrowFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        if (GetBoolReg(RegisterEnum.LoadDDRotateDone, true))
                        {
                            ShowMsg($"{StationName}->LoadDie转盘转动到位，判断吸嘴上有无Die");
                            CurPickNo = RotateInfoHelper.Instance.GetStageNo(Stn16TurnTableStnEnum.ThrowDie.ToInt(), RotateStnType.LoadDie);
                            if (IsNozzleVacArrive(CurPickNo))
                            {
                                ShowMsg($"{StationName}->吸嘴：{CurPickNo}有Die，真空释放");
                                NozzleVacuum(CurPickNo, false);
                                Delay.Start(0.2);
                                step = 50;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->吸嘴：{CurPickNo}没有Die");
                            }
                        }
                        break;
                    }
                case 50:
                    {
                        if (IsNozzleVacArrive(CurPickNo) == false)
                        {
                            ShowMsg($"{StationName}->吸嘴：{CurPickNo}真空释放完成，更新IRNgTray信息");
                            var dieModel = ProcInfoHelper.Instance.PickDieFromPicker(Stn16TurnTableStnEnum.ThrowDie, RotateStnType.LoadDie);
                            if (dieModel != null)
                            {
                                var rowNo = ProcInfoHelper.Instance.GetIRNgTrayAvlRowNo();
                                ShowMsg($"{StationName}->IRNgTray可用槽位：{rowNo}");
                                ProcInfoHelper.Instance.PlaceDieToIRNgTray(rowNo, dieModel);
                                if (rowNo == 12 || rowNo == -1)//NGTray总共13个槽位
                                {
                                    ShowMsg($"{StationName}->IRNgTray放满Die，移动到下料位置");
                                    MoveAbs(AxisName.IRNG_Y, PosName.IRNgTrayLunLoadPos);
                                    step = 100;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->IRNgTray移动到下一个槽位");
                                    DestPos = GetDestPos(rowNo);
                                    MoveAbs(AxisName.IRNG_Y, DestPos);
                                    step = 150;
                                }
                            }
                        }
                        else if (Delay.TimeUp())
                        {
                            ShowMsg($"{StationName}->吸嘴：{CurPickNo}真空释放超时", MsgType.Error);
                        }
                        break;
                    }
                case 100:
                    {
                        var ret = IsAxisArrive(AxisName.IRNG_Y, PosName.IRNgTrayLunLoadPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->IRNgTray到达下料位置，等待更换空Tray");
                            AlarmManager.CreateAlarm(AlarmCode.IRNgTrayFull.ToInt(), StationNum);
                            step = 200;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->IRNgTray到达下料位置超时", MsgType.Error);
                        }
                        break;
                    }
                case 150:
                    {
                        var ret = IsAxisArrive(AxisName.IRNG_Y, DestPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->IRNgTray移动到位");
                            step = 1;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            ShowMsg($"{StationName}->IRNgTray移动到位超时", MsgType.Error);
                        }
                        break;
                    }
                case 200:
                    {
                        if (ChangeIRNgTrayDone)
                        {
                            ChangeIRNgTrayDone = false;
                            ShowMsg($"{StationName}->更换IRNgTray完成，移动到第一个槽位位置");
                            ProcInfoHelper.Instance.ClearIRNgTray();
                            DestPos = GetDestPos(0);
                            MoveAbs(AxisName.IRNG_Y, DestPos);
                            step = 150;
                        }
                        break;
                    }
            }
            return step;
        }
        public int MoveToWaitPosFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->NG接料Tray盘移动到接料位", MsgType.Notify);
                        MoveAbs(AxisName.IRNG_Y, PosName.WaitPos);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArrive(AxisName.IRNG_Y, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->NG接料Tray盘到达待机位", MsgType.Notify);
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
            return value > 1000;
        }
        private double GetDestPos(int rowNo)
        {
            double result = 0;
            var posInfo=AxisInfoHelper.GetPosInfoByName(AxisName.IRNG_Y, PosName.IRNgTrayFirstSlotPos);
            if (posInfo!=null)
            {
                result= posInfo.Pos + rowNo * 5.0;
            }
            return result;
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
        }

        public void StopHandle()
        {
        }

        public void WaitFlowDone()
        {
        }
    }
}
