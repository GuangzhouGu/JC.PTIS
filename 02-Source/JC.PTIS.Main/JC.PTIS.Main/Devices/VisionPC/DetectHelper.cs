using JC.PTIS.Main.Devices.VisionPC;
using JC.PTIS.Main.Devices.VisionPC.MsgModel;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.Params;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using StdLib.HTCommunication.SocketWrapper;
using StdLib.HTFile;
using StdLib.LogHelper;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.PCBaseFrame.Extension.Device;
using StdLib.PCBaseFrame.Extension.Entities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Devices.Vision
{
    internal class DetectHelper : IDevice
    {
        public string DeviceName { get; set; } = "DetectPC";
        public event Action<ConnectedStsArgs> OnConnected;
        public event Action<MessageArgs> OnShowMsg;
        string FileName = @"Config\Hardware\DetectPCInfo.xml";
        ServerInfo ServerInfo = null;
        HTCOMMSocketClient Client = null;
        public static DetectHelper Instance { get { return DeviceManager.Instance.Get("DetectPC") as DetectHelper; } }
        Dictionary<string, MessgeStruct> ResponseDic = new Dictionary<string, MessgeStruct>();
        Dictionary<string, ManualResetEvent> MreDic = new Dictionary<string, ManualResetEvent>();
        public static event Action<DetectionCompletedBody> OnDetectionCompleted;
        Random Rdm = new Random();
        public void Init()
        {
            try
            {
                if (GetInfo())
                {
                    Client = new HTCOMMSocketClient();
                    Client.ClientConnected += Client_ClientConnected;
                    Client.ClientDisconnected += Client_ClientDisconnected;
                    Client.DataReceived += DataReceived;
                    Client.Connect(ServerInfo.IP, ServerInfo.Port);
                }
                else
                {
                    OnShowMsg?.Invoke(new MessageArgs() { LogName = this.GetType().Name, Message = $"DetectHelper->未找到{FileName}文件，无法连接检测PC", MsgType = MsgType.Error });
                }
            }
            catch (Exception ex)
            {
                OnConnected?.Invoke(new ConnectedStsArgs { DeviceName = ServerInfo.Desc, ConnectedSts = false });
                //OnShowMsg?.Invoke(new MessageArgs() { LogName = this.GetType().Name, Message = $"DetectHelper->Init方法异常：{ex}", MsgType = MsgType.Error });
            }
        }

        public T Detect<T>(VisionCmd cmd, DetectStationEnum type, IMsgBody body, bool isWaitReply = true)
        {
            T ret = default(T);
            var key = cmd.ToString();
            try
            {
                if (Client != null)
                {
                    if (cmd == VisionCmd.TriggerDetection)
                    {
                        key = $"{cmd}_{type}";

                        var key2 = $"{VisionCmd.DetectionCompleted}_{type}";
                        if (ResponseDic.Keys.Contains(key2))
                        {
                            ResponseDic[key2] = null;
                        }
                        else
                        {
                            ResponseDic.Add(key2, null);
                        }
                    }
                    if (ResponseDic.Keys.Contains(key))
                    {
                        ResponseDic[key] = null;
                    }
                    else
                    {
                        ResponseDic.Add(key, null);
                    }
                    if (MreDic.Keys.Contains(key))
                    {
                        MreDic[key] = new ManualResetEvent(false);
                    }
                    else
                    {
                        MreDic.Add(key, new ManualResetEvent(false));
                    }
                    MessgeStruct messgeStruct = new MessgeStruct();
                    messgeStruct.Header = new Header(cmd);
                    messgeStruct.Body = body;
                    var jsonStr = JsonConvert.SerializeObject(messgeStruct, Formatting.Indented) + "@";
                    byte[] data = Encoding.Default.GetBytes(jsonStr);
                    LogManager.GetLogger(this.GetType().Name).Info($"Local->DetectPC：{jsonStr}");
                    MreDic[key].Reset();
                    if (ParaHelper.Global.IsDetectPCEnable == false)
                    {
                        messgeStruct.Header.Cmd = cmd.ToString();
                        messgeStruct.Header.MsgType = "Response";
                        if (cmd == VisionCmd.TriggerDetection)
                        {
                            messgeStruct.Body = new TriggerDetectReqBody
                            {
                                Code = 0,
                                Station = type.ToString()
                            };
                        }
                        else if (cmd == VisionCmd.ChangeRecipe)
                        {
                            messgeStruct.Body = new ChangeRecipeReqBody
                            {
                                Code = 0
                            };
                        }
                        jsonStr = JsonConvert.SerializeObject(messgeStruct, Formatting.Indented) + "@";
                        data = Encoding.Default.GetBytes(jsonStr);
                        DataAnalyse(data);

                        if (Global.RunMode == RunMode.空跑模式)
                        {
                            Task.Run(() =>
                            {
                                if (type == DetectStationEnum.IRDetect || type == DetectStationEnum.BackDetect)
                                {
                                    Thread.Sleep(600);
                                }
                                else
                                {
                                    Thread.Sleep(10000);
                                }
                                var result = GenSimultor((TriggerDetectReqBody)body, type);
                                messgeStruct.Body = result;
                                messgeStruct.Header.Cmd = VisionCmd.DetectionCompleted.ToString();
                                jsonStr = JsonConvert.SerializeObject(messgeStruct, Formatting.Indented) + "@";
                                data = Encoding.Default.GetBytes(jsonStr);
                                DataAnalyse(data);
                            });
                        }

                    }
                    else
                    {
                        SendData(data);
                    }
                    if (isWaitReply)
                    {
                        var temp = MreDic[key].WaitOne(3000);
                        if (temp && ResponseDic[key] != null && ResponseDic[key].Body != null)
                        {
                            ret = (T)ResponseDic[key].Body;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OnShowMsg?.Invoke(new MessageArgs() { LogName = this.GetType().Name, Message = $"DetectHelper->Detect方法异常：{ex.ToString()}", MsgType = MsgType.Error });
            }
            return ret;
        }
        public DetectionCompletedBody GetDetectionResult(DetectStationEnum type)
        {
            DetectionCompletedBody ret = null;
            var key = $"{VisionCmd.DetectionCompleted}_{type}";
            if (ResponseDic.Keys.Contains(key))
            {
                var temp = ResponseDic[key].Body as DetectionCompletedBody;
                ret = temp.DeepCopy();
                ResponseDic[key] = null;
            }
            return ret;
        }
        private void SendData(byte[] data)
        {
            try
            {
                Client?.Send(data);
            }
            catch (Exception ex)
            {
                OnShowMsg?.Invoke(new MessageArgs() { LogName = this.GetType().Name, Message = $"SendData方法异常：{ex}", MsgType = MsgType.Error });
            }
        }
        private void DataReceived(object sender, SocketEventArgs e)
        {
            try
            {
                if (e.Datas != null)
                {
                    DataAnalyse(e.Datas);
                }
            }
            catch (Exception ex)
            {
                OnShowMsg?.Invoke(new MessageArgs() { LogName = this.GetType().Name, Message = $"DataReceived方法异常：{ex}", MsgType = MsgType.Error });
            }
        }
        private void DataAnalyse(byte[] datas)
        {
            try
            {
                if (datas != null)
                {
                    string key = string.Empty;
                    var rawStr = Encoding.Default.GetString(datas);
                    if (string.IsNullOrEmpty(rawStr) == false)
                    {
                        var arr = rawStr.Split(new char[] { '@' }, StringSplitOptions.RemoveEmptyEntries);
                        if (arr.Length > 0)
                        {
                            foreach (var item in arr)
                            {
                                if (string.IsNullOrEmpty(item) == false)
                                {
                                    var envelope = JObject.Parse(item);
                                    var header = envelope["Header"]?.ToObject<Header>();
                                    key = header.Cmd.ToString();
                                    var cmdEnum = EnumHelper.GetEnumValue<VisionCmd>(header.Cmd);
                                    Type bodyType;
                                    if (header.Cmd == VisionCmd.DetectionCompleted.ToString())
                                    {
                                        bodyType = typeof(DetectionCompletedBody);
                                    }
                                    else if (header.Cmd == VisionCmd.TriggerDetection.ToString())
                                    {
                                        bodyType = typeof(TriggerDetectRspBody);
                                    }
                                    else
                                    {
                                        bodyType = typeof(BodyBase);
                                    }
                                    var message = new MessgeStruct
                                    {
                                        Header = header,
                                        Body = envelope["Body"]?.ToObject(bodyType) as IMsgBody
                                    };
                                    if (message != null && message.Body != null)
                                    {
                                        if (cmdEnum != VisionCmd.Heartbeat)
                                        {
                                            key = $"{header.Cmd}_{message.Body.Station}";
                                            if (cmdEnum == VisionCmd.DetectionCompleted)
                                            {
                                                OnDetectionCompleted?.Invoke(message.Body as DetectionCompletedBody);
                                            }
                                            if (MreDic.Keys.Contains(key))
                                            {
                                                MreDic[key].Set();
                                            }
                                            ResponseDic[key] = message;
                                            LogManager.GetLogger(this.GetType().Name).Info($"DetectPC->Local：{item}");
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OnShowMsg?.Invoke(new MessageArgs() { LogName = this.GetType().Name, Message = $"DataAnalyse方法异常：{ex}", MsgType = MsgType.Error });
            }
        }
        private void Client_ClientDisconnected(object sender, SocketEventArgs e)
        {
            try
            {
                OnConnected?.Invoke(new ConnectedStsArgs() { DeviceName = ServerInfo.Desc, ConnectedSts = false });
            }
            catch (Exception ex)
            {

            }
        }

        private void Client_ClientConnected(object sender, SocketEventArgs e)
        {
            try
            {
                OnConnected?.Invoke(new ConnectedStsArgs() { DeviceName = ServerInfo.Desc, ConnectedSts = true });
            }
            catch (Exception ex)
            {

            }
        }

        private DetectionCompletedBody GenSimultor(TriggerDetectReqBody body, DetectStationEnum typeEnum)
        {
            try
            {
                //生成测试数据
                DetectionCompletedBody ret = new DetectionCompletedBody();
                ret.Code = 0;
                ret.Station = typeEnum.ToString();
                ret.LotId = "LotId";
                ret.BatchCount = 1;
                ret.TotalCostTime = 1000;
                ret.ItemResultList = new ItemResult[body.ProductList.Length];
                for (int i = 0; i < ret.ItemResultList.Length; i++)
                {
                    var value = Rdm.Next(0, 100);
                    ret.ItemResultList[i] = new ItemResult
                    {
                        Index = body.ProductList[i].Index,
                        SN = body.ProductList[i].SN,
                        Result = value > 95 ? "NG" : "OK",
                    };
                    if (ret.ItemResultList[i].Result == "NG")
                    {
                        ret.ItemResultList[i].Defects = new Defect[1];
                        for (int j = 0; j < ret.ItemResultList[i].Defects.Length; j++)
                        {
                            ret.ItemResultList[i].Defects[j] = new Defect
                            {
                                DefectId = $"DefectId_{j}",
                                DefectType = $"DefectType_{j}",
                                DefectSize = $"DefectSize_{j}",
                                DefectImageUrl = $"DefectImageUrl_{j}"
                            };
                        }
                    }
                }
                return ret;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public void Uninit()
        {
            Client?.Disconnect();
        }
        private bool GetInfo()
        {
            if (File.Exists(FileName))
            {
                ServerInfo = HTXmlHelper.EntityFromXMLFile<ServerInfo>(FileName);
            }
            return ServerInfo != null;
        }
    }
}
