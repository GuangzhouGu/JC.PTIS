using SqlSugar;
using SqlSugar.DbConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Models
{
    [Serializable]
    [SugarTable("LotTable")]
    public class LotModel
    {
        public readonly int TotalCount = 6;
        /// <summary>
        /// Lot内盒条码+在内盒中的行号+在内盒中的列号
        /// </summary>
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public string LotCode { get; set; }
        /// <summary>
        /// 批次
        /// </summary>
        public string LOT { get; set; } = string.Empty;
        public string WAFER { get; set; } = string.Empty;
        [SugarColumn(IsIgnore = true)]
        public List<TrayModel> TrayList { get; set; } = new List<TrayModel>();

        /// <summary>
        /// 每个Lot中Tray的Map信息，格式为Bin等级，用逗号分隔，表示每个Tray中Die的Bin等级分布情况，例如：1,2,3,4,5,6，表示该Lot中有6个Tray，分别对应Bin等级1到6
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string TrayMap { get; set; }
        public int TrayCount { get; set; }
        [SugarColumn(ColumnDataType = "varchar(20)", SqlParameterDbType = typeof(EnumToStringConvert))]
        public BinLevel BinLevel { get; set; }
        [SugarColumn(IsIgnore = true)]
        public LotStatusEnum LotStatus { get; set; }
        [SugarColumn(IsJson = true, IsNullable = true, ColumnDataType = "TEXT")]
        public Result[] BeltDetectResult { get; set; } = new Result[2];
        /// <summary>
        /// 投入设备时间
        /// </summary>
        public DateTime EqpInTime { get; set; }
        [SugarColumn(IsJson = true, IsNullable = true, ColumnDataType = "TEXT")]
        public string[] TrayCodeArray { get; set; }
        public void UpdateMap()
        {
            var arr = Enumerable.Repeat(-1, TotalCount).ToArray();//0：空Tray
            if (TrayList != null && TrayList.Count > 0)
            {
                if (TrayCodeArray == null)
                {
                    TrayCodeArray = new string[TrayList.Count];
                }
                for (int i = 0; i < TrayList.Count; i++)
                {
                    if (TrayList[i] != null)
                    {
                        arr[i] = TrayList[i].BinLevel;
                        TrayCodeArray[i] = TrayList[i].TrayCode;
                    }
                    else
                    {
                        TrayCodeArray[i] = string.Empty;
                    }
                }
            }
            TrayMap = string.Join(",", arr);
        }
        public string[] LoadMap()
        {
            if (string.IsNullOrEmpty(TrayMap))
            {
                return null;
            }
            return TrayMap.Split(',');
        }
    }
}
