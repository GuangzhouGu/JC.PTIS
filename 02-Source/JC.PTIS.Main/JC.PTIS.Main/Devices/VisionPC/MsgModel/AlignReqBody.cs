using JC.PTIS.Main.Devices.Vision;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Devices.VisionPC.MsgModel
{
    public class AlignReqBody : IMsgBody
    {
        public string Station { get; set; }
        public int StnNo { get; set; }
        public Xyt AxisPos { get; set; }
        public int Code { get; set; }
    }
}
