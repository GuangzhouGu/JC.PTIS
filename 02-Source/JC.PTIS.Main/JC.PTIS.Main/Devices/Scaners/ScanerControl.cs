/*----------------------------------------------------------------
//公司名：   Suzhou JingCe Technology Co.,LTD
//版权申明： Copyright © 2025 JingCe. All Rights Reserved.
//
// =================================
// CLR版本     ：4.6.81.0
// 命名空间    ：
// 文件名称    ：
// =================================
// 创 建 者    ：guguangzhou
// 创建日期    ：2025/01/01 09:00:00 
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
using StdLib.HTCommunication.SocketWrapper;
using StdLib.LogHelper;
using StdLib.PCBaseFrame.Extension.Args;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Devices.Scaner
{
    public class ScanerControl
    {
        public event Action<ConnectedStsArgs> OnConnected;
        public event Action<MessageArgs> OnShowMsg;
        private HTCOMMSocketClient Client = null;
        ManualResetEvent Mre = new ManualResetEvent(false);
        ScanerInfo Info = null;
        string Reply = string.Empty;
        string LogName = "Scaner";
        public ScanerControl(ScanerInfo info)
        {
            Info = info;
            Client = new HTCOMMSocketClient();
            Client.ClientConnected += Client_ClientConnected;
            Client.ClientDisconnected += Client_ClientDisconnected;
            Client.DataReceived += Client_DataReceived;
        }

        public void Connect()
        {
            Client.Connect(Info.IP, Info.Port);
        }

        private void Client_DataReceived(object sender, SocketEventArgs e)
        {
            ProcessMsg(e.Datas);
        }

        private void Client_ClientDisconnected(object sender, SocketEventArgs e)
        {
            OnConnected?.Invoke(new ConnectedStsArgs { DeviceName = Info.Desc, ConnectedSts = false });
        }

        private void Client_ClientConnected(object sender, SocketEventArgs e)
        {
            OnConnected?.Invoke(new ConnectedStsArgs { DeviceName = Info.Desc, ConnectedSts = true });
        }
       
        private void ProcessMsg(byte[] buffer)
        {
            try
            {
                var str=Encoding.Default.GetString(buffer).Trim();
                if (string.IsNullOrEmpty(str)==false)
                {
                    Reply = str;
                    Mre.Set();
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger(LogName).Fatal(ex.ToString());
            }
        }
        public string Scancode()
        {
            var code = string.Empty;
            if (Client.Connected)
            {
                Mre.Reset();
                var bytes = GetBytes("TRG");
                Client.Send(bytes);
                var ret=Mre.WaitOne(3000);
                if (ret)
                {
                    code = Reply;
                }
            }
            return code;
        }
        private byte[]GetBytes(string cmd)
        {
            List<byte> list = new List<byte>();
            list.Add(0x02);
            var temp=  Encoding.Default.GetBytes(cmd);
            list.AddRange(temp);
            list.Add(0x03);
            return list.ToArray();
        }
        public void Uninit()
        {
            Client.Disconnect();
            Client.Dispose();
        }
    }
}
