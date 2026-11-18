using JC.PTIS.Main.MotionInfo;
using StdLib.PCBaseFrame.Args;
using StdLib.PCBaseFrame.Extension.Entities;
using StdLib.PCBaseFrame.Motion;
using StdLib.PCBaseFrame.Station;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Entities
{
    public class InterlockMonitor
    {
        public static event Action<string> ShowMsgEvent;
        static MotionManager Motion = MotionManager.GetInstance();
        public static void StartMonitor()
        {
            Motion.OnCheckIsCanMove += CheckIsCanMove;
            Motion.OnCylinderIsCanMove += Motion_OnCylinderIsCanMove;
            InterlockThread();
        }
        private static MoveResult Motion_OnCylinderIsCanMove(CylinderMoveArgs arg)
        {
            MoveResult result = new MoveResult { Result = true };
            var cyl = EnumHelper.GetEnumValue(arg.CylinderName, CylinderName.None);

            switch (cyl)
            {
                case CylinderName.WaferTableZPowerCyl:
                case CylinderName.WaferTableZLimitCyl:
                    {
                        if (Motion.IsArrive(AxisName.WaferTable_T, PosName.WaferTableZCylOutPos) == false &&
                         arg.MoveType == CylinderMoveType.Out)
                        {
                            result.Msg = $"WaferTable_T轴不在动力气缸伸出位置，禁止手动伸出{cyl}气缸";
                            result.Result = false;
                        }
                        break;
                    }
                case CylinderName.FullTrayArmClawCyl:
                    {
                        if (!Motion.IsArrive(AxisName.Stn8TurnTable_ArmZ, PosName.FullArmPPTrayFrom4StnDDPos) &&
                            !Motion.IsArrive(AxisName.Stn8TurnTable_ArmZ, PosName.FullArmPPTrayFromHalfTrayStagePos) &&
                         arg.MoveType == CylinderMoveType.Org)
                        {
                            result.Msg = $"Stn8TurnTable_ArmZ轴不在取放Tray位置，禁止手动松开{cyl}气缸";
                            result.Result = false;
                        }
                        break;
                    }
                case CylinderName.EmptyTrayArmClawCyl:
                    {
                        if (!Motion.IsArrive(AxisName.Stn8TurnTable_ArmZ, PosName.PPTrayFromEmptyTrayBufferPos) &&
                            !Motion.IsArrive(AxisName.Stn8TurnTable_ArmZ, PosName.EmptyArmPPTrayFrom4StnDDPos) &&
                         arg.MoveType == CylinderMoveType.Org)
                        {
                            result.Msg = $"Stn8TurnTable_ArmZ轴不在取放Tray位置，禁止手动松开{cyl}气缸";
                            result.Result = false;
                        }
                        break;
                    }
            }
            return result;
        }


        private static MoveResult CheckIsCanMove(MoveArgs e)
        {
            MoveResult result = new MoveResult { Result = true };
            if (IsEQInStop() == false)
            {
                result.Msg = $"设备不在停机状态，禁止手动移动{e.AxisName}轴";
                return result;
            }
            var axisName = EnumHelper.GetEnumValue(e.AxisName, AxisName.None);
            if (axisName != AxisName.None)
            {
                switch (axisName)
                {
                    case AxisName.WaferBuffer_X:
                    case AxisName.WaferBuffer_Y:
                    case AxisName.WaferBuffer_Z:
                        {
                            //DI-10,常闭信号
                            if (Motion.GetDigitalInput(InputName.WaferBufferClawSafeDetect) == false)
                            {
                                result.Msg = $"WaferBuffer防撞Sensor被感知，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }
                            else if (Motion.GetDigitalInput(InputName.WaferTableClawIsExistWafer))
                            {
                                result.Msg = $"WaferTable夹爪检测到Wafer，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }
                            break;
                        }
                    case AxisName.WaferTable_Y:
                    case AxisName.WaferTable_Z:
                    case AxisName.WaferTable_X:
                    case AxisName.WaferTable_T:
                        {
                            if (Motion.GetDigitalInput(InputName.WaferTableClawIsExistWafer))
                            {
                                result.Msg = $"WaferTable夹爪检测到Wafer，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }
                            else if (!Motion.IsCylinderArrive(CylinderName.WaferTableArmCyl, false))
                            {
                                result.Msg = $"WaferTable夹爪升降气缸不在安全位置，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }
                            else if (Motion.IsArrive(AxisName.PickDie_Z, PosName.WaitPos) == false)
                            {
                                result.Msg = $"PickDie_Z轴不在安全位置，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }
                            else if (Motion.IsArrive(AxisName.WaferTable_Thimble, PosName.WaitPos) == false)
                            {
                                result.Msg = $"WaferTable_Thimble轴不在安全位置，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }
                            else if (Motion.IsArrive(AxisName.WaferTable_ThimbleCap, PosName.WaitPos) == false)
                            {
                                result.Msg = $"WaferTable_ThimbleCap轴不在安全位置，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }
                            else if (!Motion.IsCylinderArrive(CylinderName.WaferTableThimbleCyl, false))
                            {
                                result.Msg = $"顶针气缸不在缩回位置，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }
                            if (e.AxisName == AxisName.WaferTable_T.ToString())
                            {
                                if (!Motion.IsCylinderArrive(CylinderName.WaferTableZPowerCyl, false) ||
                                    !Motion.IsCylinderArrive(CylinderName.WaferTableZLimitCyl, false))
                                {
                                    result.Msg = $"WaferTableZ轴动力气缸和极限气缸不在缩回位置，禁止手动移动{e.AxisName}轴";
                                    result.Result = false;
                                }
                            }
                            if (e.AxisName == AxisName.WaferTable_Z.ToString())
                            {
                                if (!Motion.IsCylinderArrive(CylinderName.WaferTableZPowerCyl, true) ||
                                    !Motion.IsCylinderArrive(CylinderName.WaferTableZLimitCyl, true))
                                {
                                    result.Msg = $"WaferTableZ轴动力气缸和极限气缸不在伸出位置，禁止手动移动{e.AxisName}轴";
                                    result.Result = false;
                                }
                                if (e.TargetPos > AxisInfoHelper.GetPosInfoByName(e.AxisName, PosName.LiftPos).Pos)
                                {
                                    result.Msg = $"WaferTableZ轴目标位置超过扩摸位置，禁止手动移动{e.AxisName}轴";
                                    result.Result = false;
                                }
                            }
                            break;
                        }
                    case AxisName.LoadDie_DD:
                        {
                            if (Motion.IsArrive(AxisName.PickDie_Z, PosName.WaitPos) == false)
                            {
                                result.Msg = $"PickDie_Z轴不在安全位置，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }
                            else if (Motion.IsArrive(AxisName.PlaceDie_Z, PosName.WaitPos) == false)
                            {
                                result.Msg = $"PlaceDie_Z轴不在安全位置，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }
                            break;
                        }
                    case AxisName.PickPatch_DD:
                        {
                            if (Motion.IsArrive(AxisName.PickPatch_PickZ, PosName.WaitPos) == false)
                            {
                                result.Msg = $"PickPatch_PickZ轴不在安全位置，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }
                            else if (Motion.IsArrive(AxisName.PickPatch_PlaceZ, PosName.WaitPos) == false)
                            {
                                result.Msg = $"PickPatch_PlaceZ轴不在安全位置，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }
                            var xPos = Motion.GetAxisActPos(AxisName.Stn8TurnTable_ArmX);
                            var yPos = Motion.GetAxisActPos(AxisName.Stn8TurnTable_ArmY);
                            if (xPos > 10 && xPos < 200 && yPos > 300 && yPos < 400 &&
                                !Motion.IsArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos))
                            {
                                result.Msg = $"8工位手臂不在安全位置，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }
                            break;
                        }
                    case AxisName.HalfTray_BufferZ:
                        {
                            if (Motion.IsArrive(AxisName.HalfTray_BufferX, PosName.WaitPos) == false)
                            {
                                result.Msg = $"HalfTray_BufferX轴不在安全位置，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }
                            break;
                        }
                    case AxisName.Stn8TurnTable_DD:
                        {
                            var xPos = Motion.GetAxisActPos(AxisName.Stn8TurnTable_ArmX);
                            var yPos = Motion.GetAxisActPos(AxisName.Stn8TurnTable_ArmY);
                            if (xPos > 10 && xPos < 200 && yPos > 10 && yPos < 200 &&
                                !Motion.IsArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos))
                            {
                                result.Msg = $"8工位手臂不在安全位置，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }

                            var xPos2 = Motion.GetAxisActPos(AxisName.Packing_ArmX);
                            var yPos2 = Motion.GetAxisActPos(AxisName.Packing_ArmY);
                            if (xPos2 > 10 && xPos2 < 200 && yPos2 > 10 && yPos2 < 200 &&
                                !Motion.IsArrive(AxisName.Packing_ArmZ, PosName.WaitPos))
                            {
                                result.Msg = $"Packing手臂不在安全位置，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }
                            break;
                        }
                    case AxisName.Stn8TurnTable_ArmX:
                        {
                            if (Motion.IsArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos) == false)
                            {
                                result.Msg = $"Stn8TurnTable_ArmZ轴不在安全位置，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }

                            if (Motion.IsCylinderArrive(CylinderName.EmptyTrayArmLiftCyl, false) == false)
                            {
                                result.Msg = $"空Tray手臂升降气缸不在原点位置，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }

                            if (Motion.IsCylinderArrive(CylinderName.FullTrayArmLiftCyl, false) == false)
                            {
                                result.Msg = $"满Tray手臂升降气缸不在原点位置，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }

                            double axisXEmptyArmPPTrayFrom8StnDDPos = AxisInfoHelper.GetPosInfoByName(AxisName.Stn8TurnTable_ArmX, "EmptyArmPPTrayFrom8StnDDPos").Pos;

                            double axisXFullArmPPTrayFrom8StnDDPos = AxisInfoHelper.GetPosInfoByName(AxisName.Stn8TurnTable_ArmX, "FullArmPPTrayFrom8StnDDPos").Pos;

                            if (e.TargetPos == axisXEmptyArmPPTrayFrom8StnDDPos || e.TargetPos == axisXFullArmPPTrayFrom8StnDDPos)
                            {
                                if (Motion.IsArrive(AxisName.IsolationPaper_ArmX, PosName.WaitPos) == false ||
                                    Motion.IsArrive(AxisName.IsolationPaper_ArmY, PosName.WaitPos) == false)
                                {
                                    result.Msg = $"{AxisName.IsolationPaper_ArmX}轴或者{AxisName.IsolationPaper_ArmY}轴不在待机位，禁止手动移动{e.AxisName}轴";
                                    result.Result = false;
                                }
                            }

                            double axisXEEmptyArmPPTrayFrom4StnDDPos = AxisInfoHelper.GetPosInfoByName(AxisName.Stn8TurnTable_ArmX, "EmptyArmPPTrayFrom4StnDDPos").Pos;

                            double axisXFullArmPPTrayFrom4StnDDPos = AxisInfoHelper.GetPosInfoByName(AxisName.Stn8TurnTable_ArmX, "FullArmPPTrayFrom4StnDDPos").Pos;

                            if (Motion.IsArrive(AxisName.PickPatch_CamY, PosName.WaitPos) == false)
                            {
                                result.Msg = $"{AxisName.PickPatch_CamY}轴不在待机位，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }

                            break;
                        }
                    case AxisName.Stn8TurnTable_ArmY:
                        {
                            if (Motion.IsArrive(AxisName.Stn8TurnTable_ArmZ, PosName.WaitPos) == false)
                            {
                                result.Msg = $"Stn8TurnTable_ArmZ轴不在安全位置，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }

                            if (Motion.IsCylinderArrive(CylinderName.EmptyTrayArmLiftCyl, false) == false)
                            {
                                result.Msg = $"空Tray手臂升降气缸不在原点位置，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }

                            if (Motion.IsCylinderArrive(CylinderName.FullTrayArmLiftCyl, false) == false)
                            {
                                result.Msg = $"满Tray手臂升降气缸不在原点位置，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }


                            double axisYEmptyArmPPTrayFrom8StnDDPos = AxisInfoHelper.GetPosInfoByName(AxisName.Stn8TurnTable_ArmY, "EmptyArmPPTrayFrom8StnDDPos").Pos;

                            double axisYFullArmPPTrayFrom8StnDDPos = AxisInfoHelper.GetPosInfoByName(AxisName.Stn8TurnTable_ArmY, "FullArmPPTrayFrom8StnDDPos").Pos;

                            if (e.TargetPos == axisYEmptyArmPPTrayFrom8StnDDPos || e.TargetPos == axisYFullArmPPTrayFrom8StnDDPos)
                            {
                                if (Motion.IsArrive(AxisName.IsolationPaper_ArmX, PosName.WaitPos) == false ||
                                    Motion.IsArrive(AxisName.IsolationPaper_ArmY, PosName.WaitPos) == false)
                                {
                                    result.Msg = $"{AxisName.IsolationPaper_ArmX}轴或者{AxisName.IsolationPaper_ArmY}轴不在待机位，禁止手动移动{e.AxisName}轴";
                                    result.Result = false;
                                }
                            }

                            double axisXEEmptyArmPPTrayFrom4StnDDPos = AxisInfoHelper.GetPosInfoByName(AxisName.Stn8TurnTable_ArmY, "EmptyArmPPTrayFrom4StnDDPos").Pos;

                            double axisXFullArmPPTrayFrom4StnDDPos = AxisInfoHelper.GetPosInfoByName(AxisName.Stn8TurnTable_ArmY, "FullArmPPTrayFrom4StnDDPos").Pos;

                            if (Motion.IsArrive(AxisName.PickPatch_CamY, PosName.WaitPos) == false)
                            {
                                result.Msg = $"{AxisName.PickPatch_CamY}轴不在待机位，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }

                            break;
                        }
                    case AxisName.Stn8TurnTable_ArmZ:
                        {
                            if (Motion.IsMove(AxisName.Stn8TurnTable_ArmX) || Motion.IsMove(AxisName.Stn8TurnTable_ArmX))
                            {
                                result.Msg = $"Stn8TurnTable_ArmX轴或者Y轴在移动中，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }
                            break;
                        }
                    case AxisName.Packing_ArmX:
                    case AxisName.Packing_ArmY:
                        {
                            if (Motion.IsArrive(AxisName.Packing_ArmZ, PosName.WaitPos) == false)
                            {
                                result.Msg = $"Packing_ArmZ轴不在安全位置，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }

                            if (Motion.IsCylinderArrive(CylinderName.LabelLiftCyl, false) == false)
                            {
                                result.Msg = $"标签取放升降气缸不在原点位置，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }
                            break;
                        }
                    case AxisName.Packing_ArmZ:
                        {
                            if (Motion.IsMove(AxisName.Packing_ArmX) || Motion.IsMove(AxisName.Packing_ArmY))
                            {
                                result.Msg = $"Packing_ArmX轴或者Y轴在移动中，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }
                            break;
                        }
                    case AxisName.LotBox_ArmX:
                    case AxisName.LotBox_ArmY:
                        {
                            if (Motion.IsArrive(AxisName.Packing_ArmZ, PosName.WaitPos) == false)
                            {
                                result.Msg = $"Packing_ArmZ轴不在安全位置，禁止手动移动{e.AxisName}轴";
                                result.Result = false;
                            }
                            break;
                        }
                }
            }
            else
            {
                result.Msg = $"获取轴信息失败，禁止手动移动{e.AxisName}轴";
                result.Result = false;
            }

            return result;
        }
        public static void InterlockThread()
        {
            // Start monitoring interlocks
            // This is a placeholder for the actual monitoring logic
            Task.Run(() =>
            {
                while (true)
                {
                    // Simulate checking interlock conditions
                    bool interlockConditionMet = CheckInterlockConditions();
                    if (!interlockConditionMet)
                    {
                        ShowMsgEvent?.Invoke("Interlock condition not met!");
                    }
                    System.Threading.Thread.Sleep(1000); // Check every second
                }
            });
        }
        private static bool CheckInterlockConditions()
        {
            // Placeholder for actual interlock condition checks
            return true; // Assume conditions are met for this example
        }
        /// <summary>
        /// 设备是否处于停机状态
        /// </summary>
        /// <returns></returns>
        public static bool IsEQInStop()
        {
            return StationManager.Instance.GetCurrentEQPSts() == StdLib.PCBaseFrame.Views.EqpSts.STOP;
        }
    }
}
