using HYC.SaturnMain.SubForms;
using JC.AET.SubForms;
using JC.PTIS.Main.DB;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.RotateInfo;
using JC.PTIS.Main.Service;
using JC.PTIS.Main.Stations;
using StdLib.HTFile;
using StdLib.PCBaseFrame.Args;
using StdLib.PCBaseFrame.Entities;
using StdLib.PCBaseFrame.Extension.Entities;
using StdLib.PCBaseFrame.Extension.OpRecord;
using StdLib.PCBaseFrame.Extension.Views;
using StdLib.PCBaseFrame.Station;
using StdLib.PCBaseFrame.Views;
using StdLib.WindowsControls;
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

namespace JC.PTIS.Main
{
    public partial class FrmMain : ViewMain
    {
        public FrmMain()
        {
            InitializeComponent();
            this.Title = "PTIS主控";
            OnRegisterEvent += RegisterEvent;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();

            cmxRunMode.Items.Clear();
            foreach (var item in Enum.GetValues(typeof(RunMode)))
            {
                cmxRunMode.Items.Add(item.ToString());
            }
        }
        FrmPM FormPM = new FrmPM();
        string Barcode = "";
        private void RegisterEvent()
        {
            OnUpdateRecipe += FrmMain_OnUpdateRecipe;
            InitManager.OnInitPara += InitPara;
            InitManager.OnInitCom += InitCom;
            StationManager.Instance.OnIsCanHome += Instance_OnIsCanHome;
            StationManager.Instance.OnIsCanRun += Instance_OnIsCanRun;
            OnSwitchLang += FrmMain_OnSwitchLang;
            OnClosing += FrmMain_OnClosing;
            InterlockMonitor.ShowMsgEvent += InterlockMonitor_OnShowMsg;
            InterlockMonitor.StartMonitor();
            StationManager.Instance.OnUpdateEQPSts += Instance_OnUpdateEQPSts;
            OnSwitchView += FrmMain_OnSwitchView;
            FormLogin.OnLogIn += FormLogin_OnLogIn;
            WaferBufferStn.OnScancodeFailConfirm += OnScancodeFailConfirm;
        }

        private void FrmMain_OnSwitchView(ViewInfo obj)
        {
            RegisterService.Instance.IsManualMode = false;
            if (obj.View.ButtonName == "功能调试")
            {
                RegisterService.Instance.IsManualMode = true;
            }
        }

        private void InitCom()
        {
            RotateInfoHelper.Instance.Init();
            ProcInfoHelper.Instance.Init();
            Global.Init();
            cmxRunMode.SelectedIndex = 1;
            //htComboBox1.SelectedIndex = 1;
        }

        private void FormLogin_OnLogIn(User user, bool arg2)
        {
        }

        private void Instance_OnUpdateEQPSts(EqpSts sts)
        {
            switch (sts)
            {
                case EqpSts.IDLE:
                    break;
                case EqpSts.PAUSE:
                    break;
                case EqpSts.RUN:
                    break;
                case EqpSts.PM:
                    break;
                case EqpSts.ALARM:
                    break;
                case EqpSts.STOP:
                case EqpSts.EMG:
                    break;
                case EqpSts.HomeOK:
                    {
                        if (RedisHelper.Instance.IsConnected)
                        {
                            ProcInfoHelper.Instance.RestoreInfo();
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        private void InterlockMonitor_OnShowMsg(string obj)
        {
        }

        private void FrmMain_OnClosing()
        {
        }

        private void FrmMain_OnSwitchLang(LangType type)
        {
        }
        private string OnScancodeFailConfirm()
        {
            Barcode = "";
            if (InvokeRequired)
            {
                this.Invoke(new Func<string>(OnScancodeFailConfirm));
            }
            else
            {
                FrmCodeFailConfirm frm = new FrmCodeFailConfirm();
                frm.Msg = "扫码失败，请手动输入条码";
                frm.ShowDialog();
                Barcode = frm.Result;
            }
            return Barcode;
        }
        private bool Instance_OnIsCanRun()
        {
            RegisterService.Instance.IsManualMode = false;
            ShowMsg("FrmMain", $"当前运行模式：{Global.RunMode}", Brushes.LimeGreen);
            if (Global.RunMode == RunMode.维修)
            {
                UIMessageDialog.ShowWarningDialog(this, "当前运行模式为维修模式，禁止运行");
                return false;
            }
            else if (Global.RunMode == RunMode.强制清料)
            {
                // Global.CleanCounter = 0;
            }
            return true;
        }

        private bool Instance_OnIsCanHome()
        {
            return true;
        }

        private bool InitPara()
        {
            var ret = RedisHelper.Instance.Init("127.0.0.1:6379,password=123456,defaultDatabase=0");
            HTIniHelper ini = new HTIniHelper(AppDomain.CurrentDomain.BaseDirectory + "Config.INI");
            string connectStr = ini.Read("DB", "ConnectionString", "");
            var dbResult = DBHelper.Init(SqlSugar.DbType.SqlServer, connectStr);
            StageManager.Init();
            return ret && dbResult.Success;
        }

        private void FrmMain_OnUpdateRecipe(string obj)
        {
        }

        private void cmxRunMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (InitManager.IsInitCom)
            {
                if (StationManager.Instance.GetCurrentEQPSts() == EqpSts.STOP)
                {
                    string runMode = cmxRunMode.SelectedItem.ToString();
                    if (runMode == RunMode.维修.ToString())
                    {
                        if (FormPM == null || FormPM.IsDisposed)
                        {
                            FormPM = new FrmPM();
                        }
                        FormPM.Show();
                    }
                    Global.RunMode = EnumHelper.GetEnumValue<RunMode>(runMode);
                    OpRecordHelper.Record(new EqInfo { ViewName = "主界面", OperationType = OperationType.EqOperation, OpContent = $"运行模式：{Global.RunMode}" });
                    ShowMsg("FrmMain", $"当前设备运行模式：{runMode}", Brushes.Blue);
                }
                else
                {
                    ShowMsg("FrmMain", $"当前设备状态不是Stop，无法切换运行模式", Brushes.Red);
                }
            }
        }

        private void htComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Global.SelectedBin = EnumHelper.GetEnumValue<BinLevel>(htComboBox1.SelectedItem.ToString());
        }
        private void btnChangeIRNgTrayDone_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "确认更换IRNgTray完成？"))
            {
                ThrowNgDieStn.ChangeIRNgTrayDone = true;
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            ThreadPool.SetMinThreads(1000, 1000);
        }
    }
}
