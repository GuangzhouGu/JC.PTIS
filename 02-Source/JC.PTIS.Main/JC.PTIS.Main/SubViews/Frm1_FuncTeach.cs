using JC.PTIS.Main.Properties;
using JC.PTIS.Main.UserCtrls.FuncTeach;
using StdLib.PCBaseFrame.Extension.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.PTIS.Main.SubViews
{
    public partial class Frm1_FuncTeach : SubViewBase
    {
        public Frm1_FuncTeach()
        {
            InitializeComponent();
            this.ButtonName = "功能调试";
            this.ButtonNameEn = "FuncTeach";
            this.Index = 1;
            this.ButtonImage = Resources.Func;
            this.IsNeedLogin = false;
        }

        private void htTabControlMenu1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var page = htTabControlMenu1.TabPages[htTabControlMenu1.SelectedIndex];
            if (page.Controls.Count>0)
            {
                foreach (var item in page.Controls)
                {
                    var baseType = item.GetType().BaseType.Name;
                    if (baseType == nameof(TeachCtrlBase))
                    {
                        var stationName = ((TeachCtrlBase)item).StationName;
                        if (stationName!=MotionInfo.StationName.None)
                        {
                            singleAxisTeach1.UpdateStationName(stationName);
                            axisPosMonitorCtrl1.UpdateStationName(stationName);
                            break;
                        }
                    }
                }
            }
        }
    }
}
