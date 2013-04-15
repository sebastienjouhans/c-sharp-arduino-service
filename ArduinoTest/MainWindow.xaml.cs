using Kinrou.io.Arduino;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace ArduinoTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private ArduinoService arduino;

        private bool _isOn = false;

        public MainWindow()
        {
            InitializeComponent();
            SetupArduino();
        }

        private void SetupArduino()
        {
            arduino = new ArduinoService();
            arduino.OnInitialisedSuccessHandler += ArduinoInitialisedSuccessHandler;
            arduino.OnInitialisedFailedHandler += ArduinoInitialisedFailedHandler;
        }

        private void ArduinoInitialisedSuccessHandler(object sender)
        {
            arduino.OnInitialisedSuccessHandler -= ArduinoInitialisedSuccessHandler;
            arduino.OnInitialisedFailedHandler -= ArduinoInitialisedFailedHandler;
            arduino.OnReceivedData += ArduinoDataReceived;             
        }

        private void ArduinoInitialisedFailedHandler(object sender)
        {
            arduino.OnInitialisedSuccessHandler -= ArduinoInitialisedSuccessHandler;
            arduino.OnInitialisedFailedHandler -= ArduinoInitialisedFailedHandler;
            throw new Exception("Can't connect to arduino - check the port");
        }

        private void ArduinoDataReceived(object sender, EventArgs e)
        {
            string data = ((ArduinoServiceEventArgs)e).data;
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(() =>
            {
                Debug.WriteLine(data);
            }));
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (arduino.isInitialised)
            {
                if (_isOn)
                {
                    arduino.Write("0");
                    _isOn = false;
                }
                else
                {
                    arduino.Write("1");
                    _isOn = true;
                }
            }
        }
    }
}
