using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.LoadPort.Devices.EFEM
{
    /// <summary>
    /// 定义插槽状态
    /// </summary>
    public enum SlotStates
    {
        /// <summary>
        /// 没有Wafer
        /// </summary>
        NoWafter,

        /// <summary>
        /// 有Wafer
        /// </summary>
        HasWafter,

        /// <summary>
        /// 叠片
        /// </summary>
        Overlap,

        /// <summary>
        /// 斜插
        /// </summary>
        Tilted = 'w'
    }
}
