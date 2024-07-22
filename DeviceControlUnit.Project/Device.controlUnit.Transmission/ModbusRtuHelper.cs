using Modbus.Device;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Device.controlUnit.Transmission
{
    public class ModbusRtuHelper
    {
        private SerialPort serialPort = new SerialPort();

        private static Dictionary<string, IModbusMaster> masterSet = new Dictionary<string, IModbusMaster>();

        private string port;

        private byte slave;

        public ModbusRtuHelper(string port, byte slave)
        {
            this.port = port;
            this.slave = slave;
            if (!masterSet.Keys.Contains(port))
            {
                //设定串口参数
                serialPort.PortName = port;
                serialPort.BaudRate = 9600;
                serialPort.Parity = Parity.None;
                serialPort.DataBits = 8;
                serialPort.StopBits = StopBits.One;

                //创建ModbusRTU主站实例
                var master = ModbusSerialMaster.CreateRtu(serialPort);
                master.Transport.ReadTimeout = 2000;
                masterSet.Add(port, master);

                //打开串口
                if (!serialPort.IsOpen) 
                {
                    serialPort.Open(); 
                }
            }
        }

        public void Close()
        {
            serialPort.Close();

            masterSet = null;
        }

        public async Task<ushort[]> ReadHoldingRegistersAsync(ushort address, ushort lenght)
        {
            return await ReadHoldingRegistersAsync(address, lenght, CancellationToken.None);
        }

        /// <summary>
        /// 读取保持型寄存器
        /// </summary>
        /// <returns></returns>
        private async Task<ushort[]> ReadHoldingRegistersAsync(ushort address, ushort lenght, CancellationToken cancellationToken)
        {
            return await masterSet[port].ReadHoldingRegistersAsync(slave, address, lenght);
        }

        /// <summary>
        /// 写入单个寄存器
        /// </summary>
        public void WriteSingleRegister(ushort address, ushort result)
        {

            masterSet[port].WriteSingleRegister(slave, address, result);
        }

    }
}
