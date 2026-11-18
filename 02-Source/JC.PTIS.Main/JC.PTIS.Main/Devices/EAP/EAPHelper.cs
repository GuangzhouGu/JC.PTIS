using JC.PTIS.Main.Devices.Vision;
using JC.PTIS.Main.Models;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.PCBaseFrame.Extension.Device;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Devices.EFEM
{
    public class EAPHelper : IDevice
    {
        public string DeviceName { get; set; } = "EAP";

        public event Action<ConnectedStsArgs> OnConnected;
        public event Action<MessageArgs> OnShowMsg;
        List<WaferModel>[] WaferList = null;
        public static EAPHelper Instance { get { return DeviceManager.Instance.Get("EAP") as EAPHelper; } }
        public void Init()
        {

        }
     
        public void Uninit()
        {
        
        }
    }
}
