using Device.ControlUint.Data.Entitys;
using Device.controlUnit.Transmission;
using Device.ControlUnit.Data;
using Device.ControlUnit.Data.Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.WebRequestMethods;

namespace DeviceControlUnit.Project
{
    public partial class Form1 : Form
    {
        private DateTime nowTime = new DateTime();

        private List<TextBox> textBoxes = new List<TextBox>();

        private List<CheckBox> checkBoxes = new List<CheckBox>();

        private readonly DeviceContextDb deviceContext = new DeviceContextDb();

        private readonly List<DeviceMasterSlave> deviceMasters = new List<DeviceMasterSlave>();

        public Form1()
        {
            InitializeComponent();
            SetChartInit();
            nowTime = DateTime.Now;
            InitTableSelect();
        }

        private void radioHotTmp_CheckedChanged(object sender, EventArgs e)
        {
            this.chartTmp.Series["热水温度"].Enabled = radioHotTmp.Checked ? true : false;
            this.chartTmp.Series["冷水温度"].Enabled = radioHotTmp.Checked ? false : true;
            this.chartTmp.Series["混水温度"].Enabled = radioHotTmp.Checked ? false : true;
        }

        private void radioColdTmp_CheckedChanged(object sender, EventArgs e)
        {
            this.chartTmp.Series["热水温度"].Enabled = radioColdTmp.Checked ? false : true;
            this.chartTmp.Series["冷水温度"].Enabled = radioColdTmp.Checked ? true : false;
            this.chartTmp.Series["混水温度"].Enabled = radioColdTmp.Checked ? false : true;
        }

        private void radioMixTmp_CheckedChanged(object sender, EventArgs e)
        {
            this.chartTmp.Series["热水温度"].Enabled = radioMixTmp.Checked ? false : true;
            this.chartTmp.Series["冷水温度"].Enabled = radioMixTmp.Checked ? false : true;
            this.chartTmp.Series["混水温度"].Enabled = radioMixTmp.Checked ? true : false;
        }

        private void radioHotPress_CheckedChanged(object sender, EventArgs e)
        {
            this.chartPress.Series["热水压力"].Enabled = radioHotPress.Checked ? true : false;
            this.chartPress.Series["冷水压力"].Enabled = radioHotPress.Checked ? false : true;
            this.chartPress.Series["混水压力"].Enabled = radioHotPress.Checked ? false : true;
        }

        private void radioColdPress_CheckedChanged(object sender, EventArgs e)
        {
            this.chartPress.Series["热水压力"].Enabled = radioColdPress.Checked ? false : true;
            this.chartPress.Series["冷水压力"].Enabled = radioColdPress.Checked ? true : false;
            this.chartPress.Series["混水压力"].Enabled = radioColdPress.Checked ? false : true;
        }

        private void radioMixPress_CheckedChanged(object sender, EventArgs e)
        {
            this.chartPress.Series["热水压力"].Enabled = radioMixPress.Checked ? false : true;
            this.chartPress.Series["冷水压力"].Enabled = radioMixPress.Checked ? false : true;
            this.chartPress.Series["混水压力"].Enabled = radioMixPress.Checked ? true : false;
        }

        private void radioHotFlow_CheckedChanged(object sender, EventArgs e)
        {
            this.chartFlow.Series["热水流量"].Enabled = radioHotFlow.Checked ? true : false;
            this.chartFlow.Series["冷水流量"].Enabled = radioHotFlow.Checked ? false : true;
            this.chartFlow.Series["混水流量"].Enabled = radioHotFlow.Checked ? false : true;
        }

        private void radioColdFlow_CheckedChanged(object sender, EventArgs e)
        {
            this.chartFlow.Series["热水流量"].Enabled = radioColdFlow.Checked ? false : true;
            this.chartFlow.Series["冷水流量"].Enabled = radioColdFlow.Checked ? true : false;
            this.chartFlow.Series["混水流量"].Enabled = radioColdFlow.Checked ? false : true;
        }

        private void radioMixFlow_CheckedChanged(object sender, EventArgs e)
        {
            this.chartFlow.Series["热水流量"].Enabled = radioMixFlow.Checked ? false : true;
            this.chartFlow.Series["冷水流量"].Enabled = radioMixFlow.Checked ? false : true;
            this.chartFlow.Series["混水流量"].Enabled = radioMixFlow.Checked ? true : false;
        }

        private void SetChartInit()
        {
            // 设置 X 轴为时间
            chartTmp.ChartAreas[0].AxisX.Title = "时间";
            chartTmp.ChartAreas[0].AxisX.Interval = 1; // 设置显示间隔为 1 秒
            chartTmp.ChartAreas[0].AxisX.Minimum = 0;
            chartTmp.ChartAreas[0].AxisX.Maximum = 20;
            chartTmp.ChartAreas[0].AxisY.Minimum = 0;
            chartTmp.ChartAreas[0].AxisY.Maximum = 100;
            // 设置 Y 轴为温度
            chartTmp.ChartAreas[0].AxisY.Title = "温度 (°C)";


            chartPress.ChartAreas[0].AxisX.Title = "时间";
            chartPress.ChartAreas[0].AxisX.Interval = 1; // 设置显示间隔为 1 秒
            chartPress.ChartAreas[0].AxisX.Minimum = 0;
            chartPress.ChartAreas[0].AxisX.Maximum = 20;
            chartPress.ChartAreas[0].AxisY.Minimum = 0;
            chartPress.ChartAreas[0].AxisY.Maximum = 100;
            // 设置 Y 轴为温度
            chartPress.ChartAreas[0].AxisY.Title = "压力 (bar)";



            chartFlow.ChartAreas[0].AxisX.Title = "时间";
            chartFlow.ChartAreas[0].AxisX.Interval = 1; // 设置显示间隔为 1 秒
            chartFlow.ChartAreas[0].AxisX.Minimum = 0;
            chartFlow.ChartAreas[0].AxisX.Maximum = 20;
            chartFlow.ChartAreas[0].AxisY.Minimum = 0;
            chartFlow.ChartAreas[0].AxisY.Maximum = 100;
            // 设置 Y 轴为温度
            chartFlow.ChartAreas[0].AxisY.Title = "流量 (L/min)";
        }

        #region 添加 温度 压力 流量 数据
        private void AddTmpData(double coldTmp, double hotTmp, double mixTmp)
        {
            DateTime time = DateTime.Now;
            TimeSpan timeSpan = time - nowTime;
            int seconds = (int)timeSpan.TotalSeconds;
            chartTmp.Series["热水温度"].Points.AddXY(seconds, hotTmp);
            AddProjectValue("热水温度", hotTmp.ToString("F2"), seconds.ToString());
            chartTmp.Series["冷水温度"].Points.AddXY(seconds, coldTmp);
            AddProjectValue("冷水温度", coldTmp.ToString("F2"), seconds.ToString());
            chartTmp.Series["混水温度"].Points.AddXY(seconds, mixTmp);
            AddProjectValue("混水温度", mixTmp.ToString("F2"), seconds.ToString());
            SetTmp(hotTmp, coldTmp, mixTmp);
        }

        private void AddPressData(double coldPress, double hotPress, double mixPress)
        {
            DateTime time = DateTime.Now;
            TimeSpan timeSpan = time - nowTime;
            int seconds = (int)timeSpan.TotalSeconds;
            chartPress.Series["热水压力"].Points.AddXY(time, hotPress);
            AddProjectValue("热水压力", hotPress.ToString("F2"), seconds.ToString());
            chartPress.Series["冷水压力"].Points.AddXY(time, coldPress);
            AddProjectValue("热水压力", coldPress.ToString("F2"), seconds.ToString());
            chartPress.Series["混水压力"].Points.AddXY(time, mixPress);
            AddProjectValue("热水压力", mixPress.ToString("F2"), seconds.ToString());
            SetPress(hotPress, coldPress, mixPress);
        }

        private void AddFlowData(double coldFlow, double hotFlow, double mixFlow)
        {
            DateTime time = DateTime.Now;
            TimeSpan timeSpan = time - nowTime;
            int seconds = (int)timeSpan.TotalSeconds;
            chartFlow.Series["冷水流量"].Points.AddXY(time, coldFlow);
            AddProjectValue("冷水流量", coldFlow.ToString("F2"), seconds.ToString());
            chartFlow.Series["热水流量"].Points.AddXY(time, hotFlow);
            AddProjectValue("热水流量", hotFlow.ToString("F2"), seconds.ToString());
            chartFlow.Series["混水流量"].Points.AddXY(time, mixFlow);
            AddProjectValue("混水流量", mixFlow.ToString("F2"), seconds.ToString());
            SetFlow(hotFlow, coldFlow, mixFlow);
        }

        #endregion

        #region 设置 温度  压力  流量 数据显示
        private void SetTmp(double hotTmp, double coldTmp, double mixTmp)
        {
            this.txtHotTmp.Text = string.Format("{0} °C", hotTmp.ToString("F2"));
            this.txtColdTmp.Text = string.Format("{0} °C", coldTmp.ToString("F2"));
            this.txtMixTmp.Text = string.Format("{0} °C", mixTmp.ToString("F2"));
            
        }

        private void SetPress(double hotPress, double coldPress, double mixPress)
        {
            this.txtHotPress.Text = string.Format("{0} bar", hotPress.ToString("F2"));
            this.txtColdPress.Text = string.Format("{0} bar", coldPress.ToString("F2"));
            this.txtMixPress.Text = string.Format("{0} bar", mixPress.ToString("F2"));
        }

        private void SetFlow(double hotFlow, double coldFlow, double mixFlow)
        {
            this.txtHotFlow.Text = string.Format("{0} L/min", hotFlow.ToString("F2"));
            this.txtColdFlow.Text = string.Format("{0} L/min", coldFlow.ToString("F2"));
            this.txtMixFlow.Text = string.Format("{0} L/min", mixFlow.ToString("F2"));
        }

        private void AddProjectValue(string name,string value,string time)
        {
            deviceContext.InsertReport(new ReportTableEntity()
            {
                ItemName= name,
                Value= value,
                TypeName = time
            });
        }
        #endregion

        private void CheckNamePlateParam()
        {
            if (string.IsNullOrEmpty(txtProductName.Text))
            {
                System.Windows.MessageBox.Show("产品名称需要填写");
            }
            if (string.IsNullOrEmpty(txtStandard.Text))
            {
                System.Windows.MessageBox.Show("规格需要填写");
            }
            if (string.IsNullOrEmpty(txtModel.Text))
            {
                System.Windows.MessageBox.Show("型号需要填写");
            }
            if (string.IsNullOrEmpty(txtTestScale.Text))
            {
                System.Windows.MessageBox.Show("刻度需要填写");
            }
            if (string.IsNullOrEmpty(txtTestName.Text))
            {
                System.Windows.MessageBox.Show("测试人员需要填写");
            }
            if (string.IsNullOrEmpty(txtTestContent.Text))
            {
                System.Windows.MessageBox.Show("测试内容需要填写");
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {

            Random t = new Random();
            AddTmpData(t.NextDouble() * 50, t.NextDouble() * 60, t.NextDouble() * 50);
            UpdateData();

        }
        private void Btn_TmpLeftClick(object sender, EventArgs e)
        {
            chartTmp.ChartAreas[0].AxisX.Minimum += 1;
            chartTmp.ChartAreas[0].AxisX.Maximum += 1;
        }
        private void btnRightTmp_Click(object sender, EventArgs e)
        {
            if (chartTmp.ChartAreas[0].AxisX.Minimum > -1)
            {
                chartTmp.ChartAreas[0].AxisX.Minimum -= 1;
                chartTmp.ChartAreas[0].AxisX.Maximum -= 1;
            }
        }
        private void BtnPressRight_Click(object sender, EventArgs e)
        {
            if (chartTmp.ChartAreas[0].AxisX.Minimum > -1)
            {
                chartPress.ChartAreas[0].AxisX.Minimum -= 1;
                chartPress.ChartAreas[0].AxisX.Maximum -= 1;
            }
        }
        private void BtnPressLeft_Click(object sender, EventArgs e)
        {
            chartPress.ChartAreas[0].AxisX.Minimum += 1;
            chartPress.ChartAreas[0].AxisX.Maximum += 1;
        }
        private void BtnFlowLeft_Click(object sender, EventArgs e)
        {
            chartFlow.ChartAreas[0].AxisX.Minimum += 1;
            chartFlow.ChartAreas[0].AxisX.Maximum += 1;
        }
        private void BtnFlowRight_Click(object sender, EventArgs e)
        {
            if (chartTmp.ChartAreas[0].AxisX.Minimum > -1)
            {
                chartFlow.ChartAreas[0].AxisX.Minimum -= 1;
                chartFlow.ChartAreas[0].AxisX.Maximum -= 1;
            }
        }
        private void BtnTmpOpenClick(object sender, EventArgs e)
        {
            chartTmp.ChartAreas[0].AxisX.ScaleView.Zoom(chartTmp.ChartAreas[0].AxisX.ScaleView.ViewMinimum / 2, chartTmp.ChartAreas[0].AxisX.ScaleView.ViewMaximum * 2);
            chartTmp.ChartAreas[0].AxisY.ScaleView.Zoom(chartTmp.ChartAreas[0].AxisY.ScaleView.ViewMinimum / 2, chartTmp.ChartAreas[0].AxisY.ScaleView.ViewMaximum * 2);
        }
        private void BtnTmpClose_Click(object sender, EventArgs e)
        {
            chartTmp.ChartAreas[0].AxisX.ScaleView.Zoom(chartTmp.ChartAreas[0].AxisX.ScaleView.ViewMinimum * 2, chartTmp.ChartAreas[0].AxisX.ScaleView.ViewMaximum / 2);
            chartTmp.ChartAreas[0].AxisY.ScaleView.Zoom(chartTmp.ChartAreas[0].AxisY.ScaleView.ViewMinimum * 2, chartTmp.ChartAreas[0].AxisY.ScaleView.ViewMaximum / 2);
        }
        private void BtnPressOpen_Click(object sender, EventArgs e)
        {
            chartPress.ChartAreas[0].AxisX.ScaleView.Zoom(chartPress.ChartAreas[0].AxisX.ScaleView.ViewMinimum / 2, chartPress.ChartAreas[0].AxisX.ScaleView.ViewMaximum * 2);
            chartPress.ChartAreas[0].AxisY.ScaleView.Zoom(chartPress.ChartAreas[0].AxisY.ScaleView.ViewMinimum / 2, chartPress.ChartAreas[0].AxisY.ScaleView.ViewMaximum * 2);
        }
        private void BtnPressClose_Click(object sender, EventArgs e)
        {
            chartPress.ChartAreas[0].AxisX.ScaleView.Zoom(chartPress.ChartAreas[0].AxisX.ScaleView.ViewMinimum * 2, chartPress.ChartAreas[0].AxisX.ScaleView.ViewMaximum / 2);
            chartPress.ChartAreas[0].AxisY.ScaleView.Zoom(chartPress.ChartAreas[0].AxisY.ScaleView.ViewMinimum * 2, chartPress.ChartAreas[0].AxisY.ScaleView.ViewMaximum / 2);
        }
        private void BtnFlowOpenClick(object sender, EventArgs e)
        {
            chartFlow.ChartAreas[0].AxisX.ScaleView.Zoom(chartFlow.ChartAreas[0].AxisX.ScaleView.ViewMinimum / 2, chartFlow.ChartAreas[0].AxisX.ScaleView.ViewMaximum * 2);
            chartFlow.ChartAreas[0].AxisY.ScaleView.Zoom(chartFlow.ChartAreas[0].AxisY.ScaleView.ViewMinimum / 2, chartFlow.ChartAreas[0].AxisY.ScaleView.ViewMaximum * 2);
        }
        private void BtnFlowClose_Click(object sender, EventArgs e)
        {
            chartFlow.ChartAreas[0].AxisX.ScaleView.Zoom(chartFlow.ChartAreas[0].AxisX.ScaleView.ViewMinimum * 2, chartFlow.ChartAreas[0].AxisX.ScaleView.ViewMaximum / 2);
            chartFlow.ChartAreas[0].AxisY.ScaleView.Zoom(chartFlow.ChartAreas[0].AxisY.ScaleView.ViewMinimum * 2, chartFlow.ChartAreas[0].AxisY.ScaleView.ViewMaximum / 2);
        }
        private void BtnTmpRestore_Click(object sender, EventArgs e)
        {
            this.chartTmp.Series["热水温度"].Enabled = true;
            this.chartTmp.Series["冷水温度"].Enabled = true;
            this.chartTmp.Series["混水温度"].Enabled = true;
        }
        private void btnBtnPressRestore_Click(object sender, EventArgs e)
        {
            this.chartPress.Series["热水压力"].Enabled = true;
            this.chartPress.Series["冷水压力"].Enabled = true;
            this.chartPress.Series["混水压力"].Enabled = true;
        }
        private void BtnFlowRestore_Click(object sender, EventArgs e)
        {
            this.chartFlow.Series["热水流量"].Enabled = true;
            this.chartFlow.Series["冷水流量"].Enabled = true;
            this.chartFlow.Series["混水流量"].Enabled = true;
        }


        private void LoadingSelectTableControl()
        {
            
        }


        private void LoadDevice()
        {
            var devices = deviceContext.Query<AdjustmentTableEntity>("SELECT ID,Name,Flag,Address,Port,DeviceSort,Type,Length,WriteRead from AdjustmentTable WHERE Type='MODBUS'").OrderBy(t => t.DeviceSort).ToList();
            List<string> check = new List<string>();
            if (devices != null && devices.Count > 0)
            {
                foreach (var dev in devices)
                {
                    if (!check.Contains(dev.Port))
                    {
                        check.Add(dev.Port);
                        List<SlaveDevice> SlaveDevices = new List<SlaveDevice>();
                        devices.Where(t => t.Port == dev.Port && t.WriteRead=="读").ToList().ForEach(t =>
                        {
                            SlaveDevices.Add(new SlaveDevice()
                            {
                                Slave = (byte)int.Parse(t.Address),
                                DeviceType = DeviceTypeParam.SP,
                                Address = int.Parse(t.Address),
                                Length = int.Parse(t.Length),
                                DataType = t.Name

                            });
                        });
                        devices.Where(t => t.Port == dev.Port && t.WriteRead == "写").ToList().ForEach(t =>
                        {
                            SlaveDevices.Add(new SlaveDevice()
                            {
                                Slave = (byte)int.Parse(t.Address),
                                DeviceType = DeviceTypeParam.PV,
                                Address = int.Parse(t.Address),
                                Length = int.Parse(t.Length),
                                DataType =t.Name

                            });
                        });
                        DeviceMasterSlave Master = new DeviceMasterSlave(dev.Port, SlaveDevices);
                        deviceMasters.Add(Master);
                    }
                }
            }
        }

        private async Task<double> ReadDeviceValue(string deviceType)
        {
            if (deviceMasters != null && deviceMasters.Count > 0)
            {
                foreach (var m in deviceMasters)
                {
                    var result = await m.ReadHoldingRegisters(deviceType);
                    if (result > 0)
                    {
                        return result;
                    }
                }
            }
            return 0;
        }

        public async void UpdateData()
        {
            double coldTmp = await ReadDeviceValue("冷水温度");
            double coldPress = await ReadDeviceValue("冷水压力");
            double coldFlow = await ReadDeviceValue("冷水流量");
            double hotTmp = await ReadDeviceValue("热水温度");
            double hotPress = await ReadDeviceValue("热水压力");
            double hotFlow = await ReadDeviceValue("热水流量");
            double mixTmp = await ReadDeviceValue("混水温度");
            double mixPress = await ReadDeviceValue("混水压力");

            AddTmpData(coldTmp, hotTmp, mixTmp);
            AddPressData(coldPress, hotPress, mixPress);
            AddFlowData(coldFlow, hotFlow, coldFlow + hotFlow);
        }


        private void InitTableSelect()
        {
            if (this.tabControl1.SelectedIndex == -1)
            {
                return;
            }
            int counts = this.tabControl1.TabPages[this.tabControl1.SelectedIndex].Controls.Count;
            for (int i = 0; i < counts; i++)
            {
                Control control = this.tabControl1.TabPages[this.tabControl1.SelectedIndex].Controls[i];
                if (control is Panel)
                {
                    int panelCount = control.Controls.Count;
                    for (int j = 0; j < panelCount; j++)
                    {
                         Control control2 = control.Controls[j];
                        if (control2 is TextBox)
                        {
                            textBoxes.Add((TextBox)control2);
                        }
                        else if (control2 is CheckBox)
                        {
                            checkBoxes.Add((CheckBox)control2);
                        }
                    }
                } 
            }

            var controls = deviceContext.Query<ControlEntity>("SELECT ControlName,ControlType,ClickShowName,AdjustId,ProjectNo from ControlBanging where ProjectNo='"+ this.tabControl1.SelectedIndex + 1 + "'");

            int m = 0;
            int n = 0;
            foreach (var item in controls)
            {
                if (item.ControlType == "TextBox")
                {
                    textBoxes[m].Name = item.ControlName;
                    ToolTip toolTip1 = new ToolTip();
                    toolTip1.SetToolTip(textBoxes[m], item.ClickShowName);
                    m++;
                }
                if (item.ControlType == "CheckBox")
                {
                    checkBoxes[m].Name = item.ControlName;
                    checkBoxes[m].Text = item.ClickShowName;
                    ToolTip toolTip1 = new ToolTip();
                    toolTip1.SetToolTip(textBoxes[m], item.ClickShowName);
                    n++;
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            textBoxes = new List<TextBox>();
            checkBoxes = new List<CheckBox>();
            InitTableSelect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            deviceContext.DeleteNamePlate();

            CheckNamePlateParam();

            deviceContext.InsertNamePlate(new NameplateParameterTableEntity()
            {
                ProductName = txtProductName.Text,
                Content = txtTestContent.Text,
                ProjectNo = (this.tabControl1.SelectedIndex + 1).ToString(),
                Standard = txtStandard.Text,
                Model = txtModel.Text,
                TestName = txtTestName.Text,
                TestScale = txtTestScale.Text,
            });

            timer1.Start();


        }

        private void button2_Click(object sender, EventArgs e)
        {
            timer1.Stop();
        }

        private void chartTmp_Click(object sender, EventArgs e)
        {

        }
    }
}
