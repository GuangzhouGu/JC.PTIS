using JC.PTIS.Main.Models.ModelHelper;
using SqlSugar;
using SqlSugar.DbConvert;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using static StdLib.WindowsControls.HTRoundMeter;

namespace JC.PTIS.Main.Models
{
    [Serializable]
    [SugarTable("TrayTable")]
    public class TrayModel
    {
        [SugarColumn(IsPrimaryKey = true, IsIdentity = false)]
        public string TrayCode { get; set; } = string.Empty;
        /// <summary>
        /// 批次号
        /// </summary>
        public string LOT { get; set; } = string.Empty;
        public string WAFER { get; set; } = string.Empty;
        /// <summary>
        /// Die信息二维数组，行号从0开始，列号从0开始
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public DieModel[,] DieArray { get; set; }

        /// <summary>
        /// 每盘Tray的Die信息，使用一串字符串表示。-：空，0：有Die还未检测，1：OK Die，2：NG Die
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public string Map { get; set; } = string.Empty;
        public int BinLevel { get; set; }
        public int RowCount { get; set; }
        public int ColCount { get; set; }
        /// <summary>
        /// Tray状态，满Tray表示该Tray中所有位置都有Die
        /// </summary>
        [SugarColumn(ColumnDataType = "varchar(20)", SqlParameterDbType = typeof(EnumToStringConvert))]
        public TrayTypeEnum TrayType { get; set; }
        [SugarColumn(ColumnDataType = "varchar(20)", SqlParameterDbType = typeof(EnumToStringConvert))]
        public TrayStatusEnum TrayStatus { get; set; }
        /// <summary>
        /// 半Tray标志位：是否为OK Tray，OK Tray表示该Tray中所有位置的Die都是OK Die
        /// </summary>
        [SugarColumn(IsIgnore = true)]
        public bool IsOkTray { get; set; } = false;
        /// <summary>
        /// 投入设备时间
        /// </summary>
        public DateTime EqpInTime { get; set; }
        [SugarColumn(IsJson = true, IsNullable = true, ColumnDataType = "TEXT")]
        public string[,] DieCodeArray { get; set; }
        [SugarColumn(IsIgnore = true)]
        public bool IsExistNGDie
        { 
            get
            { 
                if (DieArray != null && DieArray.Length > 0)
                {
                    var list= DieArray.Cast<DieModel>().Where(d => d != null).ToList();
                    if (list.Count > 0)
                    {
                        var index= list.FindIndex(d => d.DieStatus == DieStatus.FrontNG || d.DieStatus == DieStatus.BackNG);
                        return index >= 0;
                    }
                }
                return false;
            } 
        }
        public void UpdateMap()
        {
            if (DieArray != null&&DieArray.Length>0)
            {
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < RowCount; i++)
                {
                    for (int j = 0; j < ColCount; j++)
                    {
                        var die = DieArray[i, j];
                        if (die == null)
                        {
                            sb.Append("-,");
                        }
                        else
                        {
                            if (die.DieStatus == DieStatus.FrontNG||die.DieStatus == DieStatus.BackNG)
                            {
                                sb.Append("2,");
                            }
                            else if(die.DieStatus.ToString().Contains("OK"))
                            {
                                sb.Append("1,");
                            }    
                            else 
                            {
                                sb.Append("0,");
                            }
                        }
                    }
                }
                Map = sb.ToString().TrimEnd(',');

                //根据TrayTypeEnum枚举的定义，结合Map给TrayType赋值
                var hasEmpty = Map.Contains("-");
                var hasUntested = Map.Contains("0");
                var hasOk = Map.Contains("1");
                var hasNg = Map.Contains("2");

                if (!hasUntested && !hasOk && !hasNg)
                {
                    TrayType= TrayTypeEnum.Empty;
                    TrayStatus = TrayStatusEnum.Empty;
                }
                else if (hasEmpty)
                {
                    TrayType = TrayTypeEnum.Half;
                    if (hasOk && !hasNg && !hasUntested)
                    {
                        TrayStatus = TrayStatusEnum.OkHalf;
                    }
                    else if (!hasOk && hasNg && !hasUntested)
                    {
                        TrayStatus = TrayStatusEnum.NgHalf;
                    }
                    else if (hasOk && hasNg && hasEmpty)
                    {
                        TrayStatus = TrayStatusEnum.MixHalf;
                    }
                    else 
                    {
                        TrayStatus = TrayStatusEnum.NoDetectHalf;
                    }
                }
                else
                {
                    TrayType = TrayTypeEnum.Full;
                    if (hasOk && !hasNg && !hasUntested)
                    {
                        TrayStatus = TrayStatusEnum.OkFull;
                    }
                    else if (!hasOk && hasNg && !hasUntested)
                    {
                        TrayStatus = TrayStatusEnum.NgFull;
                    }
                    else if (hasOk && hasNg && !hasUntested)
                    {
                        TrayStatus = TrayStatusEnum.MixFull;
                    }
                    else if (!hasOk && !hasNg && hasUntested)
                    {
                        TrayStatus = TrayStatusEnum.NoDetectFull;
                    }
                    else if (hasNg && hasUntested)
                    {
                        TrayStatus = TrayStatusEnum.NoDetectFull;
                    }
                }
                if (TrayStatus == TrayStatusEnum.OkFull||
                    TrayStatus == TrayStatusEnum.OkHalf)
                {
                    IsOkTray = true;
                }
                else
                {
                    IsOkTray = false;
                }
            }
        }
        public string[,]LoadMap()
        {
            if (string.IsNullOrEmpty(Map))
            {
                return null;
            }
            var arr = Map.Split(',');
            string[,] result = new string[RowCount, ColCount];
            for (int i = 0; i < RowCount; i++)
            {
                for (int j = 0; j < ColCount; j++)
                {
                    result[i, j] = arr[i * ColCount + j];
                }
            }
            return result;
        }
    }
}
