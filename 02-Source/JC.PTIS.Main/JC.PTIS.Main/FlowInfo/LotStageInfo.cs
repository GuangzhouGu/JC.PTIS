using JC.PTIS.Main.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.FlowInfo
{
    public class LotStageInfo:FlowInfoBase
    {
        public int StageNo { get; set; }
        public LotModel LotModel { get; set; }
        public LotStageType LotStageType { get; set; }
        public bool IsEmpty { get; set; }
        //public bool IsFull { get; set; }
        public LotPosEnum LotPos { get; set; }
        public bool IsBuzy { get; set; }
    }
  
}
