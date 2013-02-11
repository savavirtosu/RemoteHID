using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Info;

namespace RemoteHidWP7
{
    public class Communicator
    {
        private Socket _udpSocket;
        private Socket _tcpSocket;
        private DeviceInfo _deviceInfo;
        static ManualResetEvent _clientDone = new ManualResetEvent(false);
        const int TIMEOUT_MILLISECONDS = 5000;
        const int MAX_BUFFER_SIZE = 2048;
        private const int UDP_PORT = 7282;
        private const int TCP_PORT = 7283;
        private const int UDP_ECHO_PORT = 7284;
        private IPAddress _ipOfServer;

        public Communicator(DeviceInfo deviceInfo)
        {
            _deviceInfo = deviceInfo;
        }
        //returning dictionary contain as a 
        public List<KeyValuePair<string,string>> SearchComputers()
        {
            List<KeyValuePair<string, string>> listOfAvalableApplication =  new List<KeyValuePair<string, string>>();
            _udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            _tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); 
            
            try
            {
                SendMessageThroughUDP(_deviceInfo.DeviceName, UDP_ECHO_PORT);
                Debug.WriteLine("FROM Server:"+ReceiveMessageThroughUDP(UDP_ECHO_PORT));

                SendMessageThroughUDP(_deviceInfo.DeviceId + " throw UDP");
                Debug.WriteLine("FROM Server:" + ReceiveMessageThroughUDP());

                SendMessageThroughTCP(_deviceInfo.DeviceName+" throw tcp");
                Debug.WriteLine("FROM Server:" + ReceiveMessageThroughTCP());

                listOfAvalableApplication.Add(new KeyValuePair<string, string>(_ipOfServer.ToString(), "Power Point"));
                listOfAvalableApplication.Add(new KeyValuePair<string, string>(_ipOfServer.ToString(), "Virtual Keyboard"));
                listOfAvalableApplication.Add(new KeyValuePair<string, string>(_ipOfServer.ToString(), "Racer Game"));
                
            }catch(CommunicatorException e)
            {
                Debug.WriteLine("ERROR:"+e.Message);
            }
            return listOfAvalableApplication;
        }

        private string ReceiveMessageThroughTCP()
        {
            byte[] data = ReceiveDataThroughTCP(new IPEndPoint(_ipOfServer, TCP_PORT));
            return Encoding.UTF8.GetString(data, 0, data.Length);
        }

        private string ReceiveMessageThroughUDP(int port = UDP_PORT)
        {
            byte[] data = ReceiveDataThroughUDP(port);
            return Encoding.UTF8.GetString(data, 0, data.Length);
            
        }

        public void SendMessageThroughUDP(string message, int port = UDP_PORT)
        {
            if (_ipOfServer == null)
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                SendDataThroughUDP(data, new IPEndPoint(IPAddress.Broadcast, port));
            }
            else
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                SendDataThroughUDP(data, new IPEndPoint(_ipOfServer, port));
            }
        }

        private void SendMessageThroughTCP(string message)
        {
            if (_ipOfServer == null)
            {
                throw new CommunicatorException("Error 134: IP is not initilized");
            }
            else
            {
                byte[] data = Encoding.UTF8.GetBytes(message);
                SendDataThroughTCP(data, new IPEndPoint(_ipOfServer, TCP_PORT));
            }
        }

        private void SendDataThroughUDP(byte[] data, IPEndPoint ipEndPoint)
        {
            try
            {
                SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();

                // Set properties on context object
                socketEventArg.RemoteEndPoint = ipEndPoint;

                socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate(object s, SocketAsyncEventArgs e)
                {
                    // Unblock the UI thread
                    _clientDone.Set();
                });

                // Add the data to be sent into the buffer
                socketEventArg.SetBuffer(data, 0, data.Length);

                // Sets the state of the event to nonsignaled, causing threads to block
                _clientDone.Reset();

                // Make an asynchronous Send request over the socket
                _udpSocket.SendToAsync(socketEventArg);

                // Block the UI thread for a maximum of TIMEOUT_MILLISECONDS milliseconds.
                // If no response comes back within this time then proceed
                _clientDone.WaitOne(TIMEOUT_MILLISECONDS);
            }
            catch (Exception e)
            {
                throw new CommunicatorException(e.Message);
            }
        }
        private byte[] ReceiveDataThroughUDP(int port)
        {
            byte[] data = Encoding.UTF8.GetBytes("Operation Timeout");
            // Create SocketAsyncEventArgs context object
            SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
            socketEventArg.RemoteEndPoint = new IPEndPoint(IPAddress.Any, port);

            // Setup the buffer to receive the data
            socketEventArg.SetBuffer(new Byte[MAX_BUFFER_SIZE], 0, MAX_BUFFER_SIZE);

            // Inline event handler for the Completed event.
            // Note: This even handler was implemented inline in order to make this method self-contained.
            socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate(object s, SocketAsyncEventArgs e)
            {
                if (e.SocketError == SocketError.Success)
                {
                    _ipOfServer = ((IPEndPoint)e.RemoteEndPoint).Address;
                    // Retrieve the data from the buffer
                    data = new byte[e.BytesTransferred];
                    data = e.Buffer;
//                    response = Encoding.UTF8.GetString(e.Buffer, e.Offset, e.BytesTransferred);
//                    response = response.Trim('\0');
                }
                else
                {
                    data = Encoding.UTF8.GetBytes(e.SocketError.ToString());
                }

                _clientDone.Set();
            });

            // Sets the state of the event to nonsignaled, causing threads to block
            _clientDone.Reset();

            // Make an asynchronous Receive request over the socket
            _udpSocket.ReceiveFromAsync(socketEventArg);
            
            // Block the UI thread for a maximum of TIMEOUT_MILLISECONDS milliseconds.
            // If no response comes back within this time then proceed
            _clientDone.WaitOne(TIMEOUT_MILLISECONDS);
            return data;
        }

        private void SendDataThroughTCP(byte[] data, IPEndPoint ipEndPoint)
        {
             try
            {
                SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();

                // Set properties on context object
                socketEventArg.RemoteEndPoint = ipEndPoint;

                socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate(object s, SocketAsyncEventArgs e)
                {
                    // Unblock the UI thread
                    _clientDone.Set();
                });
                
                socketEventArg.SetBuffer(data, 0, data.Length);

                // Sets the state of the event to nonsignaled, causing threads to block
                _clientDone.Reset();

                // Make an asynchronous Send request over the socket
                _tcpSocket.ConnectAsync(socketEventArg);// SendToAsync(socketEventArg);

                // Block the UI thread for a maximum of TIMEOUT_MILLISECONDS milliseconds.
                // If no response comes back within this time then proceed
                _clientDone.WaitOne(TIMEOUT_MILLISECONDS);

            }
            catch (Exception e)
            {
                throw new CommunicatorException(e.Message);
            }
        }
        private byte[] ReceiveDataThroughTCP(IPEndPoint ipEndPoint)
        {
            byte[] data = Encoding.UTF8.GetBytes("Operation Timeout");
            // Create SocketAsyncEventArgs context object
            SocketAsyncEventArgs socketEventArg = new SocketAsyncEventArgs();
            socketEventArg.RemoteEndPoint = ipEndPoint;

            // Setup the buffer to receive the data
            socketEventArg.SetBuffer(new Byte[MAX_BUFFER_SIZE], 0, MAX_BUFFER_SIZE);

            // Inline event handler for the Completed event.
            // Note: This even handler was implemented inline in order to make this method self-contained.
            socketEventArg.Completed += new EventHandler<SocketAsyncEventArgs>(delegate(object s, SocketAsyncEventArgs e)
            {
                if (e.SocketError == SocketError.Success)
                {
                    // Retrieve the data from the buffer
                    data = new byte[e.BytesTransferred];
                    data = e.Buffer;
                    //                    response = Encoding.UTF8.GetString(e.Buffer, e.Offset, e.BytesTransferred);
                    //                    response = response.Trim('\0');
                }
                else
                {
                    data = Encoding.UTF8.GetBytes(e.SocketError.ToString());
                }

                _clientDone.Set();
            });

            // Sets the state of the event to nonsignaled, causing threads to block
            _clientDone.Reset();

            // Make an asynchronous Receive request over the socket
            _tcpSocket.ReceiveAsync(socketEventArg);

            // Block the UI thread for a maximum of TIMEOUT_MILLISECONDS milliseconds.
            // If no response comes back within this time then proceed
            _clientDone.WaitOne(TIMEOUT_MILLISECONDS);
            return data;
        }
    }
}
