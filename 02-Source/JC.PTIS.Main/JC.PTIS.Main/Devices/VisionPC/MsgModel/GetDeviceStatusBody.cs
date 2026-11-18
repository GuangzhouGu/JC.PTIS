using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Devices.VisionPC.MsgModel
{
    public class GetDeviceStatusReqBody : BodyBase
    {
        public string Status { get; set; }
        /// <summary>
        /// 设备序列号
        /// </summary>
        public string DeviceSN { get; set; }
        /// <summary>
        /// 当前加载的检测配方名
        /// </summary>
        public string CurrentRecipe { get; set; }
        /// <summary>
        /// 今日累计检测总数
        /// </summary>
        public int TodayTotalCount { get; set; }
        /// <summary>
        /// 今日累计不良数
        /// </summary>
        public int TodayNgCount { get; set; }
        /// <summary>
        /// 故障信息
        /// </summary>
        public string FaultMessage { get; set; }
    }
}
