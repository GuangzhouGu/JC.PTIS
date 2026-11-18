using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.DB
{
    public class DBResult
    {
        /// <summary>
        /// 成功则返回正确的格式  否则返回失败原因
        /// </summary>
        public object Result;

        /// <summary>
        /// 成功
        /// </summary>
        public bool Success = false;
    }
}
