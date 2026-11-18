/*----------------------------------------------------------------
//公司名：   Suzhou JingCe Technology Co.,LTD
//版权申明： Copyright © 2025 JingCe. All Rights Reserved.
//
// =================================
// CLR版本     ：4.6.81.0
// 命名空间    ：
// 文件名称    ：
// =================================
// 创 建 者    ：guguangzhou
// 创建日期    ：2025/01/01 09:00:00 
// 功能描述    ：
// 使用说明    ：
//
//
// 创建标识：
//
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
using JC.PTIS.Main.Models;
using SqlSugar;
using StdLib.PCBaseFrame.Extension.Para;
using StdLib.PCBaseFrame.Extension.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace JC.PTIS.Main.Models
{
    [Serializable]
    [SugarTable("WaferTable")]
    public class WaferModel
    {
        // 头部信息
        public string DEVICE { get; set; }
        public string LOT { get; set; }
        public string WAFER { get; set; }
        public string FNLOC { get; set; }
        public int ROWCT { get; set; }
        public int COLCT { get; set; }
        [SugarColumn(IsIgnore = true)]
        public int[] BCEQU { get; set; }
        public int REFPX { get; set; }
        public int REFPY { get; set; }
        public string DUTMS { get; set; }
        public float XDIES { get; set; }
        public float YDIES { get; set; }
        [SugarColumn(IsIgnore = true)]
        public DieModel[,] DieArray { get; set; }
        // BIN 汇总
        public int TotalDie { get; set; }
        public int PassDie { get; set; }
        public int FailDie { get; set; }
        public decimal Yield { get; set; }

        /// <summary>
        /// 每行实际Die 列号数组，长度为 ROWCT，每个元素表示该行的实际列数
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public int[] RealDieColNoPerRow { get; set; }
        public int CurPickingRowNo { get; set; }
        public int CurPickingColNo { get; set; }
        /// <summary>
        /// 所在Foup序号
        /// </summary>
        public int FoupNo { get; set; }
        /// <summary>
        /// 所在Foup层号
        /// </summary>
        public int LayerNo { get; set; }
        public string MapFilePath { get; set; }
        public string Code { get; set; }
        public bool IsLastWafer { get; set; }
        /// <summary>
        /// 是否已经取完所有Die
        /// </summary>
        public bool IsPickEnd { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
