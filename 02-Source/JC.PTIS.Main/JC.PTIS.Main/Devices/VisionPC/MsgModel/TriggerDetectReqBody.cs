using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Devices.VisionPC.MsgModel
{
    public class TriggerDetectReqBody : IMsgBody
    {
        public string Station { get; set; }
        public int Code { get; set; }
        public int StnNo { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        public string LotId { get; set; }
        /// <summary>
        /// 整批通用产品型号ID，整批同型号时可统一传
        /// </summary>
        public string ProductId { get; set; }
        /// <summary>
        /// 检测配方名
        /// </summary>
        public string RecipeName { get; set; }
        /// <summary>
        /// 批量检测数量
        /// </summary>
        public int BatchCount { get; set; }
        /// <summary>
        /// 产品信息列表
        /// </summary>
        public Product[] ProductList { get; set; }
        /// <summary>
        /// 缺陷图片保存路径
        /// </summary>
        public string DefectImageUrl { get; set; }
    }
    public class Product
    {
        /// <summary>
        /// 产品索引
        /// </summary>
        public int Index { get; set; }
        public string SN { get; set; }
    }
}
