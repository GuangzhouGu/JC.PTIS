using JC.PTIS.Main.AlarmInfo;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Service;
using SqlSugar;
using StdLib.LogHelper;
using StdLib.PCBaseFrame.Alarm;
using StdLib.PCBaseFrame.Entities;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.PCBaseFrame.Extension.Entities;
using StdLib.PCBaseFrame.MotionInfo;
using StdLib.PCBaseFrame.Station;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Entities
{
    public class StationBaseEx : StationBase
    {
        public event Action<RegisterEnum> ResetRegisterEvent;
        public const int AllStationAlarm = -1;
        private int StationNum { get; set; }
        private string StationName { get; set; }
        public Delay Delay { get; set; } = new Delay();
        bool[] AxesArriveFlg = new bool[100];
        bool[] CylArriveFlg = new bool[100];
        bool[] VacArriveFlg = new bool[100];
        public bool[] LastRegisterValue = new bool[1000];
        public void Init(int stationNum, string stationName)
        {
            StationNum = stationNum;
            StationName = stationName;
        }
        #region AxisMove
        public void MoveAbs<T>(AxisName axisName, T posName)
        {
            AxesArriveFlg[axisName.ToInt()] = false;
            LogManager.GetLogger("MotionDetail").Info($"{StationName}->轴：{axisName}绝对移动到{posName}位置");
            Motion.MoveAbs(axisName, posName);
        }
        public void MoveAbsM<T>(List<AxisName> axisNames, List<T> posNames)
        {
            if (axisNames != null)
            {
                var posName = posNames[0];
                AxesArriveFlg[axisNames[0].ToInt()] = false;
                axisNames.ForEach(axisName =>
                {
                    if (posNames.Count > 1)
                    {
                        posName = posNames[axisNames.IndexOf(axisName)];
                    }
                    LogManager.GetLogger("MotionDetail").Info($"{StationName}->轴：{axisName}绝对移动到{posName}位置");
                    Motion.MoveAbs(axisName, posName);
                });
            }
        }

        public void MoveRelM(List<AxisName> axisNames, List<double> poses)
        {
            if (axisNames != null)
            {
                var pos = poses[0];
                AxesArriveFlg[axisNames[0].ToInt()] = false;
                axisNames.ForEach(axisName =>
                {
                    if (poses.Count > 1)
                    {
                        pos = poses[axisNames.IndexOf(axisName)];
                    }
                    LogManager.GetLogger("MotionDetail").Info($"{StationName}->轴：{axisName}相对移动{pos}毫米");
                    Motion.MoveRel(axisName, pos);
                });
                Thread.Sleep(10);
            }
        }
        public void MoveRel(AxisName axisName, double pos)
        {
            AxesArriveFlg[axisName.ToInt()] = false;
            LogManager.GetLogger("MotionDetail").Info($"{StationName}->轴：{axisName}相对移动{pos}毫米");
            Motion.MoveRel(axisName, pos);
            Thread.Sleep(10);
        }

        public MotionStatus IsAxisArrive<T>(AxisName axisName, T posName)
        {
            MotionStatus ret = MotionStatus.None;
            if (Motion.IsArrive(axisName, posName))
            {
                if (!AxesArriveFlg[axisName.ToInt()])
                {
                    AxesArriveFlg[axisName.ToInt()] = true;
                    LogManager.GetLogger("MotionDetail").Info($"{StationName}->轴：{axisName}到达{posName}位置");
                }
                ret = MotionStatus.Arrived;
            }
            else if (Motion.IsMoveTimeOut(axisName))
            {
                ShowMsg($"{StationName}->轴：{axisName}移动到{posName}位置超时", MsgType.Error);
                AlarmManager.CreateAlarm(AlarmCode.AxisMoveTimeoutStart.ToInt() + axisName.ToInt(), StationNum);
                ret = MotionStatus.Timeout;
            }
            return ret;
        }
        public MotionStatus IsAxisArriveM<T>(List<AxisName> axisNames, List<T> posNames,bool isJudgeTimeout=true)
        {
            MotionStatus ret = MotionStatus.None;
            if (axisNames != null)
            {
                var posName = posNames[0];
                bool result = true;
                foreach (var axisName in axisNames)
                {
                    if (posNames.Count > 1)
                    {
                        posName = posNames[axisNames.IndexOf(axisName)];
                    }
                    result &= Motion.IsArrive(axisName, posName);
                }
                if (result)
                {
                    if (!AxesArriveFlg[axisNames[0].ToInt()])
                    {
                        AxesArriveFlg[axisNames[0].ToInt()] = true;
                        LogManager.GetLogger("MotionDetail").Info($"{StationName}->轴：{string.Join(",", axisNames)}到达目标位置");
                    }
                    ret = MotionStatus.Arrived;
                }
                else if(isJudgeTimeout)
                {
                    foreach (var axisName in axisNames)
                    {
                        if (Motion.IsMoveTimeOut(axisName))
                        {
                            ShowMsg($"{StationName}->轴：{axisName}移动到{posName}位置超时", MsgType.Error);
                            AlarmManager.CreateAlarm(AlarmCode.AxisMoveTimeoutStart.ToInt() + axisName.ToInt(), StationNum);
                            ret = MotionStatus.Timeout;
                        }
                    }
                }
            }
            return ret;
        }
        public MotionStatus IsAxisArrive(AxisName axisName)
        {
            MotionStatus ret = MotionStatus.None;
            if (Motion.IsArrive(axisName))
            {
                if (!AxesArriveFlg[axisName.ToInt()])
                {
                    LogManager.GetLogger("MotionDetail").Info($"{StationName}->轴：{axisName}到达目标位置");
                    AxesArriveFlg[axisName.ToInt()] = true;
                }
                ret = MotionStatus.Arrived;
            }
            else if (Motion.IsMoveTimeOut(axisName))
            {
                ShowMsg($"{StationName}->轴：{axisName}移动到目标位置超时", MsgType.Error);
                AlarmManager.CreateAlarm(AlarmCode.AxisMoveTimeoutStart.ToInt() + axisName.ToInt(), StationNum);
                ret = MotionStatus.Timeout;
            }
            return ret;
        }
        public MotionStatus IsAxisArriveM(List<AxisName> axisNames)
        {
            MotionStatus ret = MotionStatus.None;
            if (axisNames != null)
            {
                bool result = true;
                foreach (var axisName in axisNames)
                {
                    result &= Motion.IsArrive(axisName);
                }
                if (result)
                {
                    if (!AxesArriveFlg[axisNames[0].ToInt()])
                    {
                        LogManager.GetLogger("MotionDetail").Info($"{StationName}->轴：{string.Join(",", axisNames)}到达目标位置");
                        AxesArriveFlg[axisNames[0].ToInt()] = true;
                    }
                    ret = MotionStatus.Arrived;
                }
                else
                {
                    foreach (var axisName in axisNames)
                    {
                        if (Motion.IsMoveTimeOut(axisName))
                        {
                            ShowMsg($"{StationName}->轴：{axisName}移动到目标位置超时超时", MsgType.Error);
                            AlarmManager.CreateAlarm(AlarmCode.AxisMoveTimeoutStart.ToInt() + axisName.ToInt(), StationNum);
                            ret = MotionStatus.Timeout;
                        }
                    }
                }
            }
            return ret;
        }
        #endregion
        #region Cylinder
        public void Cylinder(CylinderName cylinderName, bool isExtend)
        {
            CylArriveFlg[cylinderName.ToInt()] = false;
            LogManager.GetLogger("MotionDetail").Info($"{StationName}->气缸：{cylinderName}{(isExtend ? "伸出" : "缩回")}");
            LogManager.GetLogger("MotionDetail").Info($"{StationName}->气缸：{cylinderName}{(isExtend ? "伸出" : "缩回")}");
            Motion.Cylinder(cylinderName, isExtend);
        }
        public void CylinderM(List<CylinderName> cylinderList, bool isExtend)
        {
            if (cylinderList != null)
            {
                CylArriveFlg[cylinderList[0].ToInt()] = false;
                cylinderList.ForEach(cylinderName =>
                {
                    LogManager.GetLogger("MotionDetail").Info($"{StationName}->气缸：{cylinderName}{(isExtend ? "伸出" : "缩回")}");
                    Motion.Cylinder(cylinderName, isExtend);
                });
            }
        }
        public MotionStatus IsCylinderArrive(CylinderName cylinderName, bool isExtend)
        {
            MotionStatus ret = MotionStatus.None;
            if (Motion.IsCylinderArrive(cylinderName, isExtend))
            {
                if (!CylArriveFlg[cylinderName.ToInt()])
                {
                    LogManager.GetLogger("MotionDetail").Info($"{StationName}->气缸：{cylinderName}{(isExtend ? "伸出" : "缩回")}到位");
                    CylArriveFlg[cylinderName.ToInt()] = true;
                }
                ret = MotionStatus.Arrived;
            }
            else if (Motion.IsCylinderTimeOut(cylinderName))
            {
                ShowMsg($"{StationName}->气缸：{cylinderName}{(isExtend ? "伸出" : "缩回")}超时", MsgType.Error);
                AlarmManager.CreateAlarm(AlarmCode.CylinderTimeoutStart.ToInt() + cylinderName.ToInt(), StationNum);
                ret = MotionStatus.Timeout;
            }
            return ret;
        }
        public MotionStatus IsCylinderArriveM(List<CylinderName> cylinderList, bool isExtend)
        {
            MotionStatus ret = MotionStatus.None;
            if (cylinderList != null)
            {
                bool result = true;
                cylinderList.ForEach(cylinderName =>
                {
                    result &= Motion.IsCylinderArrive(cylinderName, isExtend);
                });
                if (result)
                {
                    if (!CylArriveFlg[cylinderList[0].ToInt()])
                    {
                        LogManager.GetLogger("MotionDetail").Info($"{StationName}->气缸：{string.Join(",", cylinderList)}到达目标位置");
                        CylArriveFlg[cylinderList[0].ToInt()] = true;
                    }
                    ret = MotionStatus.Arrived;
                }
                else
                {
                    foreach (var cylinderName in cylinderList)
                    {
                        if (Motion.IsCylinderTimeOut(cylinderName))
                        {
                            ShowMsg($"{StationName}->气缸：{cylinderName}{(isExtend ? "伸出" : "缩回")}超时", MsgType.Error);
                            AlarmManager.CreateAlarm(AlarmCode.CylinderTimeoutStart.ToInt() + cylinderName.ToInt(), StationNum);
                            ret = MotionStatus.Timeout;
                        }
                    }
                }
            }
            return ret;
        }
        #endregion
        #region Vacuum
        public void Vacuum(VacuumName vacuumName, bool isOn)
        {
            VacArriveFlg[vacuumName.ToInt()] = false;
            LogManager.GetLogger("MotionDetail").Info($"{StationName}->真空：{vacuumName}{(isOn ? "开启" : "关闭")}");
            Motion.Vacuum(vacuumName, isOn);

            if (Global.RunMode == RunMode.空跑模式)
            {
                VacuumInfoHelper.GetVacuumInfo(vacuumName).IsShieldDetect = isOn;
            }
        }
        public MotionStatus IsVacuumArrive(VacuumName vacuumName, bool isOn)
        {
            MotionStatus ret = MotionStatus.None;
            if (Motion.IsVacuumArrive(vacuumName, isOn))
            {
                if (!VacArriveFlg[vacuumName.ToInt()])
                {
                    // ShowMsg($"{StationName}->真空：{vacuumName}{(isOn ? "开启" : "关闭")}到位");
                    VacArriveFlg[vacuumName.ToInt()] = true;
                }
                ret = MotionStatus.Arrived;
            }
            else if (Motion.IsVacuumTimeOut(vacuumName))
            {
                ShowMsg($"{StationName}->真空：{vacuumName}{(isOn ? "开启" : "关闭")}超时", MsgType.Error);
                AlarmManager.CreateAlarm(AlarmCode.VacuumTimeoutStart.ToInt() + vacuumName.ToInt(), StationNum);
                ret = MotionStatus.Timeout;
            }
            return ret;
        }
        public bool IsVacuumArriveEx(VacuumName vacuumName, bool isOn)
        {
            bool ret = Motion.IsVacuumArrive(vacuumName, isOn);

            return ret;
        }

        public void VacOff(VacuumName vacuumName)
        {
            VacuumInfo info = VacuumInfoHelper.GetVacuumInfo(vacuumName);
            if (info != null) {
                Motion.SetDigitalOutput(info.VacuumOffOutput, true);
                Motion.SetDigitalOutput(info.VacuumOnOutput, true);
            }
        }
        #endregion

        /// <summary>
        /// 读取寄存器变量值，isRaise为true时，表示读取上升沿
        /// </summary>
        /// <param name="regEnum"></param>
        /// <param name="isRaise"></param>
        /// <returns></returns>
        public bool GetBoolReg(RegisterEnum regEnum, bool isRaise = false)
        {
            if (RegisterService.Instance.IsManualMode)
            {
                return false;
            }
            var index = regEnum.ToInt();
            var value = RegisterService.Instance.GetBoolReg(regEnum);
            if (isRaise)
            {
                if (value && value != LastRegisterValue[index])
                {
                    LastRegisterValue[index] = value;
                    return value;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return value;
            }
        }
        public void ResetBoolReg(RegisterEnum regEnum, [CallerFilePath] string filePath = "", [CallerMemberName] string name = "", [CallerLineNumber] int num = 0)
        {
            if (RegisterService.Instance.IsManualMode)
            {
                return;
            }
            var index = regEnum.ToInt();
            RegisterService.Instance.ResetBoolReg(regEnum, filePath, name, num);
            LastRegisterValue[index] = false;
            ResetRegisterEvent?.Invoke(regEnum);
        }

    }
}
