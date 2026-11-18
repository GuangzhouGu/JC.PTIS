using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Properties;
using JC.PTIS.Main.Service;
using StdLib.LogHelper;
using StdLib.PCBaseFrame.Extension.Entities;
using StdLib.PCBaseFrame.Motion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.PTIS.Main.UserCtrls
{
    public partial class RegisterSignalMonitorCtrl : UserControl
    {
        public RegisterSignalMonitorCtrl()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            InitDgv(dgv_InputInfo);
        }
        int RowCountPerPage = 30;
        int currentPage = 0;
        int MaxPage = 0;
        bool[] LastBits = null;
        List<IOInfo> InputList = new List<IOInfo>();
        int CurrentPage
        {
            get
            {
                return currentPage;
            }
            set
            {
                if (value >= 0 && value < MaxPage)
                {
                    currentPage = value;
                    tbxPage.Text = (currentPage + 1).ToString();
                    if (currentPage == 0)
                    {
                        btnPre.Enabled = false;
                    }
                    else
                    {
                        btnPre.Enabled = true;
                    }
                    if (currentPage == MaxPage - 1)
                    {
                        btnNext.Enabled = false;
                    }
                    else
                    {
                        btnNext.Enabled = true;
                    }
                }
            }
        }
        private void UpdateDgv()
        {
            dgv_InputInfo.Rows.Clear();
            var tempList = InputList.Skip(CurrentPage * RowCountPerPage).Take(RowCountPerPage).ToList();
            foreach (var info in tempList)
            {
                int index = dgv_InputInfo.Rows.Add();
                dgv_InputInfo[0,index].Value = info.IONum;
                dgv_InputInfo[1,index].Value = info.IOName;
                dgv_InputInfo[2, index].Value = info.IONameChn;
            }
            LastBits = new bool[dgv_InputInfo.Rows.Count];
        }
        private void InitDgv(DataGridView dgv)
        {
            dgv.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgv.RowsDefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            var dgvType = dgv.GetType();
            var properInfo = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            properInfo.SetValue(dgv, true, null);
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.BorderStyle = BorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 10);
            foreach (var item in dgv.Columns)
            {
                DataGridViewColumn column = item as DataGridViewColumn;
                column.DefaultCellStyle.Font = new Font("微软雅黑", 9);
            }
        }
        private void LoadRegister()
        {
            var signals = Enum.GetNames(typeof(RegisterEnum));
            var descList = EnumHelper.EnumDescToList<RegisterEnum>();
            for (short i = 0; i < signals.Length-1; i++)
            {
                InputList.Add(new IOInfo() { IONum = i, IOName = signals[i], IsShield = false ,IONameChn= descList[i].Name });
            }
        }

        private void RegisterSignalMonitorCtrl_Load(object sender, EventArgs e)
        {
			if(ClassHelper.IsDesingMode()==false)
			{
				 LoadRegister();
                tbxTotalRowCount.Text = InputList.Count.ToString();
                MaxPage = InputList.Count / RowCountPerPage;
                if (InputList.Count % RowCountPerPage != 0)
                {
                    MaxPage += 1;
                }
                tbxTotalPages.Text = MaxPage.ToString();
                UpdateDgv();
                Monitor();
            }
        }
        private void Monitor()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    if (this.Visible)
                    {
                        try
                        {
                            if (!this.IsDisposed && this.Created)
                            {
                                Invoke(new Action(() =>
                                {
                                    for (int i = 0; i < dgv_InputInfo.Rows.Count; i++)
                                    {
                                        var index = int.Parse(dgv_InputInfo[0, i].Value.ToString());
                                        var value = RegisterService.Instance.GetBoolReg((RegisterEnum)index);
                                        if (value != LastBits[i])
                                        {
                                            dgv_InputInfo[3, i].Value = value ? Resources.Green2 : Resources.Gray2;
                                            LastBits[i] = value;
                                        }
                                    }
                                }));
                            }
                        }
                        catch (Exception ex)
                        {
                            LogManager.GetLogger("RegisterSignalMonitorCtrl").Info($"Monitor执行异常，异常信息：{ex}");
                        }
                        Thread.Sleep(200);
                    }
                    else
                    {
                        Thread.Sleep(500);
                    }
                }
            });

        }
        private void btnPre_Click(object sender, EventArgs e)
        {
            CurrentPage--;
            UpdateDgv();
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            CurrentPage++;
            UpdateDgv();
        }
        private void dgv_OutputInfo_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (dgv_InputInfo.Columns[e.ColumnIndex].Name == "ForceOutput")
                {
                    var index = int.Parse(dgv_InputInfo[0, e.RowIndex].Value.ToString());
                    var value=RegisterService.Instance.GetBoolReg((RegisterEnum)index);
                    if (value)
                    {
                        RegisterService.Instance.ResetBoolReg((RegisterEnum)index);
                    }
                    else
                    {
                        RegisterService.Instance.SetBoolReg((RegisterEnum)index);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
    }
}
