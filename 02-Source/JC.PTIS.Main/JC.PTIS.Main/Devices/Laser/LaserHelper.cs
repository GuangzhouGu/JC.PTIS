/*----------------------------------------------------------------
//公司名：   Suzhou HYC Technology Co.,LTD
//版权申明： Copyright © 2021 HYC. All Rights Reserved.
//
// =================================
// CLR版本     ：4.6.81.0
// 命名空间    ：HYC.SaturnMain.Devices.Laser
// 文件名称    ：LaserHelper.cs
// =================================
// 创 建 者    ：guguangzhou
// 创建日期    ：2022/8/19 09:00:00 
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
using JC.LidAttach.Devices.GlueDispenser;
using JC.PTIS.Main.Params;
using StdLib.HTCommunication.SerialPortWrapper;
using StdLib.HTFile;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.PCBaseFrame.Extension.Device;
using StdLib.PCBaseFrame.Extension.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace JC.LidAttach.Devices.Laser
{
    /// <summary>
    /// 激光位移传感器帮助类
    /// </summary>
    public class LaserHelper : IDevice
    {
        public event Action<ConnectedStsArgs> OnConnected;  //设备连接/断开消息
        public event Action<MessageArgs> OnShowMsg;
        public static event Action<int, double> OnShowValue;
        public static LaserHelper Instance { get { return DeviceManager.Instance.Get("LaserHelper") as LaserHelper; } set { } }
        private string FilePath = AppDomain.CurrentDomain.BaseDirectory + @"\Config\Hardware\LaserInfo.xml";
        public string DeviceName { get; set; } = "LaserHelper";
        List<LaserControl> List = new List<LaserControl>();
        public int Count { get { return List.Count; } }
        Random Ran = new Random();
        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            Task.Factory.StartNew(() =>
            {
                try
                {
                    var infos = GetInfo();
                    if (infos != null && infos.Count > 0)
                    {
                        foreach (var item in infos)
                        {
                            LaserControl laser = new LaserControl(item);
                            laser.OnConnected += OnConnected;
                            laser.OnShowMsg += OnShowMsg;
                            List.Add(laser);
                            laser.Connect();
                        }
                    }
                    else
                    {
                        OnShowMsg?.Invoke(new MessageArgs { LogName = "Laser", Message = $"LaserHelper->读取配置文件失败，文件为空", MsgType = MsgType.Normal });
                    }
                }
                catch (Exception ex)
                {
                }
            });
        }
        public double GetValue(int num=0)
        {
            double ret = -1;
            if (ParaHelper.Global.IsLaserEnable == false)
            {
                ret = Ran.Next(90, 100)/10.0;
                OnShowValue?.Invoke(num, ret);
            }
            else
            {
                var ctrl = GetLaser(num);
                if (ctrl != null)
                {
                    for (int i = 0; i < ParaHelper.Global.MeasureHeightCount; i++)
                    {
                        var value = ctrl.GetValue();
                        if (value != null)
                        {
                            ret = (double)value;
                            break;
                        }
                        Thread.Sleep(10);
                    }
                }
            }
            return ret;
        }
        public void Zero(int num)
        {
            var ctrl = GetLaser(num);
            if (ctrl != null)
            {
                 ctrl.Zero();
            }
        }
        private LaserControl GetLaser(int num)
        {
            LaserControl ret = null;
            if (List!=null&&List.Count>0)
            {
                var index=List.FindIndex(p=>p.Info.Num==num);
                if (index>=0)
                {
                    ret = List[index];
                }
            }
            return ret;
        }
        /// <summary>
        /// 获取信息
        /// </summary>
        /// <returns></returns>
        private List<LaserInfo> GetInfo()
        {
            var result = HTXmlHelper.EntityFromXMLFile<List<LaserInfo>>(FilePath);
            return result;
        }
        public void Uninit()
        {
            if (List!=null)
            {
                List.ForEach(p=>p.Uninit());
            }
        }

        public void InitRet(bool ret)
        {
        }

    }
  
}
