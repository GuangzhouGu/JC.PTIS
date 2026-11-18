using JC.PTIS.Main.Entities;
using JC.PTIS.Main.MotionInfo;
using StdLib.PCBaseFrame.Args;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.PCBaseFrame.Motion;
using StdLib.WindowsControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.PTIS.Main.UserCtrls.FuncTeach
{
    public partial class TeachCtrlBase : UserControl
    {
        public TeachCtrlBase()
        {
            InitializeComponent();
        }
        public MotionManager Motion { get; set; }=MotionManager.GetInstance();
        public StationName StationName { get; set; } = StationName.None;
        /// <summary>
        /// 轴是否可以移动
        /// </summary>
        /// <param name="axisList"></param>
        /// <param name="isHome">是否是回零操作</param>
        /// <returns></returns>
        public bool IsCanMove(List<AxisName> axisList,bool isHome=false)
        {
            bool ret = true;
            if (!InterlockMonitor.IsEQInStop())
            {
                UIMessageDialog.ShowErrorDialog(null, "设备不在停机状态，禁止移动轴");
                ret = false;
            }
            else if(axisList!=null)
            {
                foreach (var axis in axisList)
                {
                    MoveArgs args = new MoveArgs();
                    args.AxisName = axis.ToString();
                    var result = Motion.IsCanMove(args);
                    if (!isHome && !Motion.IsHomeOK(axis) )
                    {
                        UIMessageDialog.ShowErrorDialog(null, $"轴{result.Msg}原点丢失，禁止移动");
                        ret = false;
                        break;
                    }
                    else if (result.Result == false)
                    {
                        UIMessageDialog.ShowErrorDialog(null, $"轴{result.Msg}，禁止移动");
                        ret = false;
                        break;
                    }
                }
            }
            return ret;
        }
        public Brush GetBrush (MsgType msgType)
        {
            Brush bh = Brushes.Green;
            if (msgType==MsgType.Notify)
            {
                bh = Brushes.Blue;
            }
           else if (msgType == MsgType.Error)
            {
                bh = Brushes.Red;
            }
            return bh;
        }
    }
}
