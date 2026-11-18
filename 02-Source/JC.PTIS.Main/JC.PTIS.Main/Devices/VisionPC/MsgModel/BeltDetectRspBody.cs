using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Devices.VisionPC.MsgModel
{
    public class BeltDetectRspBody: BodyBase
    {
        public bool DetectResult { get; set; }
    }
}
