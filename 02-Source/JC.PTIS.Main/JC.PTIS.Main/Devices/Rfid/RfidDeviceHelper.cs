using StdLib.HTFile;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.PCBaseFrame.Extension.Device;
using StdLib.WindowsControls;
using System;
using System.Collections.Generic;

namespace JC.PTIS.LoadPort.Devices.EFEM
{
    public class RfidDeviceHelper : IDevice
    {
        private string FilePath = AppDomain.CurrentDomain.BaseDirectory + @"\Config\Hardware\RfidDeviceInfo.xml";

        public virtual string DeviceName { get; set; } = nameof(RfidDeviceHelper);
        public static RfidDeviceHelper Instance { get { return DeviceManager.Instance.Get(nameof(RfidDeviceHelper)) as RfidDeviceHelper; } set { } }

        public List<RfidDevice> List = new List<RfidDevice>();

        public int Count { get { return List.Count; } }

        public event Action<ConnectedStsArgs> OnConnected;
        public event Action<MessageArgs> OnShowMsg;

        public event Action<MessageArgs> ShowDebugMessage;

        #region 公开接口

        public void Init()
        {
            var infos = GetInfo();
            if (infos != null && infos.Count > 0)
            {
                foreach (RfidDevicetInfo item in infos)
                {
                    try
                    {
                        RfidDevice rfidDevice = new RfidDevice(item);
                        rfidDevice.OnConnected += OnConnected;
                        rfidDevice.OnShowMsg += this.RfidDevice_OnShowMsg;
                        List.Add(rfidDevice);
                        rfidDevice.Connect();
                    }
                    catch (Exception ex)
                    {
                        OnShowMsg?.Invoke(new MessageArgs { LogName = "RfidDevice", Message = string.Format("{0}->Connect异常, {1}", item.DeviceName, ex), MsgType = MsgType.Error });
                    }
                }
            }
            else
            {
                OnShowMsg?.Invoke(new MessageArgs { LogName = "RfidDevice", Message = $"RfidDeviceHelper->读取配置文件失败，文件为空", MsgType = MsgType.Normal });
            }
        }

        public void InitRet(bool ret)
        {
        }

        public void Uninit()
        {
            foreach (var item in List)
            {
                item.Disconnect();
            }
        }

        public RfidDevice GetRfidDevice(int num)
        {
            RfidDevice ret = null;
            if (List != null && List.Count > 0)
            {
                var index = List.FindIndex(p => p.Info.Num == num);
                if (index >= 0)
                {
                    ret = List[index];
                }
            }
            return ret;
        }

        #endregion

        #region 实例方法

        private List<RfidDevicetInfo> GetInfo()
        {
            var result = HTXmlHelper.EntityFromXMLFile<List<RfidDevicetInfo>>(FilePath);
            return result;
        }

        #endregion

        private void RfidDevice_OnShowMsg(MessageArgs msg)
        {
            this.ShowDebugMessage?.Invoke(msg);
        }
    }
}
