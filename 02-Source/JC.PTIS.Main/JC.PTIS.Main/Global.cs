using JC.PTIS.Main.Entities;
using JC.PTIS.Main.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main
{
    public class Global
    {
        /// <summary>
        /// 白班开始时间
        /// </summary>
        public const int DayStartHour = 8;
        /// <summary>
        /// 白班结束时间
        /// </summary>
        public const int DayEndHour = 20;
        public static double[] TurnTableAngle = new double[RotateStnType.None.ToInt()];
        public static RunMode RunMode = RunMode.正常模式;
        public static ConcurrentDictionary<int,bool> VacStsDictionary = new ConcurrentDictionary<int,bool>();
        public static BinLevel SelectedBin=BinLevel.Bin1;
        public static void Init()
        {
            for (int i = 0; i < 16; i++)
            {
                VacStsDictionary.TryAdd(i,false);
            }
        }
        public static Color GetColor(DieStatus status)
        {
            //根据不同状态返回不同的颜色
            switch (status)
            {
                case DieStatus.None:
                    return Color.White;
                //case DieStatus.IROK:
                //    return Color.MediumAquamarine;
                case DieStatus.IRNG:
                    return Color.Firebrick;
                case DieStatus.OK:
                    return Color.Turquoise;
                //case DieStatus.BackOK:
                //    return Color.DarkCyan;
                case DieStatus.BackNG:
                    return Color.Salmon;
                case DieStatus.FrontNG:
                    return Color.Red;
                default:
                    return Color.Gainsboro;
            }
        }
        public static Color GetBinColor(BinLevel bin)
        {
            Color ret = Color.LimeGreen;
            switch (bin)
            {
                case BinLevel.None:
                    ret = Color.White;
                    break;
                case BinLevel.Bin1:
                    ret = Color.LawnGreen;
                    break;
                case BinLevel.Bin2:
                    ret = Color.Violet;
                    break;
                case BinLevel.Bin3:
                    ret = Color.YellowGreen;
                    break;
                case BinLevel.Bin4:
                    ret = Color.Magenta;
                    break;
                case BinLevel.Bin5:
                    ret = Color.ForestGreen;
                    break;
                case BinLevel.Bin6:
                    ret = Color.DarkMagenta;
                    break;
                case BinLevel.Bin7:
                    ret = Color.Thistle;
                    break;
                case BinLevel.Bin8:
                    ret = Color.Orange;
                    break;
                case BinLevel.Bin9:
                    ret = Color.Yellow;
                    break;
                case BinLevel.BinM:
                    ret = Color.Purple;
                    break;
                case BinLevel.BinAt:
                case BinLevel.BinDot:
                    ret = Color.Black;
                    break;
                default:
                    break;
            }
            return ret;
        }
    }
    public enum MotionStatus
    {
        None,
        Arrived,
        Timeout,
    }
    public enum  Foup
    {
        Left,
        Right
    }
    public enum BinLevel
    {
        None,
        Bin1,
        Bin2,
        Bin3,
        Bin4,
        Bin5,
        Bin6,
        Bin7,
        Bin8,
        Bin9,
        BinM,
        BinAt,
        BinDot,
    }
    public enum DieStatus
    {
        None,
        InWafer,
        Picked,
        IRNG,
        BackNG,
        FrontNG,
        OK,
    }
    /// <summary>
    /// 料盒类型，跟随料盒位置变化而变化
    /// </summary>
    public enum LotBoxType
    {
        [Description("OK Bin料盒")]
        Bin135,
        [Description("NG Bin料盒")]
        Bin246,
        [Description("空Tray料盒")]
        EmptyTray,
        Null,
      
    }
    /// <summary>
    /// 料盒工位类型枚举，不跟随料盒位置变化而变化
    /// </summary>
    public enum LotBoxStnType
    {
        OKBinStn,
        LulLotStn,
        NGBinStn,
        NullStn,
        ExBox1Stn,
        ExBox2Stn,
    }
    /// <summary>
    /// 上料转塔工位名称枚举。注意：枚举顺序一定不能变
    /// </summary>
    public enum Stn16TurnTableStnEnum
    {
        /// <summary>
        /// 取Die
        /// </summary>
        PickDie=4,
        /// <summary>
        /// 背面检
        /// </summary>
        BackDetect=9,
        /// <summary>
        /// IR检测
        /// </summary>
        IRDetect=10,
        /// <summary>
        /// 放Die
        /// </summary>
        PlaceDie=12,
        /// <summary>
        /// IR NG Die丢弃
        /// </summary>
        ThrowDie=14,
    }
    /// <summary>
    /// 下料Tray转塔工位名称枚举。注意：枚举顺序一定不能变
    /// </summary>
    public enum Stn4TurnTableStnEnum
    {
        /// <summary>
        ///  上下Tray工位
        /// </summary>
        LuloadTrayStn,
        /// <summary>
        /// Die进Tray工位
        /// </summary>
        DieToTray,
        /// <summary>
        /// 测高工位
        /// </summary>
        MeasureHeight,
        /// <summary>
        /// 正面检+挑补工位
        /// </summary>
        PickPatchStn,
        None,
    
    }
    /// <summary>
    /// 挑补转塔工位名称枚举。注意：枚举顺序一定不能变
    /// </summary>
    public enum PickPatchStnEnum
    {
        /// <summary>
        /// 正面检工位
        /// </summary>
        FrontStn,
        /// <summary>
        /// 半Tray工位
        /// </summary>
        HalfTrayStn=8,
    }
    public enum LotStageType
    {
        [Description("Bin1")]
        Bin1Stage,
        [Description("Bin2")]
        Bin2Stage,
        [Description("Bin3")]
        Bin3Stage,
        [Description("Bin4")]
        Bin4Stage,
        [Description("Bin5")]
        Bin5Stage,
        [Description("Bin6")]
        Bin6Stage,
        /// <summary>
        /// 上下Lot平台
        /// </summary>
        [Description("上空Tray")]
        LulLotStage,
        /// <summary>
        /// 手动投满Tray平台
        /// </summary>
        [Description("手投满Tray")]
        ManualLoadStage,
    }
    /// <summary>
    /// 8工位转塔工位名称枚举。注意：枚举顺序一定不能变
    /// </summary>
    public enum Stn8TurnTableStnEnum
    {
        /// <summary>
        /// 人工投料工位
        /// </summary>
        ManunalLoadTray=0,
        /// <summary>
        /// 上下Lot工位
        /// </summary>
        LulLot =2,
        /// <summary>
        ///取空Tray工位
        /// </summary>
        PickEmptyTrayStn=6,
        /// <summary>
        ///放满Tray工位
        /// </summary>
        PlaceFullTrayStn = 6,
    }
    /// <summary>
    /// 转塔工位类型枚举
    /// </summary>
    public enum RotateStnType
    {
        /// <summary>
        /// 上料转塔工位
        /// </summary>
        LoadDie,
        /// <summary>
        /// 4工位盘转塔工位
        /// </summary>
        Stn4TurnTable,
        /// <summary>
        /// 挑补转塔工位
        /// </summary>
        PickPatch,
        /// <summary>
        /// 8工位盘转塔工位
        /// </summary>
        Stn8TurnTable,
  
        LotBox,
        None,
    }
   
    public enum RegisterEnum
    {
        [Description("缓存平台存在Wafer")]
        WaferBufferExistWafer,
        [Description("缓存平台允许取放Wafer")]
        WaferBufferAllowPickPlaceWafer,
        /// <summary>
        /// WaferTable请求从缓存平台取满Wafer
        /// </summary>
        [Description("WaferTable请求从缓存平台取满Wafer")]
        WaferTableLoadWaferReq,
        /// <summary>
        /// WaferTable请求放空Wafer到缓存平台
        /// </summary>
        [Description("WaferTable请求放空Wafer到缓存平台")]
        WaferTableUnloadWaferReq,
        /// <summary>
        /// WaferTable取Wafer完成
        /// </summary>
        [Description("WaferTable取Wafer完成")]
        WaferTableLoadWaferDone,
        /// <summary>
        /// WaferTable放Wafer完成
        /// </summary>
        [Description("WaferTable放Wafer完成")]
        WaferTableUnloadWaferDone,
        /// <summary>
        /// Die对位完成
        /// </summary>
        [Description("Die对位完成")]
        AlignDieDone,
  
        /// <summary>
        /// PickDieStn工位取Die完成
        /// </summary>
        [Description("PickDieStn工位取Die完成")]
        PickDieStnPickDone,
        /// <summary>
        /// PickDieStn工位安全信号
        /// </summary>
        [Description("PickDieStn工位安全信号")]
        PickDieStnSafe,
        /// <summary>
        /// PlaceDieStn工位安全信号
        /// </summary>
        [Description("PlaceDieStn工位安全信号")]
        PlaceDieStnSafe,
        /// <summary>
        /// LoadDDRotate完成信号
        /// </summary>
        [Description("LoadDDRotate完成信号")]
        LoadDDRotateDone,
        /// <summary>
        /// LoadDie工位请求放Die信号
        /// </summary>
        [Description("LoadDie工位请求放Die信号")]
        LoadDieStnPlaceDieReq,
        /// <summary>
        /// LoadDie工位请求放Die进Tray盘
        /// </summary>
        [Description("LoadDie工位请求放Die进Tray盘")]
        PlaceDieStnPlaceDieReq,
        /// <summary>
        /// DieToTray工位允许放Die信号
        /// </summary>
        [Description("DieToTray工位允许放Die信号")]
        DieToTrayStnAllowPlaceDie,
        /// <summary>
        /// 当前Bin等级取Die结束
        /// </summary>
        [Description("当前Bin等级取Die结束")]
        CurrentBinLevelPickOver,
        /// <summary>
        /// PlaceDie工位放Die完成信号
        /// </summary>
        [Description("PlaceDie工位放Die完成信号")]
        PlaceDieCom,
        /// <summary>
        /// DieToTray工位接收Die完成信号
        /// </summary>
        [Description("DieToTray工位接收Die完成信号")]
        DieToTrayDone,
        /// <summary>
        /// 红外检测工位拍照完成信号
        /// </summary>
        [Description("红外检测工位拍照完成信号")]
        IRDetectGrabDone,
        /// <summary>
        /// 背面检测工位拍照完成信号
        /// </summary>
        [Description("背面检测工位拍照完成信号")]
        BackDetectGrabDone,
        /// <summary>
        /// 正面检测工位拍照完成信号
        /// </summary>
        [Description("正面检测工位拍照完成信号")]
        FrontDetectGrabDone,
        /// <summary>
        /// 4工位转盘转动完成信号
        /// </summary>
        [Description("4工位转盘转动完成信号")]
        Stn4DDRotateDone,
        /// <summary>
        /// 挑补工位安全信号
        /// </summary>
       [Description("挑补工位安全信号")]
       PickPatchStnSafe,
        /// <summary>
        /// 测高工位测高完成信号
        /// </summary>
        [Description("测高工位测高完成信号")]
        MeasureHeightDone,
    
        /// <summary>
        /// HalfTray工位切换半Tray完成
        /// </summary>
        [Description("HalfTray工位切换半Tray完成")]
        ShiftHalfTrayDone,
        /// <summary>
        /// HalfTray工位取单颗Die完成
        /// </summary>
        [Description("HalfTray工位取单颗Die完成")]
        HalfTrayPickOneDieDone,
        /// <summary>
        /// 挑补DD马达转动完成
        /// </summary>
        [Description("挑补DD马达转动完成")]
        PickPatchDDRotateDone,
    
        /// <summary>
        /// HalfTray工位放单颗Die完成
        /// </summary>
        [Description("HalfTray工位放单颗Die完成")]
        HalfTrayPlaceOneDieDone,
        /// <summary>
        /// 切换Bin等级
        /// </summary>
        [Description("切换Bin等级请求")]
        ShiftBinLevelReq,
     /// <summary>
     /// 挑补流程结束
     /// </summary>
        [Description("挑补流程结束")]
        PickPatchEnd,
        /// <summary>
        /// 缓存HalfTray工位半Tray
        /// </summary>
        [Description("缓存半Tray请求")]
        StorageHalfTrayReq,
        /// <summary>
        /// 缓存半Tray完成
        /// </summary>
        [Description("缓存半Tray完成")]
        StorageHalfTrayDone,
    
        /// <summary>
        /// Tray上下料完成信号
        /// </summary>
        [Description("Tray上下料完成信号")]
        TrayLuloadDone,

        /// <summary>
        /// LuloadTray工位请求下满Tray
        /// </summary>
        [Description("LuloadTray工位下满Tray请求")]
        Stn4DDUnloadFullTrayReq,
     
        /// <summary>
        /// 4工位转盘上下Tray工位请求上空Tray
        /// </summary>
        [Description("LuloadTray工位上空Tray请求")]
       Stn4DDStnLoadEmptyReq,
     
        /// <summary>
        /// TrayArm取空Tray完成
        /// </summary>
        [Description("满TrayArm取空Tray完成")]
       FullArmPickEmptyTrayFromHalfTrayDone,
        /// <summary>
        /// TrayArm放满OkTray完成
        /// </summary>
        [Description("满TrayArm放满OkTray完成")]
        TrayArmPutOKFullTrayDone,
        /// <summary>
        /// TrayArm放空Tray到HalfTray平台完成
        /// </summary>
        [Description("满TrayArm放空Tray到HalfTray平台完成")]
        TrayArmPutEmptyTrayDone,
        /// <summary>
        /// HalfTray工位请求进满Tray
        /// </summary>
        [Description("HalfTray工位进满Tray请求")]
        HalfTrayStnLoadFullTrayReq,
        /// <summary>
        /// HalfTray工位请求进空Tray
        /// </summary>
        [Description("HalfTray工位请求进空Tray请求")]
        HalfTrayStnLoadEmptyTrayReq,
     
        /// <summary>
        /// HalfTray工位下满Tray请求
        /// </summary>
        [Description("HalfTray工位下满Tray请求")]
        HalfTrayStnUnloadFullTrayReq,
        /// <summary>
        /// HalfTray工位下空Tray请求
        /// </summary>
        [Description("HalfTray工位下空Tray请求")]
        HalfTrayStnUnloadEmptyTrayReq,
        /// <summary>
        /// TrayArm放空Tray完成
        /// </summary>
        [Description("TrayArm放空Tray完成")]
        TrayArmLoadEmptyTrayDone,
        /// <summary>
        /// TrayArm取满Tray完成
        /// </summary>
        [Description("TrayArm取满Tray完成")]
        TrayArmUnloadFullTrayDone,
        /// <summary>
        /// Stn8TurnTable工位请求上空Lot
        /// </summary>
        [Description("Stn8TurnTable工位上空Lot请求")]
        Stn8TurnTableLoadEmptyLotReq,
        /// <summary>
        /// LotBox工位上空Lot完成
        /// </summary>
        [Description("LotBox工位上空Lot完成")]
        LotBoxStnLoadEmptyLotDone,
        /// <summary>
        /// Stn8TurnTable工位请求下满Lot
        /// </summary>
        [Description("Stn8TurnTable工位下满Lot请求")]
        Stn8TurnTableUnloadFullLotReq,
        /// <summary>
        /// LotBox工位下满Lot完成
        /// </summary>
        [Description("LotBox工位下满Lot完成")]
        LotBoxStnUnloadFullLotDone,
  
        /// <summary>
        /// Ok半Tray缓存空料，通知正面检工位只挑不补
        /// </summary>
        [Description("Ok半Tray缓存空料")]
        OkHalfTrayEmpty,
        /// <summary>
        /// 正面检工位有NgDie，通知挑补工位执行挑补流程
        /// </summary>
        [Description("正面检工位有NgDie")]
        FrontDetectExistNgDie,
        /// <summary>
        /// 正面检不存在NgDie，通知挑补工位不执行挑补流程
        /// </summary>
        [Description("正面检工位不存在NgDie")]
        FrontDetectNoExistNgDie,
        /// <summary>
        /// 通知HalfTray工位挑Die开始
        /// </summary>
        [Description("通知HalfTray工位挑Die开始")]
        PickFlowStart,
        /// <summary>
        /// 通知HalfTray工位挑Die结束
        /// </summary>
        [Description("通知HalfTray工位挑Die结束")]
        PickFlowEnd,
        /// <summary>
        /// 通知HalfTray工位补Die开始
        /// </summary>
        [Description("通知HalfTray工位补Die开始")]
        PatchFlowStart,
        /// <summary>
        /// 挑补工位补Die流程准备
        /// </summary>
        [Description("挑补工位补Die流程准备")]
        PatchFlowReady,
        /// <summary>
        /// 请求从隔离纸工位放隔离纸
        /// </summary>
        [Description("8工位转盘放隔离纸请求")]
        PlacePaperReq,
        /// <summary>
        /// 隔离纸工位放置隔离纸完成
        /// </summary>
        [Description("隔离纸工位放隔离纸完成")]
        PlacePaperDone,
        /// <summary>
        /// 满Tray手臂请求从半Tray工位取满Tray
        /// </summary>
        [Description("满Tray手臂请求从半Tray工位取满Tray完成")]
        FullTrayArmPickFullTrayFromHalfTrayDone,
        /// <summary>
        /// PackingStn工位请求放满Lot到LotBox
        /// </summary>
        [Description("PackingStn工位请求放满Lot到LotBox")]
        PutFullLotToLotBoxReq,
        /// <summary>
        /// PackingStn工位请求从LotBox工位取Lot
        /// </summary>
        [Description("PackingStn工位从LotBox工位取Lot请求")]
        PickLotFromLotBoxReq,
        /// <summary>
        /// LotBox工位允许放满Lot到LotBox
        /// </summary>
        [Description("LotBox工位允许放满Lot到LotBox")]
        LotBoxStnAllowPutFullLot,
        /// <summary>
        /// 放满Lot到LotBox完成
        /// </summary>
        [Description("放满Lot到LotBox完成")]
        PutFullLotToLotBoxDone,
        /// <summary>
        /// LotBox工位允许从LotBox取Lot
        /// </summary>
        [Description("LotBox工位允许从LotBox取Lot")]
        LotBoxStnAllowPickLot,
        /// <summary>
        /// 从LotBox取Lot完成
        /// </summary>
        [Description("从LotBox取Lot完成")]
        PickLotFromLotBoxDone,

        /// <summary>
        /// 满Tray手臂存在待缓存的半Tray
        /// </summary>
        [Description("满Tray手臂缓存半Tray请求")]
        FullTrayArmStorageHalfTrayReq,
        /// <summary>
        /// 半Tray平台到达放置半Tray位置
        /// </summary>
        [Description("半Tray平台到达放置半Tray位置")]
        HalfTrayStageArrivePutTrayPos,
        /// <summary>
        /// 满Tray手臂没有半Tray需要暂存
        /// </summary>
        [Description("满Tray手臂没有半Tray需要暂存")]
        TrayArmNoExistHalfTrayToStorage,
        /// <summary>
        /// Wafer取Die完成
        /// </summary>
        [Description("Wafer取Die完成")]
        WaferEmpty,
        /// <summary>
        /// 4工位转盘请求从半Tray缓存取半Tray
        /// </summary>
        [Description("4工位转盘从半Tray缓存取半Tray请求")]
        LoadTrayFromHalfTrayBufferToStn4Req,
        /// <summary>
        /// 满Tray手臂从半Tray缓存取半Tray请求
        /// </summary>
        [Description("满Tray手臂从半Tray缓存取半Tray请求")]
        FullTrayArmPickHalfTrayFromBufferReq,
        /// <summary>
        /// Ok半Tray平台允许取Tray
        /// </summary>
        [Description("Ok半Tray平台允许取Tray")]
        OkHalfTrayStageAllowPickTray,
        /// <summary>
        /// 满Tray手臂从Ok半Tray平台取Tray完成
        /// </summary>
        [Description("满Tray手臂从Ok半Tray平台取Tray完成")]
        FullTrayArmPickTrayFromOkHalfTrayStageDone,
        /// <summary>
        /// 从半Tray缓存取半Tray到4工位转盘完成
        /// </summary>
        [Description("从半Tray缓存取半Tray到4工位转盘完成")]
        LoadTrayFromHalfTrayBufferToStn4Done,
        /// <summary>
        /// WaferTable上没有Wafer
        /// </summary>
        [Description("WaferTable上没有Wafer")]
        WaferTableNoExistWafer,
        /// <summary>
        /// WaferTable上有Wafer
        /// </summary>
        [Description("WaferTable上有Wafer")]
        WaferTableExistWafer,
        /// <summary>
        /// HalfTray工位确认是否需要暂存半Tray完成
        /// </summary>
        [Description("HalfTray工位确认是否需要暂存半Tray完成")]
        HalfTrayConfirmDone,




        None,
    }
    public enum VisionCmd
    {
        Heartbeat,
        GetDeviceStatus,
        ResetFault,
        StopDetection,
        TriggerDetection,
        ChangeRecipe,
        DetectionCompleted,

        #region 对位PC指令
        /// <summary>
        /// 获取补正值
        /// </summary>
        Align,
        /// <summary>
        /// 只拍照
        /// </summary>
        Grab,
        /// <summary>
        /// 获取四个角Die坐标
        /// </summary>
        GetDieOffset,

        /// <summary>
        /// 束带检测
        /// </summary>
        DetectBelt,
        /// <summary>
        /// 标签检测
        /// </summary>
        DetectLabel,
        #endregion
        AutoCalibration,
        MotionRequest,
        TriggerRequest,
        #region 检测PC指令
        //IRDetect,
        //BackDetect,
        //FrontDetect
        #endregion
    }

    public enum RunMode
    {
        正常模式,
        空跑模式,
        强制清料,
        维修
    }
  
    public enum IntValueEnum
    {
        /// <summary>
        /// 挑补工位当前Bin等级
        /// </summary>
        ShiftBinLevel,
        /// <summary>
        /// 总NG Die数量
        /// </summary>
        TotalNGDieCount,


        None,
    }
  
    public enum TrayTypeEnum
    {
        Empty,
        Half,
        Full
    }
    public enum TrayStatusEnum
    {
        /// <summary>
        /// 空Tray，没有Die
        /// </summary>
        Empty,
        //Half,
        /// <summary>
        /// Ok Die半Tray
        /// </summary>
        OkHalf,
        /// <summary>
        /// Ng Die半Tray
        /// </summary>
        NgHalf,
        /// <summary>
        ///未检测半Tray，只有未检测的Die
        /// </summary>
        NoDetectHalf,
        /// <summary>
        /// 混合半Tray，有Ok Die和Ng Die和空Die
        /// </summary>
        MixHalf,
        /// <summary>
        /// Ok Die满Tray
        /// </summary>
        OkFull,
        /// <summary>
        /// Ng Die满Tray
        /// </summary>
        NgFull,
        /// <summary>
        /// 混合满Tray，有Ok Die和Ng Die
        /// </summary>
        MixFull,
        /// <summary>
        ///未检测满Tray，只有未检测的Die
        /// </summary>
        NoDetectFull,
        /// <summary>
        /// 满Tray，满Tray上有Ok Die和Ng Die
        /// </summary>
        //Full
    }
    public enum HalfTrayStnEnum
    {
        OkTray,
        NgTray,
        None,
    }
    public enum LaserEnum
    {
        /// <summary>
        /// 上料转塔工位测Tray高度
        /// </summary>
        LoadDieStnMeasureTray,
        /// <summary>
        /// 满Tray手臂测高
        /// </summary>
        FullTrayArmMeasureHeight,
        /// <summary>
        /// 空Tray手臂测高
        /// </summary>
        EmptyTrayArmMeasureHeight,
        /// <summary>
        /// 隔离纸手臂测高
        /// </summary>
        IsoPaperMeasureHeight,
        /// <summary>
        /// Lot手臂测高
        /// </summary>
        LotArmMeasureHeight,
    }
    public enum TrayPosEnum
    {
        /// <summary>
        /// 4工位转盘上的Tray
        /// </summary>
        Stn4TurnTable,
        /// <summary>
        /// 8工位转盘上的Tray
        /// </summary>
        //Stn8TurnTable,
        /// <summary>
        /// 空Tray手臂上的Tray
        /// </summary>
        EmptyTrayArm,
        /// <summary>
        /// 满Tray手臂上的Tray
        /// </summary>
        FullTrayArm,
        /// <summary>
        /// 空Tray缓存上的Tray
        /// </summary>
        EmptyTrayBufferStage,
        /// <summary>
        /// Ok半Tray平台上的Tray
        /// </summary>
        OkHalfTrayStage,
        /// <summary>
        /// Ng半Tray平台上的Tray
        /// </summary>
        NgHalfTrayStage,

        /// <summary>
        /// 半Tray缓存手臂上的Tray
        /// </summary>
        HalfTrayBufferArm,
        None,
    }
    public enum LotStatusEnum
    {
        Null,
        /// <summary>
        /// 空Tray盘Lot
        /// </summary>
        EmptyTrayLot,
        /// <summary>
        /// 半Lot
        /// </summary>
        HalfLot,
        /// <summary>
        /// 准备束带
        /// </summary>
        FullTrayLot,
        /// <summary>
        /// 横向束带完成
        /// </summary>
        Belt1PackingDone,
        /// <summary>
        /// 横向束带检测完成
        /// </summary>
        Belt1DetectDone,
        /// <summary>
        /// 竖向束带完成
        /// </summary>
        Belt2PackingDone,
        /// <summary>
        /// 竖向束带检测完成
        /// </summary>
        Belt2DetectDone,
        /// <summary>
        /// 贴标签完成
        /// </summary>
        PasteLabelDone,

    }
    public enum  LotPosEnum
    {
        PackingStage,
        LotArmClaw,
        LotArmSucker,
        Stn8TurnTable,
        None
    }
   
    public enum Result
    {
        None,
        OK,
        NG,
    }
    public enum StepTimeEnum
    {
        DieMove,
        DieAlign,
        PickDie,
        PlaceDie,
    }
    public enum WaferPosEnum
    {
        WaferBufferUpLayer,
        WaferBufferDownLayer,
        WaferTable,
        None,
    }
    public enum TrayFlowStep
    {
        None,
        Loaded,//空Tray，准备放Die
        PlaceDie,//放Die中
        CurBinOver,//当前Bin等级取完
        MeasHeight,//测高中
        PickPatch,//挑补中
        Unload,//准备下料
        InLot,//放入Lot中
    }
    public enum HalfTraySource
    {
        /// <summary>
        /// 半Tray平台上的半Tray
        /// </summary>
        HalfTrayStage,
        /// <summary>
        /// 4工位转盘上的半Tray
        /// </summary>
        Stn4
    }
    public enum DetectStationEnum
    {
        BackDetect,
        IRDetect,
        FrontDetect,
    }
    public enum AlignStationEnum
    {
        /// <summary>
        /// Notch拍照
        /// </summary>
        WaferNotch,
        /// <summary>
        /// Wafer四条边拍照
        /// </summary>
        WaferEdge,
        /// <summary>
        /// WaferMark拍照
        /// </summary>
        WaferMark,
        /// <summary>
        /// Paper拍照
        /// </summary>
        IsoPaper,
        /// <summary>
        /// Lot拍照对位
        /// </summary>
        Lot,
        /// <summary>
        /// 标签对位
        /// </summary>
        Label,

        /// <summary>
        /// Wafer中的Die对位
        /// </summary>
        Die_WaferTable,
        /// <summary>
        /// 入Tray平台Die对位
        /// </summary>
        Die_DieToTray,
        /// <summary>
        /// 挑补平台Die对位
        /// </summary>
        Die_PickPatch,
        /// <summary>
        /// Ok半Tray平台Die对位
        /// </summary>
        Die_HalfOkTray,
        /// <summary>
        /// Ng半Tray平台Die对位
        /// </summary>
        Die_HalfNgTray,

        /// <summary>
        /// 入Tray平台Tray对位
        /// </summary>
        Tray_DieToTray,
        /// <summary>
        /// 挑补平台Tray对位
        /// </summary>
        Tray_PickPatch,
        /// <summary>
        /// Ok半Tray平台Tray对位
        /// </summary>
        Tray_HalfOkTray,
        /// <summary>
        /// Ng半Tray平台Tray对位
        /// </summary>
        Tray_HalfNgTray,

        None,
    }
}
