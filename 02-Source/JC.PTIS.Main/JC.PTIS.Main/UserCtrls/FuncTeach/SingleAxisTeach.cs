using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Properties;
using StdLib.PCBaseFrame.Args;
using StdLib.PCBaseFrame.Extension.Entities;
using StdLib.PCBaseFrame.Motion;
using StdLib.PCBaseFrame.Station;
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
using static System.Collections.Specialized.BitVector32;

namespace JC.PTIS.Main.UserCtrls.FuncTeach
{
    public partial class SingleAxisTeach : UserControl
    {
        public SingleAxisTeach()
        {
            InitializeComponent();
            InitCtrl();
        }
        StationName[] StationList = null;
        StationName StationName = StationName.None;
        MotionManager Motion = MotionManager.GetInstance();
      
        Timer Timer = new Timer();
        bool IsJog = true;
        List<PictureBox> PbxList = new List<PictureBox>();
        private void InitCtrl()
        {
            try
            {
                StationList = Enum.GetValues(typeof(StationName)).Cast<StationName>().ToArray();
                var descList = EnumHelper.EnumDescToList<StationName>();
                cmxStationList.Items.Clear();
                cmxStationList.Items.AddRange(descList.Select(p=>p.Name).ToArray());
                if (cmxStationList.Items.Count > 0)
                {
                    cmxStationList.SelectedIndex = 0;
                }
                PbxList = new List<PictureBox>();
                if (tableLayoutPanel1.Controls.Count > 0)
                {
                    foreach (var item in tableLayoutPanel1.Controls)
                    {
                        if (item.GetType().Name == "PictureBox")
                        {
                            PbxList.Add((PictureBox)item);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        public string GetAxisName( )
        {
            return cmxAxisList.SelectedItem.ToString();
        }
        private void ShowPosName(string axisName)
        {
            var axisInfo = AxisInfoHelper.GetAxisInfoByName(axisName);
            if (axisInfo!=null)
            {
                cmxPosList.Items.Clear();
                cmxPosList.Items.AddRange(axisInfo.PosList.Select(p => p.PosName).ToArray());
                if (cmxPosList.Items.Count>0)
                {
                    cmxPosList.SelectedIndex = 0;
                }
            }
        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            try
            {
                var axisName = GetAxisName();
                if (string.IsNullOrEmpty(axisName) == false)
                {
                    lblAxisXPos.Text = Motion.GetAxisActPos(axisName).ToString("F3");
                    var axisInfo = AxisInfoHelper.GetAxisInfoByName(axisName    );
                    if (axisInfo != null)
                    {
                        ShowAxisSts("IsPosLimit", axisInfo.IsPosLimit);
                        ShowAxisSts("IsNegLimit", axisInfo.IsNegLimit);
                        ShowAxisSts("IsHomeOK", axisInfo.IsHomeOK);
                        ShowAxisSts("IsServoOn", axisInfo.IsServoOn);
                        ShowAxisSts("IsArrive", axisInfo.IsArrive);
                        ShowAxisSts("IsMove", axisInfo.IsMove);
                        ShowAxisSts("IsAlarm", axisInfo.IsAlarm);
                    }
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void ShowAxisSts(string name, bool value)
        {
            var index = PbxList.FindIndex(p => p.Name == $"pbx{name}");
            if (index >= 0)
            {
                PbxList[index].Image = value ? Resources.Green2 : Resources.Gray2;
            }
        }

        private void SingleAxisTeach_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode()==false)
            {
                cmxMoveRel.SelectedIndex = 0;
                Timer.Interval = 200;
                Timer.Tick += Timer_Tick;
                Timer.Start();
            }
        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            var ret = Motion.IsCanMove(new MoveArgs { AxisName = GetAxisName() });
            if (ret.Result)
            {
                if (IsJog == false)
                {
                    var pos = double.Parse(cmxMoveRel.SelectedItem.ToString());
                    Motion.MoveRel(GetAxisName(), pos * -1);
                }
            }
            else
            {
                UIMessageDialog.ShowErrorDialog(null, $"{ret.Msg}");
            }
         
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            var ret = Motion.IsCanMove(new MoveArgs { AxisName = GetAxisName() });
            if (ret.Result)
            {
                if (IsJog == false)
                {
                    var pos = double.Parse(cmxMoveRel.SelectedItem.ToString());
                    Motion.MoveRel(GetAxisName(), pos);
                }
            }
            else
            {
                UIMessageDialog.ShowErrorDialog(null, $"{ret.Msg}");
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            Motion.Stop(GetAxisName());
        }

        private void rbtnJog_ValueChanged(object sender, bool value)
        {
            IsJog = value;
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (cmxPosList.SelectedItem == null)
            {
                UIMessageDialog.ShowErrorDialog(null, "请选择要保存的点位");
                return;
            }
            var posStr = cmxPosList.SelectedItem.ToString();
            string posName = GetPosName(GetAxisName(), posStr);
            var ret = AxisInfoHelper.SavePos(GetAxisName(), posName, double.Parse(lblAxisXPos.Text));
            if (ret)
            {
                tbxPos.Text = lblAxisXPos.Text;
                UIMessageDialog.ShowSuccessDialog(null, $"保存点位完成");
            }
            else
            {
                UIMessageDialog.ShowErrorDialog(null, $"保存点位失败");
            }
        }
        private string GetPosName(string axisName, string chnPosName)
        {
            string posName = string.Empty;
            var posList = AxisInfoHelper.GetAxisInfoByName(axisName).PosList;
            if (posList != null)
            {
                var index = posList.FindIndex(p => p.PosName == chnPosName);
                if (index >= 0)
                {
                    posName = posList[index].PosName;
                }
            }
            return posName;
        }

        private void cmxPosList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var posStr = cmxPosList.SelectedItem.ToString();
            var podInfo = AxisInfoHelper.GetPosInfoByName(GetAxisName(), posStr);
            if (podInfo != null)
            {
                tbxPos.Text = podInfo.Pos.ToString();
                tbxDesc.Text = podInfo.PosNameChn;
            }
        }

        private void btnRight_MouseDown(object sender, MouseEventArgs e)
        {
            var ret = Motion.IsCanMove(new MoveArgs { AxisName = GetAxisName() });
            if (ret.Result)
            {
                if (IsJog)
                {
                    Motion.Jog(GetAxisName(), 1);
                }
            }
            else
            {
                UIMessageDialog.ShowErrorDialog(null, $"{ret.Msg}");
            }
         
        }

        private void btnLeft_MouseDown(object sender, MouseEventArgs e)
        {
            var ret = Motion.IsCanMove(new MoveArgs { AxisName = GetAxisName() });
            if (ret.Result)
            {
                if (IsJog)
                {
                    Motion.Jog(GetAxisName(), -1);
                }
            }
            else
            {
                UIMessageDialog.ShowErrorDialog(null, $"{ret.Msg}");
            }
        
        }

        private void btnLeft_MouseUp(object sender, MouseEventArgs e)
        {
            var ret = Motion.IsCanMove(new MoveArgs { AxisName = GetAxisName() });
            if (ret.Result)
            {
                if (IsJog)
                    Motion.Stop(GetAxisName());
            }
            else
            {
                UIMessageDialog.ShowErrorDialog(null, $"{ret.Msg}");
            }
          
        }

        private void cmxStationList_SelectedIndexChanged(object sender, EventArgs e)
        {
            StationName = StationList[cmxStationList.SelectedIndex];
            UpdateStationName(StationName);
        }
        public void UpdateStationName(StationName name)
        {
            StationName = name;
            var strName= EnumHelper.GetEnumDesc(name);
            if (!string.IsNullOrEmpty(strName))
            {
                cmxStationList.Text = strName;
            }
            var stationInfo = StationManager.Instance.GetStationInfo(name.ToString());
            if (stationInfo!=null)
            {
                cmxAxisList.Items.Clear();
                if (stationInfo != null)
                {
                    cmxAxisList.Items.AddRange(stationInfo.AxisList.Select(p => p.AxisName).ToArray());
                    if (cmxAxisList.Items.Count > 0)
                    {
                        cmxAxisList.SelectedIndex = 0;
                    }
                }
            }
        }

        private void cmxAxisList_SelectedIndexChanged(object sender, EventArgs e)
        {
            var axisName = cmxAxisList.SelectedItem.ToString();
            ShowPosName(axisName);
        }

        private async void btnHome_Click(object sender, EventArgs e)
        {
            var ret = await Motion.HomeSync(GetAxisName(),30);
            if (ret)
            {
                UIMessageDialog.ShowSuccessDialog(null, $"轴{GetAxisName()}回零完成");
            }
            else
            {
                UIMessageDialog.ShowErrorDialog(null, $"轴{GetAxisName()}回零失败");
            }
        }

        private async void btnMoveTo_Click(object sender, EventArgs e)
        {
            var posName = cmxPosList.SelectedItem.ToString();
            var ret = await Motion.MoveAbsSync(GetAxisName(), posName);
            if (ret)
            {
                UIMessageDialog.ShowSuccessDialog(null, $"轴{GetAxisName()}移动到{posName}完成");
            }
            else
            {
                UIMessageDialog.ShowErrorDialog(null, $"轴{GetAxisName()}移动到{posName}失败");
            }
        }
    }
}
