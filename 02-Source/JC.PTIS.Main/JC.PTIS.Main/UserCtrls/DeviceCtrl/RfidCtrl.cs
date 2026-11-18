using JC.PTIS.LoadPort.Devices.EFEM;
using StdLib.PCBaseFrame.Extension.Args;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.Map.Devices.RFID
{
    public partial class RfidCtrl : UserControl
    {
        private int debugMsgLines;
        private StdLib.LogHelper.Logger logger;

        public RfidCtrl()
        {
            InitializeComponent();

            this.InitializeForm();
        }

        private void InitializeForm()
        {
            this.logger = new StdLib.LogHelper.Logger("Log", string.Empty, nameof(RfidCtrl), false);

            RfidDeviceHelper.Instance.ShowDebugMessage += this.Instance_ShowDebugMessage; ;
            comboBoxRFIDs.DataSource = RfidDeviceHelper.Instance.List;
            comboBoxRFIDs.DisplayMember = "DeviceName";

            comboBoxReadPages.DataSource = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
            comboBoxReadPages.SelectedIndex = 0;
            comboBoxWritePages.DataSource = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15, 16, 17 };
            comboBoxWritePages.SelectedIndex = 0;
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

        private void DisplayData(List<byte[]> pageDatas, int startPageNum = 1)
        {
            int pageNum = startPageNum;
            StringBuilder builder = new StringBuilder();
            foreach (byte[] pageData in pageDatas)
            {
                if (checkBoxASCIIEncoding.Checked)
                {
                    string str = Encoding.ASCII.GetString(pageData);
                    builder.AppendFormat("第{0}页:{1}\r\n", pageNum++, str);
                }
                else
                {
                    string hex = string.Join(" ", pageData.Select(v => v.ToString("X2")));
                    builder.AppendFormat("第{0}页:{1}\r\n", pageNum++, hex);
                }
            }

            labelReadResult.Text = builder.ToString();
        }

        private void Instance_ShowDebugMessage(MessageArgs message)
        {
            this.ShowMessage(message.MsgType, message.Message);
        }

        private void buttonReadAllPages_Click(object sender, EventArgs e)
        {
            RfidDevice device = comboBoxRFIDs.SelectedItem as RfidDevice;
            if (device == null)
            {
                MessageBox.Show("请选择要控制的RFID");
                return;
            }

            buttonReadAllPages.Enabled = false;

            Task.Factory.StartNew(() =>
            {
                try
                {
                    List<byte[]> pageDatas = device.ReadAllPages();
                    if (pageDatas == null)
                    {
                        this.ShowMessage(MsgType.Error, "读取RFID失败");
                        return;
                    }

                    base.Invoke(new Action(() =>
                    {
                        this.DisplayData(pageDatas);
                    }));
                }
                catch (Exception ex)
                {
                    this.ShowMessage(MsgType.Error, "读取RFID异常, {0}", ex);
                }
                finally
                {
                    base.Invoke(new Action(() =>
                    {
                        buttonReadAllPages.Enabled = true;
                    }));
                }
            });
        }

        private void buttonReadCarrierID_Click(object sender, EventArgs e)
        {
            RfidDevice device = comboBoxRFIDs.SelectedItem as RfidDevice;
            if (device == null)
            {
                MessageBox.Show("请选择要控制的RFID");
                return;
            }

            buttonReadCarrierID.Enabled = false;

            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (!device.ReadCarrierId(out string carrierId))
                    {
                        this.ShowMessage(MsgType.Error, "读取CarrierId失败");
                        return;
                    }

                    this.Invoke(new Action(() =>
                    {
                        labelReadResult.Text = carrierId;
                    }));
                }
                catch (Exception ex)
                {
                    this.ShowMessage(MsgType.Error, "读取CarrierID异常, {0}", ex);
                }
                finally
                {
                    this.Invoke(new Action(() =>
                    {
                        buttonReadCarrierID.Enabled = true;
                    }));
                }
            });
        }

        private void buttonReadPage_Click(object sender, EventArgs e)
        {
            RfidDevice device = comboBoxRFIDs.SelectedItem as RfidDevice;
            if (device == null)
            {
                MessageBox.Show("请选择要控制的RFID");
                return;
            }
            int pageIndex = Convert.ToInt32(comboBoxReadPages.SelectedItem.ToString()) - 1;
            buttonReadPage.Enabled = false;

            Task.Factory.StartNew(() =>
            {
                try
                {
                    byte[] pageData = device.ReadPage(pageIndex);
                    if (pageData == null)
                    {
                        this.ShowMessage(MsgType.Error, "读取RFID失败");
                        return;
                    }

                    this.Invoke(new Action(() =>
                    {
                        this.DisplayData(new List<byte[]>() { pageData }, pageIndex + 1);
                    }));
                }
                catch (Exception ex)
                {
                    this.ShowMessage(MsgType.Error, "读取RFID异常, {0}", ex);
                }
                finally
                {
                    this.Invoke(new Action(() =>
                    {
                        buttonReadPage.Enabled = true;
                    }));
                }
            });
        }

        private void buttonWrite_Click(object sender, EventArgs e)
        {
            RfidDevice device = comboBoxRFIDs.SelectedItem as RfidDevice;
            if (device == null)
            {
                MessageBox.Show("请选择要控制的RFID");
                return;
            }

            int pageIndex = Convert.ToInt32(comboBoxReadPages.SelectedItem.ToString()) - 1;
            string text = textBoxWriteData.Text;
            if (string.IsNullOrEmpty(text))
            {
                MessageBox.Show("请输入要写入的数据");
                return;
            }

            byte[] bytesToWrite = null;

            if (checkBoxWriteHexFormat.Checked)
            {
                // 解析为16进制字符串
                string[] numbers = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                if (numbers == null || numbers.Length == 0)
                {
                    MessageBox.Show("十六进制格式不正确\r\n请重新输入要写入的数据");
                    return;
                }

                bytesToWrite = new byte[numbers.Length];

                for (int i = 0; i < numbers.Length; i++)
                {
                    string number = numbers[i];

                    if (!byte.TryParse(number, System.Globalization.NumberStyles.HexNumber, null, out byte s))
                    {
                        MessageBox.Show(string.Format("第{0}个十六进制数不正确\r\n请重新输入要写入的数据", i + 1));
                        return;
                    }

                    bytesToWrite[i] = s;
                }
            }
            else
            {
                // 解析为ASCII码
                bytesToWrite = Encoding.ASCII.GetBytes(text);
            }

            if (bytesToWrite.Length > 16)
            {
                MessageBox.Show("每页最多写入16字节数据\r\n请重新输入要写入的数据");
                return;
            }

            buttonWrite.Enabled = false;

            Task.Factory.StartNew(() =>
            {
                try
                {
                    if (!device.WritePage(pageIndex, bytesToWrite))
                    {
                        this.ShowMessage(MsgType.Error, "写入失败");
                    }
                    else
                    {
                        this.ShowMessage(MsgType.Normal, "写入成功");
                    }
                }
                catch (Exception ex)
                {
                    this.ShowMessage(MsgType.Error, "写入RFID异常, {0}", ex);
                }
                finally
                {
                    this.Invoke(new Action(() =>
                    {
                        buttonWrite.Enabled = true;
                    }));
                }
            });
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBoxDebugMessage.Clear();
            this.debugMsgLines = 0;
        }
    }
}






