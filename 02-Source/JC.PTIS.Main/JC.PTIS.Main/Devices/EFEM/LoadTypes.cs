using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.Map.Devices.EFEM
{
    /// <summary>
    /// 指定手动上料还是天车上料
    /// </summary>
    public enum LoadTypes
    {
        None,

        /// <summary>
        /// 手动上料
        /// </summary>
        LpLoad,
        /// <summary>
        /// 手动下料
        /// </summary>
        LpUnload,

        /// <summary>
        /// 天车上料
        /// </summary>
        OHTLoad,
        /// <summary>
        /// 天车下料
        /// </summary>
        OHTUnload
    }
}
