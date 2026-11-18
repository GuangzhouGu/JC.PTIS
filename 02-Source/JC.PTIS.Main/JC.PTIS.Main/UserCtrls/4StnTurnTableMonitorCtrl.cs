using JC.PTIS.Main.Entities;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.RotateInfo;
using StdLib.PCBaseFrame.Extension.Entities;
using StdLib.PCBaseFrame.Motion;
using System;
using System.Collections;
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
    public partial class Stn4TurnTableMonitorCtrl : UserControl
    {
        MotionManager Motion => MotionManager.GetInstance();
        public Stn4TurnTableMonitorCtrl()
        {
            InitializeComponent();
            for (int i = 0; i < 4; i++)
            {
                var name = $"trayCtrl{i + 1}";
                var ctrl = this.Controls.Find(name, true).FirstOrDefault() as TrayCtrl;
                TrayCtrls[i] = ctrl;
            }
        }
        [Description("标题栏背景色")]
        public Color TitleBackColor { get { return htTitlePanel1.TitleColor; } set { htTitlePanel1.TitleColor = value; } }
        TrayCtrl[] TrayCtrls = new TrayCtrl[4];
    

        private void LoadDieStnMonitorCtrl_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode()==false)
            {
                RotateInfoHelper.Instance.RotateInfoChanged += OnRotateInfoChanged;
                ProcInfoHelper.Instance.Stn4TurnTableUpdated += OnUpdateTrayStageInfo;
            }
        }

        private void OnUpdateTrayStageInfo(int stnNo, TrayStageInfo info)
        {
			if (InvokeRequired)
			{
				this.Invoke(new Action<int, TrayStageInfo>(OnUpdateTrayStageInfo), stnNo, info);
			}
			else
			{
				TrayCtrls[stnNo].UpdateTrayInfo(info);
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
                if (type == RotateStnType.Stn4TurnTable)
                {
                    foreach (var stnNo in stnNoList)
                    {
                        var info=ProcInfoHelper.Instance.GetTrayInfo((Stn4TurnTableStnEnum)stnNo);
                        TrayCtrls[stnNo].UpdateTrayInfo(info);
                    }
                    lblRealPos.Text = Motion.GetAxisActPos(AxisName.Stn4TurnTable_DD).ToString("F3");
                }
            }
        }

   
       
    }
}
