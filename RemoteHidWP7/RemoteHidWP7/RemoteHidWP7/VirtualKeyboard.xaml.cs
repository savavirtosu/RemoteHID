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

namespace RemoteHidWP7
{
    public partial class VirtualKeyboard : PhoneApplicationPage
    {
        private Boolean focused = false;
        public VirtualKeyboard()
        {
            InitializeComponent();
            textBox1.Focus();
        }

        private void TextBox1_OnKeyDown(object sender, KeyEventArgs e)
        {
            Debug.WriteLine(e.Key+":"+e.PlatformKeyCode);
            MainPage.communicator.SendMessageThroughUDP("Key:"+e.Key+":"+e.PlatformKeyCode);
            textBox1.Text = "";
            //throw new NotImplementedException();
        }

        private void button1_Click(object sender, RoutedEventArgs e)
        {
            if (!focused)
            {
                textBox1.Focus();
                Debug.WriteLine("TAP-Keyboard");
                button1.Content = "Hide Keyboard";
                focused = true;
                //textBox1.Visibility = Visibility.Collapsed;
            }
            else
            {
                button1.Focus();
                Debug.WriteLine("TAP-NoKeyboard");
                button1.Content = "Show Keyboard";
                focused = false;
                //textBox1.Visibility = Visibility.Visible;
            }
        }
    }
}