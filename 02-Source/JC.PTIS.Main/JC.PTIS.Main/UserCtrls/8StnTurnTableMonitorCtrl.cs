using JC.PTIS.Main.Entities;
using JC.PTIS.Main.FlowInfo;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.RotateInfo;
using StdLib.PCBaseFrame.Extension.Entities;
using StdLib.PCBaseFrame.Motion;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.PTIS.Main.UserCtrls
{
    public partial class Stn8TurnTableMonitorCtrl : UserControl
    {
        MotionManager Motion => MotionManager.GetInstance();
        public Stn8TurnTableMonitorCtrl()
        {
            InitializeComponent();
            for (int i = 0; i < 8; i++)
            {
                var name = $"lotTrayCtrl{i + 1}";
                var ctrl = this.Controls.Find(name, true).FirstOrDefault() as LotTrayCtrl;
                LotTrayCtrls[i] = ctrl;
            }
        }
        [Description("标题栏背景色")]
        public Color TitleBackColor { get { return htTitlePanel1.TitleColor; } set { htTitlePanel1.TitleColor = value; } }
        LotTrayCtrl[] LotTrayCtrls = new LotTrayCtrl[8];
    
        private void LoadDieStnMonitorCtrl_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode()==false)
            {
                RotateInfoHelper.Instance.RotateInfoChanged += OnRotateInfoChanged;
                ProcInfoHelper.Instance.Stn8TurnTableUpdated += OnLotInfoUpdated;
            }
        }

        private void OnLotInfoUpdated(int stnNo, LotStageInfo info)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<int, LotStageInfo>(OnLotInfoUpdated), stnNo, info);
            }
            else
            {
                LotTrayCtrls[stnNo].UpdateLotInfo(info);
            }
        }

        private void OnRotateInfoChanged(RotateStnType type, int[] stnNoList)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<RotateStnType, int[]>(OnRotateInfoChanged), type, stnNoList);
            }
            else
            {
                if (type == RotateStnType.Stn8TurnTable)
                {
                    foreach (var stnNo in stnNoList)
                    {
                        var info = ProcInfoHelper.Instance.GetLotStageInfo((Stn8TurnTableStnEnum)stnNo);
                        LotTrayCtrls[stnNo].UpdateLotInfo(info);
                    }
                    lblRealPos.Text = Motion.GetAxisActPos(AxisName.Stn8TurnTable_DD).ToString("F3");
                }
            }
        }

    }
}
