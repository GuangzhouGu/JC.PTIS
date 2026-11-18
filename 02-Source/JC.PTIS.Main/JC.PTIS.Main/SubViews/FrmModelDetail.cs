using JC.PTIS.Main.Models;
using StdLib.PCBaseFrame.Extension.Views;
using StdLib.WindowsControls;
using StdLib.WindowsForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JC.PTIS.Main.SubViews
{
    public partial class FrmModelDetail : FormPopBase
    {
        public FrmModelDetail()
        {
            InitializeComponent();
        }
        public object Model = null;
        PropertyInfo[] Properties = null;
        public bool IsUpdated { get; set; }
        public void ShowInfo<T>(T model)
        {
            try
            {
           
                if (model!=null)
                {
                    this.Title = model.GetType().Name;
                    Model = model;
                    tlpInfo.Controls.Clear();
                    tlpInfo.RowStyles.Clear();
                    Properties = model.GetType().GetProperties();
                    if (Properties.Length > 0)
                    {
                        tlpInfo.RowCount = Properties.Length;
                        for (int i = 0; i < Properties.Length; i++)
                        {
                            var type = Properties[i].PropertyType;
                            var typeName = Properties[i].PropertyType.Name;
                            tlpInfo.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
                            for (int j = 0; j < 2; j++)
                            {
                                Label ctrl = new Label();
                                ctrl.Font = new Font("微软雅黑", 10);
                                ctrl.Dock = DockStyle.Fill;
                                ctrl.Margin = new Padding(2);
                                ctrl.ForeColor = Color.Black;
                                ctrl.BorderStyle = BorderStyle.FixedSingle;
                                if (j == 0)
                                {
                                    ctrl.BackColor = Color.LightCyan;
                                    ctrl.TextAlign = ContentAlignment.MiddleRight;
                                    ctrl.Text = Properties[i].Name;
                                    tlpInfo.Controls.Add(ctrl, j, i);
                                }
                                else if (j == 1)
                                {
                                    if (type.IsEnum||
                                        type.Name == typeof(bool).Name)
                                    {
                                        HTComboBox hTComboBox = new HTComboBox();
                                        hTComboBox.DropDownStyle = HTDropDownStyle.DropDownList;
                                        hTComboBox.Font = new Font("微软雅黑", 12);
                                        hTComboBox.Dock = DockStyle.Fill;
                                        hTComboBox.Margin = new Padding(2);
                                        hTComboBox.ForeColor = Color.Black;
                                        hTComboBox.TextAlignment = ContentAlignment.MiddleCenter;
                                        //获取枚举的所有值
                                        if (type.Name == typeof(bool).Name)
                                        {
                                            var enumValues = new List<bool> { true, false };
                                            foreach (var enumValue in enumValues)
                                            {
                                                hTComboBox.Items.Add(enumValue);
                                            }
                                        }
                                        else
                                        {
                                            var enumValues = Enum.GetValues(type);
                                            foreach (var enumValue in enumValues)
                                            {
                                                hTComboBox.Items.Add(enumValue);
                                            }
                                        }
                                        hTComboBox.SelectedItem = Properties[i]?.GetValue(model);
                                        tlpInfo.Controls.Add(hTComboBox, j, i);
                                    }
                                    else if (type.IsArray == false)
                                    {
                                        ctrl.BackColor = Color.White;
                                        ctrl.TextAlign = ContentAlignment.MiddleCenter;
                                        ctrl.Text = Properties[i]?.GetValue(model)?.ToString();
                                        tlpInfo.Controls.Add(ctrl, j, i);
                                    }
                                    else
                                    {
                                        ctrl.BorderStyle= BorderStyle.FixedSingle;
                                        ctrl.Text="双击显示Map";
                                        ctrl.TextAlign = ContentAlignment.MiddleCenter;
                                        ctrl.MouseDoubleClick += Ctrl_MouseDoubleClick;
                                        tlpInfo.Controls.Add(ctrl, j, i);
                                    }
                                }
                            }
                        }
                    }
                }
            
            }
            catch (Exception ex)
            {
                UIMessageDialog.ShowErrorDialog(null, ex.ToString());
            }
        }

        private void Ctrl_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                FrmShowTrayInfo frmModelDetail = new FrmShowTrayInfo();
                frmModelDetail.ShowInfo((TrayModel)Model);
                frmModelDetail.ShowDialog();
            }
            catch (Exception ex)
            {
                UIMessageDialog.ShowErrorDialog(null, ex.ToString());
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            //更新Model的属性值
            try
            {
                for (int i = 0; i < Properties.Length; i++)
                {
                    var type = Properties[i].PropertyType;
                    if (type.IsEnum || type.Name == typeof(bool).Name)
                    {
                        var ctrl = tlpInfo.GetControlFromPosition(1, i) as HTComboBox;
                        if (ctrl != null)
                        {
                            Properties[i].SetValue(Model, ctrl.SelectedItem);
                        }
                    }
                }
                IsUpdated = true;
                this.Close();
            }
            catch (Exception ex)
            {
                UIMessageDialog.ShowErrorDialog(null, ex.ToString());
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            Model = null;
            IsUpdated = true;
            this.Close();
        }
    }
}
