using JC.PTIS.Main.Devices.Vision;
using SqlSugar;
using SqlSugar.DbConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Models
{
    [Serializable]
    [SugarTable("DieTable")]
    public class DieModel
    {
        /// <summary>
        /// Die条码号，唯一标识一个Die，由Lot号+Wafer号+Die行号+Die列号组成
        /// </summary>
        //设置为主键，且不自增
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public string DieCode { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        public string LOT { get; set; } = string.Empty;
        public string WAFER { get; set; } = string.Empty;
        [SugarColumn(ColumnDataType = "varchar(10)", SqlParameterDbType = typeof(EnumToStringConvert))]
        public BinLevel BinLevel { get; set; }
        [SugarColumn(ColumnDataType = "varchar(10)", SqlParameterDbType = typeof(EnumToStringConvert))]
        public DieStatus DieStatus { get; set; }
        [SugarColumn(IsIgnore = true)]
        public int Index { get; set; }
        /// <summary>
        /// 在Wafer中的行号，从0开始计数
        /// </summary>
        public int InWaferRowNo { get; set; }
        /// <summary>
        /// 在Wafer中的列号，从0开始计数
        /// </summary>
        public int InWaferColNo { get; set; }
        /// <summary>
        /// 在InTray中的行号，从0开始计数
        /// </summary>
        public int InTrayRowNo { get; set; }
        /// <summary>
        /// 在InTray中的列号，从0开始计数
        /// </summary>
        public int InTrayColNo { get; set; }

        /// <summary>
        /// 是否是实际的Die，实际的Die是指在Wafer中存在的Die，而非虚拟的Die
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool IsRealDie { get; set; }
        /// <summary>
        /// 取Die开始时间
        /// </summary>
        public DateTime StartTime { get; set; }
        /// <summary>
        /// 取Die结束时间
        /// </summary>
        public DateTime EndTime { get; set; }
        /// <summary>
        /// 取Die耗时，EndTime-StartTime
        /// </summary>
        public double SpanTime { get; set; }
        /// <summary>
        /// 对位补正值
        /// </summary>
        [SugarColumn(IsJson = true, IsNullable = true, ColumnDataType = "TEXT")]
        public Xyt Offset { get; set; }
        /// <summary>
        /// 背面检测结果，OK或者NG
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar(5)", SqlParameterDbType = typeof(EnumToStringConvert))]
        public Result BackDetectResult { get; set; }
        /// <summary>
        /// 隐裂检测结果，OK或者NG
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar(5)", SqlParameterDbType = typeof(EnumToStringConvert))]
        public Result IRDetectResult { get; set; }
        /// <summary>
        /// 正面检测结果，OK或者NG
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar(5)", SqlParameterDbType = typeof(EnumToStringConvert))]
        public Result FrontDetectResult { get; set; }
        /// <summary>
        /// 总结果，OK或者NG
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar(5)", SqlParameterDbType = typeof(EnumToStringConvert))]
        public Result TotalResult { get; set; }

    }
}
