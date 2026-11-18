using JC.PTIS.Main.Devices.VisionPC;
using JC.PTIS.Main.Devices.VisionPC.MsgModel;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Params;
using JC.PTIS.Main.RotateInfo;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SqlSugar;
using StdLib.HTCommunication.SocketWrapper;
using StdLib.HTFile;
using StdLib.LogHelper;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.PCBaseFrame.Extension.Device;
using StdLib.PCBaseFrame.Extension.Entities;
using StdLib.PCBaseFrame.Motion;
using StdLib.WindowsControls.Class;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace JC.PTIS.Main.Devices.Vision
{
    public class AlignHelper : IDevice
    {
        public string DeviceName { get; set; } = "AlignPC";
        public event Action<ConnectedStsArgs> OnConnected;
        public event Action<MessageArgs> OnShowMsg;
        public static event Action<VisionCmd, BodyBase> Request;
        string FileName = @"Config\Hardware\AlignPCInfo.xml";
        ServerInfo ServerInfo = null;
        HTCOMMSocketClient Client = null;
        public static AlignHelper Instance { get { return DeviceManager.Instance.Get("AlignPC") as AlignHelper; } }
        Dictionary<string, MessgeStruct> ResponseDic = new Dictionary<string, MessgeStruct>();
        Dictionary<string, ManualResetEvent> MreDic = new Dictionary<string, ManualResetEvent>();
        MotionManager Motion = MotionManager.GetInstance();
        DateTime LastAliveTime = DateTime.Now;
        bool IsConnected = false;
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
                    Task.Run(() =>
                    {
                        while (true)
                        {
                            try
                            {
                                if (IsConnected == false)
                                {
                                    Client.Connect(ServerInfo.IP, ServerInfo.Port);
                                }
                                else
                                {
                                    //Heartbeat();
                                }
                            }
                            catch (Exception ex)
                            {
                            }
                            Thread.Sleep(2000);
                        }
                    });
                }
                else
                {
                    OnShowMsg?.Invoke(new MessageArgs() { LogName = this.GetType().Name, Message = $"AlignHelper->未找到{FileName}文件，无法连接定位PC", MsgType = MsgType.Error });
                }
            }
            catch (Exception ex)
            {
                OnShowMsg?.Invoke(new MessageArgs() { LogName = this.GetType().Name, Message = $"AlignHelper->Init方法异常：{ex}", MsgType = MsgType.Error });
            }
        }
        public void Heartbeat()
        {
            if (Client != null)
            {
                MessgeStruct messgeStruct = new MessgeStruct();
                messgeStruct.Header = new Header(VisionCmd.Heartbeat);
                var jsonStr = JsonConvert.SerializeObject(messgeStruct) + "@";
                byte[] data = Encoding.Default.GetBytes(jsonStr);
                SendData(data);
            }
        }

        /// <summary>
        /// Die对位，获取Die偏移值
        /// </summary>
        /// <param name="stnNo"></param>
        /// <returns></returns>
        public T Send<T>(VisionCmd cmd, AlignStationEnum station, int stnNo = 0)
        {
            T ret = default(T);
            var key = cmd.ToString();
            try
            {
                if (Client != null)
                {
                    if (station != AlignStationEnum.None &&
                        station.ToInt() != -1)
                    {
                        key = $"{cmd}_{station}";
                    }
                    else
                    {
                        key = $"{cmd}";
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
                    var axisPos = GetAxisPos(station);
                    switch (cmd)
                    {
                        case VisionCmd.Align:
                        case VisionCmd.Grab:
                            {
                                messgeStruct.Body = new AlignReqBody() { Station = station.ToString(), StnNo = stnNo, AxisPos = axisPos };
                            }
                            break;
                        case VisionCmd.DetectBelt:
                        case VisionCmd.DetectLabel:
                            {
                                messgeStruct.Body = new BodyBase() { Station = station.ToString(), StnNo = stnNo };
                            }
                            break;
                        default:
                            break;
                    }
                    var jsonStr = JsonConvert.SerializeObject(messgeStruct, Formatting.Indented) + "@";
                    byte[] data = Encoding.Default.GetBytes(jsonStr);
                    LogManager.GetLogger(this.GetType().Name).Info($"Local->AlignPC：{jsonStr}");
                    MreDic[key].Reset();
                    if (ParaHelper.Global.IsAlignPCEnable == false)
                    {
                        messgeStruct.Header.Cmd = cmd.ToString();
                        messgeStruct.Header.MsgType = "Response";
                        switch (cmd)
                        {
                            case VisionCmd.Grab:
                                {
                                    messgeStruct.Body = new BodyBase
                                    {
                                        Station = station.ToString(),
                                        StnNo = stnNo,
                                        Code = 0
                                    };
                                }
                                break;
                            case VisionCmd.Align:
                                {
                                    messgeStruct.Body = new AlignRspBody
                                    {
                                        Station = station.ToString(),
                                        StnNo = stnNo,
                                        Offset = new Xyt { X = 0.1, Y = 0.2, T = 0 }
                                    };
                                }
                                break;
                            case VisionCmd.DetectBelt:
                                {
                                    messgeStruct.Body = new BeltDetectRspBody
                                    {
                                        StnNo = stnNo,
                                        DetectResult = true
                                    };
                                }
                                break;
                            case VisionCmd.DetectLabel:
                                {
                                    messgeStruct.Body = new LabelDetectRspBody
                                    {
                                        StnNo = stnNo,
                                        DetectResult = true
                                    };
                                }
                                break;
                        }
                        jsonStr = JsonConvert.SerializeObject(messgeStruct, Formatting.Indented) + "@";
                        data = Encoding.Default.GetBytes(jsonStr);
                        DataAnalyse(data);
                    }
                    else
                    {
                        SendData(data);
                    }
                    var temp = MreDic[key].WaitOne(3000);
                    if (temp && ResponseDic[key] != null && ResponseDic[key].Body != null)
                    {
                        if (typeof(T) == typeof(AlignRspBody))
                        {
                            var body = ResponseDic[key].Body as AlignRspBody;
                            if (body.Offset != null)
                            {
                                if (!body.Offset.IsInRange(ParaHelper.Vision.AlignXRange, ParaHelper.Vision.AlignYRange, ParaHelper.Vision.AlignTRange))
                                {
                                    OnShowMsg?.Invoke(new MessageArgs() { LogName = this.GetType().Name, Message = $"AlignHelper->补正值超出范围，补正值：{body.ToString()}", MsgType = MsgType.Error });
                                    ResponseDic[key].Body = null;
                                }
                            }
                        }
                        if (ResponseDic[key].Body != null)
                        {
                            ret = (T)ResponseDic[key].Body;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                OnShowMsg?.Invoke(new MessageArgs() { LogName = this.GetType().Name, Message = $"AlignHelper->Send方法异常：{ex}", MsgType = MsgType.Error });
            }
            return ret;
        }
        public void Reply(VisionCmd cmd, BodyBase body, AlignStationEnum station)
        {
            var key = cmd.ToString();
            try
            {
                if (Client != null)
                {
                    MessgeStruct messgeStruct = new MessgeStruct();
                    messgeStruct.Header = new Header(cmd);
                    messgeStruct.Header.MsgType = "Reponse";
                    messgeStruct.Body = body;
                    var jsonStr = JsonConvert.SerializeObject(messgeStruct, Formatting.Indented) + "@";
                    byte[] data = Encoding.Default.GetBytes(jsonStr);
                    LogManager.GetLogger(this.GetType().Name).Info($"Local->AlignPC：{jsonStr}");
                    SendData(data);
                }
            }
            catch (Exception ex)
            {
                OnShowMsg?.Invoke(new MessageArgs() { LogName = this.GetType().Name, Message = $"AlignHelper->Send方法异常：{ex}", MsgType = MsgType.Error });
            }
        }

        public void Test(VisionCmd cmd, BodyBase body)
        {
            var key = cmd.ToString();
            try
            {
                if (Client != null)
                {
                    MessgeStruct messgeStruct = new MessgeStruct();
                    messgeStruct.Header = new Header(cmd);
                    messgeStruct.Header.Cmd = cmd.ToString();
                    messgeStruct.Header.MsgType = "Request";
                    messgeStruct.Body = body;
                    var jsonStr = JsonConvert.SerializeObject(messgeStruct, Formatting.Indented) + "@";
                    byte[] data = Encoding.Default.GetBytes(jsonStr);
                    DataAnalyse(data);
                }
            }
            catch (Exception ex)
            {
                OnShowMsg?.Invoke(new MessageArgs() { LogName = this.GetType().Name, Message = $"AlignHelper->Send方法异常：{ex}", MsgType = MsgType.Error });
            }
        }
        private void SendData(byte[] data)
        {
            try
            {
                if (IsConnected)
                {
                    Client?.Send(data);
                }
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
                                    Type bodyType = typeof(BodyBase);
                                    var cmdEnum = EnumHelper.GetEnumValue<VisionCmd>(header.Cmd);
                                    switch (cmdEnum)
                                    {
                                        case VisionCmd.Align:
                                            {
                                                bodyType = typeof(AlignRspBody);
                                            }
                                            break;
                                        case VisionCmd.Grab:
                                            {
                                                bodyType = typeof(BodyBase);
                                            }
                                            break;
                                        case VisionCmd.DetectBelt:
                                            {
                                                bodyType = typeof(BeltDetectRspBody);
                                            }
                                            break;
                                        case VisionCmd.DetectLabel:
                                            {
                                                bodyType = typeof(LabelDetectRspBody);
                                            }
                                            break;
                                        case VisionCmd.MotionRequest:
                                            {
                                                bodyType = typeof(MotionRequestReqBody);
                                            }
                                            break;
                                        case VisionCmd.AutoCalibration:
                                            {
                                                bodyType = typeof(AutoCalibrationReqBody);
                                            }
                                            break;
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
                                            LogManager.GetLogger(this.GetType().Name).Info($"AlignPC->Local：{item}");
                                            if (string.IsNullOrEmpty(message.Body.Station) == false)
                                            {
                                                key = $"{header.Cmd}_{message.Body.Station}";
                                            }
                                            if (cmdEnum == VisionCmd.MotionRequest)
                                            {
                                                var temp = envelope["Body"]?.ToObject(bodyType) as MotionRequestReqBody;
                                                Request?.Invoke(cmdEnum, temp);
                                            }
                                            else if (cmdEnum == VisionCmd.AutoCalibration)
                                            {
                                                var temp = envelope["Body"]?.ToObject(bodyType) as AutoCalibrationReqBody;
                                                Request?.Invoke(cmdEnum, temp);
                                            }
                                            else
                                            {
                                                if (MreDic.Keys.Contains(key))
                                                {
                                                    MreDic[key].Set();
                                                    ResponseDic[key] = message;
                                                }
                                            }
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
                IsConnected = false;
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
                IsConnected = true;
                OnConnected?.Invoke(new ConnectedStsArgs() { DeviceName = ServerInfo.Desc, ConnectedSts = true });
            }
            catch (Exception ex)
            {

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
        public Xyt GetAxisPos(AlignStationEnum typeEnum)
        {
            Xyt xyt = new Xyt();
            switch (typeEnum)
            {
                case AlignStationEnum.WaferMark:
                case AlignStationEnum.WaferEdge:
                case AlignStationEnum.WaferNotch:
                case AlignStationEnum.Die_WaferTable:
                    {
                        xyt.X = Motion.GetAxisActPos(AxisName.WaferTable_X);
                        xyt.Y = Motion.GetAxisActPos(AxisName.WaferTable_Y);
                        xyt.T = Motion.GetAxisActPos(AxisName.WaferTable_T);
                    }
                    break;
                case AlignStationEnum.Die_DieToTray:
                case AlignStationEnum.Tray_DieToTray:
                    {
                        var stageNo = RotateInfoHelper.Instance.GetStageNo(Stn4TurnTableStnEnum.DieToTray.ToInt(), RotateStnType.Stn4TurnTable);
                        var stage = StageManager.Get(stageNo);
                        xyt.X = Motion.GetAxisActPos(stage.X);
                        xyt.Y = Motion.GetAxisActPos(stage.Y);
                        xyt.T = Motion.GetAxisActPos(stage.T);
                    }
                    break;
                case AlignStationEnum.Die_PickPatch:
                case AlignStationEnum.Tray_PickPatch:
                    {
                        var stageNo = RotateInfoHelper.Instance.GetStageNo(Stn4TurnTableStnEnum.PickPatchStn.ToInt(), RotateStnType.Stn4TurnTable);
                        var stage = StageManager.Get(stageNo);
                        xyt.X = Motion.GetAxisActPos(stage.X);
                        xyt.Y = Motion.GetAxisActPos(stage.Y);
                        xyt.T = Motion.GetAxisActPos(stage.T);
                    }
                    break;
                case AlignStationEnum.Die_HalfOkTray:
                case AlignStationEnum.Tray_HalfOkTray:
                case AlignStationEnum.Die_HalfNgTray:
                case AlignStationEnum.Tray_HalfNgTray:
                    {
                        xyt.X = Motion.GetAxisActPos(AxisName.HalfTray_StageX);
                        xyt.Y = Motion.GetAxisActPos(AxisName.HalfTray_StageY);
                        xyt.T = Motion.GetAxisActPos(AxisName.HalfTray_StageT);
                    }
                    break;

                case AlignStationEnum.IsoPaper:
                    {
                        xyt.X = Motion.GetAxisActPos(AxisName.IsolationPaper_ArmX);
                        xyt.Y = Motion.GetAxisActPos(AxisName.IsolationPaper_ArmY);
                        xyt.T = Motion.GetAxisActPos(AxisName.IsolationPaper_ArmT);
                    }
                    break;
                case AlignStationEnum.Label:
                    {
                        xyt.X = Motion.GetAxisActPos(AxisName.Packing_ArmX);
                        xyt.Y = Motion.GetAxisActPos(AxisName.Packing_ArmY);
                        xyt.T = Motion.GetAxisActPos(AxisName.Packing_ArmT);
                    }
                    break;
            }
            return xyt;
        }
    }
    [Serializable]
    public class Xyt
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double T { get; set; }
        public List<double> ToList()
        {
            return new List<double>() { X, Y, T };
        }
        public double[] ToArray()
        {
            return new double[] { X, Y, T };
        }
        public bool IsInRange(double[] rangeX, double[] rangeY, double[] rangeT)
        {
            bool ret = true;
            if (rangeX != null && rangeX.Length == 2)
            {
                if (X < rangeX[0] || X > rangeX[1])
                {
                    ret = false;
                }
            }
            if (rangeY != null && rangeY.Length == 2)
            {
                if (Y < rangeY[0] || Y > rangeY[1])
                {
                    ret = false;
                }
            }
            if (rangeT != null && rangeT.Length == 2)
            {
                if (T < rangeT[0] || T > rangeT[1])
                {
                    ret = false;
                }
            }
            return ret;
        }
        public void FromList(List<double> list)
        {
            if (list != null && list.Count == 3)
            {
                X = list[0];
                Y = list[1];
                T = list[2];
            }
        }
        public void FromArray(double[] array)
        {
            if (array != null && array.Length == 3)
            {
                X = array[0];
                Y = array[1];
                T = array[2];
            }
        }
        public override string ToString()
        {
            return $"X:{X},Y:{Y},T:{T}";
        }
    }

}
