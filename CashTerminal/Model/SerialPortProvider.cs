using System.Collections.Generic;
using System.IO.Ports;

namespace CashTerminal.Model
{
    class SerialPortProvider
    {
        private SerialPort _sp;
        private static List<string> AllPortsName => new List<string>(SerialPort.GetPortNames());
        public string PortName
        {
            get { return _sp.PortName; }
            set
            {
                if (AllPortsName.Contains(value))
                    PortName = value;
            }
        }

        public SerialPortProvider(SerialDataReceivedEventHandler handler)
            : this(AllPortsName[0], 9600, Parity.None, 8, StopBits.One, handler)
        {

        }

        public SerialPortProvider(string portName, SerialDataReceivedEventHandler handler)
            : this(portName, 9600, Parity.None, 8, StopBits.One, handler)
        {

        }

        public SerialPortProvider(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits,
            SerialDataReceivedEventHandler handler)
        {
            _sp = new SerialPort(portName, baudRate, parity, dataBits, stopBits);

            //Prevents from catching NullReferenceException if handler is null or not properly assigned for event
            _sp.DataReceived += delegate { };
            _sp.DataReceived += handler;
        }
    }
}
