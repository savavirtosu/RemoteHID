package org.remotehid.remotehidandroid;

import android.app.Activity;
import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.hardware.SensorManager;
import android.os.Bundle;
import android.widget.TextView;

public class AccelerometerView extends Activity implements SensorEventListener {
	SensorManager sensorManager = null;
	 
	//for accelerometer values
	TextView outputAccelerometer_X;
	TextView outputAccelerometer_Y;
	TextView outputAccelerometer_Z;
	 
	//for orientation values
	TextView outputOrientation_X;
	TextView outputOrientation_Y;
	TextView outputOrientation_Z;
	 
	 @Override
	 public void onCreate(Bundle savedInstanceState) {
	    super.onCreate(savedInstanceState);
	    sensorManager = (SensorManager) getSystemService(SENSOR_SERVICE);
	 
	    //just some textviews, for data output
	    outputAccelerometer_X = (TextView) findViewById(R.id.textViewAcceleration_x);
	    outputAccelerometer_Y = (TextView) findViewById(R.id.textViewAcceleration_y);
	    outputAccelerometer_Z = (TextView) findViewById(R.id.textViewAcceleration_z);
	 
	    outputOrientation_X = (TextView) findViewById(R.id.textViewOrientation_x);
	    outputOrientation_Y = (TextView) findViewById(R.id.textViewOrientation_y);
	    outputOrientation_Z = (TextView) findViewById(R.id.textViewOrientation_z);
	 }
	 
	 @Override
	 protected void onResume() {
	    super.onResume();
	    sensorManager.registerListener(this, sensorManager.getDefaultSensor(Sensor.TYPE_ACCELEROMETER), SensorManager.SENSOR_DELAY_GAME);
	    sensorManager.registerListener(this, sensorManager.getDefaultSensor(Sensor.TYPE_ORIENTATION), SensorManager.SENSOR_DELAY_GAME);
	 }
	 
	 @Override
	 protected void onStop() {
	    super.onStop();
	    sensorManager.unregisterListener(this, sensorManager.getDefaultSensor(Sensor.TYPE_ACCELEROMETER));
	    sensorManager.unregisterListener(this, sensorManager.getDefaultSensor(Sensor.TYPE_ORIENTATION));
	 }
	 
	 public void onSensorChanged(SensorEvent event) {
		    synchronized (this) {
		        switch (event.sensor.getType()){
		            case Sensor.TYPE_ACCELEROMETER:
		                outputAccelerometer_X.setText("x:"+Float.toString(event.values[0]));
		                outputAccelerometer_Y.setText("y:"+Float.toString(event.values[1]));
		                outputAccelerometer_Z.setText("z:"+Float.toString(event.values[2]));
		            break;
		        case Sensor.TYPE_ORIENTATION:
		                outputOrientation_X.setText("x:"+Float.toString(event.values[0]));
		                outputOrientation_Y.setText("y:"+Float.toString(event.values[1]));
		                outputOrientation_Z.setText("z:"+Float.toString(event.values[2]));
		        break;
		 
		        }
		    }
		 }

	public void onAccuracyChanged(Sensor sensor, int accuracy) {
		// TODO Auto-generated method stub
		
	}

}
