using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.LoadPort.Devices.EFEM
{
    public static class E84IOReader
    {
        /// <summary>
        /// 读取指定端口状态
        /// </summary>
        /// <param name="portIndex">从0开始的端口号</param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static bool ReadPortState(int loadPortNum, E84Ports portIndex, out bool state)
        {
            state = false;

            return false;
        }

        /// <summary>
        /// 写入指定端口状态
        /// </summary>
        /// <param name="portIndex">从0开始的端口号</param>
        /// <param name="state"></param>
        /// <returns></returns>
        public static bool WritePortState(int loadPortNum, E84Ports portIndex, bool state)
        {
            return false;
        }
    }
}
