/*----------------------------------------------------------------
//公司名：   Suzhou XXX Technology Co.,LTD
//版权申明： Copyright © 2024 XXX. All Rights Reserved.
//
// =================================
// CLR版本     ：4.6.81.0
// 命名空间    ：
// 文件名称    ：
// =================================
// 创 建 者    ：guguangzhou
// 创建日期    ：2024/07/06 09:00:00 
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
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.AutoMating.Entities
{
    public class FrmTipHelper
    {
        public enum TipsSizeMode
        {
            Small,
            Medium,
            Large,
            None
        }

        public enum TipsState
        {
            Default = -12542209,
            Success = -9977286,
            Info = -7299687,
            Warning = -693140,
            Error = -1097849
        }


        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        public static Form GetParentForm(Control c)
        {
            Control parent = c.Parent;
            if (1 == 0)
            {
            }

            Form result = ((parent == null) ? null : ((!(parent is Form form)) ? GetParentForm(c.Parent) : form));
            if (1 == 0)
            {
            }

            return result;
        }

        public static Color GetDeeperColor(Color baseColor, int diff = 32)
        {
            int num = baseColor.R - diff;
            int num2 = baseColor.G - diff;
            int num3 = baseColor.B - diff;
            num = ((num >= 0) ? num : 0);
            num2 = ((num2 >= 0) ? num2 : 0);
            num3 = ((num3 >= 0) ? num3 : 0);
            return Color.FromArgb(num, num2, num3);
        }

        public static Color GetLighterColor(Color baseColor, int diff = 32)
        {
            int num = baseColor.R + diff;
            int num2 = baseColor.G + diff;
            int num3 = baseColor.B + diff;
            num = ((num > 255) ? 255 : num);
            num2 = ((num2 > 255) ? 255 : num2);
            num3 = ((num3 > 255) ? 255 : num3);
            return Color.FromArgb(num, num2, num3);
        }

        public static int CoerceValue(int raw, int lo, int hi)
        {
            if (raw <= lo)
            {
                return lo;
            }

            if (raw >= hi)
            {
                return hi;
            }

            return raw;
        }
        public static Point GetScreenLocation(Screen screen, int left, int top)
        {
            return new Point(screen.Bounds.Left + left, screen.Bounds.Top + top);
        }
       
    }
}
