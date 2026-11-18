using JC.PTIS.Main.Devices.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Devices.VisionPC.MsgModel
{
    public class MotionRequestReqBody : BodyBase
    {
        public MotionType MotionType { get; set; }
        public Xyt MovePos { get; set; }
    }
    public enum MotionType
    {
        /// <summary>
        /// 相对运动
        /// </summary>
        Relative = 0,
        /// <summary>
        /// 相对运动
        /// </summary>
        Abs = 1,
    }
    public enum MotionStation
    {
        Wafer,
        Tray,
        Paper,
        Label,

    }
}
