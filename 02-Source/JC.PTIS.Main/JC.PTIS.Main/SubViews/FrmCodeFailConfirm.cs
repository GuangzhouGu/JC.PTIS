using JC.PTIS.Main.Devices.Scaner;
using StdLib.WindowsControls;
using StdLib.WindowsForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.AET.SubForms
{
    public partial class FrmCodeFailConfirm : FormPopBase
    {
        public FrmCodeFailConfirm()
        {
            InitializeComponent();
            this.Title = "条码读取失败确认";
        }
        public string Msg { get; set; }
        public string Result = string.Empty;
        public int StationNo { get; set; }
        private void btnRetry_Click(object sender, EventArgs e)
        {
            var rfid = ScanerHelper.Instance.Scan(StationNo);
            if (string.IsNullOrEmpty(rfid) == false)
            {
                Result=rfid;
                this.Close();
            }
            else
            {
                htLabel1.Text = "读取失败";
            }
        }

        private void FrmAlignFailConfirm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (string.IsNullOrEmpty(Result))
            {
                e.Cancel = true;
            }
      
        }

        private void btnKeyin_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbxKeyinID.Text)==false)
            {
                Result = tbxKeyinID.Text;
                this.Close();
            }
            else
            {
                UIMessageDialog.ShowErrorDialog(null,"请手动输入条码");
            }
        }

        private void FrmCodeFailConfirm_Load(object sender, EventArgs e)
        {
            htLabel1.Text = $"{Msg}条码读取失败，请确认";
        }
    }
}
