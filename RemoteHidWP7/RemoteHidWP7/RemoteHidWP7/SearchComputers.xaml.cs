using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Info;

namespace RemoteHidWP7
{
    public partial class SearchComputers : PhoneApplicationPage
    {
        private BackgroundWorker backgroundWorker;
        private List<KeyValuePair<string, string>> listOfAvalableApplication; 
        public SearchComputers()
        {
            InitializeComponent();
            progressBar1.IsIndeterminate = true;
            backgroundWorker = new BackgroundWorker();
            RunBackgroundWorker();
        }
        private void RunBackgroundWorker()
        {
            backgroundWorker.DoWork += ((s, args) =>
                                            {
                                                startSearchingComputers();
                                            });

            backgroundWorker.RunWorkerCompleted += ((s, args) =>
            {
                this.Dispatcher.BeginInvoke(ShowListOfComputers);
            });
            backgroundWorker.RunWorkerAsync();
        }

        private void startSearchingComputers()
        {
            string deviceId = Convert.ToBase64String((byte[])DeviceExtendedProperties.GetValue("DeviceUniqueId"));
            string deviceName = DeviceStatus.DeviceName.ToString();
            Debug.WriteLine("DeviceID=" + deviceId);
            Debug.WriteLine("DeviceName=" + deviceName);
            DeviceInfo deviceInfo = new DeviceInfo();
            deviceInfo.DeviceId = deviceId;
            deviceInfo.DeviceName = deviceName;
            MainPage.communicator = new Communicator(deviceInfo);
            listOfAvalableApplication = MainPage.communicator.SearchComputers();
        }

        private void ShowListOfComputers()
        {
            progressBar1.Visibility = Visibility.Collapsed;
            textBlockSearchComputers.Visibility = Visibility.Collapsed;
            listBox1.ItemsSource = listOfAvalableApplication;
            
        }

        private void listBox1_Tap(object sender, GestureEventArgs e)
        {
            NavigationService.Navigate(new Uri("/AccelerometerWindow.xaml", UriKind.Relative));
        }
    }
}