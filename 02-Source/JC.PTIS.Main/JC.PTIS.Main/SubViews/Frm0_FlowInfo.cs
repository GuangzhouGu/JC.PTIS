using JC.PTIS.Main.Entities;
using JC.PTIS.Main.FlowInfo;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.Models.ModelHelper;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Params;
using JC.PTIS.Main.Properties;
using JC.PTIS.Main.RotateInfo;
using JC.PTIS.Main.Service;
using JC.PTIS.Main.Stations;
using StdLib.PCBaseFrame.Extension.Entities;
using StdLib.PCBaseFrame.Extension.Views;
using StdLib.PCBaseFrame.Station;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.PTIS.Main.SubViews
{
    public partial class Frm0_FlowInfo : SubViewBase
    {
        public Frm0_FlowInfo()
        {
            InitializeComponent();
            this.ButtonName = "物流信息";
            this.ButtonNameEn = "FlowInfo";
            this.Index = 0;
            this.ButtonImage = Resources.FlowInfo;
            this.IsNeedLogin = false;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
        }

        public override void InitCom()
        {
            StationManager.Instance.OnSignalTrigger += OnSignalTrigger;
        }

        private void OnSignalTrigger(StdLib.PCBaseFrame.Motion.IOInfo arg1, bool arg2)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<StdLib.PCBaseFrame.Motion.IOInfo, bool>(OnSignalTrigger), arg1, arg2);
            }
            else
            {
                waferTableCtrl1.UpdateSensor(arg1);
            }
        }

       
        private void Frm0_FlowInfo_Load(object sender, EventArgs e)
        {
            //cmxBinList.SelectedIndex=htComboBox1.SelectedIndex = 0;
        }
 
      

        private void btnLoadMap_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var path = openFileDialog.FileName;
                var tempWafer = WaferMapHelper.Instance.Parse(path);
                if (tempWafer != null)
                {
                    tempWafer.Code = "T0891512";
                    tempWafer.FoupNo = 0;
                    tempWafer.LayerNo = 0;
                    tempWafer.MapFilePath = @"WaferMap\T08915-12-H1.txt";
                    ProcInfoHelper.Instance.PutWafer(WaferPosEnum.WaferTable, tempWafer);
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

       

        private void button2_Click(object sender, EventArgs e)
        {
            //var lotBoxModel = RedisHelper.Instance.ObjectGet<LotBoxModel>($"LotBoxStnType:{LotBoxStnType.OKBinStn}");
            //ProcInfoHelper.Instance.PutLotBox(LotBoxStnType.LulLotStn, lotBoxModel);
            ProcInfoHelper.Instance.RestoreInfo();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //var rowNo = ProcInfoHelper.Instance.GetIRNgTrayAvlRowNo();
            //if (rowNo != -1)
            //{
            //    var dieModel = new DieModel();
            //    dieModel.BinLevel = BinLevel.Bin1;
            //    ProcInfoHelper.Instance.PlaceDieToIRNgTray(rowNo, dieModel);
            //}
            //else
            //{
            //    ProcInfoHelper.Instance.ClearIRNgTray();
            //}
            var temp = 12.ToString("D4");
            MessageBox.Show(temp);
        }

        private void buttonEx1_Click(object sender, EventArgs e)
        {
            var tray=new TrayModel();
            tray.TrayCode = "T0891512";
            tray.LOT = "L0891512";
            tray.WAFER = "W0891512";
            tray.RowCount = 2;
            tray.ColCount = 30;
            tray.DieArray = new DieModel[tray.RowCount, tray.ColCount];
            ProcInfoHelper.Instance.PutTrayToTrayStage(TrayPosEnum.EmptyTrayBufferStage,tray);
        }
    }
}
