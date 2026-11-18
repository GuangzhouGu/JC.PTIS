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
using StdLib.PCBaseFrame.Alarm;
using StdLib.PCBaseFrame.Args;
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
    public class DieToTrayStn : StationBaseEx, IStation
    {
        public int StationNum { get; set; } = 8;
        public string StationName { get; set; } = "DieToTrayStn";

        List<AxisName> Axes = new List<AxisName>();
        List<PosName> PosNames = new List<PosName>();//位置名称集合
        List<double> Poses = new List<double>();//实际位置集合

        #region 子流程状态机步骤
        int DieToTrayStep = -1;
        int TrayAlignStep = -1;
        int DieAlignStep = -1;
        #endregion
        int CurRowNo = 0;
        int CurColNo = 0;

        Xyt FirstCellPos = new Xyt();//Tray第一个穴位的坐标
        Xyt BasePos = new Xyt();//基准位置
        StageInfo CurStage = null;
        public void Home()
        {
            switch (HomeStep)
            {
                case 1:
                    {
                        UpdateHomeSts(StationName, StationNum, true);
                        HomeStep = 0;
                        break;
                    }
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
                            RegisterService.Instance.SetBoolReg(RegisterEnum.DieToTrayDone);
                            ShowMsg($"{StationName}->等待4工位转盘转动到位");
                            AutoRunStep = 50;
                            break;
                        }
                    case 50:
                        {
                            if (GetBoolReg(RegisterEnum.Stn4DDRotateDone, true))
                            {
                                var stageNo = RotateInfoHelper.Instance.GetStageNo(Stn4TurnTableStnEnum.DieToTray.ToInt(), RotateStnType.Stn4TurnTable);
                                CurStage = StageManager.Get(stageNo);
                                ShowMsg($"{StationName}->4工位转盘转动到位，判断TrayStage{CurStage.StageNum}是否有Tray");
                                if (Motion.GetDigitalInput(CurStage.IsExistTray))
                                {
                                    ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}有Tray，执行Tray对位流程");
                                    TrayAlignStep = 1;
                                    AutoRunStep = 100;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}没有Tray，通知转盘可以转动");
                                    AutoRunStep = 1;
                                }
                            }
                            break;
                        }
                    case 100:
                        {
                            TrayAlignStep = TrayAlignFlow(TrayAlignStep);
                            if (TrayAlignStep == 0)
                            {
                                ShowMsg($"{StationName}->Tray对位完成，执行DieToTray流程");
                                DieToTrayStep = 1;
                                AutoRunStep = 150;
                            }
                            break;
                        }
                    case 150:
                        {
                            DieToTrayStep = DieToTrayFlow(DieToTrayStep);
                            if (DieToTrayStep == 0)
                            {
                                ShowMsg($"{StationName}->DieToTray流程完成，通知转盘可以转动");
                                CurRowNo = 0;
                                CurColNo = 0;
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
        public int TrayAlignFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->TrayAlign流程开始，TrayStage{CurStage.StageNum}移动到拍照位置", MsgType.Notify);
                        Axes = new List<AxisName>() { CurStage.X, CurStage.Y, CurStage.T };
                        PosNames = new List<PosName>() { PosName.GrabTrayPos };
                        MoveAbsM(Axes, PosNames);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
							ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}到达拍照位置，发送AlignTray请求给AlignPC，Station：{AlignStationEnum.Tray_DieToTray}");
							var body = AlignHelper.Instance.Send<AlignRspBody>(VisionCmd.Align, AlignStationEnum.Tray_DieToTray);
							if (body != null && body.Offset != null)
                            {
                                ShowMsg($"{StationName}->对位PC返回坐标： {body.Offset.ToString()}");
                                Poses = body.Offset.ToList();
                                MoveRelM(Axes, Poses);
                                ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}移动补正值");
                                step = 100;
                            }
                            else
                            {
								ShowMsg($"{StationName}->AlignPC Tray对位失败，命令：{VisionCmd.Align}，工位：{AlignStationEnum.Tray_DieToTray}", MsgType.Error);
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
                                            step = 0;
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
                            BasePos.X = Motion.GetAxisActPos(CurStage.X);
                            BasePos.Y = Motion.GetAxisActPos(CurStage.Y);
                            BasePos.T = Motion.GetAxisActPos(CurStage.T);
                            FirstCellPos.X = BasePos.X + ParaHelper.Prod.FirstCellOffsetX;//计算Tray第一个穴位的X方向坐标
                            FirstCellPos.Y = BasePos.Y + ParaHelper.Prod.FirstCellOffsetY;//计算Tray第一个穴位的Y方向坐标
                            ShowMsg($"{StationName}->TrayAlign流程结束", MsgType.Notify);
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
        public int DieToTrayFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->DieToTray流程开始，判断当前Tray是否放满", MsgType.Notify);
                        var trayStageInfo = ProcInfoHelper.Instance.GetTrayInfo(Stn4TurnTableStnEnum.DieToTray);
                        if (trayStageInfo != null && trayStageInfo.TrayModel != null)
                        {
                            //if (Global.RunMode == RunMode.空跑模式)
                            //{
                            //    for (int i = 0; i < trayStageInfo.TrayModel.RowCount; i++)
                            //    {
                            //        for (int j = 0; j < trayStageInfo.TrayModel.ColCount; j++)
                            //        {
                            //            var dieModel = new DieModel()
                            //            {
                            //                DieStatus = DieStatus.Picked,
                            //                BinLevel = Global.SelectedBin,
                            //                DieCode = DateTime.Now.ToString("yyyyMMddHHmmssfff"),
                            //            };
                            //            ProcInfoHelper.Instance.PlaceDieToTray(Stn4TurnTableStnEnum.DieToTray, i, j, dieModel);
                            //        }
                            //    }
                            //}


                            var rcNo = ProcInfoHelper.Instance.GetAvlEmptyCellNoFromTray(Stn4TurnTableStnEnum.DieToTray);
                            if (rcNo.r != -1 && rcNo.c != -1)
                            {
                                CurRowNo = rcNo.r;
                                CurColNo = rcNo.c;
                            }
                            //当前放Die行列号超过Tray盘行列总数
                            if (CurRowNo >= ParaHelper.Prod.TrayRowCount || CurColNo >= ParaHelper.Prod.TrayColCount)
                            {
                                ShowMsg($"{StationName}->CurRowNo：{CurRowNo}，CurColNo：{CurColNo} " +
                                    $"TrayRowCount:{ParaHelper.Prod.TrayRowCount},TrayColCount:{ParaHelper.Prod.TrayColCount} 当前行列号超出Tray盘行列总数", MsgType.Error);
                                AlarmManager.CreateAlarm(AlarmCode.CurTrayRowColCountExceedTrayCount.ToInt(), StationNum);
                            }

                            if (trayStageInfo.TrayModel.TrayType == TrayTypeEnum.Full)
                            {
                                ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}满Tray，DieToTray流程结束");
                                step = 0;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}未满Tray");
                                step = 100;
                            }
                        }
                        else
                        {
                            ShowMsg($"{StationName}->从DieToTray工位Tray平台获取Tray信息失败", MsgType.Error);
                            AlarmManager.CreateAlarm(AlarmCode.GetTrayInfoFail.ToInt(), AllStationAlarm);
                        }
                        break;
                    }

                case 100:
                    {
                        if (CurColNo < ParaHelper.Prod.TrayColCount)
                        {
                            if (ProcInfoHelper.Instance.IsCanPlaceDie(Stn4TurnTableStnEnum.DieToTray, CurRowNo, CurColNo))
                            {
                                Axes = new List<AxisName>() { CurStage.X, CurStage.Y };
                                Poses = GetCellPos(CurRowNo, CurColNo).ToList();
                                MoveAbsM(Axes, Poses);
                                ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}移动到第{CurRowNo + 1}行第{CurColNo + 1}列,X:{Poses?.First()} Y:{Poses[1]}");
                                step = 150;
                            }
                            else
                            {
                                CurColNo++;
                            }
                        }
                        else
                        {
                            CurRowNo++;
                            if (CurRowNo >= ParaHelper.Prod.TrayRowCount)//放满
                            {
                                ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}满料，移动到基准位置");
                                var trayStageInfo = ProcInfoHelper.Instance.GetTrayInfo(Stn4TurnTableStnEnum.DieToTray);
                                if (trayStageInfo.TrayModel != null)
                                {
                                    trayStageInfo.TrayModel.TrayType = TrayTypeEnum.Full;
                                }
                                Axes = new List<AxisName>() { CurStage.X, CurStage.Y };
                                Poses = BasePos.ToList();
                                MoveAbsM(Axes, Poses);
                                step = 350;
                            }
                            else
                            {
                                CurColNo = 0;
                            }
                        }
                        break;
                    }
                case 150:
                    {
                        var ret = IsAxisArriveM(Axes, Poses);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}到达第{CurRowNo + 1}行第{CurColNo + 1}列，等待PlaceDie工位放Die请求");
                            step = 200;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 50;
                        }
                        break;
                    }
                case 200:
                    {
                        if (GetBoolReg(RegisterEnum.PlaceDieStnPlaceDieReq))
                        {
                            ShowMsg($"{StationName}->收到PlaceDie工位放Die请求，执行Die对位流程");
                            ResetBoolReg(RegisterEnum.PlaceDieStnPlaceDieReq);
                            if (ParaHelper.Vision.IsAlignBeforeDieToTray)
                            {
                                DieAlignStep = 1;
                                step = 250;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->不进行Die对位，等待放Die完成");
                                step = 300;
                            }
                        }
                        break;
                    }
                case 250:
                    {
                        DieAlignStep = DieAlignFlow(DieAlignStep);
                        if (DieAlignStep == 0)
                        {
                            ShowMsg($"{StationName}->Die对位流程执行完成，发送允许放Die给PlaceDie工位，等待放Die完成");
                            RegisterService.Instance.SetBoolReg(RegisterEnum.DieToTrayStnAllowPlaceDie);
                            step = 300;
                        }
                        break;
                    }
                case 300:
                    {
                        if (GetBoolReg(RegisterEnum.PlaceDieCom))
                        {
                            ResetBoolReg(RegisterEnum.PlaceDieCom);
                            var pickerNo = RotateInfoHelper.Instance.GetStageNo(Stn16TurnTableStnEnum.PlaceDie.ToInt(), RotateStnType.LoadDie);
                            ShowMsg($"{StationName}->PlaceDie工位放Die完成，吸嘴：{pickerNo}，Tray移动到下一个穴位", MsgType.Notify);
                            var dieModel = ProcInfoHelper.Instance.PickDieFromPicker(Stn16TurnTableStnEnum.PlaceDie, RotateStnType.LoadDie);
                            if (dieModel != null)
                            {
                                dieModel.EndTime = DateTime.Now;
                                dieModel.SpanTime = dieModel.EndTime.Subtract(dieModel.StartTime).TotalSeconds;
                                dieModel.InTrayRowNo = CurRowNo;
                                dieModel.InTrayColNo = CurColNo;
                                DieModelHelper.Update(dieModel);
                                ProcInfoHelper.Instance.PlaceDieToTray(Stn4TurnTableStnEnum.DieToTray, CurRowNo, CurColNo, dieModel);
                                CurColNo++;
                                step = 100;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->从上料转塔吸嘴获取DieModel信息失败，吸嘴：{pickerNo}", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.GetDieModelFromLoadDDPickerFail.ToInt(), StationNum);
                            }
                        }
                        else if (GetBoolReg(RegisterEnum.CurrentBinLevelPickOver) || !ProcInfoHelper.Instance.IsHasDieOnPicker())
                        {
                            ShowMsg($"{StationName}->PlaceDie工位吸嘴Bin等级和当前Tray等级不一致，放Die流程结束，TrayStage{CurStage.StageNum}移动到基准位置", MsgType.Notify);
                            ResetBoolReg(RegisterEnum.CurrentBinLevelPickOver);
                            Axes = new List<AxisName>() { CurStage.X, CurStage.Y };
                            Poses = BasePos.ToList();
                            MoveAbsM(Axes, Poses);
                            step = 350;
                        }
                        break;
                    }
                case 350:
                    {
                        var ret = IsAxisArriveM(Axes, Poses);
                        if (ret == MotionStatus.Arrived)
                        {
                            CurColNo = 0;
                            CurRowNo = 0;
                            ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}到达基准位置，DieToTray流程结束");
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
        public int DieAlignFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
						ShowMsg($"{StationName}->DieAlign流程开始，发送AlignDie请求给AlignPC，Station：Die_DieToTray");
						var dieIndex = CurRowNo * ParaHelper.Prod.TrayColCount + CurColNo;
						var body = AlignHelper.Instance.Send<AlignRspBody>(VisionCmd.Align, AlignStationEnum.Die_DieToTray, dieIndex);
						if (body != null && body.Offset != null)
                        {
                            ShowMsg($"{StationName}->对位PC返回坐标： {body.Offset.ToString()}");
                            Axes = new List<AxisName>() { CurStage.X, CurStage.Y };
                            Poses = body.Offset.ToList();
                            MoveRelM(Axes, Poses);
                            ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}移动补正值");
                            step = 50;
                        }
                        else
                        {
							ShowMsg($"{StationName}->AlignPC Tray对位失败，命令：{VisionCmd.Align}，工位：Die_DieToTray", MsgType.Error);
							AlarmManager.CreateAlarm(AlarmCode.DieAlignFail.ToInt(), StationNum);
                            var result = AskHowToDoRequest(StationNum, $"{StationName}->AlignPC Die对位失败，请选择如何处理");
                            switch (result)
                            {
                                case AskResult.Retry:
                                    {
                                        ShowMsg($"{StationName}->处理结果：重试");
                                        break;
                                    }
                                case AskResult.Skip:
                                    {
                                        ShowMsg($"{StationName}->处理结果：跳过对位，继续流程");
                                        step = 0;
                                        break;
                                    }
                                default:
                                    break;
                            }
                        }
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArriveM(Axes);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->TrayStage{CurStage.StageNum}对位完成");
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
        /// <summary>
        /// 获取当前穴位的放Die位置坐标
        /// </summary>
        /// <param name="rowNo"></param>
        /// <param name="colNo"></param>
        /// <returns></returns>
        private Xyt GetCellPos(int rowNo, int colNo)
        {
            //LoadDie X+，Y -
            //正面检：X -，Y +
            var x = FirstCellPos.X + colNo * ParaHelper.Prod.CellPitchX;
            //var y = FirstCellPos.Y + rowNo * ParaHelper.Prod.CellPitchY;
            var y = FirstCellPos.Y - rowNo * ParaHelper.Prod.CellPitchY;

            //var x = FirstCellPos.Y + colNo * ParaHelper.Prod.CellPitchX;
            //var y = FirstCellPos.X + rowNo * ParaHelper.Prod.CellPitchY;
            return new Xyt() { X = x, Y = y };
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
            DieToTrayStep = -1;
            TrayAlignStep = -1;

        }

        public void StopHandle()
        {
        }

        public void WaitFlowDone()
        {
        }
    }
}
