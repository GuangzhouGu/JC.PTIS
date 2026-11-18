using JC.PTIS.Main.Entities;
using JC.PTIS.Main.RotateInfo;
using StdLib.PCBaseFrame.Extension.Entities;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.PTIS.Main.UserCtrls
{
    public partial class LotBoxMonitorCtrl : UserControl
    {
        public LotBoxMonitorCtrl()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
            this.UpdateStyles();
            for (int i = 0; i < LotBoxCtrls.Length; i++)
            {
                var name = $"lotBoxCtrl{i + 1}";
                var ctrl = this.Controls.Find(name, true).FirstOrDefault() as LotBoxCtrl;
                LotBoxCtrls[i] = ctrl;
            }
        }
        LotBoxCtrl[] LotBoxCtrls = new LotBoxCtrl[4];
        private void LotBoxMonitorCtrl_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode() == false)
            {
                RotateInfoHelper.Instance.LotBoxInfoChanged += OnRotateInfoChanged;
            }
        }

        private void OnRotateInfoChanged(ConcurrentDictionary<int, LotBoxType> stnNoList)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action(() => OnRotateInfoChanged(stnNoList)));
            }
            else
            {
                foreach (var stnNo in stnNoList)
                {
                    var ctrl = GetLotBoxCtrl(stnNo.Value);
                    if (ctrl != null)
                    {
                        switch (stnNo.Key)
                        {
                            case 1:
                                tableLayoutPanel1.SetRow(ctrl, 0);
                                tableLayoutPanel1.SetColumn(ctrl, 1);
                                break;
                            case 2:
                                tableLayoutPanel1.SetRow(ctrl, 1);
                                tableLayoutPanel1.SetColumn(ctrl, 0);
                                break;
                            case 3:
                                tableLayoutPanel1.SetRow(ctrl, 1);
                                tableLayoutPanel1.SetColumn(ctrl, 1);
                                break;
                            default:
                                break;
                        }
                    }

                }

            }
        }
        private LotBoxCtrl GetLotBoxCtrl(LotBoxType type)
        {
            var index= Array.FindIndex(LotBoxCtrls, x => { if (x != null) { return x.LotBoxType == type; } return false; } );
            if (index>=0)
            {
                return LotBoxCtrls[index];
            }
            return null;
        }
    }
}
