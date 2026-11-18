using StdLib.HTFile;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.PCBaseFrame.Extension.Device;
using System;
using System.Collections.Generic;

namespace JC.PTIS.LoadPort.Devices.EFEM
{
    public class LoadPortHelper : IDevice
    {
        private string FilePath = AppDomain.CurrentDomain.BaseDirectory + @"\Config\Hardware\LoadPortInfo.xml";

        public virtual string DeviceName { get; set; } = nameof(LoadPortHelper);
        public static LoadPortHelper Instance { get { return DeviceManager.Instance.Get(nameof(LoadPortHelper)) as LoadPortHelper; } set { } }

        public List<LoadPortDevice> List = new List<LoadPortDevice>();

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
                foreach (LoadPortInfo item in infos)
                {
                    try
                    {
                        LoadPortDevice lpDevice = new LoadPortDevice(item);
                        lpDevice.OnConnected += OnConnected;
                        List.Add(lpDevice);
                        lpDevice.Connect();
                    }
                    catch (Exception ex)
                    {
                        OnShowMsg?.Invoke(new MessageArgs { LogName = "LoadPort", Message = string.Format("{0}->Connect异常, {1}", item.DeviceName, ex), MsgType = MsgType.Error });
                    }
                }
            }
            else
            {
                OnShowMsg?.Invoke(new MessageArgs { LogName = "LoadPort", Message = $"LoadPortHelper->读取配置文件失败，文件为空", MsgType = MsgType.Normal });
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

        public LoadPortDevice GetLoadPortDevice(int num)
        {
            LoadPortDevice ret = null;
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

        private List<LoadPortInfo> GetInfo()
        {
            var result = HTXmlHelper.EntityFromXMLFile<List<LoadPortInfo>>(FilePath);
            return result;
        }

        #endregion
    }
}
