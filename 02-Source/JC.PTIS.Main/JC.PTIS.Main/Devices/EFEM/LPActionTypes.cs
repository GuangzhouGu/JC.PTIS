using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.LoadPort.Devices.EFEM
{
    /// <summary>
    /// LoadPort的动作类型
    /// </summary>
    public enum LPActionTypes
    {
        /// <summary>
        /// 当前是空闲状态
        /// </summary>
        Idle,

        /// <summary>
        /// 下料中
        /// </summary>
        Unloading,

        /// <summary>
        /// 上料中
        /// </summary>
        Loading
    }
}
