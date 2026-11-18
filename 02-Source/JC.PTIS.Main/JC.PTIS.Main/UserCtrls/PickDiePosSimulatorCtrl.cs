using JC.PTIS.Main.Devices.Vision;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.Params;
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
    public partial class PickDiePosSimulatorCtrl : UserControl
    {
        public PickDiePosSimulatorCtrl()
        {
            InitializeComponent();
        }
        WaferModel WaferModel = null;
        int BaseDieRowNo= 0;
        int BaseDieColNo= 0;
        public PointF LeftTop = new PointF(0, 0);
        RectangleF DrawRect;
        Pen Pen = new Pen(Color.DeepPink, 2.0f);
        RectangleF[,] RectList = null;
        float Zoom = 1.0f;
        float RatioX = 1.0f, RatioY = 1.0f;//缩放比例
        float WaferWidth = 1.0f, WaferHeight = 1.0f;
        Xyt FirstDieAbsPos = null;
        float RectWidth = 20.0f, RectHeight = 20.0f;
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Render(e.Graphics);
        }
        private void Render(Graphics g)
        {
            try
            {
                if (WaferModel != null)
                {
                    int rows = WaferModel.ROWCT;
                    int cols = WaferModel.COLCT;
                    RectList = new RectangleF[rows, cols];
                    g.Clear(Color.Black);
                    Color color = Color.Lime;
                    // 逐晶粒绘制
                    for (int r = 0; r < rows; r++)
                    {
                        for (int c = 0; c < cols; c++)
                        {
                            var model = WaferModel.DieArray[r, c];
                            if (model.IsRealDie)
                            {
                                using (SolidBrush brush = new SolidBrush(color))
                                {
                                    var pos = GetDieGrabPos(r, c);
                                    RectList[r, c] = new RectangleF((float)pos.X, (float)pos.Y, RectWidth*1.0f, RectHeight * 0.95f);
                                }
                            }
                          
                        }
                    }
                    g.FillRectangles(Brushes.Lime, RectList.Cast<RectangleF>().ToArray());
                }
            }
            catch (Exception ex)
            {
            }
        }

        private void btnCalcPickPos_Click(object sender, EventArgs e)
        {
            try
            {
                if (WaferModel==null)
                {
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        var path = openFileDialog.FileName;
                        WaferModel = WaferMapHelper.Instance.Parse(path);
                    }
                }
                BaseDieRowNo = int.Parse(tbxBaseDieRowNo.Text);
                BaseDieColNo = int.Parse(tbxBaseDieColNo.Text);
           
                if (WaferModel!=null)
                {
                    WaferWidth = WaferModel.COLCT * WaferModel.XDIES;
                    WaferHeight = WaferModel.ROWCT * WaferModel.YDIES;
                    DrawRect = new RectangleF(LeftTop.X, LeftTop.Y, this.panel1.Width * Zoom, this.panel1.Height * Zoom);
                    RatioX = Convert.ToSingle(DrawRect.Width) / WaferWidth;
                    RatioY = Convert.ToSingle(DrawRect.Height) / WaferHeight;
                    RectWidth=this.panel1.Width / WaferModel.COLCT;
                    RectHeight=this.panel1.Height / WaferModel.ROWCT;
                    FirstDieAbsPos = new Xyt() { X = LeftTop.X + BaseDieColNo * WaferModel.XDIES * Zoom * RatioX, Y = LeftTop.Y + BaseDieRowNo * WaferModel.YDIES * Zoom * RatioY };
                    panel1.Invalidate();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private Xyt GetDieGrabPos(int rowNo, int colNo)
        {
            Xyt ret = null;
            if (WaferModel != null)
            {
                var dieXPos = (colNo - BaseDieColNo) * (WaferModel.XDIES  ) * Zoom * RatioX;
                var dieYPos = (rowNo - BaseDieRowNo) * (WaferModel.YDIES  ) * Zoom * RatioY;
                ret = new Xyt() { X = FirstDieAbsPos.X + dieXPos, Y = FirstDieAbsPos.Y + dieYPos };
            }
            return ret;
        }
    }
}
