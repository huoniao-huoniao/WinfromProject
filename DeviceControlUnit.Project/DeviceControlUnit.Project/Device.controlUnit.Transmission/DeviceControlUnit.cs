using Modbus.Device;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Device.controlUnit.Transmission
{
    public class DeviceControlUnit
    {
        private  Dictionary<string, IModbusMaster> masterSet = new Dictionary<string, IModbusMaster>();

        private SerialPort serialPort = new SerialPort();

        public DeviceControlUnit(DeviceMasterSlave deviceMasterSlave) 
        {
            serialPort.PortName = deviceMasterSlave.PortName;
            serialPort.BaudRate = 9600;
            serialPort.Parity = Parity.None;
            serialPort.DataBits = 8;
            serialPort.StopBits = StopBits.One;
        }

        public void Open()
        {
            serialPort.Open();
        }

    }
}
