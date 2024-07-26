using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Device.controlUnit.Transmission
{
    public class Device
    {
        private ModbusRtuHelper modbusRtuHelper;

        public string PortName { get; set; }

        public int Address { get; set; }

        public string DeviceType { get; set; }

        public string Protocol { get; set; }

        public string Status { get; set; }


        public Device(string port,string status,int slave,string protocol,string deviceType)
        {
            this.PortName = port;
            this.Status = status;
            this.Address = slave;
            this.DeviceType = deviceType;
        }

        public void StartDevice()
        {
            try
            {
                modbusRtuHelper = new ModbusRtuHelper(PortName, (byte)Address);
                Status = "已连接";
            }
            catch(Exception ex)
            {
                throw new Exception($"{DeviceType} 连接仪表失败:" + ex.Message);
            }
        }

        public void StopDevice() 
        {
            modbusRtuHelper.Close();
            Status = "未连接";
        }

        /// <summary>
        /// 两位ushort 转浮点数
        /// </summary>
        /// <param name="modbusRegisters"></param>
        /// <returns></returns>
        private double ConvertFloat(ushort[] modbusRegisters)
        {

            byte[] floatBytes = new byte[4]; // 用于存储浮点数的字节表示形式

            // 将两个 ushort 寄存器的值按照小端（Little Endian）顺序组合成一个字节数组
            floatBytes[0] = (byte)(modbusRegisters[0] & 0xFF);
            floatBytes[1] = (byte)(modbusRegisters[0] >> 8);
            floatBytes[2] = (byte)(modbusRegisters[1] & 0xFF);
            floatBytes[3] = (byte)(modbusRegisters[1] >> 8);

            // 使用 BitConverter 将字节数组转换为浮点数
            float result = BitConverter.ToSingle(floatBytes, 0);
            return result;
        }

        /// <summary>
        /// 读取值
        /// </summary>
        /// <param name="addr">地址</param>
        /// <param name="length">长度</param>
        /// <returns></returns>
        private async Task<double> ReadValueAsync(int addr, int length)
        {
            try
            {
                var resUshorts = await modbusRtuHelper.ReadHoldingRegistersAsync((ushort)addr, (ushort)length);
                if (length == 2)
                {
                    return ConvertFloat(resUshorts);
                }
                return resUshorts[0];
            }
            catch (Exception)
            {
                Status = "未连接";
                throw;
            }
        }


    }
}
