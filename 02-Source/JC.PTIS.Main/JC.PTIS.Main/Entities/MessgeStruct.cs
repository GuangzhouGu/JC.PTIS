using JC.PTIS.Main.Devices.Vision;
using JC.PTIS.Main.Devices.VisionPC.MsgModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Devices.VisionPC
{
    public class MessgeStruct
    {
        public Header Header { get; set; }
        public IMsgBody Body { get; set; }
    }
    public class Header
    {
        static int CmdID { get; set; }
        public string ProtocolType { get; set; }
        public string RequestId { get; set; }
        public string TriggerId { get; set; }
        public string MsgType { get; set; }
        public string Cmd { get; set; }
        public string Timestamp { get; set; }
        public Header(VisionCmd cmd)
        {
            Cmd = cmd.ToString();
            ProtocolType = "TCP";
            RequestId = "00000000";
            TriggerId = "TriggerId_001";
            MsgType = "Request";
            Timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
            if (cmd != VisionCmd.Heartbeat)
            {
                CmdID++;
                RequestId = CmdID.ToString("D8");
                TriggerId = $"TriggerId_{RequestId}";
                if (CmdID >= 99999999)
                {
                    CmdID = 1;
                }
            }
        }
    }
}
