using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Devices.VisionPC.MsgModel
{
    public class BodyBase: IMsgBody
    {
        public int Code { get; set; }
        public int StnNo { get; set; }
        public string Station { get; set; }
    }
}
