using JC.PTIS.Main.Devices.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Devices.VisionPC.MsgModel
{
    public class MotionRequestRspBody : BodyBase
    {
        public MotionType MotionType { get; set; }
        public Xyt AxisPos { get; set; }
        public string FaultMessage { get; set; } = "";
    }
   
}
