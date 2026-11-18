using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.LoadPort.Devices.EFEM
{
    /// <summary>
    /// 定义供应商提供的通信错误码
    /// </summary>
    public enum LPCodes
    {
        /// <summary>
        /// 没有错误，指令执行成功
        /// </summary>
        SUCCESS,

        /// <summary>
        /// 异常
        /// </summary>
        EXCEPTION,

        /// <summary>
        /// 和EFEM设备通信错误
        /// </summary>
        CommunicationError,

        /// <summary>
        /// 未知的错误码
        /// </summary>
        InvalidLpCode,

        /// <summary>
        /// 未知的消息格式
        /// </summary>
        InvalidMessageFormat,

        /// <summary>
        /// 设备没连接
        /// </summary>
        NotConnected,

        /// <summary>
        /// 不能执行切换Foup类型操作
        /// </summary>
        CannotSwitchFoupType,

        /// <summary>
        /// LoadPort正在执行上下料动作
        /// </summary>
        LpIsRunning,

        LpLoadNoFoup,
        LpUnloadNoFoup,
        OHTLoadHasFoup,
        OHTUnloadNoFoup,

        /// <summary>
        /// 等待VALID信号失败
        /// </summary>
        IO_VALID_ERROR,
        /// <summary>
        /// 读取CS_0或者CS_1信号失败
        /// </summary>
        IO_CSX_ERROR,
        /// <summary>
        /// CS0和CS1信号同时触发, 不支持的LoadPort上下料方式
        /// </summary>
        IO_CSX_INVALID,
        /// <summary>
        /// CS0和CS1信号都没触发, 不支持的LoadPort上下料方式
        /// </summary>
        IO_CSX_INVALID2,
        /// <summary>
        /// 上料写入L_REQ信号失败
        /// </summary>
        IO_L_REQ_ERROR,
        /// <summary>
        /// 下料写入U_REQ信号失败
        /// </summary>
        IO_U_REQ_ERROR,
        /// <summary>
        /// 等待TR_REQ信号失败
        /// </summary>
        IO_TR_REQ_ERROR,
        /// <summary>
        /// 写入READY信号失败
        /// </summary>
        IO_READY_ERROR,
        /// <summary>
        /// 等待BUSY信号失败
        /// </summary>
        IO_BUSY_ERROR,
        /// <summary>
        /// 等待Foup到位超时
        /// </summary>
        FOUP_PLACE_TIMEOUT,
        /// <summary>
        /// 等待Foup取走超时
        /// </summary>
        FOUP_TAKE_TIMEOUT,
        /// <summary>
        /// 等待COMPT完成信号失败
        /// </summary>
        IO_COMPT_ERROR,
        /// <summary>
        /// 等待CS_0信号重置失败
        /// </summary>
        IO_CS0_RESET_ERROR,
        /// <summary>
        /// 等待CS_1信号重置失败
        /// </summary>
        IO_CS1_RESET_ERROR,



        /// <summary>
        /// 未复位
        /// </summary>
        ORGYT,

        /// <summary>
        /// 系统异常
        /// </summary>
        ERROR,

        /// <summary>
        /// 系统忙碌中
        /// </summary>
        CBUSY,

        /// <summary>
        /// 急停
        /// </summary>
        EMO,

        /// <summary>
        /// 未切换到手动模式
        /// </summary>
        MENTE,

        /// <summary>
        /// 未切换到自动模式
        /// </summary>
        ONMGV,

        /// <summary>
        /// 气压异常
        /// 
        /// 正压异常 IN12 正压表信号
        /// 负压异常 IN13 负压表信号
        /// </summary>
        AIRSN,

        /// <summary>
        /// 凸片
        /// </summary>
        PROTS,

        /// <summary>
        /// 底座解锁异常
        /// </summary>
        CIOPS,

        /// <summary>
        /// 底座上锁异常
        /// </summary>
        CICLS,

        /// <summary>
        /// 平台移动异常
        /// </summary>
        YLMIT,

        /// <summary>
        /// 门移动异常（门前倾/后仰）
        /// </summary>
        DLMIT,

        /// <summary>
        /// 钥匙开门异常
        /// </summary>
        DROPS,

        /// <summary>
        /// 钥匙关门异常
        /// </summary>
        DRCLS,

        /// <summary>
        /// MAPARM动作异常
        /// </summary>
        MPBAR,

        /// <summary>
        /// FOUP加载错误
        /// </summary>
        DRDKE,

        /// <summary>
        /// 真空开关异常
        /// </summary>
        VACCS,

        /// <summary>
        /// 门钥匙状态异常
        /// </summary>
        LATCH,

        /// <summary>
        /// 料盒放置异常
        /// </summary>
        FPILG,

        /// <summary>
        /// 门位置异常
        /// </summary>
        DPOSI,

        /// <summary>
        /// 平台位置异常
        /// </summary>
        YDOOR,

        /// <summary>
        /// Z轴位置异常
        /// </summary>
        ZPOSI,

        /// <summary>
        ///  Z轴移动异常
        /// </summary>
        ZLIMIT,

        /// <summary>
        /// 障碍物异常
        /// </summary>
        SAFTY,

        /// <summary>
        /// 互锁信号
        /// </summary>
        SafeInterLock,

        /// <summary>
        /// 顶PIN气缸异常
        /// </summary>
        PNBAR
    }
}
