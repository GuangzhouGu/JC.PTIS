using StdLib.PCBaseFrame.Motion;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.FlowInfo
{
    [Serializable]
    public class FlowInfoBase
    {
        [NonSerialized]
        MotionManager Motion=MotionManager.GetInstance();
        /// <summary>
        /// 在籍Sensor名称
        /// </summary>
        public string InputName { get; set; }
        public bool? IsExist()
        {
            if (!string.IsNullOrEmpty(InputName))
            {
                return Motion.GetDigitalInput(InputName);
            }
            else
            {
                return null;
            }
        }
    }
}
