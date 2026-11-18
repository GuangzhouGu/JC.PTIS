using JC.PTIS.Main.AlarmInfo;
using JC.PTIS.Main.Devices.Vision;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.FlowInfo;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.Models.ModelHelper;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Params;
using JC.PTIS.Main.RotateInfo;
using JC.PTIS.Main.Service;
using JC.PTIS.Main.UserCtrls;
using StdLib.PCBaseFrame.Alarm;
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
    public class LuloadTrayStn : StationBaseEx, IStation
    {
        public int StationNum { get; set; } = 11;
        public string StationName { get; set; } = "LuloadTrayStn";

        List<AxisName> Axes = new List<AxisName>();
        List<PosName> PosNames = new List<PosName>();//位置名称集合

        #region 子流程状态机步骤
        int SubFlowStep = -1;
        int LoadEmptyTrayStep = -1;
        #endregion
        StageInfo CurStage = null;
        TrayStageInfo CurTrayInfo = null;
        BinLevel BinLevel = BinLevel.None;
        public void Home()
        {
            switch (HomeStep)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->开始回零");
                        UpdateHomeSts(StationName, StationNum, true);
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
                            RegisterService.Instance.SetBoolReg(RegisterEnum.TrayLuloadDone);
                            ShowMsg($"{StationName}->等待4工位转盘转动到位");
                            AutoRunStep = 50;
                            break;
                        }
                    case 50:
                        {
                            if (GetBoolReg(RegisterEnum.Stn4DDRotateDone,true))
                            {
                                var stageNo = RotateInfoHelper.Instance.GetStageNo(Stn4TurnTableStnEnum.LuloadTrayStn.ToInt(), RotateStnType.Stn4TurnTable);
                                CurStage = StageManager.Get(stageNo);
                                ShowMsg($"{StationName}->4工位转盘转动到位，判断TrayStage{CurStage.StageNum}是否有Tray");
                                if (Motion.GetDigitalInput(CurStage.IsExistTray))
                                {
                                    CurTrayInfo = ProcInfoHelper.Instance.GetTrayInfo(Stn4TurnTableStnEnum.LuloadTrayStn);
                                    if (CurTrayInfo != null && CurTrayInfo.TrayModel != null)
                                    {
                                        ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}有Tray，状态：{CurTrayInfo.TrayModel.TrayStatus}，等级：Bin{CurTrayInfo.TrayModel.BinLevel}");
                                        //TrayStage有Tray，并且是满Tray或者是Ok半Tray或者是未检测半Tray并且Wafer中没有Die，则执行下Tray流程
                                        if (CurTrayInfo.TrayModel.TrayStatus == TrayStatusEnum.OkFull||
                                            CurTrayInfo.TrayModel.TrayStatus == TrayStatusEnum.OkHalf||
                                              CurTrayInfo.TrayModel.TrayStatus == TrayStatusEnum.NoDetectFull||
                                            (CurTrayInfo.TrayModel.TrayStatus == TrayStatusEnum.NoDetectHalf&&
                                            !ProcInfoHelper.Instance.IsExistDieInWafer((BinLevel)CurTrayInfo.TrayModel.BinLevel)))
                                        {
                                            ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}检测和挑补完成");
                                            SubFlowStep = 1;
                                            //如果是Ok半Tray或者是未检测半Tray并且Wafer中没有Die，则通知HalfTray工位和8工位转盘暂存半Tray
                                            if (CurTrayInfo.TrayModel.TrayStatus == TrayStatusEnum.OkHalf ||
                                            (CurTrayInfo.TrayModel.TrayStatus == TrayStatusEnum.NoDetectHalf &&
                                            !ProcInfoHelper.Instance.IsExistDieInWafer((BinLevel)CurTrayInfo.TrayModel.BinLevel)))
                                            {
                                                ShowMsg($"{StationName}->执行UnloadTray流程，并发送暂存半Tray请求，等级：Bin{CurTrayInfo.TrayModel.BinLevel}");
                                                RegisterService.Instance.SetBoolReg(RegisterEnum.StorageHalfTrayReq);
                                            }
                                            else
                                            {
                                                ShowMsg($"{StationName}->执行UnloadTray流程，等级：Bin{CurTrayInfo.TrayModel.BinLevel}");
                                            }
                                            AutoRunStep = 150;
                                        }
                                        //空Tray或者未检测半Tray，并且Wafer中有Die，则不再请求上Tray
                                        else if (CurTrayInfo.TrayModel.TrayStatus == TrayStatusEnum.Empty||
                                            (CurTrayInfo.TrayModel.TrayStatus == TrayStatusEnum.NoDetectHalf&& 
                                            ProcInfoHelper.Instance.IsExistDieInWafer((BinLevel)CurTrayInfo.TrayModel.BinLevel)))
                                        {
                                            ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}是空Tray或者是未检测半Tray，通知4工位转盘可以转动");
                                            AutoRunStep = 1;
                                        }
                                    }
                                    else
                                    {
                                        ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}有Tray但是获取Tray信息失败", MsgType.Error, true);
                                        AlarmManager.CreateAlarm(AlarmCode.GetTrayInfoFail.ToInt(), AllStationAlarm);
                                    }
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}没有Tray，判断Wafer是否为空");
                                    AutoRunStep = 100;
                                }
                            }
                            break;
                        }
                    case 100:
                        {
                            if (GetBoolReg(RegisterEnum.WaferEmpty)||!ProcInfoHelper.Instance.IsHasDieOnPicker())
                            {
                                if (IsExistTrayInStn4())//Wafer空料，4工位转盘不再上空Tray，把转塔上的Tray盘全部下完
                                {
                                    ShowMsg($"{StationName}->Wafer为空，4工位转盘上还有Tray，通知4工位转盘可以转动");
                                    AutoRunStep = 1;
                                }
                            }
                            else
                            {
                                if (IsLastBin())//Wafer取到最后一个等级的Bin，并且DieToTray工位Tray盘剩余空穴够用，不再请求上空Tray，把转塔上的Tray盘全部下完
                                {
                                    ShowMsg($"{StationName}->Wafer取到最后一个等级的Bin，并且DieToTray工位Tray盘剩余空穴够用，不再请求上空Tray，通知4工位转盘可以转动");
                                    AutoRunStep = 1;
                                }
                                else
                                {
                                    var temp = ProcInfoHelper.Instance.GetPickerInfo(Stn16TurnTableStnEnum.PlaceDie, RotateStnType.LoadDie);
                                    if (temp != null && temp.DieModel != null)
                                    {
                                        var pickInfo = temp.DeepCopy();
                                        if (IsNeedLoadTray(pickInfo.DieModel.BinLevel))
                                        {
                                            ShowMsg($"{StationName}->DieToTray工位Tray盘剩余空穴数量不够放{pickInfo.DieModel.BinLevel}等级Die");
                                            BinLevel = pickInfo.DieModel.BinLevel;
                                        }
                                        else
                                        {
                                            ShowMsg($"{StationName}->DieToTray工位Tray盘剩余空穴数量够放{pickInfo.DieModel.BinLevel}等级Die");
                                            BinLevel = pickInfo.DieModel.BinLevel + 1;
                                        }
                                        ShowMsg($"{StationName}->请求上{BinLevel}等级Tray");
                                        SubFlowStep = 1;
                                        AutoRunStep = 200;
                                    }
                                }
                            }
                            break;
                        }
                    case 150:
                        {
                            SubFlowStep = UnloadFullTrayFlow(SubFlowStep);
                            if (SubFlowStep == 0)
                            {
                                ShowMsg($"{StationName}->UnloadTray流程结束，判断是否需要从半Tray缓存中取Tray");
                                AutoRunStep = 100;
                            }
                            break;
                        }
          
                    case 200:
                        {
                            SubFlowStep= LoadTrayFlow(SubFlowStep, BinLevel);
                            if (SubFlowStep == 0)
                            {
                                Vacuum(CurStage.Vacuum, true);
                                ShowMsg($"{StationName}->上空Tray流程结束，通知4工位转盘可以转动");
                                AutoRunStep = 1;
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
        /// 从半Tray缓存中取半Tray
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int LoadHalfTrayFormBufferFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->从半Tray缓存取半Tray流程开始，等级：{BinLevel}",MsgType.Notify);
                        ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}移动到上下料位置");
                        Axes = new List<AxisName>() { CurStage.X, CurStage.Y, CurStage.T };
                        PosNames = new List<PosName>() { PosName.LuloadTrayPos, PosName.LuloadTrayPos, PosName.LuloadTrayPos };
                        MoveAbsM(Axes, PosNames);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}移动到上下料位置，发送从半Tray缓存取Tray请求");
                            RegisterService.Instance.SetBoolReg(RegisterEnum.LoadTrayFromHalfTrayBufferToStn4Req);
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
                        if (GetBoolReg(RegisterEnum.LoadTrayFromHalfTrayBufferToStn4Done))
                        {
                            ShowMsg($"{StationName}->从半Tray缓存取半Tray流程结束，等级：{BinLevel}", MsgType.Notify);
                            ResetBoolReg(RegisterEnum.LoadTrayFromHalfTrayBufferToStn4Done);
                            step = 0;
                        }
                        break;
                    }
                default:
                    break;
            }
            return step;
        }
        public int LoadTrayFlow(int step,BinLevel binLevel)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->上Tray流程开始，等级：{binLevel}", MsgType.Notify);
                        var value = false;
                        //判断满Tray手臂是否有等级一致的Tray
                        if (Motion.GetDigitalInput(InputName.FullTrayArmIsExistTray)&&
                            ProcInfoHelper.Instance.GetTrayInfo(TrayPosEnum.FullTrayArm).TrayModel!=null)
                        {
                            var trayInfo = ProcInfoHelper.Instance.GetTrayInfo(TrayPosEnum.FullTrayArm);
                            if (trayInfo.TrayModel.BinLevel == binLevel.ToInt()&&
                                trayInfo.TrayModel.TrayStatus== TrayStatusEnum.NoDetectHalf)
                            {
                                ShowMsg($"{StationName}->满Tray手臂上有等级一致的Tray");
                                value = true;
                            }
                        }
                        if (value==false)
                        {
                            //判断Ok半Tray平台是否有等级一致的Tray
                            if (IsVacuumArriveEx(VacuumName.OKHalfTrayVac,true) &&
                                ProcInfoHelper.Instance.GetTrayInfo(TrayPosEnum.OkHalfTrayStage).TrayModel != null)
                            {
                                var trayInfo = ProcInfoHelper.Instance.GetTrayInfo(TrayPosEnum.OkHalfTrayStage);
                                if (trayInfo.TrayModel.BinLevel == binLevel.ToInt() &&
                                   trayInfo.TrayModel.TrayStatus == TrayStatusEnum.NoDetectHalf)
                                {
                                    ShowMsg($"{StationName}->Ok半Tray平台上有等级一致的Tray");
                                    value = true;
                                }
                            }
                        }
                        if (value == false)
                        {  
                            //判断半Tray缓存上是否有等级一致的Tray
                            if (Motion.GetDigitalInput(InputName.HalfTrayBufferArmSensor) &&
                                ProcInfoHelper.Instance.GetTrayInfo(TrayPosEnum.HalfTrayBufferArm).TrayModel != null)
                            {
                                var trayInfo = ProcInfoHelper.Instance.GetTrayInfo(TrayPosEnum.HalfTrayBufferArm);
                                if (trayInfo.TrayModel.BinLevel ==binLevel.ToInt() &&
                                    trayInfo.TrayModel.TrayStatus == TrayStatusEnum.NoDetectHalf)
                                {
                                    ShowMsg($"{StationName}->半Tray缓存手臂上有等级一致的Tray");
                                    value = true;
                                }
                            }
                        }
                        if (value == false)
                        {
                            //判断半Tray缓存中是否有等级一致的Tray
                            ShowMsg($"{StationName}->半Tray缓存中有等级一致的Tray");
                            value = ProcInfoHelper.Instance.IsHalfTrayBufferExistTray(binLevel, true, HalfTraySource.Stn4);
                        }
                        if (value)
                        {
                            RegisterService.Instance.ResetIntValue(IntValueEnum.ShiftBinLevel);
                            RegisterService.Instance.SetIntValue(IntValueEnum.ShiftBinLevel, BinLevel.ToInt());
                            LoadEmptyTrayStep = 1;
                            step = 50;
                        }
                        else//如果缓存中没有Bin等级的半Tray，则向8工位转盘请求上空Tray
                        {
                            ShowMsg($"{StationName}->半Tray缓存以及满Tray手臂上都没有{binLevel}等级半Tray，执行上空Tray流程");
                            LoadEmptyTrayStep = 1;
                            step = 100;
                        }
                        break;
                    }
                case 50:
                    {
                        LoadEmptyTrayStep = LoadHalfTrayFormBufferFlow(LoadEmptyTrayStep);
                        if (LoadEmptyTrayStep==0)
                        {
                            ShowMsg($"{StationName}->上Tray流程结束", MsgType.Notify);
                            step = 0;

                            if (Global.RunMode == RunMode.空跑模式)
                            {
                                IOInfoHelper.GetInputInfoByName(CurStage.IsExistTray.ToString(), out int _).IsShield = true;
                                Thread.Sleep(500);
                            }
                        }
                        break;
                    }
                case 100:
                    {
                        LoadEmptyTrayStep = LoadEmptyTrayFromStn8Flow(LoadEmptyTrayStep);
                        if (LoadEmptyTrayStep == 0)
                        {                       
                            ShowMsg($"{StationName}->上Tray流程结束", MsgType.Notify);
                            step = 0;

                            if (Global.RunMode == RunMode.空跑模式)
                            {
                                IOInfoHelper.GetInputInfoByName(CurStage.IsExistTray.ToString(), out int _).IsShield = true;
                                Thread.Sleep(500);
                            }
                        }
                        break;
                    }
                default:
                    break;
            }
            return step;
        }
        public int LoadEmptyTrayFromStn8Flow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->从8工位转盘上空Tray流程开始", MsgType.Notify);
                        ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}移动到上下料位置");
                        Axes = new List<AxisName>() { CurStage.X, CurStage.Y, CurStage.T };
                        PosNames = new List<PosName>() { PosName.LuloadTrayPos };
                        MoveAbsM(Axes, PosNames);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}移动到上下料位置，发送上空Tray请求给8工位转盘工位");
                            RegisterService.Instance.SetBoolReg(RegisterEnum.Stn4DDStnLoadEmptyReq);
                            step = 100;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 50;
                        }
                        break;
                    }
                case 100:
                    {
                        if (GetBoolReg(RegisterEnum.TrayArmLoadEmptyTrayDone))
                        {
                            ResetBoolReg(RegisterEnum.TrayArmLoadEmptyTrayDone);
                            ShowMsg($"{StationName}->8工位转盘Arm手臂放空Tray到4工位转盘完成");
                            var trayModel = ProcInfoHelper.Instance.PickTrayFromTrayStage(TrayPosEnum.EmptyTrayArm);
                            if (trayModel != null)
                            {
                                trayModel.EqpInTime = DateTime.Now;
                                ProcInfoHelper.Instance.PutTrayToTrayStage(Stn4TurnTableStnEnum.LuloadTrayStn, trayModel);
                                ShowMsg($"{StationName}->从8工位转盘上空Tray流程结束", MsgType.Notify);
                                TrayModelHelper.Insert(trayModel);
                                step = 0;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->获取EmptyTrayArm上的TrayModel信息失败", MsgType.Error);
                                AlarmManager.CreateAlarm(AlarmCode.GetTrayInfoFail.ToInt(), AllStationAlarm);
                            }
                        }
                        break;
                    }
            }
            return step;
        }
        public int UnloadFullTrayFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->4工位转盘下满Tray流程开始", MsgType.Notify);
                        ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}移动到上下料位置");
                        Axes = new List<AxisName>() { CurStage.X, CurStage.Y, CurStage.T };
                        PosNames= new List<PosName>() { PosName.LuloadTrayPos };
                        MoveAbsM(Axes, PosNames);
                        Vacuum(CurStage.Vacuum, false);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}移动到上下料位置，发送下Tray请求给8工位转盘工位");
                            RegisterService.Instance.SetBoolReg(RegisterEnum.Stn4DDUnloadFullTrayReq);
                            step = 100;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 50;
                        }
                        break;
                    }
                case 100:
                    {
                        if (GetBoolReg(RegisterEnum.TrayArmUnloadFullTrayDone))
                        {
                            ResetBoolReg(RegisterEnum.TrayArmUnloadFullTrayDone);
                            ShowMsg($"{StationName}->4工位转盘下满Tray流程结束", MsgType.Notify);
                            step = 0;

                            if (Global.RunMode == RunMode.空跑模式)
                            {
                                IOInfoHelper.GetInputInfoByName(CurStage.IsExistTray.ToString(), out int _).IsShield = false;
                                Thread.Sleep(500);
                            }
                        }
                        break;
                    }
            }
            return step;
        }
        /// <summary>
        /// 4号工位是否有Tray，返回true表示有Tray，false表示无Tray
        /// </summary>
        /// <returns></returns>
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
        /// <summary>
        /// Wafer是否取到最后一个等级的Bin，并且Tray盘空穴位够用
        /// </summary>
        /// <returns></returns>
        private bool IsLastBin()
        {
            bool ret = false;
            BinLevel bin = BinLevel.None;
            //ShowMsg($"{StationName}->判断Wafer是否取到最后一个等级的Bin");
            var isLastBin = ProcInfoHelper.Instance.IsLastBin(ref bin);//判断是否是最后一个等级的Bin
            if (isLastBin)
            {
                ShowMsg($"{StationName}->当前Wafer中取到最后一个等级的Bin，等级：{bin}");
                var remainDieCount = ProcInfoHelper.Instance.RemainDieCount(bin);//Wafer和转塔吸嘴上的Die数量
                ShowMsg($"{StationName}->Wafer和转塔吸嘴上剩余{bin}等级的Die数量：{remainDieCount}");
                var trayInfo = ProcInfoHelper.Instance.GetTrayInfo(Stn4TurnTableStnEnum.DieToTray);
                if (trayInfo != null && trayInfo.TrayModel != null)
                {
                    if (bin.ToInt() == trayInfo.TrayModel.BinLevel)
                    {
                        var emptyCellCount = trayInfo.TrayModel.DieArray.Cast<DieModel>().Count(x => x == null);
                        ShowMsg($"{StationName}->DieToTray工位Tray盘剩余空穴数量：{emptyCellCount}");
                        if (remainDieCount <= emptyCellCount)//如果Wafer上的Die数量和转塔上的Die总数小于DieToTray工位Tray盘中剩余空穴数量，则不用请求上空Tray
                        {
                            ret = true;
                        }
                    }
                    else
                    {
                        ShowMsg($"{StationName}->DieToTray工位Tray盘和当前Bin等级不匹配");
                    }
                 
                }
                else
                {
                    ShowMsg($"{StationName}->DieToTray工位没有Tray盘");
                }
            }
            return ret;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="bin"></param>
        /// <returns></returns>
        private bool IsNeedLoadTray(BinLevel bin)
        {
            bool ret = true;
            ShowMsg($"{StationName}->判断是否需要上半Tray或者空Tray");
            var remainDieCount = ProcInfoHelper.Instance.RemainDieCount(bin);//Wafer和转塔吸嘴上的Die数量
            ShowMsg($"{StationName}->Wafer和转塔吸嘴上剩余{bin}等级的Die数量：{remainDieCount}");
            var trayInfo = ProcInfoHelper.Instance.GetTrayInfo(Stn4TurnTableStnEnum.DieToTray);
            if (trayInfo != null && trayInfo.TrayModel != null)
            {
                if (bin.ToInt() == trayInfo.TrayModel.BinLevel|| trayInfo.TrayModel.BinLevel==0)
                {
                    var emptyCellCount = trayInfo.TrayModel.DieArray.Cast<DieModel>().Count(x => x == null);
                    ShowMsg($"{StationName}->DieToTray工位Tray盘剩余空穴数量：{emptyCellCount}");
                    if (remainDieCount <= emptyCellCount)//如果Wafer上的Die数量和转塔上的Die总数小于DieToTray工位Tray盘中剩余空穴数量，则不用请求上空Tray
                    {
                        ret = false;
                    }
                }
                else
                {
                    ShowMsg($"{StationName}->DieToTray工位Tray盘和当前Bin等级不匹配，Tray盘Bin等级：Bin{trayInfo.TrayModel.BinLevel}");
                }
            }
            else
            {
                ShowMsg($"{StationName}->DieToTray工位没有Tray盘");
            }
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
            var stn = StationManager.Instance.GetChild<Stn4TurnTableStn>(nameof(Stn4TurnTableStn));
            stn.ResetRegisterEvent += Stn_ResetRegisterEvent;
        }

        private void Stn_ResetRegisterEvent(RegisterEnum obj)
        {
            if (obj == RegisterEnum.Stn4DDRotateDone)
            {
                LastRegisterValue[obj.ToInt()] = false;
            }
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
