
using JC.PTIS.Main;
using JC.PTIS.Main.DB;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.Models.ModelHelper;
using Newtonsoft.Json;
using StdLib.LogHelper;
using StdLib.PCBaseFrame.Extension.Entities;
using StdLib.WindowsControls;
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
using System.Windows.Forms.DataVisualization.Charting;

namespace JC.AutoMating.UserCtrl
{
    public partial class YieldQueryCtrl : UserControl
    {
        public YieldQueryCtrl()
        {
            InitializeComponent();
            InitCtrl();
            InitDgv(dgvData.View);
        }
        List<Color> ColorList = new List<Color>() { Color.Black, Color.Maroon, Color.Red, Color.Salmon, Color.Gold, Color.Chocolate, Color.DarkOrange, Color.Olive, Color.Yellow, Color.YellowGreen, Color.LawnGreen, Color.ForestGreen, Color.Lime, Color.Aquamarine, Color.Teal, Color.Cyan, Color.DeepSkyBlue, Color.DodgerBlue, Color.RoyalBlue, Color.MediumBlue, Color.SlateBlue, Color.BlueViolet, Color.Plum, Color.Fuchsia, Color.DeepPink, Color.Pink };
        bool IsByHour = true;
        int[] OkCount = new int[24];
        int[] NgCount = new int[24];
        int[] TotalCount = new int[24];
        double[] OkRatio = new double[24];
        string[] Title = new string[24];
        SeriesChartType ChartType = SeriesChartType.Column;
         DateTime StartTime;
         DateTime EndTime;
        int SelectedRowIndex = -1;
        private void InitChart(bool isByHour)
        {
            try
            {
                IsByHour = isByHour;
                this.chart1.Series.Clear();
                this.chart1.Legends.Clear();
                this.chart1.ChartAreas.Clear();
                ChartArea chartArea = new ChartArea();
                chartArea.Axes[0].MajorGrid.Enabled = false;
                chartArea.Axes[0].Title = "时间" + (isByHour ? "(小时)" : "(天)");
                chartArea.Axes[0].Interval = 1;
                chartArea.Axes[0].TitleFont = new Font("微软雅黑", 10);


                chartArea.Axes[1].MajorGrid.Enabled = true;
                chartArea.Axes[1].MajorGrid.LineDashStyle = ChartDashStyle.Dash;
                chartArea.Axes[1].MajorGrid.LineColor = Color.Gray;
                chartArea.Axes[1].Title = "数量";
                chartArea.Axes[1].TitleFont = new Font("微软雅黑", 10);

                chartArea.Axes[3].MajorGrid.Enabled = false;
                chartArea.Axes[3].Title = "良率百分比(%)";
                chartArea.Axes[3].Interval = 10;
                chartArea.Axes[3].TitleFont = new Font("微软雅黑", 10);
                chartArea.Axes[3].LineColor = Color.Blue;

                chartArea.BackColor = Color.LightCyan;
                chartArea.Position.Auto = true;
                chartArea.Position.X = 0;
                chartArea.Position.Y = 5;
                chartArea.Position.Width = 95f;
                chartArea.Position.Height = 95f;
                this.chart1.ChartAreas.Add(chartArea);

                Legend legend = new Legend();
                legend.BackColor = Color.FromArgb(235, 243, 255);
                legend.Position.Auto = true;

                legend.Font = new Font("微软雅黑", 9f);
                this.chart1.Legends.Add(legend);


                Series series = new Series();
                series.ChartType = ChartType;
                series.Name = "OK";
                series.Color = Color.DodgerBlue;
                series.IsValueShownAsLabel = true;
                series.BorderWidth = 2;
                this.chart1.Series.Add(series);

                series = new Series();
                series.ChartType = ChartType;
                series.Name = "NG";
                series.Color = Color.IndianRed;
                series.IsValueShownAsLabel = true;
                series.BorderWidth = 2;
                this.chart1.Series.Add(series);

                series = new Series();
                series.ChartType = ChartType;
                series.Name = "Total";
                series.Color = Color.Lime;
                series.IsValueShownAsLabel = true;
                series.BorderWidth = 2;
                this.chart1.Series.Add(series);

                if (ChartType!= SeriesChartType.Pie)
                {
                    series = new Series();
                    series.ChartType = SeriesChartType.Spline;
                    series.Name = "良率";
                    series.Color = Color.Blue;
                    series.IsValueShownAsLabel = true;
                    series.YAxisType = AxisType.Secondary;
                    series.BorderWidth = 3;
                    this.chart1.Series.Add(series);
                }
            }
            catch (Exception ex)
            {
                LogManager.GetLogger("YieldQueryChart").Fatal($"InitChart执行失败，异常信息：{ex}");
            }
        }
        private void DataBind()
        {
            this.chart1.Series[0].Points.DataBindXY(Title, OkCount);
            this.chart1.Series[1].Points.DataBindXY(Title, NgCount);
            this.chart1.Series[2].Points.DataBindXY(Title, TotalCount);
            if (ChartType != SeriesChartType.Pie) this.chart1.Series[3].Points.DataBindXY(Title, OkRatio);

        }
        private void InitDgv(DataGridView dgv)
        {
            dgv.AlternatingRowsDefaultCellStyle.BackColor = System.Drawing.Color.FromArgb(175, 223, 228);
            dgv.RowsDefaultCellStyle.BackColor = System.Drawing.Color.White;
            dgv.DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;

            dgv.BorderStyle = BorderStyle.None;
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("微软雅黑", 12);

            dgv.CellBorderStyle = DataGridViewCellBorderStyle.None;
            dgv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dgv.BorderStyle = BorderStyle.None;

            foreach (var item in dgv.Columns)
            {
                DataGridViewColumn column = item as DataGridViewColumn;
                column.DefaultCellStyle.Font = new Font("微软雅黑", 10);
            }
        }
        private void btnQuery_Click(object sender, EventArgs e)
        {
            try
            {
                dgvData.DataSource = null;
                dgvData.View.Columns.Clear();
                DateTime from = dtPickerDate1.Value.Date + dtPickerTime1.Value.TimeOfDay;
                DateTime to = dtPickerDate2.Value.Date + dtPickerTime2.Value.TimeOfDay;
                if (ckbNow1.Checked)
                {
                    to = DateTime.Now;
                }
                DataSet dataSet1 = null;
                var category = cmxCategory.SelectedItem.ToString();
                switch (category)
                {
                    case "Die":
                        {
                            var result=cmxResultType.SelectedItem.ToString();
                            if (result!="None")
                            {
                                dataSet1 = DieModelHelper.GetDataSet(from, to, result);
                            }
                            else
                            {
                                dataSet1 = DieModelHelper.GetDataSet(from, to);
                            }
                         
                            break;
                        }
                    case "Tray":
                        {
                            dataSet1 = TrayModelHelper.GetDataSet(from, to);
                            break;
                        }
                    case "Wafer":
                        {
                            break;
                        }
                    case "Lot":
                        {
                            dataSet1 = LotModelHelper.GetDataSet(from, to);
                            break;
                        }
                    case "LotBox":
                        {
                            break;
                        }
                    default:
                        break;
                }

                if (dataSet1 != null)
                {
                    dgvData.DataSource = dataSet1.Tables[0];
                    for (int i = 0; i < dgvData.Columns.Count; i++)
                    {
                        dgvData.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                    }
                    //dgvData.Columns["StartTime"].DefaultCellStyle.Format = "yyyy/MM/dd HH:mm:ss";
                    // dgvData.Columns["EndTime"].DefaultCellStyle.Format = "yyyy/MM/dd HH:mm:ss";
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void ckbNow1_CheckedChanged(object sender, EventArgs e)
        {
            dtPickerDate2.Enabled = !ckbNow1.Checked;
            dtPickerTime2.Enabled = !ckbNow1.Checked;
        }
    

        private void htTabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void cmxLegendList_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmxLegendList.SelectedIndex)
            {
                case 0:
                    {
                        ChartType = SeriesChartType.Column;
                        break;
                    }
                case 1:
                    {
                        ChartType = SeriesChartType.Spline;
                        break;
                    }
                case 2:
                    {
                        ChartType = SeriesChartType.Pie;
                        break;
                    }
                default:
                    break;
            }
        }

        private void dtPickerDate1_ValueChanged(object sender, EventArgs e)
        {
           
        }

        private void YieldQueryCtrl_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode()==false)
            {
              
            }
        }
        private void InitCtrl()
        {
            var resultList= Enum.GetNames(typeof(DieStatus)).ToList();
            cmxResultType.DataSource = resultList;
            cmxResultType.SelectedIndex = 0;
            cmxCategory.SelectedIndex = 0;
        }

        private void 删除ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null,"是否确认删除？"))
            {
                try
                {
                    var category = cmxCategory.SelectedItem.ToString();
                    switch (category)
                    {
                        case "Die":
                            {
                                var code = dgvData.View[0, SelectedRowIndex].Value.ToString();
                                DieModelHelper.Delete(code);
                                break;
                            }
                        case "Tray":
                            {
                                var code = dgvData.View[0, SelectedRowIndex].Value.ToString();
                                TrayModelHelper.Delete(code);
                                break;
                            }
                        case "Wafer":
                            {
                                break;
                            }
                        case "Lot":
                            {
                                var code = dgvData.View[0, SelectedRowIndex].Value.ToString();
                                LotModelHelper.Delete(code);
                                break;
                            }
                        case "LotBox":
                            {
                                break;
                            }
                        default:
                            break;
                    }
                    dgvData.View.Rows.RemoveAt(SelectedRowIndex);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.ToString());
                }
            }
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectedRowIndex = e.RowIndex;
        }

        private void 查询DieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                var category = cmxCategory.SelectedItem.ToString();
                switch (category)
                {
                    case "Tray":
                        {
                            var code = dgvData.View[0, SelectedRowIndex].Value.ToString();
                            var trayModel = TrayModelHelper.GetModel(code);
                            if (trayModel != null)
                            {
                                List<DieModel> dieList = new List<DieModel>();
                                for (int r = 0; r < trayModel.RowCount; r++)
                                {
                                    for (int c = 0; c < trayModel.ColCount; c++)
                                    {
                                        var dieModel = DieModelHelper.GetModel(trayModel.DieCodeArray[r, c]);
                                        if (dieModel != null)
                                        {
                                            dieList.Add(dieModel);
                                        }
                                    }
                                }
                                dgvData.DataSource = null;
                                dgvData.DataSource = dieList.ToDataSet().Tables[0];
                                //把dieList转换成DataTable

                                for (int i = 0; i < dgvData.Columns.Count; i++)
                                {
                                    dgvData.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                }
                            }
                            break;
                        }
                    case "Wafer":
                        {
                            break;
                        }
                    case "Lot":
                        {
                            var code = dgvData.View[0, SelectedRowIndex].Value.ToString();
                            var lotModel = LotModelHelper.GetModel(code);
                            if (lotModel != null)
                            {
                                List<TrayModel> trayList = new List<TrayModel>();
                                for (int i = 0; i < lotModel.TrayCodeArray.Length; i++)
                                {
                                    var trayModel = TrayModelHelper.GetModel(lotModel.TrayCodeArray[i]);
                                    if (trayModel != null)
                                    {
                                        trayList.Add(trayModel);
                                    }
                                }
                                //dgvData.View.DataSource = null;
                                //dgvData.View.DataSource = trayList;
                                //for (int i = 0; i < dgvData.Columns.Count; i++)
                                //{
                                //    dgvData.Columns[i].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                                //}
                                ShowList(trayList);
                            }
                            break;
                        }
                    case "LotBox":
                        {
                            break;
                        }
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
        private void ShowList<T>(List<T> list)
        {
            try
            {
                dgvData.Columns.Clear();
                dgvData.DataSource = null;
                if (list.Count > 0)
                {
                    var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);
                    foreach (var property in properties)
                    {
                        DataGridViewColumn col = new DataGridViewTextBoxColumn();
                        col.Name = property.Name;
                        col.HeaderText = property.Name;
                        col.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                        dgvData.Columns.Add(col);
                    }
                    //把list中的元素加到表格中
                    foreach (var item in list)
                    {
                        int rowIndex = dgvData.View.Rows.Add();
                        foreach (var property in properties)
                        {
                            //获取二维数组中的值
                            var value = property.GetValue(item);
                            var array = value as Array;
                            if (array != null && array.Rank == 2)
                            {
                                StringBuilder text = new StringBuilder();
                                var str=JsonConvert.SerializeObject(array);
                                value = str.Replace("\r", "").Replace("\n", "").Replace("[", "").Replace("]", ""); ;
                            }
                            dgvData.View.Rows[rowIndex].Cells[property.Name].Value = value;
                        }
                        rowIndex++;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnClearDataBase_Click(object sender, EventArgs e)
        {
            var category = cmxCategory.SelectedItem.ToString();
            if (UIMessageDialog.ShowAskDialog(null,$"是否清空{category}Table数据表"))
            {
                switch (category)
                {
                    case "Die":
                        {
                            DBHelper.ClearTable<DieModel>();
                            break;
                        }
                    case "Tray":
                        {
                            DBHelper.ClearTable<TrayModel>();
                            break;
                        }
                    case "Wafer":
                        {
                            break;
                        }
                    case "Lot":
                        {
                            DBHelper.ClearTable<LotModel>();
                            break;
                        }
                    case "LotBox":
                        {
                            break;
                        }
                    default:
                        break;
                }
            }
        }

        private void YieldQueryCtrl_VisibleChanged(object sender, EventArgs e)
        {
            if (this.Visible)
            {
                if (UserManager.CurUser != null && UserManager.CurUser.Level == UserLevel.Engineer)
                {
                    btnClearDataTable.Visible = true;
                }
                else
                {
                    btnClearDataTable.Visible = false;
                }
            }
        }
    }
}
