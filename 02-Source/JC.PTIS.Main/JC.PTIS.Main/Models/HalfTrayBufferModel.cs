using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Models
{
    [Serializable]
    public class HalfTrayBufferModel
    {
        public TrayModel[] TrayArray { get; set; }=new TrayModel[12];
        public string Map { get; set; }
        public void UpdateMap()
        {
            var arr = Enumerable.Repeat(-1, TrayArray.Length).ToArray();//-1：空
            var stringArr = Array.ConvertAll(arr, x => x.ToString());
            if (TrayArray != null && TrayArray.Length > 0)
            {
                for (int i = 0; i < TrayArray.Length; i++)
                {
                    if (TrayArray[i] != null)
                    {
                        stringArr[i] = TrayArray[i].BinLevel.ToString();
                        stringArr[i] += TrayArray[i].IsOkTray ? "OK" : "NG";
                    }
                }
            }
            Map = string.Join(",", stringArr);
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
