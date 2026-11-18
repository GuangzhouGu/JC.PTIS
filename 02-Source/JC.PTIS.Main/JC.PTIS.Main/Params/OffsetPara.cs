using JC.PTIS.Main.Devices.Vision;
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
    public class OffsetPara : IPara
    {
        #region 接口实现
        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public string ParaName { get; set; } = "偏移值";
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

        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("上料转塔参数"), DisplayName("上料转塔吸嘴和对位相机相对距离"), Description("单位：毫米")]
        public Xyt LoadDieNozzleAndCameraOffset { get; set; } = new Xyt { X = 0, Y = 0 };
        [Category("上料转塔参数"), DisplayName("取Die吸嘴高度偏移")]
        public double[] LoadDieStnPickDieZOffset { get; set; } = new double[16];
        [Category("上料转塔参数"), DisplayName("放Die吸嘴高度偏移")]
        public double[] LoadDieStnPlaceDieZOffset { get; set; } = new double[16];

        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("挑补转塔参数"), DisplayName("挑补转塔吸嘴和对位相机相对距离"), Description("单位：毫米")]
        public Xyt PickPatchNozzleAndCameraOffset { get; set; } = new Xyt { X = 0, Y = 0 };
        [Category("挑补转塔参数"), DisplayName("取Die吸嘴高度偏移")]
        public double[] PickPatchStnPickDieZOffset { get; set; } = new double[16];
        [Category("挑补转塔参数"), DisplayName("放Die吸嘴高度偏移")]
        public double[] PickPatchStnPlaceDieZOffset { get; set; } = new double[16];

        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("隔离纸工位偏移参数"), DisplayName("隔离纸平台和Lot平台相对距离")]
        public Xyt PaperStageAndLotOffset { get; set; } = new Xyt { X = 10, Y = 10 };
        [TypeConverter(typeof(ExpandableObjectConverter))]
        [Category("贴标签参数"), DisplayName("标签吸嘴和Lot平台相对距离"), Description("单位：毫米")]
        public Xyt PastePabelOffset { get; set; } = new Xyt { X = 20, Y = 20 };

    }
}
