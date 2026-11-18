using HYC.HTImage;
using JC.PTIS.Main.AlarmInfo;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.Models.ModelHelper;
using JC.PTIS.Main.Properties;
using StdLib.PCBaseFrame.Alarm;
using StdLib.PCBaseFrame.Extension.Views;
using StdLib.PCBaseFrame.Motion;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.PTIS.Main.SubViews
{
    public partial class Frm2_DeviceCheck : SubViewBase
    {
        public Frm2_DeviceCheck()
        {
            InitializeComponent();
            this.ButtonName = "硬件点检";
            this.ButtonNameEn = "DeviceCheck";
            this.Index = 2;
            this.ButtonImage = Resources.DeviceCheck;
            this.IsNeedLogin = false;
        }

        private void btnClearControllerFaults_Click(object sender, EventArgs e)
        {
         
        }

        private void buttonEx1_Click(object sender, EventArgs e)
        {
            var rdm = new Random();
            LotModel lotModel = new LotModel();
            lotModel.EqpInTime = DateTime.Now.AddHours(-1);
            lotModel.LotCode = lotModel.EqpInTime.ToString("yyyyMMddHHmmss");
            lotModel.TrayList = new List<TrayModel>();
            lotModel.BinLevel = BinLevel.Bin1;
            for (int r = 0; r < 10; r++)
            {
                TrayModel trayModel = new TrayModel();
                trayModel.EqpInTime = DateTime.Now.AddHours(-1);
                trayModel.TrayCode = trayModel.EqpInTime.ToString("HHmmssfff");
                trayModel.BinLevel = 1;
                trayModel.RowCount = 25;
                trayModel.ColCount = 2;
                trayModel.DieArray = new DieModel[25, 2];
                trayModel.TrayType = TrayTypeEnum.Full;
                for (int i = 0; i < trayModel.RowCount; i++)
                {
                    for (int j = 0; j < trayModel.ColCount; j++)
                    {
                        var value = rdm.Next(0, 100);
                        var dieModel = new DieModel();
                        dieModel.DieCode = $"{trayModel.TrayCode}_Row{i}_Col{j}";
                        if (value > 0 && value < 30)
                        {
                            dieModel.BinLevel = BinLevel.Bin1;
                        }
                        else if (value > 30 && value < 50)
                        {
                            dieModel.BinLevel = BinLevel.Bin3;
                        }
                        else if (value > 50 && value < 70)
                        {
                            dieModel.BinLevel = BinLevel.Bin5;
                        }
                        else if (value > 70 && value < 80)
                        {
                            dieModel.BinLevel = BinLevel.Bin2;
                        }
                        else if (value > 80 && value < 90)
                        {
                            dieModel.BinLevel = BinLevel.Bin4;
                        }
                        else if (value > 90 && value < 100)
                        {
                            dieModel.BinLevel = BinLevel.Bin6;
                        }
                        dieModel.DieStatus = DieStatus.Picked;
                        dieModel.StartTime = DateTime.Now.AddHours(-1);
                        dieModel.EndTime = DateTime.Now.AddHours(-1).AddSeconds(30);
                        dieModel.SpanTime = dieModel.EndTime.Subtract(dieModel.StartTime).TotalSeconds;
                        if (i % 3 == 0)
                        {
                            dieModel.BackDetectResult = Result.NG;
                            dieModel.DieStatus = DieStatus.FrontNG;
                        }
                        else if (i % 5 == 0)
                        {
                            dieModel.FrontDetectResult = Result.NG;
                            dieModel.DieStatus = DieStatus.BackNG;
                        }
                        else
                        {
                            dieModel.DieStatus = DieStatus.OK;
                        }
                        dieModel.TotalResult = dieModel.DieStatus == DieStatus.OK ? Result.OK : Result.NG;
                        trayModel.DieArray[i, j] = dieModel;
                        DieModelHelper.Insert(dieModel);
                        Thread.Sleep(5);
                    }
                }
                trayModel.UpdateMap();
                TrayModelHelper.Insert(trayModel);
                lotModel.TrayList.Add(trayModel);
            }
            lotModel.UpdateMap();
            LotModelHelper.Insert(lotModel);
        }
    }
}
