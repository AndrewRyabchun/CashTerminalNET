using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Windows;
using CashTerminal.Commons;

namespace CashTerminal.Models
{
    /// <summary>
    /// Обертка над классом SerialPort.
    /// </summary>
    internal class SerialPortProvider
    {
        /// <summary>
        /// Используемый для чтения порт.
        /// </summary>
        private readonly SerialPort _sp;

        /// <summary>
        /// Список всех доступных портов.
        /// </summary>
        public static List<string> AllPortsName => new List<string>(SerialPort.GetPortNames());

        /// <summary>
        /// Имя порта.
        /// </summary>
        public string PortName
        {
            get { return _sp.PortName; }
            set
            {
                try
                {
                    if (AllPortsName.Contains(value))
                    {
                        _sp.Close();
                        _sp.PortName = value;
                        _sp.Open();
                    }
                }
                catch (Exception e)
                {
                    UIMediator.Instance.Update(e.Message);
                }
            }
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса SerialPortProvider, используя указанное имя порта.
        /// </summary>
        /// <param name="portName">Имя порта.</param>
        /// <param name="handler">Делегат-обработчик события - получения данных.</param>
        public SerialPortProvider(string portName, SerialDataReceivedEventHandler handler)
            : this(portName, 9600, Parity.None, 8, StopBits.One, handler)
        {
        }

        /// <summary>
        /// Инициализирует новый экземпляр класса SerialPortProvider, используя указанное имя порта, скорость передачи в бодах, бит четности, 
        /// биты данных, стоп-бит и базовый обработчик события.
        /// </summary>
        /// <param name="portName">Порт для использования.</param>
        /// <param name="baudRate">Скорость в бодах.</param>
        /// <param name="parity">Одно из значений Parity.</param>
        /// <param name="dataBits">Число битов данных.</param>
        /// <param name="stopBits">Одно из значений StopBits.</param>
        /// <param name="handler">Делегат-обработчик события - получения данных.</param>
        public SerialPortProvider(string portName, int baudRate, Parity parity, int dataBits, StopBits stopBits,
            SerialDataReceivedEventHandler handler)
        {
            _sp = new SerialPort(portName, baudRate, parity, dataBits, stopBits)
            {
                RtsEnable = true,
                DtrEnable = true
            };

            //Позволяет избежать проверок на NullReferenceException, если обработчкику события присвоено null.
            _sp.DataReceived += delegate { };
            _sp.DataReceived += handler;

            _sp.Open();
        }
    }
}