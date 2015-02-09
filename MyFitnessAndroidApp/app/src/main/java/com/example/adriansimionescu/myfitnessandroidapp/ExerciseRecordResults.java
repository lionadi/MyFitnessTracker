package com.example.adriansimionescu.myfitnessandroidapp;

import android.content.Intent;
import android.os.SystemClock;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;

import java.util.Calendar;


public class ExerciseRecordResults extends ActionBarActivity {

    Button bGoBack;
    TextView tvRecordResults;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_exercise_record_results);

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
            } else
            {
                tvRecordResults.setText(""+ UserDataContainer.CurrentExerciseRecord.Record);
            }

        }

        bGoBack.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                UserDataContainer.CurrentExerciseRecordGEOLocationAttribute = null;
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
