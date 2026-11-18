using JC.PTIS.Main.Entities;
using JC.PTIS.Main.FlowInfo;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.RotateInfo;
using JC.PTIS.Main.SubViews;
using StdLib.PCBaseFrame.Extension.Entities;
using StdLib.WindowsControls;
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
    public partial class HalfTrayBufferCtrl : UserControl
    {
        public HalfTrayBufferCtrl()
        {
            InitializeComponent();
            panel1.DoubleBuffered();
        }
        [Description("描述")]
        public string Desc { get { return htTitlePanel1.Text; } set { htTitlePanel1.Text = value; } }
        [Description("标题栏背景色")]
        public Color TitleBackColor { get { return htTitlePanel1.TitleColor; } set { htTitlePanel1.TitleColor = value; } }
        PointF LeftTop = new PointF(1, 1);
        int TotalTrayCount = 12;
        RectangleF[] RectList=new RectangleF[12];
        float SlotInterval = 10.0f;
        HalfTrayBufferModel BufferModel = null;
        PointF MousePositionRealTime = new PointF(0, 0);
        Pen Pen = new Pen(Color.Blue, 1.0f);
        int HitDieRowNo = -1;
        public void UpdateInfo(HalfTrayBufferModel lotModel)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<HalfTrayBufferModel>(UpdateInfo), lotModel);
            }
            else
            {
                BufferModel = lotModel;
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
                        g.DrawString((TotalTrayCount - i).ToString(), new Font("微软雅黑", 6), bh, LeftTop.X, LeftTop.Y + SlotInterval * i + 3);
                    }
                }
                if (BufferModel != null && BufferModel.TrayArray != null)
                {
                    RectList = new RectangleF[12];
                    #region 绘制Tray
                    var arr = BufferModel.LoadMap();
                    for (int i = 0; i < BufferModel.TrayArray.Length; i++)
                    {
                        if (BufferModel.TrayArray[i] != null)
                        {
                            var rect = new RectangleF(LeftTop.X + 15, LeftTop.Y + SlotInterval * (TotalTrayCount - i - 1) + 7, panel1.Width - 60, 5);
                            Color color = Color.White;
                            SolidBrush bh = null;
                            var result = BufferModel.TrayArray[i].IsOkTray ? "OK" : "NG";
                            var bin = $"Bin{BufferModel.TrayArray[i].BinLevel}";
                            var binlevel = EnumHelper.GetEnumValue(bin, BinLevel.None);
                            var binlevel2 = binlevel;
                            if (BufferModel.TrayArray[i].IsOkTray == false && (binlevel == BinLevel.Bin1 || binlevel == BinLevel.Bin3 || binlevel == BinLevel.Bin5))
                            {
                                binlevel2 = binlevel2 + 1;
                            }
                            color = Global.GetBinColor(binlevel2);
                            bh = new SolidBrush(color);
                            using (SolidBrush bh2 = new SolidBrush(Color.Black))
                            {
                                if (binlevel.ToInt() % 2 != 0)
                                {
                                    g.DrawString(binlevel.ToString() + result, new Font("微软雅黑", 6), bh2, LeftTop.X + panel1.Width - 40, LeftTop.Y + SlotInterval * (TotalTrayCount - i - 1) + 3);
                                }
                                else
                                {
                                    g.DrawString(binlevel.ToString(), new Font("微软雅黑", 6), bh2, LeftTop.X + panel1.Width - 40, LeftTop.Y + SlotInterval * (TotalTrayCount - i - 1) + 3);
                                }
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
                ProcInfoHelper.Instance.HalfTrayBufferInfoUpdated += OnHalfTrayBufferInfoUpdated;    
            }
        }

        private void OnHalfTrayBufferInfoUpdated(HalfTrayBufferModel model)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<HalfTrayBufferModel>(OnHalfTrayBufferInfoUpdated), model);
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
            if (BufferModel != null && BufferModel.TrayArray != null&& HitDieRowNo!=-1)
            {
                FrmModelDetail frmModelDetail = new FrmModelDetail();
                frmModelDetail.ShowInfo(BufferModel.TrayArray[HitDieRowNo]);
                frmModelDetail.ShowDialog();
                if (frmModelDetail.IsUpdated)
                {
                    try
                    {
                        if (frmModelDetail.Model!=null)
                        {
                            BufferModel.TrayArray[HitDieRowNo] = frmModelDetail.Model as TrayModel;
                            ProcInfoHelper.Instance.PutTrayToHalfTrayBuffer(BufferModel.TrayArray[HitDieRowNo], HitDieRowNo);
                        }
                        else
                        {
                            ProcInfoHelper.Instance.PickTrayFromHalfTrayBuffer(HitDieRowNo);
                        }
                    }
                    catch (Exception ex)
                    {
                        UIMessageDialog.ShowErrorDialog(null,ex.ToString());
                    }
                }
            }
        }
    }
}
