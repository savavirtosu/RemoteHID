using System;
using System.Collections.Generic;
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

namespace RemoteHidWP7
{
    public partial class TouchWindow : PhoneApplicationPage
    {
        public TouchWindow()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            MainPage.communicator.SendMessageThroughUDP("PowerPoint:start");
        }

        private void button2_Click(object sender, RoutedEventArgs e)
        {
            MainPage.communicator.SendMessageThroughUDP("PowerPoint:stop");
        }

        private void button3_Click(object sender, RoutedEventArgs e)
        {
            MainPage.communicator.SendMessageThroughUDP("PowerPoint:previous");
        }

        private void button4_Click(object sender, RoutedEventArgs e)
        {
            MainPage.communicator.SendMessageThroughUDP("PowerPoint:next");
        }
    }
}