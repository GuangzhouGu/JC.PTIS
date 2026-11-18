using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.PTIS.Main.Models
{
    [Serializable]
    public class LotCellBase
    {
        public string LotCellCode { get; set; }
        /// <summary>
        /// 在料盒中的行号
        /// </summary>
        public int RowNo { get; set; }
        /// <summary>
        /// 在料盒中的列号
        /// </summary>
        public int ColNo { get; set; }

        /// <summary>
        /// 用于标记料盒中每个穴位的状态，-1表示空穴位，0表示空托盘，1表示满托盘
        /// </summary>
        public string Map { get; set; }
        public LotBoxType LotBoxType { get; set; }
        /// <summary>
        /// 剩余Tray数量
        /// </summary>
        public int TrayCount { get; set; }
        public List<TrayModel> TrayList { get; set; }

        public int LotCount { get; set; }
        public bool IsFull { get; set; }
        public List<LotModel> LotList { get; set; }
        public BinLevel BinLevel { get; set; }
        public virtual void UpdateMap()
        {
            if (LotBoxType== LotBoxType.EmptyTray)
            {
                var temp = Enumerable.Repeat(-1, TrayCount).ToArray();
                var arr = Array.ConvertAll(temp, x => x.ToString());
                if (TrayList != null && TrayList.Count > 0)
                {
                    for (int i = 0; i < TrayList.Count; i++)
                    {
                        arr[i] = "0";
                    }
                }
                Map = string.Join(",", arr);
            }
            else if (LotBoxType == LotBoxType.Bin135 || LotBoxType == LotBoxType.Bin246)
            {
                var temp = Enumerable.Repeat(-1, LotCount).ToArray();
                var arr = Array.ConvertAll(temp, x => x.ToString());
                if (LotList != null && LotList.Count > 0)
                {
                    for (int i = 0; i < LotList.Count; i++)
                    {
                        arr[i] = "1";
                    }
                }
                Map = string.Join(",", arr);
            }

        }
        public virtual string[] LoadMap()
        {
            if (string.IsNullOrEmpty(Map))
            {
                return null;
            }
            return Map.Split(',');
        }

    }
}
