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
    public class PickDieStn : StationBaseEx, IStation
    {
        public int StationNum { get; set; } = 3;
        public string StationName { get; set; } = "PickDieStn";

        List<AxisName> Axes = new List<AxisName>();
        List<PosName> PosNames = new List<PosName>();//位置名称集合
        double PickDieZPos = 0;

        #region 子流程状态机步骤
        int ModuleMoveStep = -1;
        int PickDieStep = -1;
        #endregion
        int CurPickNo = -1;

        public void Home()
        {
            switch (HomeStep)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->开始回零");
                        ShowMsg($"{StationName}->{AxisName.PickDie_Z}轴回零");
                        Motion.Home(AxisName.PickDie_Z);
                        HomeStep = 50;
                        break;
                    }
                case 50:
                    {
                        if (Motion.IsHomeDone(AxisName.PickDie_Z))
                        {
                            ShowMsg($"{StationName}->回零完成");
                            UpdateHomeSts(StationName, StationNum, true);
                            HomeStep = 0;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.PickDie_Z))
                        {
                            ShowMsg($"{StationName}->PickDie_Z轴回零超时", MsgType.Error);
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
                            ModuleMoveStep = 1;
                            AutoRunStep = 50;
                            break;
                        }
                    case 50:
                        {
                            ModuleMoveStep = MoveToWaitPosFlow(ModuleMoveStep);
                            if (ModuleMoveStep == 0)
                            {
                                RegisterService.Instance.SetBoolReg(RegisterEnum.PickDieStnSafe);
                                ShowMsg($"{StationName}->等待LoadDie转盘转动到位");
                                PickDieStep = 1;
                                AutoRunStep = 100;
                            }
                            break;
                        }
                    case 100:
                        {
                            PickDieStep = PickDieFlow(PickDieStep);
                            if (PickDieStep == -1)
                            {
                                ShowMsg($"{StationName}->取Die流程错误", MsgType.Error, true);
                                StopHandle();
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
        public int PickDieFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        if (GetBoolReg(RegisterEnum.LoadDDRotateDone, true))
                        {
                            ShowMsg($"{StationName}->LoadDie转盘转动到位，判断当前吸嘴上是否有Die");
                            CurPickNo = RotateInfoHelper.Instance.GetStageNo(Stn16TurnTableStnEnum.PickDie.ToInt(), RotateStnType.LoadDie);
                            if (IsNozzleVacArrive(CurPickNo))
                            {
                                ShowMsg($"{StationName}->吸嘴：{CurPickNo}有Die，通知LoadDie转盘可以转动");
                                RegisterService.Instance.SetBoolReg(RegisterEnum.PickDieStnSafe);
                                step = 1;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->吸嘴：{CurPickNo}没有Die，等待WaferTable工位对位完成");
                                NozzleVacuum(CurPickNo, true);
                                step = 50;
                            }
                        }
                        break;
                    }
                case 50:
                    {
                        if (GetBoolReg(RegisterEnum.AlignDieDone))
                        {
                            ResetBoolReg(RegisterEnum.WaferEmpty);
                            PickDieZPos = GetPickPos(CurPickNo);
                            StepTimeInfoHelper.GetInfoByName(StepTimeEnum.PickDie.ToString())?.Start();
                            ShowMsg($"{StationName}->WaferTable工位对位完成，吸嘴Z轴下降到取Die位置：{PickDieZPos}，顶针顶起");
                            MoveAbs(AxisName.PickDie_Z, PickDieZPos);
                            MoveAbs(AxisName.WaferTable_Thimble, PosName.LiftPos);
                            step = 100;
                        }
                        else if (GetBoolReg(RegisterEnum.WaferEmpty) && ProcInfoHelper.Instance.IsHasDieOnPicker())
                        {
                            NozzleVacuum(CurPickNo, false);
                            ShowMsg($"{StationName}->Wafer取Die完成，通知LoadDie转盘可以转动");
                            RegisterService.Instance.SetBoolReg(RegisterEnum.PickDieStnSafe);
                            step = 1;
                        }
                        break;
                    }
                case 100:
                    {
                        var ret = IsAxisArrive(AxisName.PickDie_Z, PickDieZPos);
                        var ret2 = IsAxisArrive(AxisName.WaferTable_Thimble, PosName.LiftPos);
                        if (ret == MotionStatus.Arrived && ret2 == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->吸嘴Z轴和顶针到位，判断真空是否吸附到位");
                            ResetBoolReg(RegisterEnum.AlignDieDone);
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
                        //if (IsVacArrive(CurPickNo))
                        {
                            ShowMsg($"{StationName}->真空是否吸附到位，吸嘴Z轴移动到待机位");
                            MoveAbs(AxisName.PickDie_Z, PosName.WaitPos);
                            RegisterService.Instance.SetBoolReg(RegisterEnum.PickDieStnPickDone);
                            StepTimeInfoHelper.GetInfoByName(StepTimeEnum.PickDie.ToString())?.End();
                            step = 200;
                        }
                        break;
                    }
                case 200:
                    {
                        var ret = IsAxisArrive(AxisName.PickDie_Z, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->吸嘴Z轴到达待机位，通知LoadDie转盘可以转动");
                            RegisterService.Instance.SetBoolReg(RegisterEnum.PickDieStnSafe);
                            step = 1;
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
        public int MoveToWaitPosFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        Axes = new List<AxisName>() { AxisName.PickDie_Z};
                        PosNames = new List<PosName>() { PosName.WaitPos};
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
      
        private double GetPickPos(int pickNo)
        {
            double ret = 0;
            var posInfo = AxisInfoHelper.GetPosInfoByName(AxisName.PickDie_Z,PosName.PickDiePos.ToString());
            ret= posInfo.Pos+ParaHelper.Offset.LoadDieStnPickDieZOffset[pickNo];
            return ret;
        }
        public void Init()
        {
            Init(StationNum, StationName);
            ResetBoolReg(RegisterEnum.WaferEmpty);
            ResetBoolReg(RegisterEnum.AlignDieDone);
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
            PickDieStep = -1;
            ModuleMoveStep = -1;
           // CurPickNo = -1;
        }

        public void StopHandle()
        {
        }

        public void WaitFlowDone()
        {
        }
    }
}
