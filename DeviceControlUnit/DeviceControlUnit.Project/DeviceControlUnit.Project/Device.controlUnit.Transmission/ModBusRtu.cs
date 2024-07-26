using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using Modbus.Device;

namespace Device.controlUnit.Transmission
{
    public static class ModBusRtu
    {
        private static readonly List<SerialPort> Ports = new List<SerialPort>();

        private static readonly Dictionary<string,ModbusSerialMaster> master = new Dictionary<string,ModbusSerialMaster>();


        private static readonly Dictionary<string, double> deviceValue = new Dictionary<string, double>();




    }
}
