using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.Map.Devices.RFID
{
    public static class RfidUtils
    {
        public static string Code2Text(byte code1, byte code2)
        {
            if (code1 == '0' && code2 == '0')
            {
                return "执行成功";
            }
            else if (code1 == '1' && code2 == '4')
            {
                return "格式错误, 命令格式存在错误。（例如，命令部分未定义，或者页或地址规范不当。）";
            }
            else if (code1 == '7' && code2 == '0')
            {
                return "通信错误, 与射频标签的通信过程中发生噪声或其他妨碍，无法正常完成通信。";
            }
            else if (code1 == '7' && code2 == '1')
            {
                return "验证错误, 正确的数据无法写入到射频标签。";
            }
            else if (code1 == '7' && code2 == '2')
            {
                return "无标签错误, CIDRW头前面无射频标签，或者CIDRW头因环境因素（如噪声）无法检测射频标签。";
            }
            else if (code1 == '7' && code2 == 'B')
            {
                return "外部写入区域错误, 射频标签位于可读取但无法写入的位置，因此写入未正常完成。";
            }
            else if (code1 == '7' && code2 == 'E')
            {
                return "ID系统错误（1）, 射频标签处于无法执行命令处理的状态。";
            }
            else if (code1 == '7' && code2 == 'F')
            {
                return "ID系统错误（2）, 使用的射频标签不适用。";
            }
            else
            {
                return "未知错误码";
            }
        }
    }
}
