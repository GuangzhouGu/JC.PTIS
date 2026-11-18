using JC.PTIS.Main.Entities;
using JC.PTIS.Main.FlowInfo;
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
    public partial class LotMonitorCtrl : UserControl
    {
        public LotMonitorCtrl()
        {
            InitializeComponent();
        }
        [Description("描述")]
        public string Desc{ get { return htTitlePanel1.Text; } set { htTitlePanel1.Text = value; } }
        [Description("标题栏背景色")]
        public Color TitleBackColor { get { return htTitlePanel1.TitleColor; } set { htTitlePanel1.TitleColor = value; } }
        public LotPosEnum LotPosEnum { get; set; }

        private void LoadDieStnMonitorCtrl_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode() == false)
            {
                ProcInfoHelper.Instance.LotDicInfoUpdated += OnUpdateTrayStageInfo;
            }
        }

        private void OnUpdateTrayStageInfo(LotStageInfo lotInfo)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<LotStageInfo>(OnUpdateTrayStageInfo), lotInfo);
            }
            else
            {
                if (lotInfo.LotPos== LotPosEnum&&lotInfo.LotModel!=null)
                {
                    lotInfo.LotStageType = (LotStageType)(lotInfo.LotModel.BinLevel.ToInt()-1);
                    this.lotTrayCtrl1.UpdateLotInfo(lotInfo);
                }
            }
        }

        private void 详细信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
