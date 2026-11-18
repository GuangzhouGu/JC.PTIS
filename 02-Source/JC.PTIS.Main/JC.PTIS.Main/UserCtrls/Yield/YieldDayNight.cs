using JC.AutoMating;
using JC.PTIS.Main;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.Models.ModelHelper;
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

namespace JC.MVI.UserCtrl.YieldCtrl
{
    public partial class YieldDayNight : UserControl
    {
        public YieldDayNight()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            this.DoubleBuffered = true;
        }
        static List<List<DieModel>> YieldList = new List<List<DieModel>>();
        float BarWidth = 50.0f;
        float HeightMaxValue = 750;
        float RatioX = 1.0f;
        float RatioY = 1.0f;
        float FontSize = 9;
        // float BarHeightScale = 1.45f;
        private void btnQuery_Click(object sender, EventArgs e)
        {
            Query();
        }
        private void Query()
        {
            DateTime date = dtPickerDate1.Value.Date;
            string startTime = date.ToString("yyyy-MM-dd ") + "08:00:00";
            string endTime = date.ToString("yyyy-MM-dd ") + "20:00:00";
            bool isDay = cbxDayYield.Checked;
            YieldType type = YieldType.Day;
            if (cbxNightYield.Checked)
            {
                type = YieldType.Night;
                startTime = date.ToString("yyyy-MM-dd ") + "20:00:00";
                endTime = date.AddDays(1).ToString("yyyy-MM-dd ") + "08:00:00";
            }
            else if (cbxAllDay.Checked)
            {
                type = YieldType.All;
                startTime = date.ToString("yyyy-MM-dd ") + "00:00:00";
                endTime = date.ToString("yyyy-MM-dd ") + "23:59:59.999";
            }
            Task.Factory.StartNew(() => {
                var list = DieModelHelper.GetByTimeRange(DateTime.Parse(startTime), DateTime.Parse(endTime));
                if (list != null)
                {
                    this.RefreshYield(list, type);
                }
            });
        }
        /// <summary>
        /// 绘制图表
        /// </summary>
        /// <param name="g"></param>
        private void DrawChart(Graphics g)
        {
            try
            {
                if (YieldList==null)
                {
                    return;
                }
                var list = YieldList.Select(p => p != null && p.Count > 0 ? p.Count : 0).ToList(); ;
                if (list == null||list.Count==0)
                {
                    return;
                }
                var maxValue = list.Max();
                if (maxValue==0)
                {
                    maxValue = 500;
                }
                RatioY= HeightMaxValue / (maxValue+100);
                float startX = 50 * RatioX;
                var rectList = new List<RectangleF>();
                // 绘制 X轴
                if (cbxAllDay.Checked)
                {
                    startX = 50 * RatioX;
                    BarWidth = (this.pictureBox1.Width - 50.0f) / 25;
                    g.DrawLine(Pens.Black, startX, HeightMaxValue * RatioX, startX + (BarWidth + 10) * RatioX * 24, HeightMaxValue * RatioX); // X 轴
                }
                else
                {
                    BarWidth = (this.pictureBox1.Width - 50.0f) / 13;
                    g.DrawLine(Pens.Black, startX, HeightMaxValue * RatioX, startX + (BarWidth + 10) * RatioX * 12, HeightMaxValue * RatioX); // X 轴
                }
                // 绘制 Y轴
                g.DrawLine(Pens.Black, startX, 5, startX, HeightMaxValue ); // Y 轴
                // 绘制 Y 轴刻度
                int yTickCount = 10;
                float valueStep = 100;
                if (maxValue>1000)
                {
                    valueStep= maxValue*1.0f/ yTickCount;
                }
                for (int i = 0; i <= yTickCount; i++)
                {
                    float tickY = HeightMaxValue-(i * valueStep ) * RatioY;
                    g.DrawLine(Pens.Black, 45 * RatioX, tickY, 50 * RatioX, tickY);
                    g.DrawString((i * valueStep).ToString(), new Font("微软雅黑", FontSize), Brushes.Black, 20 * RatioX, tickY - 5 * RatioY);
                }
                // 绘制 X 轴刻度
                DrawScaleX(g);
                //绘制Bar
                for (int i = 0; i < YieldList.Count; i++)
                {
                    try
                    {
                        if (YieldList[i] != null && YieldList[i].Count > 0)
                        {
                            YieldList[i].Sort((x, y) => x.BinLevel.CompareTo(y.BinLevel));
                            var dieCategoryList = YieldList[i].GroupBy(m => m.BinLevel).Select(m => m.First()).ToList();
                            float segment = dieCategoryList.Count;
                            var dieCountList = YieldList[i].GroupBy(m => m.BinLevel).Select(m => m.Count()).ToList();
                            float heightSum = 0;
                            float rectWidth = BarWidth/2.0f * RatioX;
                            startX = BarWidth * RatioX + i * BarWidth * RatioX + rectWidth / 2.0f;
                            if (cbxAllDay.Checked)
                            {
                                startX =20+BarWidth * RatioX + i * BarWidth * RatioX + rectWidth / 2.0f;
                            }
                            for (int j = 0; j < segment; j++)
                            {
                                float rectHeight = dieCountList[j]*RatioY;
                                heightSum += rectHeight;
                                float startY = HeightMaxValue * RatioX - heightSum;//从上往下绘制矩形的高度
                                RectangleF barSegmentRect = new RectangleF(startX, startY, rectWidth, rectHeight);
                                rectList.Add(barSegmentRect);
                                if (dieCategoryList[j].BinLevel.ToInt() >= BinLevel.Bin1.ToInt())
                                {
                                    var color = Global.GetBinColor(dieCategoryList[j].BinLevel);
                                    using (SolidBrush brush = new SolidBrush(color))
                                    {
                                        g.FillRectangle(brush, barSegmentRect);
                                    }
                                }
                            }
                            //绘制小时产量
                            if (cbxAllDay.Checked)
                            {
                                //startX += rectWidth / 9.0f;
                            }
                            else
                            {
                                startX += rectWidth / 4.0f;
                            }
                            g.DrawString($"{YieldList[i].Count}", new Font("微软雅黑", FontSize), Brushes.Black, startX, HeightMaxValue * RatioX - heightSum - 15 * RatioX);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"DrawChart方法执行失败，异常信息：{ex}");
                        LogManager.GetLogger("YieldDayNight").Info($"DrawChart方法执行失败，异常信息：{ex}");
                    }
                }
                //一次性绘制所有矩形
                if (rectList.Count > 0)
                {
                    g.DrawRectangles(Pens.Black, rectList.ToArray());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"DrawChart方法执行失败，异常信息：{ex}");
            }
        }
        private void ChartCtrl_Paint(object sender, PaintEventArgs e)
        {
            if (ClassHelper.IsDesingMode() == false)
            {
                DrawChart(e.Graphics);
            }
        }
        /// <summary>
        /// 绘制图例
        /// </summary>
        /// <param name="g"></param>
        private void DrawLegends(Graphics g)
        {
            var legendRects = new List<RectangleF>();
            float leftTopX = 10 * RatioX;
            float leftTopY = 5 * RatioX;
            float rectWidth = 80 * RatioX;
            float rectHeight = 15 * RatioX;
            float heightPitch = 5 * RatioX;
            float heightSum = 0;
            float fontSize = 10 * RatioX;
            for (int i = 0; i < 7; i++)
            {
                var color = Global.GetBinColor((BinLevel)i);
                RectangleF rect = new RectangleF(leftTopX, leftTopY + heightSum, rectWidth, rectHeight);
                legendRects.Add(rect);
                using (SolidBrush brush = new SolidBrush(color))
                {
                    g.FillRectangle(brush, rect);
                }
                g.DrawString($"{(BinLevel)i}", new Font("微软雅黑", fontSize), Brushes.Black, leftTopX + rectWidth + 5 * RatioX, leftTopY + heightSum);
                heightSum += rectHeight + heightPitch;
            }
            g.DrawRectangles(Pens.Black, legendRects.ToArray());
        }

        private void ChartCtrl_SizeChanged(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode() == false)
            {
                HeightMaxValue = this.pictureBox1.Height-50;
                BarWidth = (this.pictureBox1.Width-50.0f) /12;
                RatioY= this.pictureBox1.Height / 750.0f;
                FontSize = 9 * RatioX;
                this.Refresh();
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {
            if (ClassHelper.IsDesingMode() == false)
            {
                DrawLegends(e.Graphics);
            }
        }
        private void DrawScaleX(Graphics g)
        {
            float startX = BarWidth * RatioX;
            var curHour = DateTime.Now.Hour;
            List<int> hoursList = new List<int>();
            if (cbxDayYield.Checked)//白班
            {
                for (int i = 0; i < 12; i++)
                {
                    hoursList.Add(i + Global.DayStartHour);
                }
            }
            else if (cbxNightYield.Checked)//白班
            {
                for (int i = Global.DayEndHour; i < 24; i++)
                {
                    hoursList.Add(i);
                }
                for (int i = 0; i < Global.DayStartHour; i++)
                {
                    hoursList.Add(i);
                }
            }
            else if (cbxAllDay.Checked)//全天
            {
                for (int i = 0; i < 24; i++)
                {
                    hoursList.Add(i);
                }
            }
            for (int i = 0; i < hoursList.Count; i++)
            {
                float tickX = startX + i * BarWidth  * RatioX + BarWidth * RatioX / 2;
                if (cbxAllDay.Checked)
                {
                    tickX = startX+20 + i * BarWidth  * RatioX + BarWidth * RatioX / 2 ;
                }
                g.DrawLine(Pens.Black, tickX, HeightMaxValue * RatioX, tickX, (HeightMaxValue + 5) * RatioX);
                g.DrawString($"{hoursList[i]}点", new Font("微软雅黑", FontSize), Brushes.Black, tickX - 10 * RatioX, HeightMaxValue * RatioX + 10 * RatioX);
            }
        }

        public void RefreshYield(List<DieModel> totalInfoList, YieldType type)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<List<DieModel>, YieldType>(RefreshYield), totalInfoList, type);
            }
            else
            {
                try
                {
                    YieldList = new List<List<DieModel>>();
                    var curHour = DateTime.Now.Hour;
                    switch (type)
                    {
                        case YieldType.Day:
                            {
                                for (int i = 0; i < 24; i++)
                                {
                                    if (i >= Global.DayStartHour && i < Global.DayEndHour)//白班
                                    {
                                        var temp = totalInfoList.Where(p => p.StartTime.Hour == i).ToList();
                                        YieldList.Add(temp);
                                    }
                                }
                            }
                            break;
                        case YieldType.Night:
                            {
                                for (int i = 0; i < 24; i++)
                                {
                                    if ((i >= Global.DayEndHour && i < 24))//夜班
                                    {
                                        var temp = totalInfoList.Where(p => p.StartTime.Hour == i).ToList();
                                        YieldList.Add(temp);
                                    }
                                }
                                for (int i = 0; i < 24; i++)
                                {
                                    if ((i >= 0 && i < Global.DayStartHour))//夜班
                                    {
                                        var temp = totalInfoList.Where(p => p.StartTime.Hour == i).ToList();
                                        YieldList.Add(temp);
                                    }
                                }
                            }
                            break;
                        case YieldType.All:
                            {
                                for (int i = 0; i < 24; i++)
                                {
                                    var temp = totalInfoList.Where(p => p.StartTime.Hour == i).ToList();
                                    YieldList.Add(temp);
                                }
                            }
                            break;
                        default:
                            break;
                    }

                    if (YieldList.Count > 0)
                    {
                        //YieldList.ForEach(p => p.Where(m => m.MainCode != null && m.MainCode != "").ToList().Sort((x, y) => x.MainCode.CompareTo(y.MainCode)));
                        this.pictureBox1.Invalidate();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }

        }

        private void dtPickerDate1_ValueChanged(object sender, EventArgs e)
        {
            Query();
        }

        private void cbxDayYield_ValueChanged(object sender, bool value)
        {
            Query();
        }

        private void YieldDayNight_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode() == false)
            {
                Query();
            }
        }
    }
    public enum YieldType
    {
        Day,
        Night,
        All
    }
}
