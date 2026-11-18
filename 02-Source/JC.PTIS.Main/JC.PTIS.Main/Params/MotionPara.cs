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
    public class MotionPara : IPara
    {
        #region 接口实现
        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public string ParaName { get; set; } = "运动参数";
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
        [Category("真空参数"), DisplayName("真空阈值")]
        public double VacuumThreshold { get; set; } = 1000.0;
        [Category("束带机触发参数"), DisplayName("束带机信号触发时间，单位：秒")]
        public double TriggerTime { get; set; } = 1.0;

        [Category("轴安全位参数"), DisplayName("LotBoxX轴实际位置大于该参数时为安全距离"), Description("LotBoxX轴实际位置大于该参数时为安全距离,用于判断LotBoxX轴运动轴LotBoxY轴是否需要回到WaitPos位置")]
        public double LotBoxXSafePos { get; set; } = 30;
        [Category("轴安全位参数"), DisplayName("LotBoxY轴实际位置大于该参数时为安全距离"), Description("LotBoxY轴实际位置大于该参数时为安全距离,用于判断LotBoxX轴运动轴LotBoxY轴是否需要回到WaitPos位置")]
        public double LotBoxYSafePos { get; set; } = 30;
        [Category("超时时间参数"), DisplayName("轴运动超时时间，单位 s"), Description("轴运动时间超过该设定值时，超时报警")]
        public int WaitAxisMoveTimeOut { get; set; } = 30;
    }
}
