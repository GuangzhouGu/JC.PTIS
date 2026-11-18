using APS168_W32;
using JC.B17AutoMating.SubForms;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Params;
using JC.PTIS.Main.RotateInfo;
using StdLib.PCBaseFrame.Config;
using StdLib.PCBaseFrame.Extension.Recipe;
using StdLib.PCBaseFrame.Motion;
using StdLib.PCBaseFrame.MotionInfo;
using StdLib.WindowsControls;
using StdLib.WindowsControls.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.PTIS.Main.UserCtrls.FuncTeach
{
    public partial class LoadDieDDTeachCtrl : TeachCtrlBase
    {
        public LoadDieDDTeachCtrl()
        {
            InitializeComponent();
            InitCtrl();
            StationName =MotionInfo.StationName.LoadDieDDStn;
        }
        List<AxisName> Axes = null;
        System.Timers.Timer Timer = new System.Timers.Timer();
        int DetectType = 0;
        const double RotateInterval= 22.5;
        private void InitCtrl()
        {
            try
            {
                cmxNozzleList.Items.Clear();
                for (int i = 0; i < 16; i++)
                {
                    cmxNozzleList.Items.Add($"吸嘴{i}");
                }
                cmxNozzleList.SelectedIndex = 0;
                cmxDetectType.SelectedIndex = 0;
                Timer.Elapsed += Timer_Tick;
            }
            catch (Exception ex)
            {
            }
        }

        private async void btnRotate_Click(object sender, EventArgs e)
        {
            var btn = (HTButton)sender;
            var destStnNo = int.Parse(btn.Tag.ToString());
            if (UIMessageDialog.ShowAskDialog(null, $"是否旋转上料DD马达到{(Stn16TurnTableStnEnum)destStnNo}位置") ==false)
            {
                return;
            }
            Axes= new List<AxisName>() { AxisName.LoadDie_DD };
            if (IsCanMove(Axes)==false)
            {
                return;
            }
            var nozzleNo= cmxNozzleList.SelectedIndex;
            var curStnNo = RotateInfoHelper.Instance.GetStnNo(nozzleNo, RotateStnType.LoadDie);
            var destAngle = RotateInfoHelper.Instance.GetActMoveAngle(destStnNo, nozzleNo, RotateInterval, RotateStnType.LoadDie);
            //var diff = destStnNo - curStnNo;
            //var destAngle = diff * RotateInterval;
            //if (destAngle<0)
            //{
            //    destAngle += 360;
            //}
            //else if (destAngle > 360)
            //{
            //    destAngle -= 360;
            //}
            var ret=await Motion.MoveAbsSync(AxisName.LoadDie_DD, destAngle);
            if (ret)
            {
                var actPos = Motion.GetAxisActPos(AxisName.LoadDie_DD);
                RotateInfoHelper.Instance.UpdateRotateInfo(RotateStnType.LoadDie, Math.Round(actPos,1));
                FrmTips.ShowTipsSuccess(null, $"DD马达到{(Stn16TurnTableStnEnum)destStnNo}位置完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, $"DD马达到{(Stn16TurnTableStnEnum)destStnNo}位置失败");
            }
        }

        private async void btnHome_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, $"是否回零") == false)
            {
                return;
            }
            Axes = new List<AxisName>() { AxisName.LoadDie_DD };
            if (IsCanMove(Axes,true) == false)
            {
                return;
            }
           var ret=  await Motion.HomeSync(AxisName.LoadDie_DD,60);
            if (ret)
            {
                RotateInfoHelper.Instance.UpdateRotateInfo(RotateStnType.LoadDie, 0);
                FrmTips.ShowTipsSuccess(null, $"上料DD马达回零完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, "上料DD马达回零失败");
            }
        }

        private void cmxNozzleList_SelectedIndexChanged(object sender, EventArgs e)
        {
            vacuumCtrl1.VacuumName=$"LoadDieNozzle{cmxNozzleList.SelectedIndex + 1}Vac";
            vacuumCtrl1.VacuumName = $"吸嘴{cmxNozzleList.SelectedIndex + 1}真空";
            tbxPickDieHeightOffset.Text = ParaHelper.Offset.LoadDieStnPickDieZOffset[cmxNozzleList.SelectedIndex].ToString();
        }

        private async void btnAxisWaitPos_Click(object sender, EventArgs e)
        {
            var ret=await Motion.MoveAbsSync(AxisName.PickDie_Z,PosName.WaitPos);
            if (ret)
            {
                FrmTips.ShowTipsSuccess(null, $"PickDie_Z轴移动到等待位置完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, "PickDie_Z轴移动到等待位置失败");
            }
        }

        private async void btnAxisZPickDie_Click(object sender, EventArgs e)
        {
            var nozzleNo = cmxNozzleList.SelectedIndex;
            var pos=GetPickPos(nozzleNo);
            var ret = await Motion.MoveAbsSync(AxisName.PickDie_Z, pos);
            if (ret)
            {
                FrmTips.ShowTipsSuccess(null, $"PickDie_Z轴移动到取Die位置完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, "PickDie_Z轴移动到取Die位置失败");
            }
        }
        private double GetPickPos(int pickNo)
        {
            double ret = 0;
            var posInfo = AxisInfoHelper.GetPosInfoByName(AxisName.PickDie_Z, PosName.PickDiePos.ToString());
            ret = posInfo.Pos + ParaHelper.Offset.LoadDieStnPickDieZOffset[pickNo];
            return ret;
        }

        private void btnReadOffset_Click(object sender, EventArgs e)
        {
            var pickNo = cmxNozzleList.SelectedIndex;
            tbxPickDieHeightOffset.Text = ParaHelper.Offset.LoadDieStnPickDieZOffset[pickNo].ToString();
        }

        private void btnSaveOffset_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbxPickDieHeightOffset.Text)==false)
            {
                var pickNo = cmxNozzleList.SelectedIndex;
                var ret= double.TryParse(tbxPickDieHeightOffset.Text,out var value);
                if (ret)
                {
                    ParaHelper.Offset.LoadDieStnPickDieZOffset[pickNo] = value;
                   ret= RecipeManager.SavePara(nameof(ParaHelper.Offset));
                    if (ret)
                    {
                        FrmTips.ShowTipsSuccess(null, $"保存偏移值成功");
                    }
                    else
                    {
                        FrmTips.ShowTipsWarning(null, "保存偏移值失败");
                    }
                }
            }
        }

        private void btnPlaceDieOffetRead_Click(object sender, EventArgs e)
        {
            var pickNo = cmxNozzleList.SelectedIndex;
            tbxPlaceDieHeightOffset.Text = ParaHelper.Offset.LoadDieStnPlaceDieZOffset[pickNo].ToString();
        }

        private void btnPlaceDieOffetSave_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(tbxPlaceDieHeightOffset.Text) == false)
            {
                var pickNo = cmxNozzleList.SelectedIndex;
                var ret = double.TryParse(tbxPlaceDieHeightOffset.Text, out var value);
                if (ret)
                {
                    ParaHelper.Offset.LoadDieStnPlaceDieZOffset[pickNo] = value;
                    ret = RecipeManager.SavePara(nameof(ParaHelper.Offset));
                    if (ret)
                    {
                        FrmTips.ShowTipsSuccess(null, $"保存偏移值成功");
                    }
                    else
                    {
                        FrmTips.ShowTipsWarning(null, "保存偏移值失败");
                    }
                }
            }
        }

        private async void btnPlaceZWaitPos_Click(object sender, EventArgs e)
        {
            var ret = await Motion.MoveAbsSync(AxisName.PlaceDie_Z, PosName.WaitPos);
            if (ret)
            {
                FrmTips.ShowTipsSuccess(null, $"PlaceDie_Z轴移动到等待位置完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, "PlaceDie_Z轴移动到等待位置失败");
            }
        }

        private async void btnPlaceZPlacePos_Click(object sender, EventArgs e)
        {
            var nozzleNo = cmxNozzleList.SelectedIndex;
            var pos = GetPickPos(nozzleNo);
            var ret = await Motion.MoveAbsSync(AxisName.PlaceDie_Z, pos);
            if (ret)
            {
                FrmTips.ShowTipsSuccess(null, $"PlaceDie_Z轴移动到放置Die位置完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, "PlaceDie_Z轴移动到放置Die位置失败");
            }
        }
        private void btnBackTriggerPosSave_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(tbxBackStartPos.Text)&& !string.IsNullOrEmpty(tbxBackEndPos.Text))
            {
                var startPos = double.Parse(tbxBackStartPos.Text);
                var endPos = double.Parse(tbxBackEndPos.Text);
                StdLib.PCBaseFrame.MotionInfo.CompareInfo compareInfo = CompareInfoHelper.GetCompareInfoByName(CompareName.BackDetectGrab);
                AxisName axisName = AxisName.BackDetect_X;
                if (cmxDetectType.SelectedIndex==1)
                {
                    axisName = AxisName.IRDetect_X;
                    compareInfo = CompareInfoHelper.GetCompareInfoByName(CompareName.IRDetectGrab);
                }
                AxisInfoHelper.GetPosInfoByName(axisName, PosName.GrabStartPos).Pos = startPos; ;
                AxisInfoHelper.GetPosInfoByName(axisName, PosName.GrabEndPos).Pos = endPos; ;
                compareInfo.Para.PulseTime = short.Parse(tbxPulseWidth.Text);
                compareInfo.Para.MaxErr = double.Parse(tbxMaxError.Text);
                var ret=MotionConfig.Instance.SaveStationInfo();
                if (ret)
                {
                    FrmTips.ShowTipsSuccess(null, $"保存飞拍参数成功");
                }
                else
                {
                    FrmTips.ShowTipsWarning(null, "保存飞拍参数失败");
                }
            }
        }

        private void cmxDetectType_SelectedIndexChanged(object sender, EventArgs e)
        {
            DetectType= cmxDetectType.SelectedIndex;
            PosInfo posInfo = null;
            PosInfo posInfo2 = null;
            StdLib.PCBaseFrame.MotionInfo.CompareInfo compareInfo = null;
            if (cmxDetectType.SelectedIndex == 0)
            {
                 posInfo = AxisInfoHelper.GetPosInfoByName(AxisName.BackDetect_X, PosName.GrabStartPos);
                 posInfo2 = AxisInfoHelper.GetPosInfoByName(AxisName.BackDetect_X, PosName.GrabEndPos);
                 compareInfo = CompareInfoHelper.GetCompareInfoByName(CompareName.BackDetectGrab);
            }
            else
            {
                 posInfo = AxisInfoHelper.GetPosInfoByName(AxisName.IRDetect_X, PosName.GrabStartPos);
                 posInfo2 = AxisInfoHelper.GetPosInfoByName(AxisName.IRDetect_X, PosName.GrabEndPos);
                 compareInfo = CompareInfoHelper.GetCompareInfoByName(CompareName.IRDetectGrab);
            }
            tbxBackStartPos.Text = posInfo.Pos.ToString();
            tbxBackEndPos.Text = posInfo2.Pos.ToString();
            tbxPulseWidth.Text=compareInfo.Para.PulseTime.ToString();
            tbxMaxError.Text=compareInfo.Para.MaxErr.ToString();
        }

        private async void btnBackDetectTrigger_Click(object sender, EventArgs e)
        {
            Timer.Start();
            if (cmxDetectType.SelectedIndex == 0)
            {
                Motion.CompareStop(CompareName.BackDetectGrab);
                await Motion.MoveAbsSync(AxisName.BackDetect_X, PosName.GrabStartPos);
                Motion.SetCompare(CompareName.BackDetectGrab);
                await Motion.MoveAbsSync(AxisName.BackDetect_X, PosName.GrabEndPos);    
            }
            else
            {
                Motion.CompareStop(CompareName.IRDetectGrab);
                await Motion.MoveAbsSync(AxisName.IRDetect_X, PosName.GrabStartPos);
                Motion.SetCompare(CompareName.IRDetectGrab);
                await Motion.MoveAbsSync(AxisName.IRDetect_X, PosName.GrabEndPos);
            }
        }

        private async void btnTriggerStartPos_Click(object sender, EventArgs e)
        {
            if (cmxDetectType.SelectedIndex == 0)
            {
               await Motion.MoveAbsSync(AxisName.BackDetect_X, PosName.GrabStartPos);
            }
            else
            {
                await Motion.MoveAbsSync(AxisName.IRDetect_X, PosName.GrabStartPos);
            }
        }

        private async void btnTriggerEndPos_Click(object sender, EventArgs e)
        {
            if (cmxDetectType.SelectedIndex == 0)
            {
                await Motion.MoveAbsSync(AxisName.BackDetect_X, PosName.GrabEndPos);
            }
            else
            {
                await Motion.MoveAbsSync(AxisName.IRDetect_X, PosName.GrabEndPos);
            }
        }

        private async void btnAxisZDetectPos_Click(object sender, EventArgs e)
        {
            if (cmxDetectType.SelectedIndex == 0)
            {
                await Motion.MoveAbsSync(AxisName.BackDetect_Z, PosName.DetectPos);
            }
            else
            {
                await Motion.MoveAbsSync(AxisName.IRDetect_Z, PosName.DetectPos);
            }
        }

        private void btnTriggerStop_Click(object sender, EventArgs e)
        {
            if (cmxDetectType.SelectedIndex == 0)
            {
                Motion.CompareStop(CompareName.BackDetectGrab);
            }
            else
            {
                Motion.CompareStop(CompareName.IRDetectGrab);
            }
            Timer.Stop();
        }

        private void LoadDieDDTeachCtrl_Load(object sender, EventArgs e)
        {

        }
        private void Timer_Tick(object sender, EventArgs e)
        {
            CompareName compareName = CompareName.BackDetectGrab;
            if (DetectType == 1)
            {
                compareName = CompareName.IRDetectGrab;
            }
            Motion.GetCompare(compareName, out int count, out short status);
            this.Invoke(new Action(() =>
            {
                UpdateUI(count, status);
            }));
         
        }
        private void UpdateUI(int count, short status)
        {
            tbxBackDetectTriggerCount.Text = count.ToString();
            if (status == 0)
            {
                Timer.Stop();
                FrmTips.ShowTipsSuccess(null, "飞拍完成");
            }
        }
    }
}
