using APS168_W32;
using JC.LidAttach.Devices.Laser;
using JC.PTIS.Main.AlarmInfo;
using JC.PTIS.Main.Devices.Vision;
using JC.PTIS.Main.Devices.VisionPC.MsgModel;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Params;
using JC.PTIS.Main.Service;
using StdLib.LogHelper;
using StdLib.PCBaseFrame.Alarm;
using StdLib.PCBaseFrame.Args;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.PCBaseFrame.Motion;
using StdLib.PCBaseFrame.MotionInfo;
using StdLib.PCBaseFrame.Station;
using StdLib.WindowsControls;
using StdLib.WindowsControls.Class;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace JC.PTIS.Main.Stations
{
    public class IsolationPaperStn : StationBaseEx, IStation
    {
        public int StationNum { get; set; } = 14;
        public string StationName { get; set; } = "IsolationPaperStn";


        int MoveToWaitPosStep = -1;
        int PutPaperToStageStep = -1;
        int PaperMeasureHeightStep = -1;
        int PaperAlignStep = -1;
        int PickPaperFromCabinetStep = -1;
        int PutPaperToLotStep = -1;
        int LotAlignStep = -1;

        List<AxisName> Axes = new List<AxisName>();
        List<PosName> PosNames = new List<PosName>();
        List<CylinderName> Cylinders = new List<CylinderName>();

        bool RotateCylOut = false;

        List<double> Poses = new List<double>();
        Xyt PaperXyt = null;
        Xyt LotXyt = null;
        Xyt PaperBasePos = null;
        int CabinetNo = 0;
        bool IsChecked = false;
        public void Home()
        {
            try
            {
                switch (HomeStep)
                {
                    case 1:
                        {
                            Cylinder(CylinderName.PickPaperLiftCyl, false);
                            ShowMsg($"{StationName}->{CylinderName.PickPaperLiftCyl}气缸缩回");
                            ShowMsg($"{StationName}->IsolationPaper_ArmZ轴和IsolationPaper_LiftZ轴开始回零");
                            Motion.Home(AxisName.IsolationPaper_ArmZ);
                            Motion.Home(AxisName.IsolationPaper_LiftZ);
                            HomeStep = 50;
                            break;
                        }
                    case 50:
                        {
                            var ret = IsCylinderArrive(CylinderName.PickPaperLiftCyl, false);
                            if (Motion.IsHomeDone(AxisName.IsolationPaper_ArmZ) &&
                                Motion.IsHomeDone(AxisName.IsolationPaper_LiftZ) &&
                                ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->IsolationPaper_ArmZ轴和IsolationPaper_LiftZ轴回零完成");
                                Cylinders = new List<CylinderName>() { CylinderName.PickPaperTransferCyl, CylinderName.PaperCabinetRotateCyl };
                                CylinderM(Cylinders, false);
                                ShowMsg($"{StationName}->{CylinderName.PickPaperTransferCyl}和{CylinderName.PaperCabinetRotateCyl}气缸缩回");
                                HomeStep = 100;
                            }
                            else if (Motion.IsMoveTimeOut(AxisName.IsolationPaper_ArmZ))
                            {
                                ShowMsg($"{StationName}->IsolationPaper_ArmZ轴回零超时", MsgType.Error);
                                HomeStep = 1000;
                            }
                            else if (Motion.IsMoveTimeOut(AxisName.IsolationPaper_LiftZ))
                            {
                                ShowMsg($"{StationName}->IsolationPaper_LiftZ轴回零超时", MsgType.Error);
                                HomeStep = 1000;
                            }
                            else if (ret == MotionStatus.Timeout)
                            {
                                ShowMsg($"{StationName}->{CylinderName.PickPaperLiftCyl}气缸缩回超时", MsgType.Error);
                                HomeStep = 1000;
                            }
                            break;
                        }
                    case 100:
                        {
                            var ret = IsCylinderArriveM(Cylinders, false);
                            if (ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->{CylinderName.PickPaperTransferCyl}和{CylinderName.PaperCabinetRotateCyl}气缸缩回到位");
                                ShowMsg($"{StationName}->等待{AxisName.Stn8TurnTable_ArmX}轴回原完成并运动到{PosName.WaitPos}位置");
                                HomeStep = 150;
                            }
                            else if (ret == MotionStatus.Timeout)
                            {
                                ShowMsg($"{StationName}->{CylinderName.PickPaperTransferCyl}和{CylinderName.PaperCabinetRotateCyl}气缸缩回超时", MsgType.Error);
                                HomeStep = 1000;
                            }
                            break;
                        }
                    case 150:
                        {
                            if (Motion.IsArrive(AxisName.Stn8TurnTable_ArmX, PosName.WaitPos))
                            {
                                ShowMsg($"{StationName}->{AxisName.Stn8TurnTable_ArmX}轴回零完成并到达{PosName.WaitPos}位置");
                                Motion.Home(AxisName.IsolationPaper_ArmX);
                                Motion.Home(AxisName.IsolationPaper_ArmY);
                                ShowMsg($"{StationName}->{AxisName.IsolationPaper_ArmX}轴，{AxisName.IsolationPaper_ArmY}轴开始回零");
                                HomeStep = 200;
                            }
                            else if (Motion.IsMoveTimeOut(AxisName.Stn8TurnTable_ArmX))
                            {
                                ShowMsg($"{StationName}->等待{AxisName.Stn8TurnTable_ArmX}回零完成并运动到{PosName.WaitPos}位置超时", MsgType.Error);
                                HomeStep = 1000;
                            }
                            break;
                        }
                    case 200:
                        {
                            if (Motion.IsHomeDone(AxisName.IsolationPaper_ArmX) && Motion.IsHomeDone(AxisName.IsolationPaper_ArmY))
                            {
                                ShowMsg($"{StationName}->{AxisName.IsolationPaper_ArmX}轴，{AxisName.IsolationPaper_ArmY}轴回零完成");
                                Motion.MoveAbs(AxisName.IsolationPaper_ArmX, PosName.WaitPos);
                                ShowMsg($"{StationName}->{AxisName.IsolationPaper_ArmX}轴移动到{PosName.WaitPos}位置");
                                HomeStep = 250;
                            }
                            else if (Motion.IsMoveTimeOut(AxisName.IsolationPaper_ArmX))
                            {
                                ShowMsg($"{StationName}->IsolationPaper_ArmX轴回零超时", MsgType.Error);
                                HomeStep = 1000;
                            }
                            else if (Motion.IsMoveTimeOut(AxisName.IsolationPaper_ArmY))
                            {
                                ShowMsg($"{StationName}->IsolationPaper_ArmY轴回零超时", MsgType.Error);
                                HomeStep = 1000;
                            }
                            break;
                        }
                    case 250:
                        {
                            if (Motion.IsArrive(AxisName.IsolationPaper_ArmX, PosName.WaitPos))
                            {
                                ShowMsg($"{StationName}->{AxisName.IsolationPaper_ArmX}轴到达{PosName.WaitPos}位置");
                                Motion.Home(AxisName.IsolationPaper_ArmT);
                                Motion.Home(AxisName.IsolationPaper_Shift);
                                ShowMsg($"{StationName}->{AxisName.IsolationPaper_ArmT}轴，{AxisName.IsolationPaper_Shift}轴开始回零");
                                HomeStep = 300;
                            }
                            else if (Motion.IsMoveTimeOut(AxisName.IsolationPaper_ArmX))
                            {
                                ShowMsg($"{StationName}->IsolationPaper_ArmX轴移动到{PosName.WaitPos}位置超时", MsgType.Error);
                                HomeStep = 1000;
                            }
                            break;
                        }
                    case 300:
                        {
                            if (Motion.IsHomeDone(AxisName.IsolationPaper_ArmT) && Motion.IsHomeDone(AxisName.IsolationPaper_Shift))
                            {
                                ShowMsg($"{StationName}->{AxisName.IsolationPaper_ArmT}轴，{AxisName.IsolationPaper_Shift}轴回零完成");
                                Motion.MoveAbs(AxisName.IsolationPaper_ArmT, PosName.WaitPos);
                                ShowMsg($"{StationName}->{AxisName.IsolationPaper_ArmT}轴，移动到{PosName.WaitPos}位");
                                HomeStep = 350;
                            }
                            else if (Motion.IsMoveTimeOut(AxisName.IsolationPaper_ArmT))
                            {
                                ShowMsg($"{StationName}->IsolationPaper_ArmT轴回零超时", MsgType.Error);
                                HomeStep = 1000;
                            }
                            else if (Motion.IsMoveTimeOut(AxisName.IsolationPaper_Shift))
                            {
                                ShowMsg($"{StationName}->IsolationPaper_Shift轴回零超时", MsgType.Error);
                                HomeStep = 1000;
                            }
                            break;
                        }
                    case 350:
                        {
                            if (Motion.IsArrive(AxisName.IsolationPaper_ArmT, PosName.WaitPos))
                            {
                                ShowMsg($"{StationName}->{AxisName.IsolationPaper_ArmT}轴，到达{PosName.WaitPos}位");
                                ShowMsg($"{StationName}->工位回零完成");
                                UpdateHomeSts(StationName, StationNum, true);
                                HomeStep = 0;
                                break;
                            }
                            else if (Motion.IsMoveTimeOut(AxisName.IsolationPaper_ArmT))
                            {
                                ShowMsg($"{StationName}->AxisName.IsolationPaper_ArmT轴移动到{PosName.WaitPos}超时", MsgType.Error);
                                HomeStep = 1000;
                            }
                            break;
                        }
                    case 1000:
                        {
                            UpdateHomeSts(StationName, StationNum, false);
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("Error").Error($"[IsolationPaperStn][Home]方法执行异常，异常原因:{ex}");
                HomeStep = -1;
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
                            ShowMsg($"{StationName}->模组回待机位流程开始");
                            MoveToWaitPosStep = 1;
                            AutoRunStep = 50;
                            break;
                        }
                    case 50:
                        {
                            MoveToWaitPosStep = MoveToWaitPosFlow(MoveToWaitPosStep);
                            if (MoveToWaitPosStep == 0)
                            {
                                ShowMsg($"{StationName}->模组回待机位流程结束，判断隔离纸手臂上是否有纸");
                                AutoRunStep = 100;
                            }
                            break;
                        }
                    case 100:
                        {
                            ///8工位转盘请求放隔离纸的时候再进行隔离纸流程
                            if (GetBoolReg(RegisterEnum.PlacePaperReq))
                            {
                                ShowMsg($"{StationName}->收到8工位转盘工位叠隔离纸请求，执行隔离纸对位流程");
                                var ret = IsVacuumArrive(VacuumName.PaperArmVac, true);
                                if (ret == MotionStatus.Arrived)
                                {
                                    ShowMsg($"{StationName}->隔离纸手臂上有纸，执行把隔离纸放回对位平台流程");
                                    PutPaperToStageStep = 1;
                                    AutoRunStep = 150;
                                }
                                else
                                {
                                    ShowMsg($"{StationName}->隔离纸手臂上没有纸，判断对位平台上是否有纸");
                                    AutoRunStep = 200;
                                }
                            }
                            break;
                        }
                    case 150:
                        {
                            PutPaperToStageStep = PutPaperToStageFlow(PutPaperToStageStep);
                            if (PutPaperToStageStep == 0)
                            {
                                AutoRunStep = 200;
                            }
                            break;
                        }
                    case 200:
                        {
                            var ret = IsVacuumArrive(VacuumName.PaperStageVac, true);
                            if (ret == MotionStatus.Arrived)
                            {
                                ShowMsg($"{StationName}->对位平台上有纸，执行隔离纸测高流程");
                                PaperMeasureHeightStep = 1;
                                AutoRunStep = 250;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->对位平台上没有纸，执行从隔离纸料仓取纸流程");
                                PickPaperFromCabinetStep = 1;
                                AutoRunStep = 600;
                            }
                            break;
                        }
                    case 250:
                        {
                            PaperMeasureHeightStep = PaperMeasureHeightFlow(PaperMeasureHeightStep);
                            if (PaperMeasureHeightStep == 0)
                            {
                                ShowMsg($"{StationName}->执行隔离纸对位流程");
                                PaperAlignStep = 1;
                                AutoRunStep = 300;
                            }
                            break;
                        }
                    case 300:
                        {
                            PaperAlignStep = PaperAlignFlow(PaperAlignStep);
                            if (PaperAlignStep == 0)
                            {
                                ShowMsg($"{StationName}->隔离纸对位完成，等待8工位转盘工位叠隔离纸请求");
                                AutoRunStep = 350;
                            }
                            break;
                        }
                    case 350:
                        {
                            //if (GetBoolReg(RegisterEnum.PlacePaperReq))
                            {
                                ShowMsg($"{StationName}->收到8工位转盘工位叠隔离纸请求，执行Lot对位流程");
                                ResetBoolReg(RegisterEnum.PlacePaperReq);
                                LotAlignStep = 1;
                                AutoRunStep = 400;
                            }
                            break;
                        }
                    case 400:
                        {
                            LotAlignStep = LotAlignFlow(LotAlignStep);
                            if (LotAlignStep == 0)
                            {
                                ShowMsg($"{StationName}->Lot对位流程完成，执行放置隔离纸到Lot流程");
                                PutPaperToLotStep = 1;
                                AutoRunStep = 450;
                            }
                            break;
                        }
                    case 450:
                        {
                            PutPaperToLotStep = PutPaperToLotFlow(PutPaperToLotStep);
                            if (PutPaperToLotStep == 0)
                            {
                                ShowMsg($"{StationName}->放置隔离纸到Lot流程完成");
                                RegisterService.Instance.SetBoolReg(RegisterEnum.PlacePaperDone);
                                AutoRunStep = 200;
                            }
                            break;
                        }
                    case 600:
                        {
                            PickPaperFromCabinetStep = PickPaperFromCabinetFlow(PickPaperFromCabinetStep);
                            if (PickPaperFromCabinetStep == 0)
                            {
                                AutoRunStep = 200;
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
        public int PaperMeasureHeightFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->隔离纸测高流程开始，模组移动到测高位置");
                        Axes = new List<AxisName>() { AxisName.IsolationPaper_ArmX, AxisName.IsolationPaper_ArmY };
                        PosNames = new List<PosName>() { PosName.PaperMeasureHeightPos };
                        MoveAbsM(Axes, PosNames);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组到达测高位置");
                            LaserHelper.Instance.Zero(LaserEnum.IsoPaperMeasureHeight.ToInt());
                            var height = LaserHelper.Instance.GetValue(LaserEnum.IsoPaperMeasureHeight.ToInt());
                            if (height >= ParaHelper.Global.PaperHeightBaseValue - ParaHelper.Global.PaperThicknessThreshold &&
                                height <= ParaHelper.Global.PaperHeightBaseValue + ParaHelper.Global.PaperThicknessThreshold)
                            {
                                ShowMsg($"{StationName}->测距仪测量隔离纸高度：{height}mm，符合阈值范围，隔离纸测高流程结束");
                                step = 0;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->测距仪测量隔离纸高度：{height}mm，超出阈值范围", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.PaperThicknessFail.ToInt(), StationNum);
                            }
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
        public int PaperAlignFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        PaperBasePos = null;
                        ShowMsg($"{StationName}->隔离纸对位流程开始，模组移动到拍照位置");
                        Axes = new List<AxisName>() { AxisName.IsolationPaper_ArmX, AxisName.IsolationPaper_ArmY };
                        PosNames = new List<PosName>() { PosName.PaperGrabPos };
                        MoveAbsM(Axes, PosNames);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->请求AlignPC隔离纸对位");
							var body = AlignHelper.Instance.Send<AlignRspBody>(VisionCmd.Align, AlignStationEnum.IsoPaper, 0);
							if (body != null && body.Offset != null)
                            {
                                PaperXyt = body.Offset;
                                ShowMsg($"{StationName}->对位PC返回坐标： {body.Offset.ToString()}。模组移动到取放隔离纸位置");
                                Axes = new List<AxisName>() { AxisName.IsolationPaper_ArmX, AxisName.IsolationPaper_ArmY, AxisName.IsolationPaper_ArmT };
                                PosNames = new List<PosName>() { PosName.PickPaperPos };
                                MoveAbsM(Axes, PosNames);
                                step = 100;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->隔离纸对位失败", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.PaperAlignFail.ToInt(), StationNum);
                                var result = AskHowToDoRequest(StationNum, $"{StationName}->AlignPC 隔离纸对位失败，请选择如何处理");
                                switch (result)
                                {
                                    case AskResult.Retry:
                                        {
                                            ShowMsg($"{StationName}->处理结果：重试");
                                            break;
                                        }
                                    case AskResult.Skip:
                                        {
                                            ShowMsg($"{StationName}->处理结果：不对位隔离纸，继续流程");
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
                            step = 50;
                        }
                        break;
                    }
                case 100:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->隔离纸手臂下降，吸隔离纸");
                            MoveAbs(AxisName.IsolationPaper_ArmZ, PosName.PickPaperPos);
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
                        var ret = IsAxisArrive(AxisName.IsolationPaper_ArmZ, PosName.PickPaperPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->隔离纸手臂上升，吸隔离纸真空释放");
                            Vacuum(VacuumName.PaperArmVac, true);
                            Vacuum(VacuumName.PaperStageVac, false);
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
                        var ret = IsVacuumArrive(VacuumName.PaperArmVac, true);
                        var ret2 = IsVacuumArrive(VacuumName.PaperStageVac, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->隔离纸手臂移动到待机位");
                            Axes = new List<AxisName>() { AxisName.IsolationPaper_ArmZ };
                            PosNames = new List<PosName>() { PosName.WaitPos };
                            MoveAbsM(Axes, PosNames);
                            step = 250;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 150;
                        }
                        break;
                    }
                case 250:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移动补正值");
                            Axes = new List<AxisName>() { AxisName.IsolationPaper_ArmX, AxisName.IsolationPaper_ArmY, AxisName.IsolationPaper_ArmT };
                            Poses = PaperXyt.ToList();
                            MoveRelM(Axes, Poses);
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
                        var ret = IsAxisArriveM(Axes);
                        if (ret == MotionStatus.Arrived)
                        {
                            PaperBasePos = new Xyt();
                            PaperBasePos.X = Motion.GetAxisActPos(AxisName.IsolationPaper_ArmX);
                            PaperBasePos.Y = Motion.GetAxisActPos(AxisName.IsolationPaper_ArmY);
                            PaperBasePos.T = Motion.GetAxisActPos(AxisName.IsolationPaper_ArmT);
                            ShowMsg($"{StationName}->隔离纸对位流程结束");
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 250;
                        }
                        break;
                    }
                default:
                    break;
            }
            return step;
        }
        public int LotAlignFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->Lot对位流程开始，模组移动到Lot拍照位置", MsgType.Notify);
                        Axes = new List<AxisName>() { AxisName.IsolationPaper_ArmX, AxisName.IsolationPaper_ArmY };
                        PosNames = new List<PosName>() { PosName.LotTrayGrabPos };
                        MoveAbsM(Axes, PosNames);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            var lotInfo = ProcInfoHelper.Instance.GetLotStageInfo(Stn8TurnTableStnEnum.PlaceFullTrayStn);
                            var pos = GetLotGrabHeight(lotInfo.LotModel.TrayCount);
                            Poses = new List<double>() { pos };
                            Axes = new List<AxisName>() { AxisName.IsolationPaper_ArmZ };
                            ShowMsg($"{StationName}->模组到达Lot拍照位置，Z轴下降到拍照位置：{pos}");
                            MoveAbsM(Axes, Poses);
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
                        var ret = IsAxisArriveM(Axes, Poses);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->Z轴下降到拍照位置，请求AlignPC Lot对位");
							var body = AlignHelper.Instance.Send<AlignRspBody>(VisionCmd.Align, AlignStationEnum.Lot);
							if (body != null && body.Offset != null)
                            {
                                LotXyt = body.Offset;
                                ShowMsg($"{StationName}->对位PC返回坐标： {body.Offset.ToString()}。模组移动补正值");
                                Axes = new List<AxisName>() { AxisName.IsolationPaper_ArmX, AxisName.IsolationPaper_ArmY, AxisName.IsolationPaper_ArmT };
                                Poses = body.Offset.ToList();
                                MoveRelM(Axes, Poses);
                                step = 150;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->Lot对位失败", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.LotAlignFail.ToInt(), StationNum);
                                var result = AskHowToDoRequest(StationNum, $"{StationName}->AlignPC Lot对位失败，请选择如何处理");
                                switch (result)
                                {
                                    case AskResult.Retry:
                                        {
                                            ShowMsg($"{StationName}->处理结果：重试");
                                            break;
                                        }
                                    case AskResult.Skip:
                                        {
                                            ShowMsg($"{StationName}->处理结果：不对位，继续流程");
                                            step = 150;
                                            break;
                                        }
                                    default:
                                        break;
                                }
                            }
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
                        var ret = IsAxisArriveM(Axes);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移动补正值完成，Lot对位流程结束", MsgType.Notify);
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
        public int PutPaperToLotFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->放隔离纸到Lot流程开始", MsgType.Notify);
                        Axes = new List<AxisName>() { AxisName.IsolationPaper_ArmX, AxisName.IsolationPaper_ArmY };
                        Poses = ParaHelper.Offset.PaperStageAndLotOffset.ToList();
                        MoveRelM(Axes, Poses);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsAxisArriveM(Axes);
                        if (ret == MotionStatus.Arrived)
                        {
                            var lotInfo = ProcInfoHelper.Instance.GetLotStageInfo(Stn8TurnTableStnEnum.PlaceFullTrayStn);
                            var pos = GetPlacePaperToLotHeight(lotInfo.LotModel.TrayCount);
                            ShowMsg($"{StationName}->模组下降，放隔离纸到Lot，Z轴目标高度：{pos}");
                            Axes = new List<AxisName>() { AxisName.IsolationPaper_ArmZ };
                            Poses = new List<double>() { pos };
                            MoveAbsM(Axes, Poses);
                            step = 100;
                        }
                        break;
                    }
                case 100:
                    {
                        var ret = IsAxisArriveM(Axes, Poses);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->Z轴到达放隔离纸高度，真空释放");
                            Vacuum(VacuumName.PaperArmVac, false);
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
                        var ret = IsVacuumArrive(VacuumName.PaperArmVac, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->Z轴移动到待机位");
                            Axes = new List<AxisName>() { AxisName.IsolationPaper_ArmZ };
                            PosNames = new List<PosName>() { PosName.WaitPos };
                            MoveAbsM(Axes, PosNames);
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
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移动到待机位");
                            Axes = new List<AxisName>() { AxisName.IsolationPaper_ArmX, AxisName.IsolationPaper_ArmY, AxisName.IsolationPaper_ArmT };
                            PosNames = new List<PosName>() { PosName.WaitPos };
                            MoveAbsM(Axes, PosNames);
                            step = 250;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 150;
                        }
                        break;
                    }
                case 250:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->放隔离纸到Lot流程结束", MsgType.Notify);
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 200;
                        }
                        break;
                    }
                default:
                    break;
            }
            return step;
        }
        public int PickPaperFromCabinetFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        if (Motion.GetDigitalInput(InputName.PickPaperArmDetect))
                        {
                            ShowMsg($"{StationName}->接近Sensor检测到隔离纸");
                            step = 0;
                        }
                        else
                        {
                            IsChecked = false;
                            ShowMsg($"{StationName}->从料仓取隔离纸流程开始，判断当前料仓有无隔离纸");
                            step = 50;
                        }
                        break;
                    }
                case 50:
                    {
                        var org = IsCylinderArrive(CylinderName.PaperCabinetRotateCyl, false) == MotionStatus.Arrived;//气缸在缩回状态，料仓1在取隔离纸位置
                        var @out = IsCylinderArrive(CylinderName.PaperCabinetRotateCyl, true) == MotionStatus.Arrived;//气缸在缩回状态，料仓1在取隔离纸位置
                        if (org)
                        {
                            CabinetNo = 1;
                        }
                        else if (@out)
                        {
                            CabinetNo = 2;
                        }
                        if (Motion.GetDigitalInput($"PaperCabinet{CabinetNo}Detect"))
                        {
                            ShowMsg($"{StationName}->料仓{CabinetNo}中有隔离纸，平移气缸伸出");
                            Cylinder(CylinderName.PickPaperTransferCyl, true);
                            step = 100;
                        }
                        else
                        {
                            if (IsChecked)
                            {
                                ShowMsg($"{StationName}->两个料仓中都没有隔离纸，请更换料仓", MsgType.Error, true);
                                AlarmManager.CreateAlarm(AlarmCode.PaperCabinetNoPaper.ToInt(), StationNum);
                                IsChecked = false;
                            }
                            else
                            {
                                ShowMsg($"{StationName}->料仓{CabinetNo}中没有隔离纸，气缸旋转，切换到下一个料仓");
                                if (org)
                                {
                                    RotateCylOut = true;//气缸伸出，料仓2在取隔离纸位置
                                }
                                else if (@out)
                                {
                                    RotateCylOut = false;//气缸缩回，料仓1在取隔离纸位置
                                }
                                Cylinder(CylinderName.PaperCabinetRotateCyl, RotateCylOut);
                                IsChecked = true;
                                step = 60;
                            }
                        }
                        break;
                    }
                case 60:
                    {
                        var ret = IsCylinderArrive(CylinderName.PaperCabinetRotateCyl, RotateCylOut);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->气缸旋转到位，再次判断有无隔离纸");
                            step = 50;
                        }
                        break;
                    }
                case 100:
                    {
                        var ret = IsCylinderArrive(CylinderName.PickPaperTransferCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->平移气缸到达取放纸位置，取隔离纸气缸下降");
                            Cylinder(CylinderName.PickPaperLiftCyl, true);
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
                        var ret = IsCylinderArrive(CylinderName.PickPaperLiftCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->隔离纸气缸下降到位，隔离纸顶升轴上升");
                            Motion.Jog(AxisName.IsolationPaper_LiftZ, 1);
                            step = 200;


                            if (Global.RunMode == RunMode.空跑模式)
                            {
                                IOInfoHelper.GetInputInfoByName(InputName.PickPaperArmDetect.ToString(), out int _).IsShield = true;
                                Thread.Sleep(200);
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 100;
                        }
                        break;
                    }
                case 200:
                    {
                        if (Motion.GetDigitalInput(InputName.PickPaperArmDetect))
                        {
                            ShowMsg($"{StationName}->接近Sensor检测到隔离纸，停止顶升轴，吸真空");
                            Motion.Stop(AxisName.IsolationPaper_LiftZ);
                            Vacuum(VacuumName.PickPaperVac, true);
                            step = 250;
                        }
                        break;
                    }
                case 250:
                    {
                        var ret = IsVacuumArrive(VacuumName.PickPaperVac, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->隔离纸气缸上升");
                            Cylinder(CylinderName.PickPaperLiftCyl, false);
                            step = 300;

                            if (Global.RunMode == RunMode.空跑模式)
                            {
                                IOInfoHelper.GetInputInfoByName(InputName.PickPaperArmDetect.ToString(), out int _).IsShield = false;
                                Thread.Sleep(200);
                            }
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 200;
                        }
                        break;
                    }
                case 300:
                    {
                        var ret = IsCylinderArrive(CylinderName.PickPaperLiftCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->平移气缸移动到对位平台位置");
                            Cylinder(CylinderName.PickPaperTransferCyl, false);
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
                        var ret = IsCylinderArrive(CylinderName.PickPaperTransferCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->升降气缸下降");
                            Cylinder(CylinderName.PickPaperLiftCyl, true);
                            step = 400;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 300;
                        }
                        break;
                    }
                case 400:
                    {
                        var ret = IsCylinderArrive(CylinderName.PickPaperLiftCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->对位平台吸真空");
                            Vacuum(VacuumName.PickPaperVac, false);
                            Vacuum(VacuumName.PaperStageVac, true);
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
                        var ret = IsVacuumArrive(VacuumName.PaperStageVac, true);
                        var ret2 = IsVacuumArrive(VacuumName.PickPaperVac, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->升降气缸上升");
                            Cylinder(CylinderName.PickPaperLiftCyl, false);
                            step = 500;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 400;
                        }
                        break;
                    }
                case 500:
                    {
                        var ret = IsCylinderArrive(CylinderName.PickPaperLiftCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->平移气缸回料仓位置");
                            Cylinder(CylinderName.PickPaperTransferCyl, true);
                            step = 550;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 450;
                        }
                        break;
                    }
                case 550:
                    {
                        var ret = IsCylinderArrive(CylinderName.PickPaperTransferCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->从料仓取隔离纸流程结束");
                            step = 0;
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
        /// 把对位Arm上的隔离纸放回对位平台流程
        /// </summary>
        /// <param name="step"></param>
        /// <returns></returns>
        public int PutPaperToStageFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->把隔离纸放回对位平台流程开始，判断对位平台上是否有隔离纸");
                        step = 20;
                        break;
                    }
                case 20:
                    {
                        var ret = IsVacuumArrive(VacuumName.PaperStageVac, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->对位平台上有隔离纸，平移气缸移动到从平台取放纸位置");
                            Cylinder(CylinderName.PickPaperTransferCyl, true);
                            step = 50;
                        }
                        else
                        {
                            ShowMsg($"{StationName}->对位平台上没有隔离纸，ArmZ轴移动待机位");
                            Axes = new List<AxisName>() { AxisName.IsolationPaper_ArmZ };
                            PosNames = new List<PosName>() { PosName.WaitPos };
                            MoveAbsM(Axes, PosNames);
                            step = 300;
                        }
                        break;
                    }
                case 50:
                    {
                        var ret = IsCylinderArrive(CylinderName.PickPaperTransferCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->取放隔离纸气缸下降");
                            Cylinder(CylinderName.PickPaperLiftCyl, true);
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
                        var ret = IsCylinderArrive(CylinderName.PickPaperLiftCyl, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->对位平台真空释放，吸隔离纸真空吸附");
                            Vacuum(VacuumName.PaperStageVac, false);
                            Vacuum(VacuumName.PaperArmVac, true);
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
                        var ret = IsVacuumArrive(VacuumName.PaperArmVac, true);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->取放隔离纸气缸上升");
                            Cylinder(CylinderName.PickPaperLiftCyl, false);
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
                        var ret = IsCylinderArrive(CylinderName.PickPaperLiftCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->平移气缸回待机位");
                            Cylinder(CylinderName.PickPaperTransferCyl, false);
                            step = 250;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 150;
                        }
                        break;
                    }
                case 250:
                    {
                        var ret = IsCylinderArrive(CylinderName.PickPaperTransferCyl, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            step = 20;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 200;
                        }
                        break;
                    }
                case 300:
                    {
                        var ret = IsAxisArrive(AxisName.IsolationPaper_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移动从对位平台取放纸位置");
                            Axes = new List<AxisName>() { AxisName.IsolationPaper_ArmX, AxisName.IsolationPaper_ArmY, AxisName.IsolationPaper_ArmT };
                            PosNames = new List<PosName>() { PosName.PickPaperPos };
                            MoveAbsM(Axes, PosNames);
                            step = 350;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 20;
                        }
                        break;
                    }
                case 350:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴移动从对位平台取放纸位置");
                            MoveAbs(AxisName.IsolationPaper_ArmZ, PosName.PickPaperPos);
                            step = 400;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 300;
                        }
                        break;
                    }
                case 400:
                    {
                        var ret = IsAxisArrive(AxisName.IsolationPaper_ArmZ, PosName.PickPaperPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->对位平台真空吸附，吸隔离纸真空释放");
                            Vacuum(VacuumName.PaperArmVac, false);
                            Vacuum(VacuumName.PaperStageVac, true);
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
                        var ret = IsVacuumArrive(VacuumName.PaperStageVac, true);
                        var ret2 = IsVacuumArrive(VacuumName.PaperArmVac, false);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->ArmZ轴移动待机位");
                            MoveAbs(AxisName.IsolationPaper_ArmZ, PosName.WaitPos);
                            step = 500;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 400;
                        }
                        break;
                    }
                case 500:
                    {
                        var ret = IsAxisArrive(AxisName.IsolationPaper_ArmZ, PosName.WaitPos);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组移动待机位");
                            Axes = new List<AxisName>() { AxisName.IsolationPaper_ArmX, AxisName.IsolationPaper_ArmY, AxisName.IsolationPaper_ArmT };
                            PosNames = new List<PosName>() { PosName.WaitPos };
                            MoveAbsM(Axes, PosNames);
                            step = 550;
                        }
                        else if (ret == MotionStatus.Timeout)
                        {
                            step = 450;
                        }
                        break;
                    }
                case 550:
                    {
                        var ret = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->把隔离纸放回对位平台流程结束");
                            step = 0;
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

        public int MoveToWaitPosFlow(int step)
        {
            switch (step)
            {
                case 1:
                    {
                        ShowMsg($"{StationName}->模组回待机位流程开始");
                        Cylinder(CylinderName.PickPaperLiftCyl, false);
                        Axes = new List<AxisName>() { AxisName.IsolationPaper_ArmZ };
                        PosNames = new List<PosName>() { PosName.WaitPos };
                        MoveAbsM(Axes, PosNames);
                        step = 50;
                        break;
                    }
                case 50:
                    {
                        var ret = IsCylinderArrive(CylinderName.PickPaperLiftCyl, false);
                        var re2 = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived && re2 == MotionStatus.Arrived)
                        {
                            Axes = new List<AxisName>()
                            {
                                AxisName.IsolationPaper_ArmX,
                                AxisName.IsolationPaper_ArmY };
                            MoveAbsM(Axes, PosNames);
                            Cylinders = new List<CylinderName>() { CylinderName.PickPaperTransferCyl };
                            CylinderM(Cylinders, false);
                            step = 100;
                        }
                        else if (ret == MotionStatus.Timeout && re2 == MotionStatus.Timeout)
                        {
                            step = 1;
                        }
                        break;
                    }
                case 100:
                    {
                        var ret = IsCylinderArriveM(Cylinders, false);
                        var re2 = IsAxisArriveM(Axes, PosNames);
                        if (ret == MotionStatus.Arrived && re2 == MotionStatus.Arrived)
                        {
                            ShowMsg($"{StationName}->模组回待机位流程结束");
                            step = 0;
                        }
                        else if (ret == MotionStatus.Timeout && re2 == MotionStatus.Timeout)
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
        /// <summary>
        /// 获取相机ArmZ轴拍照高度，跟随Tray数量变化
        /// </summary>
        /// <param name="trayCount"></param>
        /// <returns></returns>
        private double GetLotGrabHeight(int trayCount)
        {
            double ret = 0;
            var posInfo = AxisInfoHelper.GetPosInfoByName(AxisName.IsolationPaper_ArmZ, PosName.GrabLotBaseHeight);
            if (posInfo != null)
            {
                ret = posInfo.Pos + trayCount * ParaHelper.Prod.TrayThickness + (trayCount - 1) * ParaHelper.Global.PaperThickness;
            }
            else
            {
                ShowMsg($"{StationName}->获取{AxisName.IsolationPaper_ArmZ}轴{PosName.GrabLotBaseHeight}点位信息失败", MsgType.Error, true);
                AlarmManager.CreateAlarm(AlarmCode.GetPosInfoFail.ToInt(), AllStationAlarm);
            }
            return ret;
        }
        /// <summary>
        /// 获取ArmZ轴放隔离纸时的高度
        /// </summary>
        /// <param name="trayCount"></param>
        /// <returns></returns>
        private double GetPlacePaperToLotHeight(int trayCount)
        {
            double ret = 0;
            var posInfo = AxisInfoHelper.GetPosInfoByName(AxisName.IsolationPaper_ArmZ, PosName.PlacePaperToLotBaseHeight);
            if (posInfo != null)
            {
                ret = posInfo.Pos + trayCount * ParaHelper.Prod.TrayThickness + (trayCount - 1) * ParaHelper.Global.PaperThickness;
            }
            else
            {
                ShowMsg($"{StationName}->获取{AxisName.IsolationPaper_ArmZ}轴{PosName.PlacePaperToLotBaseHeight}点位信息失败", MsgType.Error, true);
                AlarmManager.CreateAlarm(AlarmCode.GetPosInfoFail.ToInt(), AllStationAlarm);
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
