using System;
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
    public class CommunicatorException : Exception
    {
        private string _message;
        public CommunicatorException(string message)
        {
            this._message = message;
        }
        public string Message
        {
            get { return _message; }
            set { _message = value; }
        }

    }
}
