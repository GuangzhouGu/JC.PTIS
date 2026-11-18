using JC.LidAttach.Devices.Laser;
using JC.PTIS.Main;
using JC.PTIS.Main.Devices.Vision;
using JC.PTIS.Main.Devices.VisionPC;
using JC.PTIS.Main.Devices.VisionPC.MsgModel;
using JC.PTIS.Main.Entities;
using JC.PTIS.Main.Models;
using JC.PTIS.Main.MotionInfo;
using JC.PTIS.Main.RotateInfo;
using JC.PTIS.Main.UserCtrls.FuncTeach;
using Newtonsoft.Json;
using Samrtind;
using StdLib.PCBaseFrame.Extension.Entities;
using StdLib.PCBaseFrame.Motion;
using StdLib.WindowsControls.Class;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace JC.LidDispenser.UserCtrl.Devices
{
    public partial class AlignPcCtrl : TeachCtrlBase
    {
        public AlignPcCtrl()
        {
            InitializeComponent();
        }
        private async void btnSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (AlignHelper.Instance != null)
                {
                    AlignStationEnum station = AlignStationEnum.None;
                    var cmd =EnumHelper.GetEnumValue(cmxCmds.SelectedItem.ToString(),VisionCmd.Align);
                    if (cmxAlignStation.SelectedItem!=null)
                    {
                        station = EnumHelper.GetEnumValue(cmxAlignStation.SelectedItem.ToString(), AlignStationEnum.None);
                    }
                    //if (station == AlignStationEnum.None)
                    //{
                    //    return;
                    //}
                    var stnNo = cmxStnNo.SelectedIndex;
                    var ret = await Task.Run(() => { return AlignHelper.Instance.Send<IMsgBody>(cmd, station, stnNo); });
                    if (ret != null)
                    { 
                        var jsonStr = JsonConvert.SerializeObject(ret, Formatting.Indented);
                        var strArr = jsonStr.Split('\n').Reverse().ToArray();
                        for (int i = 0; i < strArr.Length; i++)
                        {
                            lbxLaserMsg.AddListBoxMsg(strArr[i], Brushes.Green,false);
                        }
                        lbxLaserMsg.AddListBoxMsg($"命令:{cmd} 返回值：", Brushes.Green);
                    }
                    else
                    {
                        lbxLaserMsg.AddListBoxMsg($"命令:{cmd} 返回值为空", Brushes.Red);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }
      
        private void btnClearLaserMsg_Click(object sender, EventArgs e)
        {
            lbxLaserMsg.Items.Clear();
        }
        /// <summary>
        /// 初始化
        /// </summary>
        public void InitCtrl()
        {
           var cmds=Enum.GetValues(typeof(VisionCmd));
            for (int i = 0; i < cmds.Length; i++)
            {
                if (i>= VisionCmd.Align.ToInt()&& i <= VisionCmd.DetectLabel.ToInt())
                {
                    cmxCmds.Items.Add(cmds.GetValue(i));
                }
            }
            foreach (var cmd in cmds)
            {
                cmxCmds.Items.Add(cmd);
            }
            var alignTypes = Enum.GetValues(typeof(AlignStationEnum));
            foreach (var alignType in alignTypes)
            {
                cmxAlignStation.Items.Add(alignType);
            }

            if (cmxCmds.Items.Count > 0)
            {
                cmxCmds.SelectedIndex = 0;
            }
            if (cmxAlignStation.Items.Count>0)
            {
                cmxAlignStation.SelectedIndex = 0;
            }
            cmxStnNo.SelectedIndex = 0;
        }

        private void LaserCtrl_Load(object sender, EventArgs e)
        {
            AlignHelper.Request += AlignHelper_Request;
            if (ClassHelper.IsDesingMode() == false)
            {
                InitCtrl();
            }
        }

        private async void AlignHelper_Request(VisionCmd cmd,BodyBase obj)
        {
            switch (cmd)
            {
                case VisionCmd.AutoCalibration:
                    {
                        ShowMsg(obj);
                        var body = obj as AutoCalibrationReqBody;
                        var station = EnumHelper.GetEnumValue(body.Station, AlignStationEnum.None);
                        if (station != AlignStationEnum.None)
                        {
                             AxisListMove(station);
                        }
                        break;
                    }
                case VisionCmd.MotionRequest:
                    {
                        ShowMsg(obj);
                        MotionRequestRspBody replyBody = new MotionRequestRspBody();
                        var body = obj as MotionRequestReqBody;
                        var station = EnumHelper.GetEnumValue(body.Station, AlignStationEnum.None);
                        var ret = false;
                        List<AxisName> axes = new List<AxisName>();
                        if (station != AlignStationEnum.None)
                        {
                            var pos = body.MovePos;
                            axes = GetAxesName(station);
                            if (axes.Count>0)
                            {
                                if (IsCanMove(axes))
                                {
                                    ret = await Motion.MoveRelMSync(axes.ToArray(), pos.ToArray());
                                    if (ret == false)
                                    {
                                        replyBody.FaultMessage = "Move Timeout";
                                    }
                                }
                                else
                                {
                                    replyBody.FaultMessage = "Condition not satisfied";
                                }
                            }
                            else
                            {
                                replyBody.FaultMessage = "Axis Name Fail";
                            }
                        }
                        else
                        {
                            replyBody.FaultMessage = "Station Name Error";
                        }
                        replyBody.Code =ret? 0:-1;
                        var xyt = new Xyt();
                        if (axes.Count>0)
                        {
                            xyt.X = Motion.GetAxisActPos(axes[0]);
                            xyt.Y = Motion.GetAxisActPos(axes[1]);
                            xyt.T = Motion.GetAxisActPos(axes[2]);
                        }
                        replyBody.AxisPos = xyt;
                        replyBody.Station = body.Station;
                        AlignHelper.Instance.Reply(VisionCmd.MotionRequest, replyBody, station);
                        break;
                    }
            }
        }

        private void UpdateAlignTypeCmx(VisionCmd cmd)
        {
            cmxAlignStation.Items.Clear();
            cmxAlignStation.Text = "";
            switch (cmd)
            {
                case VisionCmd.Align:
                    {
                        for (int i = AlignStationEnum.WaferNotch.ToInt(); i <= AlignStationEnum.Tray_HalfNgTray.ToInt(); i++)
                        {
                            cmxAlignStation.Items.Add((AlignStationEnum)i);
                        }
                        break;
                    }
                case VisionCmd.Grab:
                    {
                        for (int i = AlignStationEnum.WaferNotch.ToInt(); i <= AlignStationEnum.Label.ToInt(); i++)
                        {
                            cmxAlignStation.Items.Add((AlignStationEnum)i);
                        }
                        break;
                    }
            }
            if (cmxAlignStation.Items.Count > 0)
            {
                cmxAlignStation.SelectedIndex = 0;
            }
        }

        private void cmxCmds_SelectedIndexChanged(object sender, EventArgs e)
        {
            var cmd = EnumHelper.GetEnumValue(cmxCmds.SelectedItem.ToString(), VisionCmd.Align);
            UpdateAlignTypeCmx(cmd);
        }
        public List<AxisName> GetAxesName(AlignStationEnum station)
        {
           List<AxisName> ret = new List<AxisName>();
            try
            {
                string str = string.Empty;
                switch (station)
                {
                    case AlignStationEnum.WaferMark:
                        {
                            ret.Add(AxisName.WaferTable_X);
                            ret.Add(AxisName.WaferTable_Y);
                            ret.Add(AxisName.WaferTable_T);
                        }
                        break;
                    case AlignStationEnum.Die_DieToTray:
                    case AlignStationEnum.Tray_DieToTray:
                        {
                            var stageNo = RotateInfoHelper.Instance.GetStageNo(Stn4TurnTableStnEnum.DieToTray.ToInt(), RotateStnType.Stn4TurnTable);
                            var stage = StageManager.Get(stageNo);
                            ret.Add(stage.X);
                            ret.Add(stage.Y);
                            ret.Add(stage.T);
                        }
                        break;
                    case AlignStationEnum.Die_PickPatch:
                    case AlignStationEnum.Tray_PickPatch:
                        {
                            var stageNo = RotateInfoHelper.Instance.GetStageNo(Stn4TurnTableStnEnum.PickPatchStn.ToInt(), RotateStnType.Stn4TurnTable);
                            var stage = StageManager.Get(stageNo);
                            ret.Add(stage.X);
                            ret.Add(stage.Y);
                            ret.Add(stage.T);
                        }
                        break;
                    case AlignStationEnum.Die_HalfOkTray:
                    case AlignStationEnum.Tray_HalfOkTray:
                    case AlignStationEnum.Die_HalfNgTray:
                    case AlignStationEnum.Tray_HalfNgTray:
                        {
                            ret.Add(AxisName.HalfTray_StageX);
                            ret.Add(AxisName.HalfTray_StageY);
                            ret.Add(AxisName.HalfTray_StageT);
                        }
                        break;
                    case AlignStationEnum.IsoPaper:
                        {
                            ret.Add(AxisName.IsolationPaper_ArmX);
                            ret.Add(AxisName.IsolationPaper_ArmY);
                            ret.Add(AxisName.IsolationPaper_ArmT);
                        }
                        break;
                    case AlignStationEnum.Label:
                        {
                            ret.Add(AxisName.Packing_ArmX);
                            ret.Add(AxisName.Packing_ArmY);
                            ret.Add(AxisName.Packing_ArmT);
                        }
                        break;
                }
            
            }
            catch (Exception ex)
            {
                lbxLaserMsg.AddListBoxMsg($"GetAxisName执行异常:{ex.ToString()}", Brushes.Red);
            }
            return ret;
        }
        public async void AxisListMove(AlignStationEnum station)
        {
            bool ret = false;
            List<AxisName> axisList = new List<AxisName>();
            List <PosName>posNames=new List<PosName>();
            switch (station)
            {
                case AlignStationEnum.WaferMark:
                    {
                        axisList.Add(AxisName.WaferTable_X);
                        axisList.Add(AxisName.WaferTable_Y);
                        axisList.Add(AxisName.WaferTable_T);
                        posNames.Add(PosName.MarkDie1Pos);
                        posNames.Add(PosName.MarkDie1Pos);
                        posNames.Add(PosName.WaitPos);
                    }
                    break;
                case AlignStationEnum.Tray_DieToTray:
                    {
                        var stageNo = RotateInfoHelper.Instance.GetStageNo(Stn4TurnTableStnEnum.DieToTray.ToInt(), RotateStnType.Stn4TurnTable);
                        var stage = StageManager.Get(stageNo);
                        axisList.Add(stage.X);
                        axisList.Add(stage.Y);
                        axisList.Add(stage.T);
                        posNames.Add(PosName.GrabTrayPos);
                        posNames.Add(PosName.GrabTrayPos);
                        posNames.Add(PosName.GrabTrayPos);
                    }
                    break;
                case AlignStationEnum.Tray_PickPatch:
                    {
                        var stageNo = RotateInfoHelper.Instance.GetStageNo(Stn4TurnTableStnEnum.PickPatchStn.ToInt(), RotateStnType.Stn4TurnTable);
                        var stage = StageManager.Get(stageNo);
                        axisList.Add(stage.X);
                        axisList.Add(stage.Y);
                        axisList.Add(stage.T);
                        posNames.Add(PosName.GrabTrayPos);
                        posNames.Add(PosName.GrabTrayPos);
                        posNames.Add(PosName.GrabTrayPos);
                    }
                    break;
                case AlignStationEnum.Tray_HalfOkTray:
                case AlignStationEnum.Tray_HalfNgTray:
                    {
                        axisList.Add(AxisName.HalfTray_StageX);
                        axisList.Add(AxisName.HalfTray_StageY);
                        axisList.Add(AxisName.HalfTray_StageT);
                        if (station == AlignStationEnum.Tray_HalfOkTray)
                        {
                            posNames.Add(PosName.GrabOkTrayPos);
                            posNames.Add(PosName.GrabOkTrayPos);
                            posNames.Add(PosName.GrabOkTrayPos);
                        }
                        else
                        {
                            posNames.Add(PosName.GrabNgTrayPos);
                            posNames.Add(PosName.GrabNgTrayPos);
                            posNames.Add(PosName.GrabNgTrayPos);
                        }
                    }
                    break;

                case AlignStationEnum.IsoPaper:
                    {
                        axisList.Add(AxisName.IsolationPaper_ArmX);
                        axisList.Add(AxisName.IsolationPaper_ArmY);
                        axisList.Add(AxisName.IsolationPaper_ArmT);
                        posNames.Add(PosName.PaperGrabPos);
                        posNames.Add(PosName.PaperGrabPos);
                        posNames.Add(PosName.PaperGrabPos);
                    }
                    break;
                case AlignStationEnum.Label:
                    {
                        axisList.Add(AxisName.Packing_ArmX);
                        axisList.Add(AxisName.Packing_ArmY);
                        axisList.Add(AxisName.Packing_ArmT);
                        posNames.Add(PosName.GrabLabelPos);
                        posNames.Add(PosName.GrabLabelPos);
                        posNames.Add(PosName.GrabLabelPos);
                    }
                    break;
            }
            AutoCalibrationRspBody replyBody = new AutoCalibrationRspBody();
            replyBody.Station = station.ToString();
            if (axisList.Count>0)
            {
                if (IsCanMove(axisList))
                {
                    ret = await Motion.MoveAbsMSync(axisList.ToArray(), posNames.ToArray(), 30);
                    if (ret)
                    {
                        var xyt= new Xyt();
                        xyt.X= Motion.GetAxisActPos(axisList[0]);
                        xyt.Y= Motion.GetAxisActPos(axisList[1]);
                        xyt.T= Motion.GetAxisActPos(axisList[2]);
                        replyBody.AxisPos= xyt;
                    }
                    else
                    {
                        replyBody.FaultMessage = "Move Timeout";
                    }
                }
                else
                {
                    replyBody.FaultMessage = "Condition not satisfied";
                }
            }
            else
            {
                replyBody.FaultMessage = "Axis Name Error";
            }
            replyBody.Code = ret ? 0 : -1;
            AlignHelper.Instance.Reply(VisionCmd.AutoCalibration, replyBody, station);
        }
        private void ShowMsg(IMsgBody msg)
        {
            try
            {
                if (InvokeRequired)
                {
                    this.Invoke(new Action<IMsgBody>(ShowMsg), msg);
                }
                else
                {
                    var jsonStr = JsonConvert.SerializeObject(msg, Formatting.Indented);
                    var strArr = jsonStr.Split('\n').Reverse().ToArray();
                    for (int i = 0; i < strArr.Length; i++)
                    {
                        lbxLaserMsg.AddListBoxMsg(strArr[i], Brushes.Green, false);
                    }
                }
            }
            catch (Exception ex)
            {
                lbxLaserMsg.AddListBoxMsg(ex.ToString(), Brushes.Red);
            }
        }
        private void btnCalibration_Click(object sender, EventArgs e)
        {
            AutoCalibrationReqBody body = new AutoCalibrationReqBody();
            body.Station = AlignStationEnum.WaferMark.ToString();
            AlignHelper.Instance.Test(VisionCmd.AutoCalibration,body);
        }

        private void btnMotionRequest_Click(object sender, EventArgs e)
        {
            MotionRequestReqBody body = new MotionRequestReqBody();
            body.Station = AlignStationEnum.WaferMark.ToString();
            body.MovePos =new Xyt() { X=10,Y=10,T=10};
            body.MotionType = MotionType.Relative;
            AlignHelper.Instance.Test(VisionCmd.MotionRequest, body);
        }
    }
}
