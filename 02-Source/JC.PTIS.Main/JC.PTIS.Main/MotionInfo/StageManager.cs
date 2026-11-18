using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JC.PTIS.Main.MotionInfo
{
    public class StageManager
    {
        static List<StageInfo> StageList= new List<StageInfo>();
        public static void Init()
        {
            for (int i = 0; i < 4; i++)
            {
                var stageInfo= new StageInfo()
                {
                    StageNum = i ,
                    X = (AxisName)Enum.Parse(typeof(AxisName), $"TrayStage{i+1}X"),
                    Y = (AxisName)Enum.Parse(typeof(AxisName), $"TrayStage{i+1}Y"),
                    T = (AxisName)Enum.Parse(typeof(AxisName), $"TrayStage{i+1}T"),
                    IsExistTray = (InputName)Enum.Parse(typeof(InputName), $"TrayStage{i+1}VacDetect"),
                    Vacuum = (VacuumName)Enum.Parse(typeof(VacuumName), $"TrayStage{i+1}Vac")
                };
                StageList.Add(stageInfo);
            }
        }
        public static StageInfo Get(int stageNum)
        {
            return StageList.FirstOrDefault(s => s.StageNum == stageNum);
        }
        public static List<StageInfo> GetList()
        {
            return StageList;
        }
    }
    public class StageInfo
    {
        public int StageNum { get; set; }
        public AxisName X { get; set; }
        public AxisName Y { get; set; }
        public AxisName T { get; set; }
        public InputName IsExistTray { get; set; }
        public VacuumName Vacuum { get; set; }
    }
}
