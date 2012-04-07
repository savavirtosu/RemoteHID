using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Text;
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
        private UdpAnySourceMulticastClient _udpSocket;
        private DeviceInfo _deviceInfo;

        public Communicator(DeviceInfo deviceInfo)
        {
            _deviceInfo = deviceInfo;
        }
        //returning dictionary contain as a 
        public List<KeyValuePair<string,string>> SearchComputers()
        {
            _udpSocket = new UdpAnySourceMulticastClient(IPAddress.Broadcast,7282);
            
            try
            {
                SendMessageThroughUDP(_deviceInfo.DeviceName, new IPEndPoint(IPAddress.Broadcast, 7282));
            }catch(CommunicatorException e)
            {
                Debug.WriteLine(e.Message);
            }
            return null;

        }

        private void SendDataThroughUDP(byte[] data, IPEndPoint ipEndPoint)
        {
            try
            {
                Int32 length = data.Length;
                byte[] intBytes = BitConverter.GetBytes(length);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(intBytes);
                }
                byte[] result = intBytes;
                _udpSocket.BeginSendTo(result, 0, 4,ipEndPoint,null,null);
                _udpSocket.BeginSendTo(data, 0, length,ipEndPoint,null,null);
            }
            catch (Exception e)
            {
                throw new CommunicatorException(e.Message);
            }
        }
        private void SendMessageThroughUDP(string message,IPEndPoint ipEndPoint)
        {
            try
            {
                Debug.WriteLine("111");
                UTF8Encoding utf = new UTF8Encoding();
                Debug.WriteLine("222");
                Int32 length = utf.GetByteCount(message);
                Debug.WriteLine("333");
                byte[] intBytes = BitConverter.GetBytes(length);
                if (BitConverter.IsLittleEndian)
                {
                    Array.Reverse(intBytes);
                }
                Debug.WriteLine("444");
                byte[] result = intBytes;
                Debug.WriteLine("555");
                _udpSocket.BeginSendTo(result, 0, 4,ipEndPoint,null,null);
                Debug.WriteLine("666");
                _udpSocket.BeginSendTo(utf.GetBytes(message), 0, length, ipEndPoint, null, null);
            }
            catch (Exception e)
            {
                throw new CommunicatorException(e.Message);
            }
        }
        private string ReceiveMessageThroughUDP()
        {
//            try
//            {
//                byte[] lenghtInBytes = new byte[4];
//                sslStream.Read(lenghtInBytes, 0, lenghtInBytes.Length);
//                if (BitConverter.IsLittleEndian)
//                {
//                    Array.Reverse(lenghtInBytes);
//                }
//                int lenght = BitConverter.ToInt32(lenghtInBytes, 0);
//                //alternative way to do the same thing:
//                //int lenght = lenghtInBytes[0] << 24 | lenghtInBytes[1] << 16 | lenghtInBytes[2] << 8 | lenghtInBytes[3];
//                byte[] data = new byte[lenght];
//                sslStream.Read(data, 0, data.Length);
//                UTF8Encoding utf = new UTF8Encoding();
//                return utf.GetString(data);
//            }
//            catch (Exception e)
//            {
//                throw new SecureCommunicatorException(e.Message);
//            }
            return null;
        }
        private byte[] ReceiveDataThroughUDP()
        {
//            try
//            {
//                byte[] lenghtInBytes = new byte[4];
//                sslStream.Read(lenghtInBytes, 0, lenghtInBytes.Length);
//                if (BitConverter.IsLittleEndian)
//                {
//                    Array.Reverse(lenghtInBytes);
//                }
//                int lenght = BitConverter.ToInt32(lenghtInBytes, 0);
//                //alternative way to do the same thing:
//                //int lenght = lenghtInBytes[0] << 24 | lenghtInBytes[1] << 16 | lenghtInBytes[2] << 8 | lenghtInBytes[3];
//                byte[] data = new byte[lenght];
//                sslStream.Read(data, 0, data.Length);
//                return data;
//            }
//            catch (Exception e)
//            {
//                throw new SecureCommunicatorException(e.Message);
//            }
            return null;

        }
        private void SendDataThroughTCP(byte[] data)
        {
//            try
//            {
//                Int32 length = data.Length;
//                byte[] intBytes = BitConverter.GetBytes(length);
//                if (BitConverter.IsLittleEndian)
//                {
//                    Array.Reverse(intBytes);
//                }
//                byte[] result = intBytes;
//                sslStream.Write(result, 0, 4);
//                sslStream.Write(data, 0, length);
//                sslStream.Flush();
//            }
//            catch (Exception e)
//            {
//                throw new SecureCommunicatorException(e.Message);
//            }
        }
        private byte[] ReceiveDataThroughTCP()
        {
//            try
//            {
//                byte[] lenghtInBytes = new byte[4];
//                sslStream.Read(lenghtInBytes, 0, lenghtInBytes.Length);
//                if (BitConverter.IsLittleEndian)
//                {
//                    Array.Reverse(lenghtInBytes);
//                }
//                int lenght = BitConverter.ToInt32(lenghtInBytes, 0);
//                //alternative way to do the same thing:
//                //int lenght = lenghtInBytes[0] << 24 | lenghtInBytes[1] << 16 | lenghtInBytes[2] << 8 | lenghtInBytes[3];
//                byte[] data = new byte[lenght];
//                sslStream.Read(data, 0, data.Length);
//                return data;
//            }
//            catch (Exception e)
//            {
//                throw new SecureCommunicatorException(e.Message);
//            }
            return null;
        }
    }
}
