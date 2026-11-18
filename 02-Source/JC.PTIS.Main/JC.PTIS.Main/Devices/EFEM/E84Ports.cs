using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.LoadPort.Devices.EFEM
{
    public enum E84Ports
    {
        /// <summary>
        /// Load 请求（放 FOUP）
        /// </summary>
        OUTPUT_L_REQ,

        /// <summary>
        /// Unload 请求（取 FOUP）
        /// </summary>
        OUTPUT_U_REQ,
        /// <summary>
        /// Passive 就绪，允许交接
        /// </summary>
        OUTPUT_READY,
        OUTPUT_VA_STAR,
        OUTPUT_VS_0,
        OUTPUT_VS_1,
        OUTPUT_HO_AVBL,
        OUTPUT_ES,




        /// <summary>
        /// Active 有效（对位完成，链路建立）
        /// </summary>
        INPUT_VALID,

        /// <summary>
        /// 端口选择 Bit0
        /// </summary>
        INPUT_CS_0,

        /// <summary>
        /// 端口选择 Bit1
        /// </summary>
        INPUT_CS_1,
        INPUT_AM_AVBL,

        /// <summary>
        /// Active 交接请求
        /// </summary>
        INPUT_TR_REQ,

        /// <summary>
        /// Active 机械动作执行中
        /// </summary>
        INPUT_BUSY,

        /// <summary>
        /// 本次交接完成确认
        /// </summary>
        INPUT_COMPT,
        INPUT_CONT
    }
}
