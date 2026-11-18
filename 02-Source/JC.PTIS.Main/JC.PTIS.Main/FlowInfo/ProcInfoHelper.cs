using JC.PTIS.Common;
using JC.PTIS.Main.FlowInfo;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.Models.ModelHelper;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Params;
using JC.PTIS.Main.RotateInfo;
using JC.PTIS.Main.Stations;
using SqlSugar;
using StdLib.LogHelper;
using StdLib.PCBaseFrame.Motion;
using StdLib.WindowsControls.Class;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace JC.PTIS.Main.Entities
{
    /// <summary>
    /// 制程物流信息帮助类，负责管理取料器、Tray、Foup、Wafer等信息，并提供更新和查询接口
    /// </summary>
    public class ProcInfoHelper : Singleton<ProcInfoHelper>
    {
        PickerInfo[] LoadDiePickerArray = null;
        PickerInfo[] PickPatchPickerArray = null;
        TrayStageInfo[] Stn4TurnTable = null;
        LotStageInfo[] Stn8TurnTable = null;
        HalfTrayBufferModel HalfTrayBuffer = null;
        IRNgTrayModel IRNgTray = null;
        LotBoxModel[] LotBoxArray = null;
        ConcurrentDictionary<TrayPosEnum, TrayStageInfo> TrayStageDic = null;
        ConcurrentDictionary<LotPosEnum, LotStageInfo> LotStageDic = null;
        ConcurrentDictionary<WaferPosEnum, WaferModel> WaferDic = null;
        ConcurrentDictionary<BinLevel, List<DieModel>> BinMapDic = null;
        public event Action<int, PickerInfo, RotateStnType> PickerInfoUpdated;//第一个参数：工位号，不跟随转盘转动改变，第二个参数：吸嘴信息
        public event Action<int, TrayStageInfo> Stn4TurnTableUpdated;//第一个参数：工位号，不跟随转盘转动改变，第二个参数：Tray信息
        public event Action<int, FoupModel> FoupInfoUpdated;
        public event Action<WaferModel, WaferPosEnum> WaferInfoUpdated;
        public event Action<int, LotStageInfo> Stn8TurnTableUpdated;//第一个参数：工位号，不跟随转盘转动改变
        public event Action<HalfTrayBufferModel> HalfTrayBufferInfoUpdated;
        public event Action<LotCellBase, LotBoxType> LotCellInfoUpdated;
        public event Action<LotBoxModel, LotBoxStnType> LotBoxInfoUpdated;
        public event Action<TrayStageInfo> TrayDicInfoUpdated;
        public event Action<LotStageInfo> LotDicInfoUpdated;
        public event Action<IRNgTrayModel> IRNgTrayModelInfoUpdated;
        Random Ran = new Random();
        public void Init()
        {
            LoadDiePickerArray = new PickerInfo[16];
            for (int i = 0; i < LoadDiePickerArray.Length; i++)
            {
                PickerInfo info = new PickerInfo();
                info.PickNo = i;
                LoadDiePickerArray[i] = info;
            }

            PickPatchPickerArray = new PickerInfo[16];
            for (int i = 0; i < PickPatchPickerArray.Length; i++)
            {
                PickerInfo info = new PickerInfo();
                info.PickNo = i;
                PickPatchPickerArray[i] = info;
            }

            Stn4TurnTable = new TrayStageInfo[4];
            for (int i = 0; i < Stn4TurnTable.Length; i++)
            {
                TrayStageInfo info = new TrayStageInfo();
                info.TrayPos = TrayPosEnum.Stn4TurnTable;
                info.StageNo = i;
                Stn4TurnTable[i] = info;
            }

            Stn8TurnTable = new LotStageInfo[8];
            for (int i = 0; i < Stn8TurnTable.Length; i++)
            {
                LotStageInfo info = new LotStageInfo();
                info.StageNo = i;
                info.LotModel = new LotModel();
                info.LotModel.LotCode = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                info.LotStageType = (LotStageType)i;
                Stn8TurnTable[i] = info;
            }

            HalfTrayBuffer = new HalfTrayBufferModel();
            HalfTrayBuffer.TrayArray = new TrayModel[12];

            IRNgTray = new IRNgTrayModel();
            IRNgTray.DieArray = new DieModel[13];


            LotBoxArray = new LotBoxModel[6];
            for (int i = 0; i < LotBoxArray.Length; i++)
            {
                LotBoxArray[i] = new LotBoxModel();
                LotBoxArray[i].LotBoxType = (LotBoxType)i;
            }


            TrayStageDic = new ConcurrentDictionary<TrayPosEnum, TrayStageInfo>();
            for (int i = 1; i < TrayPosEnum.None.ToInt(); i++)
            {
                TrayStageDic.TryAdd((TrayPosEnum)i, new TrayStageInfo() { TrayPos = (TrayPosEnum)i });
            }

            LotStageDic = new ConcurrentDictionary<LotPosEnum, LotStageInfo>();
            for (int i = 0; i < LotPosEnum.None.ToInt(); i++)
            {
                LotStageDic.TryAdd((LotPosEnum)i, new LotStageInfo() { LotPos = (LotPosEnum)i });
            }
            WaferDic = new ConcurrentDictionary<WaferPosEnum, WaferModel>();
            for (int i = 0; i < WaferPosEnum.None.ToInt(); i++)
            {
                WaferDic.TryAdd((WaferPosEnum)i, null);
            }

            BinMapDic = new ConcurrentDictionary<BinLevel, List<DieModel>>();
            for (int i = 1; i < 7; i++)
            {
                BinMapDic.TryAdd((BinLevel)i, null);
            }
            //RestoreInfo();
        }

        public void RestoreInfo()
        {
            try
            {
                for (int i = 0; i < LoadDiePickerArray.Length; i++)
                {
                    var pickerNo = RotateInfoHelper.Instance.GetStageNo(i, RotateStnType.LoadDie);
                    var dieModel = RedisHelper.Instance.ObjectGet<DieModel>($"RotateStnType:{RotateStnType.LoadDie}_Picker{pickerNo}");
                    if (dieModel != null)
                    {
                        LoadDiePickerArray[pickerNo].DieModel = dieModel;
                        PickerInfoUpdated?.Invoke(i, LoadDiePickerArray[pickerNo], RotateStnType.LoadDie);
                    }
                    if (Global.RunMode == RunMode.空跑模式)
                    {
                        IOInfoHelper.GetAnalogInputInfoByName($"LoadDieNozzle{pickerNo + 1}Value").AIValue = dieModel != null ? Ran.Next(1100, 1500) : Ran.Next(100, 150);
                    }
                }
                for (int i = 0; i < PickPatchPickerArray.Length; i++)
                {
                    var pickerNo = RotateInfoHelper.Instance.GetStageNo(i, RotateStnType.PickPatch);
                    var dieModel = RedisHelper.Instance.ObjectGet<DieModel>($"RotateStnType:{RotateStnType.PickPatch}_Picker{pickerNo}");
                    if (dieModel != null)
                    {
                        PickPatchPickerArray[pickerNo].DieModel = dieModel;
                        PickerInfoUpdated?.Invoke(i, PickPatchPickerArray[pickerNo], RotateStnType.PickPatch);
                    }
                    if (Global.RunMode == RunMode.空跑模式)
                    {
                        var ioInfo = IOInfoHelper.GetAnalogInputInfoByName($"PickPatchNozzle{pickerNo + 1}Value");
                        if (ioInfo != null)
                        {
                            ioInfo.AIValue = dieModel != null ? Ran.Next(1100, 1500) : Ran.Next(100, 150);
                        }
                    }
                }
                for (int i = 0; i < Stn4TurnTable.Length; i++)
                {
                    var stageNo = RotateInfoHelper.Instance.GetStageNo(i, RotateStnType.Stn4TurnTable);
                    var trayModel = RedisHelper.Instance.ObjectGet<TrayModel>($"TrayStnEnum:{RotateStnType.Stn4TurnTable}_Stage:{stageNo}");
                    if (trayModel != null)
                    {
                        Stn4TurnTable[stageNo].TrayModel = trayModel;
                        Stn4TurnTableUpdated?.Invoke(i, Stn4TurnTable[stageNo]);
                    }
                    if (Global.RunMode == RunMode.空跑模式)
                    {
                        IOInfoHelper.GetInputInfoByName($"TrayStage{stageNo + 1}VacDetect", out int _).IsShield = trayModel != null;
                    }
                }
                foreach (var key in TrayStageDic.Keys)
                {
                    var trayModel = RedisHelper.Instance.ObjectGet<TrayModel>($"TrayStnEnum:{key}_Stage:{TrayStageDic[key].StageNo}");
                    if (trayModel != null)
                    {
                        TrayStageDic[key].TrayModel = trayModel;
                        TrayDicInfoUpdated?.Invoke(TrayStageDic[key]);
                    }

                    if (Global.RunMode == RunMode.空跑模式)
                    {
                        switch (key)
                        {
                            case TrayPosEnum.EmptyTrayArm:
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.EmptyTrayArmIsExistTray.ToString(), out int _).IsShield = trayModel != null;
                                }
                                break;
                            case TrayPosEnum.FullTrayArm:
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.FullTrayArmIsExistTray.ToString(), out int _).IsShield = trayModel != null;
                                }
                                break;
                            case TrayPosEnum.EmptyTrayBufferStage:
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.EmptyTrayBufferVacDetect.ToString(), out int _).IsShield = trayModel != null;
                                }
                                break;
                            case TrayPosEnum.OkHalfTrayStage:
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.OKHalfTrayVacDetect.ToString(), out int _).IsShield = trayModel != null;
                                }
                                break;
                            case TrayPosEnum.NgHalfTrayStage:
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.NGHalfTrayVacDetect.ToString(), out int _).IsShield = trayModel != null;
                                }
                                break;
                            case TrayPosEnum.HalfTrayBufferArm:
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.HalfTrayBufferArmSensor.ToString(), out int _).IsShield = trayModel != null;
                                }
                                break;
                        }
                    }
                }
                for (int i = 0; i < Stn8TurnTable.Length; i++)
                {
                    var stageNo = RotateInfoHelper.Instance.GetStageNo(i, RotateStnType.Stn8TurnTable);
                    var lotModel = RedisHelper.Instance.ObjectGet<LotModel>($"LotStnEnum:{RotateStnType.Stn8TurnTable}_Stage:{stageNo}");
                    if (lotModel != null)
                    {
                        Stn8TurnTable[stageNo].LotModel = lotModel;
                        Stn8TurnTableUpdated?.Invoke(i, Stn8TurnTable[stageNo]);
                    }
                    if (Global.RunMode == RunMode.空跑模式)
                    {
                        if (i == 6)
                        {
                            IOInfoHelper.GetInputInfoByName($"Stn8LulStageDetect", out int _).IsShield = lotModel != null && lotModel.TrayCount != 0;
                        }
                        else if (i == 7)
                        {
                            IOInfoHelper.GetInputInfoByName($"Stn8ManualLoadDetect", out int _).IsShield = lotModel != null && lotModel.TrayCount != 0;
                        }
                        else
                        {
                            IOInfoHelper.GetInputInfoByName($"Stn8Bin{stageNo + 1}StageDetect", out int _).IsShield = lotModel != null && lotModel.TrayCount != 0;
                        }
                    }
                }
                foreach (var key in LotStageDic.Keys)
                {
                    var lotModel = RedisHelper.Instance.ObjectGet<LotModel>($"LotStnEnum:{key}");
                    if (lotModel != null)
                    {
                        LotStageDic[key].LotModel = lotModel;
                        LotDicInfoUpdated?.Invoke(LotStageDic[key]);
                    }
                    if (Global.RunMode == RunMode.空跑模式)
                    {
                        switch (key)
                        {
                            case LotPosEnum.PackingStage:
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.PackingStageLotDetect.ToString(), out int _).IsShield = lotModel != null && lotModel.TrayCount != 0;
                                }
                                break;
                            case LotPosEnum.LotArmClaw:
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.LotTransferArmDetect.ToString(), out int _).IsShield = lotModel != null && lotModel.TrayCount != 0;
                                }
                                break;
                            case LotPosEnum.LotArmSucker:
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.LotTransferArmVacDetect.ToString(), out int _).IsShield = lotModel != null && lotModel.TrayCount != 0;
                                }
                                break;
                        }
                    }
                }
                //wafer信息恢复
                foreach (var key in WaferDic.Keys)
                {
                    var waferModel = RedisHelper.Instance.ObjectGet<WaferModel>($"WaferStnEnum:{key}");
                    if (waferModel != null)
                    {
                        WaferDic[key] = waferModel;
                        WaferInfoUpdated?.Invoke(waferModel, key);
                    }

                    if (Global.RunMode == RunMode.空跑模式)
                    {
                        switch (key)
                        {
                            case WaferPosEnum.WaferBufferUpLayer:
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.UpLayerWaferDetect1.ToString(), out int _).IsShield = waferModel != null;
                                }
                                break;
                            case WaferPosEnum.WaferBufferDownLayer:
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.DownLayerWaferDetect1.ToString(), out int _).IsShield = waferModel != null;
                                }
                                break;
                            case WaferPosEnum.WaferTable:
                                {
                                    IOInfoHelper.GetInputInfoByName(InputName.WaferTableIsExistWafer.ToString(), out int _).IsShield = waferModel != null;
                                }
                                break;
                        }
                    }
                }
                if (WaferDic.Keys.Contains(WaferPosEnum.WaferTable) &&
                    WaferDic[WaferPosEnum.WaferTable] != null)
                {
                    InitBinMapDic(WaferDic[WaferPosEnum.WaferTable]);
                }

                //HalfTrayBuffer信息恢复
                for (int i = 0; i < HalfTrayBuffer.TrayArray.Length; i++)
                {
                    var trayModel = RedisHelper.Instance.ObjectGet<TrayModel>($"HalfTrayBuffer:{i}");
                    if (trayModel != null)
                    {
                        HalfTrayBuffer.TrayArray[i] = trayModel;
                        HalfTrayBufferInfoUpdated?.Invoke(HalfTrayBuffer);
                    }
                    if (Global.RunMode == RunMode.空跑模式)
                    {
                        IOInfoHelper.GetInputInfoByName($"HalfTrayBufferLayer{i + 1}Detect", out int _).IsShield = trayModel != null;
                    }
                }
                //LotBox信息恢复
                for (int i = 0; i < LotBoxArray.Length; i++)
                {
                    var stageNo = RotateInfoHelper.Instance.GetStageNo(i, RotateStnType.LotBox);
                    var lotBoxModel = RedisHelper.Instance.ObjectGet<LotBoxModel>($"LotBoxStnType:{(LotBoxStnType)i}");
                    if (lotBoxModel != null)
                    {
                        LotBoxArray[stageNo] = lotBoxModel;
                        LotBoxInfoUpdated?.Invoke(LotBoxArray[stageNo], (LotBoxStnType)i);
                    }
                    if (Global.RunMode == RunMode.空跑模式)
                    {
                        switch ((LotBoxStnType)i)
                        {
                            case LotBoxStnType.OKBinStn:
                            case LotBoxStnType.LulLotStn:
                            case LotBoxStnType.NGBinStn:
                            case LotBoxStnType.NullStn:
                                {
                                    for (int j = 0; j < 2; j++)
                                    {
                                        var inputName = $"LotBox{i + 1}IsExist{j + 1}";
                                        IOInfoHelper.GetInputInfoByName(inputName, out int _).IsShield = lotBoxModel != null && lotBoxModel.LotCellArray != null;
                                    }
                                }
                                break;
                            case LotBoxStnType.ExBox1Stn:
                                {
                                    for (int j = 0; j < 2; j++)
                                    {
                                        var inputName = $"ExBox1IsExist{j + 1}";
                                        IOInfoHelper.GetInputInfoByName(inputName, out int _).IsShield = lotBoxModel != null;
                                    }
                                    break;
                                }
                            case LotBoxStnType.ExBox2Stn:
                                {
                                    for (int j = 0; j < 2; j++)
                                    {
                                        var inputName = $"ExBox2IsExist{j + 1}";
                                        IOInfoHelper.GetInputInfoByName(inputName, out int _).IsShield = lotBoxModel != null;
                                    }
                                }
                                break;
                            default:
                                break;
                        }
                    }
                }

                //IRNgTray信息恢复
                var model = RedisHelper.Instance.ObjectGet<IRNgTrayModel>($"IRNgTray");
                if (model != null)
                {
                    IRNgTray = model;
                    IRNgTrayModelInfoUpdated?.Invoke(IRNgTray);
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("ProcInfoHelper").Info($"方法：RestoreInfo执行异常：{ex}");
            }
        }

        public void DeleteAllKey()
        {
            RedisHelper.Instance.KeyDelete($"IRNgTray");
            for (int i = 0; i < LoadDiePickerArray.Length; i++)
            {
                RedisHelper.Instance.KeyDelete($"RotateStnType:{RotateStnType.LoadDie}_Picker{i}");
            }
            for (int i = 0; i < PickPatchPickerArray.Length; i++)
            {
                RedisHelper.Instance.KeyDelete($"RotateStnType:{RotateStnType.PickPatch}_Picker{i}");
            }
            for (int i = 0; i < Stn4TurnTable.Length; i++)
            {
                RedisHelper.Instance.KeyDelete($"TrayStnEnum:{RotateStnType.Stn4TurnTable}_Stage:{i}");
            }
            foreach (var key in TrayStageDic.Keys)
            {
                RedisHelper.Instance.KeyDelete($"TrayStnEnum:{key}_Stage:{TrayStageDic[key].StageNo}");
            }
            for (int i = 0; i < Stn8TurnTable.Length; i++)
            {
                RedisHelper.Instance.KeyDelete($"LotStnEnum:{RotateStnType.Stn8TurnTable}_Stage:{i}");
            }
            foreach (var key in LotStageDic.Keys)
            {
                RedisHelper.Instance.KeyDelete($"LotStnEnum:{key}");
            }
            //wafer信息恢复
            foreach (var key in WaferDic.Keys)
            {
                RedisHelper.Instance.KeyDelete($"WaferStnEnum:{key}");
            }

            //HalfTrayBuffer信息恢复
            for (int i = 0; i < HalfTrayBuffer.TrayArray.Length; i++)
            {
                RedisHelper.Instance.KeyDelete($"HalfTrayBuffer:{i}");
            }
            //LotBox信息恢复
            for (int i = 0; i < LotBoxArray.Length; i++)
            {
                RedisHelper.Instance.KeyDelete($"LotBoxStnType:{(LotBoxStnType)i}");
            }
        }
        #region PickerInfo
        public void PlaceDieToPicker<T>(T stnEnum, DieModel dieModel, RotateStnType rotateType)
        {
            var stnNo = Convert.ToInt32(stnEnum);
            var info = GetPickerInfo(stnEnum, rotateType);
            if (info != null)
            {
                LogManager.GetLogger("ProcInfoHelper").Info($"方法：PlaceDieToPicker，工位：{stnEnum}，DieCode：{dieModel.DieCode}");
                info.DieModel = dieModel;
                PickerInfoUpdated?.Invoke(stnNo, info, rotateType);
                RedisHelper.Instance.ObjectSet($"RotateStnType:{rotateType}_Picker{info.PickNo}", dieModel);
            }
        }
        public PickerInfo GetPickerInfo<T>(T stnEnum, RotateStnType rotateType)
        {
            PickerInfo ret = null;
            var stnNo = Convert.ToInt32(stnEnum);
            var pickNo = RotateInfoHelper.Instance.GetStageNo(stnNo, rotateType);
            if (rotateType == RotateStnType.LoadDie)
            {
                ret = LoadDiePickerArray[pickNo];
            }
            else if (rotateType == RotateStnType.PickPatch)
            {
                ret = PickPatchPickerArray[pickNo];
            }

            return ret;
        }
        public DieModel PickDieFromPicker<T>(T stnEnum, RotateStnType rotateType)
        {
            DieModel ret = null;
            var stnNo = Convert.ToInt32(stnEnum);
            var pickInfo = GetPickerInfo(stnEnum, rotateType);
            var pickNo = RotateInfoHelper.Instance.GetStageNo(stnNo, rotateType);
            if (pickInfo != null && pickInfo.DieModel != null)
            {
                LogManager.GetLogger("ProcInfoHelper").Info($"方法：PickDieFromPicker，工位：{stnEnum}，DieCode：{pickInfo.DieModel.DieCode}");
                ret = pickInfo.DieModel.DeepCopy();
                pickInfo.DieModel = null;
                PickerInfoUpdated?.Invoke(stnNo, pickInfo, rotateType);
                RedisHelper.Instance.ObjectSet<DieModel>($"RotateStnType:{rotateType}_Picker{pickNo}", null);
            }
            return ret;
        }
        /// <summary>
        /// 任一吸嘴上是否有Die，若有则返回true，否则返回false
        /// </summary>
        /// <returns></returns>
        public bool IsHasDieOnPicker()
        {
            for (int i = 0; i < 16; i++)
            {
                if (IsNozzleVacArrive(i))
                {
                    return true;
                }
            }
            return false;
        }
        private bool IsNozzleVacArrive(int nozzleNo)
        {
            var inputName = $"LoadDieNozzle{nozzleNo + 1}Value";
            var value = IOInfoHelper.GetAnalogInputInfoByName($"LoadDieNozzle{nozzleNo + 1}Value").AIValue;
            return value > 1000;
        }
        /// <summary>
        /// 计算PickPatch工位吸嘴上Ng Die数量
        /// </summary>
        /// <returns></returns>
        public int GetPickerNgDieCount()
        {
            int result = 0;
            //计算PickPatch工位吸嘴上Ng Die数量
            if (PickPatchPickerArray != null)
            {
                result = PickPatchPickerArray.Where(p => p.DieModel != null && (p.DieModel.DieStatus >= DieStatus.BackNG && p.DieModel.DieStatus <= DieStatus.FrontNG)).Count();
            }
            return result;
        }
        public PickerInfo[] GetPickerArray(RotateStnType rotateType)
        {
            if (rotateType == RotateStnType.LoadDie)
            {
                return LoadDiePickerArray;
            }
            else if (rotateType == RotateStnType.PickPatch)
            {
                return PickPatchPickerArray;
            }
            return null;
        }
        public void UpdatePickInfo(PickerInfo pickerInfo, RotateStnType rotateStnType)
        {
            var stnNo = RotateInfoHelper.Instance.GetStnNo(pickerInfo.PickNo, rotateStnType);
            PickerInfoUpdated?.Invoke(stnNo, pickerInfo, rotateStnType);
        }
        #endregion
        #region Tray物流信息
        /// <summary>
        /// 从Tray平台获取工位对应的TrayStageInfo信息，stnEnum为工位枚举类型，可以是Stn4TurnTableStnEnum或TrayPosEnum
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stnEnum"></param>
        /// <returns></returns>
        public TrayStageInfo GetTrayInfo<T>(T stnEnum)
        {
            var type = typeof(T);
            if (type == typeof(Stn4TurnTableStnEnum))
            {
                var @enum = (Stn4TurnTableStnEnum)(object)stnEnum;
                var stageNo = RotateInfoHelper.Instance.GetStageNo(@enum.ToInt(), RotateStnType.Stn4TurnTable);
                if (Stn4TurnTable != null)
                {
                    return Stn4TurnTable[stageNo];
                }
            }
            else if (type == typeof(TrayPosEnum))
            {
                var @enum = (TrayPosEnum)(object)stnEnum;
                if (TrayStageDic != null)
                {
                    var info = TrayStageDic.TryGetValue(@enum, out var trayInfo) ? trayInfo : null;
                    if (info != null)
                    {
                        return info;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// 从Tray平台取出工位对应的TrayModel信息，stnEnum为工位枚举类型，可以是Stn4TurnTableStnEnum或TrayPosEnum，取出后TrayModel会被清空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stnEnum"></param>
        /// <returns></returns>
        public TrayModel PickTrayFromTrayStage<T>(T stnEnum)
        {
            TrayModel ret = null;
            var info = GetTrayInfo(stnEnum);
            if (info != null && info.TrayModel != null)
            {
                LogManager.GetLogger("ProcInfoHelper").Info($"方法：PickTrayFromTrayStage，工位：{stnEnum}，TrayCode：{info.TrayModel.TrayCode}");
                ret = info.TrayModel.DeepCopy();
                info.TrayModel = null;
                TrayStageInfoUpdated(stnEnum, info);
            }
            return ret;
        }
        /// <summary>
        /// Tray放到Tray平台更新TrayModel，stnEnum为工位枚举类型，可以是Stn4TurnTableStnEnum或TrayPosEnum，model为要放置的TrayModel信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stnEnum"></param>
        /// <param name="model"></param>
        public void PutTrayToTrayStage<T>(T stnEnum, TrayModel trayModel)
        {
            try
            {
                var info = GetTrayInfo(stnEnum);
                if (info != null)
                {
                    LogManager.GetLogger("ProcInfoHelper").Info($"方法：PutTrayToTrayStage，工位：{stnEnum}，TrayCode：{trayModel?.TrayCode}");
                    info.TrayModel = trayModel;
                    TrayStageInfoUpdated(stnEnum, info);
                }
                else
                {
                    LogManager.GetLogger("ProcInfoHelper").Info($"方法：PutTrayToTrayStage，工位：{stnEnum}，获取TrayStageInfo失败");
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("ProcInfoHelper").Info($"PutTrayToTrayStage方法执行异常：{ex}");
            }
        }


        /// <summary>
        /// 放置Die到Tray中，stnEnum为工位枚举类型，可以是Stn4TurnTableStnEnum或TrayPosEnum，model为要放置的DieModel信息
        /// </summary>
        /// <param name="stnNo"></param>
        /// <param name="rowNo"></param>
        /// <param name="colNo"></param>
        /// <param name="dieModel"></param>
        public void PlaceDieToTray<T>(T stnEnum, int rowNo, int colNo, DieModel dieModel)
        {
            try
            {
                var info = GetTrayInfo(stnEnum);
                dieModel.InTrayRowNo = rowNo;
                dieModel.InTrayColNo = colNo;
                if (stnEnum.ToString() == Stn4TurnTableStnEnum.DieToTray.ToString())
                {
                    if (string.IsNullOrEmpty(info.TrayModel.TrayCode))
                    {
                        info.TrayModel.TrayCode = $"{dieModel.LOT}_{dieModel.WAFER}_{DateTime.Now.ToString("HHmmss")}";
                    }
                }
                info.TrayModel.DieArray[rowNo, colNo] = dieModel;
                info.TrayModel.UpdateMap();
                TrayModelHelper.Update(info.TrayModel);
                info.TrayModel.BinLevel = dieModel.BinLevel.ToInt();
                if (string.IsNullOrEmpty(info.TrayModel.LOT))
                {
                    info.TrayModel.LOT = dieModel.LOT;
                    info.TrayModel.WAFER = dieModel.WAFER;
                }
                TrayStageInfoUpdated(stnEnum, info);
                LogManager.GetLogger("ProcInfoHelper").Info($"方法：PlaceDieToTray，工位：{stnEnum}，RowNo：{rowNo}，ColNo：{colNo}，DieCode：{dieModel?.DieCode}");
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("ProcInfoHelper").Fatal(ex);
            }
        }
        /// <summary>
        /// 从Tray中取出Die，stnEnum为工位枚举类型，可以是Stn4TurnTableStnEnum或TrayPosEnum
        /// </summary>
        /// <param name="stnEnum">工位枚举类型，可以是Stn4TurnTableStnEnum或TrayPosEnum</param>
        /// <param name="rowNo"></param>
        /// <param name="colNo"></param>
        /// <returns></returns>
        public DieModel PickDieFromTray<T>(T stnEnum, int rowNo, int colNo)
        {
            DieModel ret = null;
            try
            {
                var info = GetTrayInfo(stnEnum);
                if (info.TrayModel != null &&
                    info.TrayModel.DieArray.Length > 0 &&
                    info.TrayModel.DieArray[rowNo, colNo] != null)
                {
                    ret = info.TrayModel.DieArray[rowNo, colNo].DeepCopy();
                    info.TrayModel.DieArray[rowNo, colNo] = null;
                    info.TrayModel.UpdateMap();
                    TrayModelHelper.Update(info.TrayModel);
                    TrayStageInfoUpdated(stnEnum, info);
                    LogManager.GetLogger("ProcInfoHelper").Info($"方法：PickDieFromTray，工位：{stnEnum}，RowNo：{rowNo}，ColNo：{colNo}，DieCode：{ret?.DieCode}");
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("ProcInfoHelper").Info($"PickDieFromTray方法执行异常：{ex}");
            }
            return ret;
        }
        /// <summary>
        /// 从Tray中获取可用的空穴位行列号，若无可用穴位则返回(-1,-1)
        /// </summary>
        /// <param name="stnNo">工位号，不跟随转盘转动变化</param>
        /// <returns></returns>
        public (int r, int c) GetAvlEmptyCellNoFromTray<T>(T stnEnum)
        {
            var info = GetTrayInfo(stnEnum);
            if (info != null && info.TrayModel != null)
            {
                for (int r = 0; r < info.TrayModel.RowCount; r++)
                {
                    for (int c = 0; c < info.TrayModel.ColCount; c++)
                    {
                        var dieModel = info.TrayModel.DieArray[r, c];
                        if (dieModel == null || dieModel.DieStatus == DieStatus.None)
                        {
                            return (r, c);
                        }
                    }
                }
            }
            return (-1, -1);
        }
        /// <summary>
        /// 判断指定位置是否可以放置Die
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stnEnum"></param>
        /// <param name="rowNo"></param>
        /// <param name="colNo"></param>
        /// <returns></returns>
        public bool IsCanPlaceDie<T>(T stnEnum, int rowNo, int colNo)
        {
            bool ret = false;
            var info = GetTrayInfo(stnEnum);
            if (info != null && info.TrayModel != null && info.TrayModel.DieArray.Length > 0)
            {
                var dieModel = info.TrayModel.DieArray[rowNo, colNo];
                if (dieModel == null || dieModel.DieStatus == DieStatus.None)
                {
                    return true;
                }
            }
            return ret;
        }
        /// <summary>
        /// 从Tray中获取有Die的穴位行列号，若无可用穴位则返回(-1,-1)
        /// </summary>
        /// <param name="stnEnum">工位枚举类型，可以是Stn4TurnTableStnEnum或TrayPosEnum</param>
        /// <returns></returns>
        public (int r, int c) GetAvlDieNoFromTray<T>(T stnEnum)
        {
            var info = GetTrayInfo(stnEnum);
            if (info != null && info.TrayModel != null)
            {
                for (int r = 0; r < info.TrayModel.RowCount; r++)
                {
                    for (int c = 0; c < info.TrayModel.ColCount; c++)
                    {
                        var dieModel = info.TrayModel.DieArray[r, c];
                        if (dieModel != null && dieModel.DieStatus != DieStatus.None)
                        {
                            return (r, c);
                        }
                    }
                }
            }
            return (-1, -1);
        }

        public void TrayStageInfoUpdated<T>(T stnEnum, TrayStageInfo info)
        {
            try
            {
                var type = typeof(T);
                if (type == typeof(Stn4TurnTableStnEnum))
                {
                    var @enum = (Stn4TurnTableStnEnum)(object)stnEnum;
                    Stn4TurnTableUpdated?.Invoke(@enum.ToInt(), info);
                    RedisHelper.Instance.ObjectSet($"TrayStnEnum:{RotateStnType.Stn4TurnTable}_Stage:{info.StageNo}", info.TrayModel);
                }
                else if (type == typeof(TrayPosEnum))
                {
                    TrayDicInfoUpdated?.Invoke(info);
                    RedisHelper.Instance.ObjectSet($"TrayStnEnum:{stnEnum}_Stage:{info.StageNo}", info.TrayModel);
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("ProcInfoHelper").Fatal(ex);
            }
        }
        public int TrayRemainNgCount<T>(T stnEnum)
        {
            int count = -1;
            try
            {
                var info = GetTrayInfo(stnEnum);
                if (info != null && info.TrayModel != null)
                {
                    count = info.TrayModel.DieArray.Cast<DieModel>().Where(d => d != null && (d.DieStatus == DieStatus.BackNG || d.DieStatus == DieStatus.FrontNG)).Count();
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("ProcInfoHelper").Fatal(ex);
            }
            return count;
        }
        #endregion

        #region Foup信息相关接口
        /// <summary>
        /// 插入FOUP信息，foupNo为FOUP编号，rfid为RFID号
        /// </summary>
        /// <param name="foupNo"></param>
        /// <param name="rfid"></param>
        public void LoadFoup(int foupNo, string rfid)
        {
            var slotCount = ParaHelper.Prod.FoupSlotCount;
            var arr = Enumerable.Repeat(1, slotCount).ToArray();
            var model = new FoupModel
            {
                FoupNo = foupNo,
                TotalSlot = slotCount,
                Rfid = rfid,
                RemainWaferCount = slotCount,
                CurSlotNo = 0,
                WaferMap = string.Join(",", arr),
                InEqpTime = DateTime.Now
            };
            var ret = FoupModelHelper.Insert(model);
            if (ret)
            {
                FoupInfoUpdated?.Invoke(foupNo, model);
            }
        }
        public void LoadFoup(int foupNo, FoupModel model)
        {
            var ret = FoupModelHelper.Insert(model);
            if (ret)
            {
                FoupInfoUpdated?.Invoke(foupNo, model);
            }
        }
        public void PickWaferFromFoup(int foupNo, int slotNo)
        {
            UpdateFoupInfo(foupNo, slotNo, true);
        }
        public void PlaceWaferToFoup(int foupNo, int slotNo)
        {
            UpdateFoupInfo(foupNo, slotNo, false);
        }
        /// <summary>
        /// 更新FOUP信息，isPick为true表示取晶圆，为false表示放晶圆
        /// </summary>
        /// <param name="foupNo"></param>
        /// <param name="slotNo"></param>
        /// <param name="isPick">true：取晶圆，false：放晶圆</param>
        /// <returns></returns>

        public bool UpdateFoupInfo(int foupNo, int slotNo, bool isPick)
        {
            bool ret = false;
            try
            {
                var model = FoupModelHelper.GetFoup(foupNo);
                if (model != null && slotNo != -1)
                {
                    model.CurSlotNo = slotNo;
                    var arr = model.WaferMap.Split(',').Select(x => int.Parse(x)).ToArray();
                    if (isPick)
                    {
                        arr[slotNo] = 0;
                        model.RemainWaferCount--;
                    }
                    else
                    {
                        arr[slotNo] = 2;
                        // model.RemainWaferCount++;
                    }
                    model.WaferMap = string.Join(",", arr);
                    ret = FoupModelHelper.Update(foupNo, model);
                    if (ret)
                    {
                        FoupInfoUpdated?.Invoke(foupNo, model);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("ProcInfoHelper").Error($"UpdateFoupInfo方法执行异常：{ex}");
            }
            return ret;
        }
        /// <summary>
        /// 获取FOUP中可用的满晶圆槽位号，返回槽位号，若无可用槽位则返回-1
        /// </summary>
        /// <param name="foupNo"></param>
        /// <returns></returns>
        public int GetAvailableFullWaferSlotNo(int foupNo)
        {
            var model = FoupModelHelper.GetFoup(foupNo);
            if (model != null)
            {
                var arr = model.WaferMap.Split(',').Select(x => int.Parse(x)).ToArray();
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i] == 1)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        /// <summary>
        /// 获取FOUP中可用的空晶圆槽位号，返回槽位号，若无可用槽位则返回-1
        /// </summary>
        /// <param name="foupNo"></param>
        /// <returns></returns>
        public int GetAvailableEmptySlotNo(int foupNo)
        {
            var model = FoupModelHelper.GetFoup(foupNo);
            if (model != null)
            {
                var arr = model.WaferMap.Split(',').Select(x => int.Parse(x)).ToArray();
                for (int i = 0; i < arr.Length; i++)
                {
                    if (arr[i] == 0)
                    {
                        return i;
                    }
                }
            }
            return -1;
        }
        /// <summary>
        /// 清空FOUP信息
        /// </summary>
        /// <param name="foupNo"></param>
        /// <returns></returns>
        public bool UnloadFoup(int foupNo)
        {
            var ret = FoupModelHelper.Delete(foupNo);
            if (ret)
            {
                FoupInfoUpdated?.Invoke(foupNo, null);
            }
            return ret;
        }
        #endregion

        #region Wafer信息相关接口
        /// <summary>
        /// 初始化BinMapDic字典，key为BinLevel，value为对应BinLevel的DieModel列表
        /// </summary>
        /// <param name="waferModel"></param>
        public void InitBinMapDic(WaferModel waferModel)
        {
            if (waferModel.DieArray != null)
            {
                for (int i = 1; i < BinLevel.Bin7.ToInt(); i++)
                {
                    var binLevel = (BinLevel)i;
                    BinMapDic[binLevel] = waferModel.DieArray.Cast<DieModel>().Where(d => d.BinLevel == binLevel).ToList();
                }
            }
        }
        /// <summary>
        /// 指定行列号的Die是否可以取料，判断条件为：Die为实Die、Die未被取料、Die的BinLevel与指定的binLevel相同
        /// </summary>
        /// <param name="rowNo"></param>
        /// <param name="colNo"></param>
        /// <param name="binLevel"></param>
        /// <returns></returns>
        public bool IsCanPick(int rowNo, int colNo, BinLevel binLevel)
        {
            var waferModel = GetWaferModel(WaferPosEnum.WaferTable);
            if (waferModel != null && waferModel.DieArray != null)
            {
                if (rowNo >= 0 && rowNo < waferModel.ROWCT && colNo >= 0 && colNo < waferModel.COLCT)
                {
                    var dieModel = waferModel.DieArray[rowNo, colNo];
                    if (dieModel.IsRealDie &&
                        dieModel.DieStatus != DieStatus.Picked &&
                        dieModel.BinLevel == binLevel)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public DieModel GetDie(int no, BinLevel binLevel)
        {
            if (BinMapDic.Keys.Contains(binLevel))
            {
                if (BinMapDic[binLevel].Count > no)
                {
                    return BinMapDic[binLevel][no];
                }
            }
            return null;
        }
        /// <summary>
        /// 从Wafer中取Die，若Die符合条件则返回DieModel，否则返回null
        /// </summary>
        /// <param name="rowNo"></param>
        /// <param name="colNo"></param>
        /// <returns></returns>
        public DieModel PickDieFromWafer(int rowNo, int colNo)
        {
            DieModel dieModel = null;
            var waferModel = GetWaferModel(WaferPosEnum.WaferTable);
            if (waferModel != null && waferModel.DieArray != null)
            {
                if (rowNo >= 0 && rowNo < waferModel.ROWCT && colNo >= 0 && colNo < waferModel.COLCT)
                {
                    dieModel = waferModel.DieArray[rowNo, colNo];
                    dieModel.LOT = waferModel.LOT;
                    dieModel.WAFER = waferModel.WAFER;
                    dieModel.DieStatus = DieStatus.Picked;
                    dieModel.StartTime = DateTime.Now;
                    UpdateWafer(WaferPosEnum.WaferTable, waferModel);
                    return dieModel;
                }
            }
            return null;
        }
        /// <summary>
        /// 获取指定工位的WaferModel信息，若工位没有Wafer则返回null
        /// </summary>
        /// <param name="posEnum"></param>
        /// <returns></returns>
        public WaferModel GetWaferModel(WaferPosEnum posEnum)
        {
            WaferModel ret = null;
            if (WaferDic[posEnum] != null)
            {
                WaferDic.TryGetValue(posEnum, out var model);
                ret = model;
            }
            return ret;
        }
        /// <summary>
        /// 从Wafer平台取出WaferModel信息，取出后WaferModel会被清空，若工位没有Wafer则返回null
        /// </summary>
        /// <param name="posEnum"></param>
        /// <returns></returns>
        public WaferModel PickWafer(WaferPosEnum posEnum)
        {
            WaferModel ret = null;
            if (WaferDic[posEnum] != null)
            {
                WaferDic.TryGetValue(posEnum, out var model);
                ret = model.DeepCopy();
                UpdateWafer(posEnum, null);
            }
            return ret;
        }
        /// <summary>
        /// 放置Wafer到Wafer平台，若工位已有Wafer则会被覆盖，放置后会触发WaferInfoUpdated事件
        /// </summary>
        /// <param name="posEnum"></param>
        /// <param name="model"></param>
        public void PutWafer(WaferPosEnum posEnum, WaferModel model)
        {
            UpdateWafer(posEnum, model);
        }
        public void UpdateWafer(WaferPosEnum posEnum, WaferModel model)
        {
            try
            {
                WaferDic[posEnum] = model;
                WaferInfoUpdated?.Invoke(model, posEnum);
                RedisHelper.Instance.ObjectSet($"WaferStnEnum:{posEnum}", model);
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("Error").Error($"[UpdateWafer]方法执行异常，异常原因:{ex}");
            }
        }
        /// <summary>
        /// 是否取到最后一个等级的Bin
        /// </summary>
        /// <param name="bin"></param>
        /// <returns></returns>
        public bool IsLastBin(ref BinLevel bin)
        {
            bool ret = false;
            if (BinMapDic != null)
            {
                bin = BinLevel.None;
                var lastBin = BinMapDic.Where(x => x.Value != null && x.Value.Count > 0).Select(x => x.Key).LastOrDefault();
                ret = lastBin == WaferTableStn.CurBin;
                if (ret)
                {
                    bin = lastBin;
                }
            }
            return ret;
        }
        /// <summary>
        /// 上料转塔和WaferTable剩余Die数量，bin为指定Bin等级，返回剩余Die数量
        /// </summary>
        /// <param name="bin"></param>
        /// <returns></returns>
        public int RemainDieCount(BinLevel bin)
        {
            int waferRemainCount = 0;
            int dieCountInPicker = 0;
            if (BinMapDic.Keys.Contains(bin))
            {
                if (BinMapDic[bin] != null && BinMapDic[bin].Count > 0)
                {
                    waferRemainCount = BinMapDic[bin].Where(x => x != null && x.DieStatus == DieStatus.InWafer).Count();
                }
            }
            for (int i = 0; i < 16; i++)
            {
                var pickInfo = GetPickerInfo(i, RotateStnType.LoadDie);
                if (pickInfo != null && pickInfo.DieModel != null && pickInfo.DieModel.BinLevel == bin)
                {
                    dieCountInPicker++;
                }
            }
            return waferRemainCount + dieCountInPicker;
        }
        /// <summary>
        /// Wafer中是否还有指定Bin等级的Die，若有则返回true，否则返回false
        /// </summary>
        /// <param name="binLevel"></param>
        /// <returns></returns>
        public bool IsExistDieInWafer(BinLevel binLevel)
        {
            bool ret = false;
            if (BinMapDic.Keys.Contains(binLevel))
            {
                if (BinMapDic[binLevel] != null && BinMapDic[binLevel].Count > 0)
                {
                    ret = BinMapDic[binLevel].Where(x => x != null && x.DieStatus == DieStatus.InWafer).Count() > 0;
                }
            }
            return ret;
        }

        #endregion

        #region Lot信息相关接口
        /// <summary>
        /// 获取工位对应的Lot信息
        /// </summary>
        /// <param name="stnNo">工位号，不跟随转盘转动变化</param>
        /// <returns></returns>
        public LotStageInfo GetLotStageInfo<T>(T stnEnum)
        {
            var type = typeof(T);
            if (type == typeof(Stn8TurnTableStnEnum))
            {
                var @enum = (Stn8TurnTableStnEnum)(object)stnEnum;
                var stageNo = RotateInfoHelper.Instance.GetStageNo(@enum.ToInt(), RotateStnType.Stn8TurnTable);
                if (Stn8TurnTable != null)
                {
                    return Stn8TurnTable[stageNo];
                }
            }
            else if (type == typeof(LotPosEnum))
            {
                var @enum = (LotPosEnum)(object)stnEnum;
                if (LotStageDic != null)
                {
                    var info = LotStageDic.TryGetValue(@enum, out var trayInfo) ? trayInfo : null;
                    if (info != null)
                    {
                        return info;
                    }
                }
            }
            return null;
        }
        /// <summary>
        ///  从LotStage中取出LotModel信息，stnEnum为工位枚举类型，可以是Stn8TurnTableStnEnum或LotPosEnum，取出后LotModel会被清空
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stnEnum"></param>
        /// <returns></returns>
        public LotModel PickLotFromLotStage<T>(T stnEnum)
        {
            LotModel ret = null;
            var info = GetLotStageInfo(stnEnum);
            if (info != null && info.LotModel != null)
            {
                ret = info.LotModel.DeepCopy();
                info.LotModel = new LotModel { LotCode = DateTime.Now.ToString("yyyyMMddHHmmssfff") };
                LotStageInfoUpdated(stnEnum, info);
            }
            LogManager.GetLogger("ProcInfoHelper").Info($"方法：PickLotFromLotStage，工位：{stnEnum}，LotCode：{ret?.LotCode}");
            return ret;
        }
        /// <summary>
        /// 放置Lot到LotStage中，stnEnum为工位枚举类型，可以是Stn8TurnTableStnEnum或LotPosEnum，lotModel为要放置的Lot信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stnEnum"></param>
        /// <param name="lotModel"></param>
        public void PutLotToLotStage<T>(T stnEnum, LotModel lotModel)
        {
            var info = GetLotStageInfo(stnEnum);
            if (info != null)
            {
                info.LotModel = lotModel;
                LotStageInfoUpdated(stnEnum, info);
                LogManager.GetLogger("ProcInfoHelper").Info($"方法：PutLotToLotStage，工位：{stnEnum}，LotCode：{lotModel?.LotCode}");
            }
        }
        public void LotStageInfoUpdated<T>(T stnEnum, LotStageInfo info)
        {
            try
            {
                var type = typeof(T);
                if (type == typeof(Stn8TurnTableStnEnum))
                {
                    var @enum = (Stn8TurnTableStnEnum)(object)stnEnum;
                    Stn8TurnTableUpdated?.Invoke(@enum.ToInt(), info);
                    var stageNo = RotateInfoHelper.Instance.GetStageNo(@enum.ToInt(), RotateStnType.Stn8TurnTable);
                    RedisHelper.Instance.ObjectSet($"LotStnEnum:{RotateStnType.Stn8TurnTable}_Stage:{stageNo}", info.LotModel);
                }
                else if (type == typeof(LotPosEnum))
                {
                    LotDicInfoUpdated?.Invoke(info);
                    RedisHelper.Instance.ObjectSet($"LotStnEnum:{stnEnum}", info.LotModel);
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("ProcInfoHelper").Fatal(ex);
            }
        }
        /// <summary>
        /// 上空LotTray
        /// </summary>
        /// <param name="stnNo">工位号，不跟随转盘转动变化</param>
        /// <param name="lotModel"></param>
        public void LoadEmptyLot(Stn8TurnTableStnEnum stnNo, List<TrayModel> trayList)
        {
            var info = GetLotStageInfo(stnNo);
            if (info != null && trayList != null)
            {
                if (info.LotModel == null)
                {
                    info.LotModel = new LotModel();
                    info.LotModel.LotCode = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                }
                info.IsEmpty = false;
                for (int i = 0; i < trayList.Count; i++)
                {
                    if (info.LotModel.TrayList.Count >= info.LotModel.TotalCount)
                    {
                        return;
                    }
                    info.LotModel.TrayList.Add(trayList[i]);
                    info.LotModel.TrayCount++;
                }
                info.LotModel.UpdateMap();
                LotStageInfoUpdated(stnNo, info);
            }
        }

        /// <summary>
        /// 从Lot中取空Tray，返回取出的Tray信息，若该工位无Tray则返回null
        /// </summary>
        /// <param name="stnNo">工位号，不跟随转盘转动变化</param>
        /// <returns></returns>
        public TrayModel PickTrayFromLot(Stn8TurnTableStnEnum stnNo, ref bool isEmpty)
        {
            isEmpty = false;
            TrayModel ret = null;
            var info = GetLotStageInfo(stnNo);
            var temp = info.IsExist();
            if (info != null && info.LotModel != null && info.LotModel.TrayList.Count > 0)
            {
                ret = info.LotModel.TrayList.Last().DeepCopy();
                info.LotModel.TrayList.RemoveAt(info.LotModel.TrayList.Count - 1);
                info.LotModel.UpdateMap();
                info.LotModel.TrayCount--;
                LotStageInfoUpdated(stnNo, info);
            }
            else
            {
                info.IsEmpty = true;
            }
            if (info.LotModel.TrayList.Count == 0)
            {
                info.IsEmpty = true;
            }
            if (info.IsEmpty)
            {
                info.LotModel.LotStatus = LotStatusEnum.Null;
            }
            isEmpty = info.IsEmpty;
            LogManager.GetLogger("ProcInfoHelper").Info($"方法：PickTrayFromLot，工位：{stnNo}，TrayCode：{ret?.TrayCode}");
            return ret;
        }
        /// <summary>
        /// 往Lot中放置满Tray
        /// </summary>
        /// <param name="stnNo">工位号，不跟随转盘转动变化</param>
        /// <param name="trayModel">要放置的Tray信息</param>
        public bool PutTrayToLot(Stn8TurnTableStnEnum stnNo, TrayModel trayModel)
        {
            bool ret = false;
            try
            {
                var info = GetLotStageInfo(stnNo);
                if (info != null)
                {
                    if (info.LotModel == null)
                    {
                        info.LotModel = new LotModel();
                        info.LotModel.LOT = trayModel.LOT;
                        info.LotModel.WAFER = trayModel.WAFER;
                        info.LotModel.EqpInTime = DateTime.Now;
                    }
                    if (string.IsNullOrEmpty(info.LotModel.LotCode))
                    {
                        info.LotModel.LotCode = $"{info.LotModel.LOT}_{info.LotModel.WAFER}_{info.LotModel.EqpInTime.ToString("HHmmss")}";
                        LotModelHelper.Insert(info.LotModel);
                    }
                    if (info.LotModel.LotStatus != LotStatusEnum.FullTrayLot)
                    {
                        if (trayModel.BinLevel != 0)
                        {
                            info.LotModel.BinLevel = (BinLevel)trayModel.BinLevel;
                            if (trayModel.TrayStatus == TrayStatusEnum.NgFull &&
                                trayModel.BinLevel % 2 == 1)
                            {
                                info.LotModel.BinLevel += 1;
                            }
                        }
                        info.LotModel.TrayList.Add(trayModel);
                        info.LotModel.TrayCount++;
                        info.LotModel.UpdateMap();
                        info.LotModel.LotStatus = LotStatusEnum.HalfLot;
                        if (info.LotModel.TrayList.Count >= 6)
                        {
                            info.LotModel.LotStatus = LotStatusEnum.FullTrayLot;
                        }
                        LogManager.GetLogger("ProcInfoHelper").Info($"方法：PutTrayToLot，工位：{stnNo}，LotCode：{info.LotModel?.LotCode}，TrayBinLevel：{trayModel?.BinLevel}，LotBinLevel：{info.LotModel?.BinLevel}");
                        ret = true;
                        LotModelHelper.Update(info.LotModel);
                        LotStageInfoUpdated(stnNo, info);
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("ProcInfoHelper").Fatal(ex);
            }
            return ret;
        }
        public void UpdateStn8Status(Stn8TurnTableStnEnum stnNo, bool isBuzy)
        {
            var info = GetLotStageInfo(stnNo);
            if (info != null)
            {
                info.IsBuzy = isBuzy;
                LotStageInfoUpdated(stnNo, info);
            }
        }
        public void UpdateLotStatus(LotPosEnum posEnum, LotModel model)
        {
            var info = GetLotStageInfo(posEnum);
            if (info != null)
            {
                info.LotModel = model;
                LotStageInfoUpdated(posEnum, info);
            }
        }
        public LotStageInfo GetLotStageInfo(LotStageType lotStageType)
        {
            if (Stn8TurnTable != null)
            {
                return Stn8TurnTable[lotStageType.ToInt()];
            }
            return null;
        }
        #endregion

        #region HalfTrayBuffer
        public bool IsHalfTrayBufferExistTray(BinLevel binLevel, bool isOk, HalfTraySource source)
        {
            bool ret = false;
            if (binLevel != BinLevel.None)
            {
                var slotNo = GetTrayBufferLayerNo(binLevel, isOk, source);
                if (HalfTrayBuffer != null && HalfTrayBuffer.TrayArray != null && HalfTrayBuffer.TrayArray[slotNo] != null)
                {
                    ret = true;
                }
            }
            return ret;
        }
        public TrayModel GetTrayModelFromHalfTrayBuffer(BinLevel binLevel, bool isOk, HalfTraySource source)
        {
            TrayModel ret = null;
            if (binLevel != BinLevel.None)
            {
                var slotNo = GetTrayBufferLayerNo(binLevel, isOk, source);
                if (HalfTrayBuffer != null && HalfTrayBuffer.TrayArray != null && HalfTrayBuffer.TrayArray[slotNo] != null)
                {
                    ret = HalfTrayBuffer.TrayArray[slotNo].DeepCopy();
                }
            }
            return ret;
        }

        public TrayModel PickTrayFromHalfTrayBuffer(int layerNo)
        {
            TrayModel ret = null;
            if (HalfTrayBuffer != null && HalfTrayBuffer.TrayArray != null && HalfTrayBuffer.TrayArray[layerNo] != null)
            {
                ret = HalfTrayBuffer.TrayArray[layerNo].DeepCopy();
                HalfTrayBuffer.TrayArray[layerNo] = null;
                HalfTrayBuffer.UpdateMap();
                HalfTrayBufferInfoUpdated?.Invoke(HalfTrayBuffer);
                RedisHelper.Instance.ObjectSet<TrayModel>($"HalfTrayBuffer:{layerNo}", null);
            }
            return ret;
        }

        /// <summary>
        /// 往半Tray缓存中放置满Tray
        /// </summary>
        /// <param name="trayModel">要放置的Tray信息</param>
        /// <param name="layerNo">层号</param>
        public void PutTrayToHalfTrayBuffer(TrayModel trayModel, int layerNo)
        {
            if (HalfTrayBuffer != null)
            {
                if (HalfTrayBuffer.TrayArray == null)
                {
                    HalfTrayBuffer.TrayArray = new TrayModel[12];
                }
                HalfTrayBuffer.TrayArray[layerNo] = trayModel;
                HalfTrayBuffer.UpdateMap();
                HalfTrayBufferInfoUpdated?.Invoke(HalfTrayBuffer);
                RedisHelper.Instance.ObjectSet<TrayModel>($"HalfTrayBuffer:{layerNo}", trayModel);
            }
        }
        /// <summary>
        ///获取Bin等级在缓存中层号； Bin1,3,5的OK和NG半Tray分别存在1,2,3,4,5,6,7,8,9槽位中，Bin2,4,6分别存在10,11,12槽位中
        /// </summary>
        /// <param name="binLevel"></param>
        /// <param name="isOk"></param>
        /// <param name="source">来源标志，HalfTrayStage：半Tray缓存，Stn4：4工位转盘</param>
        /// <returns></returns>
        public int GetTrayBufferLayerNo(BinLevel binLevel, bool isOk, HalfTraySource source)
        {
            int ret = -1;
            var index = binLevel.ToInt();
            if (index % 2 == 1)
            {
                index = index / 2;
                ret = isOk ? 2 * index : 2 * index + 1;
                if (source == HalfTraySource.Stn4)
                {
                    ret = 6;
                    if (binLevel == BinLevel.Bin3)
                    {
                        ret = 7;
                    }
                    else if (binLevel == BinLevel.Bin5)
                    {
                        ret = 8;
                    }
                }
            }
            else
            {
                ret = 9;
                if (binLevel == BinLevel.Bin4)
                {
                    ret = 10;
                }
                else if (binLevel == BinLevel.Bin6)
                {
                    ret = 11;
                }
            }
            return ret;
        }
        #endregion

        #region LotBox
        public void LoadLotBox(string rfid, LotBoxStnType lotBoxStnType)
        {
            var stageNo = RotateInfoHelper.Instance.GetStageNo(lotBoxStnType.ToInt(), RotateStnType.LotBox);
            var lotBoxModel = new LotBoxModel();
            var prodPara = ParaHelper.Prod;
            lotBoxModel.Rfid = rfid;
            lotBoxModel.RowCount = prodPara.RowCountPerLotBox;
            lotBoxModel.ColCount = prodPara.ColCountPerLotBox;
            lotBoxModel.LotCellArray = new LotCellBase[prodPara.RowCountPerLotBox, prodPara.ColCountPerLotBox];

            LotBoxArray[stageNo] = lotBoxModel;
            lotBoxModel.UpdateMap();
            LotBoxInfoUpdated?.Invoke(lotBoxModel, lotBoxStnType);
            RedisHelper.Instance.ObjectSet($"LotBoxStnType:{lotBoxStnType}", lotBoxModel);
        }

        /// <summary>
        /// 从外盒取出LotBox内盒，model为要取出的LotBox信息
        /// </summary>
        /// <param name="model"></param>
        public LotBoxModel PickLotBox(LotBoxStnType lotBoxStnType)
        {
            var stageNo = RotateInfoHelper.Instance.GetStageNo(lotBoxStnType.ToInt(), RotateStnType.LotBox);
            LotBoxModel model = LotBoxArray[stageNo].DeepCopy();
            LotBoxArray[stageNo] = null;
            LotBoxInfoUpdated?.Invoke(null, lotBoxStnType);
            RedisHelper.Instance.ObjectSet<LotBoxModel>($"LotBoxStnType:{lotBoxStnType}", null);
            return model;
        }
        public void PutLotBox(LotBoxStnType lotBoxStnType, LotBoxModel model)
        {
            var stageNo = RotateInfoHelper.Instance.GetStageNo(lotBoxStnType.ToInt(), RotateStnType.LotBox);
            LotBoxArray[stageNo] = model;
            LotBoxInfoUpdated?.Invoke(model, lotBoxStnType);
            RedisHelper.Instance.ObjectSet($"LotBoxStnType:{lotBoxStnType}", model);
        }
        public LotBoxModel GetLotBoxInfo(LotBoxStnType lotBoxStnType)
        {
            LotBoxModel ret = null;
            var stageNo = RotateInfoHelper.Instance.GetStageNo(lotBoxStnType.ToInt(), RotateStnType.LotBox);
            if (LotBoxArray != null)
            {
                ret = LotBoxArray[stageNo];
            }
            return ret;
        }
        /// <summary>
        /// 从LotBox中取出Tray
        /// </summary>
        /// <returns></returns>
        public List<TrayModel> PickTraysFromLotBox(int rowNo, int colNo, int pickTrayCount)
        {
            List<TrayModel> trayList = null;
            try
            {
                var lotBoxModel = GetLotBoxInfo(LotBoxStnType.LulLotStn);
                if (lotBoxModel != null && lotBoxModel.LotCellArray != null)
                {
                    for (int r = 0; r < lotBoxModel.RowCount; r++)
                    {
                        for (int c = 0; c < lotBoxModel.ColCount; c++)
                        {
                            var cell = lotBoxModel.LotCellArray[r, c];
                            if (cell != null && cell.TrayList.Count >= pickTrayCount)
                            {
                                var tempList = cell.TrayList.Take(pickTrayCount).ToList();
                                if (tempList != null)
                                {
                                    trayList = tempList.Select(x => x.DeepCopy()).ToList();
                                    cell.TrayList.RemoveAll(x => tempList.Contains(x));
                                    cell.RowNo = r;
                                    cell.ColNo = c;
                                    cell.TrayCount -= pickTrayCount;
                                    cell.UpdateMap();
                                    LotCellInfoUpdated?.Invoke(cell, LotBoxType.EmptyTray);
                                    RedisHelper.Instance.ObjectSet($"LotBoxStnType:{LotBoxStnType.LulLotStn}", lotBoxModel);
                                    return trayList;
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(this.GetType().Name).Error($"PickTraysFromLotBox方法执行异常：{ex}");
            }
            return null;
        }
        /// <summary>
        /// 获取LotBox中可用的空Lot行列号，若无可用空Lot则返回(-1,-1)
        /// </summary>
        /// <returns></returns>
        public (int r, int c) GetAvlEmptyLotCellNo()
        {
            try
            {
                var lotBoxModel = GetLotBoxInfo(LotBoxStnType.LulLotStn);
                if (lotBoxModel != null && lotBoxModel.LotCellArray != null)
                {
                    for (int r = 0; r < lotBoxModel.RowCount; r++)
                    {
                        for (int c = 0; c < lotBoxModel.ColCount; c++)
                        {
                            var cell = lotBoxModel.LotCellArray[r, c];
                            if (cell != null && cell.TrayList.Count > 0)
                            {
                                return (r, c);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(this.GetType().Name).Error($"PickTraysFromLotBox方法执行异常：{ex}");
            }
            return (-1, -1);
        }
        /// <summary>
        /// 获取LotBox中可用的无Lot的穴位行列号，若无可用穴位则返回(-1,-1)
        /// </summary>
        /// <param name="stnNo"></param>
        /// <returns></returns>
        public (int r, int c) GetAvlNullCellNo(LotBoxStnType stnNo, ref LotCellBase model)
        {
            try
            {
                var lotBoxModel = GetLotBoxInfo(stnNo);
                if (lotBoxModel != null && lotBoxModel.LotCellArray != null)
                {
                    for (int r = 0; r < lotBoxModel.RowCount; r++)
                    {
                        for (int c = 0; c < lotBoxModel.ColCount; c++)
                        {
                            var cell = lotBoxModel.LotCellArray[r, c];
                            if (cell == null)
                            {
                                model = new LotCellBase();
                                model.RowNo = r;
                                model.ColNo = c;
                                model.LotList = new List<LotModel>();
                                model.LotCount = ParaHelper.Prod.LotCountPerLotCell;
                                lotBoxModel.LotCellArray[r, c] = model;
                                LogManager.GetLogger("ProcInfoHelper").Info($"方法：GetAvlNullCellNo，工位：{stnNo}，RowNo：{r}，ColNo：{c}");
                                return (r, c);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(this.GetType().Name).Error($"GetAvailableEmptyCellRCNo方法执行异常：{ex}");
            }
            return (-1, -1);
        }
        /// <summary>
        /// 获取LotBox中指定Bin等级Lot的可用穴位行列号，若无可用穴位则返回(-1,-1)
        /// </summary>
        /// <param name="binLevel"></param>
        /// <param name="stnNo"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        public (int r, int c) GetAvlBinLotBoxCellNo(BinLevel binLevel, LotBoxStnType stnNo, ref LotCellBase model)
        {
            if (binLevel != BinLevel.None)
            {
                var info = GetLotBoxInfo(stnNo);
                if (info != null && info.LotCellArray != null && info.LotCellArray.Length == info.RowCount * info.ColCount)
                {
                    for (int r = 0; r < info.RowCount; r++)
                    {
                        for (int c = 0; c < info.ColCount; c++)
                        {
                            var cell = info.LotCellArray[r, c];
                            if (cell != null && cell.BinLevel == binLevel && !cell.IsFull)
                            {
                                model = cell;
                                LogManager.GetLogger("ProcInfoHelper").Info($"方法：GetAvlBinLotBoxCellNo，工位：{stnNo}，RowNo：{r}，ColNo：{c}，BinLevel：{binLevel}");
                                return (r, c);
                            }
                        }
                    }
                }

            }
            return (-1, -1);
        }
        public LotCellBase GetLotCellModel(LotBoxStnType lotBoxStnType, int rowNo, int colNo)
        {
            LotCellBase ret = null;
            var info = GetLotBoxInfo(lotBoxStnType);
            if (info != null && info.LotCellArray != null)
            {
                ret = info.LotCellArray[rowNo, colNo];
            }
            return ret;
        }
        /// <summary>
        /// 放Lot到LotBox中指定的行列号穴位中，若该穴位已满则返回false
        /// </summary>
        /// <param name="lotModel"></param>
        /// <param name="lotBoxType"></param>
        /// <param name="rowNo"></param>
        /// <param name="colNo"></param>
        /// <returns></returns>
        public bool PutLotToLotBox(LotModel lotModel, LotBoxType lotBoxType, int rowNo, int colNo)
        {
            bool ret = false;
            try
            {
                var info = LotBoxArray[lotBoxType.ToInt()];
                if (info != null && info.LotCellArray != null)
                {
                    LotCellBase model = info.LotCellArray[rowNo, colNo];
                    model.BinLevel = lotModel.BinLevel;
                    if (model.IsFull == false)
                    {
                        model.LotList.Add(lotModel);
                        if (model.LotList.Count == ParaHelper.Prod.LotCountPerLotCell)
                        {
                            model.IsFull = true;
                        }
                        model.UpdateMap();
                        ret = true;
                    }
                    LotCellInfoUpdated?.Invoke(model, lotBoxType);
                    LotBoxStnType lotBoxStnType = lotModel.BinLevel.ToInt() % 2 != 0 ? LotBoxStnType.OKBinStn : LotBoxStnType.NGBinStn;
                    RedisHelper.Instance.ObjectSet($"LotBoxStnType:{lotBoxStnType}", model);
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(this.GetType().Name).Error($"PutLotToLotCell方法执行异常：{ex}");
            }
            return ret;
        }
        #endregion

        #region IRNgTray
        /// <summary>
        /// 获取IRNgTray中可用的行号，若无可用行则返回-1
        /// </summary>
        /// <returns></returns>
        public int GetIRNgTrayAvlRowNo()
        {
            int ret = -1;
            if (IRNgTray == null)
            {
                IRNgTray = new IRNgTrayModel();
            }
            for (int i = 0; i < IRNgTray.DieArray.Length; i++)
            {
                if (IRNgTray.DieArray[i] == null)
                {
                    ret = i;
                    break;
                }
            }
            return ret;
        }
        /// <summary>
        /// 放置Die到IRNgTray中指定的行号，若该行已有Die则会被覆盖
        /// </summary>
        /// <param name="rowNo"></param>
        /// <param name="dieModel"></param>
        public void PlaceDieToIRNgTray(int rowNo, DieModel dieModel)
        {
            if (IRNgTray == null)
            {
                IRNgTray = new IRNgTrayModel();
            }
            IRNgTray.DieArray[rowNo] = dieModel;
            IRNgTray.UpdateMap();
            IRNgTrayModelInfoUpdated?.Invoke(IRNgTray);
            RedisHelper.Instance.ObjectSet($"IRNgTray", IRNgTray);
        }
        public void PickTrayFromIRNgTray(int rowNo)
        {
            if (IRNgTray == null)
            {
                IRNgTray = new IRNgTrayModel();
            }
            IRNgTray.DieArray[rowNo] = null;
            IRNgTray.UpdateMap();
            IRNgTrayModelInfoUpdated?.Invoke(IRNgTray);
            RedisHelper.Instance.ObjectSet($"IRNgTray", IRNgTray);
        }
        /// <summary>
        /// 清空IRNgTray中的所有Die信息
        /// </summary>
        public void ClearIRNgTray()
        {
            IRNgTray = null;
            IRNgTrayModelInfoUpdated?.Invoke(IRNgTray);
            RedisHelper.Instance.ObjectSet($"IRNgTray", IRNgTray);
        }
        #endregion
    }
}
