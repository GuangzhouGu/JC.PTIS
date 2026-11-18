using APS168_W32;
using JC.B17AutoMating.SubForms;
using JC.PTIS.Main.Devices.Scaner;
using JC.PTIS.Main.Devices.Vision;
using JC.PTIS.Main.Devices.VisionPC.MsgModel;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Params;
using JC.PTIS.Main.Service;
using JC.PTIS.Main.Stations;
using JC.PTIS.Main.SubViews;
using StdLib.HTCard;
using StdLib.PCBaseFrame.Args;
using StdLib.PCBaseFrame.Extension.Entities;
using StdLib.PCBaseFrame.Extension.Recipe;
using StdLib.PCBaseFrame.Motion;
using StdLib.PCBaseFrame.Station;
using StdLib.WindowsControls;
using StdLib.WindowsControls.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.PTIS.Main.UserCtrls.FuncTeach
{
    public partial class WaferTableStnTeachCtrl : TeachCtrlBase
    {
        public WaferTableStnTeachCtrl()
        {
            InitializeComponent();
            StationName = StationName.WaferTableStn;
        }
        WaferTableStn Station = null;
        CancellationTokenSource Cts = null;
        Xyt Xyt = null;
        AxisName[] Axes = null;
        PosName[] Poses = null;

        private void InitCtrl()
        {
            cmxMarkList.SelectedIndex = 0;
            cmxMoveRel.SelectedIndex = 0;
        }

        private void MeasureHeightTeachCtrl_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode() == false)
            {
                InitCtrl();
                ShowPara();
                Station = StationManager.Instance.GetChild<WaferTableStn>(nameof(WaferTableStn));
            }
        }

        private async void btnPickWaferFromFoup_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否执行从缓存平台中取Wafer流程") == false)
            {
                return;
            }
            if (IsCanMove(null) == false)
            {
                return;
            }
            btnPickWaferFromFoup.Enabled = false;
            Cts = new CancellationTokenSource();
            var ret = await TaskTool.RunStateMachine(Station.LoadWaferFlow, 1, Cts.Token, 100);
            if (ret == 0)
            {
                FrmTips.ShowTipsSuccess(null, $"从缓存平台中取Wafer完成");
            }
            else if (ret == -1)
            {
                FrmTips.ShowTipsWarning(null, $"从缓存平台中取Wafer失败");
            }
            btnPickWaferFromFoup.Enabled = true;
        }

        private async void btnWaitPos_Click(object sender, EventArgs e)
        {
            var axes = new List<AxisName>() { AxisName.WaferTable_Y, AxisName.WaferTable_Z };
            if (UIMessageDialog.ShowAskDialog(null, "是否执行移动到待机位流程") == false)
            {
                return;
            }
            if (IsCanMove(axes) == false)
            {
                return;
            }
            await Motion.MoveAbsSync(AxisName.WaferTable_X, PosName.WaitPos);
            var poses = new List<PosName>() { PosName.WaitPos, PosName.WaitPos };
            await Motion.MoveAbsMSync(axes.ToArray(), poses.ToArray(), 30);
        }

        private void btnAutoPickStop_Click(object sender, EventArgs e)
        {
            Cts.Cancel();
            StationManager.Instance.Stop();
            EnableBtns(true);
        }

        private void btnPause_Click(object sender, EventArgs e)
        {
            TaskTool.Pause();
        }

        private void btnResume_Click(object sender, EventArgs e)
        {
            TaskTool.Resume();
        }

        private async void btnPlaceWafer_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否执行放Wafer到缓存平台流程") == false)
            {
                return;
            }
            if (IsCanMove(null) == false)
            {
                return;
            }
            btnPlaceWafer.Enabled = false;
            Cts = new CancellationTokenSource();
            var ret = await TaskTool.RunStateMachine(Station.UnloadWaferFlow, 1, Cts.Token, 100);
            if (ret == 0)
            {
                FrmTips.ShowTipsSuccess(null, $"放Wafer到缓存平台完成");
            }
            else if (ret == -1)
            {
                FrmTips.ShowTipsWarning(null, $"放Wafer到缓存平台失败");
            }
            btnPlaceWafer.Enabled = true;
        }

        private void btnLoadMap_Click(object sender, EventArgs e)
        {
            cmxRowList.Items.Clear();
            cmxColList.Items.Clear();
            OpenFileDialog openFileDialog = new OpenFileDialog();
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                var path = openFileDialog.FileName;
                var tempWafer = WaferMapHelper.Instance.Parse(path);
                if (tempWafer != null)
                {
                    tempWafer.Code = "T0891512";
                    tempWafer.FoupNo = 0;
                    tempWafer.LayerNo = 0;
                    tempWafer.MapFilePath = @"WaferMap\T08915-12-H1.txt";
                    ProcInfoHelper.Instance.PutWafer(WaferPosEnum.WaferTable, tempWafer);
                    for (int i = 0; i < tempWafer.ROWCT; i++)
                    {
                        cmxRowList.Items.Add(i.ToString());
                    }
                    for (int i = 0; i < tempWafer.COLCT; i++)
                    {
                        cmxColList.Items.Add(i.ToString());
                    }
                    cmxRowList.SelectedIndex = 0;
                    cmxColList.SelectedIndex = 0;
                }
            }
        }

        private void btnGrabMark_Click(object sender, EventArgs e)
        {
            var markNo = cmxMarkList.SelectedIndex;
            var ret2 = AlignHelper.Instance.Send<BodyBase>(VisionCmd.Grab, AlignStationEnum.WaferMark, markNo);
            if (ret2.Code == 0)
            {
                FrmTips.ShowTipsSuccess(null, $"第{markNo + 1}个Mark拍照完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, $"第{markNo + 1}个Mark拍照失败");
            }
        }

        private void btnGetWaferXyt_Click(object sender, EventArgs e)
        {
            var markNo = cmxMarkList.SelectedIndex;
            var ret = AlignHelper.Instance.Send<AlignRspBody>(VisionCmd.Align, AlignStationEnum.WaferMark);
            if (ret != null && ret.Offset != null)
            {
                Xyt = ret.Offset;
                tbxWaferOffset.Text = ret.Offset.ToString();
            }
        }

        private async void btnWaferAlign_Click(object sender, EventArgs e)
        {
            var axes = new List<AxisName>() { AxisName.WaferTable_X, AxisName.WaferTable_Y, AxisName.WaferTable_T };
            if (IsCanMove(null) == false)
            {
                return;
            }
            if (Xyt != null)
            {
                var poses = Xyt.ToList();
                var ret = await Motion.MoveRelMSync(axes.ToArray(), poses.ToArray());
                if (ret)
                {
                    FrmTips.ShowTipsSuccess(null, $"Wafer补正完成");
                }
                else
                {
                    FrmTips.ShowTipsWarning(null, $"Wafer补正失败");
                }
                Xyt = null;
            }
        }

        private async void btnMoveToDie_Click(object sender, EventArgs e)
        {
            bool isCam = rbtnCam.Checked;
            if (cmxRowList.Items.Count == 0 || cmxColList.Items.Count == 0)
            {
                FrmTips.ShowTipsWarning(null, $"未加载WaferMap");
                return;
            }
            var rowNo = cmxRowList.SelectedIndex;
            var colNo = cmxColList.SelectedIndex;
            if (UIMessageDialog.ShowAskDialog(null, $"是否移动到第{rowNo + 1}行，第{colNo + 1}列Die") == false)
            {
                return;
            }
            var axes = new List<AxisName>() { AxisName.WaferTable_X, AxisName.WaferTable_Y };
            if (IsCanMove(null) == false)
            {
                return;
            }
            var diePos = GetDieGrabPos(rowNo, colNo);
            if (diePos != null)
            {
                var poses = new List<double>() { diePos.X, diePos.Y };
                if (isCam == false)//如果移动吸嘴，则需要加上吸嘴和相机的偏移量
                {
                    poses[0] += ParaHelper.Offset.LoadDieNozzleAndCameraOffset.X;
                    poses[1] += ParaHelper.Offset.LoadDieNozzleAndCameraOffset.Y;
                }
                var ret = await Motion.MoveAbsMSync(axes.ToArray(), poses.ToArray());
                if (ret)
                {
                    FrmTips.ShowTipsSuccess(null, $"模组移动到第{rowNo + 1}行，第{colNo + 1}列Die完成");
                }
                else
                {
                    FrmTips.ShowTipsWarning(null, $"模组移动到第{rowNo + 1}行，第{colNo + 1}列Die失败");
                }
            }
        }

        private void btnDieGrab_Click(object sender, EventArgs e)
        {
            if (cmxRowList.Items.Count == 0 || cmxColList.Items.Count == 0)
            {
                FrmTips.ShowTipsWarning(null, $"未加载WaferMap");
                return;
            }
            var rowNo = cmxRowList.SelectedIndex;
            var colNo = cmxColList.SelectedIndex;
            var dieIndex = rowNo * cmxColList.Items.Count + colNo;
            var ret = AlignHelper.Instance.Send<AlignRspBody>(VisionCmd.Align, AlignStationEnum.Die_WaferTable, dieIndex);
            if (ret.Code == 0 && ret.Offset != null)
            {
                Xyt = ret.Offset;
                tbxDieOffset.Text = ret.Offset.ToString();
                FrmTips.ShowTipsSuccess(null, $"第{rowNo + 1}行，第{colNo + 1}列Die拍照完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, $"第{rowNo + 1}行，第{colNo + 1}列Die拍照失败");
            }
        }

        private async void btnDieAlign_Click(object sender, EventArgs e)
        {
            if (Xyt != null)
            {
                var axes = new List<AxisName>() { AxisName.WaferTable_X, AxisName.WaferTable_Y, AxisName.WaferTable_T };
                var poses = Xyt.ToList();
                var ret = await Motion.MoveRelMSync(axes.ToArray(), poses.ToArray());
                if (ret)
                {
                    FrmTips.ShowTipsSuccess(null, $"Die补正完成");
                }
                else
                {
                    FrmTips.ShowTipsWarning(null, $"Die补正失败");
                }
                Xyt = null;
            }
        }

        private async void btnMoveToMark_Click(object sender, EventArgs e)
        {
            var axes = new List<AxisName>() { AxisName.WaferTable_X, AxisName.WaferTable_Y };
            var markNo = cmxMarkList.SelectedIndex;
            if (UIMessageDialog.ShowAskDialog(null, $"是否移动到第{markNo + 1}个Mark") == false)
            {
                return;
            }
            if (IsCanMove(axes) == false)
            {
                return;
            }
            var poses = new List<PosName>() { PosName.MarkDie1Pos };
            var ret = await Motion.MoveAbsMSync(axes.ToArray(), poses.ToArray());
            if (ret)
            {
                FrmTips.ShowTipsSuccess(null, $"模组移动到第{markNo + 1}个Mark完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, $"模组移动到第{markNo + 1}个Mark失败");
            }
        }
        private Xyt GetDieGrabPos(int rowNo, int colNo)
        {
            Xyt ret = null;

            var waferModel = ProcInfoHelper.Instance.GetWaferModel(WaferPosEnum.WaferTable);
            if (waferModel != null)
            {
                var dieXPos = (colNo - 0) * (waferModel.XDIES / 1000.0);
                var dieYPos = (rowNo - 0) * (waferModel.YDIES / 1000.0);
                ret = new Xyt() { X = AxisInfoHelper.GetPosInfoByName(AxisName.WaferTable_X, PosName.MarkDie1Pos).Pos + dieXPos, Y = AxisInfoHelper.GetPosInfoByName(AxisName.WaferTable_Y, PosName.MarkDie1Pos).Pos + dieYPos };
            }
            return ret;
        }

        private void btnFirstDieCrd_Click(object sender, EventArgs e)
        {
            //if (UIMessageDialog.ShowAskDialog(null, $"Wafer是否在基准位") == false)
            //{
            //    return;
            //}
            //ParaHelper.Prod.BaseDieCrd.X = AxisInfoHelper.GetPosInfoByName(AxisName.WaferTable_X,PosName.MarkDie1Pos).Pos; 
            //ParaHelper.Prod.BaseDieCrd.Y = AxisInfoHelper.GetPosInfoByName(AxisName.WaferTable_Y, PosName.MarkDie1Pos).Pos;
            //tbxBaseDieCrd.Text = $" {ParaHelper.Prod.BaseDieCrd.ToString()}";
        }

        private void btnSaveCrd_Click(object sender, EventArgs e)
        {
            try
            {
                ParaHelper.Prod.BaseDieRowNo = int.Parse(tbxBaseDieRowNo.Text);
                ParaHelper.Prod.BaseDieColNo = int.Parse(tbxBaseDieColNo.Text);
                var ret = RecipeManager.SavePara(nameof(ParaHelper.Prod));
                if (ret)
                {
                    FrmTips.ShowTipsSuccess(null, $"保存成功");
                }
                else
                {
                    FrmTips.ShowTipsWarning(null, $"保存失败");
                }
            }
            catch (Exception ex)
            {
                UIMessageDialog.ShowErrorDialog(null, ex.ToString());
            }
        }

        private async void btnWaferAutoAlign_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否Wafer对位流程") == false)
            {
                return;
            }
            if (IsCanMove(null) == false)
            {
                return;
            }
            btnWaferAutoAlign.Enabled = false;
            Cts = new CancellationTokenSource();
            var ret = await TaskTool.RunStateMachine(Station.WaferAlignFlow, 1, Cts.Token, 100);
            if (ret == 0)
            {
                FrmTips.ShowTipsSuccess(null, $"Wafer对位完成");
            }
            else if (ret == -1)
            {
                FrmTips.ShowTipsWarning(null, $"Wafer对位失败");
            }
            btnWaferAutoAlign.Enabled = true;
        }

        private async void btnDieAutoAlign_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否Wafer对位流程") == false)
            {
                return;
            }
            if (IsCanMove(null) == false)
            {
                return;
            }
            btnDieAutoAlign.Enabled = false;
            Cts = new CancellationTokenSource();
            Station.IsManual = true;
            var ret = await TaskTool.RunStateMachine(Station.AlignDieFlow, 1, Cts.Token, 100);
            if (ret == 0)
            {
                FrmTips.ShowTipsSuccess(null, $"Wafer对位完成");
            }
            else if (ret == -1)
            {
                FrmTips.ShowTipsWarning(null, $"Wafer对位失败");
            }
            Station.IsManual = false;
            btnDieAutoAlign.Enabled = true;
        }
        private void btnUp_Click(object sender, EventArgs e)
        {
            AxisName axis = AxisName.None;
            var btn = (Button)sender;
            var axisName = btn.Text.Substring(0, 1);
            var bit = btn.Text.Substring(1, 1) == "+";
            var dir = (short)(bit ? 1 : -1);
            axis = GetAxisName(axisName);
            if (rbtnMoveRel.Checked)
            {
                if (axis != AxisName.None)
                {
                    var ret = Motion.IsCanMove(new MoveArgs { AxisName = axis.ToString() });
                    if (ret.Result)
                    {
                        var pos = double.Parse(cmxMoveRel.SelectedItem.ToString());
                        Motion.MoveRel(axis, pos * dir);
                    }
                    else
                    {
                        FrmTips.ShowTipsWarning(null, $"{ret.Msg}");
                    }
                }
                else
                {
                    FrmTips.ShowTipsWarning(null, "当前轴为空");
                }
            }
        }

        private void btnUp_MouseDown(object sender, MouseEventArgs e)
        {
            AxisName axis = AxisName.None;
            var btn = (Button)sender;
            var axisName = btn.Text.Substring(0, 1);
            var bit = btn.Text.Substring(1, 1) == "+";
            axis = GetAxisName(axisName);
            var dir = (short)(bit ? 1 : -1);
            if (axis != AxisName.None)
            {
                var ret = Motion.IsCanMove(new MoveArgs { AxisName = axis.ToString() });
                if (ret.Result)
                {
                    if (rbtnJog.Checked)
                    {
                        Motion.Jog(axis, dir);
                    }
                }
                else
                {
                    FrmTips.ShowTipsWarning(null, $"{ret.Msg}");
                }
            }
            else
            {
                FrmTips.ShowTipsWarning(null, "当前轴为空");
            }
        }

        private void btnUp_MouseUp(object sender, MouseEventArgs e)
        {
            var btn = (Button)sender;
            var axisName = btn.Text.Substring(0, 1);
            AxisName axis = AxisName.None;
            axis = GetAxisName(axisName);
            if (axis != AxisName.None)
            {
                var ret = Motion.IsCanMove(new MoveArgs { AxisName = axis.ToString() });
                if (ret.Result)
                {
                    if (rbtnJog.Checked)
                    {
                        Motion.Stop(axis);
                    }
                }
                else
                {
                    FrmTips.ShowTipsWarning(null, $"{ret.Msg}");
                }
            }
            else
            {
                FrmTips.ShowTipsWarning(null, "当前轴为空");
            }
        }
        private AxisName GetAxisName(string axisName)
        {
            var axis = AxisName.None;
            var str = $"WaferTable_{axisName}";
            axis = EnumHelper.GetEnumValue<AxisName>(str);
            return axis;
        }
        private void EnableBtns(bool isEnable)
        {
            btnPickWaferFromFoup.Enabled = isEnable;
            btnWaitPos.Enabled = isEnable;
            btnAutoPickStop.Enabled = isEnable;
            btnDieAutoAlign.Enabled = isEnable;
            btnWaferAutoAlign.Enabled = isEnable;
        }

        private async void btnLoadWaferPos_Click(object sender, EventArgs e)
        {
            var axes = new List<AxisName>() { AxisName.WaferTable_X, AxisName.WaferTable_Y, AxisName.WaferTable_T };
            if (UIMessageDialog.ShowAskDialog(null, $"是否移动到取放Wafer位置") == false)
            {
                return;
            }
            if (IsCanMove(axes) == false)
            {
                return;
            }
            var ret = await TaskTool.RunStateMachine(Station.WafterTableToLulPos, 1, Cts.Token, 100);
            if (ret == 0)
            {
                FrmTips.ShowTipsSuccess(null, $"移动到取放Wafer位置完成");
            }
            else if (ret == -1)
            {
                FrmTips.ShowTipsWarning(null, $"移动到取放Wafer位置失败");
            }
        }



        private async void htButton1_Click(object sender, EventArgs e)
        {
            var axes = new List<AxisName>() { AxisName.WaferTable_X, AxisName.WaferTable_Y, AxisName.WaferTable_T };
            if (UIMessageDialog.ShowAskDialog(null, $"是否移动扩摸位置") == false)
            {
                return;
            }
            if (IsCanMove(axes) == false)
            {
                return;
            }
            Cts = new CancellationTokenSource();
            var ret = await TaskTool.RunStateMachine(Station.WaferTableZMove, 1, PosName.LiftPos, Cts.Token, 30);
            if (ret == 0)
            {
                FrmTips.ShowTipsSuccess(null, $"移动扩摸位置完成");
            }
            else if (ret == -1)
            {
                FrmTips.ShowTipsWarning(null, $"移动扩摸位置失败");
            }
        }

        private async void htButton2_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, $"是否移动顶针帽到顶起位置") == false)
            {
                return;
            }
            var axes = new List<AxisName>() { AxisName.WaferTable_ThimbleCap };
            if (IsCanMove(axes) == false)
            {
                return;
            }
            await Motion.CylinderAsync(CylinderName.WaferTableThimbleCyl, true);
            var poses = new List<PosName>() { PosName.LiftPos };
            var ret = await Motion.MoveAbsMSync(axes.ToArray(), poses.ToArray());
            if (ret)
            {
                FrmTips.ShowTipsSuccess(null, $"顶针帽顶起完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, $"顶针帽顶起失败");
            }
        }

        private async void btnThimbleCapMoveToWaitPos_Click(object sender, EventArgs e)
        {
            var axes = new List<AxisName>() { AxisName.WaferTable_ThimbleCap, AxisName.WaferTable_Thimble };
            var poses = new List<PosName>() { PosName.WaitPos };
            if (IsCanMove(axes) == false)
            {
                return;
            }
            await Motion.CylinderAsync(CylinderName.WaferTableThimbleCyl, false);
            var ret = await Motion.MoveAbsMSync(axes.ToArray(), poses.ToArray());
            if (ret)
            {
                FrmTips.ShowTipsSuccess(null, $"顶针移动到待机位完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, $"顶针移动到待机位失败");
            }
        }

        private async void btnThimbleLift_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, $"是否移动顶针到顶起位置") == false)
            {
                return;
            }
            var axes = new List<AxisName>() { AxisName.WaferTable_Thimble };
            if (IsCanMove(axes) == false)
            {
                return;
            }
            var poses = new List<PosName>() { PosName.LiftPos };
            var ret = await Motion.MoveAbsMSync(axes.ToArray(), poses.ToArray());
            if (ret)
            {
                FrmTips.ShowTipsSuccess(null, $"顶针顶起完成");
            }
            else
            {
                FrmTips.ShowTipsWarning(null, $"顶针顶起失败");
            }
        }

        private void ShowPara()
        {
            try
            {
                //tbxBaseDieCrd.Text = ParaHelper.Prod.BaseDieCrd.ToString();
                tbxBaseDieRowNo.Text = ParaHelper.Prod.BaseDieRowNo.ToString();
                tbxBaseDieColNo.Text = ParaHelper.Prod.BaseDieColNo.ToString();
            }
            catch (Exception ex)
            {

            }
        }

    }
}
