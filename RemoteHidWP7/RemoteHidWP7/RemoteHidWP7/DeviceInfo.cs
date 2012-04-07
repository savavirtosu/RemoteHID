using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace RemoteHidWP7
{
    public class DeviceInfo
    {
        public string DeviceId { get; set; }
        public string DeviceName { get; set; }
        public Dictionary<string, string> DeviceScreenDetails { get; set; }
        public List<string> DeviceInterfaces { get; set; }

    }
}
