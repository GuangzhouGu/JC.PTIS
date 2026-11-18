using System.ComponentModel;
using System.IO.Ports;
using System.Xml.Serialization;

namespace JC.PTIS.LoadPort.Devices.EFEM
{
    [XmlRoot("LoadPortInfo")]
    public class LoadPortInfo
    {
        [XmlElement("Num")]
        public int Num { get; set; }

        /// <summary>
        /// COM口
        /// </summary>
        [XmlElement("portName")]
        [Category("RFID参数"), Description("COM口")]
        public string COM { get; set; }

        [XmlElement("baudRate")]
        [Category("RFID参数"), Description("波特率")]
        public int BaudRate { get; set; } = 9600;

        [XmlElement("parity")]
        [Category("RFID参数"), Description("奇偶校验")]
        public Parity Parity { get; set; } = Parity.Even;

        [XmlElement("readTimeout")]
        [Category("RFID参数"), Description("读取超时时间")]
        public int ReadTimeout { get; set; } = 2000;

        [XmlElement("ipAddress")]
        [Category("连接参数"), Description("IP地址")]
        public string IPAddress { get; set; }

        [XmlElement("port")]
        [Category("连接参数"), Description("端口号")]
        public int Port { get; set; }

        [XmlElement("DeviceName")]
        [Category("日志"), Description("日志名字"), Browsable(false)]
        public string DeviceName { get; set; }

        [XmlElement("L_REQ")]
        [Category("E84端口映射"), Description("L_REQ端口号")]
        public int L_REQ { get; set; }

        [XmlElement("U_REQ")]
        [Category("E84端口映射"), Description("U_REQ端口号")]
        public int U_REQ { get; set; }

        [XmlElement("READY")]
        [Category("E84端口映射"), Description("READY端口号")]
        public int READY { get; set; }

        [XmlElement("VALID")]
        [Category("E84端口映射"), Description("VALID端口号")]
        public int VALID { get; set; }

        [XmlElement("VALID_TIMEOUT")]
        [Category("E84信号超时时间"), Description("VALID超时时间")]
        public int VALID_TIMEOUT { get; set; } = 2000;

        [XmlElement("CS_0")]
        [Category("E84端口映射"), Description("CS_0端口号")]
        public int CS_0 { get; set; }

        [XmlElement("CS_0_TIMEOUT")]
        [Category("E84信号超时时间"), Description("CS_0超时时间")]
        public int CS_0_TIMEOUT { get; set; } = 5000;

        [XmlElement("CS_1")]
        [Category("E84端口映射"), Description("CS_1端口号")]
        public int CS_1 { get; set; }

        [XmlElement("CS_1_TIMEOUT")]
        [Category("E84信号超时时间"), Description("CS_1超时时间")]
        public int CS_1_TIMEOUT { get; set; } = 5000;

        [XmlElement("TR_REQ")]
        [Category("E84端口映射"), Description("TR_REQ端口号")]
        public int TR_REQ { get; set; }

        [XmlElement("TR_REQ_TIMEOUT")]
        [Category("E84信号超时时间"), Description("TR_REQ超时时间")]
        public int TR_REQ_TIMEOUT { get; set; } = 5000;

        [XmlElement("BUSY")]
        [Category("E84端口映射"), Description("BUSY端口号")]
        public int BUSY { get; set; }

        [XmlElement("BUSY_TIMEOUT")]
        [Category("E84信号超时时间"), Description("BUSY超时时间")]
        public int BUSY_TIMEOUT { get; set; } = 5000;

        [XmlElement("COMPT")]
        [Category("E84端口映射"), Description("COMPT端口号")]
        public int COMPT { get; set; }

        [XmlElement("COMPT_TIMEOUT")]
        [Category("E84信号超时时间"), Description("COMPT超时时间")]
        public int COMPT_TIMEOUT { get; set; } = 5000;

        /// <summary>
        /// 取放Foup超时时间
        /// </summary>
        [XmlElement("FOUP_TIMEOUT")]
        public int FOUP_TIMEOUT { get; set; } = 20000;

        public LoadPortInfo()
        {
            this.IPAddress = "192.168.1.51";
            this.Port = 945;
            this.DeviceName = nameof(LoadPortDevice);
        }
    }
}
