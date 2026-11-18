using JC.PTIS.Main.AlarmInfo;
using JC.PTIS.Main.Devices.Vision;
using JC.PTIS.Main.Devices.VisionPC.MsgModel;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.Models.ModelHelper;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Params;
using JC.PTIS.Main.RotateInfo;
using JC.PTIS.Main.Service;
using Newtonsoft.Json.Linq;
using StdLib.PCBaseFrame.Alarm;
using StdLib.PCBaseFrame.Args;
using StdLib.PCBaseFrame.Entities;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.PCBaseFrame.Extension.Entities;
using StdLib.PCBaseFrame.Extension.Recipe;
using StdLib.PCBaseFrame.Motion;
using StdLib.PCBaseFrame.Param;
using StdLib.PCBaseFrame.Station;
using StdLib.WindowsControls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Stations
{
    public class PickPatchStn : StationBaseEx, IStation
    {
        public int StationNum { get; set; } = 10;
        public string StationName { get; set; } = "PickPatchStn";

        List<AxisName> Axes = new List<AxisName>();
        List<PosName> PosNames = new List<PosName>();//位置名称集合
        List<double> Poses = new List<double>();//实际位置集合


        #region 子流程状态机步骤
        int ModuleMoveStep = -1;
        int DetectStep = -1;
        int PickStep = -1;
        int PatchStep = -1;
        int WaitDetectResultStep = -1;
        #endregion
        int CurPickNo = -1;
        int CurFlyGrabRowNo = -1;//飞拍行号=Tray盘的行号
        int CurDieNo = -1;
        int PPCounter = 0;
        int RotateNo = 0;
        // int LastRotateNo = 0;
        int RemainNGDieCount = 0;
        (int R, int C) EmptyCellRCNo = (-1, -1);//Tray中空穴位的行列号

        bool IsTrayAligned = false;//Tray是否对位完成
        bool IsFrontDetectEnd = false;
        double[] PickPatchDDPos = new double[16];

        Xyt BasePos = new Xyt();//基准位置
        Xyt FirstCellPos = new Xyt();//Tray第一个穴位位置
        StageInfo CurStage = null;
        TrayStageInfo CurTrayInfo = null;
        List<DieModel> NGDieList = null;

        Random Rdm = new Random();
        public void Home()
        {
            switch (HomeStep)
            {
                case 1:
                    {
                        RotateNo = 0;
                        ShowMsg($"{StationName}->开始回零");
                        Motion.Home(AxisName.PickPatch_PickZ);
                        Motion.Home(AxisName.PickPatch_PlaceZ);
                        Motion.Home(AxisName.PickPatch_CamZ);
                        ShowMsg($"{StationName}->{AxisName.PickPatch_PickZ}轴、{AxisName.PickPatch_PlaceZ}轴、{AxisName.PickPatch_CamZ}轴开始回零");
                        HomeStep = 50;
                        break;
                    }
                case 50:
                    {
                        if (Motion.IsHomeDone(AxisName.PickPatch_PickZ) &&
                            Motion.IsHomeDone(AxisName.PickPatch_PlaceZ) &&
                            Motion.IsHomeDone(AxisName.PickPatch_CamZ))
                        {
                            ShowMsg($"{StationName}->{AxisName.PickPatch_PickZ}轴、{AxisName.PickPatch_PlaceZ}轴、{AxisName.PickPatch_CamZ}轴回零完成");
                            Motion.Home(AxisName.PickPatch_CamY);
                            ShowMsg($"{StationName}->{AxisName.PickPatch_CamY}轴开始回零");
                            HomeStep = 100;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.PickPatch_PickZ))
                        {
                            ShowMsg($"{StationName}->PickPatch_PickZ轴回零超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.PickPatch_PlaceZ))
                        {
                            ShowMsg($"{StationName}->PickPatch_PlaceZ轴回零超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.PickPatch_CamZ))
                        {
                            ShowMsg($"{StationName}->PickPatch_CamZ轴回零超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 100:
                    {
                        if (Motion.IsHomeDone(AxisName.PickPatch_CamY))
                        {
                            ShowMsg($"{StationName}->{AxisName.PickPatch_CamY}轴回零完成");
                            Motion.MoveAbs(AxisName.PickPatch_CamY, PosName.WaitPos);
                            ShowMsg($"{StationName}->{AxisName.PickPatch_CamY}轴移动到{PosName.WaitPos}位");

                            HomeStep = 150;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.PickPatch_CamY))
                        {
                            ShowMsg($"{StationName}->PickPatch_CamY轴回零超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 150:
                    {
                        if (Motion.IsArrive(AxisName.PickPatch_CamY, PosName.WaitPos))
                        {
                            ShowMsg($"{StationName}->{AxisName.PickPatch_CamY}轴到达{PosName.WaitPos}位");
                            Motion.Home(AxisName.PickPatch_DD);
                            ShowMsg($"{StationName}->{AxisName.PickPatch_DD}轴开始回零");
                            HomeStep = 200;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.PickPatch_CamY))
                        {
                            ShowMsg($"{StationName}->PickPatch_CamY移动到{PosName.WaitPos}位超时", MsgType.Error);
                            HomeStep = 1000;
                        }
                        break;
                    }
                case 200:
                    {
                        if (Motion.IsHomeDone(AxisName.PickPatch_DD))
                        {
                            ShowMsg($"{StationName}->{AxisName.PickPatch_DD}轴回零完成");
                            ShowMsg($"{StationName}->回零完成");
                            RotateInfoHelper.Instance.UpdateRotateInfo(RotateStnType.PickPatch, 0);
                            UpdateHomeSts(StationName, StationNum, true);
                            HomeStep = 0;
                        }
                        else if (Motion.IsMoveTimeOut(AxisName.PickPatch_DD))
                        {
                            ShowMsg($"{StationName}->PickPatch_DD轴回零超时", MsgType.Error);
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
                            ShowMsg($"{StationName}->正面检测模组移动到等待位");
                            ModuleMoveStep = 1;
                            AutoRunStep = 50;
                            break;
                        }
                    case 50:
                        {
                            ModuleMoveStep = MoveToWaitPosFlow(ModuleMoveStep);
                            if (ModuleMoveStep == 0)
                            {
                                ShowMsg($"{StationName}->正面检测模组到达待机位，通知4工位转盘可以转动");
                                AutoRunStep = 100;
                            }
                            break;
                        }
                    case 100:
                        {
                            RegisterService.Instance.SetBoolReg(RegisterEnum.PickPatchStnSafe);
                            ShowMsg($"{StationName}->等待4工位转盘转动到位");
                            AutoRunStep = 150;
                            break;
                        }
                    case 150:
                        {
                            if (GetBoolReg(RegisterEnum.Stn4DDRotateDone, true))
                            {
                                var stageNo = RotateInfoHelper.Instance.GetStageNo(Stn4TurnTableStnEnum.PickPatchStn.ToInt(), RotateStnType.Stn4TurnTable);
                                CurStage = StageManager.Get(stageNo);
                                ShowMsg($"{StationName}->4工位转盘转动到位，判断Tray{stageNo}平台是否有Tray");
                                if (Motion.GetDigitalInput(CurStage.IsExistTray))
                                {
                                    var temp = ProcInfoHelper.Instance.GetTrayInfo(Stn4TurnTableStnEnum.PickPatchStn);
                                    if (temp != null && temp.TrayModel != null)
                                    {
                                        CurTrayInfo = temp.DeepCopy();
                                        if (CurTrayInfo.TrayModel.TrayStatus == TrayStatusEnum.NoDetectFull ||
                                             CurTrayInfo.TrayModel.TrayStatus == TrayStatusEnum.NoDetectHalf)
                                        {
                                            var value = false;//是否执行正面检
                                            //var wafer = ProcInfoHelper.Instance.GetWaferModel(WaferPosEnum.WaferTable);
                                            bool isLastWafer = false;
                                            ShowMsg($"{StationName}->Tray{stageNo}平台Tray等级：Bin{CurTrayInfo.TrayModel.BinLevel}，类型：{CurTrayInfo.TrayModel.TrayType}，状态：{CurTrayInfo.TrayModel.TrayStatus}，是否最后一枚Wafer：{isLastWafer}");
                                            if (CurTrayInfo.TrayModel.BinLevel % 2 == 1)//是否是Bin135等级
                                            {
                                                if (CurTrayInfo.TrayModel.TrayType == TrayTypeEnum.Full ||
                                                    (CurTrayInfo.TrayModel.TrayType == TrayTypeEnum.Half &&
                                                    isLastWafer))
                                                {
                                                    value = true;
                                                    ShowMsg($"{StationName}->Tray{stageNo}平台Bin等级属于1,3,5。执行正面检流程");
                                                }
                                                else if (CurTrayInfo.TrayModel.TrayType == TrayTypeEnum.Half)
                                                {
                                                    ShowMsg($"{StationName}->Tray{stageNo}平台Bin等级属于1,3,5，并且是半Tray，不做正面检测");
                                                }
                                            }
                                            else
                                            {
                                                if (CurTrayInfo.TrayModel.TrayType == TrayTypeEnum.Half)
                                                {
                                                    ShowMsg($"{StationName}->Tray{stageNo}平台Bin等级是2,4,6，并且是半Tray，不做正面检测");
                                                }
                                            }
                                            if (value)
                                            {
                                                DetectStep = 1;
                                                AutoRunStep = 200;
                                                //if (Global.RunMode == RunMode.空跑模式)
                                                //{
                                                //    WaitDetectResultStep = 1;
                                                //    AutoRunStep = 250;
                                                //}
                                            }
                                            else
                                            {
                                                AutoRunStep = 100;
                                            }
                                        }
                                        else if (CurTrayInfo.TrayModel.TrayStatus == TrayStatusEnum.OkHalf ||
                                            CurTrayInfo.TrayModel.TrayStatus == TrayStatusEnum.OkFull)
                                        {
                                            ShowMsg($"{StationName}->Tray{stageNo}平台正面检已挑补完");
                                            AutoRunStep = 100;
                                        }
                                        else if (CurTrayInfo.TrayModel.TrayStatus == TrayStatusEnum.Empty)
                                        {
                                            ShowMsg($"{StationName}->Tray{stageNo}平台是空Tray");
                                            AutoRunStep = 100;
                                        }
                                        else
                                        {
                                            ShowMsg($"{StationName}->Tray{stageNo}平台正面检已检测完，判断有无NG Die");
                                            if (CurTrayInfo.TrayModel.IsExistNGDie)
                                            {
                                                CalcNgDieCount(CurTrayInfo.TrayModel);//统计Tray中NG Die数量
                                            }
                                            else
                                            {
                                                RegisterService.Instance.SetBoolReg(RegisterEnum.FrontDetectNoExistNgDie);
                                            }
                                            PickStep = 1;
                                            PPCounter = 0;
                                            CurDieNo = 0;
                                            RegisterService.Instance.SetBoolReg(RegisterEnum.PickFlowStart);
                                            AutoRunStep = 300;
                                        }
                                    }
                                    else
                                    {
                                        ShowMsg($"{StationName}->从PickPatchStn平台获取Tray信息失败", MsgType.Error, true);
                                        AlarmManager.CreateAlarm(AlarmCode.GetTrayInfoFail.ToInt(), AllStationAlarm);
                                    }
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->Tray{stageNo}平台没有Tray，通知4工位转盘可以转动");
                                    AutoRunStep = 100;
                                }
                            }
                            break;
                        }
                    case 200:
                        {
                            DetectStep = FrontDetectFlow(DetectStep);
                            if (DetectStep == 0)
                            {
                                ShowMsg($"{StationName}->正面检取图流程执行完成，等待检测PC返回检测结果");
                                WaitDetectResultStep = 1;
                                Delay.Start(ParaHelper.Vision.FrontDetectTimeout);
                                AutoRunStep = 250;
                            }
                            else if (DetectStep == -1)
                            {
                                ShowMsg($"{StationName}->正面检测流程错误", MsgType.Error, true);
                                var ret = AskHowToDoRequest(StationNum, $"{StationName}->正面检测流程错误，弹窗询问如何处理");
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
                                            ShowMsg($"{StationName}->处理结果：跳过。通知4工位转盘可以转动");
                                            AutoRunStep = 100;
                                            break;
                                        }
                                    default:
                                        break;
                                }
                            }
                            break;
                        }
                    case 250:
                        {
                            var result = DetectHelper.Instance.GetDetectionResult(DetectStationEnum.FrontDetect);
                            if (result != null)
                            {
                                UpdateDetectResult(result);
                                CurTrayInfo = ProcInfoHelper.Instance.GetTrayInfo(Stn4TurnTableStnEnum.PickPatchStn);
                                ShowMsg($"{StationName}->检测PC返回检测结果完成，判断是否有NG");
                                if (CurTrayInfo.TrayModel.IsExistNGDie)
                                {
                                    CalcNgDieCount(CurTrayInfo.TrayModel);
                                    ShowMsg($"{StationName}->通知HalfTray工位切换到{(BinLevel)CurTrayInfo.TrayModel.BinLevel}等级Tray");
                                    PickStep = 1;
                                    RegisterService.Instance.SetBoolReg(RegisterEnum.PickFlowStart);
                                    PPCounter = 0;
                                    CurDieNo = 0;
                                    AutoRunStep = 300;
                                }
                                else
                                {
                                    CurTrayInfo = ProcInfoHelper.Instance.GetTrayInfo(Stn4TurnTableStnEnum.PickPatchStn);
                                    RegisterService.Instance.SetBoolReg(RegisterEnum.FrontDetectNoExistNgDie);
                                    ShowMsg($"{StationName}->Tray中没有NG Die，跳过挑补流程");
                                    AutoRunStep = 100;
                                }
                            }
                            else if (Delay.TimeUp())
                            {
                                var ret = AskHowToDoRequest(StationNum, $"{StationName}->等待检测PC返回正面检测结果超时，请选择如何处理", "强制判NG");
                                switch (ret)
                                {
                                    case AskResult.Retry:
                                        {
                                            ShowMsg($"{StationName}->处理结果：重试");
                                            Delay.Start(ParaHelper.Vision.FrontDetectTimeout);
                                            break;
                                        }
                                    case AskResult.Skip:
                                        {
                                            ShowMsg($"{StationName}->处理结果：跳过。通知4工位转盘可以转动");
                                            AutoRunStep = 100;
                                            break;
                                        }
                                    case AskResult.Other:
                                        {
                                            ForceNg();
                                            ShowMsg($"{StationName}->处理结果：强制判NG");
                                            CalcNgDieCount(CurTrayInfo.TrayModel);
                                            ShowMsg($"{StationName}->通知HalfTray工位切换到{(BinLevel)CurTrayInfo.TrayModel.BinLevel}等级Tray");
                                            PickStep = 1;
                                            RegisterService.Instance.SetBoolReg(RegisterEnum.PickFlowStart);
                                            PPCounter = 0;
                                            CurDieNo = 0;
                                            AutoRunStep = 300;
                                            break;
                                        }
                                    default:
                                        break;
                                }
                            }
                            break;
                        }
                    case 300:
                        {
                            PickStep = PickFlow(PickStep, (BinLevel)CurTrayInfo.TrayModel.BinLevel);
                            if (PickStep == 0)
                            {
                                ShowMsg($"{StationName}->挑Die流程结束。执行补Die流程，等级：{(BinLevel)CurTrayInfo.TrayModel.BinLevel}", MsgType.Notify);
                                RegisterService.Instance.SetBoolReg(RegisterEnum.PatchFlowStart);
                                PatchStep = 1;
                                PPCounter = 0;
                                AutoRunStep = 350;
                            }
                            break;
                        }
                    case 350:
                        {
                            PatchStep = PatchFlow(PatchStep);
                            if (PatchStep == 0)
                            {
                                ResetBoolReg(RegisterEnum.PatchFlowReady);
                                ShowMsg($"{StationName}->补Die流程结束。判断正面检工位Tray中是否还有NG Die");
                                if (RemainNGDieCount > 0)
                                {
                                    ShowMsg($"{StationName}->正面检工位Tray中还有NG Die，执行挑Die流程");
                                    PickStep = 1;
                                    RegisterService.Instance.SetBoolReg(RegisterEnum.PickFlowStart);
                                    AutoRunStep = 300;
                                }
                                else
                                {
                                    ResetBoolReg(RegisterEnum.OkHalfTrayEmpty);
                                    ShowMsg($"{StationName}->正面检工位Tray中NG Die全部挑补完，等待HalfTray工位确认是否要暂存半Tray");
                                    CurTrayInfo = ProcInfoHelper.Instance.GetTrayInfo(Stn4TurnTableStnEnum.PickPatchStn);
                                    RemainNGDieCount = 0;
                                    CurDieNo = 0;
                                    AutoRunStep = 400;
                                }
                            }
                            break;
                        }
                    case 400:
                        {
                            if (GetBoolReg(RegisterEnum.HalfTrayConfirmDone))
                            {
                                ResetBoolReg(RegisterEnum.HalfTrayConfirmDone);
                                ShowMsg($"{StationName}->HalfTray工位确认是否要暂存半Tray完成");
                                ModuleMoveStep = 1;
                                AutoRunStep = 50;
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
                        RotateNo = 0;
                        ShowMsg($"{StationName}->正面检模组移动到待机位开始");
                        Axes = new List<AxisName>() { AxisName.PickPatch_PickZ, AxisName.PickPatch_PlaceZ, AxisName.PickPatch_CamZ };
                        PosNames = new List<PosName>() { PosName.WaitPos, PosName.WaitPos, PosName.FrontDetectPos };
                        MoveAbsM(Axes, PosNames);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            MoveAbs(AxisName.PickPatch_CamY, PosName.WaitPos);
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
                        var ret = IsAxisArrive(AxisName.PickPatch_CamY, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            MoveAbs(AxisName.PickPatch_DD, PickPatchDDPos[RotateNo]);
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
                        var ret = IsAxisArrive(AxisName.PickPatch_DD, PickPatchDDPos[RotateNo]);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->正面检模组移动到待机位结束");
                            RotateInfoHelper.Instance.UpdateRotateInfo(RotateStnType.PickPatch, PickPatchDDPos[RotateNo]);
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 100;
                        }
                        break;
                    }
                default:
                    break;
            }
            return step;
        }
        public int FrontDetectFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->正面检取图流程开始，PickPatch_CamY轴移动到GrabStartPos", MsgType.Notify);
                        //记录当前TrayStage位置为基准位置
                        BasePos = new Xyt()
                        {
                            X = Motion.GetAxisActPos(CurStage.X),
                            Y = Motion.GetAxisActPos(CurStage.Y),
                            T = Motion.GetAxisActPos(CurStage.T)
                        };
                        MoveAbs(AxisName.PickPatch_CamY, PosName.GrabStartPos);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArrive(AxisName.PickPatch_CamY, PosName.GrabStartPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            //var wafer = ProcInfoHelper.Instance.GetWaferModel(WaferPosEnum.WaferTable);
                            var tray = ProcInfoHelper.Instance.GetTrayInfo(Stn4TurnTableStnEnum.PickPatchStn);

                            var req = new TriggerDetectReqBody();
                            if (tray.TrayModel != null && tray.TrayModel.DieArray.Length > 0)
                            {
                                var dieModel = tray.TrayModel.DieArray[0, 0];
                                string lotID = string.Empty;
                                if (string.IsNullOrEmpty(dieModel.DieCode) == false)
                                {
                                    var arr = dieModel.DieCode.Split("_");
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
                                int index = 0;
                                List<Product> prodList = new List<Product>();
                                for (int i = 0; i < tray.TrayModel.DieArray.GetLongLength(0); i++)
                                {
                                    for (int j = 0; j < tray.TrayModel.DieArray.GetLongLength(1); j++)
                                    {
                                        Product product = new Product();
                                        product.Index = index;
                                        if (tray.TrayModel.DieArray[i, j] != null)
                                        {
                                            product.SN = tray.TrayModel.DieArray[i, j].DieCode;
                                        }
                                        prodList.Add(product);
                                        index++;
                                    }
                                }
                                req.ProductList = prodList.ToArray();
                            }
                            var body = DetectHelper.Instance.Detect<TriggerDetectRspBody>(VisionCmd.TriggerDetection, DetectStationEnum.FrontDetect, req);
                            if (body != null && body.Code == 0)
                            {
                                ShowMsg($"{StationName}->检测PC正面检测准备完成");
                                CurFlyGrabRowNo = 0;
                                step = 100;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->检测PC正面检测准备失败", MsgType.Error);
                                AlarmManager.CreateAlarm(AlarmCode.DetectPCFrontDetectReadyFail.ToInt(), StationNum);
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 50;
                        }
                        break;
                    }
                case 100:
                    {
                        if (CurFlyGrabRowNo < ParaHelper.Prod.TrayRowCount)
                        {
                            ShowMsg($"{StationName}->{CurStage.X}轴移动到第{CurFlyGrabRowNo}行飞拍位置");
                            Axes = new List<AxisName>() { CurStage.X };
                            Poses = new List<double>() { GetPerRowPos(CurFlyGrabRowNo) };
                            MoveAbsM(Axes, Poses);
                            step = 150;
                        }
                        else
                        {
                            ShowMsg($"{StationName}->飞拍取图结束，Tray移动到基准位置");
                            MoveAbs(AxisName.PickPatch_CamY, PosName.GrabTrayPos);
                            Axes = new List<AxisName>() { CurStage.X, CurStage.Y };
                            Poses = BasePos.ToList();
                            MoveAbsM(Axes, Poses);
                            step = 350;
                        }
                        break;
                    }
                case 150:
                    {
                        var ret = IsAxisArriveM(Axes, Poses);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->开始飞拍");
                            Motion.CompareStop(CompareName.FrontDetectGrab);
                            Motion.SetCompare(CompareName.FrontDetectGrab);
                            MoveAbs(AxisName.PickPatch_CamY, PosName.GrabEndPos);
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
                        var info = Motion.GetCompare(CompareName.FrontDetectGrab, out int triggerCoun, out short status);
                        //ShowMsg($"{StationName}->已触发次数：{triggerCoun}");
                        var ret = IsAxisArrive(AxisName.PickPatch_CamY, PosName.GrabEndPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->第{CurFlyGrabRowNo}行飞拍结束");
                            CurFlyGrabRowNo++;
                            if (CurFlyGrabRowNo < ParaHelper.Prod.TrayRowCount)
                            {
                                ShowMsg($"{StationName}->{CurStage.X}轴移动到第{CurFlyGrabRowNo}行飞拍位置");
                                Axes = new List<AxisName>() { CurStage.X };
                                Poses = new List<double>() { GetPerRowPos(CurFlyGrabRowNo) };
                                MoveAbsM(Axes, Poses);
                                step = 250;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->飞拍取图结束，Tray移动到基准位置");
                                MoveAbs(AxisName.PickPatch_CamY, PosName.GrabTrayPos);
                                Axes = new List<AxisName>() { CurStage.X, CurStage.Y };
                                Poses = BasePos.ToList();
                                MoveAbsM(Axes, Poses);
                                step = 350;
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 100;
                        }
                        break;
                    }
                case 250:
                    {
                        var ret = IsAxisArriveM(Axes, Poses);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->开始飞拍");
                            Motion.CompareStop(CompareName.FrontDetectGrab);
                            Motion.SetCompare(CompareName.FrontDetectGrab);
                            MoveAbs(AxisName.PickPatch_CamY, PosName.GrabStartPos);
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
                        var info = Motion.GetCompare(CompareName.FrontDetectGrab, out int triggerCoun, out short status);
                        //ShowMsg($"{StationName}->已触发次数：{triggerCoun}");
                        var ret = IsAxisArrive(AxisName.PickPatch_CamY, PosName.GrabStartPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->第{CurFlyGrabRowNo}行飞拍结束");
                            CurFlyGrabRowNo++;
                            step = 100;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 250;
                        }
                        break;
                    }
                case 350:
                    {
                        var ret = IsAxisArriveM(Axes, Poses);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->正面检测取图流程结束，Tray移动到基准位置完成", MsgType.Notify);
                            CurFlyGrabRowNo = 0;
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


        /// <summary>
        /// 取Die流程
        /// </summary>
        /// <param name="step"></param>
        /// <param name="binLevel"></param>
        /// <returns></returns>
        public int PickFlow(int step, BinLevel binLevel)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->挑Die流程开始，等级：{binLevel}", MsgType.Notify);
                        if (IsTrayAligned)
                        {
                            ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}已对位过，移动到OkTray基准位置");
                            Axes = new List<AxisName>() { CurStage.X, CurStage.Y, CurStage.T };
                            Poses = BasePos.ToList();
                            MoveAbsM(Axes, Poses);
                            step = 20;
                        }
                        else
                        {
                            ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}移动到拍照位置");
                            Axes = new List<AxisName>() { CurStage.X, CurStage.Y, CurStage.T, AxisName.PickPatch_CamY, AxisName.PickPatch_CamZ };
                            PosNames = new List<PosName>() { PosName.GrabTrayPos };
                            MoveAbsM(Axes, PosNames);
                            step = 50;
                        }
                        break;
                    }
                case 20:
                    {
                        var ret = IsAxisArriveM(Axes, Poses);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}到达基准位置，等待HalfTray工位切换Tray完成");
                            step = 200;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 1;
                        }
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
							ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}到达拍照位置，发送AlignTray请求给AlignPC，Station：Tray_PickPatch");
							var body = AlignHelper.Instance.Send<AlignRspBody>(VisionCmd.Align, AlignStationEnum.Tray_PickPatch);
							if (body != null && body.Offset != null)
                            {
                                ShowMsg($"{StationName}->对位PC返回坐标： {body.Offset.ToString()}");
                                Axes = new List<AxisName>() { CurStage.X, CurStage.Y, CurStage.T };
                                Poses = body.Offset.ToList();
                                MoveRelM(Axes, Poses);
                                ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}移动补正值");
                                step = 100;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->AlignPC Tray对位失败", MsgType.Error);
                                AlarmManager.CreateAlarm(AlarmCode.TrayAlignFail.ToInt(), StationNum);
                                var result = AskHowToDoRequest(StationNum, $"{StationName}->AlignPC Tray对位失败，请选择如何处理");
                                switch (result)
                                {
                                    case AskResult.Retry:
                                        {
                                            ShowMsg($"{StationName}->处理结果：重试");
                                            step = 50;
                                            break;
                                        }
                                    case AskResult.Skip:
                                        {
                                            ShowMsg($"{StationName}->处理结果：不对位Tray，继续流程");
                                            step = 100;
                                            break;
                                        }
                                    default:
                                        break;
                                }
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
                        var ret = IsAxisArriveM(Axes);
                        if (ret == MotionStatus.Arrived)
                        {
                            IsTrayAligned = true;
                            ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}移动补正值完成，相机模组移动到GrabDiePos");
                            CalcFirstDieBasePos();
                            Axes = new List<AxisName>() { AxisName.PickPatch_CamZ, AxisName.PickPatch_CamY };
                            PosNames = new List<PosName>() { PosName.GrabDiePos, PosName.GrabDiePos };
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
                            ShowMsg($"{StationName}->相机模组到达GrabDiePos，等待HalfTray工位切换Tray完成");
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
                        if (GetBoolReg(RegisterEnum.ShiftHalfTrayDone))
                        {
                            ResetBoolReg(RegisterEnum.ShiftHalfTrayDone);
                            ShowMsg($"{StationName}->HalfTray工位切换到{binLevel}等级Tray盘完成，开始取NG Die");
                            PPCounter = 0;
                            step = 250;
                        }
                        break;
                    }
                case 250:
                    {
                        RegisterService.Instance.SetBoolReg(RegisterEnum.PickPatchDDRotateDone);
                        CurPickNo = RotateInfoHelper.Instance.GetStageNo(PickPatchStnEnum.FrontStn.ToInt(), RotateStnType.PickPatch);
                        var isExist = false;
                        var pickInfo = ProcInfoHelper.Instance.GetPickerInfo(PickPatchStnEnum.FrontStn, RotateStnType.PickPatch);
                        isExist = IsNozzleVacArrive(CurPickNo) && pickInfo.DieModel != null;
                        if (isExist == false)
                        {
                            NozzleVacuum(CurPickNo, false);
                            ShowMsg($"{StationName}->DD马达转动到位，挑Die吸嘴：{CurPickNo}无料，挑补Die序号：{CurDieNo}");
                            ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}移动到第{NGDieList[CurDieNo].InTrayRowNo}行，第{NGDieList[CurDieNo].InTrayColNo}列位置");
                            Axes = new List<AxisName>() { CurStage.X, CurStage.Y, CurStage.T };
                            Poses = GetPerCellPos(NGDieList[CurDieNo].InTrayRowNo, NGDieList[CurDieNo].InTrayColNo).ToList();
                            MoveAbsM(Axes, Poses);
                            step = 300;
                        }
                        else
                        {
                            ShowMsg($"{StationName}->DD马达转动到位，挑Die吸嘴：{CurPickNo}有料，Z轴移动到待机位");
                            MoveAbs(AxisName.PickPatch_PickZ, PosName.WaitPos);
                            PPCounter++;
                            step = 400;
                        }
                        break;
                    }
                case 300:
                    {
                        var ret = IsAxisArriveM(Axes, Poses);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}到达第{NGDieList[CurDieNo].InTrayRowNo}行，第{NGDieList[CurDieNo].InTrayColNo}列位置");
                            MoveAbs(AxisName.PickPatch_PickZ, PosName.PickDiePos);
                            NozzleVacuum(CurPickNo, true);
                            Delay.Start(1000);
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
                        var ret = IsAxisArrive(AxisName.PickPatch_PickZ, PosName.PickDiePos);
                        if (ret == MotionStatus.Arrived)
                        {
                            if (IsNozzleVacArrive(CurPickNo))
                            {
                                ShowMsg($"{StationName}->吸嘴：{CurPickNo}吸真空完成，Z轴移动到待机位");
                                var dieModel = ProcInfoHelper.Instance.PickDieFromTray(Stn4TurnTableStnEnum.PickPatchStn, NGDieList[CurDieNo].InTrayRowNo, NGDieList[CurDieNo].InTrayColNo);
                                if (dieModel != null)
                                {
                                    PPCounter++;
                                    CurDieNo++;
                                    ProcInfoHelper.Instance.PlaceDieToPicker(PickPatchStnEnum.FrontStn, dieModel, RotateStnType.PickPatch);
                                    MoveAbs(AxisName.PickPatch_PickZ, PosName.WaitPos);
                                    step = 400;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->从正面检Tray盘获取DieModel信息失败，行号：{NGDieList[CurDieNo].InTrayRowNo}，列号：{NGDieList[CurDieNo].InTrayColNo}", MsgType.Error, true);
                                    AlarmManager.CreateAlarm(AlarmCode.GetDieModelFail.ToInt(), StationNum);
                                }
                            }
                            else if (Delay.TimeUp())
                            {
                                ShowMsg($"{StationName}->PickPatchStn{CurPickNo}吸真空失败", MsgType.Error);
                                var ret2 = AskHowToDoRequest(StationNum, $"{StationName}->PickPatchStn{CurPickNo}吸真空失败，请选择如何处理");
                                switch (ret2)
                                {
                                    case AskResult.Retry:
                                        {
                                            ShowMsg($"{StationName}->处理结果：重试");
                                            NozzleVacuum(CurPickNo, true);
                                            Delay.Start(1000);
                                            break;
                                        }
                                    case AskResult.Skip:
                                        {
                                            ShowMsg($"{StationName}->处理结果：跳过");
                                            step = 400;
                                            break;
                                        }
                                    default:
                                        break;
                                }
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 300;
                        }
                        break;
                    }
                case 400:
                    {
                        var ret = IsAxisArrive(AxisName.PickPatch_PickZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->Z轴到达待机位，等待HalfTray工位PickDie完成");
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
                        if (GetBoolReg(RegisterEnum.HalfTrayPickOneDieDone) || GetBoolReg(RegisterEnum.OkHalfTrayEmpty))
                        {
                            ResetBoolReg(RegisterEnum.HalfTrayPickOneDieDone);
                            ShowMsg($"{StationName}->HalfTray工位取单颗NG Die完成，判断转盘是否可以转动，CurDieNo:{CurDieNo}，NGCount：{NGDieList.Count}，PPCounter:{PPCounter}");
                            if (CurDieNo < NGDieList.Count && PPCounter < 4)
                            {
                                RotateNo++;//DD马达旋转22.5度，取下一颗Die    
                            }
                            else
                            {
                                RotateNo = 8;//DD马达旋转到180度位置，准备补Die
                            }
                            step = 500;
                        }
                        break;
                    }
                case 500:
                    {
                        if (IsDDCanRotate())
                        {
                            MoveAbs(AxisName.PickPatch_DD, PickPatchDDPos[RotateNo]);
                            step = 550;
                        }
                        break;
                    }
                case 550:
                    {
                        var ret = IsDDArrive();
                        if (ret == MotionStatus.Arrived)
                        {
                            if (RotateNo >= 16)
                            {
                                RotateNo = 0;
                            }
                            CurPickNo = RotateInfoHelper.Instance.GetStageNo(PickPatchStnEnum.FrontStn.ToInt(), RotateStnType.PickPatch);
                            if (CurDieNo < NGDieList.Count && PPCounter < 4)
                            {
                                step = 250;
                            }
                            else if (CurDieNo == NGDieList.Count || PPCounter == 4)//正面检挑Die完成
                            {
                                ShowMsg($"{StationName}->挑Die流程结束，发送挑Die结束给挑补工位", MsgType.Notify);
                                RegisterService.Instance.SetBoolReg(RegisterEnum.PickFlowEnd);
                                PPCounter = 0;
                                step = 0;
                            }
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
        /// 补Die流程
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int PatchFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        if (GetBoolReg(RegisterEnum.PatchFlowReady))
                        {
                            CurPickNo = RotateInfoHelper.Instance.GetStageNo(PickPatchStnEnum.FrontStn.ToInt(), RotateStnType.PickPatch);
                            ShowMsg($"{StationName}->DD马达转动到位");
                            RegisterService.Instance.SetBoolReg(RegisterEnum.PickPatchDDRotateDone);
                            var isExist = false;
                            var pickerInfo = ProcInfoHelper.Instance.GetPickerInfo(PickPatchStnEnum.FrontStn, RotateStnType.PickPatch);
                            isExist = IsNozzleVacArrive(CurPickNo) && pickerInfo.DieModel != null;
                            if (isExist)
                            {
                                ShowMsg($"{StationName}->补Die吸嘴：{CurPickNo}有料");
                                EmptyCellRCNo = ProcInfoHelper.Instance.GetAvlEmptyCellNoFromTray(Stn4TurnTableStnEnum.PickPatchStn);
                                if (EmptyCellRCNo.R != -1 && EmptyCellRCNo.C != -1)
                                {
                                    ShowMsg($"{StationName}->获取可放Die的空穴位行列号，行：{EmptyCellRCNo.R}，列：{EmptyCellRCNo.C}");
                                    var xyt = GetPerCellPos(EmptyCellRCNo.R, EmptyCellRCNo.C);
                                    Axes = new List<AxisName>() { CurStage.X, CurStage.Y, CurStage.T };
                                    Poses = xyt.ToList();
                                    ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}移动到空穴位位置");
                                    MoveAbsM(Axes, Poses);
                                    step = 50;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->正面检Tray盘已经放满", MsgType.Error);
                                    step = 0;
                                }
                            }
                            else if (GetBoolReg(RegisterEnum.OkHalfTrayEmpty))
                            {
                                ShowMsg($"{StationName}->Ok半Tray为空");
                                step = 300;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->补Die吸嘴：{CurPickNo}无料，Z轴移动待机位");
                                NozzleVacuum(CurPickNo, false);
                                MoveAbs(AxisName.PickPatch_PlaceZ, PosName.WaitPos);
                                step = 250;
                            }
                        }
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArriveM(Axes, Poses);
                        if (ret == MotionStatus.Arrived)
                        {
							ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}到达空穴位位置，发送AlignDie请求给AlignPC，Station：Die_PickPatch");
							if (ParaHelper.Vision.IsAlignBeforeDieToTray)
							{
								var dieIndex = EmptyCellRCNo.R * ParaHelper.Prod.TrayColCount + EmptyCellRCNo.C;
								var body = AlignHelper.Instance.Send<AlignRspBody>(VisionCmd.Align, AlignStationEnum.Die_PickPatch, dieIndex);
								if (body != null && body.Offset != null)
                                {
                                    ShowMsg($"{StationName}->对位PC返回坐标： {body.Offset.ToString()}");
                                    Axes = new List<AxisName>() { CurStage.X, CurStage.Y, CurStage.T };
                                    Poses = body.Offset.ToList();
                                    MoveRelM(Axes, Poses);
                                    ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}移动补正值");
                                    step = 100;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->AlignPC Die对位失败", MsgType.Error);
                                    AlarmManager.CreateAlarm(AlarmCode.DieAlignFail.ToInt(), StationNum);
                                    var result = AskHowToDoRequest(StationNum, $"{StationName}->AlignPC Die对位失败，请选择如何处理");
                                    switch (result)
                                    {
                                        case AskResult.Retry:
                                            {
                                                ShowMsg($"{StationName}->处理结果：重试");
                                                step = 50;
                                                break;
                                            }
                                        case AskResult.Skip:
                                            {
                                                ShowMsg($"{StationName}->处理结果：不对位Die，继续流程");
                                                step = 100;
                                                break;
                                            }
                                        default:
                                            break;
                                    }
                                }
                            }
                            else
                            {
                                ShowMsg($"{StationName}->不对位Die，放DieZ轴下降到放Die位置");
                                MoveAbs(AxisName.PickPatch_PlaceZ, PosName.PlaceDiePos);
                                step = 150;
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
                        var ret = IsAxisArriveM(Axes);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}移动补正值完成");
                            MoveAbs(AxisName.PickPatch_PlaceZ, PosName.PlaceDiePos);
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
                        var ret = IsAxisArrive(AxisName.PickPatch_PlaceZ, PosName.PlaceDiePos);
                        if (ret == MotionStatus.Arrived)
                        {
                            NozzleVacuum(CurPickNo, false);
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
                        if (IsNozzleVacArrive(CurPickNo) == false)
                        {
                            ShowMsg($"{StationName}->吸嘴：{CurPickNo}破真空完成，Z轴移动到待机位");
                            var dieModel = ProcInfoHelper.Instance.PickDieFromPicker(PickPatchStnEnum.FrontStn, RotateStnType.PickPatch);
                            if (dieModel != null)
                            {
                                ProcInfoHelper.Instance.PlaceDieToTray(Stn4TurnTableStnEnum.PickPatchStn, EmptyCellRCNo.R, EmptyCellRCNo.C, dieModel);
                                MoveAbs(AxisName.PickPatch_PlaceZ, PosName.WaitPos);
                                step = 250;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->从吸嘴：{CurPickNo}获取DieModel信息失败", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.GetDieModelFromPickPatchDDPickerFail.ToInt(), StationNum);
                            }
                        }
                        break;
                    }
                case 250:
                    {
                        var ret = IsAxisArrive(AxisName.PickPatch_PlaceZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->等待HalfTray工位放Die完成");
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
                        if (GetBoolReg(RegisterEnum.HalfTrayPlaceOneDieDone))
                        {
                            ResetBoolReg(RegisterEnum.HalfTrayPlaceOneDieDone);
                            ShowMsg($"{StationName}->HalfTray工位放Die完成，判断转盘是否可以转动");
                            RemainNGDieCount--;
                            PPCounter++;
                            ShowMsg($"{StationName}->剩余NG Die数量：{RemainNGDieCount}");
                            if (RemainNGDieCount == 0 || PPCounter >= 4)
                            {
                                RotateNo = 0;
                                IsTrayAligned = false;
                            }
                            else
                            {
                                RotateNo++;
                            }
                            step = 350;
                        }
                        break;
                    }
                case 350:
                    {
                        if (IsDDCanRotate())
                        {
                            MoveAbs(AxisName.PickPatch_DD, PickPatchDDPos[RotateNo]);
                            step = 400;
                        }
                        break;
                    }
                case 400:
                    {
                        var ret = IsDDArrive();
                        if (ret == MotionStatus.Arrived)
                        {
                            if (RotateNo == 16)
                            {
                                RotateNo = 0;
                            }
                            if (PPCounter < 4 && RemainNGDieCount > 0)
                            {
                                step = 1;
                            }
                            else
                            {
                                PPCounter = 0;
                                if (RemainNGDieCount == 0)
                                {
                                    IsTrayAligned = false;
                                    ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}挑补完成，Tray移动到基准位");
                                    Axes = new List<AxisName>() { CurStage.X, CurStage.Y, CurStage.T };
                                    Poses = BasePos.ToList();
                                    MoveAbsM(Axes, Poses);
                                    step = 450;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->进入下一轮挑Die流程", MsgType.Notify);
                                    step = 0;
                                }
                            }

                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 350;
                        }
                        break;
                    }
                case 450:
                    {
                        var ret = IsAxisArriveM(Axes, Poses);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->Tray移动到基准位完成，补Die流程结束", MsgType.Notify);
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 400;
                        }
                        break;
                    }

                default:
                    break;
            }
            return step;
        }
        private void CalcNgDieCount(TrayModel trayModel)
        {
            var list = trayModel.DieArray.Cast<DieModel>();
            NGDieList = list.Where(d => d != null && (d.DieStatus >= DieStatus.BackNG && d.DieStatus <= DieStatus.FrontNG)).ToList();
            ShowMsg($"{StationName}->Bin{trayModel.BinLevel}等级Tray中NG总数量：{NGDieList.Count}，执行挑Die流程");
            RemainNGDieCount = NGDieList.Count;
            RemainNGDieCount += ProcInfoHelper.Instance.GetPickerNgDieCount();
            RegisterService.Instance.SetIntValue(IntValueEnum.TotalNGDieCount, RemainNGDieCount);
            RegisterService.Instance.SetBoolReg(RegisterEnum.FrontDetectExistNgDie);
        }
        /// <summary>
        /// 计算首颗Die的基准位置
        ///    ______________
        ///   / □                 |
        ///   |                     |
        ///   |                     |
        ///   |                     |
        ///   |                     |
        ///   |                     |
        ///   ¯¯¯¯¯¯¯¯¯¯¯¯¯¯¯
        /// </summary>
        private void CalcFirstDieBasePos()
        {
            BasePos.X = Motion.GetAxisActPos(CurStage.X);
            BasePos.Y = Motion.GetAxisActPos(CurStage.Y);
            BasePos.T = Motion.GetAxisActPos(CurStage.T);
            FirstCellPos.X = BasePos.X + ParaHelper.Prod.FirstCellOffsetX;//计算Tray第一个穴位的X方向坐标
            FirstCellPos.Y = BasePos.Y + ParaHelper.Prod.FirstCellOffsetY;//计算Tray第一个穴位的Y方向坐标
        }
        /// <summary>
        /// 获取每行飞拍的Y轴位置
        /// </summary>
        /// <returns></returns>
        public double GetPerRowPos(int rowNo)
        {
            var pos = Motion.GetAxisActPos(CurStage.X) + rowNo * ParaHelper.Prod.CellPitchY;
            return pos;
        }
        private Xyt GetPerCellPos(int rowNo, int colNo)
        {
            Xyt ret = new Xyt();
            //var x = FirstCellPos.Y + colNo * ParaHelper.Prod.CellPitchX;
            //var y = FirstCellPos.X + rowNo * ParaHelper.Prod.CellPitchY;

            //LoadDie X+，Y -
            //正面检：X -，Y +
            var x = FirstCellPos.X - colNo * ParaHelper.Prod.CellPitchX;
            var y = FirstCellPos.Y + rowNo * ParaHelper.Prod.CellPitchY;
            return ret;
        }
        public void Init()
        {
            for (int i = 0; i < 16; i++)
            {
                PickPatchDDPos[i] = 22.5 * i;
            }
            Init(StationNum, StationName);
        }

        private void NozzleVacuum(int nozzleNo, bool value)
        {
            nozzleNo = 0;
            var ran = new Random();
            VacuumName vacuumName = EnumHelper.GetEnumValue<VacuumName>($"PickPatchNozzle{nozzleNo + 1}Vac");
            if (Global.RunMode == RunMode.空跑模式)
            {
                if (value)
                {
                    IOInfoHelper.GetAnalogInputInfoByName($"PickPatchNozzle{nozzleNo + 1}Value").AIValue = ran.Next(1100, 1500);
                }
                else
                {
                    IOInfoHelper.GetAnalogInputInfoByName($"PickPatchNozzle{nozzleNo + 1}Value").AIValue = ran.Next(100, 200);
                }
                Thread.Sleep(20);
            }
            Vacuum(vacuumName, value);
        }
        private bool IsNozzleVacArrive(int nozzleNo)
        {
            nozzleNo = 0;
            var inputName = $"PickPatchNozzle{nozzleNo + 1}Value";
            //var value = Motion.GetAnalogInput(inputName);
            var value = IOInfoHelper.GetAnalogInputInfoByName(inputName).AIValue;
            return value > 1000;
        }
        private bool IsDDCanRotate()
        {
            if (Motion.IsArrive(AxisName.PickPatch_PickZ, PosName.WaitPos) &&
                Motion.IsArrive(AxisName.PickPatch_PlaceZ, PosName.WaitPos))
            {
                return true;
            }
            else
            {
                //ShowMsg($"{StationName}->条件不满足，禁止转盘转动", MsgType.Error);
            }
            return false;
        }
        private MotionStatus IsDDArrive()
        {
            var ret = IsAxisArrive(AxisName.PickPatch_DD, PickPatchDDPos[RotateNo]);
            if (ret == MotionStatus.Arrived)
            {
                if (PickPatchDDPos[RotateNo] == 360)
                {
                    Motion.ZeroPos(AxisName.PickPatch_DD);
                }
                RotateInfoHelper.Instance.UpdateRotateInfo(RotateStnType.PickPatch, PickPatchDDPos[RotateNo]);
            }
            return ret;
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
            DetectHelper.OnDetectionCompleted += Instance_OnDetectionCompleted;
        }

        private void Stn_ResetRegisterEvent(RegisterEnum obj)
        {
            if (obj == RegisterEnum.Stn4DDRotateDone)
            {
                LastRegisterValue[obj.ToInt()] = false;
            }
        }
        private void Instance_OnDetectionCompleted(DetectionCompletedBody obj)
        {
            // UpdateDetectResult(obj);
        }
        private bool UpdateDetectResult(DetectionCompletedBody obj)
        {
            bool ret = false;
            try
            {
                if (obj.Station == DetectStationEnum.FrontDetect.ToString())
                {
                    if (obj.ItemResultList != null && obj.ItemResultList.Length > 0)
                    {
                        var tray = ProcInfoHelper.Instance.GetTrayInfo(Stn4TurnTableStnEnum.PickPatchStn);
                        var arr = tray.TrayModel.DieArray.Cast<DieModel>().ToArray();
                        for (int i = 0; i < obj.ItemResultList.Length; i++)
                        {
                            var sn = obj.ItemResultList[i].SN;
                            if (string.IsNullOrEmpty(sn) == false)
                            {
                                var index = Array.FindIndex(arr, x => x.DieCode == sn);
                                if (index >= 0)
                                {
                                    if (obj.ItemResultList[i].Result == "NG")
                                    {
                                        arr[index].FrontDetectResult = Result.NG;
                                        if (arr[index].DieStatus != DieStatus.BackNG)
                                        {
                                            arr[index].DieStatus = DieStatus.FrontNG;
                                        }
                                    }
                                    else
                                    {
                                        arr[index].FrontDetectResult = Result.OK;
                                        if (arr[index].DieStatus != DieStatus.BackNG)
                                        {
                                            arr[index].DieStatus = DieStatus.OK;
                                        }
                                    }
                                    arr[index].TotalResult = arr[index].DieStatus == DieStatus.OK ? Result.OK : Result.NG;
                                    DieModelHelper.Update(arr[index]);
                                }
                            }
                        }
                        IsFrontDetectEnd = true;
                        tray.TrayModel.UpdateMap();
                        ProcInfoHelper.Instance.TrayStageInfoUpdated(Stn4TurnTableStnEnum.PickPatchStn, tray);
                    }
                }
            }
            catch (Exception ex)
            {
                ShowMsg($"{StationName}->UpdateDetectResult方法异常：{ex}", MsgType.Error, true);
            }
            return ret;
        }
        private void ForceNg()
        {
            var tray = ProcInfoHelper.Instance.GetTrayInfo(Stn4TurnTableStnEnum.PickPatchStn);
            var arr = tray.TrayModel.DieArray.Cast<DieModel>().ToArray();
            for (int i = 0; i < arr.Length; i++)
            {
                arr[i].DieStatus = DieStatus.FrontNG;
            }
            tray.TrayModel.UpdateMap();
            ProcInfoHelper.Instance.TrayStageInfoUpdated(Stn4TurnTableStnEnum.PickPatchStn, tray);
        }
        public void ResetBits()
        {
            ModuleMoveStep = -1;
            DetectStep = -1;
            PickStep = -1;
            PatchStep = -1;
        }

        public void StopHandle()
        {
        }

        public void WaitFlowDone()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            while (true)
            {
                if (PatchStep < 200 || PatchStep > 300)
                {
                    return;
                }
                else if (sw.ElapsedMilliseconds > 2000)
                {
                    return;
                }
                Thread.Sleep(5);
            }
        }
    }
}
