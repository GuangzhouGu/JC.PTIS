/*----------------------------------------------------------------
//公司名：   Suzhou HYC Technology Co.,LTD
//版权申明： Copyright © 2021 HYC. All Rights Reserved.
//
// =================================
// CLR版本     ：4.6.81.0
// 命名空间    ：TPAssembly.Devices.Keyence
// 文件名称    ：LaserInfo.cs
// =================================
// 创 建 者    ：guguangzhou
// 创建日期    ：2021/9/25 09:00:00 
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.LidAttach.Devices.Laser
{
    public class LaserInfo
    {
        public int Num { get; set; }
        public string PortName { get; set; }
        public int BaudRate { get; set; }
        public string Desc { get; set; }
    }
}
