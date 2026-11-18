using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Models
{
    [Serializable]
    [SugarTable("LotBoxTable")]
    public class LotBoxModel
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public string LotBoxCode { get; set; }
        /// <summary>
        /// 批次号
        /// </summary>
        public string LOT { get; set; } = string.Empty;
        public string WAFER { get; set; } = string.Empty;
        /// <summary>
        /// 穴位行数
        /// </summary>
        public int RowCount { get; set; }
        /// <summary>
        /// 穴位列数
        /// </summary>
        public int ColCount { get; set; }
        [SugarColumn(IsIgnore = true)]
        public LotCellBase[,] LotCellArray { get; set; }
        [SugarColumn(IsJson = true, IsNullable = true, ColumnDataType = "TEXT")]
        public string[,] LotCodeArray { get; set; }
        public LotBoxType LotBoxType { get; set; }
        [SugarColumn(IsIgnore = true)]
        public bool IsFull { get; set; }
        public string Rfid { get; set; }
        /// <summary>
        /// 用于标记料盒中每个穴位的状态，-1表示空穴位，1表示有托盘
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string Map { get; set; }
        public void UpdateMap()
        {
            var temp = Enumerable.Repeat(-1, RowCount * ColCount).ToArray();
            var arr = Array.ConvertAll(temp, x => x.ToString());
            if (LotCellArray != null)
            {
                for (int i = 0; i < RowCount; i++)
                {
                    for (int j = 0; j < ColCount; j++)
                    {
                        var cell = LotCellArray[i, j];
                        if (cell != null)
                        {
                            arr[i * ColCount + j] = "1";
                        }
                    }
                }
            }
            Map = string.Join(",", arr);
            IsFull = !arr.Contains("-1");
        }
        public string[] LoadMap()
        {
            if (string.IsNullOrEmpty(Map))
            {
                return null;
            }
            return Map.Split(',');
        }
    }
}
