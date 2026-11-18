using JC.PTIS.Common;
using JC.PTIS.Main;
using JC.PTIS.Main.Models;
using StdLib.LogHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public class WaferMapHelper:Singleton<WaferMapHelper>
{
    //WaferModel WaferMapData = null;
    //public WaferModel GetWaferModel()
    //{
    //    return WaferMapData;
    //}
    public  WaferModel Parse(string filePath)
    {
        try
        {
            if (File.Exists(filePath)==false)
            {
                return null;
            }
            var lines = File.ReadAllLines(filePath).Select(l => l.TrimEnd()).ToList();
            var wafer = new WaferModel();
        
            int mapStartLine = 0;
            int mapEndLine = 0;

            // 1. 解析头部
            for (int i = 0; i < lines.Count; i++)
            {
                var line = lines[i].Trim();
                if (line.StartsWith("DEVICE:")) wafer.DEVICE = line.Split(':')[1];
                else if (line.StartsWith("LOT:")) wafer.LOT = line.Split(':')[1];
                else if (line.StartsWith("WAFER:")) wafer.WAFER = line.Split(':')[1];
                else if (line.StartsWith("FNLOC:")) wafer.FNLOC = line.Split(':')[1];
                else if (line.StartsWith("ROWCT:")) wafer.ROWCT = int.Parse(line.Split(':')[1]);
                else if (line.StartsWith("COLCT:")) wafer.COLCT = int.Parse(line.Split(':')[1]);
                else if (line.StartsWith("BCEQU:"))
                {
                    var arr = line.Split(':');
                    if (arr[1].Length > 1 && arr[1].Contains(','))
                    {
                        var arr2 = arr[1].Split(',');
                        wafer.BCEQU = arr2.Select(int.Parse).ToArray();
                    }
                    else
                    {
                        wafer.BCEQU = new int[] { int.Parse(arr[1]) };
                    }
                }
                else if (line.StartsWith("REFPX:")) wafer.REFPX = int.Parse(line.Split(':')[1]);
                else if (line.StartsWith("REFPY:")) wafer.REFPY = int.Parse(line.Split(':')[1]);
                else if (line.StartsWith("DUTMS:")) wafer.DUTMS = line.Split(':')[1];
                else if (line.StartsWith("XDIES:")) wafer.XDIES = float.Parse(line.Split(':')[1]);
                else if (line.StartsWith("YDIES:")) wafer.YDIES = float.Parse(line.Split(':')[1]);
                else if (line.StartsWith("@") || line.StartsWith("."))
                {
                    mapStartLine = i;
                    break;
                }
            }
            wafer.RealDieColNoPerRow = Enumerable.Repeat(-1, wafer.ROWCT).ToArray();
            // 2. 定位 MAP 结束行
            for (int i = mapStartLine; i < lines.Count; i++)
            {
                if (lines[i].Contains("[ BIN SUMMARY]"))
                {
                    mapEndLine = i - 1;
                    break;
                }
            }

            // 3. 加载 BIN 矩阵
            int rowCount = wafer.ROWCT;
            int colCount = wafer.COLCT;
            wafer.DieArray = new DieModel[rowCount, colCount];
            int dieIndex = 0;
            for (int r = 0; r < rowCount; r++)
            {
                var line = lines[mapStartLine + r];
                for (int c = 0; c < colCount && c < line.Length; c++)
                {
                    dieIndex++;
                    DieModel die= new DieModel
                    {
                        DieCode = $"{wafer.DEVICE}_{wafer.LOT}_{wafer.WAFER}_{r}_{c}",
                        BinLevel = CharToBinLevel(line[c]),
                        DieStatus = DieStatus.InWafer,
                        InWaferRowNo = r,
                        InWaferColNo = c,
                        Index = dieIndex
                    };
                    die.IsRealDie = die.BinLevel>BinLevel.None && die.BinLevel < BinLevel.BinM;
                    wafer.DieArray[r, c] = die;
                    if (wafer.RealDieColNoPerRow[r] == -1 && die.IsRealDie)
                    {
                        wafer.RealDieColNoPerRow[r] = c;
                    }
                }
            }
            // 4. 解析 BIN SUMMARY
            var summary = lines.SkipWhile(x => !x.Contains("PassDie =")).Take(3).ToList();
            foreach (var line in summary)
            {
                if (line.StartsWith("PassDie"))
                {
                    var parts = line.Split(new[] { ' ', '=', '%' }, StringSplitOptions.RemoveEmptyEntries);
                    wafer.PassDie = int.Parse(parts[1]);
                    wafer.Yield = decimal.Parse(parts[2]);
                }
                else if (line.StartsWith("FailDie"))
                    wafer.FailDie = int.Parse(line.Split(new[] { ' ', '=' }, StringSplitOptions.RemoveEmptyEntries)[1]);
                else if (line.StartsWith("TotalDie"))
                    wafer.TotalDie = int.Parse(line.Split(new[] { ' ', '=' }, StringSplitOptions.RemoveEmptyEntries)[1]);
            }
            //WaferMapData = wafer;
            return wafer;
        }
        catch (Exception ex)
        {
            LogManager.GetLogger("WaferMapHelper").Info($"WaferMapHelper->Parse方法执行异常，异常信息：{ex}");
            return null;
        }
    }
    public  bool WaferSave(WaferModel waferMapData,string filePath)
    {
        bool result = false;
        //保存WaferMapData数据到文件filePath
        try
        {
            using (StreamWriter sw = new StreamWriter(filePath))
            {
                sw.WriteLine($"DEVICE:{waferMapData.DEVICE}");
                sw.WriteLine($"LOT:{waferMapData.LOT}");
                sw.WriteLine($"WAFER:{waferMapData.WAFER}");
                sw.WriteLine($"FNLOC:{waferMapData.FNLOC}");
                sw.WriteLine($"ROWCT:{waferMapData.ROWCT}");
                sw.WriteLine($"COLCT:{waferMapData.COLCT}");
                sw.WriteLine($"BCEQU:{waferMapData.BCEQU}");
                sw.WriteLine($"REFPX:{waferMapData.REFPX}");
                sw.WriteLine($"REFPY:{waferMapData.REFPY}");
                sw.WriteLine($"DUTMS:{waferMapData.DUTMS}");
                sw.WriteLine($"XDIES:{waferMapData.XDIES}");
                sw.WriteLine($"YDIES:{waferMapData.YDIES}");
                //写入DieArray
                for (int r = 0; r < waferMapData.ROWCT; r++)
                {
                    for (int c = 0; c < waferMapData.COLCT; c++)
                    {
                        var die = waferMapData.DieArray[r, c];
                        char binChar = BinLevelToChar(die.BinLevel);
                        sw.Write(binChar);
                    }
                    sw.WriteLine();
                }
                //写入BIN SUMMARY
                sw.WriteLine("[ BIN SUMMARY]");
                sw.WriteLine($"PassDie = {waferMapData.PassDie} ({waferMapData.Yield}%)");
                sw.WriteLine($"FailDie = {waferMapData.FailDie}");
                sw.WriteLine($"TotalDie = {waferMapData.TotalDie}");
                result = true;
            }
        }
        catch (Exception ex)
        {
            LogManager.GetLogger("WaferMapHelper").Info($"WaferSave方法执行异常，异常信息：{ex}");
            result = false;
        }
        return result;
    }
 
    private  BinLevel CharToBinLevel(char c)
    {
        switch (c)
        {
            case '1': return BinLevel.Bin1;
            case '2': return BinLevel.Bin2;
            case '3': return BinLevel.Bin3;
            case '4': return BinLevel.Bin4;
            case '5': return BinLevel.Bin5;
            case '6': return BinLevel.Bin6;
            case '7': return BinLevel.Bin7;
            case '8': return BinLevel.Bin8;
            case '9': return BinLevel.Bin9;
            case '@': return BinLevel.BinAt;
            case '.': return BinLevel.BinDot;
            default: return BinLevel.None;
        }
    }
    private  char BinLevelToChar(BinLevel binLevel)
    {
        switch (binLevel)
        {
            case BinLevel.Bin1: return '1';
            case BinLevel.Bin2: return '2';
            case BinLevel.Bin3: return '3';
            case BinLevel.Bin4: return '4';
            case BinLevel.Bin5: return '5';
            case BinLevel.Bin6: return '6';
            case BinLevel.Bin7: return '7';
            case BinLevel.Bin8: return '8';
            case BinLevel.Bin9: return '9';
            case BinLevel.BinAt: return '@';
            case BinLevel.BinM: return 'M';
            case BinLevel.BinDot: return '.';
            default: return '0'; // None or unknown
        }
    }
}