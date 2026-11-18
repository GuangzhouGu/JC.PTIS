using JC.PTIS.Main.FlowInfo;
using JC.PTIS.Main.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.RotateInfo
{
    [Serializable]
    public class PickerInfo: FlowInfoBase
    {
        /// <summary>
        /// 所转动到的工位号
        /// </summary>
        public int StnNo { get; set; }
        /// <summary>
        /// 吸嘴
        /// </summary>
        public int PickNo { get; set; } = -1;
        /// <summary>
        /// 吸嘴上的Die信息
        /// </summary>
        public DieModel DieModel { get; set; }
    }
}
