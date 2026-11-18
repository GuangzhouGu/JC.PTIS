/*----------------------------------------------------------------
//公司名：   Suzhou HYC Technology Co.,LTD
//版权申明： Copyright © 2022 HYC. All Rights Reserved.
//
// =================================
// CLR版本     ：4.6.81.0
// 命名空间    ：HYC.SaturnMain.Devices.Rfid
// 文件名称    ：RfidHelper
// =================================
// 创 建 者    ：guguangzhou
// 创建日期    ：
// 功能描述    ：
// 使用说明    ：
//
//
// 创建标识：
//
// 修改标识：
// 修改描述：
//
// 修改标识：
// 修改描述：
//----------------------------------------------------------------*/
using JC.PTIS.Main.Params;
using StdLib.HTCommunication.SocketWrapper;
using StdLib.HTFile;
using StdLib.HTFile.HTXML;
using StdLib.HTModbus;
using StdLib.HTTools;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.PCBaseFrame.Extension.Device;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Devices.Scaner
{
    public class ScanerHelper : IDevice
    {
        public string DeviceName { get; set; } = "Scaner";
        public static ScanerHelper Instance { get { return DeviceManager.Instance.Get("Scaner") as ScanerHelper; } set { } }
        public event Action<ConnectedStsArgs> OnConnected;
        public event Action<MessageArgs> OnShowMsg;
        private string ConfigPath = AppDomain.CurrentDomain.BaseDirectory + @"\Config\Hardware\ScanerInfo.xml";
        public Dictionary<int, ScanerControl> ScanerDic = null;
        public int ScanerCount { get { return ScanerDic.Count; } }
        public void Init()
        {
            List<ScanerInfo> list = GetConfig();
            Task.Factory.StartNew(()=> {
                try
                {
                    ScanerDic = new Dictionary<int, ScanerControl>();
                    if (list != null && list.Count > 0)
                    {
                        for (int i = 0; i < list.Count; i++)
                        {
                            ScanerControl ctrl = new ScanerControl(list[i]);
                            ctrl.OnConnected += OnConnected;
                            ctrl.OnShowMsg += OnShowMsg;
                            ScanerDic.Add(i, ctrl);
                            ctrl.Connect();
                        }
                    }
                    else
                    {
                        OnShowMsg?.Invoke(new MessageArgs { LogName = DeviceName, Message = $"ScanerHelper->读取配置文件失败，文件为空", MsgType = MsgType.Error });
                    }
                }
                catch (Exception ex)
                {
                    OnConnected?.Invoke(new ConnectedStsArgs { DeviceName = list[0].Desc, ConnectedSts = false });
                    OnShowMsg?.Invoke(new MessageArgs { LogName= DeviceName ,Message=$"ScanerHelper->Init方法执行异常，异常信息：{ex}",MsgType=MsgType.Error} );
                }
            });

        }
        public string Scan(int num = 0)
        {
            try
            {
                string ret = string.Empty;
                if (ParaHelper.Global.IsScanerEnable==false)
                {
                    ret = DateTime.Now.ToString("yyyyMMddHH")+$"{num}";
                }
                else
                {
                    ret = ScanerDic[num].Scancode();
                }
                return ret;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        /// <summary>
        /// 读取配置信息
        /// </summary>
        /// <returns></returns>
        private List<ScanerInfo> GetConfig()
        {
            if (File.Exists(ConfigPath))
            {
                List<ScanerInfo> result = HTXmlHelper.EntityFromXMLFile<List<ScanerInfo>>(ConfigPath);
                return result;
            }
            return null;
        }

        public void Uninit()
        {
            try
            {
                Array.ForEach(ScanerDic.Values.ToArray(), p => p.Uninit());
            }
            catch (Exception ex)
            {
                OnShowMsg?.Invoke(new MessageArgs { LogName = DeviceName, Message = $"ScanerHelper->Uninit方法执行异常，异常信息：{ex}", MsgType = MsgType.Error });
            }
        }
    }
}
