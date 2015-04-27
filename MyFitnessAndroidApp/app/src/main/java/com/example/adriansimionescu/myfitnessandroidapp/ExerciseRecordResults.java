package com.example.adriansimionescu.myfitnessandroidapp;

import android.content.Intent;
import android.graphics.Color;
import android.os.SystemClock;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.util.Log;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import java.util.Calendar;
import com.google.android.gms.maps.*;
import com.google.android.gms.maps.model.LatLng;
import com.google.android.gms.maps.model.MarkerOptions;
import com.google.android.gms.maps.model.PolylineOptions;


public class ExerciseRecordResults extends ActionBarActivity {

    Button bGoBack;
    TextView tvRecordResults;
    GoogleMap googleMap;

    private void addMarker(double latitude, double longitude){

        /** Make sure that the map has been initialised **/
        if(null != googleMap){
            googleMap.addMarker(new MarkerOptions()
                            .position(new LatLng(latitude, longitude))
                            .title("Marker")
                            .draggable(true)
            );
        }
    }

    private void createMapView(){
        /**
         * Catch the null pointer exception that
         * may be thrown when initialising the map
         */
        try {
            if(null == googleMap){
                googleMap = ((MapFragment) getFragmentManager().findFragmentById(
                        R.id.mapView)).getMap();

                /**
                 * If the map is still null after attempted initialisation,
                 * show an error to the user
                 */
                if(null == googleMap) {
                    Toast.makeText(getApplicationContext(),
                            "Error creating map", Toast.LENGTH_SHORT).show();
                }
            }
        } catch (NullPointerException exception){
            Log.e("mapApp", exception.toString());
        }
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_exercise_record_results);
        createMapView();

        bGoBack = (Button) findViewById(R.id.bGoBack);
        tvRecordResults = (TextView) findViewById(R.id.tvResults);

        if(UserDataContainer.CurrentExerciseRecord != null)
        {
            if(UserDataContainer.CurrentExerciseRecord.Record > 1000)
            {
                long elapsedMillis = (long)UserDataContainer.CurrentExerciseRecord.Record;
                long seconds = elapsedMillis / 1000;
                int minutes = (int)seconds / 60;
                int hours = minutes / 60;
                tvRecordResults.setText("Hours: " + hours + " Min: " + minutes % 60 + " Sec: " + seconds % 60);
                if(UserDataContainer.CurrentExerciseRecordGEOLocationDataForAttribute != null)
                {
                    googleMap.moveCamera(CameraUpdateFactory.newLatLngZoom(new LatLng(UserDataContainer.CurrentExerciseRecordGEOLocationDataForAttribute.get(0).Latitude, UserDataContainer.CurrentExerciseRecordGEOLocationDataForAttribute.get(0).Longitude), 15));
                    for(int x = 0; x < UserDataContainer.CurrentExerciseRecordGEOLocationDataForAttribute.size(); ++x)
                    {
                        addMarker(UserDataContainer.CurrentExerciseRecordGEOLocationDataForAttribute.get(x).Latitude, UserDataContainer.CurrentExerciseRecordGEOLocationDataForAttribute.get(x).Longitude);
                        if(x < (UserDataContainer.CurrentExerciseRecordGEOLocationDataForAttribute.size()-1)) {
                            PolylineOptions line =
                                    new PolylineOptions().add(new LatLng(UserDataContainer.CurrentExerciseRecordGEOLocationDataForAttribute.get(x).Latitude,
                                                    -UserDataContainer.CurrentExerciseRecordGEOLocationDataForAttribute.get(x).Longitude),
                                            new LatLng(UserDataContainer.CurrentExerciseRecordGEOLocationDataForAttribute.get(x + 1).Latitude,
                                                    -UserDataContainer.CurrentExerciseRecordGEOLocationDataForAttribute.get(x + 1).Longitude)).width(5).color(Color.RED);

                            googleMap.addPolyline(line);
                        }
                    }
                }
            } else
            {
                tvRecordResults.setText(""+ UserDataContainer.CurrentExerciseRecord.Record);
            }

        }

        bGoBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                UserDataContainer.CurrentExerciseRecordGEOLocationAttribute = null;
                UserDataContainer.CurrentExerciseRecordGEOLocationDataForAttribute = null;
                UserDataContainer.CurrentExerciseRecord = null;
                Intent intent = new Intent(ExerciseRecordResults.this, MainActivity.class);
                startActivity(intent);
            }
        });



    }


    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_exercise_record_results, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        // Handle action bar item clicks here. The action bar will
        // automatically handle clicks on the Home/Up button, so long
        // as you specify a parent activity in AndroidManifest.xml.
        int id = item.getItemId();

        //noinspection SimplifiableIfStatement
        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }
}
