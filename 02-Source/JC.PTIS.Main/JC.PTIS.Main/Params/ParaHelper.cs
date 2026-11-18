using StdLib.PCBaseFrame.Extension.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.Params
{
    public class ParaHelper
    {
        public static GlobalPara Global { get { return RecipeManager.GetPara<GlobalPara>(nameof(GlobalPara) ); } set { } }
        public static ProdPara Prod { get { return RecipeManager.GetPara<ProdPara>(nameof(ProdPara)); } set { } }
        public static VisionPara Vision { get { return RecipeManager.GetPara<VisionPara>(nameof(VisionPara)); } set { } }
        public static MotionPara Motion { get { return RecipeManager.GetPara<MotionPara>(nameof(MotionPara)); } set { } }
        public static OffsetPara Offset { get { return RecipeManager.GetPara<OffsetPara>(nameof(OffsetPara)); } set { } }
    }
   
}
