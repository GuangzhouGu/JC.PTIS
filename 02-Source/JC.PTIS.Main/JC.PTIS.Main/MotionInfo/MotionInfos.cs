using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.MotionInfo
{
    public enum StationName
    {
        [Description("Wafer缓存工位")]
        WaferBufferStn,
        [Description("Wafer平台工位")]
        WaferTableStn,
        [Description("上料转塔工位")]
        LoadDieDDStn,
        [Description("转塔取Die工位")]
        PickDieStn,
        [Description("隐裂检测工位")]
        IRDetectStn,
        [Description("背面检测工位")]
        BackDetectStn,
        [Description("转塔放Die工位")]
        PlaceDieStn,
        [Description("4工位转塔工位")]
        Stn4TurnTableStn,
        [Description("4工位转塔入Tray工位")]
        DieToTrayStn,
        [Description("4工位转塔测高工位")]
        MeasureHeightStn,
        [Description("4工位转塔正面检工位")]
        PickPatchStn,
        [Description("4工位转塔取放Tray工位")]
        LuloadTrayStn,
        [Description("半Tray挑补工位")]
        HalfTrayStn,
        [Description("8工位转塔工位")]
        Stn8TurnTableStn,
        [Description("隔离纸工位")]
        IsolationPaperStn,
        [Description("束带工位")]
        PackingStn,
        [Description("取放Lot工位")]
        LotBoxStn,
        [Description("IRNgDie抛料工位")]
        ThrowNgDieStn,
        [Description("Null")]
        None
    }
    public enum AxisName
    {
        #region  WaferBuffer工位
        WaferBuffer_X,
        WaferBuffer_Y,
        WaferBuffer_Z,
        WaferBuffer_ClawZ,
        #endregion

        #region WaferTable工位
        WaferTable_X,
        WaferTable_Y,
        WaferTable_Z,
        WaferTable_T,
        /// <summary>
        /// Wafer夹爪轴
        /// </summary>
        WaferTable_ArmX,
        /// <summary>
        /// 顶针帽
        /// </summary>
        WaferTable_ThimbleCap,
        /// <summary>
        /// 顶针
        /// </summary>
        WaferTable_Thimble,
        #endregion

        /// <summary>
        /// Die上料DD马达
        /// </summary>
        LoadDie_DD,

        /// <summary>
        /// 从Wafer中取DieZ轴
        /// </summary>
        PickDie_Z,



        #region 背面检工位
        BackDetect_X,
        BackDetect_Z,
        #endregion

        #region IR检测工位
        IRDetect_X,
        IRDetect_Z,
        IRNG_Y,
        #endregion

        /// <summary>
        /// 往Tray盘中放DieZ轴
        /// </summary>
        PlaceDie_Z,

        #region UnloadTray工位
        TrayStage1X,
        TrayStage1Y,
        TrayStage1T,
        TrayStage2X,
        TrayStage2Y,
        TrayStage2T,
        TrayStage3X,
        TrayStage3Y,
        TrayStage3T,
        TrayStage4X,
        TrayStage4Y,
        TrayStage4T,
        Stn4TurnTable_DD,
        #endregion


        #region 挑补工位
        PickPatch_DD,
        PickPatch_PickZ,
        PickPatch_PlaceZ,
        PickPatch_CamZ,
        PickPatch_CamY,
        #endregion

        #region 半Tray工位
        HalfTray_StageX,
        HalfTray_StageY,
        HalfTray_StageT,
        HalfTray_Y,
        HalfTray_BufferZ,
        HalfTray_BufferX,
        #endregion

        #region 搬Tray工位
        Stn8TurnTable_ArmX,
        Stn8TurnTable_ArmY,
        Stn8TurnTable_ArmZ,
        Stn8TurnTable_DD,
        Stn8TurnTable_Shift,
        #endregion

        #region 隔离纸工位
        IsolationPaper_LiftZ,
        IsolationPaper_Shift,
        IsolationPaper_ArmX,
        IsolationPaper_ArmY,
        IsolationPaper_ArmZ,
        IsolationPaper_ArmT,
        #endregion



        #region 打包工位
        /// <summary>
        /// Lot手臂X轴
        /// </summary>
        Packing_ArmX,
        /// <summary>
        /// Lot手臂Y轴
        /// </summary>
        Packing_ArmY,
        /// <summary>
        /// Lot手臂T轴
        /// </summary>
        Packing_ArmT,
        /// <summary>
        /// Lot手臂Z轴
        /// </summary>
        Packing_ArmZ,
        /// <summary>
        /// Lot手臂夹爪轴
        /// </summary>
        Packing_Claw,
        /// <summary>
        /// 打包平台切机轴
        /// </summary>
        Packing_Shift,
        /// <summary>
        /// 取标签Z轴
        /// </summary>
        Packing_LabelZ,
        #endregion

        #region 搬Lot工位
        LotBox_ArmX,
        LotBox_ArmY,
        LotBox_ArmZ,
        #endregion
        None,
    }
    public enum CompareName
    {
        /// <summary>
        /// 红外检测飞拍
        /// </summary>
        IRDetectGrab,
        /// <summary>
        /// 背面检测飞拍
        /// </summary>
        BackDetectGrab,
        /// <summary>
        /// 正面检测飞拍
        /// </summary>
        FrontDetectGrab,
    }
    public enum InputName
    {
        #region 第1站
        /// <summary>
        /// 设备动力电源检测信号，DI-00
        /// </summary>
        PowerDetect,
        /// <summary>
        /// 设备正压检知1，DI-01
        /// </summary>
        Cda1Detect,
        /// <summary>
        /// 设备正压检知2，DI-02
        /// </summary>
        Cda2Detect,
        /// <summary>
        /// 设备负压检知1，DI-03
        /// </summary>
        Vac1Detect,
        /// <summary>
        /// 设备负压检知2，DI-04
        /// </summary>
        Vac2Detect,
        /// <summary>
        /// 设备电箱烟雾报警检知，DI-05
        /// </summary>
        SmokeAlarmDetect,
        /// <summary>
        /// 夹爪到位，DI-09
        /// </summary>
        WaferBufferClawArriveDetect,
        /// <summary>
        /// 夹爪防撞Sensor，DI-10
        /// </summary>
        WaferBufferClawSafeDetect,
        /// <summary>
        /// 夹爪伸缩气缸伸出到位，DI-11
        /// </summary>
        StretchCylOutDetect,
        /// <summary>
        /// 夹爪伸缩气缸缩回到位，DI-12
        /// </summary>
        StretchCylOrgDetect,
        /// <summary>
        /// 吸盘吸附到位，DI-13
        /// </summary>
        WaferBufferClawVacDetect,
        /// <summary>
        /// Wafer缓存平台上层是否有Wafer检测信号1，DI-14
        /// </summary>
        UpLayerWaferDetect1,
        /// <summary>
        /// Wafer缓存平台下层是否有Wafer检测信号1，DI-15
        /// </summary>
        DownLayerWaferDetect1,
        /// <summary>
        /// WaferBuffer夹爪检测是否有Wafer信号，DI-16
        /// </summary>
        WaferBufferClawIsExistWafer,
        /// <summary>
        /// WaferTable夹爪检测是否有Wafer信号，DI-17
        /// </summary>
        WaferTableClawIsExistWafer,
        /// <summary>
        /// WaferTable是否有Wafer信号，DI-18
        /// </summary>
        WaferTableIsExistWafer,
        /// <summary>
        /// WaferTableZ轴扩摸到位检测信号，DI-19
        /// </summary>
        WaferTableZLiftArrive,
        /// <summary>
        /// WaferTableZ轴动力气缸伸出到位信号，DI-20
        /// </summary>
        WaferTableZPowerCylOutDetect,
        /// <summary>
        /// WaferTableZ轴动力气缸缩回到位信号，DI-21
        /// </summary>
        WaferTableZPowerCylOrgDetect,
        /// <summary>
        /// WaferTableZ轴极限Sensor气缸伸出到位信号，DI-22
        /// </summary>
        WaferTableZLimitCylOutDetect,
        /// <summary>
        /// WaferTableZ轴极限Sensor气缸缩回到位信号，DI-23
        /// </summary>
        WaferTableZLimitCylOrgDetect,
        /// <summary>
        /// WaferTable顶针气缸伸出到位信号，DI-24
        /// </summary>
        WaferTableThimbleCylOutDetect,
        /// <summary>
        /// WaferTable顶针气缸缩回到位信号，DI-25
        /// </summary>
        WaferTableThimbleCylOrgDetect,
        /// <summary>
        /// 顶针真空吸附到位信号，DI-26
        /// </summary>
        ThimbleVacArriveDetect,
        /// <summary>
        /// Wafer缓存平台上层是否有Wafer检测信号2，DI-27
        /// </summary>
        UpLayerWaferDetect2,
        /// <summary>
        /// Wafer缓存平台下层是否有Wafer检测信号2，DI-28
        /// </summary>
        DownLayerWaferDetect2,
        /// <summary>
        /// 入料口手自动旋钮手动信号，DI-31
        /// </summary>
        LoadPortManualBtn,
        /// <summary>
        /// 入料口手自动旋钮自动信号，DI-32
        /// </summary>
        LoadPortAutoBtn,
        /// <summary>
        /// 入料口急停按钮信号，DI-33
        /// </summary>
        LoadPortEmgStopBtn,
        /// <summary>
        /// 维修门检知信号1，DI-34
        /// </summary>
        MaintainDoor1Detect,
        /// <summary>
        /// 维修门1门锁检知信号，DI-35
        /// </summary>
        MaintainDoor1LockDetect,
        /// <summary>
        /// 维修门1按钮检知信号，DI-36
        /// </summary>
        MaintainDoor1BtnDetect,
        /// <summary>
        /// 维修门1检知信号，DI-37
        /// </summary>
        MaintainDoor2Detect,
        /// <summary>
        /// 维修门2门锁检知信号，DI-38
        /// </summary>
        MaintainDoor2LockDetect,
        /// <summary>
        /// 维修门2按钮检知信号，DI-39
        /// </summary>
        MaintainDoor2BtnDetect,
        /// <summary>
        /// 挑补工位急停按钮信号，DI-40
        /// </summary>
        PickPatchEmgStopBtn,
        /// <summary>
        /// 维修门3门锁检知信号，DI-41
        /// </summary>
        MaintainDoor3LockDetect,
        /// <summary>
        /// 维修门3按钮检知信号，DI-42
        /// </summary>
        MaintainDoor3BtnDetect,
        /// <summary>
        /// 维修门4门锁检知信号，DI-43
        /// </summary>
        MaintainDoor4LockDetect,
        /// <summary>
        /// 维修门4按钮检知信号，DI-44
        /// </summary>
        MaintainDoor4BtnDetect,
        /// <summary>
        /// 维修门5门锁检知信号，DI-45
        /// </summary>
        MaintainDoor5LockDetect,
        /// <summary>
        /// 维修门5按钮检知信号，DI-46
        /// </summary>
        MaintainDoor5BtnDetect,
        /// <summary>
        /// 隔离纸工位急停按钮信号，DI-47
        /// </summary>
        IsolationEmgStopBtn,
        #endregion
        #region 第5站
        /// <summary>
        /// 设备电箱温度检测信号，DI-48
        /// </summary>
        TempDetect,
        /// <summary>
        /// 4工位转盘平台1真空检知，DI-49
        /// </summary>
        TrayStage1VacDetect,
        /// <summary>
        /// 4工位转盘平台2真空检知，DI-50
        /// </summary>
        TrayStage2VacDetect,
        /// <summary>
        /// 4工位转盘平台3真空检知，DI-51
        /// </summary>
        TrayStage3VacDetect,
        /// <summary>
        /// 4工位转盘平台4真空检知，DI-52
        /// </summary>
        TrayStage4VacDetect,
        /// <summary>
        /// NG下料平台4真空检知，DI-53
        /// </summary>
        NgUnloadTrayVacDetect,
        /// <summary>
        /// 半Tray缓存第1层真空检知，DI-56
        /// </summary>
        HalfTrayBufferLayer1VacDetect,
        /// <summary>
        /// 半Tray缓存第2层真空检知，DI-57
        /// </summary>
        HalfTrayBufferLayer2VacDetect,
        /// <summary>
        /// 半Tray缓存第3层真空检知，DI-58
        /// </summary>
        HalfTrayBufferLayer3VacDetect,
        /// <summary>
        /// 半Tray缓存第4层真空检知，DI-59
        /// </summary>
        HalfTrayBufferLayer4VacDetect,
        /// <summary>
        /// 半Tray缓存第5层真空检知，DI-60
        /// </summary>
        HalfTrayBufferLayer5VacDetect,
        /// <summary>
        /// 半Tray缓存第6层真空检知，DI-61
        /// </summary>
        HalfTrayBufferLayer6VacDetect,
        /// <summary>
        /// 半Tray缓存第7层真空检知，DI-62
        /// </summary>
        HalfTrayBufferLayer7VacDetect,
        /// <summary>
        /// 半Tray缓存第8层真空检知，DI-63
        /// </summary>
        HalfTrayBufferLayer8VacDetect,
        /// <summary>
        /// 半Tray缓存第9层真空检知，DI-64
        /// </summary>
        HalfTrayBufferLayer9VacDetect,
        /// <summary>
        /// 半Tray缓存第10层真空检知，DI-65
        /// </summary>
        HalfTrayBufferLayer10VacDetect,
        /// <summary>
        /// 半Tray缓存第11层真空检知，DI-66
        /// </summary>
        HalfTrayBufferLayer11VacDetect,
        /// <summary>
        /// 半Tray缓存第12层真空检知，DI-67
        /// </summary>
        HalfTrayBufferLayer12VacDetect,
        /// <summary>
        /// 内盒工位1在籍检知1，DI-68,常闭信号
        /// </summary>
        LotBox1IsExist1,
        /// <summary>
        /// 内盒工位1在籍检知2，DI-69,常闭信号
        /// </summary>
        LotBox1IsExist2,
        /// <summary>
        /// 内盒工位2在籍检知1，DI-70,常闭信号
        /// </summary>
        LotBox2IsExist1,
        /// <summary>
        /// 内盒工位2在籍检知2，DI-71,常闭信号
        /// </summary>
        LotBox2IsExist2,
        /// <summary>
        /// 内盒工位3在籍检知1，DI-72,常闭信号
        /// </summary>
        LotBox3IsExist1,
        /// <summary>
        /// 内盒工位3在籍检知2，DI-73,常闭信号
        /// </summary>
        LotBox3IsExist2,
        /// <summary>
        /// 内盒工位4在籍检知1，DI-74,常闭信号
        /// </summary>
        LotBox4IsExist1,
        /// <summary>
        /// 内盒工位4在籍检知2，DI-75,常闭信号
        /// </summary>
        LotBox4IsExist2,
        /// <summary>
        /// 维修门6门锁检知信号，DI-80
        /// </summary>
        MaintainDoor6LockDetect,
        /// <summary>
        /// 维修门6按钮检知信号，DI-81
        /// </summary>
        MaintainDoor6BtnDetect,
        /// <summary>
        /// 入料口开始按钮信号，DI-82
        /// </summary>
        LoadPortStartBtn,
        /// <summary>
        /// 入料口暂停按钮信号，DI-83
        /// </summary>
        LoadPortPauseBtn,
        /// <summary>
        /// 入料口复位按钮信号，DI-84
        /// </summary>
        LoadPortResetBtn,
        /// <summary>
        /// 出料口急停按钮信号，DI-86
        /// </summary>
        UnloadPortEmgStopBtn,
        /// <summary>
        /// 出料口开始按钮信号，DI-87
        /// </summary>
        UnloadPortStartBtn,
        /// <summary>
        /// 出料口暂停按钮信号，DI-88
        /// </summary>
        UnloadPortPauseBtn,
        /// <summary>
        /// 出料口复位按钮信号，DI-89
        /// </summary>
        UnloadPortResetBtn,
        /// <summary>
        /// 入料口手自动旋钮手动信号，DI-90
        /// </summary>
        UnloadPortManualBtn,
        /// <summary>
        /// 入料口手自动旋钮自动信号，DI-91
        /// </summary>
        UnloadPortAutoBtn,
        #endregion
        #region 第7站
        /// <summary>
        /// 半Tray缓存第1层在籍检知，DI-96
        /// </summary>
        HalfTrayBufferLayer1Detect,
        /// <summary>
        /// 半Tray缓存第2层在籍检知，DI-97
        /// </summary>
        HalfTrayBufferLayer2Detect,
        /// <summary>
        /// 半Tray缓存第3层在籍检知，DI-98
        /// </summary>
        HalfTrayBufferLayer3Detect,
        /// <summary>
        /// 半Tray缓存第4层在籍检知，DI-99
        /// </summary>
        HalfTrayBufferLayer4Detect,
        /// <summary>
        /// 半Tray缓存第5层在籍检知，DI-100
        /// </summary>
        HalfTrayBufferLayer5Detect,
        /// <summary>
        /// 半Tray缓存第6层在籍检知，DI-101
        /// </summary>
        HalfTrayBufferLayer6Detect,
        /// <summary>
        /// 半Tray缓存第7层在籍检知，DI-102
        /// </summary>
        HalfTrayBufferLayer7Detect,
        /// <summary>
        /// 半Tray缓存第8层在籍检知，DI-103
        /// </summary>
        HalfTrayBufferLayer8Detect,
        /// <summary>
        /// 半Tray缓存第9层在籍检知，DI-104
        /// </summary>
        HalfTrayBufferLayer9Detect,
        /// <summary>
        /// 半Tray缓存第10层在籍检知，DI-105
        /// </summary>
        HalfTrayBufferLayer10Detect,
        /// <summary>
        /// 半Tray缓存第11层在籍检知，DI-106
        /// </summary>
        HalfTrayBufferLayer11Detect,
        /// <summary>
        /// 半Tray缓存第12层在籍检知，DI-107
        /// </summary>
        HalfTrayBufferLayer12Detect,
        /// <summary>
        ///  半Tray缓存分离气缸伸出到位信号，DI-120
        /// </summary>
        HalfTrayBufferSplitCylOutDetect,
        /// <summary>
        ///  半Tray缓存分离气缸缩回到位信号，DI-121
        /// </summary>
        HalfTrayBufferSplitCylOrgDetect,
        /// <summary>
        /// 半Tray缓存手臂检测Sensor，DI-122
        /// </summary>
        HalfTrayBufferArmSensor,
        /// <summary>
        ///  半Tray缓存牙插气缸伸出到位信号，DI-123
        /// </summary>
        HalfTrayBufferClawCylOutDetect,
        /// <summary>
        ///  半Tray缓存牙插气缸缩回到位信号，DI-124
        /// </summary>
        HalfTrayBufferClawCylOrgDetect,
        /// <summary>
        /// 半Tray缓存平台上下气缸伸出到位信号，DI-125
        /// </summary>
        HalfTrayBufferLiftCylOutDetect,
        /// <summary>
        /// 半Tray缓存平台上下气缸缩回到位信号，DI-126
        /// </summary>
        HalfTrayBufferLiftCylOrgDetect,
        /// <summary>
        /// Ok半Tray平台真空检知，DI-127
        /// </summary>
        OKHalfTrayVacDetect,
        /// <summary>
        /// Ng半Tray平台真空检知，DI-128
        /// </summary>
        NGHalfTrayVacDetect,
        /// <summary>
        /// 外盒工位1在籍检知1，DI-129
        /// </summary>
        ExBox1IsExist1,
        /// <summary>
        /// 外盒工位2在籍检知2，DI-130
        /// </summary>
        ExBox1IsExist2,
        /// <summary>
        /// 外盒工位1在籍检知1，DI-131
        /// </summary>
        ExBox2IsExist1,
        /// <summary>
        /// 外盒工位2在籍检知2，DI-132
        /// </summary>
        ExBox2IsExist2,
        /// <summary>
        /// 维修门1检知信号，DI-135
        /// </summary>
        MaintainDoor3Detect,
        /// <summary>
        /// 维修门1检知信号，DI-136
        /// </summary>
        MaintainDoor4Detect,
        #endregion
        #region 第8站
        /// <summary>
        /// 空Tray手臂升降气缸伸出到位信号，DI-144
        /// </summary>
        EmptyTrayArmLiftCylOutDetect,
        /// <summary>
        /// 空Tray手臂升降气缸缩回到位信号，DI-145
        /// </summary>
        EmptyTrayArmLiftCylOrgDetect,
        /// <summary>
        /// 空Tray手臂升降气缸伸出到位信号，DI-146
        /// </summary>
        EmptyTrayArmClawCylOutDetect,
        /// <summary>
        /// 空Tray手臂升降气缸缩回到位信号1，DI-147
        /// </summary>
        EmptyTrayArmClawCylOrgDetect1,
        /// <summary>
        /// 空Tray手臂升降气缸缩回到位信号2，DI-148
        /// </summary>
        EmptyTrayArmClawCylOrgDetect2,
        /// <summary>
        /// 空Tray手臂升降气缸缩回到位信号3，DI-149
        /// </summary>
        EmptyTrayArmClawCylOrgDetect3,
        /// <summary>
        /// 空Tray手臂是否有Tray，DI-150
        /// </summary>
        EmptyTrayArmIsExistTray,

        /// <summary>
        /// 满Tray手臂升降气缸伸出到位信号，DI-151
        /// </summary>
        FullTrayArmLiftCylOutDetect,
        /// <summary>
        /// 满Tray手臂升降气缸缩回到位信号，DI-152
        /// </summary>
        FullTrayArmLiftCylOrgDetect,
        /// <summary>
        /// 满Tray手臂升降气缸伸出到位信号，DI-153
        /// </summary>
        FullTrayArmClawCylOutDetect,
        /// <summary>
        /// 满Tray手臂升降气缸缩回到位信号1，DI-154
        /// </summary>
        FullTrayArmClawCylOrgDetect1,
        /// <summary>
        /// 满Tray手臂升降气缸缩回到位信号2，DI-155
        /// </summary>
        FullTrayArmClawCylOrgDetect2,
        /// <summary>
        /// 满Tray手臂升降气缸缩回到位信号3，DI-156
        /// </summary>
        FullTrayArmClawCylOrgDetect3,
        /// <summary>
        /// 满Tray手臂是否有Tray，DI-157
        /// </summary>
        FullTrayArmIsExistTray,
        /// <summary>
        /// 空Tray缓存平台是否有Tray，DI-159
        /// </summary>
        EmptyBufferIsExistTray,
        /// <summary>
        /// 8工位转盘Bin1平台在籍检知，DI-160
        /// </summary>
        Stn8Bin1StageDetect,
        /// <summary>
        /// 8工位转盘Bin2平台在籍检知，DI-161
        /// </summary>
        Stn8Bin2StageDetect,
        /// <summary>
        /// 8工位转盘Bin3平台在籍检知，DI-162
        /// </summary>
        Stn8Bin3StageDetect,
        /// <summary>
        /// 8工位转盘Bin4平台在籍检知，DI-163
        /// </summary>
        Stn8Bin4StageDetect,
        /// <summary>
        /// 8工位转盘Bin5平台在籍检知，DI-164
        /// </summary>
        Stn8Bin5StageDetect,
        /// <summary>
        /// 8工位转盘Bin6平台在籍检知，DI-165
        /// </summary>
        Stn8Bin6StageDetect,
        /// <summary>
        /// 8工位转盘取放Lot平台在籍检知，DI-166
        /// </summary>
        Stn8LulStageDetect,
        /// <summary>
        /// 8工位转盘手动投料平台在籍检知，DI-167
        /// </summary>
        Stn8ManualLoadDetect,
        /// <summary>
        /// 隔离纸料仓1中是否有隔离纸信号，DI-168
        /// </summary>
        PaperCabinet1Detect,
        /// <summary>
        /// 隔离纸料仓2中是否有隔离纸信号，DI-169
        /// </summary>
        PaperCabinet2Detect,
        /// <summary>
        /// 隔离纸旋转气缸伸出到位信号，DI-170
        /// </summary>
        PaperCabinetRotateCylOutDetect,
        /// <summary>
        /// 隔离纸旋转气缸缩回到位信号，DI-171
        /// </summary>
        PaperCabinetRotateCylOrgDetect,
        /// <summary>
        /// 隔离纸升降气缸伸出到位信号，DI-172
        /// </summary>
        PickPaperLiftCylOutDetect,
        /// <summary>
        /// 隔离纸升降气缸缩回到位信号，DI-173
        /// </summary>
        PickPaperLiftCylOrgDetect,

        /// <summary>
        /// 隔离纸搬运Arm真空检知信号，DI-174
        /// </summary>
        PaperArmVacDetect,
        /// <summary>
        /// 隔离纸搬运Arm在籍检知，DI-175
        /// </summary>
        PickPaperArmDetect,
        /// <summary>
        /// 内盒搬运手臂在籍检知1，DI-176
        /// </summary>
        LotBoxTransferDetect1,
        /// <summary>
        /// 内盒搬运手臂在籍检知2，DI-177
        /// </summary>
        LotBoxTransferDetect2,
        /// <summary>
        /// 内盒搬运手臂升降气缸伸出到位信号，DI-178
        /// </summary>
        LotBoxTransferLiftCylOutDetect,
        /// <summary>
        /// 内盒搬运手臂升降气缸缩回到位信号，DI-179
        /// </summary>
        LotBoxTransferLiftCylOrgDetect,

        /// <summary>
        /// 隔离纸升降气缸伸出到位信号，DI-180
        /// </summary>
        PickPaperTransferCylOutDetect,
        /// <summary>
        /// 隔离纸搬运气缸缩回到位信号，DI-181
        /// </summary>
        PickPaperTransferCylOrgDetect,
        #endregion

        #region 第9站
        /// <summary>
        /// 隔离纸对位平台吸真空检知信号，DI-192
        /// </summary>
        PaperStageVacDetect,
        /// <summary>
        /// 隔离纸入料Arm吸真空检知信号，DI-194
        /// </summary>
        PaperLoadArmVacDetect,
        /// <summary>
        /// Lot搬运Arm在籍检知，DI-197
        /// </summary>
        LotTransferArmDetect,
        /// <summary>
        /// Lot搬运Arm升降气缸伸出到位信号，DI-198
        /// </summary>
        LotTransferArmLiftCylOutDetect,
        /// <summary>
        /// Lot搬运Arm升降气缸缩回到位信号，DI-199
        /// </summary>
        LotTransferArmLiftCylOrgDetect,
        /// <summary>
        /// Lot搬运Arm吸标签真空检知信号，DI-200
        /// </summary>
        LotTransferArmVacDetect,
        /// <summary>
        /// 空Tray缓存平台吸真空到位检知,DI-201
        /// </summary>
        EmptyTrayBufferVacDetect,
        /// <summary>
        /// 标签出料气缸伸出到位信号，DI-205
        /// </summary>
        LabelCylOutDetect,
        /// <summary>
        /// 标签出料气缸缩回到位信号，DI-206
        /// </summary>
        LabelCylOrgDetect,
        /// <summary>
        /// 标签在籍检知信号，DI-207
        /// </summary>
        LabelExistDetect,
        /// <summary>
        /// 束带机准备完成信号，DI-208
        /// </summary>
        PackerReady,
        /// <summary>
        /// 束带机报警信号，DI-209
        /// </summary>
        PackerAlarm,
        /// <summary>
        /// 束带机忙碌信号，DI-210
        /// </summary>
        PackerBusy,
        /// <summary>
        /// 束带机完成信号，DI-211
        /// </summary>
        PackerComp,
        /// <summary>
        /// 束带机平移气缸伸出到位信号，DI-213
        /// </summary>
        PackerTransferCylOutDetect,
        /// <summary>
        /// 束带机平移气缸缩回到位信号，DI-214
        /// </summary>
        PackerTransferCylOrgDetect,

        /// <summary>
        /// 内盒工位1夹持气缸1伸出到位信号，DI-216
        /// </summary>
        LotBox1FixLotCyl1OutDetect,
        /// <summary>
        /// 内盒工位1夹持气缸1缩回到位信号，DI-217
        /// </summary>
        LotBox1FixLotCyl1OrgDetect,
        LotBox1FixLotCyl2OutDetect,
        LotBox1FixLotCyl2OrgDetect,
        /// <summary>
        /// 内盒工位2夹持气缸1伸出到位信号，DI-220
        /// </summary>
        LotBox2FixLotCyl1OutDetect,
        /// <summary>
        /// 内盒工位2夹持气缸1缩回到位信号，DI-221
        /// </summary>
        LotBox2FixLotCyl1OrgDetect,
        LotBox2FixLotCyl2OutDetect,
        LotBox2FixLotCyl2OrgDetect,
        /// <summary>
        /// 内盒工位3夹持气缸1伸出到位信号，DI-224
        /// </summary>
        LotBox3FixLotCyl1OutDetect,
        /// <summary>
        /// 内盒工位3夹持气缸1缩回到位信号，DI-225
        /// </summary>
        LotBox3FixLotCyl1OrgDetect,
        LotBox3FixLotCyl2OutDetect,
        LotBox3FixLotCyl2OrgDetect,
        /// <summary>
        /// 内盒工位4夹持气缸1伸出到位信号，DI-228
        /// </summary>
        LotBox4FixLotCyl1OutDetect,
        /// <summary>
        /// 内盒工位4夹持气缸1缩回到位信号，DI-229
        /// </summary>
        LotBox4FixLotCyl1OrgDetect,
        LotBox4FixLotCyl2OutDetect,
        LotBox4FixLotCyl2OrgDetect,
        /// <summary>
        /// 维修门5检知信号，DI-235
        /// </summary>
        MaintainDoor5Detect,
        /// <summary>
        /// 维修门6检知信号，DI-236
        /// </summary>
        MaintainDoor6Detect,
        /// <summary>
        /// 束带平台Lot检测信号，DI-237
        /// </summary>
        PackingStageLotDetect,
        #endregion

        ExBoxStn1LoadDone,
        ExBoxStn2LoadDone,
        ExBoxStn1UnloadDone,
        ExBoxStn2UnloadDone,
        ChangeTrayDone,
    }
    public enum AnalogInputName
    {
        #region 第2站
        /// <summary>
        /// 上料转塔吸嘴1真空值，AI-01
        /// </summary>
        LoadDieNozzle1Value,
        /// <summary>
        /// 上料转塔吸嘴2真空值，AI-02
        /// </summary>
        LoadDieNozzle2Value,
        /// <summary>
        /// 上料转塔吸嘴3真空值，AI-03
        /// </summary>
        LoadDieNozzle3Value,
        /// <summary>
        /// 上料转塔吸嘴4真空值，AI-04
        /// </summary>
        LoadDieNozzle4Value,
        /// <summary>
        /// 上料转塔吸嘴5真空值，AI-05
        /// </summary>
        LoadDieNozzle5Value,
        /// <summary>
        /// 上料转塔吸嘴6真空值，AI-06
        /// </summary>
        LoadDieNozzle6Value,
        /// <summary>
        /// 上料转塔吸嘴7真空值，AI-07
        /// </summary>
        LoadDieNozzle7Value,
        /// <summary>
        /// 上料转塔吸嘴8真空值，AI-08
        /// </summary>
        LoadDieNozzle8Value,
        #endregion
        #region 第3站
        /// <summary>
        /// 上料转塔吸嘴9真空值，AI-09
        /// </summary>
        LoadDieNozzle9Value,
        /// <summary>
        /// 上料转塔吸嘴10真空值，AI-10
        /// </summary>
        LoadDieNozzle10Value,
        /// <summary>
        /// 上料转塔吸嘴11真空值，AI-11
        /// </summary>
        LoadDieNozzle11Value,
        /// <summary>
        /// 上料转塔吸嘴12真空值，AI-12
        /// </summary>
        LoadDieNozzle12Value,
        /// <summary>
        /// 上料转塔吸嘴13真空值，AI-13
        /// </summary>
        LoadDieNozzle13Value,
        /// <summary>
        /// 上料转塔吸嘴14真空值，AI-14
        /// </summary>
        LoadDieNozzle14Value,
        /// <summary>
        /// 上料转塔吸嘴15真空值，AI-15
        /// </summary>
        LoadDieNozzle15Value,
        /// <summary>
        /// 上料转塔吸嘴16真空值，AI-16
        /// </summary>
        LoadDieNozzle16Value,
        #endregion
        #region 第6站
        /// <summary>
        /// 挑补转塔吸嘴1真空值，AI-17
        /// </summary>
        PickPatchNozzle1Value,
        /// <summary>
        /// 挑补转塔吸嘴2真空值，AI-18
        /// </summary>
        PickPatchNozzle2Value,

        #endregion
    }
    public enum OutputName
    {
        /// <summary>
        /// 红灯，DO-00
        /// </summary>
        RedLight,
        /// <summary>
        /// 黄灯，DO-01
        /// </summary>
        YellowLight,
        /// <summary>
        /// 绿灯，DO-02
        /// </summary>
        GreenLight,
        /// <summary>
        /// 蓝灯，DO-03
        /// </summary>
        BlueLight,
        /// <summary>
        /// 蜂蜜器，DO-04
        /// </summary>
        Buzzer,

        /// <summary>
        /// WaferTable线扫光源
        /// </summary>
        WaferTableLineScanPower,
        /// <summary>
        /// 上料转盘取Die相机光源
        /// </summary>
        LoadStnPickDiePower,
        /// <summary>
        /// 上料转盘放Die相机光源
        /// </summary>
        LoadStnPlaceDiePower,
        Stn4GrabTrayPower,
        BackDetectPower,
        IRDetectPower,
        WaferBufferVac,
        MaintainLight,
        TrayStage1VacValve,
        TrayStage2VacValve,
        TrayStage3VacValve,
        TrayStage4VacValve,
        FrontDetectPower,
        PickPatchPower1,
        PickPatchPower2,

        /// <summary>
        /// 隔离纸料仓1风刀1
        /// </summary>
        PaperCabinet1Blow1,
        PaperCabinet1Blow2,
        PaperCabinet2Blow1,
        PaperCabinet2Blow2,
        /// <summary>
        /// 隔离纸对位光源
        /// </summary>
        PaperAlignPower,
        /// <summary>
        /// 标签对位光源
        /// </summary>
        LabelAlignPower,
        /// <summary>
        /// 束带检测相机光源
        /// </summary>
        PackingDetectPower,
        /// <summary>
        /// 标签打印机锁定电磁阀
        /// </summary>
        LabelPrinterLockValve,
        /// <summary>
        /// 束带机上料信号
        /// </summary>
        PackerLoad,
        /// <summary>
        /// 束带机工作开始信号
        /// </summary>
        PackingStart,
        /// <summary>
        /// 束带机工作停止信号
        /// </summary>
        PackingStop,
        /// <summary>
        /// 束带机下料信号
        /// </summary>
        PackerUnload,






        /// <summary>
        /// 吸盘吸附到位，DO-10
        /// </summary>
        WaferBufferClawCylOut,
        /// <summary>
        /// WaferBuffer夹爪伸缩气缸伸出，DO-11
        /// </summary>
        WaferBufferStnClawCylOut,
        /// <summary>
        /// WaferBuffer夹爪伸缩气缸缩回，DO-12
        /// </summary>
        WaferBufferStnClawCylOrg,
        /// <summary>
        /// WaferBuffer夹爪吸盘吸真空，DO-13
        /// </summary>
        WaferBufferClawVacOn,
        /// <summary>
        /// WaferBuffer夹爪吸盘破真空，DO-14
        /// </summary>
        WaferBufferClawVacOff,
        /// <summary>
        /// WaferTable夹爪伸缩气缸伸出，DO-15
        /// </summary>
        WaferTableClawCylOut,

        /// <summary>
        /// 吸盘吸附到位，DO-13
        /// </summary>
        WaferBufferClawVacDetect,



        /// <summary>
        /// 请求天车上内盒到外盒工位1
        /// </summary>
        ExBoxStn1LoadReq,
        /// <summary>
        /// 请求天车上内盒到外盒工位2
        /// </summary>
        ExBoxStn2LoadReq,
        /// <summary>
        /// 请求天车从外盒工位1取内盒
        /// </summary>
        ExBoxStn1UnloadReq,
        /// <summary>
        /// 请求天车从外盒工位2取内盒
        /// </summary>
        ExBoxStn2UnloadReq,
    }
    public enum CylinderName
    {
        /// <summary>
        /// WaferTableZ轴动力气缸
        /// </summary>
        WaferTableZPowerCyl,
        /// <summary>
        /// WaferTableZ轴正负极限气缸
        /// </summary>
        WaferTableZLimitCyl,
        /// <summary>
        /// Wafer缓存平台夹爪气缸
        /// </summary>
        WaferBufferClawCyl,
        /// <summary>
        /// Wafer缓存平台伸缩气缸
        /// </summary>
        WaferBufferStretchCyl,
        /// <summary>
        /// WaferTable模组夹爪升降气缸
        /// </summary>
        WaferTableArmCyl,
        /// <summary>
        /// WaferTable模组夹爪气缸
        /// </summary>
        WaferTableClawCyl,
        /// <summary>
        /// WaferTable顶针气缸
        /// </summary>
        WaferTableThimbleCyl,
        /// <summary>
        /// 半Tray缓存平台上下气缸
        /// </summary>
        HalfTrayBufferLiftCyl,
        /// <summary>
        /// 半Tray缓存手臂取料气缸
        /// </summary>
        HalfTrayBufferClawCyl,
        /// <summary>
        /// HalfTray缓存平台分料气缸
        /// </summary>
        HalfTrayBufferSplitCyl,
        /// <summary>
        /// 空Tray手臂夹爪气缸
        /// </summary>
        EmptyTrayArmClawCyl,
        /// <summary>
        /// 满Tray手臂夹爪气缸
        /// </summary>
        FullTrayArmClawCyl,
        /// <summary>
        /// 空Tray手臂升降气缸
        /// </summary>
        EmptyTrayArmLiftCyl,
        /// <summary>
        /// 满Tray手臂升降气缸
        /// </summary>
        FullTrayArmLiftCyl,


        /// <summary>
        /// 隔离纸平移气缸
        /// </summary>
        PickPaperTransferCyl,
        /// <summary>
        /// 隔离纸旋转气缸
        /// </summary>
        PaperCabinetRotateCyl,
        /// <summary>
        /// 隔离纸升降气缸
        /// </summary>
        PickPaperLiftCyl,


        /// <summary>
        /// Lot搬运手臂升降气缸
        /// </summary>
        LotTransferArmLiftCyl,
        /// <summary>
        /// 取标签气缸
        /// </summary>
        PickLabelCyl,
        /// <summary>
        /// 打包站平移气缸，true：束带。false：取放Lot和贴标签
        /// </summary>
        PackerTransferCyl,
        /// <summary>
        /// LotBox工位1固定气缸
        /// </summary>
        LotBox1FixLotCyl,
        LotBox2FixLotCyl,
        LotBox3FixLotCyl,
        LotBox4FixLotCyl,
        /// <summary>
        /// 标签取放升降气缸
        /// </summary>
        LabelLiftCyl,
        /// <summary>
        /// 下料顶升气缸
        /// </summary>
        LotBoxTransferLiftCyl,



        None

    }
    public enum VacuumName
    {
        /// <summary>
        /// WaferBuffer吸盘
        /// </summary>
        WaferBufferClawVac,
        /// <summary>
        /// WaferTable顶针帽真空
        /// </summary>
        WaferTableThimbleCapVac,

        #region 4工位转盘真空
        TrayStage1Vac,
        TrayStage2Vac,
        TrayStage3Vac,
        TrayStage4Vac,
        #endregion

        #region 半Tray缓存真空
        HalfTrayBufferLayer1Vac,
        HalfTrayBufferLayer2Vac,
        HalfTrayBufferLayer3Vac,
        HalfTrayBufferLayer4Vac,
        HalfTrayBufferLayer5Vac,
        HalfTrayBufferLayer6Vac,
        HalfTrayBufferLayer7Vac,
        HalfTrayBufferLayer8Vac,
        HalfTrayBufferLayer9Vac,
        HalfTrayBufferLayer10Vac,
        HalfTrayBufferLayer11Vac,
        HalfTrayBufferLayer12Vac,
        #endregion

        OKHalfTrayVac,
        NGHalfTrayVac,

        /// <summary>
        /// 取隔离纸真空
        /// </summary>
        PickPaperVac,
        /// <summary>
        /// 隔离纸对位平台真空
        /// </summary>
        PaperStageVac,
        /// <summary>
        /// 取隔离纸手臂真空
        /// </summary>
        PaperArmVac,
        /// <summary>
        /// 标签气缸上的真空
        /// </summary>
        LabelCylVac,

        /// <summary>
        /// 打包手臂上的真空
        /// </summary>
        PackingArmVac,
        /// <summary>
        /// 标签对位平台上的真空
        /// </summary>
        LabelAlignStageVac,

        #region 上料转塔吸嘴真空
        LoadDieNozzle1Vac,
        LoadDieNozzle2Vac,
        LoadDieNozzle3Vac,
        LoadDieNozzle4Vac,
        LoadDieNozzle5Vac,
        LoadDieNozzle6Vac,
        LoadDieNozzle7Vac,
        LoadDieNozzle8Vac,
        LoadDieNozzle9Vac,
        LoadDieNozzle10Vac,
        LoadDieNozzle11Vac,
        LoadDieNozzle12Vac,
        LoadDieNozzle13Vac,
        LoadDieNozzle14Vac,
        LoadDieNozzle15Vac,
        LoadDieNozzle16Vac,
        #endregion

        #region 挑补转塔吸嘴真空
        PickPatchNozzle1Vac,
        PickPatchNozzle2Vac,
        PickPatchNozzle7Vac,
        PickPatchNozzle8Vac,
        PickPatchNozzle9Vac,

        PickPatchNozzle14Vac,
        PickPatchNozzle15Vac,
        PickPatchNozzle16Vac,
        #endregion
        EmptyTrayBufferVac,
    }
    public enum PosName
    {
        None,
        WaitPos,
        /// <summary>
        /// Foup1取放Wafer位置
        /// </summary>
        Foup1PickPlacePos,
        /// <summary>
        /// Foup2取放Wafer位置
        /// </summary>
        Foup2PickPlacePos,
        /// <summary>
        /// 从Foup中取Wafer时，夹爪Z轴夹取Wafer位置
        /// </summary>
        UpLayerClawWaferPos,
        /// <summary>
        /// 从Foup中取Wafer时，夹爪Z轴吸取Wafer位置
        /// </summary>
        UpLayerSuckWaferPos,
        /// <summary>
        /// 往Foup中放Wafer时，夹爪Z轴夹取Wafer位置
        /// </summary>
        DownLayerClawWaferPos,
        /// <summary>
        /// 往Foup中放Wafer时，夹爪Z轴吸Wafer位置
        /// </summary>
        DownLayerSuckWaferPos,
        /// <summary>
        /// 从Foup中取放Wafer时，夹爪X轴释放Wafer位置1
        /// </summary>
        ClawXReleaseWaferPos1,
        /// <summary>
        /// 从Foup中取放Wafer时，夹爪X轴释放Wafer位置2
        /// </summary>
        ClawXReleaseWaferPos2,
        /// <summary>
        /// 从Foup中取放Wafer时，夹爪X轴释放Wafer位置3
        /// </summary>
        ClawXReleaseWaferPos3,
        /// <summary>
        /// 从Foup中取Wafer时，夹爪X轴吸取Wafer位置1
        /// </summary>
        ClawXSuckWaferPos1,
        /// <summary>
        /// 从Foup中取Wafer时，夹爪X轴吸取Wafer位置2
        /// </summary>
        ClawXSuckWaferPos2,

        /// <summary>
        /// 夹爪X轴在Foup中夹取Wafer位置
        /// </summary>
        ClampWaferFromFoup,

        /// <summary>
        /// Wafer从缓存平台到WaferTable位置
        /// </summary>
        WaferFromBufferToTablePos,
        /// <summary>
        /// Wafer从WaferTable到缓存平台位置
        /// </summary>
        WaferFromTableToBufferPos,
        MarkDie1Pos,
        MarkDie2Pos,
        ScancodePos,
        /// <summary>
        /// WaferTable模组取放Wafer位置
        /// </summary>
        LoadWaferPos,
        /// <summary>
        /// WaferTable模组Arm从缓存平台取放Wafer位置
        /// </summary>
        ArmPickWaferFromBufferPos,
        /// <summary>
        /// WaferTable模组Arm取放Wafer到WaferTable位置
        /// </summary>
        ArmPickWaferToTablePos,
        /// <summary>
        /// WaferTableZ扩摸位置
        /// </summary>
        LiftPos,
        /// <summary>
        /// 左上角Die拍照位置
        /// </summary>
        LeftTopDiePos,
        /// <summary>
        /// 右上角Die拍照位置
        /// </summary>
        RightTopDiePos,
        /// <summary>
        /// 左下角Die拍照位置
        /// </summary>
        LeftBottomDiePos,
        /// <summary>
        /// 右下角Die拍照位置
        /// </summary>
        RightBottomDiePos,
        /// <summary>
        /// 取Die位置
        /// </summary>
        PickDiePos,
        /// <summary>
        /// 放Die位置
        /// </summary>
        PlaceDiePos,

        /// <summary>
        /// 飞拍起始位置
        /// </summary>
        GrabStartPos,
        /// <summary>
        /// 飞拍结束位置
        /// </summary>
        GrabEndPos,
        /// <summary>
        /// 检测位置
        /// </summary>
        DetectPos,
        /// <summary>
        /// Tray拍照位置
        /// </summary>
        GrabTrayPos,
        /// <summary>
        /// HalfTray工位OkTray拍照位置
        /// </summary>
        GrabOkTrayPos,
        /// <summary>
        /// HalfTray工位NgTray拍照位置
        /// </summary>
        GrabNgTrayPos,
        /// <summary>
        /// Die拍照位置
        /// </summary>
        GrabDiePos,

        /// <summary>
        /// 正面检测拍照位置
        ///  </summary>
        FrontDetectPos,

        /// <summary>
        /// 4工位转盘上下Tray位置
        /// </summary>
        LuloadTrayPos,
        /// <summary>
        /// HalfTray平台OkTray上下位置
        /// </summary>
        OkTrayLuloadPos,
        /// <summary>
        /// HalfTray平台NgTray上下位置
        /// </summary>
        NgTrayLuloadPos,
        /// <summary>
        /// 缓存手臂在TrayBuffer中取放Tray位置
        /// </summary>
        BufferArmPPTrayFromBufferPos,
        /// <summary>
        /// 缓存手臂从挑补平台取放Tray位置
        /// </summary>
        BufferArmPPTrayFromHalfTrayStagePos,
        /// <summary>
        /// TrayBuffer第一层位置
        /// </summary>
        FirstLayerPos,
        /// <summary>
        /// 空TrayArm从8工位转盘取放Tray位置
        /// </summary>
        EmptyArmPPTrayFrom8StnDDPos,
        /// <summary>
        /// 满TrayArm从8工位转盘取放Tray位置
        /// </summary>
        FullArmPPTrayFrom8StnDDPos,
        /// <summary>
        /// 满TrayArm从4工位转盘取Tray位置
        /// </summary>
        FullArmPPTrayFrom4StnDDPos,
        /// <summary>
        /// 空TrayArm从4工位转盘取放Tray位置
        /// </summary>
        EmptyArmPPTrayFrom4StnDDPos,

        /// <summary>
        /// 满TrayArm在挑补平台取放Tray位置
        /// </summary>
        FullArmPPTrayFromHalfTrayStagePos,
        /// <summary>
        /// 满TrayArm从8工位转盘取Tray位置
        /// </summary>
        //FullArmPickTrayFrom8StnDDPos,



        /// <summary>
        /// 从空Tray缓存平台取放Tray位置
        /// </summary>
        PPTrayFromEmptyTrayBufferPos,
        /// <summary>
        /// 从隔离纸对位平台取放隔离纸位置
        /// </summary>
        PickPaperPos,

        /// <summary>
        /// 隔离纸测高位置
        /// </summary>
        PaperMeasureHeightPos,
        /// <summary>
        /// 隔离纸拍照位置
        /// </summary>
        PaperGrabPos,
        /// <summary>
        /// LotTray拍照位置
        /// </summary>
        LotTrayGrabPos,
        /// <summary>
        /// Lot夹爪从打包平台取放Lot位置
        /// </summary>
        LotClawPPLotFromPackingStagePos,
        /// <summary>
        /// Lot夹爪抓取Lot位置
        /// </summary>
        ClawLotPos,
        /// <summary>
        /// 检测束带位置
        /// </summary>
        DetectPackingBeltPos,
        /// <summary>
        /// 从LotBox中取放Lot待机位置
        /// </summary>
        PPLotFromLotBoxWaitPos,
        /// <summary>
        /// 从标签平台取放标签位置
        /// </summary>
        PickLabelFromStagePos,
        /// <summary>
        /// Z轴标签预对位位置
        /// </summary>
        LabelPreAlignPos,
        /// <summary>
        /// 标签Z轴把标签放在对位平台位置
        /// </summary>
        PutLabelToAlignStagePos,
        /// <summary>
        /// 标签对位位置
        /// </summary>
        GrabLabelPos,
        /// <summary>
        /// 贴标签位置
        /// </summary>
        PasteLabelPos,
        /// <summary>
        /// 束带检测位置
        /// </summary>
        PackingBeltDetectPos,
        /// <summary>
        /// Lot吸盘取放Lot位置
        /// </summary>
        LotPickerPPLotFromPackingStagePos,
        /// <summary>
        /// LotArm从8工位转盘取放Lot位置
        /// </summary>
        PPLotFrom8StnTurnTablePos,

        /// <summary>
        /// LotBoxStn1放Lot位置
        /// </summary>
        LotBoxStn1PPLotBoxFromInBoxPos,
        LotBoxStn2PPLotBoxFromInBoxPos,
        LotBoxStn3PPLotBoxFromInBoxPos,
        LotBoxStn4PPLotBoxFromInBoxPos,
        /// <summary>
        /// LotBoxStn1 Lot读码位置
        /// </summary>
        LotBoxStn1ScanPos,
        LotBoxStn2ScanPos,
        LotBoxStn3ScanPos,
        LotBoxStn4ScanPos,
        ExBoxStn1PPLotBoxFromExBoxPos,
        ExBoxStn2PPLotBoxFromExBoxPos,
        ExBoxStn1PrePPLotBoxFromExBoxPos,
        ExBoxStn2PrePPLotBoxFromExBoxPos,
        ExBoxStn1ReadRfidPos,
        ExBoxStn2ReadRfidPos,
        //LotBoxStn1LulPos,
        //LotBoxStn2LulPos,
        /// <summary>
        /// LotArm放Lot到内盒工位1起始位置
        /// </summary>
        PPTrayFromLotBoxStn1StartPos,
        /// <summary>
        /// LotArm从内盒工位2取Lot起始位置
        /// </summary>
        PPTrayFromLotBoxStn2StartPos,
        /// <summary>
        /// LotArm放Lot到内盒工位2起始位置
        /// </summary>
        PutTrayToLotBoxStn2StartPos,
        /// <summary>
        /// 隔离纸Z轴拍Tray基准高度
        /// </summary>
        GrabLotBaseHeight,
        /// <summary>
        /// 隔离纸Z轴放隔离纸基准高度
        /// </summary>
        PlacePaperToLotBaseHeight,
        /// <summary>
        /// 8工位转盘Z轴在转盘上取放Tray基准高度
        /// </summary>
        PPTrayBaseHeight,
        /// <summary>
        /// LotArm放Lot到内盒基准高度
        /// </summary>
        PutFullLotToLotBoxBaseHeight,
        /// <summary>
        /// 从LotBox中取Lot基准高度
        /// </summary>
        PickTrayFromLotBoxBaseHeight,
        /// <summary>
        /// Wafer缓存夹爪X轴判断Wafer属于哪一层位置
        /// </summary>
        JudgeLayerPos,
        /// <summary>
        /// WaferTableZ轴动力气缸伸出位置
        /// </summary>
        WaferTableZCylOutPos,


        /// <summary>
        /// 半Tray平台避让位置，防止碰撞
        /// </summary>
        HalfTrayStageSafePos,
        /// <summary>
        /// 挑补平台大Y轴取放Tray位置
        /// </summary>
        HalfTrayYLulTrayPos,
        /// <summary>
        /// 挑补平台大Y轴拍Tray位置
        /// </summary>
        HalfTrayYGrabTrayPos,
        /// <summary>
        /// IRNgTray取放Tray位置
        /// </summary>
        IRNgTrayLunLoadPos,
        /// <summary>
        /// IRNgTray第一个槽位位置
        /// </summary>
        IRNgTrayFirstSlotPos,
        /// <summary>
        /// 放隔离纸到叠Tray位置
        /// </summary>
        PutPaperToTrayPos,
        /// <summary>
        /// 标签检测位置
        /// </summary>
        LabelDetectPos,
    }
}
