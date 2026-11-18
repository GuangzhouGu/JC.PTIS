using System.IO.Ports;
using System.Xml.Serialization;

namespace JC.PTIS.LoadPort.Devices.EFEM
{
    [XmlRoot("RfidDeviceInfo")]
    public class RfidDevicetInfo
    {
        [XmlElement("Num")]
        public int Num { get; set; }

        /// <summary>
        /// 如果是1比N协议，指定节点编号
        /// </summary>
        [XmlElement("nodeNumber")]
        public int NodeNumber { get; set; }

        /// <summary>
        /// COM口
        /// </summary>
        [XmlElement("portName")]
        public string PortName { get; set; }

        [XmlElement("baudRate")]
        public int BaudRate { get; set; } = 9600;

        [XmlElement("readTimeout")]
        public int ReadTimeout { get; set; } = 5000;

        [XmlElement("deviceName")]
        public string DeviceName { get; set; }

        /// <summary>
        /// 是否是1比1协议
        /// </summary>
        [XmlElement("Is1to1Protocol")]
        public bool Is1to1Protocol { get; set; } = false;

        public RfidDevicetInfo()
        {
        }
    }
}
