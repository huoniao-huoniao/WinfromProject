using Device.controlUnit.Transmission;
using Device.ControlUnit.Data;
using Device.ControlUnit.Data.Entitys;
using LiveCharts.Wpf.Charts.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace DeviceControlUnit.Project
{
    public partial class MainForm : Form
    {

        private readonly double ZoomFactor = 1.1;  // 放大倍数

        private readonly List<DeviceMasterSlave> deviceMasters = new List<DeviceMasterSlave>();
        public MainForm()
        {
            InitializeComponent();
            InitializeChart();
            this.chartTmp.MouseWheel += ChartTmp_MouseWheel;
            LoadDevice();
            UpdateData();
        }

        private void InitializeChart()
        {
            // 设置 X 轴为时间
            chartTmp.ChartAreas[0].AxisX.Title = "时间";
            chartTmp.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";

            // 设置 Y 轴为温度
            chartTmp.ChartAreas[0].AxisY.Title = "温度 (°C)";


            chartPress.ChartAreas[0].AxisX.Title = "时间";
            chartPress.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";

            // 设置 Y 轴为温度
            chartPress.ChartAreas[0].AxisY.Title = "压力 (bar)";



            chartFlow.ChartAreas[0].AxisX.Title = "时间";
            chartFlow.ChartAreas[0].AxisX.LabelStyle.Format = "HH:mm:ss";

            // 设置 Y 轴为温度
            chartFlow.ChartAreas[0].AxisY.Title = "流量 (L/min)";

        }

        #region 添加 温度 压力 流量 数据
        private void AddTmpData(double coldTmp,double hotTmp,double mixTmp)
        {
            DateTime time = DateTime.Now;
            chartTmp.Series["热水温度"].Points.AddXY(time, hotTmp);
            chartTmp.Series["冷水温度"].Points.AddXY(time, coldTmp);
            chartTmp.Series["混水温度"].Points.AddXY(time, mixTmp);
            SetTmp(hotTmp, coldTmp, mixTmp);
        }

        private void AddPressData(double coldPress, double hotPress, double mixPress)
        {
            DateTime time = DateTime.Now;
            chartPress.Series["热水压力"].Points.AddXY(time, hotPress);
            chartPress.Series["冷水压力"].Points.AddXY(time, coldPress);
            chartPress.Series["混水压力"].Points.AddXY(time, mixPress);
            SetPress(hotPress, coldPress, mixPress);
        }

        private void AddFlowData(double coldFlow, double hotFlow, double mixFlow)
        {
            DateTime time = DateTime.Now;
            chartFlow.Series["热水流量"].Points.AddXY(time, coldFlow);
            chartFlow.Series["冷水流量"].Points.AddXY(time, hotFlow);
            chartFlow.Series["混水流量"].Points.AddXY(time, mixFlow);
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
        #endregion

        private void Timer1_Tick(object sender, EventArgs e)
        {
            
        }

        private void ChartTmp_MouseEnter(object sender, EventArgs e)
        {
            if (!chartTmp.Focused)
                chartTmp.Focus();
        }

        private void ChartTmp_MouseLeave(object sender, EventArgs e)
        {
            if (chartTmp.Focused)
                chartTmp.Parent.Focus();
        }

        private void ChartTmp_MouseWheel(object sender, MouseEventArgs e)
        {

            if (e.Delta > 0)  // 滚轮向前滚动，放大
            {
                ZoomIn();
            }
            else if (e.Delta < 0)  // 滚轮向后滚动，缩小
            {
                ZoomOut();
            }
        }
        private void ZoomIn()
        {
            chartTmp.ChartAreas[0].AxisX.ScaleView.Zoom(chartTmp.ChartAreas[0].AxisX.ScaleView.ViewMinimum * (1 / ZoomFactor),
                                                       chartTmp.ChartAreas[0].AxisX.ScaleView.ViewMaximum * (1 / ZoomFactor));

            chartTmp.ChartAreas[0].AxisY.ScaleView.Zoom(chartTmp.ChartAreas[0].AxisY.ScaleView.ViewMinimum * (1 / ZoomFactor),
                                                       chartTmp.ChartAreas[0].AxisY.ScaleView.ViewMaximum * (1 / ZoomFactor));
        }

        private void ZoomOut()
        {
            chartTmp.ChartAreas[0].AxisX.ScaleView.Zoom(chartTmp.ChartAreas[0].AxisX.ScaleView.ViewMinimum * ZoomFactor,
                                                       chartTmp.ChartAreas[0].AxisX.ScaleView.ViewMaximum * ZoomFactor);

            chartTmp.ChartAreas[0].AxisY.ScaleView.Zoom(chartTmp.ChartAreas[0].AxisY.ScaleView.ViewMinimum * ZoomFactor,
                                                       chartTmp.ChartAreas[0].AxisY.ScaleView.ViewMaximum * ZoomFactor);
        }


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

        private void btn_StartClick(object sender, EventArgs e)
        {
            CheckNamePlateParam();
            this.timer1.Start();
        }




        private void LoadDevice()
        {
            DeviceContextDb deviceContext = new DeviceContextDb();
            var devices = deviceContext.Query<AdjustmentTableEntity>("SELECT ID,Name,Flag,Address,Port,Sort,Type from AdjustmentTable WHERE Type='MODBUS'").OrderBy(t => t.Sort).ToList();
            List<string> check = new List<string>();
            if (devices != null && devices.Count > 0)
            {
                foreach (var dev in devices)
                {
                    if (!check.Contains(dev.Port))
                    {
                        check.Add(dev.Port);
                        List<SlaveDevice> SlaveDevices =new List<SlaveDevice>();
                        devices.Where(t => t.Port == dev.Port).ToList().ForEach(t =>
                        {
                            SlaveDevices.Add(new SlaveDevice()
                            {
                                Slave = (byte)int.Parse(t.Address),
                                DeviceType = DeviceTypeParam.SP,
                                Address = 257,
                                Length = 1,
                                DataType= ConvertNameParam(t.Name)

                            });
                            SlaveDevices.Add(new SlaveDevice()
                            {
                                Slave = (byte)int.Parse(t.Address),
                                DeviceType = DeviceTypeParam.PV,
                                Address = 256,
                                Length = 1,
                                DataType = ConvertNameParam(t.Name)

                            });
                        });
                        DeviceMasterSlave Master = new DeviceMasterSlave(dev.Port, SlaveDevices);
                        deviceMasters.Add(Master);
                    }
                }
            }
        }

        private async Task<double> ReadDeviceValue(DeviceTypeEnum deviceType)
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
            double coldTmp = await ReadDeviceValue(DeviceTypeEnum.COLDTMP);
            double coldPress = await ReadDeviceValue(DeviceTypeEnum.COLDPRESS);
            double coldFlow = await ReadDeviceValue(DeviceTypeEnum.COLDFLOW);
            double hotTmp = await ReadDeviceValue(DeviceTypeEnum.HOTTMP);
            double hotPress = await ReadDeviceValue(DeviceTypeEnum.HOTPRESS);
            double hotFlow = await ReadDeviceValue(DeviceTypeEnum.HOTFLOW);
            double mixTmp = await ReadDeviceValue(DeviceTypeEnum.MIXTMP);
            double mixPress = await ReadDeviceValue(DeviceTypeEnum.MIXPRESS);

            AddTmpData(coldTmp, hotTmp, mixTmp);
            AddPressData(coldPress, hotPress, mixPress);
            AddFlowData(coldFlow, hotFlow, coldFlow+hotFlow);
        }





        private DeviceTypeEnum ConvertNameParam(string name)
        {
            if (name == "冷水温度")
            {
                return DeviceTypeEnum.COLDTMP;
            }
            else if (name == "热水温度")
            {
                return DeviceTypeEnum.HOTTMP;
            }
            else if (name == "冷水压力")
            {
                return DeviceTypeEnum.COLDPRESS;
            }
            else if (name == "冷水流量")
            {
                return DeviceTypeEnum.COLDFLOW;
            }
            else if (name == "热水压力")
            {
                return DeviceTypeEnum.HOTPRESS;
            }
            else if (name == "热水流量")
            {
                return DeviceTypeEnum.HOTFLOW;
            }
            else if (name == "混水压力")
            {
                return DeviceTypeEnum.MIXPRESS;
            }
            else
            {
                return DeviceTypeEnum.MIXTMP;
            }
        }

        private void SetChart()
        {
            this.chartTmp.ChartAreas[0].AxisX.ScrollBar.ButtonStyle = ScrollBarButtonStyles.All;
            this.chartTmp.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = true;
            this.chartTmp.ChartAreas[0].AxisX.ScrollBar.Size = 20;

        }
    }

        
    }
}
