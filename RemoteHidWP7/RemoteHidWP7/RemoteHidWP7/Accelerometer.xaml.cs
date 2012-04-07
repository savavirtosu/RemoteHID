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
using Microsoft.Phone.Controls;
using Microsoft.Internal;
using Microsoft.Phone.Info;

namespace RemoteHidWP7
{
    public partial class Accelerometer : PhoneApplicationPage
    {
        public Accelerometer()
        {
            InitializeComponent();
            Init();
        }
        public void Init()
        {
            string deviceId = Convert.ToBase64String((byte[])DeviceExtendedProperties.GetValue("DeviceUniqueId"));
            string deviceName = DeviceStatus.DeviceName.ToString();
            Debug.WriteLine("DeviceID=" + deviceId);
            Debug.WriteLine("DeviceName=" + deviceName);
            DeviceInfo deviceInfo = new DeviceInfo();
            deviceInfo.DeviceId = deviceId;
            deviceInfo.DeviceName = deviceName;
            Communicator communicator = new Communicator(deviceInfo);
            communicator.SearchComputers();
            PageTitle.Text = "here you go";

        }
    }
}