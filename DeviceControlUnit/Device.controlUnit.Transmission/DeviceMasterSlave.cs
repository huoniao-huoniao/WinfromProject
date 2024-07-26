using Modbus.Device;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device.controlUnit.Transmission
{
    public class DeviceMasterSlave
    {
        public string PortName { get; set; }

        private SerialPort serialPort = new SerialPort();

        private ModbusSerialMaster SlaveDeviceSerial { get; set; }

        private List<SlaveDevice> SlaveDevices { get; set; }



        public DeviceMasterSlave(string portName, List<SlaveDevice> slaveDevices) 
        {
            this.SlaveDevices=slaveDevices;
            serialPort.PortName = portName;
            serialPort.BaudRate = 9600;
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
            if (!serialPort.IsOpen) 
            { 
                serialPort.Open();
            }
            SlaveDeviceSerial = ModbusSerialMaster.CreateRtu(serialPort);
            SlaveDeviceSerial.Transport.ReadTimeout = 2000;
        }

        public void Close()
        {
            serialPort.Close();
        }

        public async Task<double> ReadHoldingRegisters(string dataType)
        {
            try
            {
                if (SlaveDevices != null)
                {
                    var slave = SlaveDevices.Where(t => t.DataType == dataType && t.DeviceType== DeviceTypeParam.PV).FirstOrDefault();

                    if (slave == null) return 0;

                    var result = await SlaveDeviceSerial.ReadHoldingRegistersAsync(slave.Slave, (ushort)slave.Address, (ushort)slave.Length);
                     
                    if (slave.Length == 2)
                    {
                        return ConvertFloat(result);
                    }
                    return result[0];
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void WriteHoldingRegisters(string dataType,double result)
        {
            try
            {
                if (SlaveDevices != null)
                {
                    var slave = SlaveDevices.Where(t => t.DataType == dataType && t.DeviceType == DeviceTypeParam.PV).FirstOrDefault();
                    var intValues = BitConverter.GetBytes(result); // 转换为字节数组
                    SlaveDeviceSerial.WriteSingleRegister(slave.Slave,(ushort)slave.Address, intValues[0]);
                    SlaveDeviceSerial.WriteSingleRegister(slave.Slave, (ushort)(slave.Address + 1), intValues[1]);
                }
            }

            catch(Exception ex)
            {
                throw ex;
            }
        }

        public void WriteHoldingRegisters(string dataType, int result)
        {
            try
            {
                if (SlaveDevices != null)
                {
                    var slave = SlaveDevices.Where(t => t.DataType == dataType && t.DeviceType == DeviceTypeParam.PV).FirstOrDefault();
                    SlaveDeviceSerial.WriteSingleRegister(slave.Slave, (ushort)slave.Address, (byte)result);
                }
            }

            catch (Exception ex)
            {
                throw ex;
            }
        }





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

    }


    public class SlaveDevice
    {
        public byte Slave { get; set; }

        public DeviceTypeParam DeviceType { get; set; }

        public int Address { get; set; }  //设备物理地址

        public int Length { get; set; }

        public string DataType { get; set; } //温度 压力 流量
    }


    public enum DeviceTypeParam
    {
        SP,
        MV,
        PV,
        AUTO,
        RUN,
    }
}
