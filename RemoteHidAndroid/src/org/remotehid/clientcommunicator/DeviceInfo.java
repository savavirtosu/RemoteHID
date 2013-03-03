package org.remotehid.clientcommunicator;

import java.util.Dictionary;
import java.util.List;

public class DeviceInfo {
	
	private String DeviceId;
    private String DeviceName;
    private Dictionary<String, String> DeviceScreenDetails;
    private List<String> DeviceInterfaces;
    
	public String getDeviceId() {
		return DeviceId;
	}
	public void setDeviceId(String deviceId) {
		DeviceId = deviceId;
	}
	public String getDeviceName() {
		return DeviceName;
	}
	public void setDeviceName(String deviceName) {
		DeviceName = deviceName;
	}
	public List<String> getDeviceInterfaces() {
		return DeviceInterfaces;
	}
	public void setDeviceInterfaces(List<String> deviceInterfaces) {
		DeviceInterfaces = deviceInterfaces;
	}
	public Dictionary<String, String> getDeviceScreenDetails() {
		return DeviceScreenDetails;
	}
	public void setDeviceScreenDetails(Dictionary<String, String> deviceScreenDetails) {
		DeviceScreenDetails = deviceScreenDetails;
	}

}
