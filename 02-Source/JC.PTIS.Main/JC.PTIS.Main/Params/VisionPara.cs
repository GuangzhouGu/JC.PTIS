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
    public class VisionPara:IPara
    {
        #region 接口实现
        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public string ParaName { get; set; } = "视觉参数";
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


        [Category("对位参数"), DisplayName("对位失败重试次数")]
        public int RetrayCount { get; set; } = 3;
        [Category("对位参数"), DisplayName("正面检测超时时间")]
        public double FrontDetectTimeout { get; set; } = 11.0;
        [Category("对位参数"), DisplayName("X方向对位补正值范围")]
        public double[] AlignXRange { get; set; } = new double[] { -10.0,10.0 };
        [Category("对位参数"), DisplayName("Y方向对位补正值范围")]
        public double[] AlignYRange { get; set; } = new double[] { -10.0, 10.0 };
        [Category("对位参数"), DisplayName("T方向对位补正值范围")]
        public double[] AlignTRange { get; set; } = new double[] { -5.0, 5.0 };
        [Category("对位参数"), DisplayName("Die进Tray盘前是否对位")]
        public bool IsAlignBeforeDieToTray { get; set; } = true;
    }
}
