using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Models
{
    [Serializable]
    [SugarTable("FoupTable")]
    public class FoupModel
    {
        public int TotalSlot { get; set; }
        /// <summary>
        /// Foup编号
        /// </summary>
        public int FoupNo { get; set; }
        public string Rfid { get; set; }= string.Empty;
        /// <summary>
        /// 剩余晶圆数量
        /// </summary>
        public int RemainWaferCount { get; set; }
        /// <summary>
        /// 当前层号
        /// </summary>
        public int CurSlotNo{ get; set; }
        /// <summary>
        /// 每层Wafer信息，0：无晶圆，1：满晶圆，2：空晶圆
        /// </summary>
        public string WaferMap { get; set; } = string.Empty;
        /// <summary>
        /// 进设备时间
        /// </summary>
        public DateTime InEqpTime { get; set; }
    }
}
