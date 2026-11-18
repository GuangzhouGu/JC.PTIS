using StdLib.PCBaseFrame.Extension.Para;
using StdLib.PCBaseFrame.Extension.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Params
{
    public class GlobalPara:IPara
    {
        #region 接口实现
        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public string ParaName { get; set; } = "全局参数";
        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public bool IsShow { get; set; } = true;
        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public ParaViewBase ParaViewBase { get; set; }
        [Browsable(false)]
        [System.Xml.Serialization.XmlIgnore]
        public bool IsGlobal { get; set; } = true;

        #endregion
        #region 硬件启用参数
        [Category("硬件启用"), DisplayName("Rfid启用")]
        public bool IsRfidEnable { get; set; } = true;
        [Category("硬件启用"), DisplayName("测距仪启用")]
        public bool IsLaserEnable { get; set; } = true;
        [Category("硬件启用"), DisplayName("相机启用")]
        public bool IsCameraEnable { get; set; } = true;
        [Category("硬件启用"), DisplayName("扫码枪启用")]
        public bool IsScanerEnable { get; set; } = true;
        [Category("硬件启用"), DisplayName("对位PC启用")]
        public bool IsAlignPCEnable { get; set; } = true;
        [Category("硬件启用"), DisplayName("检测PC启用")]
        public bool IsDetectPCEnable { get; set; } = true;
        #endregion

        [Category("测高参数"), DisplayName("测高失败重试次数")]
        public int MeasureHeightCount { get; set; } = 3;
        [Category("测高参数"), DisplayName("Tray盘高度最低阈值")]
        public double TrayHeightMin { get; set; } = 9.5;
        [Category("测高参数"), DisplayName("Tray盘高度最高阈值")]
        public double TrayHeightMax { get; set; } = 10.5;
        [Category("隔离纸参数"), DisplayName("隔离纸厚度"), Description("单位：毫米")]
        public double PaperThickness { get; set; } = 0.2;
        [Category("隔离纸参数"), DisplayName("隔离纸厚度阈值"), Description("单位：毫米")]
        public double PaperThicknessThreshold { get; set; } = 0.2;
        [Category("隔离纸参数"), DisplayName("隔离纸高度基准值"), Description("单位：毫米")]
        public double PaperHeightBaseValue { get; set; } = 10.0;

        [Category("天车参数"), DisplayName("天车上下料超时时间"), Description("单位：秒")]
        public double OHTLulTimeout { get; set; } = 60.0;
    }
}
