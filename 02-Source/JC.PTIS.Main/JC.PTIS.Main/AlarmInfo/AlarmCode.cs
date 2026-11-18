using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.AlarmInfo
{
    public enum AlarmCode
    {
        #region 运动相关报警
        AxisMoveTimeoutStart = 1000,
        CylinderTimeoutStart = 1500,
        VacuumTimeoutStart = 1700,
        #endregion

        #region 轻报警，只弹窗，不暂停流程
        /// <summary>
        /// IRNgTray满料，请取走
        /// </summary>
        IRNgTrayFull = 2000,
        #endregion

        #region 中报警，弹窗并且暂停流程
        /// <summary>
        /// MarkDie1拍照超时
        /// </summary>
        AlignPCMarkDie1AlignFail = 3000,
        /// <summary>
        /// MarkDie2拍照超时
        /// </summary>
        AlignPCMarkDie2AlignFail,
        /// <summary>
        /// 计算Wafer偏移值失败
        /// </summary>
        AlignPCCalcWaferOffsetFail,
        /// <summary>
        /// 计算Die偏移值失败
        /// </summary>
        AlignPCCalcDieOffsetFail,
        /// <summary>
        /// 计算四个顶点Die抓取位置失败
        /// </summary>
        CalcFourCornerDieGrabPosFail,
        /// <summary>
        /// 获取Die拍照位置失败
        /// </summary>
        GetDieGrabPosFail,

        /// <summary>
        /// 从Wafer中获取DieModel信息失败
        /// </summary>
        GetDieModelFromWaferFail,
        /// <summary>
        /// 检测PC IR检测准备失败
        /// </summary>
        DetectPCIRDetectReadyFail,
        /// <summary>
        /// 检测PC 背面检测准备失败
        /// </summary>
        DetectPCBackDetectReadyFail,
        /// <summary>
        /// 检测PC 正面检测准备失败
        /// </summary>
        DetectPCFrontDetectReadyFail,
        /// <summary>
        /// Die对位失败
        /// </summary>
        DieAlignFail,
        /// <summary>
        /// Tray对位失败
        /// </summary>
        TrayAlignFail,
        /// <summary>
        /// 标签对位失败
        /// </summary>
        AlignLabelFail,

        /// <summary>
        /// 从上料转达吸嘴获取DieModel失败
        /// </summary>
        GetDieModelFromLoadDDPickerFail,
        /// <summary>
        /// 读取高度值失败
        /// </summary>
        MeasureHeightFail,
        /// <summary>
        /// 获取LotModel失败
        /// </summary>
        GetLotModelFail,
        /// <summary>
        /// 隔离纸厚度异常
        /// </summary>
        PaperThicknessFail,
        /// <summary>
        /// 隔离纸对位失败
        /// </summary>
        PaperAlignFail,
        /// <summary>
        /// 标签对位失败
        /// </summary>
        LabelAlignFail,
        /// <summary>
        /// Lot对位失败
        /// </summary>
        LotAlignFail,
        /// <summary>
        /// 隔离纸料仓中没有纸
        /// </summary>
        PaperCabinetNoPaper,
        /// <summary>
        /// 束带异常，请人工处理
        /// </summary>
        PackingBeltDetectFail,
        /// <summary>
        /// 获取对应Bin等级的可放Lot穴位号失败
        /// </summary>
        GetAvlBinLotBoxCellNoFail,
        /// <summary>
        /// 获取可以取空Lot的穴位号失败
        /// </summary>
        GetEmptyLotCellNoFail,
        /// <summary>
        /// 标签机出标签失败
        /// </summary>
        PrintLabelFail,
        /// <summary>
        /// 外盒1中已经有内盒，无法请求天车上内盒
        /// </summary>
        ExBoxStn1ExistBox,
        /// <summary>
        /// 外盒2中已经有内盒，无法请求天车上内盒
        /// </summary>
        ExBoxStn2ExistBox,
        /// <summary>
        /// 2个外盒中已经有内盒，无法请求天车上内盒
        /// </summary>
        BothExBoxStnExistBox,
        /// <summary>
        /// 外盒1中没有内盒，无法进行上内盒流程
        /// </summary>
        ExBoxStn1NoExistBox,
        /// <summary>
        /// 外盒2中没有内盒，无法进行上内盒流程
        /// </summary>
        ExBoxStn2NoExistBox,
        /// <summary>
        /// 2个外盒中都没有内盒，无法请求天车下内盒
        /// </summary>
        BothExBoxStnNoExistBox,
        /// <summary>
        /// 天车上料盒超时
        /// </summary>
        OHTLoadLotBoxTimeout,
        /// <summary>
        /// 天车下料盒超时
        /// </summary>
        OHTUnloadLotBoxTimeout,
        /// <summary>
        /// 从挑补转塔吸嘴获取DieModel失败
        /// </summary>
        GetDieModelFromPickPatchDDPickerFail,
        /// <summary>
        /// 从半Tray缓存中获取TrayModel信息失败
        /// </summary>
        GetTrayInfoFromBufferFail,
        /// <summary>
        /// Lot传感器检测Lot失败
        /// </summary>
        LotSensorDetectLotFail,
        /// <summary>
        /// 空TrayArm存在Tray，无法执行取空Tray流程
        /// </summary>
        EmptyTrayArmExistTray,
        /// <summary>
        /// 满TrayArm存在Tray，无法执行取满Tray流程
        /// </summary>
        FullTrayArmExistTray,
        /// <summary>
        /// 夹爪Sensor未检测到Wafer，无法执行取Wafer流程
        /// </summary>
        WaferClawNoDetectWafer,
        /// <summary>
        /// 扫码失败
        /// </summary>
        ScancodeFail,
        /// <summary>
        /// 空TrayArm上没有Tray
        /// </summary>
        EmptyTrayArmNoExistTray,
        GetWaferModelFromTableFail,
        /// <summary>
        /// 当前Tray计算行列号超过Tray行列总数
        /// </summary>
        CurTrayRowColCountExceedTrayCount,
        /// <summary>
        /// 获取LoadPort状态失败
        /// </summary>
        GetLoadPortStatusFail,
        /// <summary>
        /// LoadPort1上料Foup失败
        /// </summary>
        LoadPort1LoadFoupFail,
        /// <summary>
        /// LoadPort2上料Foup失败
        /// </summary>
        LoadPort2LoadFoupFail,
        #endregion

        #region 重报警，弹窗并且停止流程
        /// <summary>
        /// WaferBuffer工位程序异常，请检查代码
        /// </summary>
        ProgramExceptionStart = 10000,
        /// <summary>
        ///  WaferBufferStn工位程序异常，请检查代码
        /// </summary>
        WaferTableStnException,
        /// <summary>
        ///LoadDieDDStnException工位程序异常，请检查代码
        /// </summary>
        LoadDieDDStnException,
        /// <summary>
        /// PickDieStnException工位程序异常，请检查代码
        /// </summary>
        PickDieStnException,
        /// <summary>
        /// BackDetectStnException工位程序异常，请检查代码
        /// </summary>
        BackDetectStnException,
        /// <summary>
        /// IRDetectStnException工位程序异常，请检查代码
        /// </summary>
        IRDetectStnException,
        /// <summary>
        /// PlaceDieStnException工位程序异常，请检查代码
        /// </summary>
        PlaceDieStnException,
        /// <summary>
        /// Stn4TurnTableStnException工位程序异常，请检查代码
        /// </summary>
        Stn4TurnTableStnException,
        /// <summary>
        /// DieToTrayStnException工位程序异常，请检查代码
        /// </summary>
        DieToTrayStnException,
        /// <summary>
        /// MeasureHeightStnException工位程序异常，请检查代码
        /// </summary>
        MeasureHeightStnException,
        /// <summary>
        /// PickPatchStnException工位程序异常，请检查代码
        /// </summary>
        PickPatchStnException,
        /// <summary>
        /// LuloadTrayStnException工位程序异常，请检查代码
        /// </summary>
        LuloadTrayStnException,
        /// <summary>
        /// HalfTrayStnException工位程序异常，请检查代码
        /// </summary>
        HalfTrayStnException,
        /// <summary>
        /// Stn8TurnTableStnException工位程序异常，请检查代码
        /// </summary>
        Stn8TurnTableStnException,
        /// <summary>
        /// IsolationPaperStnException工位程序异常，请检查代码
        /// </summary>
        IsolationPaperStnException,
        /// <summary>
        /// PackingStnException工位程序异常，请检查代码
        /// </summary>
        PackingStnException,
        /// <summary>
        /// LotBoxStnException工位程序异常，请检查代码
        /// </summary>
        LotBoxStnException,
        /// <summary>
        /// ThrowNgDieStnException工位程序异常，请检查代码
        /// </summary>
        ThrowNgDieStnException,
        /// <summary>
        /// 计算扩摸后每颗Die的偏移量失败
        /// </summary>
        CalcDieOffsetFail = 10020,
        /// <summary>
        /// Lot夹爪上有Lot，禁止回零
        /// </summary>
        ClawExistLot,
        /// <summary>
        /// 获取点位失败
        /// </summary>
        GetPosInfoFail,
        /// <summary>
        /// 放Die流程出错
        /// </summary>
        PlaceDieFlowError,
        /// <summary>
        /// 获取Tray信息失败
        /// </summary>
        GetTrayInfoFail,
        /// <summary>
        /// 获取可放Die的行列号失败
        /// </summary>
        GetAvlEmptyRCNoFail,
        /// <summary>
        /// OkTray平台上没有Tray盘，无法执行UnloadTray流程
        /// </summary>
        OkTrayStageNoExistTrayCantUnloadTrayFlow,
        /// <summary>
        /// NgTray平台上没有Tray盘，无法执行UnloadTray流程
        /// </summary>
        NgTrayStageNoExistTrayCantUnloadTrayFlow,
        /// <summary>
        /// OkTray平台上有Tray盘，无法执行LoadTray流程
        /// </summary>
        OkTrayStageExistTrayCantLoadTrayFlow,
        /// <summary>
        /// NgTray平台上有Tray盘，无法执行LoadTray流程
        /// </summary>
        NgTrayStageExistTrayCantLoadTrayFlow,
        /// <summary>
        /// 获取Die信息失败
        /// </summary>
        GetDieModelFail,
        #endregion
        None = 0,
    }
}
