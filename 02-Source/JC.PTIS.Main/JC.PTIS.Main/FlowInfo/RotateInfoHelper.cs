using JC.PTIS.Common;
using JC.PTIS.Main.Service;
using StdLib.WindowsControls.Class;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.RotateInfo
{
    /// <summary>
    /// 转塔物流信息帮助类
    /// </summary>
    public class RotateInfoHelper : Singleton<RotateInfoHelper>
    {
        public event Action<ConcurrentDictionary<int, LotBoxType>> LotBoxInfoChanged;
        public event Action<RotateStnType, int[]> RotateInfoChanged;
        ConcurrentDictionary<int, int> LoadDieStnDic = new ConcurrentDictionary<int, int>();  //键：每个转塔的工位号，值：每个转塔的吸嘴编号
         ConcurrentDictionary<int, int> Stn4TurnTableDic = new ConcurrentDictionary<int, int>();//键：每个转塔的工位名称，值：每个平台编号
         ConcurrentDictionary<int, int> PickPatchStnDic = new ConcurrentDictionary<int, int>();
         ConcurrentDictionary<int, int> Stn8TurnTableDic = new ConcurrentDictionary<int, int>();//键：每个转塔的工位名称，值：每个平台编号
        ConcurrentDictionary<int, LotBoxType> LotBoxStnDic = new ConcurrentDictionary<int, LotBoxType>();
        const double LoadDieDDStepAngle = 22.5;
        const double Stn4DDStepAngle = 90;//4工位转塔每个工位对应的角度
        const double Stn8DDStepAngle = 45;//8工位转塔
        const double PickPatchDDStepAngle =22.5;//挑补转塔
        const int LoadDieStnCount = 16;
        const int Stn8TurnTableStnCount = 8;
        const int Stn4TurnTableStnCount = 4;
        const int PickPatchStnCount = 16;
        const int LotBoxStnCount = 6;
        //const int HalfTrayStnCount = 2;
        public  void Init()
        {
            //转塔角度在0度时，工位号和吸嘴编号的映射关系
            for (int i = 0; i < LoadDieStnCount; i++)//16个吸嘴，虚拟出16个工位
            {
                LoadDieStnDic.TryAdd(i,i);
                PickPatchStnDic.TryAdd(i, i);
                if (i< Stn8TurnTableStnCount)
                {
                    Stn8TurnTableDic.TryAdd(i, i);
                    if (i < Stn4TurnTableStnCount)
                    {
                        Stn4TurnTableDic.TryAdd(i, i);
                    }
                    if (i< LotBoxStnCount)
                    {
                        LotBoxStnDic.TryAdd(i, (LotBoxType)i);
                    }
                }
            }
        }
        /// <summary>
        /// 获取转塔工位号对应的吸嘴编号或平台编号
        /// </summary>
        /// <param name="stnNo">工位号</param>
        /// <param name="RotateStnType">转塔类型</param>
        /// <returns></returns>
        public int GetStageNo(int stnNo, RotateStnType RotateStnType)
        {
            int no = -1;
            switch (RotateStnType)
            {
                case RotateStnType.LoadDie:
                    {
                        LoadDieStnDic.TryGetValue(stnNo, out no);
                    }
                    break;
                case RotateStnType.Stn4TurnTable:
                    {
                        Stn4TurnTableDic.TryGetValue(stnNo, out no);
                    }
                    break;
                case RotateStnType.PickPatch:
                    {
                        PickPatchStnDic.TryGetValue(stnNo, out no);
                    }
                    break;
                case RotateStnType.Stn8TurnTable:
                    {
                        Stn8TurnTableDic.TryGetValue(stnNo, out no);
                    }
                    break;
            
                case RotateStnType.LotBox:
                    {
                        LotBoxStnDic.TryGetValue(stnNo, out var lotBoxType);
                        no = (int)lotBoxType;
                    }
                    break;
                default:
                    break;
            }
            return no;   
        }
        /// <summary>
        /// 获取吸嘴编号或平台编号对应的转塔工位号
        /// </summary>
        /// <param name="stageNo">吸嘴编号或平台编号</param>
        /// <param name="RotateStnType">转塔类型</param>
        /// <returns>转塔工位号</returns>
        public int GetStnNo(int stageNo,RotateStnType RotateStnType)
        {
            int stnNo = 0;
            switch (RotateStnType)
            {
                case RotateStnType.LoadDie:
                    stnNo= GetStnNo(LoadDieStnDic, stageNo);
                    break;
                case RotateStnType.Stn4TurnTable:
                    stnNo = GetStnNo(Stn4TurnTableDic, stageNo);
                    break;
                case RotateStnType.PickPatch:
                    stnNo = GetStnNo(PickPatchStnDic, stageNo);
                    break;
                case RotateStnType.Stn8TurnTable:
                    stnNo = GetStnNo(Stn8TurnTableDic, stageNo);
                    break;
                case RotateStnType.LotBox:
                    foreach (var kvp in LotBoxStnDic)
                    {
                        if (kvp.Value == (LotBoxType)stageNo)
                        {
                            stnNo = kvp.Key;
                            break;
                        }
                    }
                    break;
            }
            return stnNo;
        }
        private int GetStnNo(ConcurrentDictionary<int, int> dic,int stageNo)
        {
             int ret = 0;
            foreach (var kvp in dic)
            {
                if (kvp.Value == stageNo)
                {
                    ret = kvp.Key;
                    break;
                }
            } 
            return ret;
        }
        public void UpdateRotateInfo(RotateStnType RotateStnType,double pos)
        {
            switch (RotateStnType)
            {
                case RotateStnType.LoadDie:
                    {
                        UpdateDic(LoadDieStnDic, LoadDieDDStepAngle, LoadDieStnCount, pos, RotateStnType.LoadDie);
                    }
                    break;
                case RotateStnType.Stn4TurnTable:
                    {
                        UpdateDic(Stn4TurnTableDic, Stn4DDStepAngle, Stn4TurnTableStnCount, pos, RotateStnType.Stn4TurnTable);
                    }
                    break;
                case RotateStnType.PickPatch:
                    {
                        UpdateDic(PickPatchStnDic, PickPatchDDStepAngle, PickPatchStnCount, pos, RotateStnType.PickPatch);
                    }
                    break;
                case RotateStnType.Stn8TurnTable:
                    {
                        UpdateDic(Stn8TurnTableDic, Stn8DDStepAngle, Stn8TurnTableStnCount, pos, RotateStnType.Stn8TurnTable);
                    }
                    break;
          
                default:
                    break;
            }
           
        }
        /// <summary>
        /// 更新工位号(⓪、①、②、③)和内盒平台的映射关系。工位号不变，内盒平台变化
        /// </summary>
        /// <param name="type"></param>
        public void UpdateLotBoxStn(LotBoxPosEnum type)
        {
            switch (type)
            {
                case LotBoxPosEnum.Home:
                    {
                        /////////////////////////////////////////
                        ////       ⓪      /////////          ①      ///
                        ////  Bin135   /////////  EmptyLot  ///
                        ////                /////////                   ///
                        ////////////////////////////////////////
                        ////////////////////////////////////////
                        ////       ②      ///////////       ③     ///
                        ////  Bin246  ///////////                ///
                        ////               ///////////                ///
                        ////////////////////////////////////////
                        LotBoxStnDic[0] = LotBoxType.Bin135;//OK Bin
                        LotBoxStnDic[1] = LotBoxType.EmptyTray;//空料盒
                        LotBoxStnDic[2] = LotBoxType.Bin246;//NG Bin
                        LotBoxStnDic[3] = LotBoxType.Null;//空位
                        break;
                    }
                case LotBoxPosEnum.Stn2Null:
                    {
                        /////////////////////////////////////////
                        ////       ⓪      /////////          ①      ///
                        ////  Bin135   /////////                   ///
                        ////                /////////                   ///
                        ////////////////////////////////////////
                        ////////////////////////////////////////
                        ////       ②      /////////         ③      ///
                        ////  Bin246  //////////  EmptyLot ///
                        ////               //////////                  ///
                        ////////////////////////////////////////
                        LotBoxStnDic[0] = LotBoxType.Bin135;//OK Bin
                        LotBoxStnDic[1] = LotBoxType.Null;//空位
                        LotBoxStnDic[2] = LotBoxType.Bin246;
                        LotBoxStnDic[3] = LotBoxType.EmptyTray;
                        break;
                    }
                case LotBoxPosEnum.Bin246InStn2:
                    {
                        /////////////////////////////////////////
                        ////       ⓪      /////////          ①      ///
                        ////  Bin135   /////////    Bin246    ///
                        ////                /////////                   ///
                        ////////////////////////////////////////
                        ////////////////////////////////////////
                        ////       ②      /////////        ③       ///
                        ////                /////////  EmptyLot  ///
                        ////               /////////                   ///
                        ////////////////////////////////////////
                        LotBoxStnDic[0] = LotBoxType.Bin135;
                        LotBoxStnDic[1] = LotBoxType.Bin246;
                        LotBoxStnDic[2] = LotBoxType.Null;
                        LotBoxStnDic[3] = LotBoxType.EmptyTray;
                        break;
                    }
                default:
                    break;
            }
            LotBoxInfoChanged?.Invoke(LotBoxStnDic);
        }
        private void UpdateDic(ConcurrentDictionary<int, int> dic, double stepAngle,int stnCount, double pos, RotateStnType rotateStnType)
        {
            if (pos % stepAngle == 0)
            {
                if (pos == 0)
                {
                    for (int i = 0; i < stnCount; i++)
                    {
                        dic[i] = i;
                    }
                }
                else
                {
                    var step = pos / stepAngle;
                    foreach (var key in dic.Keys.ToList())
                    {
                        var pickNo = key - (int)step;
                        if (pickNo < 0)
                        {
                            pickNo += stnCount;
                        }
                        dic[key] = pickNo;
                    }
                }
                RotateInfoChanged?.Invoke(rotateStnType, dic.Keys.ToArray());
            }
        }
        public double GetActMoveAngle(int stationNo, int stageNo, double stepAngle, RotateStnType rotateStnType)
        {
            var currentStageNo = GetStageNo(stationNo, rotateStnType);
            var currentAngle = NormalizeAngle((stationNo - currentStageNo) * stepAngle);
            var currentStationNo = GetStnNo(stageNo, rotateStnType);
            var targetOffsetAngle = NormalizeOffsetAngle((stationNo - currentStationNo) * stepAngle);
            var destAngle = NormalizeAngle(currentAngle + targetOffsetAngle);
            return destAngle;
        }
        /// <summary>
        /// 归一化角度到0-360范围内
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public double NormalizeAngle(double angle)
        {
            while (angle < 0)
            {
                angle += 360;
            }

            while (angle >= 360)
            {
                angle -= 360;
            }

            return angle;
        }
        /// <summary>
        /// 归一化角度到-180到180范围内
        /// </summary>
        /// <param name="angle"></param>
        /// <returns></returns>
        public double NormalizeOffsetAngle(double angle)
        {
            while (angle > 180)
            {
                angle -= 360;
            }

            while (angle < -180)
            {
                angle += 360;
            }

            return angle;
        }
    }
    public enum LotBoxPosEnum
    {
        /// <summary>
        /// 初始位置
        /// </summary>
        Home,
        /// <summary>
        /// 工位2空位
        /// </summary>
        Stn2Null,
        /// <summary>
        /// Bin246移动到工位2，EmptySlot移动到工位3
        /// </summary>
        Bin246InStn2,
    
    }
}
