using JC.AutoMating.SubForms;
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
    public class ConfigPara :  IPara
    {
        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public bool IsShow { get; set; } = true;
        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public string ParaName { get; set; } = "其他参数";
        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public ParaViewBase ParaViewBase { get; set; } = FrmInfoConfig.Instance;
        [System.Xml.Serialization.XmlIgnore]
        [Browsable(false)]
        public bool IsGlobal { get; set; }
      
    }
}
