using JC.PTIS.Main.FlowInfo;
using JC.PTIS.Main.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.RotateInfo
{
    /// <summary>
    /// 下料Tray平台信息
    /// </summary>
    [Serializable]
    public class TrayStageInfo: FlowInfoBase
    {
        public int StageNo { get; set; }
        public TrayPosEnum TrayPos { get; set; }
        public TrayModel TrayModel { get; set; }

    }
  
}
