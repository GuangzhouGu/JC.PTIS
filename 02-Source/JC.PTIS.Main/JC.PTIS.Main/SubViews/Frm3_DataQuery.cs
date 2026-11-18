using JC.PTIS.Main.Properties;
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
    public partial class Frm3_DataQuery : SubViewBase
    {
        public Frm3_DataQuery()
        {
            InitializeComponent();
            this.ButtonName = "数据查询";
            this.ButtonNameEn = "DataQuery";
            this.Index = 3;
            this.ButtonImage = Resources.Query;
            this.IsNeedLogin = false;
        }
    }
}
