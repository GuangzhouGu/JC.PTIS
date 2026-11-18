/*----------------------------------------------------------------
//公司名：   Suzhou JingCe Technology Co.,LTD
//版权申明： Copyright © 2025 JingCe. All Rights Reserved.
//
// =================================
// CLR版本     ：4.6.81.0
// 命名空间    ：
// 文件名称    ：
// =================================
// 创 建 者    ：guguangzhou
// 创建日期    ：2025/01/01 09:00:00 
// 功能描述    ：
// 使用说明    ：
//
//
// 创建标识：
//
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
using JC.PTIS.Main.Devices.Vision;
using JC.PTIS.Main.SubForms;
using JC.PTIS.Main.SubViews;
using StdLib.PCBaseFrame.Extension.Para;
using StdLib.PCBaseFrame.Extension.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Inovance.InoMotionCotrollerShop.InoServiceContract.EtherCATConfigApi.ImcApi;

namespace JC.PTIS.Main.Params
{
    public class ProdPara :  IPara
    {
        #region 接口实现
        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public string ParaName { get; set; } = "机种参数";
        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public bool IsShow { get; set; } = true;
        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public ParaViewBase ParaViewBase { get; set; } = FrmSizeEdit.Instance;

        [Browsable(false)]
        [System.Xml.Serialization.XmlIgnore]
        public bool IsGlobal { get; set; }
        #endregion

        #region Foup尺寸参数
        [Category("Foup尺寸参数"), DisplayName("Foup每层间距"), Description("单位：mm")]
        public double FoupPitch { get; set; } = 20;
        [Category("Foup尺寸参数"), DisplayName("Foup层数"), Description("单位：层")]
        public int FoupSlotCount { get; set; } = 13;

        #endregion

        #region Tray尺寸参数
        [Category("Tray尺寸参数"), DisplayName("Tray行数"), Description("单位：行")]
        public int TrayRowCount { get; set; } = 2;
        [Category("Tray尺寸参数"), DisplayName("Tray列数"), Description("单位：列")]
        public int TrayColCount { get; set; } = 30;
        [Category("Tray尺寸参数"), DisplayName("Tray宽度"), Description("单位：毫米")]
        public double TrayWidth { get; set; } = 100.0;
        [Category("Tray尺寸参数"), DisplayName("Tray高度"), Description("单位：毫米")]
        public double TrayHeight { get; set; } = 100.0;
        [Category("Tray尺寸参数"), DisplayName("Tray厚度"), Description("单位：毫米")]
        public double TrayThickness { get; set; } = 2.5;
        [Category("Tray尺寸参数"), DisplayName("X方向间距"), Description("单位：毫米")]
        public double CellPitchX { get; set; } = 18.0;
        [Category("Tray尺寸参数"), DisplayName("Y方向间距"), Description("单位：毫米")]
        public double CellPitchY { get; set; } = 2.5;
        [Category("Tray尺寸参数"), DisplayName("首个穴位相对于拍照基准位X方向偏移量"), Description("单位：毫米")]
        public double FirstCellOffsetX { get; set; } = 0;
        [Category("Tray尺寸参数"), DisplayName("首个穴位相对于拍照基准位Y方向偏移量"), Description("单位：毫米")]
        public double FirstCellOffsetY { get; set; } = 0;
        [Category("Tray尺寸参数"), DisplayName("首个测高位置对于拍照基准位X方向偏移量"), Description("单位：毫米")]
        public double FirstMeasureHeightPointOffsetX { get; set; } = 10;
        [Category("Tray尺寸参数"), DisplayName("首个测高位置对于拍照基准位Y方向偏移量"), Description("单位：毫米")]
        public double FirstMeasureHeightPointOffsetY { get; set; } = 10;
        [Category("Tray尺寸参数"), DisplayName("测高点之间的偏移量"), Description("单位：毫米")]
        public double MeasureHeightOffset { get; set; } = 10;

        #endregion

        #region LotBox参数
        [Category("LotBox参数"), DisplayName("LotBox行数"), Description("单位：行")]
        public int RowCountPerLotBox { get; set; } = 4;
        [Category("LotBox参数"), DisplayName("LotBox列数"), Description("单位：列")]
        public int ColCountPerLotBox { get; set; } = 4;
        [Category("LotBox参数"), DisplayName("LotBox宽度"), Description("单位：毫米")]
        public double LotBoxWidth { get; set; } = 200;
        [Category("LotBox参数"), DisplayName("LotBox高度"), Description("单位：毫米")]
        public double LotBoxHeight { get; set; } = 200;
        [Category("LotBox参数"), DisplayName("每个空Lot里Tray的数量"), Description("单位：个")]
        public int TrayCountPerEmptyLot { get; set; } = 18;
        [Category("LotBox参数"), DisplayName("每个满LotCell里Lot的数量"), Description("单位：个")]
        public int LotCountPerLotCell { get; set; } = 6;
        [Category("LotBox参数"), DisplayName("每次从Lot盒中取出的Tray数量"), Description("单位：个")]
        public int PickTrayCountFromLotOnce { get; set; } = 3;
        [Category("LotBox参数"), DisplayName("内盒中每个穴位的X方向间距"), Description("单位：毫米")]
        public double LotCellPitchX { get; set; } = 30;
        [Category("LotBox参数"), DisplayName("内盒中每个穴位的Y方向间距"), Description("单位：毫米")]
        public double LotCellPitchY { get; set; } = 30;

        [Category("LotBox参数"), DisplayName("左上角首个穴位X方向偏移量"), Description("单位：毫米")]
        public double FirstLotCellOffsetX { get; set; } = 30;
        [Category("LotBox参数"), DisplayName("左上角首个穴位Y方向偏移量"), Description("单位：毫米")]
        public double FirstLotCellOffsetY { get; set; } = 30;
        #endregion

        #region Wafer参数
        [Category("Wafer参数"), DisplayName("基准Die行号"), Description("单位：列")]
        public int BaseDieRowNo { get; set; } = 3;
        [Category("Wafer参数"), DisplayName("基准Die列号"), Description("单位：行")]
        public int BaseDieColNo { get; set; } = 0;
        [Category("Wafer参数"), DisplayName("Wafer总行数"), Description("单位：列")]
        public int WaferRowCount { get; set; } = 8;
        [Category("Wafer参数"), DisplayName("Wafer总列数"), Description("单位：行")]
        public int WaferColCount { get; set; } = 298;
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("Wafer参数"), DisplayName("首颗Die的实际绝对坐标"), Description("单位：毫米")]
        public Xyt FirstDieCrd { get; set; } = new Xyt();
        #endregion

        #region TrayBuffer参数
        [Category("TrayBuffer参数"), DisplayName("Buffer每层的间距"), Description("单位：毫米")]
        public double TrayBufferPitch { get; set; } = 20.0;
        [Category("TrayBuffer参数"), DisplayName("Buffer每层的Offset"), Description("单位：毫米")]
        public double[] OffsetArr { get; set; } = new double[12];
        #endregion

        #region Lot参数
        [Category("Lot参数"), DisplayName("8工位转盘每个工位Tray盘数量"), Description("单位：个")]
        public int TrayCountPerStn8 { get; set; } = 6; 
        #endregion

        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("测试参数"), DisplayName("测试"), Description("单位：mm/s")]
        public TestPara Test { get; set; } = new TestPara() { MyProperty = 123, MyProperty1 = "Test" };
    }
    public class TestPara
    {
        public int MyProperty { get; set; }
        public string MyProperty1 { get; set; }
    }
}
