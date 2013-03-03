package org.remotehid.clientcommunicator;

import java.net.Socket;
import java.util.Map;

import android.util.Log;

public class Communicator {
	private Socket _udpSocket;
    private Socket _tcpSocket;
    private DeviceInfo _deviceInfo;
    final int TIMEOUT_MILLISECONDS = 5000;
    final int MAX_BUFFER_SIZE = 2048;
    private final int UDP_PORT = 7282;
    private final int TCP_PORT = 7283;
    private final int UDP_ECHO_PORT = 7284;
//    private IPAddress _ipOfServer;
	
	public Communicator(DeviceInfo deviceInfo)
    {
        _deviceInfo = deviceInfo;
    }
	
	Map SearchComputers() {
		
		List<KeyValuePair<string, string>> listOfAvalableApplication =  new List<KeyValuePair<string, string>>();
        _udpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
        _tcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); 
        
        try
        {
            SendMessageThroughUDP(_deviceInfo.getDeviceName(), UDP_ECHO_PORT);
            Log.d(this.getClass().getSimpleName(),"FROM Server:"+ReceiveMessageThroughUDP(UDP_ECHO_PORT));
            SendMessageThroughUDP(_deviceInfo.getDeviceId() + " throw UDP");
            Log.d(this.getClass().getSimpleName(),"FROM Server:" + ReceiveMessageThroughUDP());

            SendMessageThroughTCP(_deviceInfo.getDeviceName()+" throw tcp");
            Log.d(this.getClass().getSimpleName(),"FROM Server:" + ReceiveMessageThroughTCP());

            listOfAvalableApplication.Add(new KeyValuePair<string, string>(_ipOfServer.ToString(), "Power Point"));
            listOfAvalableApplication.Add(new KeyValuePair<string, string>(_ipOfServer.ToString(), "Virtual Keyboard"));
            listOfAvalableApplication.Add(new KeyValuePair<string, string>(_ipOfServer.ToString(), "Racer Game"));
            
        }catch(CommunicatorException e)
        {
            Log.d(this.getClass().getSimpleName(),"ERROR:"+e.get_message());
        }
        return listOfAvalableApplication;
	}

	private void SendMessageThroughTCP(String string) {
		// TODO Auto-generated method stub
		
	}

	private String ReceiveMessageThroughTCP() {
		// TODO Auto-generated method stub
		return null;
	}

	private void SendMessageThroughUDP(String string) {
		// TODO Auto-generated method stub
		
	}

	private String ReceiveMessageThroughUDP() {
		// TODO Auto-generated method stub
		return null;
	}

	private void SendMessageThroughUDP(String deviceName, int uDP_ECHO_PORT2) {
		// TODO Auto-generated method stub
		
	}

	private String ReceiveMessageThroughUDP(int uDP_ECHO_PORT2) {
		// TODO Auto-generated method stub
		return null;
	}

}
