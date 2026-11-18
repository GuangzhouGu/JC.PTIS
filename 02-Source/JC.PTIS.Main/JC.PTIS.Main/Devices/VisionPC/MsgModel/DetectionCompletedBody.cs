using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Devices.VisionPC.MsgModel
{
    [Serializable]
    public class DetectionCompletedBody : BodyBase
    {
        public string LotId { get; set; }
        public int BatchCount { get; set; }
        public int TotalCostTime { get; set; }
        public ItemResult[] ItemResultList { get; set; }
    }
    [Serializable]
    public class ItemResult
    {
        public int Index { get; set; }
        public string SN { get; set; }
        public string Result { get; set; }
        public Defect[] Defects { get; set; }
    }
    [Serializable]
    public class Defect
    {
        public string DefectId { get; set; }
        public string DefectType { get; set; }
        public string DefectSize { get; set; }
        public string DefectImageUrl { get; set; }
    }
}
