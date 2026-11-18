using JC.PTIS.Main.Entities;
using JC.PTIS.Main.Models;
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
    public partial class ExLotBoxCtrl : UserControl
    {
        public ExLotBoxCtrl()
        {
            InitializeComponent();
        }
        LotBoxModel LotBoxModel = null;
        [Description("标题栏背景色")]
        public Color TitleBackColor { get { return htTitlePanel1.TitleColor; } set { htTitlePanel1.TitleColor = value; } }
        [Description("Foup名称")]
        public string Desc { get { return htTitlePanel1.Text; } set { htTitlePanel1.Text = value; } }
        public LotBoxStnType LotBoxStnType { get; set; }
        public int LotBoxNo { get; set; }
        public void OnLotBoxInfoUpdated(LotBoxModel model, LotBoxStnType lotBoxStnType)
        {
            if (lotBoxStnType == LotBoxStnType)
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action<LotBoxModel, LotBoxStnType>(OnLotBoxInfoUpdated), model, lotBoxStnType);
                }
                else
                {
                    if (model == null)
                    {
                        panel1.BackColor = Color.White;
                        lblRfid.Text = string.Empty;
                        lblLotBoxType.Text = string.Empty;
                    }
                    else
                    {
                        lblRfid.Text = model.Rfid;
                        lblLotBoxType.Text = model.LotBoxType.ToString();
                        switch (model.LotBoxType)
                        {
                            case LotBoxType.Bin135:
                                panel1.BackColor = Color.LightBlue;
                                break;
                            case LotBoxType.Bin246:
                                panel1.BackColor = Color.IndianRed;
                                break;
                            case LotBoxType.EmptyTray:
                                panel1.BackColor = Color.LightGreen;
                                break;

                        }
                    }
                }
            }
        }

        private void 上料盒ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lotCode = DateTime.Now.ToString("yyyyMMddHHmmss");
            var lotboxType = LotBoxNo == 0 ? LotBoxStnType.ExBox1Stn : LotBoxStnType.ExBox2Stn;
            ProcInfoHelper.Instance.LoadLotBox(lotCode, lotboxType);
        }

        private void 请求出料盒ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var lotboxType = LotBoxNo == 0 ? LotBoxStnType.ExBox1Stn : LotBoxStnType.ExBox2Stn;
            ProcInfoHelper.Instance.PickLotBox(lotboxType);
        }

        private void ExLotBoxCtrl_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode() == false)
            {
                ProcInfoHelper.Instance.LotBoxInfoUpdated += OnLotBoxInfoUpdated;
            }
        }


    }
}
