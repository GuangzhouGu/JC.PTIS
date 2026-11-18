using JC.PTIS.Main.Entities;
using JC.PTIS.Main.Models;
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

namespace JC.PTIS.Main.UserCtrls
{
    public partial class LotBoxCtrl : UserControl
    {
        public LotBoxCtrl()
        {
            InitializeComponent();
            tlpLotBox.DoubleBuffered();
            //this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.AllPaintingInWmPaint, true);
        }
        [Description("标题栏背景色")]
        public Color TitleBackColor { get { return htTitlePanel1.TitleColor; } set { htTitlePanel1.TitleColor = value; } }
        public LotBoxStnType LotBoxStnType { get; set; }
        LotBoxType lotBoxType = LotBoxType.EmptyTray;
        [Description("料盒类型")]
        public LotBoxType LotBoxType
        {
            get
            {
                return lotBoxType;
            }
            set
            {
                lotBoxType = value;
                this.htTitlePanel1.Text = lotBoxType.GetDescription();
                if (lotBoxType== LotBoxType.Null)
                {
                    htTitlePanel1.Visible = false;
                }
            }
        }
        LotCellCtrl[,] LotCellCtrls = null;
        public void UpdateLotBoxInfo(LotBoxModel lotBoxModel)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<LotBoxModel>(UpdateLotBoxInfo), lotBoxModel);
            }
            else
            {
                try
                {
                    tlpLotBox.SuspendLayout();
                    if (lotBoxModel != null)
                    {
                        var rowCount = lotBoxModel.RowCount;
                        var colCount = lotBoxModel.ColCount;
                        tlpLotBox.Controls.Clear();
                        tlpLotBox.RowStyles.Clear();
                        tlpLotBox.ColumnStyles.Clear();
                        if (lotBoxModel.LotCellArray != null && lotBoxModel.LotCellArray.Length > 0)
                        {
                            if (LotBoxType != LotBoxType.Null)
                            {
                                tlpLotBox.RowCount = rowCount;
                                tlpLotBox.ColumnCount = colCount;
                                for (int i = 0; i < rowCount; i++)
                                {
                                    tlpLotBox.RowStyles.Add(new RowStyle(SizeType.Percent, 100f / rowCount));
                                }
                                for (int i = 0; i < colCount; i++)
                                {
                                    tlpLotBox.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100f / colCount));
                                }
                                LotCellCtrls = new LotCellCtrl[rowCount, colCount];
                                for (int i = 0; i < rowCount; i++)
                                {
                                    for (int j = 0; j < colCount; j++)
                                    {
                                        LotCellCtrl lotCellCtrl = new LotCellCtrl();
                                        lotCellCtrl.Dock = DockStyle.Fill;
                                        lotCellCtrl.LotCount = 0;
                                        tlpLotBox.Controls.Add(lotCellCtrl, j, i);
                                        LotCellCtrls[i, j] = lotCellCtrl;
                                        if (lotBoxModel.LotCellArray != null && lotBoxModel.LotCellArray.Length > 0)
                                        {
                                            lotCellCtrl.Init(lotBoxModel.LotCellArray[i, j], LotBoxType);
                                        }
                                    }
                                }
                            }
                        }
                         
                    }
                    else
                    {
                        tlpLotBox.Controls.Clear();
                        tlpLotBox.RowStyles.Clear();
                        tlpLotBox.ColumnStyles.Clear();
                    }
                    tlpLotBox.ResumeLayout();
                }
                catch (Exception ex)
                {
                    LogManager.GetLogger("Form").Error($"LotBoxCtrl->UpdateLotBoxInfo方法执行异常:{ex}");
                }
            }
        }

        private void LotBoxCtrl_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode()==false)
            {
                ProcInfoHelper.Instance.LotBoxInfoUpdated += OnLotBoxInfoUpdated;
                ProcInfoHelper.Instance.LotCellInfoUpdated +=OnLotCellInfoUpdated;
            }
        }

        private void OnLotCellInfoUpdated(LotCellBase arg1, LotBoxType arg2)
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<LotCellBase, LotBoxType>(OnLotCellInfoUpdated), arg1, arg2);
            }
            else
            {
                if (arg2 == LotBoxType)
                {
                    LotCellCtrls[arg1.RowNo, arg1.ColNo].UpdateLotCellInfo(arg1);
                }
            }
        }

        private void OnLotBoxInfoUpdated(LotBoxModel model,LotBoxStnType lotBoxStnType )
        {
            if (InvokeRequired)
            {
                this.Invoke(new Action<LotBoxModel, LotBoxStnType>(OnLotBoxInfoUpdated), model, lotBoxStnType);
            }
            else
            {
                if (LotBoxStnType == lotBoxStnType)
                {
                    UpdateLotBoxInfo(model);
                }
            }
        }
    }
}
