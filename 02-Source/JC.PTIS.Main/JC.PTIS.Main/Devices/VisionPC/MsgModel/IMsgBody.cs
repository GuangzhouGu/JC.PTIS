using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Devices.VisionPC.MsgModel
{
    public interface IMsgBody
    {
        /// <summary>
        /// 返回值，0表示成功，非0表示失败
        /// </summary>
        int Code { get; set; }
        int StnNo { get; set; }
        string Station { get; set; }
    }
}
