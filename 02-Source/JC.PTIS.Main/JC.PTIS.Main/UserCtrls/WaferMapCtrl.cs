using JC.PTIS.Main;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.SubViews;
using StdLib.LogHelper;
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

namespace HYC.MicroLedATDetect.Controls
{
    public partial class WaferMapCtrl : UserControl
    {
        public WaferMapCtrl()
        {
            InitializeComponent();
            LeftTop = pictureBox1.Location;
            pictureBox1.DoubleBuffered();
        }
        [Description("标题栏背景色")]
        public Color TitleBackColor { get { return htTitlePanel1.TitleColor; } set { htTitlePanel1.TitleColor = value; } }
        float  RatioX = 1, RatioY = 1;//缩放比例
         RectangleF DrawRect;
        RectangleF[,] RectList = null;
        float ZoomOld = 1.0f;
         float Zoom = 1.0f;
         WaferModel WaferPara = null;
        float WaferWidth = 1.0f;
        float WaferHeight = 1.0f;
        public int HitDieRowNo { get; set; } = -1;
        public int HitDieColNo { get; set; } = -1;
        /// <summary>
        /// 表格的左上角
        /// </summary>
        public PointF LeftTop = new PointF(0,0);
         bool LeftButtonPress = false;
        PointF MousePosition = new PointF(0, 0);
         PointF MousePositionRealTime = new PointF(0, 0);
        Pen Pen = new Pen(Color.DeepPink, 2.0f);
        private void Init()
        {
            try
            {
                if (ClassHelper.IsDesingMode() == false)
                {
                    this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
                    this.pictureBox1.MouseWheel += this_MouseWheel;
                    this.pictureBox1.MouseMove += this_MouseMove;
                    this.pictureBox1.MouseDown += this_MouseDown;
                    this.pictureBox1.MouseUp += WaferMapBase_MouseUp;
                    ProcInfoHelper.Instance.WaferInfoUpdated += OnWaferInfoUpdated;
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("WaferMap").Fatal(ex.ToString());
            }
        }



        public void OnWaferInfoUpdated(WaferModel e,WaferPosEnum waferPosEnum)
        {
            if (waferPosEnum== WaferPosEnum.WaferTable)
            {
                DrawWafer(e);
            }
             
        }
        private void x100ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomFunc(2F);
            pictureBox1.Invalidate();
        }

        private void x500ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomFunc(5F);
            pictureBox1.Invalidate();
        }

        private void WaferMapBase_Load(object sender, EventArgs e)
        {
            Init();
        }

        private void x1000ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomFunc(10F);
            pictureBox1.Invalidate();
        }
      
        private void x5000ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomFunc(50F);
            pictureBox1.Invalidate();
        }

        private void resetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Zoom = 1F;
            LeftTop = new PointF(0, 0);
            Reset();
            pictureBox1.Invalidate();
        }
        public virtual void Reset()
        {
            DrawRect = new RectangleF(LeftTop.X, LeftTop.Y, this.pictureBox1.Width * Zoom, this.pictureBox1.Height * Zoom);
            RatioX = Convert.ToSingle(DrawRect.Width) / WaferWidth;
            RatioY = Convert.ToSingle(DrawRect.Height) / WaferHeight;
        }

        private void WaferMapBase_MouseUp(object sender, MouseEventArgs e)
        {
            LeftButtonPress = false;
            this.Cursor = Cursors.Default;
        }
        private void ZoomFunc(float zoom)
        {
            ZoomOld = Zoom;
            Zoom = zoom;
            var zoomNew = zoom;
            var zoomOld = ZoomOld;
            var deltaZoomNewToOld = zoomNew / zoomOld;
            var zero = LeftTop;
            zero.X = MousePosition.X - (MousePosition.X - zero.X) * deltaZoomNewToOld;
            zero.Y = MousePosition.Y - (MousePosition.Y - zero.Y) * deltaZoomNewToOld;
            LeftTop = zero;
        }
        private void this_MouseWheel(object sender, MouseEventArgs e)
        {
            var delta = e.Delta;
            if (Math.Abs(delta) < 10)
            {
                return;
            }
            var mousePosition = new PointF();
            mousePosition.X = e.X;
            mousePosition.Y = e.Y;
            ZoomOld = Zoom;

            if (delta < 0)
            {
                Zoom -= FetchStep(delta);
            }
            else if (delta > 0)
            {
                Zoom += FetchStep(delta);
            }
            var zoomNew = Zoom;
            var zoomOld = ZoomOld;
            var deltaZoomNewToOld = zoomNew / zoomOld;

            var zero = LeftTop;
            zero.X = mousePosition.X - (mousePosition.X - zero.X) * deltaZoomNewToOld;
            zero.Y = mousePosition.Y - (mousePosition.Y - zero.Y) * deltaZoomNewToOld;
            LeftTop = zero;
            pictureBox1.Invalidate();
        }
        /// <summary>
        /// 获取缩放的步进
        /// </summary>
        /// <returns></returns>
        private float FetchStep(float delta)
        {
            if (Zoom == 1)
            {
                return delta > 0 ? 1 : 0.05f;
            }
            else
            {
                return Zoom >= 1 ? 1 : 0.05f;
            }
        }
        private void this_MouseMove(object sender, MouseEventArgs e)
        {
            var offsetX = e.X - MousePosition.X;
            var offsetY = e.Y - MousePosition.Y;
            if (LeftButtonPress)
            {
                LeftTop.X += offsetX;
                LeftTop.Y += offsetY;
                MousePosition.X = e.X;
                MousePosition.Y = e.Y;
                pictureBox1.Invalidate();
            }
            else
            {
                MousePositionRealTime.X = e.X;
                MousePositionRealTime.Y = e.Y;
                pictureBox1.Invalidate();
            }
        }
        private void this_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                MousePosition.X = e.X;
                MousePosition.Y = e.Y;

                LeftButtonPress = true;
                this.Cursor = Cursors.Hand;
            }
            else if (e.Button == MouseButtons.Right)
            {
                MousePosition.X = e.X;
                MousePosition.Y = e.Y;
            }
        }

        private void WaferMapBase_Paint(object sender, PaintEventArgs e)
        {
            if (WaferPara!=null)
            {
                Render(WaferPara, e.Graphics);
            }
        }
        public void DrawWafer(WaferModel wafer)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<WaferModel>(DrawWafer),wafer);
            }
            else
            {
                WaferPara = wafer;
                if (wafer!=null)
                {
                    int rows = wafer.ROWCT;
                    int cols = wafer.COLCT;
                    WaferWidth = cols * wafer.XDIES;
                    WaferHeight = rows * wafer.YDIES;
                }
                Reset();
                pictureBox1.Invalidate();
            }
       
        }
        private void x100ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            ZoomFunc(100F);
            pictureBox1.Invalidate();
        }

        private void x200ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ZoomFunc(200F);
            pictureBox1.Invalidate();
        }
      
     
        private  void Render(WaferModel wafer, Graphics g)
        {
            try
            {
                g.Clear(Color.White);
                if (wafer!=null)
                {
                    int rows = wafer.ROWCT;
                    int cols = wafer.COLCT;
                    RectList = new RectangleF[rows, cols];
                    g.Clear(Color.Black);
                    // 逐晶粒绘制
                    for (int r = 0; r < rows; r++)
                    {
                        for (int c = 0; c < cols; c++)
                        {
                            var model = wafer.DieArray[r, c];
                            Color color = Color.White;
                            if (model.DieStatus == DieStatus.InWafer)
                            {
                                color = Global.GetBinColor(model.BinLevel);
                            }
                            using (SolidBrush brush = new SolidBrush(color))
                            {
                                RectList[r, c] = new RectangleF(LeftTop.X + c * wafer.XDIES * Zoom * RatioX, LeftTop.Y + r * wafer.YDIES * Zoom * RatioY, wafer.XDIES * Zoom * RatioX - 1.0f, wafer.YDIES * Zoom * RatioY - 1.0f);
                                g.FillRectangle(brush, RectList[r, c]);
                            }
                        }
                    }
                    g.DrawLine(Pen, MousePositionRealTime.X, 0, MousePositionRealTime.X, this.pictureBox1.Height);
                    g.DrawLine(Pen, 0, MousePositionRealTime.Y, this.pictureBox1.Width, MousePositionRealTime.Y);
                    using (SolidBrush brush = new SolidBrush(Color.Blue))
                    {
                        var value = GetHitRect();
                        if (value.r != -1 && value.c != -1)
                        {
                            HitDieRowNo = value.r;
                            HitDieColNo = value.c;
                            g.DrawRectangles(Pen, new RectangleF[] { RectList[value.r, value.c] });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
            }
        }
        private (int r,int c)GetHitRect()
        {
            int row = -1;
            int col = -1;
            if (RectList!=null)
            {
                for (int r = 0; r < RectList.GetLength(0); r++)
                {
                    for (int c = 0; c < RectList.GetLength(1); c++)
                    {
                       var ret= RectList[r, c].Contains(MousePositionRealTime.X, MousePositionRealTime.Y);
                        if (ret)
                        {
                            row = r;
                            col = c;
                            lblRowNo.Text = row.ToString();
                            lblColNo.Text = col.ToString();
                            if (WaferPara.DieArray[row, col].BinLevel<=BinLevel.Bin9)
                            {
                                lblBinNo.Text = WaferPara.DieArray[row, col].BinLevel.ToString();
                            }
                            break;
                        }
                    }
                }
            }
            return (row, col);
        }

        private void pictureBox1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            if (WaferPara != null)
            {
                FrmShowWaferMap frmShowWaferMap = new FrmShowWaferMap();
                frmShowWaferMap.ShowWaferMap(WaferPara);
                frmShowWaferMap.ShowDialog();
            }
        }

        private void btnLoadMap_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var path = openFileDialog.FileName;
                var wafer = WaferMapHelper.Instance.Parse(path);
                if (wafer != null)
                {
                    DrawWafer(wafer);
                }
            }
        }
    }
  
}
