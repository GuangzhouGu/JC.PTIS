using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.LoadPort.Devices.EFEM
{
    /// <summary>
    /// 上下料状态定义
    /// </summary>
    public enum LoadStatus
    {
        /// <summary>
        /// 上料或者下料成功
        /// </summary>
        SUCCESS,

        /// <summary>
        /// IO通信错误
        /// </summary>
        IO_ERROR,

        /// <summary>
        /// 无效的上下料端口
        /// </summary>
        INVALID_PORT,

        /// <summary>
        /// 非法的端口选择
        /// </summary>
        INVALID_SELECTION,

        /// <summary>
        /// 运行出现异常
        /// </summary>
        EXCEPTION,

        /// <summary>
        /// Map错误
        /// </summary>
        MAP_FAILED,

        /// <summary>
        /// 放置Foup超时
        /// </summary>
        PLACE_FOUP_TIMEOUT,

        /// <summary>
        /// 取走Foup超时
        /// </summary>
        TAKE_FOUP_TIMEOUT,

        /// <summary>
        /// LoadPort执行Loading动作失败
        /// </summary>
        LP_LOADING_FAILED,

        /// <summary>
        /// LoadPort执行Unloading动作失败
        /// </summary>
        LP_UNLOADING_FAILED
    }
}
