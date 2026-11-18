using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Models
{
    [Serializable]
    public class IRNgTrayModel
    {
        public DieModel[] DieArray { get; set; }=new DieModel[13];
        public string Map { get; set; }
        public void UpdateMap()
        {
            var arr = Enumerable.Repeat(-1, DieArray.Length).ToArray();//-1：空
            var stringArr = Array.ConvertAll(arr, x => x.ToString());
            if (DieArray != null && DieArray.Length > 0)
            {
                for (int i = 0; i < DieArray.Length; i++)
                {
                    if (DieArray[i] != null)
                    {
                        stringArr[i] = DieArray[i].BinLevel.ToString();
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
