using JC.Map.Devices.EFEM;
using StdLib.PCBaseFrame.Extension.Args;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.PTIS.LoadPort.Devices.EFEM
{
    /// <summary>
    /// 
    /// 
    /// 表示一个LoadPort设备
    /// 
    /// FOUP，全称Front Opening Unified Pod（或Front Opening Universal Pod），是一种专门设计用于半导体晶圆厂的容器，用于在高洁净环境下保护、储存和自动化传送晶圆 
    /// 它通常用于12英寸（300mm）晶圆，每个FOUP可容纳25片晶圆，并通过前开口门让机械手直接抓取晶圆，无需打开整个容器
    /// 
    /// Passive：上位机
    /// Active：天车
    /// 
    /// 
    /// RFID：
    /// 拨码开关设置：62页
    /// 通信协议：66页，85页
    /// 一共有17页数据，1页里存储8字节数据
    /// 客服电话：4008204535拨2再拨2
    /// </summary>
    public class LoadPortDevice
    {
        private static char[] Splitters = new char[] { ';', ':' };
        private static char[] Splitters2 = new char[] { '/' };

        #region 公开事件

        /// <summary>
        /// 当天车上料或者下料状态改变的时候触发
        /// </summary>
        public event Action<int, LoadTypes, LPCodes, List<SlotStates>> LoadStatusChanged;
        public event Action<ConnectedStsArgs> OnConnected;  //设备连接/断开消息

        #endregion

        #region 实例变量

        private LoadPortInfo lpParam;
        private TcpClient tcpClient;
        private StreamWriter writer;
        private StreamReader reader;
        private LoadTypes loadType = LoadTypes.None;
        private Task task;
        private object requestLock = new object();
        private Stopwatch stopwatch = new Stopwatch();
        private StdLib.LogHelper.Logger logger;
        private FoupTypes foupType = FoupTypes.Unknown;

        #endregion

        #region 公开属性

        public string DeviceName { get { return this.lpParam.DeviceName; } }

        public LoadPortInfo Info { get { return this.lpParam; } }

        #endregion

        public LoadPortDevice(LoadPortInfo loadPortInfo)
        {
            this.lpParam = loadPortInfo;
            this.logger = new StdLib.LogHelper.Logger("Log", string.Empty, loadPortInfo.DeviceName, false);
        }

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

        private bool OpenTcpClient()
        {
            try
            {
                this.WriteLog(MsgType.Normal, "LoadPort网口连接中, IPAddress = {0}, Port = {1}", this.lpParam.IPAddress, this.lpParam.Port);

                this.tcpClient = new TcpClient();
                this.tcpClient.BeginConnect(IPAddress.Parse(this.lpParam.IPAddress), this.lpParam.Port, this.OnClientConnected, null);
                return false;
            }
            catch (Exception ex)
            {
                this.WriteLog(MsgType.Error, "打开LoadPort网口异常, IPAddress = {0}, Port = {1}, {2}", this.lpParam.IPAddress, this.lpParam.Port, ex);
                return false;
            }
        }

        private void CloseTcpClient()
        {
            if (this.tcpClient != null)
            {
                try
                {
                    this.tcpClient.Close();
                    this.tcpClient.Dispose();
                    this.tcpClient = null;
                    this.writer = null;
                    this.reader = null;
                }
                finally
                { }
            }
        }

        /// <summary>
        /// 发送一条消息并接收回复
        /// </summary>
        /// <param name="request">发送的请求</param>
        /// <param name="messageCount">要收几次消息</param>
        /// <param name="message1">收到的第1条消息</param>
        /// <param name="message2">收到的第2条消息</param>
        /// <returns></returns>
        private LPCodes PerformRequest(string request, bool isControlMessage, out string message1, out string message2)
        {
            message1 = string.Empty;
            message2 = string.Empty;

            StreamWriter writer = this.writer;
            StreamReader reader = this.reader;
            if (writer == null || reader == null)
            {
                this.WriteLog(MsgType.Error, "发送消息失败, LoadPort未连接, {0}", request);
                return LPCodes.NotConnected;
            }

            this.WriteLog(MsgType.Normal, "发送:{0}", request);

            string errorMessage = string.Empty;
            bool success = true;

            lock (this.requestLock)
            {
                if (!this.SendMessage(writer, request, out errorMessage) ||
                    !this.GetMessage(reader, out message1, out errorMessage))
                {
                    success = false;
                }
                else
                {
                    // 控制消息才有可能是两条
                    if (isControlMessage)
                    {
                        // 如果是ACK，那么再收下一条消息
                        if (message1.StartsWith("s00ACK"))
                        {
                            if (!this.GetMessage(reader, out message2, out errorMessage))
                            {
                                success = false;
                            }
                        }
                        else
                        {
                            // SetControlMessage永远以第一条消息是ACK，第二条消息是结果来解析
                            // LoadPort的Bug：发送控制消息之后，有的时候只回复结果不回复ACK，有的时候结果和ACK都回复
                            // 确保控制类型的消息第一条回复是ACK，第二条回复是结果
                            // 如果控制消息只回复了结果没回复ACK，那么第一条消息设置为空，第二条消息是结果
                            message2 = message1;
                            message1 = string.Empty;
                        }
                    }
                }
            }

            if (!success)
            {
                this.WriteLog(MsgType.Error, errorMessage);
                this.CloseTcpClient();
                this.OnConnected?.Invoke(new ConnectedStsArgs() { ConnectedSts = false, DeviceName = this.DeviceName });
                return LPCodes.CommunicationError;
            }

            return LPCodes.SUCCESS;
        }

        private bool GetMessage(StreamReader reader, out string message, out string errorMessage)
        {
            message = string.Empty;
            errorMessage = string.Empty;

            for (int i = 0; i < 3; i++)
            {
                try
                {
                    message = reader.ReadLine();
                }
                catch (Exception ex)
                {
                    errorMessage = string.Format("接收消息异常, {0}, 关闭连接", ex);
                    return false;
                }

                if (!string.IsNullOrEmpty(message))
                {
                    return true;
                }
            }

            errorMessage = "连续收到3次空消息, 关闭连接";
            return false;
        }

        private bool SendMessage(StreamWriter writer, string message, out string errorMessage)
        {
            errorMessage = string.Empty;

            try
            {
                writer.WriteLine(message);
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = string.Format("发送消息异常, 关闭连接, {0}", ex);
                return false;
            }
        }

        /// <summary>
        /// 向LoadPort发送一条控制指令
        /// </summary>
        /// <param name="controlMessage"></param>
        /// <returns></returns>
        private LPCodes SendControlMessage(string controlMessage)
        {
            LPCodes lpCode = this.PerformRequest(controlMessage, true, out string message1, out string message2);
            if (lpCode != LPCodes.SUCCESS)
            {
                return lpCode;
            }

            // 判断返回的是不是ACK
            string[] strings = controlMessage.Split(Splitters, StringSplitOptions.RemoveEmptyEntries);
            if (strings.Length != 2)
            {
                this.WriteLog(MsgType.Error, "回复消息格式错误");
                return LPCodes.InvalidMessageFormat;
            }

            string command = strings[1];

            #region 处理Message1（ACK）

            this.WriteLog(MsgType.Normal, "收到的第一条回复:{0}", message1);
            if (!string.IsNullOrEmpty(message1))
            {
                string ack = string.Format("s00ACK:{0};", command);
                if (message1 != ack)
                {
                    this.WriteLog(MsgType.Error, "收到的第一条回复不是ACK, {0}", message1);
                    return LPCodes.InvalidMessageFormat;
                }
            }

            #endregion

            #region 处理Message2（结果）

            this.WriteLog(MsgType.Normal, "收到的第二条回复:{0}", message2);

            if (message2 == string.Format("s00INF:{0};", command))
            {
                this.WriteLog(MsgType.Normal, "指令执行成功");
                return LPCodes.SUCCESS;
            }

            if (message2.StartsWith("s00NAK:INTER"))
            {
                // 联锁操作前错误
                string lpCodeText = message2.Substring(13).TrimEnd(';');
                if (!Enum.TryParse<LPCodes>(lpCodeText, out LPCodes lpCode1))
                {
                    this.WriteLog(MsgType.Error, "指令执行失败, 未知的错误码:{0}", lpCodeText);
                    return LPCodes.InvalidLpCode;
                }
                this.WriteLog(MsgType.Error, "指令执行失败, {0}", LpUtils.GetErrorMessage(lpCode1));
                return lpCode1;
            }
            else if (message2.StartsWith("s00ABS"))
            {
                // 联锁运行期间出错

                // 分两种格式，一种是s00ABS:CLDMP/SAFTY，一种是s00ABS:SAFTY
                string[] strs = message2.Split(Splitters, StringSplitOptions.RemoveEmptyEntries);
                if (strs.Length != 2)
                {
                    this.WriteLog(MsgType.Error, "指令执行失败, 没法识别的回复格式");
                    return LPCodes.InvalidMessageFormat;
                }

                string lpCodeText = string.Empty;
                LPCodes lpCode1 = LPCodes.SUCCESS;

                if (strs[1].Contains("/"))
                {
                    // 格式是s00ABS:CLDMP/SAFTY;
                    lpCodeText = message2.Substring(13).TrimEnd(';');
                }
                else
                {
                    // 格式是s00ABS:SAFTY;
                    lpCodeText = message2.Substring(7).TrimEnd(';');
                }

                if (!Enum.TryParse<LPCodes>(lpCodeText, out lpCode1))
                {
                    this.WriteLog(MsgType.Error, "指令执行失败, 未知的错误码:{0}", lpCodeText);
                    return LPCodes.InvalidLpCode;
                }
                this.WriteLog(MsgType.Error, "指令执行失败, {0}", LpUtils.GetErrorMessage(lpCode1));
                return lpCode1;
            }
            else
            {
                this.WriteLog(MsgType.Error, "未知的回复格式:{0}", message2);
                return LPCodes.InvalidMessageFormat;
            }

            #endregion
        }

        private LPCodes SendGetMessage(string getMessage, out string getResult)
        {
            getResult = string.Empty;
            LPCodes lpCode = this.PerformRequest(getMessage, false, out string message, out string message2);
            if (lpCode != LPCodes.SUCCESS)
            {
                return lpCode;
            }

            this.WriteLog(MsgType.Normal, "接收:{0}", message);

            string[] strings = message.Split(Splitters2, StringSplitOptions.RemoveEmptyEntries);
            if (strings.Length != 2)
            {
                this.WriteLog(MsgType.Error, "回复消息格式错误");
                return LPCodes.InvalidMessageFormat;
            }

            this.WriteLog(MsgType.Normal, "指令执行成功");

            getResult = strings[1].TrimEnd(';');
            return LPCodes.SUCCESS;
        }

        private LPCodes SendSetMessage(string setMessage)
        {
            LPCodes lpCode = this.PerformRequest(setMessage, false, out string message, out string message2);
            if (lpCode != LPCodes.SUCCESS)
            {
                return lpCode;
            }

            this.WriteLog(MsgType.Normal, "接收:{0}", message);

            string[] strings = message.Split(Splitters, StringSplitOptions.RemoveEmptyEntries);
            if (strings.Length != 2)
            {
                this.WriteLog(MsgType.Error, "回复消息格式错误");
                return LPCodes.InvalidMessageFormat;
            }

            string command = strings[1];
            string ack = string.Format("s00ACK:{0};", command);
            if (message != ack)
            {
                this.WriteLog(MsgType.Error, "SendSetMessage失败, 回复:{0}", message);
                return LPCodes.InvalidMessageFormat;
            }

            this.WriteLog(MsgType.Normal, "指令执行成功");
            return LPCodes.SUCCESS;
        }


        private bool ReadPortState(E84Ports port, out bool state)
        {
            return E84IOReader.ReadPortState(this.lpParam.Num, port, out state);
        }

        private bool WritePortState(E84Ports port, bool state)
        {
            return E84IOReader.WritePortState(this.lpParam.Num, port, state);
        }

        private bool WaitPortState(E84Ports port, bool targetState, int timeout)
        {
            targetState = false;

            this.stopwatch.Restart();
            while (this.stopwatch.ElapsedMilliseconds < timeout)
            {
                try
                {
                    if (!this.ReadPortState(port, out bool currentState))
                    {
                        this.WriteLog(MsgType.Error, "读取端口状态失败, {0}", port.ToString());
                        continue;
                    }

                    if (currentState == targetState)
                    {
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    this.WriteLog(MsgType.Error, "读取端口状态异常, {0}, {1}", port.ToString(), ex);
                }
                finally
                {
                    Thread.Sleep(500);
                }
            }

            this.WriteLog(MsgType.Error, "等待端口状态超时, {0}", port.ToString());
            return false;
        }

        private void ProcessOHTErrorState(LoadTypes loadTypes, LPCodes lpCode)
        {
            // 1. * *VALID * *：端口选择有效信号 → OFF（核心，关闭端口寻址）
            // 2. * *CS_0 / CS_1 * *：端口 Stage 编码 → 全部 OFF
            // 3. * *TR_REQ * *：交接请求 → OFF
            // 4. * *BUSY * *：正在交接忙信号 → OFF
            // 5. * *COMPT * *：交接完成通知 → OFF
            // 6. * *CONT * *：连续交接标志 → OFF
            // 7. * *VA * *：车辆到达 → OFF
            // 8. * *AM_AVBL * *：搬运臂可用 → OFF

            this.WritePortState(E84Ports.OUTPUT_U_REQ, false);
            this.WritePortState(E84Ports.OUTPUT_L_REQ, false);
            this.WritePortState(E84Ports.OUTPUT_READY, false);

            this.WriteLog(MsgType.Error, LpUtils.GetErrorMessage(lpCode));
            this.LoadStatusChanged?.Invoke(this.lpParam.Num, loadTypes, lpCode, null);
        }

        /// <summary>
        /// LoadPort执行Load动作
        /// 根据LoadPort手册Loading流程图实现
        /// </summary>
        /// <param name="slotStates">Foup里每个插槽的状态</param>
        /// <returns></returns>
        private LPCodes LpLoad(out List<SlotStates> slotStates)
        {
            // 需要先设置成自动模式

            slotStates = null;

            LPCodes lpCode = this.SendControlMessage("s00MOV:CLDMP;");
            if (lpCode != LPCodes.SUCCESS)
            {
                // 没复位先复位
                if (lpCode == LPCodes.ORGYT)
                {
                    lpCode = this.ABORG();
                    if (lpCode != LPCodes.SUCCESS)
                    {
                        return lpCode;
                    }
                }
                else
                {
                    return lpCode;
                }
            }

            return this.MAPRD(out slotStates);
        }

        /// <summary>
        /// LoadPort执行Unload动作
        /// 根据LoadPort手册Unloading流程图实现
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private LPCodes LpUnload()
        {
            return this.SendControlMessage("s00MOV:CULOD;");
        }

        /// <summary>
        /// 执行天车上料或者下料操作
        /// </summary>
        /// <param name="userData"></param>
        private void OHTLoad(object userData)
        {
            OHTLoadOptions loadOptions = userData as OHTLoadOptions;
            LoadTypes loadType = loadOptions.LoadType;
            FoupTypes foupType = loadOptions.FoupType;

            try
            {
                if (loadType == LoadTypes.OHTUnload)
                {
                    LPCodes lpCode = this.LpUnload();
                    if (lpCode != LPCodes.SUCCESS)
                    {
                        this.ProcessOHTErrorState(loadType, lpCode);
                        return;
                    }
                }
                else if (loadType == LoadTypes.OHTLoad)
                {
                    // 上料之前需要设置载具类型
                    LPCodes lpCode = this.SetFoupType(foupType);
                    if (lpCode != LPCodes.SUCCESS)
                    {
                        this.ProcessOHTErrorState(loadType, lpCode);
                        return;
                    }
                }

                #region 端口选择 None

                // Passive（LoadPort）检测 VALID 信号，解析端口编码；检查目标端口状态：端口空载、舱门状态正常、无安全联锁报警。校验通过后，Passive 输出 L_REQ=ON（允许下放 FOUP）

                if (!this.WaitPortState(E84Ports.INPUT_VALID, true, this.lpParam.VALID_TIMEOUT))
                {
                    this.ProcessOHTErrorState(loadType, LPCodes.IO_VALID_ERROR);
                    return;
                }

                // cs0表示第一个LoadPort, cs1表示第二个LoadPort
                // TODO：华天的设备1个LoadPort上只能放1个Foup，需要和华天的技术人员定义cs0信号还是cs1信号表示LoadPort
                if (!this.ReadPortState(E84Ports.INPUT_CS_0, out bool CS_0) || !this.ReadPortState(E84Ports.INPUT_CS_1, out bool CS_1))
                {
                    this.ProcessOHTErrorState(loadType, LPCodes.IO_CSX_ERROR);
                    return;
                }

                // 检查目标端口状态：端口空载、舱门状态正常、无安全联锁报警
                if (CS_0 && !CS_1)
                {
                    // cs0上料
                }
                else if (!CS_0 && CS_1)
                {
                    // cs1上料
                }
                else if (CS_0 && CS_1)
                {
                    // cs0和cs1同时上料
                    this.ProcessOHTErrorState(loadType, LPCodes.IO_CSX_INVALID);
                    return;
                }
                else
                {
                    // 天车还没确定要对那个LoadPort上料
                    this.ProcessOHTErrorState(loadType, LPCodes.IO_CSX_INVALID2);
                    return;
                }

                // 设置L_REQ，允许下放 FOUP
                if (loadType == LoadTypes.OHTLoad)
                {
                    // 如果是上料那么写入L_REQ
                    if (!this.WritePortState(E84Ports.OUTPUT_L_REQ, true))
                    {
                        this.ProcessOHTErrorState(loadType, LPCodes.IO_L_REQ_ERROR);
                        return;
                    }
                }
                else
                {
                    // 如果是下料那么写入U_REQ
                    if (!this.WritePortState(E84Ports.OUTPUT_U_REQ, true))
                    {
                        this.ProcessOHTErrorState(loadType, LPCodes.IO_U_REQ_ERROR);
                        return;
                    }
                }

                #endregion

                #region 搬运请求 TR_REQ

                // Passive 收到 TR_REQ，确认可以执行交接，释放安全互锁，输出 READY=ON

                if (!this.WaitPortState(E84Ports.INPUT_TR_REQ, true, this.lpParam.TR_REQ_TIMEOUT))
                {
                    this.ProcessOHTErrorState(loadType, LPCodes.IO_TR_REQ_ERROR);
                    return;
                }

                // TODO：释放安全互锁，让LoadPort进入可以放Foup的状态

                if (!this.WritePortState(E84Ports.OUTPUT_READY, true))
                {
                    this.ProcessOHTErrorState(loadType, LPCodes.IO_READY_ERROR);
                    return;
                }

                #endregion

                #region 机构动作等待 BUSY -> READY

                if (!this.WaitPortState(E84Ports.INPUT_BUSY, true, this.lpParam.BUSY_TIMEOUT))
                {
                    this.ProcessOHTErrorState(loadType, LPCodes.IO_BUSY_ERROR);
                    return;
                }

                // FOUP 完全落位、机械传感器确认载具放置稳定后，Passive 撤销请求信号：L_REQ=OFF。

                if (loadType == LoadTypes.OHTLoad)
                {
                    // 判断是否完全落位
                    if (!this.IsFoupPlaced(this.lpParam.FOUP_TIMEOUT))
                    {
                        this.ProcessOHTErrorState(loadType, LPCodes.FOUP_PLACE_TIMEOUT);
                        return;
                    }

                    if (!this.WritePortState(E84Ports.OUTPUT_L_REQ, false))
                    {
                        this.ProcessOHTErrorState(loadType, LPCodes.IO_L_REQ_ERROR);
                        return;
                    }
                }
                else
                {
                    // 判断是否被取走
                    if (this.IsFoupTaken(this.lpParam.FOUP_TIMEOUT))
                    {
                        this.ProcessOHTErrorState(loadType, LPCodes.FOUP_TAKE_TIMEOUT);
                        return;
                    }

                    if (!this.WritePortState(E84Ports.OUTPUT_U_REQ, false))
                    {
                        this.ProcessOHTErrorState(loadType, LPCodes.IO_U_REQ_ERROR);
                        return;
                    }
                }

                #endregion

                #region COMPT

                if (!this.WaitPortState(E84Ports.INPUT_BUSY, false, this.lpParam.BUSY_TIMEOUT))
                {
                    this.ProcessOHTErrorState(loadType, LPCodes.IO_BUSY_ERROR);
                    return;
                }

                if (!this.WaitPortState(E84Ports.INPUT_TR_REQ, false, this.lpParam.TR_REQ_TIMEOUT))
                {
                    this.ProcessOHTErrorState(loadType, LPCodes.IO_TR_REQ_ERROR);
                    return;
                }

                if (!this.WaitPortState(E84Ports.INPUT_COMPT, true, this.lpParam.COMPT_TIMEOUT))
                {
                    this.ProcessOHTErrorState(loadType, LPCodes.IO_COMPT_ERROR);
                    return;
                }

                if (!this.WritePortState(E84Ports.OUTPUT_READY, false))
                {
                    this.ProcessOHTErrorState(loadType, LPCodes.IO_READY_ERROR);
                    return;
                }

                #endregion

                #region COMPT2

                if (!this.WaitPortState(E84Ports.INPUT_COMPT, false, this.lpParam.COMPT_TIMEOUT))
                {
                    this.ProcessOHTErrorState(loadType, LPCodes.IO_COMPT_ERROR);
                    return;
                }

                if (!this.WaitPortState(E84Ports.INPUT_VALID, false, this.lpParam.VALID_TIMEOUT))
                {
                    this.ProcessOHTErrorState(loadType, LPCodes.IO_VALID_ERROR);
                    return;
                }

                if (!this.WaitPortState(E84Ports.INPUT_CS_0, false, this.lpParam.CS_0_TIMEOUT))
                {
                    this.ProcessOHTErrorState(loadType, LPCodes.IO_CS0_RESET_ERROR);
                    return;
                }

                if (!this.WaitPortState(E84Ports.INPUT_CS_1, false, this.lpParam.CS_1_TIMEOUT))
                {
                    this.ProcessOHTErrorState(loadType, LPCodes.IO_CS1_RESET_ERROR);
                    return;
                }

                #endregion

                List<SlotStates> slotStates = null;

                if (loadType == LoadTypes.OHTLoad)
                {
                    LPCodes lpCode = this.LpLoad(out slotStates);
                    if (lpCode != LPCodes.SUCCESS)
                    {
                        this.ProcessOHTErrorState(loadType, lpCode);
                        return;
                    }
                }

                this.LoadStatusChanged?.Invoke(this.lpParam.Num, loadType, LPCodes.SUCCESS, slotStates);
            }
            catch (Exception ex)
            {
                this.WriteLog(MsgType.Error, "天车上下料异常, {0}", ex);
                this.ProcessOHTErrorState(loadType, LPCodes.EXCEPTION);
            }
            finally
            {
                this.loadType = LoadTypes.None;
            }
        }

        /// <summary>
        /// 判断Foup是否放置到位
        /// </summary>
        /// <param name="timeout">放置到位的超时时间</param>
        /// <returns></returns>
        private bool IsFoupPlaced(int timeout)
        {
            DateTime start = DateTime.Now;

            while ((DateTime.Now - start).TotalMilliseconds < timeout)
            {
                LPCodes lpCode = this.HasFoup(out bool hasFoup);
                if (lpCode != LPCodes.SUCCESS)
                {
                    Thread.Sleep(500);
                    continue;
                }

                if (hasFoup)
                {
                    return true;
                }

                Thread.Sleep(500);
            }

            return false;
        }

        /// <summary>
        /// 判断Foup是否被取走
        /// </summary>
        /// <param name="timeout">取走的超时时间</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        private bool IsFoupTaken(int timeout)
        {
            DateTime start = DateTime.Now;

            while ((DateTime.Now - start).TotalMilliseconds < timeout)
            {
                LPCodes lpCode = this.HasFoup(out bool hasFoup);
                if (lpCode != LPCodes.SUCCESS)
                {
                    Thread.Sleep(500);
                    continue;
                }

                if (!hasFoup)
                {
                    return true;
                }

                Thread.Sleep(500);
            }

            return false;
        }

        #endregion

        #region LoadPort接口

        /// <summary>
        /// 判断某个端口是否可以上料
        /// 检查目标端口状态：端口空载、舱门状态正常、无安全联锁报警
        /// </summary>
        /// <returns></returns>
        private LPCodes CanLpLoad()
        {
            LPCodes lpCode = this.HasFoup(out bool hasFoup);
            if (lpCode != LPCodes.SUCCESS)
            {
                this.WriteLog(MsgType.Error, "获取Foup状态失败, 不允许Load");
                return lpCode;
            }

            if (!hasFoup)
            {
                this.WriteLog(MsgType.Error, "该端口没有Foup，不允许Load");
                return LPCodes.LpLoadNoFoup;
            }

            return LPCodes.SUCCESS;
        }

        /// <summary>
        /// 判断某个端口是否可以下料
        /// 检查目标端口状态：端口空载、舱门状态正常、无安全联锁报警
        /// </summary>
        /// <returns></returns>
        private LPCodes CanLpUnload()
        {
            LPCodes lpCode = this.HasFoup(out bool hasFoup);
            if (lpCode != LPCodes.SUCCESS)
            {
                this.WriteLog(MsgType.Error, "获取Foup状态失败, 不允许Unload");
                return lpCode;
            }

            if (!hasFoup)
            {
                this.WriteLog(MsgType.Error, "该端口没有Foup，不允许Unload");
                return LPCodes.LpUnloadNoFoup;
            }

            return LPCodes.SUCCESS;
        }

        private LPCodes CanOHTLoad()
        {
            LPCodes lpCode = this.HasFoup(out bool hasFoup);
            if (lpCode != LPCodes.SUCCESS)
            {
                this.WriteLog(MsgType.Error, "获取Foup状态失败, 不允许OHTLoad");
                return lpCode;
            }

            if (hasFoup)
            {
                this.WriteLog(MsgType.Error, "该端口有Foup，不允许Load");
                return LPCodes.OHTLoadHasFoup;
            }

            return LPCodes.SUCCESS;
        }

        private LPCodes CanOHTUnload()
        {
            LPCodes lpCode = this.HasFoup(out bool hasFoup);
            if (lpCode != LPCodes.SUCCESS)
            {
                this.WriteLog(MsgType.Error, "获取Foup状态失败, 不允许OHTLoad");
                return lpCode;
            }

            if (!hasFoup)
            {
                this.WriteLog(MsgType.Error, "该端口没有Foup，不允许Unload");
                return LPCodes.OHTUnloadNoFoup;
            }

            return LPCodes.SUCCESS;
        }

        #endregion

        #region 公开接口

        public void Connect()
        {
            this.OnConnected?.Invoke(new ConnectedStsArgs() { ConnectedSts = false, DeviceName = this.DeviceName });



            this.OpenTcpClient();
        }

        public void Disconnect()
        {
            this.CloseTcpClient();
        }

        /// <summary>
        /// 开始执行天车上料动作（并不会马上就上料，要等天车的信号）
        /// 如果此时天车还没过来，那么会一直等待天车过来，直到上料结束或者有错误发生
        /// </summary>
        public LPCodes OHTLoadAsync(OHTLoadOptions loadOptions)
        {
            // 判断当前是否正在上料或者下料
            if (this.loadType != LoadTypes.None)
            {
                this.WriteLog(MsgType.Error, "OHTLoadAsync失败, LoadPort不是空闲状态, {0}", this.loadType);
                return LPCodes.LpIsRunning;
            }

            // 判断端口状态是否可以上料
            LPCodes lpCode = this.CanOHTLoad();
            if (lpCode != LPCodes.SUCCESS)
            {
                this.WriteLog(MsgType.Error, "StartOHTLoad失败, 当前状态不允许Load");
                return lpCode;
            }

            // 上料
            loadOptions.LoadType = LoadTypes.OHTLoad;
            this.WriteLog(MsgType.Normal, "开始执行：OHTLoadAsync");
            this.loadType = LoadTypes.OHTLoad;
            this.task = Task.Factory.StartNew(this.OHTLoad, loadOptions);
            return LPCodes.SUCCESS;
        }

        /// <summary>
        /// 开始执行天车下料动作（并不会马上就下料，要等天车的信号）
        /// 如果此时天车还没过来，那么会一直等待天车过来取料，直到下料结束或者有错误发生
        /// </summary>
        public LPCodes OHTUnloadAsync()
        {
            // 判断当前是否正在上料或者下料
            if (this.loadType != LoadTypes.None)
            {
                this.WriteLog(MsgType.Error, "OHTUnloadAsync失败, LoadPort不是空闲状态, {0}", this.loadType);
                return LPCodes.LpIsRunning;
            }

            // 判断端口状态是否可以下料
            LPCodes lpCode = this.CanOHTUnload();
            if (lpCode != LPCodes.SUCCESS)
            {
                this.WriteLog(MsgType.Error, "OHTUnloadAsync失败, 当前状态不允许Unload");
                return lpCode;
            }

            // 下料
            this.WriteLog(MsgType.Normal, "开始执行：OHTUnloadAsync");
            this.loadType = LoadTypes.OHTUnload;
            this.task = Task.Factory.StartNew(this.OHTLoad, new OHTLoadOptions() { LoadType = LoadTypes.OHTUnload });
            return LPCodes.SUCCESS;
        }

        /// <summary>
        /// 开始执行LoadPort的上料和Mapping动作
        /// </summary>
        /// <returns></returns>
        public LPCodes LpLoadAsync()
        {
            if (this.loadType != LoadTypes.None)
            {
                this.WriteLog(MsgType.Error, "LpLoadAsync失败, LoadPort不是空闲状态, {0}", this.loadType);
                return LPCodes.LpIsRunning;
            }

            LPCodes lpCode = this.CanLpLoad();
            if (lpCode != LPCodes.SUCCESS)
            {
                this.WriteLog(MsgType.Error, "LpLoadAsync失败, 当前状态不可以Load");
                return lpCode;
            }

            this.WriteLog(MsgType.Normal, "开始执行：LpLoadAsync");
            this.loadType = LoadTypes.LpLoad;

            Task.Factory.StartNew(() =>
            {
                List<SlotStates> slotStates = null;

                try
                {
                    lpCode = this.LpLoad(out slotStates);
                    if (lpCode != LPCodes.SUCCESS)
                    {
                        this.LoadStatusChanged?.Invoke(this.lpParam.Num, LoadTypes.LpLoad, lpCode, null);
                        this.WriteLog(MsgType.Error, "StartLpLoad失败, LpCode = {0}, Message = {1}", lpCode, LpUtils.GetErrorMessage(lpCode));
                    }
                    else
                    {
                        this.LoadStatusChanged?.Invoke(this.lpParam.Num, LoadTypes.LpLoad, lpCode, slotStates);
                        this.WriteLog(MsgType.Normal, "StartLpLoad成功");
                    }
                }
                catch (Exception ex)
                {
                    this.WriteLog(MsgType.Error, "StartLpLoad异常, {0}", ex);
                    this.LoadStatusChanged?.Invoke(this.lpParam.Num, LoadTypes.LpLoad, LPCodes.EXCEPTION, null);
                }
                finally
                {
                    this.loadType = LoadTypes.None;
                }
            });

            return LPCodes.SUCCESS;
        }

        /// <summary>
        /// 开始执行LoadPort的下料动作
        /// </summary>
        /// <returns></returns>
        public LPCodes LpUnloadAsync()
        {
            if (this.loadType != LoadTypes.None)
            {
                this.WriteLog(MsgType.Error, "LpUnloadAsync失败, LoadPort不是空闲状态, {0}", this.loadType);
                return LPCodes.LpIsRunning;
            }

            LPCodes lpCode = this.CanLpUnload();
            if (lpCode != LPCodes.SUCCESS)
            {
                this.WriteLog(MsgType.Error, "LpUnloadAsync失败, 当前状态不可以Unload");
                return lpCode;
            }

            this.WriteLog(MsgType.Normal, "开始执行：LpUnloadAsync");
            this.loadType = LoadTypes.LpUnload;

            Task.Factory.StartNew(() =>
            {
                try
                {
                    lpCode = this.LpUnload();
                    if (lpCode != LPCodes.SUCCESS)
                    {
                        this.LoadStatusChanged?.Invoke(this.lpParam.Num, LoadTypes.LpUnload, lpCode, null);
                        this.WriteLog(MsgType.Error, "StartLpUnload失败, LpCode = {0}, Message = {1}", lpCode, LpUtils.GetErrorMessage(lpCode));
                    }
                    else
                    {
                        this.LoadStatusChanged?.Invoke(this.lpParam.Num, LoadTypes.LpUnload, lpCode, null);
                        this.WriteLog(MsgType.Normal, "StartLpUnload成功");
                    }
                }
                catch (Exception ex)
                {
                    this.WriteLog(MsgType.Error, "StartLpUnload异常, {0}", ex);
                    this.LoadStatusChanged?.Invoke(this.lpParam.Num, LoadTypes.LpUnload, LPCodes.EXCEPTION, null);
                }
                finally
                {
                    this.loadType = LoadTypes.None;
                }
            });

            return LPCodes.SUCCESS;
        }

        /// <summary>
        /// 切换载具类型
        /// Casstle类型不需要定位柱，要把定位柱移下去
        /// Foup类型需要定位柱，要把定位柱移上来
        /// </summary>
        /// <param name="foupType"></param>
        /// <returns></returns>
        public LPCodes SetFoupType(FoupTypes foupType)
        {
            if (this.foupType == foupType)
            {
                return LPCodes.SUCCESS;
            }

            LPCodes lpCode = this.HasFoup(out bool hasFoup);
            if (lpCode != LPCodes.SUCCESS)
            {
                return lpCode;
            }

            if (hasFoup)
            {
                return LPCodes.CannotSwitchFoupType;
            }

            lpCode = this.FOUPTYPE((int)foupType);
            if (lpCode != LPCodes.SUCCESS)
            {
                return lpCode;
            }

            lpCode = this.ABORG();
            if (lpCode != LPCodes.SUCCESS)
            {
                return lpCode;
            }

            this.foupType = foupType;

            return LPCodes.SUCCESS;
        }

        /// <summary>
        /// 判断LoadPort上是否有Foup
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public LPCodes HasFoup(out bool hasFoup)
        {
            hasFoup = false;

            // 通过传感器信号判断Foup是否在位
            LPCodes lpCode = this.FOUPSTATE(out List<bool> states);
            if (lpCode != LPCodes.SUCCESS)
            {
                return lpCode;
            }

            hasFoup = states.All(v => v);

            return LPCodes.SUCCESS;
        }

        #region SDK接口

        /// <summary>
        /// 获取设备状态
        /// </summary>
        /// <returns></returns>
        public LPCodes STATE(out List<int> states)
        {
            states = null;
            LPCodes lpCode = LPCodes.SUCCESS;
            lpCode = this.SendGetMessage("s00GET:STATE;", out string result);
            states = result.Select(v => Convert.ToInt32(v.ToString())).ToList();
            return lpCode;
        }

        /// <summary>
        /// 获取料盒状态
        /// LoadPort上的几个光电传感器的状态，顺序是从左上到右下
        /// </summary>
        /// <returns></returns>
        public LPCodes FOUPSTATE(out List<bool> states)
        {
            states = null;
            LPCodes lpCode = LPCodes.SUCCESS;
            lpCode = this.SendGetMessage("s00GET:FOUPSTATE;", out string result);
            List<bool> bools = result.Select(v => v == '0' ? false : true).ToList();
            if (bools.Count > 0)
            {
                states = new List<bool>();
                states.Add(bools[3]);
                states.Add(bools[1]);
                states.Add(bools[2]);
                states.Add(bools[5]);
                states.Add(bools[6]);
                states.Add(bools[0]);
                states.Add(bools[7]);
                states.Add(bools[8]);
                states.Add(bools[4]);
            }

            return lpCode;
        }

        /// <summary>
        /// 获取指示灯状态
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public LPCodes LEDST()
        {
            return this.SendGetMessage("s00GET:LEDST;", out string getResult);
        }

        /// <summary>
        /// 获取Mapping数据(降序)
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public LPCodes MAPDT(out List<SlotStates> mapData)
        {
            mapData = null;
            LPCodes lpCode = this.SendGetMessage("s00GET:MAPDT;", out string result);
            mapData = result.Select(v => (SlotStates)Convert.ToInt32(v.ToString())).ToList();
            return lpCode;
        }

        /// <summary>
        /// 获取Mapping数据(升序)
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public LPCodes MAPRD(out List<SlotStates> mapData)
        {
            mapData = null;
            LPCodes lpCode = this.SendGetMessage("s00GET:MAPRD;", out string result);
            mapData = result.Select(v => (SlotStates)Convert.ToInt32(v.ToString())).ToList();
            return lpCode;
        }

        /// <summary>
        /// 获取正常晶圆数量
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public LPCodes WFCNT(out int cnt)
        {
            cnt = 0;
            LPCodes lpCode = this.SendGetMessage("s00GET:WFCNT;", out string result);
            if (lpCode == LPCodes.SUCCESS)
            {
                cnt = Convert.ToInt32(result);
            }
            return lpCode;
        }

        /// <summary>
        /// 获取版本信息
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public LPCodes VERSN(out string version)
        {
            version = string.Empty;
            LPCodes lpCode = this.SendGetMessage("s00GET:VERSN;", out string result);
            version = result;
            return lpCode;
        }

        /// <summary>
        /// 清除错误
        /// </summary>
        /// <returns></returns>
        public LPCodes RESET()
        {
            return this.SendControlMessage("s00SET:RESET;");
        }

        /// <summary>
        /// 设置料盒类型
        /// </summary>
        /// <param name="type">0表示Foup，1表示Casstle</param>
        /// <returns></returns>
        public LPCodes FOUPTYPE(int type)
        {
            string request = string.Format("s00SET:FOUPTYPE/{0};", type);
            LPCodes lpCode = this.PerformRequest(request, false, out string message1, out string message2);
            if (lpCode != LPCodes.SUCCESS)
            {
                return lpCode;
            }

            this.WriteLog(MsgType.Normal, "接收:{0}", message1);

            if (message1 == "s00:INTER/ERROR;")
            {
                return LPCodes.ERROR;
            }
            else if (message1 == string.Format("s00ACK:FOUPTYPE/{0};", type))
            {
                return LPCodes.SUCCESS;
            }
            else
            {
                return LPCodes.InvalidMessageFormat;
            }
        }



        /// <summary>
        /// 移动至原点
        /// </summary>
        /// <returns></returns>
        public LPCodes ORGSH()
        {
            return this.SendControlMessage("s00MOV:ORGSH;");
        }

        /// <summary>
        /// 强制初始化
        /// </summary>
        /// <returns></returns>
        public LPCodes ABORG()
        {
            return this.SendControlMessage("s00MOV:ABORG;");
        }

        /// <summary>
        /// 装载
        /// </summary>
        /// <returns></returns>
        public LPCodes CLOAD()
        {
            return this.SendControlMessage("s00MOV:CLOAD;");
        }

        /// <summary>
        /// 装载
        /// (至门打开前步骤)
        /// </summary>
        /// <returns></returns>
        public LPCodes CLDDK()
        {
            return this.SendControlMessage("s00MOV:CLDDK;");
        }

        /// <summary>
        /// 装载
        /// (从门打开步骤开始执行)
        /// </summary>
        /// <returns></returns>
        public LPCodes CLDOP()
        {
            return this.SendControlMessage("s00MOV:CLDOP;");
        }

        /// <summary>
        /// 装载并执行Mapping
        /// </summary>
        /// <returns></returns>
        public LPCodes CLDMP()
        {
            return this.SendControlMessage("s00MOV:CLDMP;");
        }

        /// <summary>
        /// 装载并执行
        /// Mapping(从门打开步骤开始执行)
        /// </summary>
        /// <returns></returns>
        public LPCodes CLMPO()
        {
            return this.SendControlMessage("s00MOV:CLMPO;");
        }

        /// <summary>
        /// 卸载
        /// </summary>
        /// <returns></returns>
        public LPCodes CULOD()
        {
            return this.SendControlMessage("s00MOV:CULOD;");
        }

        /// <summary>
        /// 卸载(至门关闭前步骤)
        /// </summary>
        /// <returns></returns>
        public LPCodes CULDK()
        {
            return this.SendControlMessage("s00MOV:CULDK;");
        }

        /// <summary>
        /// 卸载(至真空关闭步骤)
        /// </summary>
        /// <returns></returns>
        public LPCodes CULYD()
        {
            return this.SendControlMessage("s00MOV:CULYD;");
        }

        /// <summary>
        /// 卸载(从门关闭步骤到底座解锁前)
        /// </summary>
        /// <returns></returns>
        public LPCodes CUDCL()
        {
            return this.SendControlMessage("s00MOV:CUDCL;");
        }

        /// <summary>
        /// 卸载(从门关闭步骤开始执行)
        /// </summary>
        /// <returns></returns>
        public LPCodes CUDNC()
        {
            return this.SendControlMessage("s00MOV:CUDNC;");
        }

        /// <summary>
        /// 卸载(执行到底座解锁前)
        /// </summary>
        /// <returns></returns>
        public LPCodes CULFC()
        {
            return this.SendControlMessage("s00MOV:CULFC;");
        }

        /// <summary>
        /// 已装载完成状态下Mapping
        /// </summary>
        /// <returns></returns>
        public LPCodes MAPDO()
        {
            return this.SendControlMessage("s00MOV:MAPDO;");
        }

        /// <summary>
        /// 重新Mapping
        /// </summary>
        /// <returns></returns>
        public LPCodes REMAP()
        {
            return this.SendControlMessage("s00MOV:REMAP;");
        }

        /// <summary>
        /// 卸载且Mapping
        /// </summary>
        /// <returns></returns>
        public LPCodes CUDMP()
        {
            return this.SendControlMessage("s00MOV:CUDMP;");
        }

        /// <summary>
        /// 卸载且Mapping(步骤执行到门关闭之前)
        /// </summary>
        /// <returns></returns>
        public LPCodes CUMDK()
        {
            return this.SendControlMessage("s00MOV:CUMDK;");
        }

        /// <summary>
        /// 卸载且Mapping(步骤执行到底座解锁之前)
        /// </summary>
        /// <returns></returns>
        public LPCodes CUMFC()
        {
            return this.SendControlMessage("s00MOV:CUMFC;");
        }

        #region 单一动作

        /// <summary>
        /// 底座解锁
        /// </summary>
        /// <returns></returns>
        public LPCodes PODOP()
        {
            return this.SendControlMessage("s00MOV:PODOP;");
        }

        /// <summary>
        /// 底座上锁
        /// </summary>
        /// <returns></returns>
        public LPCodes PODCL()
        {
            return this.SendControlMessage("s00MOV:PODCL;");
        }

        /// <summary>
        /// 吸盘真空开
        /// </summary>
        /// <returns></returns>
        public LPCodes VACON()
        {
            return this.SendControlMessage("s00MOV:VACON;");
        }

        /// <summary>
        /// 吸盘真空关
        /// </summary>
        /// <returns></returns>
        public LPCodes VACOF()
        {
            return this.SendControlMessage("s00MOV:VACOF;");
        }

        /// <summary>
        /// 料盒盖解锁
        /// </summary>
        /// <returns></returns>
        public LPCodes DOROP()
        {
            return this.SendControlMessage("s00MOV:DOROP;");
        }

        /// <summary>
        /// 料盒盖上锁
        /// </summary>
        /// <returns></returns>
        public LPCodes DORCL()
        {
            return this.SendControlMessage("s00MOV:DORCL;");
        }

        /// <summary>
        /// 门上升到开门位
        /// </summary>
        /// <returns></returns>
        public LPCodes ZDRUP()
        {
            return this.SendControlMessage("s00MOV:ZDRUP;");
        }

        /// <summary>
        /// Mapping并移动到Mapping结束位
        /// </summary>
        /// <returns></returns>
        public LPCodes ZDRMP()
        {
            return this.SendControlMessage("s00MOV:ZDRMP;");
        }

        /// <summary>
        /// 门下降到待机位
        /// </summary>
        /// <returns></returns>
        public LPCodes ZDRDW()
        {
            return this.SendControlMessage("s00MOV:ZDRDW;");
        }

        /// <summary>
        /// 装载平台后退
        /// </summary>
        /// <returns></returns>
        public LPCodes YWAIT()
        {
            return this.SendControlMessage("s00MOV:YWAIT;");
        }

        /// <summary>
        /// 装载平台前进
        /// </summary>
        /// <returns></returns>
        public LPCodes YDOOR()
        {
            return this.SendControlMessage("s00MOV:YDOOR;");
        }

        /// <summary>
        /// 门打开(料盒盖后仰) 
        /// </summary>
        /// <returns></returns>
        public LPCodes DORBK()
        {
            return this.SendControlMessage("s00MOV:DORBK;");
        }

        /// <summary>
        /// 门关闭(料盒盖前倾)
        /// </summary>
        /// <returns></returns>
        public LPCodes DORFW()
        {
            return this.SendControlMessage("s00MOV:DORFW;");
        }

        /// <summary>
        /// MapArm伸出
        /// </summary>
        /// <returns></returns>
        public LPCodes MAPOP()
        {
            return this.SendControlMessage("s00MOV:MAPOP;");
        }

        /// <summary>
        /// MapArm缩回
        /// </summary>
        /// <returns></returns>
        public LPCodes MAPCL()
        {
            return this.SendControlMessage("s00MOV:MAPCL;");
        }

        /// <summary>
        /// 上升到Mapping起始位
        /// </summary>
        /// <returns></returns>
        public LPCodes ZMPST()
        {
            return this.SendControlMessage("s00MOV:ZMPST;");
        }

        /// <summary>
        /// 下降到Mapping结束位
        /// </summary>
        /// <returns></returns>
        public LPCodes ZMPED()
        {
            return this.SendControlMessage("s00MOV:ZMPED;");
        }

        #endregion

        #region 模式切换

        /// <summary>
        /// 自动模式
        /// 自动模式下才可执行复杂操作，
        /// 自动模式下自动启用互锁
        /// </summary>
        /// <returns></returns>
        public LPCodes ONMGV()
        {
            return this.SendSetMessage("s00MOD:ONMGV;");
        }

        /// <summary>
        /// 手动模式
        /// 手动模式下才可进行单步操作
        /// </summary>
        /// <returns></returns>
        public LPCodes MENTE()
        {
            return this.SendSetMessage("s00MOD:MENTE;");
        }

        /// <summary>
        /// 解除互锁
        /// 解除互锁条件后，无安全卡控，非专业人员请勿使用
        /// </summary>
        /// <returns></returns>
        public LPCodes UNLOCK()
        {
            return this.SendSetMessage("s00MOD:UNLOCK;");
        }

        #endregion

        #endregion

        #endregion

        #region 事件处理器

        private void OnClientConnected(IAsyncResult iar)
        {
            if (this.tcpClient == null)
            {
                // Unint的时候如果还没连上会触发
                return;
            }

            try
            {
                this.tcpClient.EndConnect(iar);
            }
            catch (Exception ex)
            {
                this.WriteLog(MsgType.Error, "网口连接失败, IPAddress = {0}, Port = {1}, {2}", this.lpParam.IPAddress, this.lpParam.Port, ex);
                this.CloseTcpClient();
                this.OpenTcpClient();
                return;
            }

            NetworkStream stream = this.tcpClient.GetStream();
            this.writer = new StreamWriter(stream);
            this.writer.AutoFlush = true;
            this.reader = new StreamReader(stream);

            this.WriteLog(MsgType.Normal, "网口连接成功, IPAddress = {0}, Port = {1}", this.lpParam.IPAddress, this.lpParam.Port);
            this.OnConnected?.Invoke(new ConnectedStsArgs() { ConnectedSts = true, DeviceName = this.lpParam.DeviceName });
        }

        #endregion
    }
}
