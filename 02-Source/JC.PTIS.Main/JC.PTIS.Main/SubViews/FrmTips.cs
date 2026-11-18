using JC.AutoMating.Entities;
using JC.PTIS.Main.Properties;
using StdLib.LogHelper;
using StdLib.PCBaseFrame.Extension.Entities;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using static JC.AutoMating.Entities.FrmTipHelper;

namespace JC.B17AutoMating.SubForms
{
    public partial class FrmTips : Form
    {
        private ContentAlignment m_showAlign = ContentAlignment.BottomLeft;

        private static List<FrmTips> m_lstTips = new List<FrmTips>();

        private int m_CloseTime = 0;

        private static KeyValuePair<string, FrmTips> m_lastTips = default(KeyValuePair<string, FrmTips>);
        static FrmTips FrmTip = new FrmTips();

        public static Form MainForm;
        public ContentAlignment ShowAlign
        {
            get
            {
                return m_showAlign;
            }
            set
            {
                m_showAlign = value;
            }
        }

        public int CloseTime
        {
            get
            {
                return m_CloseTime;
            }
            set
            {
                m_CloseTime = value;
                if (value > 0)
                {
                    timer1.Interval = value;
                }
            }
        }

        public FrmTips()
        {
            SetStyle(ControlStyles.UserPaint, value: true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, value: true);
            SetStyle(ControlStyles.DoubleBuffer, value: true);
            InitializeComponent();
        }

        public static void ClearTips()
        {
            for (int num = m_lstTips.Count - 1; num >= 0; num--)
            {
                FrmTips xFrmTips = m_lstTips[num];
                if (!xFrmTips.IsDisposed)
                {
                    xFrmTips.Close();
                    xFrmTips.Dispose();
                }
            }
            m_lstTips.Clear();
        }

        public void ResetTimer()
        {
            if (m_CloseTime > 0)
            {
                timer1.Enabled = false;
                timer1.Enabled = true;
            }
        }

        public static FrmTips ShowTips(Form frm,
            string strMsg,
            int intAutoColseTime = 0,
            bool blnShowCoseBtn = true,
            ContentAlignment align = ContentAlignment.BottomLeft,
            Point? point = null,
            TipsSizeMode mode = TipsSizeMode.Small,
            Size? size = null,
            TipsState state = TipsState.Default)
        {
            FrmTips xFrmTips = new FrmTips();
            if (m_lastTips.Key == strMsg + state && !m_lastTips.Value.IsDisposed && m_lastTips.Value.Visible)
            {
                m_lastTips.Value.ResetTimer();
                return m_lastTips.Value;
            }


            switch (mode)
            {
                case TipsSizeMode.Small:
                    xFrmTips.Size = new Size(350, 35);
                    break;

                case TipsSizeMode.Medium:
                    xFrmTips.Size = new Size(350, 50);
                    break;

                case TipsSizeMode.Large:
                    xFrmTips.Size = new Size(350, 65);
                    break;

                case TipsSizeMode.None:
                    if (!size.HasValue)
                    {
                        xFrmTips.Size = new Size(350, 35);
                    }
                    else
                    {
                        xFrmTips.Size = size.Value;
                    }

                    break;
            }

            xFrmTips.BackColor = Color.FromArgb((int)state);
            if (state == TipsState.Default)
            {
                xFrmTips.lblMsg.ForeColor = SystemColors.ControlText;
            }
            else
            {
                xFrmTips.lblMsg.ForeColor = Color.White;
            }

            switch (state)
            {
                case TipsState.Default:
                    xFrmTips.pctStat.Image = Resources.warning;
                    break;

                case TipsState.Success:
                    xFrmTips.pctStat.Image = Resources.success;
                    break;

                case TipsState.Info:
                    xFrmTips.pctStat.Image = Resources.warning;
                    break;

                case TipsState.Warning:
                    xFrmTips.pctStat.Image = Resources.warning;
                    break;

                case TipsState.Error:
                    xFrmTips.pctStat.Image = Resources.error;
                    break;

                default:
                    xFrmTips.pctStat.Image = Resources.warning;
                    break;
            }

            xFrmTips.lblMsg.Text = strMsg;
            xFrmTips.CloseTime = intAutoColseTime;
            xFrmTips.btnClose.Visible = blnShowCoseBtn;
            xFrmTips.ShowAlign = align;
            xFrmTips.Owner = frm;
            m_lstTips.Add(xFrmTips);
            ReshowTips();
            xFrmTips.Show();
            if (frm != null && !frm.IsDisposed)
            {
                FrmTipHelper.SetForegroundWindow(frm.Handle);
            }

            m_lastTips = new KeyValuePair<string, FrmTips>(strMsg + state, xFrmTips);
            return xFrmTips;
        }

        public static void ReshowTips()
        {
            lock (m_lstTips)
            {
                m_lstTips.RemoveAll((FrmTips p) => p.IsDisposed);
                var enumerable = from p in m_lstTips
                                 group p by new { p.ShowAlign };
                Screen currentScreen = Screen.PrimaryScreen;
                FrmTips xFrmTips = m_lstTips.FirstOrDefault();
                if (xFrmTips != null && xFrmTips.Owner != null)
                {
                    currentScreen = Screen.FromControl(xFrmTips.Owner);
                }

                Size size = currentScreen.Bounds.Size;
                foreach (var item in enumerable)
                {
                    List<FrmTips> list = m_lstTips.FindAll((FrmTips p) => p.ShowAlign == item.Key.ShowAlign);
                    for (int i = 0; i < list.Count; i++)
                    {
                        FrmTips frmTips = list[i];
                        if (frmTips.InvokeRequired)
                        {
                            frmTips.BeginInvoke((MethodInvoker)delegate
                            {
                                switch (item.Key.ShowAlign)
                                {
                                    case ContentAlignment.BottomCenter:
                                        frmTips.Location = FrmTipHelper.GetScreenLocation(currentScreen, (size.Width - frmTips.Width) / 2, size.Height - 100 - (i + 1) * (frmTips.Height + 10));
                                        break;

                                    case ContentAlignment.BottomLeft:
                                        frmTips.Location = FrmTipHelper.GetScreenLocation(currentScreen, 10, size.Height - 100 - (i + 1) * (frmTips.Height + 10));
                                        break;

                                    case ContentAlignment.BottomRight:
                                        frmTips.Location = FrmTipHelper.GetScreenLocation(currentScreen, size.Width - frmTips.Width - 10, size.Height - 100 - (i + 1) * (frmTips.Height + 10));
                                        break;

                                    case ContentAlignment.MiddleCenter:
                                        frmTips.Location = FrmTipHelper.GetScreenLocation(currentScreen, (size.Width - frmTips.Width) / 2, size.Height - (size.Height - list.Count * (frmTips.Height + 10)) / 2 - (i + 1) * (frmTips.Height + 10));
                                        break;

                                    case ContentAlignment.MiddleLeft:
                                        frmTips.Location = FrmTipHelper.GetScreenLocation(currentScreen, 10, size.Height - (size.Height - list.Count * (frmTips.Height + 10)) / 2 - (i + 1) * (frmTips.Height + 10));
                                        break;

                                    case ContentAlignment.MiddleRight:
                                        frmTips.Location = FrmTipHelper.GetScreenLocation(currentScreen, size.Width - frmTips.Width - 10, size.Height - (size.Height - list.Count * (frmTips.Height + 10)) / 2 - (i + 1) * (frmTips.Height + 10));
                                        break;

                                    case ContentAlignment.TopCenter:
                                        frmTips.Location = FrmTipHelper.GetScreenLocation(currentScreen, (size.Width - frmTips.Width) / 2, 10 + (i + 1) * (frmTips.Height + 10));
                                        break;

                                    case ContentAlignment.TopLeft:
                                        frmTips.Location = FrmTipHelper.GetScreenLocation(currentScreen, 10, 10 + (i + 1) * (frmTips.Height + 10));
                                        break;

                                    case ContentAlignment.TopRight:
                                        frmTips.Location = FrmTipHelper.GetScreenLocation(currentScreen, size.Width - frmTips.Width - 10, 10 + (i + 1) * (frmTips.Height + 40));
                                        break;
                                }
                            });
                        }
                        else
                        {
                            switch (item.Key.ShowAlign)
                            {
                                case ContentAlignment.BottomCenter:
                                    frmTips.Location = FrmTipHelper.GetScreenLocation(currentScreen, (size.Width - frmTips.Width) / 2, size.Height - 100 - (i + 1) * (frmTips.Height + 10));
                                    break;

                                case ContentAlignment.BottomLeft:
                                    frmTips.Location = FrmTipHelper.GetScreenLocation(currentScreen, 10, size.Height - 100 - (i + 1) * (frmTips.Height + 10));
                                    break;

                                case ContentAlignment.BottomRight:
                                    frmTips.Location = FrmTipHelper.GetScreenLocation(currentScreen, size.Width - frmTips.Width - 10, size.Height - 100 - (i + 1) * (frmTips.Height + 10));
                                    break;

                                case ContentAlignment.MiddleCenter:
                                    frmTips.Location = FrmTipHelper.GetScreenLocation(currentScreen, (size.Width - frmTips.Width) / 2, size.Height - (size.Height - list.Count * (frmTips.Height + 10)) / 2 - (i + 1) * (frmTips.Height + 10));
                                    break;

                                case ContentAlignment.MiddleLeft:
                                    frmTips.Location = FrmTipHelper.GetScreenLocation(currentScreen, 10, size.Height - (size.Height - list.Count * (frmTips.Height + 10)) / 2 - (i + 1) * (frmTips.Height + 10));
                                    break;

                                case ContentAlignment.MiddleRight:
                                    frmTips.Location = FrmTipHelper.GetScreenLocation(currentScreen, size.Width - frmTips.Width - 10, size.Height - (size.Height - list.Count * (frmTips.Height + 10)) / 2 - (i + 1) * (frmTips.Height + 10));
                                    break;

                                case ContentAlignment.TopCenter:
                                    frmTips.Location = FrmTipHelper.GetScreenLocation(currentScreen, (size.Width - frmTips.Width) / 2, 10 + (i + 1) * (frmTips.Height + 10));
                                    break;

                                case ContentAlignment.TopLeft:
                                    frmTips.Location = FrmTipHelper.GetScreenLocation(currentScreen, 10, 10 + (i + 1) * (frmTips.Height + 10));
                                    break;

                                case ContentAlignment.TopRight:
                                    frmTips.Location = FrmTipHelper.GetScreenLocation(currentScreen, size.Width - frmTips.Width - 10, 10 + (i + 1) * (frmTips.Height + 40));
                                    break;
                            }
                        }
                    }
                }
            }
        }

        private void FrmTips_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (m_lastTips.Value == this)
            {
                m_lastTips = default(KeyValuePair<string, FrmTips>);
            }

            m_lstTips.Remove(this);
            ReshowTips();
        }

        private void FrmTips_Load(object sender, EventArgs e)
        {
            if (m_CloseTime > 0)
            {
                timer1.Interval = m_CloseTime;
                timer1.Enabled = true;
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            timer1.Enabled = false;
            Close();
        }

        private void btnClose_MouseDown(object sender, MouseEventArgs e)
        {
            timer1.Enabled = false;
            Close();
        }

        public static void ShowTipsSuccess(Form frm, string strMsg)
        {
            if (MainForm != null && MainForm.InvokeRequired)
            {
                MainForm.Invoke(new Action<Form, string>(ShowTipsSuccess), frm, strMsg);
            }
            else
            {
                ShowTips(frm, strMsg, 3000, blnShowCoseBtn: false, ContentAlignment.TopRight, null, TipsSizeMode.Large, null, TipsState.Success);
            }
        }

        public static void ShowTipsError(Form frm, string strMsg)
        {
            if (MainForm != null && MainForm.InvokeRequired)
            {
                MainForm.Invoke(new Action<Form, string>(ShowTipsError), MainForm, strMsg);
            }
            else
            {
                if (string.IsNullOrEmpty(strMsg) == false && UserManager.CurUser != null)
                {
                    LogManager.GetLogger("InterLockMonitor").Error($"InterLock:[{strMsg}] 用户:[{UserManager.CurUser.Name}]");
                }
                ShowTips(MainForm, strMsg, 3000, blnShowCoseBtn: false, ContentAlignment.TopRight, null, TipsSizeMode.Large, null, TipsState.Error);
            }
        }

        public static void ShowTipsInfo(Form frm, string strMsg)
        {
            if (MainForm != null && MainForm.InvokeRequired)
            {
                MainForm.Invoke(new Action<Form, string>(ShowTipsInfo), MainForm, strMsg);
            }
            else
            {
                ShowTips(MainForm, strMsg, 3000, blnShowCoseBtn: false, ContentAlignment.TopRight, null, TipsSizeMode.Large, null, TipsState.Info);
            }
        }

        public static void ShowTipsWarning(Form frm, string strMsg)
        {
            if (MainForm != null && MainForm.InvokeRequired)
            {
                MainForm.Invoke(new Action<Form, string>(ShowTipsWarning), MainForm, strMsg);
            }
            else
            {
                ShowTips(MainForm, strMsg, 3000, blnShowCoseBtn: false, ContentAlignment.TopRight, null, TipsSizeMode.Large, null, TipsState.Warning);
            }
        }

        private void FrmTips_FormClosed(object sender, FormClosedEventArgs e)
        {
            Dispose();
            GC.Collect();
        }

    }
}