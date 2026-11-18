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
    public partial class TrayCtrl : UserControl
    {
        public TrayCtrl()
        {
            InitializeComponent();
            panel1.DoubleBuffered();
        }
        /// <summary>
        /// 不跟随转塔旋转改变
        /// </summary>
        [Description("所在转盘工位号")]
        public int DDStnNo { get; set; }
        [Description("Tray所在位置")]
        public TrayPosEnum TrayPos { get; set; }
        RectangleF[,] DieArray = null;
        TrayStageInfo TrayStageInfo = null;
        Pen Pen = new Pen(Color.Blue, 1.0f);
         PointF MousePositionRealTime = new PointF(-1, -1);
        [Description("交换行列")]
        public bool IsReverseRC { get; set; }
        private void Init()
        {
            try
            {
                if (TrayStageInfo != null && TrayStageInfo.TrayModel != null)
                {
                    var rectHeight = panel1.Height / ((float)TrayStageInfo.TrayModel.RowCount);
                    var rectWidth = panel1.Width / ((float)TrayStageInfo.TrayModel.ColCount+5.0f);
                    var rowCount = TrayStageInfo.TrayModel.RowCount;
                    var colCount = TrayStageInfo.TrayModel.ColCount;
                    DieArray = new RectangleF[TrayStageInfo.TrayModel.RowCount, TrayStageInfo.TrayModel.ColCount];
                    if (IsReverseRC)
                    {
                        rectWidth = panel1.Width / ((float)TrayStageInfo.TrayModel.RowCount);
                        rectHeight = panel1.Height / ((float)TrayStageInfo.TrayModel.ColCount + 5.0f);
                    }
                    for (int i = 0; i < rowCount; i++)
                    {
                        for (int j = 0; j < colCount; j++)
                        {
                            RectangleF rect = new RectangleF();
                            rect.Width = rectWidth;
                            rect.Height = rectHeight;
                            rect.X = j * (rect.Width + 1);
                            rect.Y = i * (rect.Height + 1);
                            if (IsReverseRC)
                            {
                                rect.X = i * (rect.Width + 1);
                                rect.Y = j * (rect.Height + 1);
                            }
                            DieArray[i, j] = rect;
                        }
                    }
                }
                else
                {
                    DieArray = null;
                }
            }
            catch (Exception ex)
            {

            }
        }

        public void UpdateTrayInfo(TrayStageInfo model)
        {
            try
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action(() => UpdateTrayInfo(model)));
                }
                else
                {
                    TrayStageInfo = model;
                    Init();
                    lblStageNo.Text = model.StageNo.ToString();
                    if (model != null && model.TrayModel != null&& model.TrayModel.BinLevel != BinLevel.None.ToInt())
                    {
                        lblBinLevel.Text = model.TrayModel.BinLevel.ToString();
                        lblSum.Text = model.TrayModel.DieArray.Cast<DieModel>().Count(x => x != null).ToString();
                    }
                    else
                    {
                        lblBinLevel.Text = string.Empty;
                        lblSum.Text = string.Empty;
                    }
                    panel1.Invalidate();
                }
            }
            catch (Exception ex)
            {

            }
        }
        public void UpdateTrayModel(TrayModel model)
        {
            try
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action(() => UpdateTrayModel(model)));
                }
                else
                {
                    TrayStageInfo = new TrayStageInfo();
                    TrayStageInfo.TrayModel = model;
                    Init();
                    if (model != null && model.BinLevel != BinLevel.None.ToInt())
                    {
                        lblBinLevel.Text = model.BinLevel.ToString();
                        lblSum.Text = model.DieArray.Cast<DieModel>().Count(x => x != null).ToString();
                    }
                  
                    panel1.Invalidate();
                }
            }
            catch (Exception ex)
            {

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
            if (DieArray != null && DieArray.Length > 0)
            {
                g.Clear(Color.Gray);
                using (SolidBrush brush = new SolidBrush(Color.White))
                {
                    g.FillRectangles(brush, DieArray.Cast<RectangleF>().ToArray());
                }
                if (TrayStageInfo.TrayModel != null && TrayStageInfo.TrayModel.DieArray != null)
                {
                    for (int i = 0; i < TrayStageInfo.TrayModel.RowCount; i++)
                    {
                        for (int j = 0; j < TrayStageInfo.TrayModel.ColCount; j++)
                        {
                            var color = Color.White;
                            if (TrayStageInfo.TrayModel.DieArray.Length > 0 && TrayStageInfo.TrayModel.DieArray[i, j] != null)
                            {
                                if (TrayStageInfo.TrayModel.DieArray[i, j].DieStatus==DieStatus.Picked||
                                    TrayStageInfo.TrayModel.DieArray[i, j].DieStatus==DieStatus.OK)
                                {
                                    color = Global.GetBinColor(TrayStageInfo.TrayModel.DieArray[i, j].BinLevel);
                                }
                                else
                                {
                                    color = Global.GetColor(TrayStageInfo.TrayModel.DieArray[i, j].DieStatus);
                                }
                            }
                            using (SolidBrush brush = new SolidBrush(color))
                            {
                                g.FillRectangle(brush, DieArray[i, j]);
                            }
                        }
                    }
                    using (SolidBrush brush = new SolidBrush(Color.Blue))
                    {
                        var value = GetHitRect();
                        if (value.r != -1&& value.c != -1)
                        {
                            g.DrawRectangles(Pen, new RectangleF[] { DieArray[value.r, value.c] });
                        }
                    }
                }
            }
        }
        private (int r, int c) GetHitRect()
        {
            int row = -1;
            int col = -1;
            if (DieArray != null)
            {
                for (int r = 0; r < DieArray.GetLength(0); r++)
                {
                    for (int c = 0; c < DieArray.GetLength(1); c++)
                    {
                        var ret = DieArray[r, c].Contains(MousePositionRealTime.X, MousePositionRealTime.Y);
                        if (ret)
                        {
                            row = r;
                            col = c;
                            break;
                        }
                    }
                }
            }
            return (row, col);
        }
        private void TrayCtrl_Load(object sender, EventArgs e)
        {
          
        }

        private void panel1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                var row = GetHitRect();
                if (row.r != -1 && TrayStageInfo != null && TrayStageInfo.TrayModel != null && TrayStageInfo.TrayModel.DieArray.Length>0)
                {
                    var dieModel = TrayStageInfo.TrayModel.DieArray[row.r, row.c];
                    if (dieModel != null)
                    {
                        FrmModelDetail frmModelDetail = new FrmModelDetail();
                        frmModelDetail.ShowInfo(dieModel);
                        frmModelDetail.ShowDialog();


                        if (TrayPos == TrayPosEnum.Stn4TurnTable)
                        {
                            var stnNo = RotateInfoHelper.Instance.GetStnNo(TrayStageInfo.StageNo, RotateStnType.Stn4TurnTable);
                            if (frmModelDetail.Model != null)
                            {
                                TrayStageInfo.TrayModel.DieArray[row.r, row.c] = frmModelDetail.Model as DieModel;
                                ProcInfoHelper.Instance.PlaceDieToTray((Stn4TurnTableStnEnum)stnNo, row.r, row.c, TrayStageInfo.TrayModel.DieArray[row.r, row.c]);
                            }
                            else
                            {
                                ProcInfoHelper.Instance.PickDieFromTray((Stn4TurnTableStnEnum)stnNo, row.r, row.c);
                            }
                        }
                        else
                        {
                            if (frmModelDetail.Model != null)
                            {
                                TrayStageInfo.TrayModel.DieArray[row.r, row.c] = frmModelDetail.Model as DieModel;
                                ProcInfoHelper.Instance.PlaceDieToTray(TrayPos, row.r, row.c, TrayStageInfo.TrayModel.DieArray[row.r, row.c]);
                            }
                            else
                            {
                                ProcInfoHelper.Instance.PickDieFromTray(TrayPos, row.r, row.c);
                            }
                        }
             
                    }
                }
            }
            catch (Exception ex)
            {
                UIMessageDialog.ShowErrorDialog(null, ex.ToString());
            }
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            MousePositionRealTime.X = e.X;
            MousePositionRealTime.Y = e.Y;
            panel1.Invalidate();
        }

        private void 详细信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (TrayStageInfo != null && TrayStageInfo.TrayModel != null)
            {
                FrmModelDetail frmModelDetail = new FrmModelDetail();
                frmModelDetail.ShowInfo(TrayStageInfo.TrayModel);
                frmModelDetail.ShowDialog();
                if (frmModelDetail.IsUpdated)
                {
                    try
                    {
                        if (TrayPos==TrayPosEnum.Stn4TurnTable)
                        {
                            var stnNo = RotateInfoHelper.Instance.GetStnNo(TrayStageInfo.StageNo, RotateStnType.Stn4TurnTable);
                            if (frmModelDetail.Model != null)
                            {
                                TrayStageInfo.TrayModel = frmModelDetail.Model as TrayModel;
                                ProcInfoHelper.Instance.PutTrayToTrayStage((Stn4TurnTableStnEnum)stnNo, TrayStageInfo.TrayModel);
                            }
                            else
                            {
                                ProcInfoHelper.Instance.PickTrayFromTrayStage((Stn4TurnTableStnEnum)stnNo);
                                if (Global.RunMode == RunMode.空跑模式)
                                {
                                    IOInfoHelper.GetInputInfoByName($"TrayStage{TrayStageInfo.StageNo + 1}VacDetect", out int _).IsShield = false;
                                }
                            }
                        }
                        else
                        {
                            if (frmModelDetail.Model != null)
                            {
                                TrayStageInfo.TrayModel = frmModelDetail.Model as TrayModel;
                                ProcInfoHelper.Instance.PutTrayToTrayStage(TrayPos, TrayStageInfo.TrayModel);
                            }
                            else
                            {
                                ProcInfoHelper.Instance.PickTrayFromTrayStage(TrayPos);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        UIMessageDialog.ShowErrorDialog(null, ex.ToString());
                    }
                }
            }
        }

        private void 清除信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否清除Tray信息"))
            {
                if (TrayPos == TrayPosEnum.Stn4TurnTable)
                {
                    var stnNo = RotateInfoHelper.Instance.GetStnNo(TrayStageInfo.StageNo, RotateStnType.Stn4TurnTable);
                    ProcInfoHelper.Instance.PickTrayFromTrayStage((Stn4TurnTableStnEnum)stnNo);
                    if (Global.RunMode == RunMode.空跑模式)
                    {
                        IOInfoHelper.GetInputInfoByName($"TrayStage{TrayStageInfo.StageNo + 1}VacDetect", out int _).IsShield = false;
                    }
                }
                else
                {
                    ProcInfoHelper.Instance.PickTrayFromTrayStage(TrayPos);

                }
            }
        }

        private void TrayCtrl_SizeChanged(object sender, EventArgs e)
        {

        }
    }
}
