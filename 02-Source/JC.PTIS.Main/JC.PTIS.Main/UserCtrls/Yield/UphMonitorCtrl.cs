using JC.LidAttach;
using JC.PTIS.Main;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.Models.ModelHelper;
using StdLib.LogHelper;
using StdLib.PCBaseFrame.Extension.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.LidDispenser.UserCtrl.Yield
{
    public partial class UphMonitorCtrl : UserControl
    {
        public UphMonitorCtrl()
        {
            InitializeComponent();
            DieModelHelper.UpdateTotalCount += DieModelDal_UpdateTotalCount;
        }
        int OkCount=0, NgCount = 0, Total = 0;
        bool IsClearYieldComp;
        System.Windows.Forms.Timer Timer = new System.Windows.Forms.Timer();
        private void LoadYield()
        {
            Total = 0;
            try
            {
                string startTime = DateTime.Now.ToString("yyyy-MM-dd ") + "00:00:00";
                //白班产能
                if (DateTime.Now.Hour >= Global.DayStartHour && DateTime.Now.Hour < Global.DayEndHour)
                {
                    startTime = DateTime.Now.ToString("yyyy-MM-dd ") + $"{Global.DayStartHour.ToString("00")}:00:00";
                }
                else
                {
                    //当天夜班产能从夜班开始时间查询
                    if (DateTime.Now.Hour >= Global.DayEndHour)
                    {
                        startTime = DateTime.Now.ToString("yyyy-MM-dd ") + $"{Global.DayEndHour.ToString("00")}:00:00";
                    }
                    else//超过凌晨12点，夜班产能要从昨天夜班开始时间查询
                    {
                        startTime = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd ") + $"{Global.DayEndHour.ToString("00")}:00:00";
                    }
                }
                var list = DieModelHelper.GetByTimeRange(DateTime.Parse(startTime), DateTime.Now);
                if (list != null && list.Count > 0)
                {
                    list.Sort((x, y) => x.StartTime.CompareTo(y.StartTime));
                    OkCount = list.Where(x => x.TotalResult == Result.OK).Count();
                    NgCount = list.Where(x => x.TotalResult == Result.NG).Count();
                    Total = OkCount + NgCount;
                    lblOkCount.Text = OkCount.ToString();
                    lblNgCount.Text = NgCount.ToString();
                    lblTotal.Text = Total.ToString();
                    var endTime2 = list.Last().StartTime;
                    ShowUPH(endTime2.AddHours(-1).AddMinutes(-5), endTime2.AddMinutes(5));
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void ShowCount(DieModel obj,bool isCalcUph)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<DieModel,bool>(ShowCount),obj, isCalcUph);
            }
            else
            {
                if (obj.TotalResult==Result.OK)
                {
                    OkCount++;
                }
                else if (obj.TotalResult == Result.NG)
                {
                    NgCount++;
                }
                lblOkCount.Text = OkCount.ToString();
                lblNgCount.Text = NgCount.ToString();
                lblTotal.Text = Total.ToString();
                if (isCalcUph)
                {
                    ShowUPH(obj.StartTime.AddHours(-1),DateTime.Now);
                }
            }
        }
     
        private void DieModelDal_UpdateTotalCount(DieModel obj)
        {
            if (obj.TotalResult == Result.None)
            {
                Total++;//记录总数
            }
            ShowCount(obj, true);
        }
        private void ShowUPH(DateTime startTime,DateTime endTime)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<DateTime,DateTime>(ShowUPH), startTime, endTime);
            }
            else
            {
                var list = DieModelHelper.GetByTimeRange(startTime, endTime);
                if (list != null)
                {
                    var count = list.Count;
                    lblUph.Text = count.ToString();
                }
            }
        }
        private void UphMonitorCtrl_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode()==false)
            {
                DelayLoadYield();
                Timer.Interval = 1000 * 30;
                Timer.Tick += Timer_Tick;
                Timer.Start();
            }
        }
        private async void DelayLoadYield()
        {
            await Task.Factory.StartNew(() => { Thread.Sleep(1000); });
            LoadYield();
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (DateTime.Now.Hour == Global.DayStartHour && DateTime.Now.Minute == 0 && !IsClearYieldComp)
            {
                ClearYield();
                IsClearYieldComp = true;
            }
            else if (DateTime.Now.Hour == Global.DayEndHour && DateTime.Now.Minute == 0 && !IsClearYieldComp)
            {
                ClearYield();
                IsClearYieldComp = true;
            }

            if (DateTime.Now.Minute != 0 && IsClearYieldComp)
            {
                IsClearYieldComp = false;
            }
        }
        private void ClearYield()
        {
            if (this.InvokeRequired)
            {
                this?.Invoke(new Action(() => ClearYield()));
            }
            else
            {
                Total =OkCount=NgCount= 0;
                lblOkCount.Text = OkCount.ToString();
                lblNgCount.Text = NgCount.ToString();
                lblTotal.Text = Total.ToString();
            }
        }
    }
}
