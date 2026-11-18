using JC.PTIS.Main.Devices.Scaner;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.PCBaseFrame.Extension.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Devices.Printer
{
    public class PrinterHelper : IDevice
    {
        public string DeviceName { get; set; } = "Printer";

        public event Action<ConnectedStsArgs> OnConnected;
        public event Action<MessageArgs> OnShowMsg;
        public static PrinterHelper Instance { get { return DeviceManager.Instance.Get("Printer") as PrinterHelper; } set { } }
        public bool Print()
        {
            return true;
        }
        public void Init()
        {
        }

        public void Uninit()
        {
        }
    }
}
