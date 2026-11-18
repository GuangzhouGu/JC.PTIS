using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.PTIS.Main.UserCtrls.FuncTeach
{
    public partial class TurnTableTeachCtrl : UserControl
    {
        public TurnTableTeachCtrl()
        {
            InitializeComponent();
        }
        [Description("转塔类型")]
        public RotateStnType RotateStnType { get; set; }

    }
}
