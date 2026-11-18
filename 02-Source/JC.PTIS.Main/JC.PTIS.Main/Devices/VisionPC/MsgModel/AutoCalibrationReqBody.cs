using JC.PTIS.Main.Devices.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Devices.VisionPC.MsgModel
{
    public class AutoCalibrationReqBody : BodyBase
    {
        public Xyt MovePos { get; set; }
    }
}
