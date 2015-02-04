package com.example.adriansimionescu.myfitnessandroidapp;

import android.app.Activity;
import android.content.Intent;
import android.os.AsyncTask;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.RadioGroup;
import android.widget.Spinner;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.Collections;
import java.util.List;


public class MainActivity extends ActionBarActivity {

    private void PopulateSetSpinnerWithServerData()
    {
        if(UserDataContainer.UserSets == null)
            return;

        List<String> spinnerSetsArray = new ArrayList<String>();
        List<String> spinnerExercisesArray = new ArrayList<String>();

        for(int x = 0; x < UserDataContainer.UserSets.size(); x++)
            spinnerSetsArray.add(UserDataContainer.UserSets.get(x).Name);

        // Fill the Sets and Exercises Spinners with the data from the server
        //-------------------------------------------------------------------
        ArrayAdapter<String> adapterSets = new ArrayAdapter<String>(
                MainActivity.this, android.R.layout.simple_spinner_item, spinnerSetsArray);

        adapterSets.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        Spinner spinner = (Spinner) findViewById(R.id.sSets);
        spinner.setAdapter(adapterSets);
        //-------------------------------------------------------------------
    }

    private void PopulateExerciseSpinnerWithServerData(int setIndexInCollection)
    {
        if(UserDataContainer.UserSets == null)
            return;

        List<String> spinnerExercisesArray = new ArrayList<String>();

        for(int x = 0; x < UserDataContainer.UserSets.get(setIndexInCollection).Exercises.size(); x++)
            spinnerExercisesArray.add(UserDataContainer.UserSets.get(setIndexInCollection).Exercises.get(x).Name);

        // Fill the Sets and Exercises Spinners with the data from the server
        //-------------------------------------------------------------------

        ArrayAdapter<String> adapterExercises = new ArrayAdapter<String>(
                MainActivity.this, android.R.layout.simple_spinner_item, spinnerExercisesArray);

        adapterExercises.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
        Spinner spinnerExercises = (Spinner) findViewById(R.id.sExercises);
        spinnerExercises.setAdapter(adapterExercises);
        //-------------------------------------------------------------------
    }

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);



        AsyncTask<Void, Void, Void> asyncTask = new AsyncTask<Void, Void, Void>() {

            @Override
            protected Void doInBackground(Void... params) {
                if(UserDataContainer.LoginData != null) {

                    // Get Server data and parse it
                    //-------------------------------------------------------------------
                    String serverSetsData = WebApiHelper.GetAsJSON("/sets");
                    try {

                        // Start Server Data parsing
                        //-------------------------------------------------------------------

                        // Create an array for easy server data manipulation
                        JSONArray array = new JSONArray(serverSetsData);

                        // Allocate enough memory for the sets
                        if(array.length() > 0)
                            UserDataContainer.UserSets = new ArrayList<UserSetData>(array.length());

                        // Go through the server data(sets)
                        for(int x = 0; x < array.length(); x++) {

                            // Load a user set given from the server
                            UserSetData setData = new UserSetData();
                            JSONObject set = (JSONObject)array.get(x);
                            setData.Name = set.getString("Name");
                            setData.ID = set.getString("Id");

                            // Load the user exercises for a given set
                            JSONArray setExercisesArray = set.getJSONArray("Exercises");
                            setData.Exercises = new ArrayList<UserExerciseData>(setExercisesArray.length());
                            for(int y = 0; y < setExercisesArray.length(); y++) {
                                UserExerciseData exerciseData = new UserExerciseData();
                                JSONObject exercise = (JSONObject)setExercisesArray.get(y);
                                exerciseData.Name = exercise.getString("Name");
                                exerciseData.ID = exercise.getString("Id");

                                // Add the finished Exercise to a set
                                setData.Exercises.add(exerciseData);
                            }
                            // Add a finished Set to the collection of user sets on the server
                            UserDataContainer.UserSets.add(setData);
                        }
                        //-------------------------------------------------------------------
                    } catch (JSONException e) {
                        e.printStackTrace();
                    }
                }

                return null;
            }

            @Override
            protected void onPostExecute(Void none) {
                PopulateSetSpinnerWithServerData();
            }
        }.execute();


        Spinner spinner = (Spinner) findViewById(R.id.sSets);
        spinner.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {

            @Override
            public void onItemSelected(AdapterView<?> arg0, View arg1,
                                       int pos, long id) {
                PopulateExerciseSpinnerWithServerData(pos);
            }
            @Override
            public void onNothingSelected(AdapterView<?> parent) {
                // Another interface callback
            }
        });

        Spinner spinnerExercise = (Spinner) findViewById(R.id.sExercises);
        spinnerExercise.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {

            @Override
            public void onItemSelected(AdapterView<?> arg0, View arg1,
                                       int pos, long id) {

            }
            @Override
            public void onNothingSelected(AdapterView<?> parent) {
                // Another interface callback
            }
        });

        RadioGroup radioGroup = (RadioGroup) findViewById(R.id.radio);
        radioGroup.setOnCheckedChangeListener(new RadioGroup.OnCheckedChangeListener() {
            @Override
            public void onCheckedChanged(RadioGroup group, int checkedId) {

                // find which radio button is selected

                if(checkedId == R.id.SingleRecord) {

                } else if(checkedId == R.id.Timer) {

                }
            }
        });

    }


    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        // Inflate the menu; this adds items to the action bar if it is present.
        getMenuInflater().inflate(R.menu.menu_main, menu);
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
