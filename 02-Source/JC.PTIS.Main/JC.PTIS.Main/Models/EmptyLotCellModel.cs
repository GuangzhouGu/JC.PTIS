using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Models
{
    [Serializable]
    public class EmptyLotCellModel: LotCellBase
    {
        /// <summary>
        /// 剩余Tray数量
        /// </summary>
        public int TrayCount { get; set; }
        public List<TrayModel> TrayList { get; set; }
        public override void UpdateMap()
        {
            var temp=Enumerable.Repeat(-1, TrayCount).ToArray();
            var arr=Array.ConvertAll(temp, x => x.ToString());
            if (TrayList != null && TrayList.Count > 0)
            {
                for (int i = 0; i < TrayList.Count; i++)
                {
                    arr[i] = "0";
                }
            }
            Map=string.Join(",", arr);
        }
        public override string[] LoadMap()
        {
            if (string.IsNullOrEmpty(Map))
            {
                return null;
            }
            return Map.Split(',');
        }
    }
}
