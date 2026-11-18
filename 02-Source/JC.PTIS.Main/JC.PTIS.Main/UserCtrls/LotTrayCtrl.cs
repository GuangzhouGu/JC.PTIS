using JC.PTIS.Main.Entities;
using JC.PTIS.Main.FlowInfo;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.RotateInfo;
using JC.PTIS.Main.SubViews;
using StdLib.PCBaseFrame.Extension.Entities;
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
    public partial class LotTrayCtrl : UserControl
    {
        public LotTrayCtrl()
        {
            InitializeComponent();
            panel1.DoubleBuffered();
        }
        [Description("所在转盘工位号")]
        public string DDStnNo { get; set; }
        PointF LeftTop = new PointF(1, 1);
        public LotPosEnum LotPos { get; set; }
        int TotalTrayCount = 6;
        List<RectangleF> RectList = null;
        float SlotInterval = 10.0f;
        LotStageInfo LotStageInfo = null;
         PointF MousePositionRealTime = new PointF(0, 0);
        Pen Pen = new Pen(Color.Blue, 1.0f);
        public void UpdateLotInfo(LotStageInfo lotStageInfo)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<LotStageInfo>(UpdateLotInfo), lotStageInfo);
            }
            else
            {
                LotStageInfo = lotStageInfo;
                if (lotStageInfo != null)
                {
                    lblStageNo.Text = lotStageInfo.StageNo.ToString();
                    SlotInterval = (float)Math.Round((panel1.Height) * 1.0f / (TotalTrayCount + 1), 3);
                    if (lotStageInfo.LotModel != null)
                    {
                        lblTrayCount.Text = lotStageInfo.LotModel.TrayCount.ToString();
                    }
                    lblBinLevel.Text = lotStageInfo.LotStageType.GetDescription(); ;
                    if (lotStageInfo.IsBuzy)
                    {
                        htTitlePanel1.FillColor = Color.LightGreen;
                    }
                    else
                    {
                        htTitlePanel1.FillColor = Color.FromArgb(235, 243, 255); ;
                    }
                }
                else
                {
                    lblTrayCount.Text = "0";
                    lblBinLevel.Text = "";
                    htTitlePanel1.FillColor = Color.FromArgb(235, 243, 255); ;
                }
                panel1.Invalidate();
            }
        }
        public void UpdateLotInfoEx(LotModel lotModel)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<LotModel>(UpdateLotInfoEx), lotModel);
            }
            else
            {
                if (lotModel != null)
                {
                    LotStageInfo = new LotStageInfo();
                    LotStageInfo.LotModel = lotModel;
                    SlotInterval = (float)Math.Round((panel1.Height) * 1.0f / (TotalTrayCount + 1), 3);
                    lblTrayCount.Text = lotModel.TrayCount.ToString();
                    lblBinLevel.Text = lotModel.BinLevel.ToString();
                }
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
            g.Clear(Color.White);
            if (LotStageInfo != null && LotStageInfo.LotModel!=null)
            {
                RectList=new List<RectangleF>();
                #region 绘制Tray
                for (int i = 0; i < TotalTrayCount; i++)
                {
                    using (SolidBrush bh = new SolidBrush(Color.Blue))
                    {
                        g.DrawString((TotalTrayCount - i).ToString(), new Font("微软雅黑", 6), bh, LeftTop.X, LeftTop.Y + SlotInterval * i);
                    }
                }

                var arr = LotStageInfo.LotModel.LoadMap();
                if (arr != null)
                {
                    for (int i = 0; i < LotStageInfo.LotModel.TrayList.Count; i++)
                    {
                        var rect = new RectangleF(LeftTop.X + 10, LeftTop.Y + SlotInterval *(TotalTrayCount - i-1) + 5, panel1.Width - 15, SlotInterval/2.0f);
                        Color color = Color.White;
                        if (LotStageInfo.LotModel.TrayList[i]!=null)
                        {
                            if (LotStageInfo.LotModel.TrayList[i].TrayType == TrayTypeEnum.Empty)
                            {
                                color = Color.Gray;
                            }
                            else if (arr[0] != "-1"&& LotStageInfo.LotModel.TrayList[i].BinLevel !=0)
                            {
                                var temp = $"Bin{LotStageInfo.LotModel.TrayList[i].BinLevel}";
                                var bin = EnumHelper.GetEnumValue<BinLevel>(temp);
                                // color = Global.GetBinColor(LotStageInfo.LotModel.BinLevel);
                                color = Global.GetBinColor(bin);
                            }
                            SolidBrush bh = new SolidBrush(color);
                            g.FillRectangle(bh, rect);
                            RectList.Add(rect);
                            bh.Dispose();
                        }
                    }
                    //g.DrawLine(Pen, MousePositionRealTime.X, 0, MousePositionRealTime.X, this.panel1.Height);
                    //g.DrawLine(Pen, 0, MousePositionRealTime.Y, this.panel1.Width, MousePositionRealTime.Y);
                    using (SolidBrush brush = new SolidBrush(Color.Blue))
                    {
                        var value = GetHitRect();
                        if (value != -1 )
                        {
                            g.DrawRectangles(Pen, new RectangleF[] { RectList[value] });
                        }
                    }
                }
            
                #endregion
            }

        }
        private int GetHitRect()
        {
            int row = -1;
            if (RectList != null)
            {
                for (int i = 0; i < RectList.Count; i++)
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
            try
            {
                var row = GetHitRect();
                if (row != -1 && LotStageInfo != null && LotStageInfo.LotModel != null && LotStageInfo.LotModel.TrayList.Count > row)
                {
                    var trayModel = LotStageInfo.LotModel.TrayList[row];
                    if (trayModel != null)
                    {
                        FrmModelDetail frmModelDetail = new FrmModelDetail();
                        frmModelDetail.ShowInfo(trayModel);
                        frmModelDetail.ShowDialog();
                        if (frmModelDetail.IsUpdated)
                        {
                            try
                            {
                                if (LotPos==LotPosEnum.Stn8TurnTable)
                                {
                                    var stnNo = RotateInfoHelper.Instance.GetStnNo(LotStageInfo.StageNo, RotateStnType.Stn8TurnTable);
                                    if (frmModelDetail.Model != null)
                                    {
                                        LotStageInfo.LotModel.TrayList[row] = frmModelDetail.Model as TrayModel;
                                        ProcInfoHelper.Instance.LotStageInfoUpdated((Stn8TurnTableStnEnum)stnNo, LotStageInfo);
                                    }
                                    else
                                    {
                                        ProcInfoHelper.Instance.PickLotFromLotStage((Stn8TurnTableStnEnum)stnNo);
                                    }
                                }
                                else
                                {
                                    if (frmModelDetail.Model != null)
                                    {
                                        LotStageInfo.LotModel.TrayList[row] = frmModelDetail.Model as TrayModel;
                                        ProcInfoHelper.Instance.LotStageInfoUpdated(LotPos, LotStageInfo);
                                    }
                                    else
                                    {
                                        ProcInfoHelper.Instance.PickLotFromLotStage(LotPos);
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
            }
            catch (Exception ex)
            {
                UIMessageDialog.ShowErrorDialog(null,ex.ToString());
            }
        }

        private void 详细信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (LotStageInfo != null && LotStageInfo.LotModel != null)
            {
                FrmModelDetail frmModelDetail = new FrmModelDetail();
                frmModelDetail.ShowInfo(LotStageInfo.LotModel);
                frmModelDetail.ShowDialog();
            }
        }

        private void 清空信息ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null,"是否清除LotModel信息，清除后无法恢复"))
            {
                ProcInfoHelper.Instance.PickLotFromLotStage(LotPos);
            }
        }
    }
}
