using JC.PTIS.Main.Params;
using StdLib.WindowsControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.PTIS.Main.UserCtrls.SizeEdit
{
    public partial class BufferSizeEditCtrl : SizeEditBaseCtrl
    {
        public BufferSizeEditCtrl()
        {
            InitializeComponent();
            for (int i = 0; i < 12; i++)
            {
                var name= $"tbxLayer{i+1}Offset";
                var ctrs = Controls.Find(name,true);
                if (ctrs.Length>0)
                {
                    TbxArr[i] = ctrs[0] as HTTextBox;
                }
            }
        }
        HTTextBox[] TbxArr = new HTTextBox[12]; 
        public override void LoadPara()
        {
            try
            {
                tbxGap.Text = ParaHelper.Prod.TrayBufferPitch.ToString();
                for (int i = 0; i < 12; i++)
                {
                    TbxArr[i].Text = ParaHelper.Prod.OffsetArr[i].ToString();
                }
            }
            catch (Exception ex)
            {
                UIMessageDialog.ShowErrorDialog(null, $"Boat参数加载异常，异常信息：{ex}");
            }
        }
        public override bool SavePara()
        {
            try
            {
                ParaHelper.Prod.TrayBufferPitch = double.Parse(tbxGap.Text);
                for (int i = 0; i < 12; i++)
                {
                    ParaHelper.Prod.OffsetArr[i] = double.Parse(TbxArr[i].Text);
                }
                return true;
            }
            catch (Exception ex)
            {
                UIMessageDialog.ShowErrorDialog(null, $"Boat参数保存异常，异常信息：{ex}");
                return false;
            }
        }
    }
}
