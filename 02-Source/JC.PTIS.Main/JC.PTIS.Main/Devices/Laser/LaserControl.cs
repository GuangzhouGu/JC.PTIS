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
using JC.LidAttach.Devices.Laser;
using StdLib.HTCommunication.SerialPortWrapper;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.PCBaseFrame.Extension.Entities;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JC.LidAttach.Devices.GlueDispenser
{
    public class LaserControl
    {
        public event Action<ConnectedStsArgs> OnConnected;  //设备连接/断开消息
        public event Action<MessageArgs> OnShowMsg;
        HTCOMMSerialPort Port = null;
        string Msg = string.Empty;
        ManualResetEvent Mre = new ManualResetEvent(false);
        public LaserInfo Info = null;
        public  LaserControl(LaserInfo info)
        {
            Info = info;
            Port = new HTCOMMSerialPort();
            Port.DataReceived += DataReceivedHandle;
        }
        public void Connect()
        {
            try
            {
                HTSerialPortInfo _info = new HTSerialPortInfo { PortName = Info.PortName, BaudRate = Info.BaudRate, DataBits = 8, StopBits = StopBits.One, Parity = Parity.None };
                bool ret = Port.Init(_info);
                OnConnected?.Invoke(new ConnectedStsArgs { DeviceName = Info.Desc, ConnectedSts = ret });
            }
            catch (Exception ex)
            {
                OnConnected?.Invoke(new ConnectedStsArgs { DeviceName = Info.Desc, ConnectedSts = false });
                OnShowMsg?.Invoke(new MessageArgs { LogName = "Laser", Message = $"LaserControl->Connect方法执行异常，异常信息：{ex}", MsgType = MsgType.Normal });
            }
        }
        private void DataReceivedHandle(object sender, SerialPortEventArgs e)
        {
            string msg = Encoding.Default.GetString(e.Datas);
            string flag = msg.Substring(3, 1);
            string cmd = msg.Substring(4, 3);
            if (flag == "$")
            {
                Mre.Set();
                switch (cmd)
                {
                    case "RMD":
                        {
                            Msg = msg.Substring(7, 8);
                            OnShowMsg?.Invoke(new MessageArgs { LogName="Laser",Message= $"获取测量值：{Msg}" ,MsgType=MsgType.Normal});
                            break;
                        }
                    case "WZS":
                        {
                            OnShowMsg?.Invoke(new MessageArgs { LogName = "Laser", Message = "$清零指令发送完成", MsgType = MsgType.Normal });
                            break;
                        }
                }
            }
            else
            {
                int errorCode = int.Parse(msg.Substring(4, 2));
                OnShowMsg?.Invoke(new MessageArgs { LogName = "Laser", Message = "指令:" + cmd + EnumHelper.GetEnumDesc((ErrorEnum)errorCode), MsgType = MsgType.Error });
            }
        }

        public double? GetValue(int channel = 0)
        {
            double? value = null;
            string cmd = $"%0{channel}#RMD**\r";
            Send(cmd);
            Mre.Reset();
            bool ret = Mre.WaitOne(3000);
            if (ret)
            {
                value = Math.Round(double.Parse(Msg), 3);
            }
            return value;
        }

        public bool Zero(int channel = 0)
        {
            string cmd = $"%0{channel}#RZS00001**\r";
            Send(cmd);
            Mre.Reset();
            bool ret = Mre.WaitOne(1000);
            return ret;
        }

        public void Uninit()
        {
            Port.Close();
            Port.Dispose();
        }
        private void Send(string msg)
        {
            Msg = string.Empty;
            if (Port.IsOpen)
            {
                Port.Send(msg);
            }
            else
            {
                Port.Open();
            }
        }
    }
    public enum ErrorEnum
    {
        指令错误 = 01,
        地址错误 = 02,
        数据错误 = 03,
        BBC错误 = 04,
        通讯错误 = 11,
        控制流程错误 = 21,
        执行错误 = 22,
    }
}
