using JC.PTIS.Main.AlarmInfo;
using JC.PTIS.Main.Devices.Vision;
using JC.PTIS.Main.Devices.VisionPC.MsgModel;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.Models.ModelHelper;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.RotateInfo;
using JC.PTIS.Main.Service;
using StdLib.LogHelper;
using StdLib.PCBaseFrame.Alarm;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.PCBaseFrame.Motion;
using StdLib.PCBaseFrame.Station;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Stations
{
    public class LoadDieDDStn : StationBaseEx, IStation
    {
        public int StationNum { get; set; } = 2;
        public string StationName { get; set; } = "LoadDieDDStn";

        List<AxisName> Axes = new List<AxisName>();
        List<PosName> PosNames = new List<PosName>();//位置名称集合

        #region 子流程状态机步骤
        int ModuleMoveStep = -1;

        #endregion
        double[] LoadDDPos = new double[17];

        int RotateNo = 0;
        int LastRotateNo = 0;
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
                        if (Motion.IsHomeDone(AxisName.PickDie_Z) &&
                            Motion.IsHomeDone(AxisName.PlaceDie_Z))
                        {
                            ShowMsg($"{StationName}->开始回零");
                            ShowMsg($"{StationName}->{AxisName.LoadDie_DD}轴回零");
                            Motion.Home(AxisName.LoadDie_DD);
                            HomeStep = 100;
                        }
                        break;
                    }
                case 100:
                    {
                        if (Motion.IsHomeDone(AxisName.LoadDie_DD))
                        {
                            ShowMsg($"{StationName}->回零完成");
                            RotateInfoHelper.Instance.UpdateRotateInfo(RotateStnType.LoadDie, 0);
                            UpdateHomeSts(StationName, StationNum, true);
                            HomeStep = 0;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.LoadDie_DD))
                        {
                            ShowMsg($"{StationName}->LoadDie_DD轴回零超时");
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
                                ShowMsg($"{StationName}->等待取Die工位、放Die工位、背面检工位和红外检工位安全信号");
                                AutoRunStep = 100;
                            }
                            break;
                        }
                    case 100:
                        {
                            if (GetBoolReg(RegisterEnum.PickDieStnSafe) &&
                                GetBoolReg(RegisterEnum.PlaceDieStnSafe) &&
                                GetBoolReg(RegisterEnum.IRDetectGrabDone) &&
                                GetBoolReg(RegisterEnum.BackDetectGrabDone))
                            {
                                ResetBoolReg(RegisterEnum.PickDieStnSafe);
                                ResetBoolReg(RegisterEnum.PlaceDieStnSafe);
                                ResetBoolReg(RegisterEnum.IRDetectGrabDone);
                                ResetBoolReg(RegisterEnum.BackDetectGrabDone);
                                ResetBoolReg(RegisterEnum.LoadDDRotateDone);
                                ShowMsg($"{StationName}->收到取Die工位和放Die工位安全信号，判断转盘是否可以转动");
                                AutoRunStep = 150;
                            }
                            break;
                        }
                    case 150:
                        {
                            if (IsDDCanRotate())
                            {
                                ShowMsg($"{StationName}->转盘转动到{LoadDDPos[RotateNo]}度位置");
                                MoveAbs(AxisName.LoadDie_DD, LoadDDPos[RotateNo]);
                                AutoRunStep = 200;
                            }
                            break;
                        }
                    case 200:
                        {
                            var ret = IsDDArrive();
                            if (ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->转盘到达{LoadDDPos[RotateNo]}度位置，通知取Die工位和放Die工位转盘转动完成");
                                RegisterService.Instance.SetBoolReg(RegisterEnum.LoadDDRotateDone);
                                LastRotateNo = RotateNo;
                                RotateNo++;
                                if (RotateNo == 17)
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
        private bool IsDDCanRotate()
        {
            if (Motion.IsArrive(AxisName.PickDie_Z, PosName.WaitPos) &&
                Motion.IsArrive(AxisName.PlaceDie_Z, PosName.WaitPos))
            {
                return true;
            }
            else
            {
                ShowMsg($"{StationName}->条件不满足，禁止转盘转动", MsgType.Error);
            }
            return false;
        }
        private MotionStatus IsDDArrive()
        {
            var ret = IsAxisArrive(AxisName.LoadDie_DD, LoadDDPos[RotateNo]);
            if (ret == MotionStatus.Arrived)
            {
                if (LoadDDPos[RotateNo] == 360)
                {
                    Thread.Sleep(50);
                    Motion.ZeroPos(AxisName.LoadDie_DD);
                }
                RotateInfoHelper.Instance.UpdateRotateInfo(RotateStnType.LoadDie, LoadDDPos[RotateNo]);
            }
            return ret;
        }
        public int MoveToWaitPosFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        RotateNo = LastRotateNo;
                        Axes = new List<AxisName>() { AxisName.PickDie_Z, AxisName.PlaceDie_Z };
                        PosNames = new List<PosName>() { PosName.WaitPos, PosName.WaitPos };
                        ShowMsg($"{StationName}->等待Z轴回待机位完成");
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            MoveAbs(AxisName.LoadDie_DD, LoadDDPos[RotateNo]);
                            step = 100;
                        }
                        break;
                    }
                case 100:
                    {
                        var ret = IsAxisArrive(AxisName.LoadDie_DD, LoadDDPos[RotateNo]);
                        if (ret == MotionStatus.Arrived)
                        {
                            RotateInfoHelper.Instance.UpdateRotateInfo(RotateStnType.LoadDie, LoadDDPos[RotateNo]);
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

        public void Init()
        {
            for (int i = 0; i <= 16; i++)
            {
                LoadDDPos[i] = 22.5 * i;
            }
            Init(StationNum, StationName);
            ResetBoolReg(RegisterEnum.LoadDDRotateDone);
        }

        public void OnButtonTrigger(IOInfo io, bool sts)
        {
        }

        public void PauseHandle()
        {
        }

        public void RegisterEvent()
        {
            DetectHelper.OnDetectionCompleted += Instance_OnDetectionCompleted;
        }

        private void Instance_OnDetectionCompleted(DetectionCompletedBody obj)
        {
            UpdateDetectResult(obj);
        }
        private void UpdateDetectResult(DetectionCompletedBody obj)
        {
            try
            {
                if (obj.Station == DetectStationEnum.IRDetect.ToString() ||
                    obj.Station == DetectStationEnum.BackDetect.ToString())
                {
                    if (obj.ItemResultList != null && obj.ItemResultList.Length > 0)
                    {
                        var sn = obj.ItemResultList[0].SN;
                        if (string.IsNullOrEmpty(sn) == false)
                        {
                            ShowMsg($"{StationName}->收到{obj.Station}检测完成消息，类型:{obj.Station}，条码:{sn}，结果:{obj.ItemResultList[0].Result}");
                            var arr = ProcInfoHelper.Instance.GetPickerArray(RotateStnType.LoadDie);
                            var picker = arr?.AsEnumerable()?.Where(x => x != null && x.DieModel != null && x.DieModel.DieCode == sn)?.FirstOrDefault();
                            if (picker != null)
                            {
                                picker.DieModel.DieStatus = DieStatus.OK;
                                switch (obj.Station)
                                {
                                    case nameof(DetectStationEnum.IRDetect):
                                        {
                                            if (obj.ItemResultList[0].Result == "NG")
                                            {
                                                picker.DieModel.DieStatus = DieStatus.IRNG;
                                            }
                                            break;
                                        }
                                    case nameof(DetectStationEnum.BackDetect):
                                        {
                                            if (obj.ItemResultList[0].Result == "NG")
                                            {
                                                picker.DieModel.DieStatus = DieStatus.BackNG;
                                            }
                                            break;
                                        }
                                    default:
                                        break;
                                }
                                DieModelHelper.Update(picker.DieModel);
                                ProcInfoHelper.Instance.UpdatePickInfo(picker, RotateStnType.LoadDie);
                            }
                            else
                            {
                                ShowMsg($"{StationName}->[LoadDieDDStn][UpdateDetectResult]方法执行异常,异常原因：未能查询到该SN:{sn}相关信息");
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMsg($"{StationName}->UpdateDetectResult方法异常：{ex}", MsgType.Error, true);
                LogManager.GetLogger("Error").Error($"[LoadDieDDStn][UpdateDetectResult]方法执行异常，异常原因：{ex}");
            }
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
