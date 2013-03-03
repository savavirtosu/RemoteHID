package org.remotehid.remotehidandroid;

import android.app.Activity;
import android.os.Bundle;
import android.view.View;
import android.view.View.OnClickListener;
import android.widget.Button;

public class RemoteHidActivity  extends Activity{
	
	@Override
	public void onCreate(Bundle savedInstanceState) {
	   super.onCreate(savedInstanceState);
	   setContentView(R.layout.main);
	   
	   Button search = (Button) findViewById(R.id.search_computer);
	   search.setOnClickListener(new OnClickListener() {
		
		public void onClick(View v) {
			setContentView(R.layout.accelerometer_view);
			
		}
	});
	}
}