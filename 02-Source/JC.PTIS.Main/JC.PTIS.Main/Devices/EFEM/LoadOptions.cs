using JC.PTIS.LoadPort.Devices.EFEM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.Map.Devices.EFEM
{
    public class LoadOptions
    {
        /// <summary>
        /// 载具类型
        /// </summary>
        public FoupTypes FoupType { get; set; }
    }

    public class OHTLoadOptions
    {
        /// <summary>
        /// 载具类型
        /// </summary>
        public FoupTypes FoupType { get; set; }

        /// <summary>
        /// 不需要赋值
        /// </summary>
        public LoadTypes LoadType { get; set; }
    }
}
