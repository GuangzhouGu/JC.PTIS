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
    public partial class FoupCtrl : UserControl
    {
        public FoupCtrl()
        {
            InitializeComponent();
        }
         PointF LeftTop = new PointF(1, 1);
        [Description("标题栏背景色")]
        public Color TitleBackColor { get { return htTitlePanel1.TitleColor; } set { htTitlePanel1.TitleColor = value; } }
        [Description("Foup名称")]
        public string  Desc { get {return htTitlePanel1.Text; } set { htTitlePanel1.Text = value; } }
        [Description("Foup编号")]
        public int FoupNo { get; set; }
        [Description("Wafer总数")]
        public int TotalWaferCount { get { return int.Parse(lblTotalCount.Text); } set { lblTotalCount.Text = value.ToString(); } }
        List<RectangleF> RectList = null;
        float SlotInterval = 10.0f;
        FoupModel FoupModel = null;
        public void UpdateFoupInfo(FoupModel foupModel)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<FoupModel>(UpdateFoupInfo), foupModel);
            }
            else
            {
                if (foupModel!=null)
                {
                    FoupModel = foupModel;
                    lblRfid.Text = foupModel.Rfid.ToString();
                    SlotInterval = (float)Math.Round((panel1.Height) * 1.0f / (TotalWaferCount+1), 3);
                    lblRemainCount.Text = foupModel.RemainWaferCount.ToString();
                }
                else
                {
                    FoupModel = null;
                    lblRfid.Text = string.Empty;
                    lblRemainCount.Text = "0";
                }
                this.Refresh();
            }
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (ClassHelper.IsDesingMode()==false)
            {
                Render(e.Graphics);
            }
        }
        private void Render(Graphics g)
        {
            if (FoupModel!=null&&!string.IsNullOrEmpty(FoupModel.WaferMap))
            {
                if (panel1.InvokeRequired)
                {
                    panel1.Invoke(new Action<Graphics>(Render), g);
                }
                else
                {
                    #region 绘制Boat
                    for (int i =0; i <TotalWaferCount; i++)
                    {
                        using (SolidBrush bh = new SolidBrush(Color.Blue))
                        {
                            g.DrawString((TotalWaferCount-i).ToString(), new Font("微软雅黑", 6), bh, LeftTop.X, LeftTop.Y + SlotInterval * i);
                        }
                    }
                    RectList = new List<RectangleF>();
                    var arr= FoupModel.WaferMap.Split(',');
                    for (int i =0; i < arr.Length; i++)
                    {
                        var rect = new RectangleF(LeftTop.X + 15, LeftTop.Y + SlotInterval * i+5, panel1.Width - 20, 5);
                        SolidBrush bh = new SolidBrush(Color.Blue);
                        switch (arr[arr.Length-i-1])
                        {
                            case "0":
                                bh = new SolidBrush(Color.LightGray);
                                break;
                            case "1":
                                bh = new SolidBrush(Color.Lime);
                                break;
                            case "2":
                                bh = new SolidBrush(Color.LightPink);
                                break;
                            default:
                                break;
                        }
                        g.FillRectangle(bh, rect);
                        RectList.Add(rect);
                        bh.Dispose();
                    }
                 
                    #endregion
                }
            }
       
        }

        private void FoupCtrl_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode()==false)
            {
                ProcInfoHelper.Instance.FoupInfoUpdated += OnFoupInfoUpdated;
            }
        }

        private  void OnFoupInfoUpdated(int foupNo, FoupModel foupModel)
        {
            if (foupNo== FoupNo)
            {
                UpdateFoupInfo(foupModel);
            }
        }

        private void 请求上FoupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var rfid=DateTime.Now.ToString("yyyyMMddHHmmss");
            ProcInfoHelper.Instance.LoadFoup(FoupNo, rfid);
        }

        private void 请求取走FoupToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcInfoHelper.Instance.UnloadFoup(FoupNo);
        }
    }
}
