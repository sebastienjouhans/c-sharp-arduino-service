using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Diagnostics;
using System.Windows;
using System.Timers;

namespace Kinrou.io.Arduino
{
    public class ArduinoService
    {
        public static string ARDUINO_PORT = "COM4";
        public static int ARDUINO_BAUD_RATE = 9600;

        private string _port;
        private int _baudRate;
        private bool _isInitialised = false;

        private SerialPort _serialPort;
        private Timer _initTimer;

        public delegate void ReceivedDataHandler(object sender, ArduinoServiceEventArgs e);
        public event ReceivedDataHandler OnReceivedData;

        public delegate void InitialisedHandler(object sender);
        public event InitialisedHandler OnInitialisedSuccessHandler;
        public event InitialisedHandler OnInitialisedFailedHandler;


        public ArduinoService()
        {
            _port = ARDUINO_PORT;
            _baudRate = ARDUINO_BAUD_RATE;
            Initialise();
        }

        public ArduinoService(string port, int baudRate)
        {
            _port = port;
            _baudRate = baudRate;
            Initialise();
        }


        public bool isInitialised { get { return _isInitialised; } }


        public void Initialise()
        {
            try
            {
                _serialPort = new SerialPort(_port, _baudRate);
                _serialPort.DataReceived += DataReceivedHandler;
                _serialPort.Open();
            }
            catch (Exception e)
            {
                initialisedFailed(this);
                return;
            }

            _initTimer = new Timer(500);
            _initTimer.Elapsed += new ElapsedEventHandler(TimerElapsed);
            _initTimer.Enabled = true;
        }


        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            string indata = sp.ReadExisting();
            //Debug.WriteLine("Data Received:");
            //Debug.Write(indata);
            receivedData(this, new ArduinoServiceEventArgs(indata));
        }


        private void initialisedSuccess(object sender)
        {
            if (OnInitialisedSuccessHandler != null)
            {
                OnInitialisedSuccessHandler(this);
            }
        }


        private void initialisedFailed(object sender)
        {
            if (OnInitialisedFailedHandler != null)
            {
                OnInitialisedFailedHandler(this);
            }
        }


        private void receivedData(object sender, ArduinoServiceEventArgs e)
        {
            if (OnReceivedData != null)
            {
                OnReceivedData(this, e);
            }
        }



        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (_serialPort.IsOpen)
            {
                _initTimer.Stop();
                _initTimer.Close();
                _initTimer.Elapsed -= TimerElapsed;
                _initTimer.Dispose();
                _initTimer = null;
                //Debug.WriteLine("Arduino ready");
                _isInitialised = true;
                initialisedSuccess(this);
            }
        }


        public void Write(string s)
        {
            if (_isInitialised) _serialPort.Write(s + "\n");
        }


        public void Dispose()
        {
            if (_initTimer != null)
            {
                _initTimer.Stop();
                _initTimer.Close();
                _initTimer.Elapsed -= TimerElapsed;
                _initTimer.Dispose();
                _initTimer = null;
            }

            if (_serialPort != null)
            {
                _serialPort.Close();
                _serialPort = null;
            }
        }

    }
}


/*

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO.Ports;
using System.Timers;
using System.Diagnostics;



/*
 * 
 *  private void SetupArduino()
    {
        arduino = new ArduinoConnector(ArduinoConnector.ARDUINO_PORT, ArduinoConnector.ARDUINO_BAUD_RATE);
        arduino.initialisedSuccessHandler += ArduinoInitialisedSuccessHandler;
        arduino.initialisedFailedHandler += ArduinoInitialisedFailedHandler;
    }

    private void ArduinoInitialisedSuccessHandler(object sender)
    {
        arduino.initialisedSuccessHandler -= ArduinoInitialisedSuccessHandler;
        arduino.initialisedFailedHandler -= ArduinoInitialisedFailedHandler;
        arduino.Write("0");            
    }

    private void ArduinoInitialisedFailedHandler(object sender)
    {
        arduino.initialisedSuccessHandler -= ArduinoInitialisedSuccessHandler;
        arduino.initialisedFailedHandler -= ArduinoInitialisedFailedHandler;
        throw new Exception("Can't connect to arduino - check the port");
    }
 * 
 * 
 * arduino.Write("1");
 * 
 * 
 


namespace Kinrou.io.Arduino
{
    public class ArduinoConnector
    {
        public static string ARDUINO_PORT = "COM4";
        public static int ARDUINO_BAUD_RATE = 9600;

        private string port;
        private int baudRate;
        private bool isInitialised = false;

        private SerialPort serialPort;
        private Timer initTimer;



        public delegate void InitialisedHandler(object sender);
        public event InitialisedHandler initialisedSuccessHandler;
        public event InitialisedHandler initialisedFailedHandler;


        public ArduinoConnector()
        {
            port = ARDUINO_PORT;
            baudRate = ARDUINO_BAUD_RATE;
            initialise();
        }


        public ArduinoConnector(string port, int baudRate)
        {
            this.port = port;
            this.baudRate = baudRate;
            initialise();
        }


        private void initialise()
        {
            try
            {
                serialPort = new SerialPort(port, baudRate);
                serialPort.DataReceived
                serialPort.Open();
            }
            catch
            {
                //throw new Exception("connection to arduino failed");
                initialisedFailedHandler(this);
                return;
            }
            Debug.WriteLine("Arduino initialise");
            initTimer = new Timer(500);
            initTimer.Elapsed += new ElapsedEventHandler(TimerElapsed);
            initTimer.Enabled = true;
        }



        private void TimerElapsed(object sender, ElapsedEventArgs e)
        {
            if (serialPort.IsOpen)
            {
                initTimer.Stop();
                initTimer.Close();
                initTimer.Elapsed -= new ElapsedEventHandler(TimerElapsed);
                initTimer.Dispose();
                initTimer = null;
                Debug.WriteLine("Arduino ready");
                isInitialised = true;
                initialisedSuccessHandler(this);
            }
        }


        public void Write(string s)
        {
            if (isInitialised) serialPort.Write(s + "\n");
        }
    }
}


*/