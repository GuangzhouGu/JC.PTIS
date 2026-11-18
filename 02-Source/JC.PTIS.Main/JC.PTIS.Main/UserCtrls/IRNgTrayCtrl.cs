using JC.PTIS.Main.Entities;
using JC.PTIS.Main.FlowInfo;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.RotateInfo;
using JC.PTIS.Main.SubViews;
using StdLib.PCBaseFrame.Extension.Entities;
using StdLib.PCBaseFrame.Motion;
using StdLib.WindowsControls;
using StdLib.WindowsControls.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.PTIS.Main.UserCtrls
{
    public partial class IRNgTrayCtrl : UserControl
    {
        public IRNgTrayCtrl()
        {
            InitializeComponent();
            panel1.DoubleBuffered();
        }
        [Description("描述")]
        public string Desc { get { return htTitlePanel1.Text; } set { htTitlePanel1.Text = value; } }
        [Description("标题栏背景色")]
        public Color TitleBackColor { get { return htTitlePanel1.TitleColor; } set { htTitlePanel1.TitleColor = value; } }
        PointF LeftTop = new PointF(1, 1);
        const int TotalTrayCount = 13;
        RectangleF[] RectList = new RectangleF[TotalTrayCount];
        float SlotInterval = 10.0f;
        IRNgTrayModel IRNgTrayModel = null;
        PointF MousePositionRealTime = new PointF(0, 0);
        Pen Pen = new Pen(Color.Blue, 1.0f);
        int HitDieRowNo = -1;
        public void UpdateInfo(IRNgTrayModel model)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<IRNgTrayModel>(UpdateInfo), model);
            }
            else
            {
                IRNgTrayModel = model;
                panel1.Invalidate();
            }
        }
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            if (ClassHelper.IsDesingMode() == false)
            {
                Render(e.Graphics);
            }
        }
        private void Render(Graphics g)
        {
            try
            {
                SlotInterval = (float)Math.Round((panel1.Height) * 1.0f / (TotalTrayCount + 1), 3);
                for (int i = 0; i < TotalTrayCount; i++)
                {
                    using (SolidBrush bh = new SolidBrush(Color.Blue))
                    {
                        g.DrawString((TotalTrayCount - i).ToString(), new Font("微软雅黑", 8), bh, LeftTop.X, LeftTop.Y + SlotInterval * i + 5);
                    }
                }
                if (IRNgTrayModel != null && IRNgTrayModel.DieArray != null)
                {
                    RectList = new RectangleF[TotalTrayCount];
                    #region 绘制Tray
                    for (int i = 0; i < IRNgTrayModel.DieArray.Length; i++)
                    {
                        if (IRNgTrayModel.DieArray[i] != null)
                        {
                            var rect = new RectangleF(LeftTop.X + 15, LeftTop.Y + SlotInterval * (TotalTrayCount - i - 1) + 10, panel1.Width - 50, 7);
                            Color color = Color.White;
                            SolidBrush bh = null;
                            var bin = $"{IRNgTrayModel.DieArray[i].BinLevel}";
                            var binlevel = EnumHelper.GetEnumValue(bin, BinLevel.None);
                            color = Global.GetBinColor(binlevel);
                            bh = new SolidBrush(color);
                            using (SolidBrush bh2 = new SolidBrush(Color.Black))
                            {
                                g.DrawString(binlevel.ToString(), new Font("微软雅黑", 8), bh2, LeftTop.X + panel1.Width - 30, LeftTop.Y + SlotInterval * (TotalTrayCount - i - 1) + 3);
                            }
                            g.FillRectangle(bh, rect);
                            RectList[i] = rect;
                            bh.Dispose();
                        }
                    }
                    using (SolidBrush brush = new SolidBrush(Color.Blue))
                    {
                        HitDieRowNo = GetHitRect();
                        if (HitDieRowNo != -1)
                        {
                            g.DrawRectangles(Pen, new RectangleF[] { RectList[HitDieRowNo] });
                        }
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {

            }
        }
        private int GetHitRect()
        {
            int row = -1;
            if (RectList != null)
            {
                for (int i = 0; i < RectList.Length; i++)
                {
                    var ret = RectList[i].Contains(MousePositionRealTime.X, MousePositionRealTime.Y);
                    if (ret)
                    {
                        row = i;
                        break;
                    }
                }
            }
            return row;
        }
        private void LotTrayCtrl_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode() == false)
            {
                this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
                ProcInfoHelper.Instance.IRNgTrayModelInfoUpdated += OnHalfTrayBufferInfoUpdated;
            }
        }

        private void OnHalfTrayBufferInfoUpdated(IRNgTrayModel model)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<IRNgTrayModel>(OnHalfTrayBufferInfoUpdated), model);
            }
            else
            {
                UpdateInfo(model);
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            MousePositionRealTime.X = e.X;
            MousePositionRealTime.Y = e.Y;
            panel1.Invalidate();
        }

        private void panel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (IRNgTrayModel != null && IRNgTrayModel.DieArray != null && HitDieRowNo != -1)
            {
                FrmModelDetail frmModelDetail = new FrmModelDetail();
                frmModelDetail.ShowInfo(IRNgTrayModel.DieArray[HitDieRowNo]);
                frmModelDetail.ShowDialog();
                if (frmModelDetail.IsUpdated)
                {
                    try
                    {
                        if (frmModelDetail.Model != null)
                        {
                            IRNgTrayModel.DieArray[HitDieRowNo] = frmModelDetail.Model as DieModel;
                            ProcInfoHelper.Instance.PlaceDieToIRNgTray(HitDieRowNo,IRNgTrayModel.DieArray[HitDieRowNo]);
                        }
                        else
                        {
                            ProcInfoHelper.Instance.PickTrayFromIRNgTray(HitDieRowNo);
                        }
                    }
                    catch (Exception ex)
                    {
                        UIMessageDialog.ShowErrorDialog(null, ex.ToString());
                    }
                }
            }
        }

        private void 详细信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcInfoHelper.Instance.ClearIRNgTray();
        }
    }
}
