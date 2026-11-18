using JC.LidAttach.Devices.Laser;
using JC.PTIS.Main.AlarmInfo;
using JC.PTIS.Main.Devices.Vision;
using JC.PTIS.Main.Entities;
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
using System.Threading.Tasks;

namespace JC.PTIS.Main.Stations
{
    public class MeasureHeightStn : StationBaseEx, IStation
    {
        public int StationNum { get; set; } = 9;
        public string StationName { get; set; } = "MeasureHeightStn";

        List<AxisName> Axes = new List<AxisName>();
        List<PosName> PosNames = new List<PosName>();//位置名称集合
        List<double> Poses = new List<double>();//实际位置集合

        #region 子流程状态机步骤
        int MeasureHeightStep = -1;
        #endregion
        int PointNo = 0;//测高点位编号

        double[]HeightValues= new double[4];//测高点位高度值

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
                            RegisterService.Instance.SetBoolReg(RegisterEnum.MeasureHeightDone);
                            ShowMsg($"{StationName}->等待4工位转盘转动到位");
                            AutoRunStep = 50;
                            break;
                        }
                    case 50:
                        {
                            if (GetBoolReg(RegisterEnum.Stn4DDRotateDone,true))
                            {
                                var stageNo = RotateInfoHelper.Instance.GetStageNo(Stn4TurnTableStnEnum.MeasureHeight.ToInt(), RotateStnType.Stn4TurnTable);
                                CurStage = StageManager.Get(stageNo);
                                ShowMsg($"{StationName}->4工位转盘转动到位，判断Tray{stageNo + 1}平台是否有Tray");
                                if (Motion.GetDigitalInput(CurStage.IsExistTray))
                                {
                                    ShowMsg($"{StationName}->Tray{stageNo + 1}平台有Tray，执行测高流程");
                                    MeasureHeightStep = 1;
                                    AutoRunStep = 100;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->Tray{stageNo + 1}平台没有Tray，通知转盘可以转动");
                                    AutoRunStep = 1;
                                }
                            }
                            break;
                        }
                    case 100:
                        {
                            MeasureHeightStep = MeasureHeightFlow(MeasureHeightStep);
                            if (MeasureHeightStep == 0)
                            {
                                ShowMsg($"{StationName}->测高流程执行完成，通知转盘可以转动");
                                AutoRunStep = 1;
                            }
                            else if (MeasureHeightStep == -1)
                            {
                                ShowMsg($"{StationName}->测高失败，弹窗询问如何处理", MsgType.Error, true);
                                var ret = AskHowToDoRequest(StationNum,"测高失败，弹窗询问如何处理");
                                switch (ret)
                                {
                                    case AskResult.Retry:
                                        {
                                            ShowMsg($"{StationName}->处理结果：重试");
                                            MeasureHeightStep = 1;
                                            break;
                                        }
                                    case AskResult.Skip:
                                        {
                                            ShowMsg($"{StationName}->处理结果：跳过。通知转盘可以转动");
                                            AutoRunStep = 1;
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
        public int MeasureHeightFlow(int step)
        {
            try
            {
                switch (step)
                {
                    case 1:
                        {
                            PointNo = 0;
                            var stageNo = RotateInfoHelper.Instance.GetStageNo(Stn4TurnTableStnEnum.MeasureHeight.ToInt(), RotateStnType.Stn4TurnTable);
                            CurStage = StageManager.Get(stageNo);
                            ShowMsg($"{StationName}->测高流程开始", MsgType.Notify);
                            BasePos = new Xyt() { X = Motion.GetAxisActPos(CurStage.X), Y = Motion.GetAxisActPos(CurStage.Y), T = Motion.GetAxisActPos(CurStage.T) };//记录当前TrayStage位置为基准位置
                            //var trayInfo = ProcInfoHelper.Instance.GetTrayInfo(Stn4TurnTableStnEnum.MeasureHeight);
                            //if (trayInfo != null&& trayInfo.TrayModel!=null)
                            //{
                            //    trayInfo.TrayModel.TrayFlowStep = TrayFlowStep.MeasHeight;
                            //}
                            step = 50;
                            break;
                        }
                    case 50:
                        {
                            if (PointNo < 4)
                            {
                                ShowMsg($"{StationName}->TrayStage{CurStage.StageNum + 1}移动到第{PointNo + 1}个测高位置");
                                Axes = new List<AxisName>() { CurStage.X, CurStage.Y };
                                var xyt = GetMeasureHeightPos(PointNo);
                                Poses = xyt.ToList();
                                MoveAbsM(Axes, Poses);
                                step = 100;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->测高流程执行完成，TrayStage{CurStage.StageNum + 1}移动到基准位置");
                                Axes = new List<AxisName>() { CurStage.X, CurStage.Y };
                                Poses = BasePos.ToList();
                                MoveAbsM(Axes, Poses);
                                step = 150;
                            }
                            break;
                        }
                    case 100:
                        {
                            var ret = IsAxisArriveM(Axes, Poses);
                            if (ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->TrayStage{CurStage.StageNum + 1}到达第{PointNo + 1}个测高位置");
                                var height = LaserHelper.Instance.GetValue(LaserEnum.LoadDieStnMeasureTray.ToInt());
                                if (height != -1)
                                {
                                    ShowMsg($"{StationName}->读取高度值：{height}毫米");
                                    if (PointNo<4)
                                    {
                                        HeightValues[PointNo] = height;
                                    }
                                    PointNo++;
                                    step = 50;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->读取高度值失败", MsgType.Error);
                                    AlarmManager.CreateAlarm(AlarmCode.MeasureHeightFail.ToInt(), StationNum);
                                    step = -1;
                                }
                            }
                            else if (ret == MotionStatus.Timeout)
                            {
                                step = 1;
                            }
                            break;
                        }
                    case 150:
                        {
                            var ret = IsAxisArriveM(Axes, Poses);
                            if (ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->TrayStage{CurStage.StageNum + 1}到达基准位置，测高流程结束", MsgType.Notify);
                                RegisterService.Instance.SetBoolReg(RegisterEnum.MeasureHeightDone);
                                PointNo = 0;
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
            catch (Exception ex)
            {
                return -1;
            }
        }
       
        private Xyt GetMeasureHeightPos(int no)
        {
            var x = BasePos.X + no * ParaHelper.Prod.FirstMeasureHeightPointOffsetX;
            var y = BasePos.Y + no * ParaHelper.Prod.FirstMeasureHeightPointOffsetY;
            switch (no)
            {
                case 1:
                    {
                        x= BasePos.X + ParaHelper.Prod.MeasureHeightOffset;
                        break;
                    }
                case 2:
                    {
                        x = BasePos.X + ParaHelper.Prod.MeasureHeightOffset;
                        y = BasePos.Y + ParaHelper.Prod.MeasureHeightOffset;
                        break;
                    }
                case 3:
                    {
                        y = BasePos.Y + ParaHelper.Prod.MeasureHeightOffset;
                        break;
                    }
                default:
                    break;
            }
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
            MeasureHeightStep = -1;
            PointNo = -1;
        }

        public void StopHandle()
        {
        }

        public void WaitFlowDone()
        {
        }
    }
}
