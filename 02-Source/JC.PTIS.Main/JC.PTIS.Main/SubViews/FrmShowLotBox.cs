using JC.PTIS.Main.Models;
using JC.PTIS.Main.UserCtrls;
using StdLib.PCBaseFrame.Extension.Views;
using StdLib.WindowsControls;
using StdLib.WindowsForms;
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

namespace JC.PTIS.Main.SubViews
{
    public partial class FrmShowLotBox : FormPopBase
    {
        public FrmShowLotBox()
        {
            InitializeComponent();
        }
        List<LotModel> LotList = null;
        public void ShowInfo(List<LotModel> lotList)
        {
            try
            {
                if (lotList!=null&&lotList.Count>0)
                {
                    LotList = lotList;
                    htComboBox1.Items.Clear();
                    foreach (var item in lotList)
                    {
                        if (item!=null)
                        {
                            htComboBox1.Items.Add(item.LotCode);
                        }
                    }
                    if (htComboBox1.Items.Count>0)
                    {
                        htComboBox1.SelectedIndex = 0;
                    }
                }
            }
            catch (Exception ex)
            {
                UIMessageDialog.ShowErrorDialog(null, ex.ToString());
            }
        }

        private void htComboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LotList.Count>0)
            {
                var lotModel = LotList[htComboBox1.SelectedIndex];
                ShowLotModel(lotModel);
            }
        }
        private void ShowLotModel(LotModel lotModel)
        {
            if (lotModel != null)
            {
                panel1.Controls.Clear();
                LotTrayCtrl lotTrayCtrl = new LotTrayCtrl();
                lotTrayCtrl.Dock = DockStyle.Fill;
                lotTrayCtrl.Parent = panel1;
                lotTrayCtrl.UpdateLotInfoEx(lotModel);
            }
        }
    }
}
