using JC.LidAttach.Devices.Laser;
using JC.PTIS.Main;
using JC.PTIS.Main.Devices.Vision;
using JC.PTIS.Main.Devices.VisionPC;
using JC.PTIS.Main.Devices.VisionPC.MsgModel;
using Newtonsoft.Json;
using StdLib.PCBaseFrame.Extension.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.LidDispenser.UserCtrl.Devices
{
    public partial class DetectPcCtrl : UserControl
    {
        public DetectPcCtrl()
        {
            InitializeComponent();
        }

        private async void btnLaserSend_Click(object sender, EventArgs e)
        {
            try
            {
                if (DetectHelper.Instance != null)
                {
                
                    var cmdIndex = cmxCmds.SelectedIndex;
                    var detectTypeIndex = cmxDetectType.SelectedIndex;
                    var cmd = (VisionCmd)cmdIndex;
                    if (cmd== VisionCmd.TriggerDetection)
                    {
                        var type = (DetectStationEnum)detectTypeIndex;
                        var stnNo = int.Parse(tbxStnNo.Text);
                        var body = GetBody(cmd, type);
                        body.Station = type.ToString();
                        var ret = await Task.Run(() => { return DetectHelper.Instance.Detect<IMsgBody>(cmd, type, body); });
                        if (ret != null)
                        {
                            var jsonStr = JsonConvert.SerializeObject(ret, Formatting.Indented);
                            var strArr = jsonStr.Split('\n').Reverse().ToArray();
                            for (int i = 0; i < strArr.Length; i++)
                            {
                                lbxLaserMsg.AddListBoxMsg(strArr[i], Brushes.Green, false);
                            }
                            lbxLaserMsg.AddListBoxMsg($"命令:{cmd} 返回值：", Brushes.Green);
                        }
                        else
                        {
                            lbxLaserMsg.AddListBoxMsg($"命令:{cmd} 返回值为空", Brushes.Red);
                        }
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
            foreach (var cmd in cmds)
            {
                cmxCmds.Items.Add(cmd);
            }
            if (cmxCmds.Items.Count>0)
            {
                cmxCmds.SelectedIndex = 0;
            }

            var cmds2 = Enum.GetValues(typeof(DetectStationEnum));
            foreach (var cmd in cmds2)
            {
                cmxDetectType.Items.Add(cmd);
            }
            if (cmxDetectType.Items.Count > 0)
            {
                cmxDetectType.SelectedIndex = 0;
            }
        }

        private void LaserCtrl_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode() == false)
            {
                InitCtrl();
            }
        }
        private TriggerDetectReqBody GetBody(VisionCmd visionCmd, DetectStationEnum detectType)
        {
            var body = new TriggerDetectReqBody
            {
                Station = detectType.ToString(),
                Code = 0,
                LotId = "LOT123456",
                ProductId = "PROD001",
                RecipeName = "Recipe1",
                DefectImageUrl = @"C:\DefectImages"
            };
            switch (detectType)
            {
                case DetectStationEnum.IRDetect:
                case DetectStationEnum.BackDetect:
                    {
                        body.ProductList = new Product[]
                        {
                            new Product { Index=0, SN="SN0001" },
                        };
                    }
                    break;
                case DetectStationEnum.FrontDetect:
                    {
                        body.ProductList = new Product[50];
                        for (int i = 0; i < body.ProductList.Length; i++)
                        {
                            body.ProductList[i] = new Product { Index = i, SN = $"SN{i:D4}" };
                        }
                    }
                    break;
                default:
                    break;
            }
            body.BatchCount = body.ProductList.Length;
            return body;
        }
    }
}
