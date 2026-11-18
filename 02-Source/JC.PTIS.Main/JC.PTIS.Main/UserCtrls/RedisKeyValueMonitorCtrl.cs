using JC.PTIS.Main.Entities;
using StdLib.HTFile;
using StdLib.PCBaseFrame.Extension.Entities;
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

namespace JC.PTIS.Main.UserCtrls
{
    public partial class RedisKeyValueMonitorCtrl : UserControl
    {
        public RedisKeyValueMonitorCtrl()
        {
            InitializeComponent();
            listBox1.DoubleBuffered();
            htTreeView1.DoubleBuffered();
        }
        HTIniHelper IniFile = new HTIniHelper(AppDomain.CurrentDomain.BaseDirectory + "Config.INI");
        string ConnectStr = string.Empty;
        private void RedisKeyValueMonitorCtrl_Load(object sender, EventArgs e)
        {
            if (ClassHelper.IsDesingMode()==false)
            {
                ConnectStr = IniFile.Read("Redis", "ConnectionString", "");
                if (string.IsNullOrEmpty(ConnectStr)==false)
                {
                    var strArr=ConnectStr.Split(',');
                    if (strArr.Length==3)
                    {
                        var ipPort = strArr[0].Split(':');
                        tbxAddress.Text= ipPort[0];
                        tbxPort.Text = ipPort[1];
                        tbxPassword.Text= strArr[1];
                    }
                }
                if (RedisHelper.Instance.IsConnected)
                {
                    ShowAllKeys();
                }
            }
        }
    
        private void btnConnect_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(ConnectStr)==false)
            {
                RedisHelper.Instance.Init(ConnectStr);
            }
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            RedisHelper.Instance.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                var ip = tbxAddress.Text;
                var port = tbxPort.Text;
                var passWord = tbxPassword.Text;
                ConnectStr = $"{ip}:{port},password={passWord},defaultDatabase=0";
                IniFile.Write("Redis", "ConnectionString", ConnectStr);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void btnGetAllKey_Click(object sender, EventArgs e)
        {
            ShowAllKeys();
        }
        private void ShowAllKeys()
        {
            var keyList = RedisHelper.Instance.GetAllKeys();
            if (keyList != null && keyList.Count > 0)
            {
                htTreeView1.SuspendLayout();
                htTreeView1.Nodes.Clear();
                for (int i = 0; i < keyList.Count; i++)
                {
                    TreeNode tn1 = htTreeView1.Nodes.Add(keyList[i]);
                    tn1.Name = keyList[i];
                }
            }
        }

        private async void htTreeView1_AfterSelect(object sender, TreeViewEventArgs e)
        {
            htTreeView1.Enabled=false;
            await ShowMeeageAsync(e.Node.Name);
            htTreeView1.Enabled = true;
        }

        private async Task ShowMeeageAsync(string nodeName)
        {
            listBox1.Items.Clear();
            var str = await Task.Run(() => RedisHelper.Instance.StringGet(nodeName));
            if (string.IsNullOrEmpty(str))
            {
                return;
            }
            string[] items = str.Split(new[] { ',', '\\' }, StringSplitOptions.None);
            listBox1.BeginUpdate();
            try
            {
                const int batchSize = 500;
                for (int i = 0; i < items.Length; i += batchSize)
                {
                    int count = Math.Min(batchSize, items.Length - i);
                    string[] batch = new string[count];
                    Array.Copy(items, i, batch, 0, count);
                    listBox1.Items.AddRange(batch);
                    // 分批更新，避免一次性添加大量数据导致界面长时间无响应
                    listBox1.EndUpdate();
                    await Task.Delay(1);
                    listBox1.BeginUpdate();
                }
            }
            finally
            {
                listBox1.EndUpdate();
            }
        }

        private void btnDeleteKey_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null,"是否删除选中的Key"))
            {
                RedisHelper.Instance.KeyDelete(htTreeView1.SelectedNode.Name);
                htTreeView1.Nodes.Remove(htTreeView1.SelectedNode);
            }
          
        }

        private void btnDeleteAllKeys_Click(object sender, EventArgs e)
        {
            if (UIMessageDialog.ShowAskDialog(null, "是否删除所有Key"))
            {
                ProcInfoHelper.Instance.DeleteAllKey();
                htTreeView1.Nodes.Clear();
            }
        }
    }
}
