package org.remotehid.clientcommunicator;

public class CommunicatorException {
	private String _message;
	
	public CommunicatorException(String message) {
		this._message = message;
	}

	public String get_message() {
		return _message;
	}

	public void set_message(String _message) {
		this._message = _message;
	}

}
