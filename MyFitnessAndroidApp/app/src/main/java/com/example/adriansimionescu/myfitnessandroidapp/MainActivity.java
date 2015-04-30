package com.example.adriansimionescu.myfitnessandroidapp;

import android.animation.Animator;
import android.animation.AnimatorListenerAdapter;
import android.annotation.TargetApi;
import android.app.Activity;
import android.content.Intent;
import android.content.res.Resources;
import android.location.Location;
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

import com.google.gson.Gson;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.util.ArrayList;
import java.util.Calendar;
import java.util.Collections;
import java.util.List;
import java.util.concurrent.ExecutionException;

import microsoft.aspnet.signalr.client.SignalRFuture;
import microsoft.aspnet.signalr.client.hubs.HubConnection;
import microsoft.aspnet.signalr.client.hubs.HubProxy;
import microsoft.aspnet.signalr.client.hubs.SubscriptionHandler1;

import android.app.AlertDialog;
import android.content.DialogInterface;
import android.os.AsyncTask;
import com.google.android.gms.gcm.*;
import com.microsoft.windowsazure.messaging.*;
import com.microsoft.windowsazure.notifications.NotificationsManager;

//http://azure.microsoft.com/fi-fi/documentation/articles/notification-hubs-android-get-started/

// Todo: improve network traffic usage
public class MainActivity extends ActionBarActivity {

    private View mProgressView;
    private boolean timerStarted = false;
    private RegisterClient registerClient;
    private static final String BACKEND_ENDPOINT = "<Enter Your Backend Endpoint>";

    private String SENDER_ID = "407169728181";
    private GoogleCloudMessaging gcm;
    private NotificationHub hub;
    private String HubName = "fittracker";
    private String HubListenConnectionString = "Endpoint=sb://fittracker-ns.servicebus.windows.net/;SharedAccessKeyName=DefaultListenSharedAccessSignature;SharedAccessKey=s0AifFs0m1gIN02PO7CIfLb9ciy3yeVylGEU2Jfp5yk=";

    Button activityAction;
    public static Chronometer chronometer;
    EditText activityStatusInfo;
    Spinner spinner;
    Spinner spinnerExercise;
    RadioGroup radioGroup;
    Button bLogOut;
    GPSTracker gps;

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

    @SuppressWarnings("unchecked")
    private void registerWithNotificationHubs() {
        new AsyncTask() {
            @Override
            protected Object doInBackground(Object... params) {
                try {
                    String regid = gcm.register(SENDER_ID);
                    DialogNotify("Registered Successfully","RegId : " +
                            hub.register(regid).getRegistrationId());
                } catch (Exception e) {
                    DialogNotify("Exception",e.getMessage());
                    return e;
                }
                return null;
            }
        }.execute(null, null, null);
    }

    /**
     * A modal AlertDialog for displaying a message on the UI thread
     * when theres an exception or message to report.
     *
     * @param title   Title for the AlertDialog box.
     * @param message The message displayed for the AlertDialog box.
     */
    public void DialogNotify(final String title,final String message)
    {
        final AlertDialog.Builder dlg;
        dlg = new AlertDialog.Builder(this);

        runOnUiThread(new Runnable() {
            @Override
            public void run() {
                AlertDialog dlgAlert = dlg.create();
                dlgAlert.setTitle(title);
                dlgAlert.setButton(DialogInterface.BUTTON_POSITIVE,
                        (CharSequence) "OK",
                        new DialogInterface.OnClickListener() {
                            public void onClick(DialogInterface dialog, int which) {
                                dialog.dismiss();
                            }
                        });
                dlgAlert.setMessage(message);
                dlgAlert.setCancelable(false);
                dlgAlert.show();
            }
        });
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
        gps = new GPSTracker(MainActivity.this);

        showProgress(true);

        MyHandler.mainActivity = this;
        NotificationsManager.handleNotifications(this, SENDER_ID, MyHandler.class);
        gcm = GoogleCloudMessaging.getInstance(this);
        hub = new NotificationHub(HubName, HubListenConnectionString, this);
        registerWithNotificationHubs();


        HubConnection connection = new HubConnection( Constants.WebServiceLocation );
        connection.getHeaders().put("Accept", "application/json");
        connection.getHeaders().put("Content-Type", "application/json");
        connection.getHeaders().put("Authorization", UserDataContainer.LoginData.token_type + " " + UserDataContainer.LoginData.access_token);
        HubProxy hub = connection.createHubProxy( "myFitHub" );


        SignalRFuture<Void> awaitConnection = connection.start();


        try {
            awaitConnection.get();
        } catch (InterruptedException e) {
            // Handle ...
            e.printStackTrace();
        } catch (ExecutionException e) {
            // Handle ...
            e.printStackTrace();
        }

        hub.on( "refreshSets",
                new SubscriptionHandler1<String>() {
                    @Override
                    public void run( String status ) {
                        // Since we are updating the UI,
                        // we need to use a handler of the UI thread.
                        String statusa = status;
                    }
                }
                , String.class );

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
                if(!gps.canGetLocation()) {
                    gps.showSettingsAlert();
                }
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

                boolean saveRequested = false;
                int checkedId = radioGroup.getCheckedRadioButtonId();
                UserDataContainer.CurrentExerciseRecord = new ExerciseRecord();
                if (checkedId == R.id.SingleRecord) {

                    UserDataContainer.CurrentExerciseRecord.Record = Double.parseDouble(activityStatusInfo.getText().toString());
                    saveRequested = true;
                } else if (checkedId == R.id.Timer) {

                    if(timerStarted == false) {

                        Location newStartLocation = gps.getLocation();
                        UserDataContainer.CurrentExerciseRecordGEOLocationAttribute = new ExerciseRecordAttribute();
                        UserDataContainer.CurrentExerciseRecordGEOLocationAttribute.Name = Constants.ServerExerciseRecordAttributeName_GEOLocation;
                        UserDataContainer.CurrentExerciseRecordGEOLocationAttribute.Data = new String();
                        UserDataContainer.CurrentExerciseRecordGEOLocationDataForAttribute = new ArrayList<GPSLocationData>();

                        // Get the current start location
                        if(UserDataContainer.CurrentExerciseRecordGEOLocationDataForAttribute != null && newStartLocation != null) {
                            GPSLocationData locationData = new GPSLocationData();
                            locationData.Latitude = newStartLocation.getLatitude();
                            locationData.Longitude = newStartLocation.getLongitude();

                            locationData.LocationTime = 0;
                            UserDataContainer.CurrentExerciseRecordGEOLocationDataForAttribute.add(locationData);
                        }
                        chronometer.setBase(SystemClock.elapsedRealtime());
                        chronometer.start();
                        activityAction.setText(R.string.Stop);
                        timerStarted = true;
                    } else {
                        chronometer.stop();
                        Location stopLocation = gps.getLocation();
                        long elapsedMillis = SystemClock.elapsedRealtime() - chronometer.getBase();
                        long seconds = elapsedMillis / 1000;
                        int minutes = (int)seconds / 60;
                        int hours = minutes / 60;
                        timerStarted = false;
                        activityAction.setText(R.string.Start);
                        UserDataContainer.CurrentExerciseRecord.Record = elapsedMillis;




                        // Get the current stop location where the user has stoped moving
                        if(UserDataContainer.CurrentExerciseRecordGEOLocationDataForAttribute != null && stopLocation != null) {
                            GPSLocationData locationData = new GPSLocationData();
                            locationData.Latitude = stopLocation.getLatitude();
                            locationData.Longitude = stopLocation.getLongitude();

                            locationData.LocationTime = elapsedMillis;
                            UserDataContainer.CurrentExerciseRecordGEOLocationDataForAttribute.add(locationData);
                        }

                        Gson gson = new Gson();
                        //JSONArray jsArray = new JSONArray(UserDataContainer.CurrentExerciseRecordGEOLocationDataForAttribute);
                        UserDataContainer.CurrentExerciseRecordGEOLocationAttribute.Data = gson.toJson(UserDataContainer.CurrentExerciseRecordGEOLocationDataForAttribute);
                        saveRequested = true;
                        gps.stopUsingGPS();
                    }
                }

                if(saveRequested)
                {
                    AsyncTask<Void, Void, Void> asyncTask = new AsyncTask<Void, Void, Void>() {

                        @Override
                        protected Void doInBackground(Void... params) {
                            UserDataContainer.CurrentExerciseRecord.StartDate = UserDataContainer.CurrentExerciseRecord.EndDate = UserDataContainer.CurrentExerciseRecord.Date = Calendar.getInstance().getTime();
                            UserDataContainer.CurrentExerciseRecord.ExerciseId = Long.parseLong(UserDataContainer.UserSets.get((int)spinner.getSelectedItemId()).Exercises.get((int)spinnerExercise.getSelectedItemId()).ID);

                            JSONHttpClient jsonHttpClient = new JSONHttpClient();
                            UserDataContainer.CurrentExerciseRecord = (ExerciseRecord) jsonHttpClient.PostObject(Constants.WebServiceLocation + "/api/ExerciseRecords", UserDataContainer.CurrentExerciseRecord, ExerciseRecord.class);
                            // TODO: Add geo location support
                            if(UserDataContainer.CurrentExerciseRecordGEOLocationAttribute != null && UserDataContainer.CurrentExerciseRecord.Id > 0) {
                                UserDataContainer.CurrentExerciseRecordGEOLocationAttribute.ExerciseRecordID = UserDataContainer.CurrentExerciseRecord.Id;
                                UserDataContainer.CurrentExerciseRecordGEOLocationAttribute = (ExerciseRecordAttribute) jsonHttpClient.PostObject(Constants.WebServiceLocation + "/api/ExerciseRecordAttributes", UserDataContainer.CurrentExerciseRecordGEOLocationAttribute, ExerciseRecordAttribute.class);
                            }

                            return null;
                        }

                        @Override
                        protected void onPostExecute(Void none) {
                            Intent intent = new Intent(MainActivity.this, ExerciseRecordResults.class);
                            startActivity(intent);

                        }

                        @Override
                        protected void onCancelled() {


                        }
                    }.execute();


                }
            }
        });


        bLogOut.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Calendar cal = Calendar.getInstance();
                /*if(UserDataContainer.LoginData.ExpiresAsDate.after(cal.getTime()) == false) {
                    DeviceDataStorage.RemoveFileFromDeviceStorage(Constants.UserLoginData_FileName);
                }*/
                DeviceDataStorage.RemoveFileFromDeviceStorage(Constants.UserLoginData_FileName);
                Intent intent = new Intent(MainActivity.this, LoginActivity.class);
                startActivity(intent);
                showProgress(false);
            }
        });

    }

    public String GetGPSLocationsForAttribute()
    {
        StringBuilder data = new StringBuilder();
        for(int x = 0; x < UserDataContainer.CurrentExerciseRecordGEOLocationDataForAttribute.size(); ++x)
        {


        }

        return data.toString();
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
