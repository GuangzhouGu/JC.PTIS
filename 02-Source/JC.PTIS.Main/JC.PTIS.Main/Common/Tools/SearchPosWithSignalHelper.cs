using StdLib.PCBaseFrame.Motion;
using StdLib.PCBaseFrame.Station;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JC.PTIS.Common.Tools
{
    public class SearchPosWithSignalHelper:Singleton<SearchPosWithSignalHelper>
    {
        private readonly MotionManager Motion= MotionManager.GetInstance();
        public (bool, double) StartSearchWithMultiTimes(string axisName, string signalName, bool isEnableLimitPos=false,double limitPos=0,int timeout = 10000)
        {
            bool res = false;
            double triggerPos=0;
            IOInfo iOInfo = IOInfoHelper.GetInputInfoByName(signalName, out var ioModuleInfo);
            var watch = Stopwatch.StartNew();
            if (!Motion.IsHomeDone(axisName))
            {
                return (false, 0);
            }
            Motion.Jog(axisName, 20);
            while(watch.ElapsedMilliseconds < timeout)
            {
                if (iOInfo.GetRising())
                {
                    triggerPos =Motion.GetAxisActPos(axisName);
                    Motion.Stop(axisName);
                    res = true;
                    break;
                }
                else if (isEnableLimitPos && Motion.GetAxisActPos(axisName) > limitPos)
                {
                    Motion.Stop(axisName);
                    res = false;
                    break;
                }
                Task.Delay(1).Wait();
            }
            Motion.Stop(axisName);
            if (res)
            {
                res= false;
                watch.Restart();
                //脱离信号后停止运动
                Motion.Jog(axisName, -5);
                while (watch.ElapsedMilliseconds < timeout)
                {
                    if (iOInfo.GetFalling())
                    {
                        Motion.Stop(axisName);
                        res= true;
                        break;
                    }
                    Task.Delay(10).Wait();
                }
                Motion.Stop(axisName);
            }
            if (res)
            {
                //再次低速碰到传感器，监控上升沿
                Motion.Jog(axisName, 2);
                while (watch.ElapsedMilliseconds < timeout)
                {
                    if (iOInfo.GetRising())
                    {
                        triggerPos = Motion.GetAxisActPos(axisName);
                        Motion.Stop(axisName);
                        res = true;
                        break;
                    }
                    else if (isEnableLimitPos && Motion.GetAxisActPos(axisName) > limitPos)
                    {
                        Motion.Stop(axisName);
                        res = false;
                        break;
                    }
                    Task.Delay(1).Wait();
                }
                Motion.Stop(axisName);
            }
            return (res, triggerPos);
        }
        public (bool, double) StartSearchWithOnce(string axisName, string signalName,int timeout=10000)
        {
            bool res = false;
            double triggerPos = -999;
            IOInfo iOInfo = IOInfoHelper.GetInputInfoByName(signalName, out var ioModuleInfo);
            var watch = Stopwatch.StartNew();
            if (!Motion.IsHomeDone(axisName))
            {
                return (false, -999);
            }
            Motion.Jog(axisName, 10);
            while (watch.ElapsedMilliseconds < timeout)
            {
                if (iOInfo.GetRising())
                {
                    Motion.Stop(axisName);
                    triggerPos = Motion.GetAxisActPos(axisName);
                    res = true;
                    break;
                }
                Task.Delay(1).Wait();
            }
            Motion.Stop(axisName);
            return (res, triggerPos);
        }
    }
}
