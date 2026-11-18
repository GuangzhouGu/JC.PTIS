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
    public partial class TurnTableCtrl : UserControl
    {
        public TurnTableCtrl()
        {
            InitializeComponent();
        }

        private int stnCount = 16;
        [Description("工位数量")]
        public int StnCount { get { return stnCount; } set { stnCount = value; this.Refresh(); } }
        private void TurnTableCtrl_Paint(object sender, PaintEventArgs e)
        {
            //在picturenBox1上绘制一个圆形并且填充颜色
            var g = e.Graphics;
            var rect = this.ClientRectangle;
            rect.X += 2;
            rect.Y += 2;
            rect.Width -= 4;
            rect.Height -= 4;
            g.FillEllipse(Color.LightSkyBlue, rect);
            //等分成16份，绘制16条线
            int lines = stnCount;
            for (int i = 0; i < lines; i++)
            {
                double angle = i * 2 * Math.PI / lines;
                float x = rect.Width / 2 + (float)(rect.Width / 2 * Math.Cos(angle));
                float y = rect.Height / 2 + (float)(rect.Height / 2 * Math.Sin(angle));
                g.DrawLine(Pens.Gray, rect.Width / 2, rect.Height / 2, x, y);
            }
        }

        private void TurnTableCtrl_SizeChanged(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}
