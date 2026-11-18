using JC.B17AutoMating.SubForms;
using JC.PTIS.Main.Devices.Vision;
using JC.PTIS.Main.Devices.VisionPC.MsgModel;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.Params;
using JC.PTIS.Main.RotateInfo;
using JC.PTIS.Main.Stations;
using StdLib.PCBaseFrame.Station;
using StdLib.WindowsControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.PTIS.Main.UserCtrls.FuncTeach
{
	public partial class PickPatchTeachCtrl : TeachCtrlBase
	{
		public PickPatchTeachCtrl()
		{
			InitializeComponent();
			StationName = StationName.PickPatchStn;
		}
		CancellationTokenSource Cts = null;
		Xyt[] AlignXyt = new Xyt[3];
		Xyt[] FirstCellPos = new Xyt[3];
		double[] PickPatchDDPos = new double[16];
		const double RotateInterval = 22.5;
		int NozzleNo = 0;
		private void PickPatchTeachCtrl_Load(object sender, EventArgs e)
		{
			cmxTrayList.SelectedIndex = 0;
			for (int i = 0; i < 16; i++)
			{
				PickPatchDDPos[i] = 22.5 * i;
			}
			for (int i = 0; i < 12; i++)
			{
				cmxLayerList.Items.Add(i + 1);
			}
			cmxLayerList.SelectedIndex = 0;
		}

		private async void btnTrayGrabPos_Click(object sender, EventArgs e)
		{
			if (UIMessageDialog.ShowAskDialog(null, "是否移动Tray到拍照位置") == false)
			{
				return;
			}
			List<AxisName> axes = null;
			if (cmxTrayList.SelectedIndex == 0)
			{
				var stageNo = RotateInfoHelper.Instance.GetStageNo(Stn4TurnTableStnEnum.PickPatchStn.ToInt(), RotateStnType.Stn4TurnTable);
				var curStage = StageManager.Get(stageNo);
				axes = new List<AxisName>() { curStage.X, curStage.Y, curStage.T, AxisName.PickPatch_CamZ, AxisName.PickPatch_CamY };
			}
			else
			{
				axes = new List<AxisName>() { AxisName.HalfTray_Y, AxisName.HalfTray_StageX, AxisName.HalfTray_StageY, AxisName.HalfTray_StageT };
			}
			if (IsCanMove(axes) == false)
			{
				return;
			}
			if (cmxTrayList.SelectedIndex == 0)
			{
				var posNames = new List<PosName>() { PosName.GrabTrayPos };
				var ret = await Motion.MoveAbsMSync(axes.ToArray(), posNames.ToArray());
				if (ret)
				{
					FrmTips.ShowTipsSuccess(null, $"移动Tray到拍照位置完成");
				}
				else
				{
					FrmTips.ShowTipsWarning(null, "移动Tray到拍照位置失败");
				}
			}
			else
			{
				bool isOkTray = cmxTrayList.SelectedIndex == 1;
				Cts = new CancellationTokenSource();
				var station = StationManager.Instance.GetChild<HalfTrayStn>(nameof(HalfTrayStn));
				var ret = await TaskTool.RunStateMachine(station.HalfTrayToGrabTrayPosFlow, 1, isOkTray, Cts.Token);
				if (ret == 0)
				{
					FrmTips.ShowTipsSuccess(null, $"移动Tray到拍照位置完成");
				}
				else
				{
					FrmTips.ShowTipsWarning(null, "移动Tray到拍照位置失败");
				}
			}
		}

		private void btnTrayGrab_Click(object sender, EventArgs e)
		{
			AlignStationEnum alignStation = AlignStationEnum.Tray_PickPatch;
			if (cmxTrayList.SelectedIndex == 1)
			{
				alignStation = AlignStationEnum.Tray_HalfOkTray;
			}
			else if (cmxTrayList.SelectedIndex == 2)
			{
				alignStation = AlignStationEnum.Tray_HalfNgTray;
			}
			var body = AlignHelper.Instance.Send<AlignRspBody>(VisionCmd.Align, alignStation);
			if (body != null && body.Offset != null)
			{
				AlignXyt[cmxTrayList.SelectedIndex] = body.Offset;
				tbxTrayXyt.Text = $"{AlignXyt.ToString()}";
				FrmTips.ShowTipsSuccess(null, $"Tray拍照完成");
			}
			else
			{
				tbxTrayXyt.Text = "";
				AlignXyt = null;
				FrmTips.ShowTipsWarning(null, "Tray拍照失败");
			}
		}

		private async void btnTrayAlign_Click(object sender, EventArgs e)
		{
			if (UIMessageDialog.ShowAskDialog(null, "是否移动对位补正值") == false)
			{
				return;
			}
			List<AxisName> axes = null;
			if (cmxTrayList.SelectedIndex == 0)
			{
				var stageNo = RotateInfoHelper.Instance.GetStageNo(Stn4TurnTableStnEnum.PickPatchStn.ToInt(), RotateStnType.Stn4TurnTable);
				var curStage = StageManager.Get(stageNo);
				axes = new List<AxisName>() { curStage.X, curStage.Y, curStage.T };
			}
			else
			{
				axes = new List<AxisName>() { AxisName.HalfTray_StageX, AxisName.HalfTray_StageY, AxisName.HalfTray_StageT };
			}
			if (IsCanMove(axes) == false)
			{
				return;
			}
			if (AlignXyt[cmxTrayList.SelectedIndex] != null)
			{
				var ret = await Motion.MoveRelMSync(axes.ToArray(), AlignXyt[cmxTrayList.SelectedIndex].ToArray());
				if (ret)
				{
					FirstCellPos[cmxTrayList.SelectedIndex] = new Xyt();
					FirstCellPos[cmxTrayList.SelectedIndex].X = Motion.GetAxisActPos(axes[0]) + ParaHelper.Prod.FirstCellOffsetX;//计算Tray第一个穴位的X方向坐标
					FirstCellPos[cmxTrayList.SelectedIndex].Y = Motion.GetAxisActPos(axes[1]) + ParaHelper.Prod.FirstCellOffsetY;//计算Tray第一个穴位的Y方向坐标
					FrmTips.ShowTipsSuccess(null, $"移动补正值完成");
				}
				else
				{
					FrmTips.ShowTipsWarning(null, "移动补正值失败");
				}
				AlignXyt = null;
			}
		}

		private async void btnDieGrabPos_Click(object sender, EventArgs e)
		{
			if (UIMessageDialog.ShowAskDialog(null, "是否移动相机到拍Die位置") == false)
			{
				return;
			}
			List<AxisName> axes = null;
			List<PosName> posNames = null;
			if (cmxTrayList.SelectedIndex == 0)
			{
				var stageNo = RotateInfoHelper.Instance.GetStageNo(Stn4TurnTableStnEnum.PickPatchStn.ToInt(), RotateStnType.Stn4TurnTable);
				var curStage = StageManager.Get(stageNo);
				axes = new List<AxisName>() { curStage.X, curStage.Y, curStage.T };
				posNames = new List<PosName>() { PosName.GrabDiePos };
			}

			if (IsCanMove(axes) == false)
			{
				return;
			}
			var ret = await Motion.MoveAbsMSync(axes.ToArray(), posNames.ToArray());
			if (ret)
			{
				FrmTips.ShowTipsSuccess(null, $"相机到拍Die位置完成");
			}
			else
			{
				FrmTips.ShowTipsWarning(null, "相机到拍Die位置失败");
			}
		}

		private async void btnMoveToDie_Click(object sender, EventArgs e)
		{
			var rowIndex = cmxTrayRowList.SelectedIndex;
			var colIndex = cmxTrayColList.SelectedIndex;
			if (UIMessageDialog.ShowAskDialog(null, $"是否移动Tray到第{rowIndex}行第{colIndex}列") == false)
			{
				return;
			}
			List<AxisName> axes = null;
			if (cmxTrayList.SelectedIndex == 0)
			{
				var stageNo = RotateInfoHelper.Instance.GetStageNo(Stn4TurnTableStnEnum.PickPatchStn.ToInt(), RotateStnType.Stn4TurnTable);
				var curStage = StageManager.Get(stageNo);
				axes = new List<AxisName>() { curStage.X, curStage.Y, curStage.T };
			}
			else
			{
				axes = new List<AxisName>() { AxisName.HalfTray_StageX, AxisName.HalfTray_StageY, AxisName.HalfTray_StageT };
			}
			if (IsCanMove(axes) == false)
			{
				return;
			}
			var pos = GetCellPos(rowIndex, colIndex);
			var ret = await Motion.MoveAbsMSync(axes.ToArray(), pos.ToArray());
			if (ret)
			{
				FrmTips.ShowTipsSuccess(null, $"移动Tray到第{rowIndex}行第{colIndex}列完成");
			}
			else
			{
				FrmTips.ShowTipsWarning(null, $"移动Tray到第{rowIndex + 1}行第{colIndex + 1}列失败");
			}
		}
		private Xyt GetCellPos(int rowNo, int colNo)
		{
			var x = FirstCellPos[cmxTrayList.SelectedIndex].X + colNo * ParaHelper.Prod.CellPitchX;
			var y = FirstCellPos[cmxTrayList.SelectedIndex].Y + rowNo * ParaHelper.Prod.CellPitchY;
			return new Xyt() { X = x, Y = y };
		}

		private void cmxTrayList_SelectedIndexChanged(object sender, EventArgs e)
		{

		}

		private void btnDieGrab_Click(object sender, EventArgs e)
		{
			AlignStationEnum alignStation = AlignStationEnum.Die_PickPatch;
			if (cmxTrayList.SelectedIndex == 1)
			{
				alignStation = AlignStationEnum.Die_HalfOkTray;
			}
			else if (cmxTrayList.SelectedIndex == 2)
			{
				alignStation = AlignStationEnum.Die_HalfNgTray;
			}
			var rowNo = cmxTrayRowList.SelectedIndex;
			var colNo = cmxTrayColList.SelectedIndex;
			var dieNo = rowNo * ParaHelper.Prod.TrayColCount + colNo;
			var body = AlignHelper.Instance.Send<AlignRspBody>(VisionCmd.Align, alignStation, dieNo);
			if (body != null && body.Offset != null)
			{
				AlignXyt[cmxTrayList.SelectedIndex] = body.Offset;
				tbxTrayXyt.Text = $"{AlignXyt[cmxTrayList.SelectedIndex].ToString()}";
				FrmTips.ShowTipsSuccess(null, $"Die拍照完成");
			}
			else
			{
				AlignXyt[cmxTrayList.SelectedIndex] = null;
				FrmTips.ShowTipsWarning(null, "Die拍照失败");
			}
		}

		private async void btnDieAlign_Click(object sender, EventArgs e)
		{
			if (UIMessageDialog.ShowAskDialog(null, "是否移动对位补正值") == false)
			{
				return;
			}
			List<AxisName> axes = null;
			if (cmxTrayList.SelectedIndex == 0)
			{
				var stageNo = RotateInfoHelper.Instance.GetStageNo(Stn4TurnTableStnEnum.PickPatchStn.ToInt(), RotateStnType.Stn4TurnTable);
				var curStage = StageManager.Get(stageNo);
				axes = new List<AxisName>() { curStage.X, curStage.Y, curStage.T };
			}
			else
			{
				axes = new List<AxisName>() { AxisName.HalfTray_StageX, AxisName.HalfTray_StageY, AxisName.HalfTray_StageT };
			}
			if (IsCanMove(axes) == false)
			{
				return;
			}
			if (AlignXyt[cmxTrayList.SelectedIndex] != null)
			{
				var ret = await Motion.MoveRelMSync(axes.ToArray(), AlignXyt[cmxTrayList.SelectedIndex].ToArray());
				if (ret)
				{
					FrmTips.ShowTipsSuccess(null, $"移动补正值完成");
				}
				else
				{
					FrmTips.ShowTipsWarning(null, "移动补正值失败");
				}
				AlignXyt = null;
			}
		}

		private async void btnAxisZPickDiePos_Click(object sender, EventArgs e)
		{
			if (cmxTrayList.SelectedIndex == 0)
			{
				await Motion.MoveAbsSync(AxisName.PickPatch_PickZ, PosName.PickDiePos);
			}
			else
			{
				await Motion.MoveAbsSync(AxisName.PickPatch_PlaceZ, PosName.PickDiePos);
			}
		}

		private async void htButton1_Click(object sender, EventArgs e)
		{
			if (cmxTrayList.SelectedIndex == 0)
			{
				await Motion.MoveAbsSync(AxisName.PickPatch_PickZ, PosName.WaitPos);
			}
			else
			{
				await Motion.MoveAbsSync(AxisName.PickPatch_PlaceZ, PosName.WaitPos);
			}
		}

		private async void btnHome_Click(object sender, EventArgs e)
		{
			if (UIMessageDialog.ShowAskDialog(null, $"是否回零") == false)
			{
				return;
			}
			var axes = new List<AxisName>() { AxisName.PickPatch_DD };
			if (IsCanMove(axes, true) == false)
			{
				return;
			}
			NozzleNo = 0;
			var ret = await Motion.HomeSync(AxisName.PickPatch_DD, 60);
			if (ret)
			{
				RotateInfoHelper.Instance.UpdateRotateInfo(RotateStnType.PickPatch, 0);
				FrmTips.ShowTipsSuccess(null, $"挑补DD马达回零完成");
			}
			else
			{
				FrmTips.ShowTipsWarning(null, "挑补DD马达回零失败");
			}
		}

		private async void btnNozzle1Pos_Click(object sender, EventArgs e)
		{
			var btn = (HTButton)sender;
			if (btn.Tag.ToString() == "Next")
			{
				NozzleNo++;
				if (NozzleNo == 16)
				{
					NozzleNo = 0;
				}
			}
			else
			{
				NozzleNo = int.Parse(btn.Tag.ToString());
			}
			if (UIMessageDialog.ShowAskDialog(null, $"是否旋转挑补DD马达到吸嘴{NozzleNo}位置") == false)
			{
				return;
			}
			var axes = new List<AxisName>() { AxisName.PickPatch_DD };
			if (IsCanMove(axes) == false)
			{
				return;
			}

			var destAngle = PickPatchDDPos[NozzleNo];
			var ret = await Motion.MoveAbsSync(AxisName.PickPatch_DD, destAngle);
			if (ret)
			{
				var actPos = Motion.GetAxisActPos(AxisName.PickPatch_DD);
				RotateInfoHelper.Instance.UpdateRotateInfo(RotateStnType.PickPatch, Math.Round(actPos, 1));
				FrmTips.ShowTipsSuccess(null, $"挑补DD马达到吸嘴{NozzleNo}位置完成");
			}
			else
			{
				FrmTips.ShowTipsWarning(null, $"挑补DD马达到吸嘴{NozzleNo}位置失败");
			}
		}

		private async void btnLulTrayPos_Click(object sender, EventArgs e)
		{
			if (UIMessageDialog.ShowAskDialog(null, "是否移动Tray到拍照位置") == false)
			{
				return;
			}
			List<AxisName> axes = null;
			if (cmxTrayList.SelectedIndex == 0)
			{
				var stageNo = RotateInfoHelper.Instance.GetStageNo(Stn4TurnTableStnEnum.PickPatchStn.ToInt(), RotateStnType.Stn4TurnTable);
				var curStage = StageManager.Get(stageNo);
				axes = new List<AxisName>() { curStage.X, curStage.Y, curStage.T };
			}
			else
			{
				axes = new List<AxisName>() { AxisName.HalfTray_Y, AxisName.HalfTray_StageX, AxisName.HalfTray_StageY, AxisName.HalfTray_StageT };
			}
			if (IsCanMove(axes) == false)
			{
				return;
			}
			if (cmxTrayList.SelectedIndex == 0)
			{
				bool isOkTray = cmxTrayList.SelectedIndex == 1;
				Cts = new CancellationTokenSource();
				var station = StationManager.Instance.GetChild<HalfTrayStn>(nameof(HalfTrayStn));
				var ret = await TaskTool.RunStateMachine(station.HalfTrayToLulPosFlow, 1, isOkTray, Cts.Token);
				if (ret == 0)
				{
					FrmTips.ShowTipsSuccess(null, $"移动Tray到取放位置完成");
				}
				else
				{
					FrmTips.ShowTipsWarning(null, "移动Tray到取放位置失败");
				}
			}
			else
			{
				var posNames = new List<PosName>() { PosName.LuloadTrayPos };
				var ret = await Motion.MoveAbsMSync(axes.ToArray(), posNames.ToArray());
				if (ret)
				{
					FrmTips.ShowTipsSuccess(null, $"移动到取放Tray位置完成");
				}
				else
				{
					FrmTips.ShowTipsWarning(null, "移动到取放Tray位置失败");
				}
			}
		}

		private async void btnMoveToDestLayer_Click(object sender, EventArgs e)
		{
			var layerNo = cmxLayerList.SelectedIndex;
			var station = StationManager.Instance.GetChild<HalfTrayStn>(nameof(HalfTrayStn));
			var destPos = station.GetBufferPos(layerNo);
			var axes = new List<AxisName>() { AxisName.HalfTray_BufferZ };
			var poses = new double[] { destPos };
			if (IsCanMove(axes) == false)
			{
				return;
			}
			var ret = await Motion.MoveAbsSync(axes.ToArray(), destPos);
			if (ret)
			{
				FrmTips.ShowTipsSuccess(null, $"半Tray缓存移动到第{layerNo}层完成");
			}
			else
			{
				FrmTips.ShowTipsWarning(null, $"半Tray缓存移动到第{layerNo}层失败");
			}
		}

		private async void btnPPTrayFronTrayStage_Click(object sender, EventArgs e)
		{
			var axes = new List<AxisName>() { AxisName.HalfTray_BufferX };
			var posNames = new List<PosName>() { PosName.BufferArmPPTrayFromHalfTrayStagePos };
			if (IsCanMove(axes) == false)
			{
				return;
			}
			var ret = await Motion.MoveAbsMSync(axes.ToArray(), posNames.ToArray());
			if (ret)
			{
				FrmTips.ShowTipsSuccess(null, $"半Tray缓存X轴移动到从平台取放Tray位置完成");
			}
			else
			{
				FrmTips.ShowTipsWarning(null, $"半Tray缓存X轴移动到从平台取放Tray位置失败");
			}
		}

		private async void btnPPTrayFromBufferPos_Click(object sender, EventArgs e)
		{
			var axes = new List<AxisName>() { AxisName.HalfTray_BufferX };
			var posNames = new List<PosName>() { PosName.BufferArmPPTrayFromBufferPos };
			if (IsCanMove(axes) == false)
			{
				return;
			}
			var ret = await Motion.MoveAbsMSync(axes.ToArray(), posNames.ToArray());
			if (ret)
			{
				FrmTips.ShowTipsSuccess(null, $"半Tray缓存X轴移动到从缓存取放Tray位置完成");
			}
			else
			{
				FrmTips.ShowTipsWarning(null, $"半Tray缓存X轴移动到从缓存取放Tray位置失败");
			}
		}
	}
}
