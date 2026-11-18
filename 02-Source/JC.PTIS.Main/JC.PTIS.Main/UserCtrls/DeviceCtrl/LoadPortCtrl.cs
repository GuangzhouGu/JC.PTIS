using JC.PTIS.LoadPort.Devices.EFEM;
using StdLib.PCBaseFrame.Extension.Args;
using StdLib.PCBaseFrame.Extension.Device;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.Map.Devices.EFEM
{
    public partial class LoadPortCtrl : UserControl
    {
        private class PortInfo
        {
            public E84Ports Port { get; set; }

            public Label Label { get; set; }

            public bool State { get; set; }

            public PortInfo(E84Ports e84Ports, Label label)
            {
                this.Port = e84Ports;
                Label = label;
            }
        }

        private class SlotItem : INotifyPropertyChanged
        {
            private string status;

            public string Status
            {
                get { return this.status; }
                set
                {
                    if (this.status != value)
                    {
                        this.status = value;
                        this.OnPropertyChanged("Status");
                    }
                }
            }

            public event PropertyChangedEventHandler PropertyChanged;
            private void OnPropertyChanged(string propertyName)
            {
                if (this.PropertyChanged != null)
                {
                    this.PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
                }
            }
        }

        private const int UpdateInterval = 1000;

        private LoadPortHelper loadPortHelper;
        private List<LoadPortDevice> loadPortDevices;
        private List<MethodInfo> methods;
        private List<Label> FOUPSTATE_Labels;
        private List<SlotItem> slotItems;
        private List<CheckBox> STATE_CheckBoxs;
        private List<PortInfo> e84Ports;
        private int debugMsgLines;

        public LoadPortCtrl()
        {
            InitializeComponent();

            this.InitializeForm();
        }

        private void InitializeForm()
        {
            LoadPortHelper.Instance.ShowDebugMessage += this.Instance_ShowDebugMessage;
            this.methods = typeof(LoadPortDevice).GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic).ToList();
            this.loadPortHelper = DeviceManager.Instance.Get<string>(nameof(LoadPortHelper)) as LoadPortHelper;
            this.loadPortDevices = new List<LoadPortDevice>();
            this.loadPortDevices.AddRange(this.loadPortHelper.List);
            comboBoxLoadPorts.DataSource = this.loadPortDevices;
            comboBoxLoadPorts.DisplayMember = "DeviceName";
            textBoxDebugMessage.ScrollBars = ScrollBars.Both;
            comboBoxFoupTypes.SelectedIndex = 0;

            // 料盒状态
            this.FOUPSTATE_Labels = new List<Label>();
            for (int i = 0; i < 9; i++)
            {
                string labelName = string.Format("labelIOS{0}", i);
                Label label = groupBox2.Controls.OfType<Label>().Where(v => v.Tag != null).FirstOrDefault(v => v.Name == labelName);
                this.FOUPSTATE_Labels.Add(label);
            }

            // 设备状态
            this.STATE_CheckBoxs = tabPage1.Controls.OfType<CheckBox>().ToList();

            // Mapping数据
            this.slotItems = new List<SlotItem>();
            for (int i = 0; i < 25; i++)
            {
                this.slotItems.Add(new SlotItem() { Status = "无Wafer" });
            }
            dataGridViewSlotItems.DataSource = this.slotItems;

            // E84Port
            this.e84Ports = new List<PortInfo>()
            {
                new PortInfo(E84Ports.OUTPUT_L_REQ, labelL_REQ),
                new PortInfo(E84Ports.OUTPUT_U_REQ, labelU_REQ),
                new PortInfo(E84Ports.OUTPUT_READY, labelREADY),
                new PortInfo(E84Ports.INPUT_VALID, labelVALID),
                new PortInfo(E84Ports.INPUT_CS_0, labelCS_0),
                new PortInfo(E84Ports.INPUT_CS_1, labelCS_1),
                new PortInfo(E84Ports.INPUT_TR_REQ, labelTR_REQ),
                new PortInfo(E84Ports.INPUT_BUSY, labelBUSY),
                new PortInfo(E84Ports.INPUT_COMPT, labelCOMPT)
            };

            Task.Factory.StartNew(this.UpdateThreadProc);
        }

        private void AsyncInvoke(Action action, Button button)
        {
            button.Enabled = false;
            string actionName = button.Text;

            Task.Factory.StartNew(() =>
            {
                try
                {
                    action();
                }
                catch (Exception ex)
                {
                    this.ShowMessage(MsgType.Error, "{0}异常, {1}", actionName, ex);
                }
                finally
                {
                    Invoke(new Action(() =>
                    {
                        button.Enabled = true;
                    }));
                }
            });
        }

        private void UpdateMapData(List<SlotStates> mapData)
        {
            for (int i = 0; i < mapData.Count; i++)
            {
                SlotItem slotItem = this.slotItems[i];

                switch (mapData[i])
                {
                    case SlotStates.NoWafter:
                        {
                            slotItem.Status = "无Wafer";
                            break;
                        }

                    case SlotStates.HasWafter:
                        {
                            slotItem.Status = "有Wafer";
                            break;
                        }

                    case SlotStates.Overlap:
                        {
                            slotItem.Status = "叠片";
                            break;
                        }

                    case SlotStates.Tilted:
                        {
                            slotItem.Status = "斜插";
                            break;
                        }

                    default:
                        {
                            slotItem.Status = string.Format("未知状态:{0}", mapData[i]);
                            break;
                        }
                }
            }
        }

        private void ShowMessage(MsgType msgType, string format, params object[] param)
        {
            string message = string.Empty;

            if (param == null || param.Length == 0)
            {
                message = format;
            }
            else
            {
                string log = string.Format(format, param);
                message = log;
            }

            Invoke(new Action(() =>
            {
                if (debugMsgLines == 1000)
                {
                    textBoxDebugMessage.Clear();
                }

                textBoxDebugMessage.AppendText(message + "\r\n");
                textBoxDebugMessage.SelectionStart = textBoxDebugMessage.TextLength;
                textBoxDebugMessage.ScrollToCaret();
                debugMsgLines++;
            }));
        }

        private void Instance_ShowDebugMessage(MessageArgs message)
        {
            this.ShowMessage(message.MsgType, message.Message);
        }

        private void buttonControl_Click(object sender, EventArgs e)
        {
            LoadPortDevice loadPortDevice = comboBoxLoadPorts.SelectedItem as LoadPortDevice;
            if (loadPortDevice == null)
            {
                MessageBox.Show("请选择要控制的LoadPort");
                return;
            }

            Button button = sender as Button;
            string methodName = button.Tag.ToString();
            MethodInfo methodInfo = this.methods.FirstOrDefault(v => v.Name == methodName);
            if (methodInfo == null)
            {
                MessageBox.Show(string.Format("控制失败, 没有找到方法, {0}", methodName));
                return;
            }

            button.Enabled = false;

            Task.Factory.StartNew(() =>
            {
                try
                {
                    LPCodes lpCodes = (LPCodes)methodInfo.Invoke(loadPortDevice, null);
                    if (lpCodes != LPCodes.SUCCESS)
                    {
                        this.ShowMessage(MsgType.Error, "执行控制命令失败, {0}, {1}, {2}", button.Text, methodName, LpUtils.GetErrorMessage(lpCodes));
                        return;
                    }

                    this.ShowMessage(MsgType.Normal, "{0}执行成功", button.Text);
                }
                catch (Exception ex)
                {
                    this.ShowMessage(MsgType.Error, "执行控制命令异常, {0}, {1}, {2}", button.Text, methodName, ex);
                }
                finally
                {
                    Invoke(new Action(() =>
                    {
                        button.Enabled = true;
                    }));
                }
            });
        }

        private void buttonSTATE_Click(object sender, EventArgs e)
        {
            this.STATE_CheckBoxs.ForEach(v => v.Checked = false);

            LoadPortDevice loadPortDevice = comboBoxLoadPorts.SelectedItem as LoadPortDevice;
            if (loadPortDevice == null)
            {
                MessageBox.Show("请选择要控制的LoadPort");
                return;
            }

            LPCodes lpCodes = loadPortDevice.STATE(out List<int> states);
            if (lpCodes != LPCodes.SUCCESS)
            {
                this.ShowMessage(MsgType.Error, "获取设备状态失败, {0}", LpUtils.GetErrorMessage(lpCodes));
                return;
            }

            for (int i = 0; i < 15; i++)
            {
                string checkBoxName = string.Format("radioButton{0}_{1}", i, states[i]);
                CheckBox checkBox = this.STATE_CheckBoxs.FirstOrDefault(v => v.Name == checkBoxName);
                checkBox.Checked = true;
            }
        }

        private void buttonFOUPSTATE_Click(object sender, EventArgs e)
        {
            this.FOUPSTATE_Labels.ForEach(v => v.BackColor = Color.Gray);

            LoadPortDevice loadPortDevice = comboBoxLoadPorts.SelectedItem as LoadPortDevice;
            if (loadPortDevice == null)
            {
                MessageBox.Show("请选择要控制的LoadPort");
                return;
            }

            LPCodes lpCodes = loadPortDevice.FOUPSTATE(out List<bool> states);
            if (lpCodes != LPCodes.SUCCESS)
            {
                this.ShowMessage(MsgType.Error, "获取料盒状态失败, {0}", LpUtils.GetErrorMessage(lpCodes));
                return;
            }

            for (int i = 0; i < states.Count; i++)
            {
                Label label = this.FOUPSTATE_Labels[i];
                label.BackColor = states[i] ? Color.Green : Color.Gray;
            }
        }

        private void buttonLEDST_Click(object sender, EventArgs e)
        {
            LoadPortDevice loadPortDevice = comboBoxLoadPorts.SelectedItem as LoadPortDevice;
            if (loadPortDevice == null)
            {
                MessageBox.Show("请选择要控制的LoadPort");
                return;
            }

            LPCodes lpCodes = loadPortDevice.LEDST();
            if (lpCodes != LPCodes.SUCCESS)
            {
                this.ShowMessage(MsgType.Error, "获取指示灯状态失败, {0}", LpUtils.GetErrorMessage(lpCodes));
                return;
            }
        }

        private void buttonMAPDT_Click(object sender, EventArgs e)
        {
            LoadPortDevice loadPortDevice = comboBoxLoadPorts.SelectedItem as LoadPortDevice;
            if (loadPortDevice == null)
            {
                MessageBox.Show("请选择要控制的LoadPort");
                return;
            }

            LPCodes lpCodes = loadPortDevice.MAPDT(out List<SlotStates> mapData);
            if (lpCodes != LPCodes.SUCCESS)
            {
                return;
            }

            this.UpdateMapData(mapData);
        }

        private void buttonMAPRD_Click(object sender, EventArgs e)
        {
            LoadPortDevice loadPortDevice = comboBoxLoadPorts.SelectedItem as LoadPortDevice;
            if (loadPortDevice == null)
            {
                MessageBox.Show("请选择要控制的LoadPort");
                return;
            }

            LPCodes lpCodes = loadPortDevice.MAPRD(out List<SlotStates> mapData);
            if (lpCodes != LPCodes.SUCCESS)
            {
                return;
            }

            this.UpdateMapData(mapData);
        }

        private void buttonWFCNT_Click(object sender, EventArgs e)
        {
            LoadPortDevice loadPortDevice = comboBoxLoadPorts.SelectedItem as LoadPortDevice;
            if (loadPortDevice == null)
            {
                MessageBox.Show("请选择要控制的LoadPort");
                return;
            }

            LPCodes lpCodes = loadPortDevice.WFCNT(out int cnt);
            if (lpCodes != LPCodes.SUCCESS)
            {
                this.ShowMessage(MsgType.Error, "获取正常晶圆数量失败, {0}", LpUtils.GetErrorMessage(lpCodes));
                return;
            }

        }

        private void buttonVERSN_Click(object sender, EventArgs e)
        {
            LoadPortDevice loadPortDevice = comboBoxLoadPorts.SelectedItem as LoadPortDevice;
            if (loadPortDevice == null)
            {
                MessageBox.Show("请选择要控制的LoadPort");
                return;
            }

            LPCodes lpCodes = loadPortDevice.VERSN(out string version);
            if (lpCodes != LPCodes.SUCCESS)
            {
                this.ShowMessage(MsgType.Error, "获取版本信息失败, {0}", LpUtils.GetErrorMessage(lpCodes));
                return;
            }
        }

        private void buttonRESET_Click(object sender, EventArgs e)
        {
            LoadPortDevice loadPortDevice = comboBoxLoadPorts.SelectedItem as LoadPortDevice;
            if (loadPortDevice == null)
            {
                MessageBox.Show("请选择要控制的LoadPort");
                return;
            }

            LPCodes lpCodes = loadPortDevice.RESET();
            if (lpCodes != LPCodes.SUCCESS)
            {
                this.ShowMessage(MsgType.Error, "清除错误失败, {0}", LpUtils.GetErrorMessage(lpCodes));
                return;
            }
        }

        private void buttonONMGV_Click(object sender, EventArgs e)
        {
            LoadPortDevice loadPortDevice = comboBoxLoadPorts.SelectedItem as LoadPortDevice;
            if (loadPortDevice == null)
            {
                MessageBox.Show("请选择要控制的LoadPort");
                return;
            }

            LPCodes lpCodes = loadPortDevice.ONMGV();
            if (lpCodes != LPCodes.SUCCESS)
            {
                this.ShowMessage(MsgType.Error, "自动模式失败, {0}", LpUtils.GetErrorMessage(lpCodes));
                return;
            }
        }

        private void buttonMENTE_Click(object sender, EventArgs e)
        {
            LoadPortDevice loadPortDevice = comboBoxLoadPorts.SelectedItem as LoadPortDevice;
            if (loadPortDevice == null)
            {
                MessageBox.Show("请选择要控制的LoadPort");
                return;
            }

            LPCodes lpCodes = loadPortDevice.MENTE();
            if (lpCodes != LPCodes.SUCCESS)
            {
                this.ShowMessage(MsgType.Error, "手动模式失败, {0}", LpUtils.GetErrorMessage(lpCodes));
                return;
            }
        }

        private void buttonUNLOCK_Click(object sender, EventArgs e)
        {
            LoadPortDevice loadPortDevice = comboBoxLoadPorts.SelectedItem as LoadPortDevice;
            if (loadPortDevice == null)
            {
                MessageBox.Show("请选择要控制的LoadPort");
                return;
            }

            LPCodes lpCodes = loadPortDevice.UNLOCK();
            if (lpCodes != LPCodes.SUCCESS)
            {
                this.ShowMessage(MsgType.Error, "解除互锁失败, {0}", LpUtils.GetErrorMessage(lpCodes));
                return;
            }
        }

        private void comboBoxLoadPorts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void UpdateThreadProc()
        {
            while (true)
            {
                if (!this.IsHandleCreated)
                {
                    Thread.Sleep(UpdateInterval);
                    continue;
                }

                if (!this.Visible || !checkBoxAutoUpdate.Checked)
                {
                    Thread.Sleep(UpdateInterval);
                    continue;
                }

                try
                {
                    LoadPortDevice lpDevice = null;

                    Invoke(new Action(() =>
                    {
                        lpDevice = comboBoxLoadPorts.SelectedItem as LoadPortDevice;
                    }));

                    if (lpDevice == null)
                    {
                        continue;
                    }

                    #region 更新料盒状态

                    LPCodes lpCodes = lpDevice.FOUPSTATE(out List<bool> states);
                    if (lpCodes == LPCodes.SUCCESS)
                    {
                        Invoke(new Action(() =>
                        {
                            for (int i = 0; i < states.Count; i++)
                            {
                                Label label = this.FOUPSTATE_Labels[i];
                                label.BackColor = states[i] ? Color.Green : Color.Gray;
                            }
                        }));
                    }

                    #endregion

                    #region 更新设备状态

                    lpCodes = lpDevice.STATE(out List<int> STATES);
                    if (lpCodes == LPCodes.SUCCESS)
                    {
                        Invoke(new Action(() =>
                        {
                            this.STATE_CheckBoxs.ForEach(v => v.Checked = false);
                            for (int i = 0; i < 15; i++)
                            {
                                string checkBoxName = string.Format("radioButton{0}_{1}", i, STATES[i]);
                                CheckBox checkBox = this.STATE_CheckBoxs.FirstOrDefault(v => v.Name == checkBoxName);
                                checkBox.Checked = true;
                            }
                        }));
                    }

                    #endregion

                    #region 更新E84状态

                    bool success = true;
                    foreach (PortInfo portInfo in this.e84Ports)
                    {
                        if (!E84IOReader.ReadPortState(lpDevice.Info.Num, portInfo.Port, out bool state))
                        {
                            success = false;
                            break;
                        }

                        portInfo.State = state;
                    }

                    if (success)
                    {
                        Invoke(new Action(() =>
                        {
                            foreach (PortInfo portInfo in this.e84Ports)
                            {
                                Label label = portInfo.Label;
                                bool state = portInfo.State;

                                if (state)
                                {
                                    if (label.BackColor != Color.Green)
                                    {
                                        label.BackColor = Color.Green;
                                    }
                                }
                                else
                                {
                                    if (label.BackColor != Color.Gray)
                                    {
                                        label.BackColor = Color.Gray;
                                    }
                                }
                            }
                        }));
                    }

                    #endregion
                }
                catch (Exception ex)
                {
                    this.ShowMessage(MsgType.Error, "刷新LoadPort状态异常, {0}", ex);
                }
                finally
                {
                    Thread.Sleep(UpdateInterval);
                }
            }
        }

        private void buttonClearDebugMessage_Click(object sender, EventArgs e)
        {
            this.debugMsgLines = 0;
            textBoxDebugMessage.Clear();
        }

        private void buttonLpLoad_Click(object sender, EventArgs e)
        {
            LoadPortDevice lpDevice = comboBoxLoadPorts.SelectedItem as LoadPortDevice;
            if (lpDevice == null)
            {
                this.ShowMessage(MsgType.Error, "请选择要操作的LoadPort");
                return;
            }

            this.AsyncInvoke(() =>
            {
                LPCodes lpCode = lpDevice.LpLoadAsync();
                if (lpCode != LPCodes.SUCCESS)
                {
                    this.ShowMessage(MsgType.Error, "StartLpLoad失败, {0}", LpUtils.GetErrorMessage(lpCode));
                }
            }, buttonLpLoad);
        }

        private void buttonLpUnload_Click(object sender, EventArgs e)
        {
            LoadPortDevice lpDevice = comboBoxLoadPorts.SelectedItem as LoadPortDevice;
            if (lpDevice == null)
            {
                this.ShowMessage(MsgType.Error, "请选择要操作的LoadPort");
                return;
            }

            this.AsyncInvoke(() =>
            {
                LPCodes lpCode = lpDevice.LpUnloadAsync();
                if (lpCode != LPCodes.SUCCESS)
                {
                    this.ShowMessage(MsgType.Error, "StartLpUnload失败, {0}", LpUtils.GetErrorMessage(lpCode));
                }
            }, buttonLpUnload);
        }

        private void buttonOHTLoad_Click(object sender, EventArgs e)
        {
            LoadPortDevice lpDevice = comboBoxLoadPorts.SelectedItem as LoadPortDevice;
            if (lpDevice == null)
            {
                this.ShowMessage(MsgType.Error, "请选择要操作的LoadPort");
                return;
            }

            OHTLoadOptions loadOptions = new OHTLoadOptions()
            {
                FoupType = (FoupTypes)comboBoxFoupTypes.SelectedIndex
            };

            lpDevice.OHTLoadAsync(loadOptions);
        }

        private void buttonOHTUnload_Click(object sender, EventArgs e)
        {
            LoadPortDevice lpDevice = comboBoxLoadPorts.SelectedItem as LoadPortDevice;
            if (lpDevice == null)
            {
                this.ShowMessage(MsgType.Error, "请选择要操作的LoadPort");
                return;
            }

            lpDevice.OHTUnloadAsync();
        }

        private void buttonFOUPTY_Click(object sender, EventArgs e)
        {
            int foupType = comboBoxFoupTypes.SelectedIndex;
            LoadPortDevice lpDevice = comboBoxLoadPorts.SelectedItem as LoadPortDevice;
            if (lpDevice == null)
            {
                this.ShowMessage(MsgType.Error, "请选择要操作的LoadPort");
                return;
            }

            this.AsyncInvoke(() =>
            {
                LPCodes lpCode = lpDevice.SetFoupType((FoupTypes)foupType);
                if (lpCode != LPCodes.SUCCESS)
                {
                    if (lpCode != LPCodes.SUCCESS)
                    {
                        this.ShowMessage(MsgType.Error, "切换料盒类型失败, {0}", LpUtils.GetErrorMessage(lpCode));
                        return;
                    }
                }
            }, buttonFOUPTY);
        }

        private void buttonHasFoup_Click(object sender, EventArgs e)
        {
            int foupType = comboBoxFoupTypes.SelectedIndex;
            LoadPortDevice lpDevice = comboBoxLoadPorts.SelectedItem as LoadPortDevice;
            if (lpDevice == null)
            {
                this.ShowMessage(MsgType.Error, "请选择要操作的LoadPort");
                return;
            }

            LPCodes lpCode = lpDevice.HasFoup(out bool hasFoup);
            if (lpCode != LPCodes.SUCCESS)
            {
                this.ShowMessage(MsgType.Error, "读取是否有料盒失败, {0}", LpUtils.GetErrorMessage(lpCode));
                return;
            }

            checkBoxHasFoup.Checked = hasFoup;
        }
    }
}
