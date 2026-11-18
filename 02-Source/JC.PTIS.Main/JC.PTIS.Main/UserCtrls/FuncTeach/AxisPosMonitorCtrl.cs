using JC.PTIS.Main.MotionInfo;
using StdLib.PCBaseFrame.Extension.Entities;
using StdLib.PCBaseFrame.Motion;
using StdLib.PCBaseFrame.Station;
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
using static System.Collections.Specialized.BitVector32;

namespace JC.PTIS.Main.UserCtrls.FuncTeach
{
    public partial class AxisPosMonitorCtrl : UserControl
    {
        public AxisPosMonitorCtrl()
        {
            InitializeComponent();
            InitCtrl();
        }
        StationName[] StationList = null;
        StationName StationName = StationName.None;
        Timer Timer = new Timer();
        private void InitCtrl()
        {
            try
            {
                InitDgv();
                StationList = Enum.GetValues(typeof(StationName)).Cast<StationName>().ToArray();
                var descList = EnumHelper.EnumDescToList<StationName>();
                cmxStationList.Items.Clear();
                cmxStationList.Items.AddRange(descList.Select(p => p.Name).ToArray());
                if (cmxStationList.Items.Count > 0)
                {
                    cmxStationList.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void InitDgv()
        {
            dgvAxisList.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.LightBlue;
            dgvAxisList.RowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(235, 243, 255);
            dgvAxisList.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
            var dgvType = dgvAxisList.GetType();
            var properInfo = dgvType.GetProperty("DoubleBuffered", BindingFlags.Instance | BindingFlags.NonPublic);
            properInfo.SetValue(dgvAxisList, true, null);
            ClearDgvBorderStyle(dgvAxisList);
        }
        private void ClearDgvBorderStyle(DataGridView dgv)
        {
            dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.BorderStyle = BorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 12);
            foreach (var item in dgv.Columns)
            {
                DataGridViewColumn column = item as DataGridViewColumn;
                column.DefaultCellStyle.Font = new Font("微软雅黑", 12);
            }
        }
        public void UpdateStationName(StationName name)
        {
            StationName = name;
            var strName = EnumHelper.GetEnumDesc(name);
            if (!string.IsNullOrEmpty(strName))
            {
                cmxStationList.Text = strName;
            }
            var stationInfo = StationManager.Instance.GetStationInfo(name.ToString());
            if (stationInfo != null && stationInfo.AxisList != null)
            {
                dgvAxisList.Rows.Clear();
                foreach (var axis in stationInfo.AxisList)
                {
                    dgvAxisList.Rows.Add(
                    axis.AxisName, axis.ActPos);
                }
            }
        }
        private void cmxStationList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var station = StationList[cmxStationList.SelectedIndex];
            UpdateStationName(station);
        }

        private void AxisPosMonitorCtrl_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode() == false)
            {
                Timer.Interval = 200;
                Timer.Tick += Timer_Tick;
                Timer.Start();
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                //刷新轴位置
                var stationInfo = StationManager.Instance.GetStationInfo(StationName.ToString());
                if (stationInfo != null && stationInfo.AxisList != null)
                {
                    foreach (DataGridViewRow row in dgvAxisList.Rows)
                    {
                        var axisName = row.Cells[0].Value.ToString();
                        var axisInfo = stationInfo.AxisList.FirstOrDefault(p => p.AxisName == axisName);
                        if (axisInfo != null)
                        {
                            row.Cells[1].Value = axisInfo.ActPos;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
    }
}
