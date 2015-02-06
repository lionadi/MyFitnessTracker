package com.example.adriansimionescu.myfitnessandroidapp;

import android.animation.Animator;
import android.animation.AnimatorListenerAdapter;
import android.annotation.TargetApi;
import android.app.Activity;
import android.content.Intent;
import android.content.res.Resources;
import android.os.AsyncTask;
import android.os.Build;
import android.os.SystemClock;
import android.support.v7.app.ActionBarActivity;
import android.os.Bundle;
import android.view.Menu;
import android.view.MenuItem;
import android.view.View;
import android.widget.AdapterView;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.Chronometer;
import android.widget.EditText;
import android.widget.RadioGroup;
import android.widget.Spinner;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.Calendar;
import java.util.Collections;
import java.util.List;


public class MainActivity extends ActionBarActivity {

    private View mProgressView;
    private boolean timerStarted = false;

    Button activityAction;
    Chronometer chronometer;
    EditText activityStatusInfo;
    Spinner spinner;
    Spinner spinnerExercise;
    RadioGroup radioGroup;
    Button bLogOut;

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

        activityAction = (Button) findViewById(R.id.bActivityAction);
        chronometer = (Chronometer) findViewById(R.id.chronometer);
        activityStatusInfo = (EditText) findViewById(R.id.etActivityStatusInfo);
        spinner = (Spinner) findViewById(R.id.sSets);
        spinnerExercise = (Spinner) findViewById(R.id.sExercises);
        radioGroup = (RadioGroup) findViewById(R.id.radioGroup);
        bLogOut = (Button) findViewById(R.id.bLogOut);

        //mLoginFormView = findViewById(R.id.login_form);
        mProgressView = findViewById(R.id.login_progress);

        showProgress(true);


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
                showProgress(false);
            }

            @Override
            protected void onCancelled() {
                Intent intent = new Intent(MainActivity.this, LoginActivity.class);
                startActivity(intent);
                showProgress(false);
            }

        }.execute();



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


        spinnerExercise.setOnItemSelectedListener(new AdapterView.OnItemSelectedListener() {

            @Override
            public void onItemSelected(AdapterView<?> arg0, View arg1,
                                       int pos, long id) {
                radioGroup.setVisibility(View.VISIBLE);

                activityAction.setVisibility(View.VISIBLE);

                activityStatusInfo.setVisibility(View.VISIBLE);
            }
            @Override
            public void onNothingSelected(AdapterView<?> parent) {
                // Another interface callback
            }
        });


        if(radioGroup != null) {
            radioGroup.setOnCheckedChangeListener(new RadioGroup.OnCheckedChangeListener() {
                @Override
                public void onCheckedChanged(RadioGroup group, int checkedId) {

                    if (checkedId == R.id.SingleRecord) {
                        activityStatusInfo.setVisibility(View.VISIBLE);
                        chronometer.setVisibility(View.INVISIBLE);
                        activityAction.setText(R.string.AddExerciseRecord);
                        activityStatusInfo.setText(R.string.ActivityRecordTextBoxHintMessage);
                        chronometer.stop();
                        chronometer.setBase(SystemClock.elapsedRealtime());
                    } else if (checkedId == R.id.Timer) {
                        activityStatusInfo.setVisibility(View.INVISIBLE);
                        chronometer.setVisibility(View.VISIBLE);
                        activityAction.setText(R.string.Start);
                        timerStarted = false;
                    }
                }
            });
        }


        activityAction.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {

                int checkedId = radioGroup.getCheckedRadioButtonId();
                if (checkedId == R.id.SingleRecord) {
                    ExerciseRecord er = new ExerciseRecord();
                    er.StartDate = er.EndDate = er.Date = Calendar.getInstance().getTime();
                    er.Record = Double.parseDouble(activityStatusInfo.getText().toString());
                    er.ExerciseId = Long.parseLong(UserDataContainer.UserSets.get((int)spinner.getSelectedItemId()).Exercises.get((int)spinnerExercise.getSelectedItemId()).ID);
                    JSONHttpClient jsonHttpClient = new JSONHttpClient();
                    er = (ExerciseRecord) jsonHttpClient.PostObject(Constants.WebServiceLocation + "/api" + "todo:relativeURL", er, ExerciseRecord.class);
                    // todo: Intent intent = getIntent();
                    //setResult(ResultCode.PRODUCT_UPDATE_SUCCESS, intent);
                    //finish();

                } else if (checkedId == R.id.Timer) {

                    if(timerStarted == false) {
                        chronometer.setBase(SystemClock.elapsedRealtime());
                        chronometer.start();
                        activityAction.setText(R.string.Stop);
                        timerStarted = true;
                    } else {
                        chronometer.stop();
                        long elapsedMillis = SystemClock.elapsedRealtime() - chronometer.getBase();
                        long seconds = elapsedMillis / 1000;
                        int minutes = (int)seconds / 60;
                        int hours = minutes / 60;
                        timerStarted = false;
                        activityAction.setText(R.string.Start);
                    }
                }
            }
        });


        bLogOut.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Calendar cal = Calendar.getInstance();
                if(UserDataContainer.LoginData.ExpiresAsDate.after(cal.getTime()) == false) {
                    DeviceDataStorage.RemoveFileFromDeviceStorage(Constants.UserLoginData_FileName);
                }
                Intent intent = new Intent(MainActivity.this, LoginActivity.class);
                startActivity(intent);
                showProgress(false);
            }
        });

    }

    // TODO: Optimize but moving inside a panel type
    @TargetApi(Build.VERSION_CODES.HONEYCOMB_MR2)
    public void showProgress(final boolean show) {
        // On Honeycomb MR2 we have the ViewPropertyAnimator APIs, which allow
        // for very easy animations. If available, use these APIs to fade-in
        // the progress spinner.
        if (Build.VERSION.SDK_INT >= Build.VERSION_CODES.HONEYCOMB_MR2) {
            int shortAnimTime = getResources().getInteger(android.R.integer.config_shortAnimTime);

            /*mLoginFormView.setVisibility(show ? View.GONE : View.VISIBLE);
            mLoginFormView.animate().setDuration(shortAnimTime).alpha(
                    show ? 0 : 1).setListener(new AnimatorListenerAdapter() {
                @Override
                public void onAnimationEnd(Animator animation) {
                    mLoginFormView.setVisibility(show ? View.GONE : View.VISIBLE);
                }
            });*/

/*

Button activityAction = (Button) findViewById(R.id.bActivityAction);
    Chronometer chronometer = (Chronometer) findViewById(R.id.chronometer);
    EditText activityStatusInfo = (EditText) findViewById(R.id.etActivityStatusInfo);
    Spinner spinner = (Spinner) findViewById(R.id.sSets);
    Spinner spinnerExercise = (Spinner) findViewById(R.id.sExercises);
    final RadioGroup radioGroup = (RadioGroup) findViewById(R.id.radioGroup);

 */
            activityAction.setVisibility(show ? View.GONE : View.VISIBLE);
            activityAction.animate().setDuration(shortAnimTime).alpha(
                    show ? 0 : 1).setListener(new AnimatorListenerAdapter() {
                @Override
                public void onAnimationEnd(Animator animation) {
                    activityAction.setVisibility(show ? View.GONE : View.VISIBLE);
                }
            });

            /*chronometer.setVisibility(show ? View.GONE : View.VISIBLE);
            chronometer.animate().setDuration(shortAnimTime).alpha(
                    show ? 0 : 1).setListener(new AnimatorListenerAdapter() {
                @Override
                public void onAnimationEnd(Animator animation) {
                    chronometer.setVisibility(show ? View.GONE : View.VISIBLE);
                }
            });*/

            activityStatusInfo.setVisibility(show ? View.GONE : View.VISIBLE);
            activityStatusInfo.animate().setDuration(shortAnimTime).alpha(
                    show ? 0 : 1).setListener(new AnimatorListenerAdapter() {
                @Override
                public void onAnimationEnd(Animator animation) {
                    activityStatusInfo.setVisibility(show ? View.GONE : View.VISIBLE);
                }
            });

            spinner.setVisibility(show ? View.GONE : View.VISIBLE);
            spinner.animate().setDuration(shortAnimTime).alpha(
                    show ? 0 : 1).setListener(new AnimatorListenerAdapter() {
                @Override
                public void onAnimationEnd(Animator animation) {
                    spinner.setVisibility(show ? View.GONE : View.VISIBLE);
                }
            });

            spinnerExercise.setVisibility(show ? View.GONE : View.VISIBLE);
            spinnerExercise.animate().setDuration(shortAnimTime).alpha(
                    show ? 0 : 1).setListener(new AnimatorListenerAdapter() {
                @Override
                public void onAnimationEnd(Animator animation) {
                    spinnerExercise.setVisibility(show ? View.GONE : View.VISIBLE);
                }
            });

            radioGroup.setVisibility(show ? View.GONE : View.VISIBLE);
            radioGroup.animate().setDuration(shortAnimTime).alpha(
                    show ? 0 : 1).setListener(new AnimatorListenerAdapter() {
                @Override
                public void onAnimationEnd(Animator animation) {
                    radioGroup.setVisibility(show ? View.GONE : View.VISIBLE);
                }
            });

            bLogOut.setVisibility(show ? View.GONE : View.VISIBLE);
            bLogOut.animate().setDuration(shortAnimTime).alpha(
                    show ? 0 : 1).setListener(new AnimatorListenerAdapter() {
                @Override
                public void onAnimationEnd(Animator animation) {
                    bLogOut.setVisibility(show ? View.GONE : View.VISIBLE);
                }
            });




            mProgressView.setVisibility(show ? View.VISIBLE : View.GONE);
            mProgressView.animate().setDuration(shortAnimTime).alpha(
                    show ? 1 : 0).setListener(new AnimatorListenerAdapter() {
                @Override
                public void onAnimationEnd(Animator animation) {
                    mProgressView.setVisibility(show ? View.VISIBLE : View.GONE);
                }
            });
        } else {
            // The ViewPropertyAnimator APIs are not available, so simply show
            // and hide the relevant UI components.
            mProgressView.setVisibility(show ? View.VISIBLE : View.GONE);
            //mLoginFormView.setVisibility(show ? View.GONE : View.VISIBLE);
        }
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
