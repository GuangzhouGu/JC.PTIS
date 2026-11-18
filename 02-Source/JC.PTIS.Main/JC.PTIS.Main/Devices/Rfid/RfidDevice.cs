using JC.Map.Devices.RFID;
using StdLib.HTCommunication.SerialPortWrapper;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.WindowsControls;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;

namespace JC.PTIS.LoadPort.Devices.EFEM
{
    /// <summary>
    /// RFID：
    /// 拨码开关设置：62页
    /// 通信协议：66页，85页
    /// 一共有17页数据，1页里存储16字节数据
    /// 客服电话：4008204535拨2再拨2
    /// </summary>
    public class RfidDevice
    {
        #region 公开事件

        public event Action<ConnectedStsArgs> OnConnected;  //设备连接/断开消息
        public event Action<MessageArgs> OnShowMsg;

        #endregion

        #region 实例变量

        private RfidDevicetInfo deviceInfo;
        private static object lockObject = new object();
        private static HTCOMMSerialPort serialPort1ToN;
        private HTCOMMSerialPort serialPort;
        private StdLib.LogHelper.Logger logger;
        private byte nodeNumber;
        private bool is1to1Protocol;
        private AutoResetEvent receiveEvent;
        private int nextReceiveLength;
        private List<byte> buffer;

        #endregion

        #region 公开属性

        public string DeviceName { get { return this.deviceInfo.DeviceName; } }

        public RfidDevicetInfo Info { get { return this.deviceInfo; } }

        #endregion

        #region 构造方法

        public RfidDevice(RfidDevicetInfo rfidDeviceInfo)
        {
            this.deviceInfo = rfidDeviceInfo;
            this.nodeNumber = (byte)rfidDeviceInfo.NodeNumber;
            this.logger = new StdLib.LogHelper.Logger("Log", string.Empty, rfidDeviceInfo.DeviceName, false);
            this.is1to1Protocol = rfidDeviceInfo.Is1to1Protocol;
            this.receiveEvent = new AutoResetEvent(false);
            this.buffer = new List<byte>();
        }

        #endregion

        #region 实例方法

        private void WriteLog(MsgType msgType, string format, params object[] param)
        {
            string message = string.Empty;

            if (param == null || param.Length == 0)
            {
                message = format;
            }
            else
            {
                message = string.Format(format, param);
            }

            switch (msgType)
            {
                case MsgType.Normal:
                case MsgType.Notify:
                    {
                        this.logger.Info(message);
                        break;
                    }

                case MsgType.Error:
                    {
                        this.logger.Error(message);
                        break;
                    }
            }
        }

        private bool OpenRfid()
        {
            if (this.is1to1Protocol)
            {
                try
                {
                    this.serialPort = new HTCOMMSerialPort();
                    this.serialPort.DataReceived += this.SerialPort_DataReceived;
                    this.serialPort.Init(new HTSerialPortInfo()
                    {
                        PortName = this.deviceInfo.PortName,
                        BaudRate = this.deviceInfo.BaudRate,
                        Parity = Parity.Even,
                    });
                    this.serialPort.Open();
                    return true;
                }
                catch (Exception ex)
                {
                    this.WriteLog(MsgType.Error, "打开RFID串口异常, {0}", ex);
                    return false;
                }
            }
            else
            {
                if (serialPort1ToN == null)
                {
                    try
                    {
                        serialPort1ToN = new HTCOMMSerialPort();
                        serialPort1ToN.Init(new HTSerialPortInfo()
                        {
                            PortName = this.deviceInfo.PortName,
                            BaudRate = this.deviceInfo.BaudRate,
                            Parity = Parity.None
                        });
                        serialPort1ToN.Open();
                    }
                    catch (Exception ex)
                    {
                        serialPort1ToN = null;
                        this.WriteLog(MsgType.Error, "打开RFID串口异常, {0}", ex);
                        return false;
                    }
                }

                serialPort1ToN.DataReceived += this.SerialPort_DataReceived;
                this.serialPort = serialPort1ToN;
                return true;
            }
        }

        private void CloseRfid()
        {
            if (this.serialPort != null)
            {
                try
                {
                    this.serialPort.Close();
                }
                finally
                {

                }
            }
        }

        private byte[] CalcFCS(IEnumerable<byte> payload)
        {
            byte xorSum = 0;
            foreach (byte b in payload)
            {
                xorSum ^= b; // EOR = 逐字节异或累加
            }

            string s1 = (xorSum >> 4).ToString();
            byte[] bytes1 = Encoding.ASCII.GetBytes(s1);
            string s2 = (xorSum & 0x0F).ToString();
            byte[] bytes2 = Encoding.ASCII.GetBytes(s2);

            List<byte> results = new List<byte>();
            results.AddRange(bytes1);
            results.AddRange(bytes2);
            return results.ToArray();
        }

        /// <summary>
        /// 设置某一位的值
        /// </summary>
        /// <param name="value">要设置的值</param>
        /// <param name="bit">要设置第几位</param>
        /// <param name="state">设置为1还是0</param>
        /// <returns></returns>
        private int SetBit(int value, byte bit, bool state)
        {
            if (state)
            {
                // 设置某一位为1
                return value | (1 << bit);
            }
            else
            {
                // 设置某一位为0
                return value & ~(1 << bit);
            }
        }


        private bool SendRequest(byte[] request, int readsize, out byte[] response)
        {
            response = null;

            if (this.serialPort == null)
            {
                this.WriteLog(MsgType.Error, "RFID指令执行失败, 串口没连接");
                return false;
            }

            try
            {
                if (this.is1to1Protocol)
                {
                    this.nextReceiveLength = readsize;
                    this.buffer.Clear();
                    this.serialPort.Send(request);
                    if (!this.receiveEvent.WaitOne(this.deviceInfo.ReadTimeout))
                    {
                        this.WriteLog(MsgType.Error, "RFID指令执行失败，接收数据超时");
                        return false;
                    }
                    byte[] bytes = this.buffer.ToArray();
                    string hex = string.Join(" ", bytes.Select(v => v.ToString("X2")));
                    this.WriteLog(MsgType.Normal, "接收报文:{0}", hex);
                    if (bytes[0] != '0' || bytes[1] != '0')
                    {
                        this.WriteLog(MsgType.Error, "RFID指令执行失败，错误码:{0}{1}, {2}", (char)bytes[0], (char)bytes[1], RfidUtils.Code2Text(bytes[0], bytes[1]));
                        return false;
                    }
                    response = bytes;
                }
                else
                {
                    byte[] bytes = null;

                    lock (lockObject)
                    {
                        this.nextReceiveLength = readsize;
                        this.buffer.Clear();
                        this.serialPort.Send(request);
                        if (!this.receiveEvent.WaitOne(this.deviceInfo.ReadTimeout))
                        {
                            this.WriteLog(MsgType.Error, "RFID指令执行失败，接收数据超时");
                            return false;
                        }
                        bytes = this.buffer.ToArray();
                    }

                    string hex = string.Join(" ", bytes.Select(v => v.ToString("X2")));
                    this.WriteLog(MsgType.Normal, "接收报文:{0}", hex);
                    if (bytes[3] != '0' || bytes[4] != '0')
                    {
                        this.WriteLog(MsgType.Error, "RFID指令执行失败，错误码:{0}{1}, {2}", (char)bytes[3], (char)bytes[4], RfidUtils.Code2Text(bytes[3], bytes[4]));
                        return false;
                    }

                    response = bytes;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.WriteLog(MsgType.Error, "RFID通信异常, {0}", ex);
                return false;
            }
        }


        private byte[] PackReadData(int startPageIndex, int numPages)
        {
            // 85页最底面
            List<byte> bytes = new List<byte>();

            if (!this.is1to1Protocol)
            {
                // SOH
                bytes.Add(0x01);

                // 节点编号
                string nn = this.nodeNumber.ToString().PadLeft(2, '0');
                bytes.Add(Convert.ToByte(nn[0]));
                bytes.Add(Convert.ToByte(nn[1]));
            }

            // 命令代码
            bytes.Add((byte)'0');
            bytes.Add((byte)'1');
            bytes.Add((byte)'0');
            bytes.Add((byte)'0');

            // 页名
            int pageName = 0;
            for (int i = 0; i < numPages; i++)
            {
                pageName = SetBit(pageName, (byte)(startPageIndex + i + 2), true);
            }
            byte[] pageNameBytes = BitConverter.GetBytes(pageName).Reverse().ToArray();
            for (int i = 0; i < pageNameBytes.Length; i++)
            {
                string str = pageNameBytes[i].ToString("X2").PadLeft(2, '0');
                bytes.Add(Convert.ToByte(str[0]));
                bytes.Add(Convert.ToByte(str[1]));
            }

            //bytes.Add((byte)'0');
            //bytes.Add((byte)'0');
            //bytes.Add((byte)'0');
            //bytes.Add((byte)'0');
            //bytes.Add((byte)'0');
            //bytes.Add((byte)'0');
            //bytes.Add((byte)'0');
            //bytes.Add((byte)'0');

            if (!this.is1to1Protocol)
            {
                // FCS
                byte[] fcs = this.CalcFCS(bytes.Skip(1));
                bytes.AddRange(fcs);
            }

            // CR
            bytes.Add(0x0D);

            return bytes.ToArray();
        }

        private byte[] PackWriteData(int startPageIndex, int numPages, byte[] pageData)
        {
            // 86页写入
            List<byte> bytes = new List<byte>();

            if (!this.is1to1Protocol)
            {
                // SOH
                bytes.Add(0x01);

                // 节点编号
                string nn = this.nodeNumber.ToString().PadLeft(2, '0');
                bytes.Add(Convert.ToByte(nn[0]));
                bytes.Add(Convert.ToByte(nn[1]));
            }

            // 命令代码
            bytes.Add((byte)'0');
            bytes.Add((byte)'2');
            bytes.Add((byte)'0');
            bytes.Add((byte)'0');

            // 页名
            int pageName = 0;
            for (int i = 0; i < numPages; i++)
            {
                pageName = SetBit(pageName, (byte)(startPageIndex + i + 2), true);
            }
            byte[] pageNameBytes = BitConverter.GetBytes(pageName).Reverse().ToArray();
            for (int i = 0; i < pageNameBytes.Length; i++)
            {
                string str = pageNameBytes[i].ToString("X2").PadLeft(2, '0');
                bytes.Add(Convert.ToByte(str[0]));
                bytes.Add(Convert.ToByte(str[1]));
            }

            // 写入数据
            bytes.AddRange(pageData);

            if (!this.is1to1Protocol)
            {
                // FCS
                byte[] fcs = this.CalcFCS(bytes.Skip(1));
                bytes.AddRange(fcs);
            }

            // CR
            bytes.Add(0x0D);

            return bytes.ToArray();
        }

        private byte[] PackWriteData(int pageIndex, byte[] pageData)
        {
            return this.PackWriteData(pageIndex, 1, pageData);
        }

        #endregion

        #region 公开接口

        public void Connect()
        {
            this.OnConnected?.Invoke(new ConnectedStsArgs() { ConnectedSts = false, DeviceName = this.DeviceName });
            if (this.OpenRfid())
            {
                this.OnConnected?.Invoke(new ConnectedStsArgs() { ConnectedSts = true, DeviceName = this.DeviceName });
            }
        }

        public void Disconnect()
        {
            this.CloseRfid();
        }

        /// <summary>
        /// 通过RFID读取CarrierID
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool ReadCarrierId(out string carrierId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 读取RFID里的所有数据
        /// </summary>
        /// <returns></returns>
        public List<byte[]> ReadAllPages()
        {
            List<byte> pageDatas = new List<byte>();

            if (this.is1to1Protocol)
            {
                if (!this.SendRequest(this.PackReadData(0, 16), 259, out byte[] bytes1) ||
                    !this.SendRequest(this.PackReadData(15, 1), 19, out byte[] bytes2))
                {
                    return null;
                }

                pageDatas.AddRange(bytes1.Skip(2).Take(256));
                pageDatas.AddRange(bytes2.Skip(2).Take(16));
            }
            else
            {
                if (!this.SendRequest(this.PackReadData(0, 16), 264, out byte[] bytes1) ||
                    !this.SendRequest(this.PackReadData(15, 1), 24, out byte[] bytes2))
                {
                    return null;
                }

                pageDatas.AddRange(bytes1.Skip(5).Take(256));
                pageDatas.AddRange(bytes2.Skip(5).Take(16));
            }

            List<byte[]> result = new List<byte[]>();
            for (int i = 0; i < 17; i++)
            {
                byte[] pageData = pageDatas.Skip(i * 8).Take(8).ToArray();
                result.Add(pageData);
            }

            return result;
        }

        /// <summary>
        /// 读取指定页的数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <returns></returns>
        public byte[] ReadPage(int pageIndex)
        {
            if (this.is1to1Protocol)
            {
                if (!this.SendRequest(this.PackReadData(pageIndex, 1), 19, out byte[] bytes1))
                {
                    return null;
                }

                return bytes1.Skip(2).Take(16).ToArray();
            }
            else
            {
                if (!this.SendRequest(this.PackReadData(pageIndex, 1), 24, out byte[] bytes1))
                {
                    return null;
                }

                return bytes1.Skip(5).Take(16).ToArray();
            }
        }

        /// <summary>
        /// 向指定页写入指定数据，最多写入16字节
        /// </summary>
        /// <param name="pageIndex">要写入的页索引</param>
        /// <param name="pageData">要写入的页数据</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public bool WritePage(int pageIndex, byte[] pageData)
        {
            if (pageData.Length < 16)
            {
                // 小于16个字节，那么在后面补0
                List<byte> buffer = new List<byte>();
                buffer.AddRange(pageData);
                buffer.AddRange(Enumerable.Repeat((byte)'0', 16 - pageData.Length));
                pageData = buffer.ToArray();
            }
            else if (pageData.Length > 16)
            {
                // 大于16字节，那么把超出的截断
                pageData = pageData.Take(16).ToArray();
            }

            if (this.is1to1Protocol)
            {
                return this.SendRequest(this.PackWriteData(pageIndex, pageData), 3, out byte[] bytes1);
            }
            else
            {
                return this.SendRequest(this.PackWriteData(pageIndex, pageData), 8, out byte[] bytes1);
            }
        }

        #endregion

        #region 事件处理器

        private void SerialPort_DataReceived(object sender, SerialPortEventArgs e)
        {
            this.buffer.AddRange(e.Datas);

            if (this.is1to1Protocol)
            {
                if (this.buffer.Count < 2)
                {
                    // 继续接收
                    return;
                }

                if (this.buffer[0] != '0' || this.buffer[1] != '0')
                {
                    if (this.buffer.Count == 3)
                    {
                        this.receiveEvent.Set();
                    }
                }
                else
                {
                    if (this.buffer.Count == this.nextReceiveLength)
                    {
                        this.receiveEvent.Set();
                    }
                }
            }
            else
            {
                if (this.buffer.Count < 5)
                {
                    // 继续接收
                    return;
                }

                string number = this.nodeNumber.ToString().PadLeft(2, '0');

                if (this.buffer[1] != number[0] || this.buffer[2] != number[1])
                {
                    return;
                }

                if (this.buffer[3] != '0' || this.buffer[4] != '0')
                {
                    if (this.buffer.Count == 8)
                    {
                        this.receiveEvent.Set();
                    }
                }
                else
                {
                    if (this.buffer.Count == this.nextReceiveLength)
                    {
                        this.receiveEvent.Set();
                    }
                }
            }
        }

        #endregion
    }
}



