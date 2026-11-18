using JC.PTIS.Main.AlarmInfo;
using JC.PTIS.Main.Devices.Vision;
using JC.PTIS.Main.Devices.VisionPC.MsgModel;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Params;
using JC.PTIS.Main.RotateInfo;
using JC.PTIS.Main.Service;
using StdLib.PCBaseFrame.Alarm;
using StdLib.PCBaseFrame.Args;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.PCBaseFrame.Extension.Recipe;
using StdLib.PCBaseFrame.Motion;
using StdLib.PCBaseFrame.Station;
using StdLib.WindowsControls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Stations
{
    public class BackDetectStn : StationBaseEx, IStation
    {
        public int StationNum { get; set; } = 4;
        public string StationName { get; set; } = "BackDetectStn";

        List<AxisName> Axes = new List<AxisName>();
        List<PosName> PosNames = new List<PosName>();//位置名称集合
        List<double> Poses = new List<double>();//实际位置集合

        #region 子流程状态机步骤
        int ModuleMoveStep = -1;
        int DetectStep = -1;
        #endregion
        int CurPickNo = -1;
       // string WaferCode = string.Empty;
        public void Home()
        {
            switch (HomeStep)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->开始回零");
                        Motion.Home(AxisName.BackDetect_Z);
                        ShowMsg($"{StationName}->{AxisName.BackDetect_Z}轴开始回零");
                        HomeStep = 50;
                        break;
                    }
                case 50:
                    {
                        if (Motion.IsHomeDone(AxisName.BackDetect_Z))
                        {
                            ShowMsg($"{StationName}->{AxisName.BackDetect_Z}轴回零完成");
                            Motion.Home(AxisName.BackDetect_X);
                            ShowMsg($"{StationName}->{AxisName.BackDetect_X}轴开始回零");
                            HomeStep = 100;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.BackDetect_Z))
                        {
                            ShowMsg($"{StationName}->BackDetect_Z轴回零超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 100:
                    {
                        if (Motion.IsHomeDone(AxisName.BackDetect_X))
                        {
                            ShowMsg($"{StationName}->{AxisName.BackDetect_X}轴回零完成");
                            ShowMsg($"{StationName}->回零完成");
                            UpdateHomeSts(StationName, StationNum, true);
                            HomeStep = 0;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.BackDetect_X))
                        {
                            ShowMsg($"{StationName}->BackDetect_X轴回零超时", MsgType.Error);
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
                            ShowMsg($"{StationName}->背面检测模组移动到等待位");
                            ModuleMoveStep = 1;
                            AutoRunStep = 50;
                            break;
                        }
                    case 50:
                        {
                            ModuleMoveStep = MoveToWaitPosFlow(ModuleMoveStep);
                            if (ModuleMoveStep == 0)
                            {
                                ShowMsg($"{StationName}->背面检测模组到达待机位，通知LoadDie转盘可以转动");
                                AutoRunStep = 100;
                            }
                            break;
                        }
                    case 100:
                        {
                            DetectStep = 1;
                            RegisterService.Instance.SetBoolReg(RegisterEnum.BackDetectGrabDone);
                            ShowMsg($"{StationName}->等待LoadDie转盘转动到位");
                            AutoRunStep = 150;
                            break;
                        }
                    case 150:
                        {
                            DetectStep = BackDetectFlow(DetectStep);
                            if (DetectStep == 0)
                            {
                                ShowMsg($"{StationName}->背面检测流程执行结束，通知LoadDie转盘可以转动");
                                AutoRunStep = 100;
                            }
                            else if (DetectStep == -1)
                            {
                                ShowMsg($"{StationName}->背面检测流程错误", MsgType.Error, true);
                                var ret = AskHowToDoRequest(StationNum,$"{StationName}->背面检测流程错误，弹窗询问如何处理");
                                switch (ret)
                                {
                                    case AskResult.Retry:
                                        {
                                            ShowMsg($"{StationName}->处理结果：重试");
                                            DetectStep = 1;
                                            break;
                                        }
                                    case AskResult.Skip:
                                        {
                                            ShowMsg($"{StationName}->处理结果：跳过。通知LoadDie转盘可以转动");
                                            AutoRunStep = 100;
                                            break;
                                        }
                                    default:
                                        break;
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
                        Axes = new List<AxisName>() { AxisName.BackDetect_Z, AxisName.BackDetect_X };
                        PosNames = new List<PosName>() { PosName.DetectPos, PosName.WaitPos };
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

        public int BackDetectFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        if (GetBoolReg(RegisterEnum.LoadDDRotateDone, true))
                        {
                            ShowMsg($"{StationName}->LoadDie转盘转动到位，判断吸嘴上有无Die");
                            CurPickNo = RotateInfoHelper.Instance.GetStageNo(Stn16TurnTableStnEnum.BackDetect.ToInt(), RotateStnType.LoadDie);
                            if (IsNozzleVacArrive(CurPickNo) == false)
                            {
                                ShowMsg($"{StationName}->吸嘴：{CurPickNo}没有Die，不需要执行背面检测");
                                step = 0;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->吸嘴：{CurPickNo}有Die，判断是够需要执行背面检");
                                step = 20;
                            }
                        }
                        break;
                    }
                case 20:
                    {
                        var pickDieInfo = ProcInfoHelper.Instance.GetPickerInfo(Stn16TurnTableStnEnum.BackDetect, RotateStnType.LoadDie);
                        if (pickDieInfo != null && pickDieInfo.DieModel != null)
                        {
                            if (pickDieInfo.DieModel.BinLevel.ToInt() % 2 == 1)
                            {
                                ShowMsg($"{StationName}->吸嘴：{CurPickNo}上的Die等级是：{pickDieInfo.DieModel.BinLevel}，执行背面检测");
                                string lotID = string.Empty;
                                var req = new TriggerDetectReqBody();
                                if (string.IsNullOrEmpty(pickDieInfo.DieModel.DieCode) == false)
                                {
                                    var arr = pickDieInfo.DieModel.DieCode.Split("_");
                                    lotID = arr[1];
                                }
                                else
                                {
                                    lotID = DateTime.Now.ToString("yyyyMMddHHmmss");
                                }
                                req.LotId = lotID;
                                req.ProductId = lotID;
                                req.BatchCount = 1;
                                req.RecipeName = RecipeManager.CurRecipe.RecipeID;
                                req.ProductList = new Product[] { new Product() { SN = pickDieInfo.DieModel.DieCode, Index = pickDieInfo.DieModel.Index } };

                                var body = DetectHelper.Instance.Detect<TriggerDetectRspBody>(VisionCmd.TriggerDetection, DetectStationEnum.BackDetect, req);
                                if (body != null && body.Code == 0)
                                {
                                    ShowMsg($"{StationName}->检测PC背面检测准备完成");
                                    step = 50;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->检测PC背面检测准备失败", MsgType.Error);
                                    AlarmManager.CreateAlarm(AlarmCode.DetectPCBackDetectReadyFail.ToInt(), StationNum);
                                }
                            }
                            else
                            {
                                ShowMsg($"{StationName}->吸嘴：{CurPickNo}上的Die等级是：{pickDieInfo.DieModel.BinLevel}，不需要执行背面检测");
                                step = 0;
                            }
                        }
                        else
                        {
                            //AlarmManager.CreateAlarm(AlarmCode.GetDieModelFromLoadDDPickerFail.ToInt(), StationNum);
                            ShowMsg($"{StationName}->吸嘴：{CurPickNo}上的Die信息为空，不做检测", MsgType.Error);
                            step = 0;
                        }
                        break;
                    }
                case 50:
                    {
                        ShowMsg($"{StationName}->X轴移动到飞拍起始位置");
                        MoveAbs(AxisName.BackDetect_X, PosName.GrabStartPos);
                        step = 100;
                        break;
                    }
                case 100:
                    {
                        var ret = IsAxisArrive(AxisName.BackDetect_X, PosName.GrabStartPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->X轴到达飞拍起始位置，X轴移动到飞拍结束位置，飞拍开始");
                            Motion.CompareStop(CompareName.BackDetectGrab);
                            Motion.SetCompare(CompareName.BackDetectGrab);
                            MoveAbs(AxisName.BackDetect_X, PosName.GrabEndPos);
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
                        var info = Motion.GetCompare(CompareName.BackDetectGrab, out int triggerCoun, out short status);
                        //ShowMsg($"{StationName}->已触发次数：{triggerCoun}");
                        var ret = IsAxisArrive(AxisName.BackDetect_X, PosName.GrabEndPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->X轴到达飞拍结束位置，飞拍完成，背面检测流程结束", MsgType.Notify);
                            RegisterService.Instance.SetBoolReg(RegisterEnum.BackDetectGrabDone);
                            Motion.CompareStop(CompareName.BackDetectGrab);
                            MoveAbs(AxisName.BackDetect_X, PosName.GrabStartPos);
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 100;
                        }
                        break;
                    }
            }
            return step;
        }
        private bool IsNozzleVacArrive(int nozzleNo)
        {
            var inputName = $"LoadDieNozzle{nozzleNo + 1}Value";
            //var value = Motion.GetAnalogInput(inputName);
            var value = IOInfoHelper.GetAnalogInputInfoByName($"LoadDieNozzle{nozzleNo + 1}Value").AIValue;
            return value > 1000;
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
            ModuleMoveStep = -1;
            DetectStep = -1;
        }

        public void StopHandle()
        {
        }

        public void WaitFlowDone()
        {
        }
    }
}
