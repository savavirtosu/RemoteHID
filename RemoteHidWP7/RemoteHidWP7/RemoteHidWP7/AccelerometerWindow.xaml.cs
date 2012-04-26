using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Threading;
using Microsoft.Phone.Controls;
using Microsoft.Internal;
using Microsoft.Phone.Info;
using Microsoft.Xna.Framework;
using Microsoft.Devices.Sensors;

namespace RemoteHidWP7
{
    public partial class AccelerometerWindow : PhoneApplicationPage
    {
        Accelerometer accelerometer;
        DispatcherTimer timer;
        Vector3 acceleration;
        bool isDataValid;

        //private Communicator communicator;


        public AccelerometerWindow()
        {
            InitializeComponent();
            Init();
        }

        public void Init()
        {
            if (!Accelerometer.IsSupported)
            {
                // The device on which the application is running does not support
                // the accelerometer sensor. Alert the user and hide the
                // application bar.
                statusTextBlock.Text = "device does not support compass";
                ApplicationBar.IsVisible = false;
            }
            else
            {
                // Initialize the timer and add Tick event handler, but don't start it yet.
                timer = new DispatcherTimer();
                timer.Interval = TimeSpan.FromMilliseconds(30);
                timer.Tick += new EventHandler(timer_Tick);
            }

            //MainPage.communicator

            if (accelerometer == null)
            {
                // Instantiate the accelerometer.
                accelerometer = new Accelerometer();


                // Specify the desired time between updates. The sensor accepts
                // intervals in multiples of 20 ms.
                accelerometer.TimeBetweenUpdates = TimeSpan.FromMilliseconds(200);

                // The sensor may not support the requested time between updates.
                // The TimeBetweenUpdates property reflects the actual rate.
                timeBetweenUpdatesTextBlock.Text = accelerometer.TimeBetweenUpdates.TotalMilliseconds + " ms";


                accelerometer.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(accelerometer_CurrentValueChanged);
            }

            try
            {
                statusTextBlock.Text = "starting accelerometer.";
                accelerometer.Start();
                timer.Start();
            }
            catch (InvalidOperationException)
            {
                statusTextBlock.Text = "unable to start accelerometer.";
            }



//            string deviceId = Convert.ToBase64String((byte[])DeviceExtendedProperties.GetValue("DeviceUniqueId"));
//            string deviceName = DeviceStatus.DeviceName.ToString();
//            Debug.WriteLine("DeviceID=" + deviceId);
//            Debug.WriteLine("DeviceName=" + deviceName);
//            DeviceInfo deviceInfo = new DeviceInfo();
//            deviceInfo.DeviceId = deviceId;
//            deviceInfo.DeviceName = deviceName;
//            communicator = new Communicator(deviceInfo);
//            communicator.SearchComputers();
        }

        void accelerometer_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> e)
        {
            // Note that this event handler is called from a background thread
            // and therefore does not have access to the UI thread. To update 
            // the UI from this handler, use Dispatcher.BeginInvoke() as shown.
            // Dispatcher.BeginInvoke(() => { statusTextBlock.Text = "in CurrentValueChanged"; });


            isDataValid = accelerometer.IsDataValid;

            acceleration = e.SensorReading.Acceleration;
        }

        void timer_Tick(object sender, EventArgs e)
        {
            if (isDataValid)
            {
                statusTextBlock.Text = "receiving data from accelerometer.";

                // Show the numeric values
                xTextBlock.Text = "X: " + acceleration.X.ToString("0.00");
                yTextBlock.Text = "Y: " + acceleration.Y.ToString("0.00");
                zTextBlock.Text = "Z: " + acceleration.Z.ToString("0.00");

                MainPage.communicator.SendMessageThroughUDP(acceleration.X.ToString("0.00") +
                                                   ":" + acceleration.Y.ToString("0.00") +
                                                   ":" + acceleration.Z.ToString("0.00"));

                // Show the values graphically
                xLine.X2 = xLine.X1 + acceleration.X * 100;
                yLine.Y2 = yLine.Y1 - acceleration.Y * 100;
                zLine.X2 = zLine.X1 - acceleration.Z * 50;
                zLine.Y2 = zLine.Y1 + acceleration.Z * 50;
            }
        }
    }
}