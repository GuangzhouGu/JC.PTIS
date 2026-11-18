using JC.PTIS.Main.Entities;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.RotateInfo;
using StdLib.PCBaseFrame.Extension.Entities;
using System;
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
    public partial class TrayMonitorCtrl : UserControl
    {
        public TrayMonitorCtrl()
        {
            InitializeComponent();
        }
        public int TrayCount { get; set; } = 1;
        [Description("描述")]
        public string Desc{ get { return htTitlePanel1.Text; } set { htTitlePanel1.Text = value; } }
        [Description("标题栏背景色")]
        public Color TitleBackColor { get { return htTitlePanel1.TitleColor; } set { htTitlePanel1.TitleColor = value; } }

        private TrayPosEnum trayPosEnum;
        public TrayPosEnum TrayPosEnum { get { return trayPosEnum; } set { trayPosEnum = value; trayCtrl1.TrayPos = value; } }
        [Description("交换行列")]
        public bool IsReverseRC { get; set; }
        private void LoadDieStnMonitorCtrl_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode() == false)
            {
                ProcInfoHelper.Instance.TrayDicInfoUpdated += OnUpdateTrayStageInfo;
                trayCtrl1.IsReverseRC = this.IsReverseRC;
            }
        }

        private void OnUpdateTrayStageInfo(TrayStageInfo trayModel)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<TrayStageInfo>(OnUpdateTrayStageInfo), trayModel);
            }
            else
            {
                if (trayModel.TrayPos == TrayPosEnum)
                {
                    this.trayCtrl1.UpdateTrayInfo(trayModel);
                }
            }
        }
    
    }
}
