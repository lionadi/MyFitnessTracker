package com.example.adriansimionescu.myfitnessandroidapp;

import java.io.IOException;
import java.io.UnsupportedEncodingException;
import java.util.Set;

import org.apache.http.HttpResponse;
import org.apache.http.HttpStatus;
import org.apache.http.client.ClientProtocolException;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpPost;
import org.apache.http.client.methods.HttpPut;
import org.apache.http.client.methods.HttpUriRequest;
import org.apache.http.entity.StringEntity;
import org.apache.http.impl.client.DefaultHttpClient;
import org.apache.http.util.EntityUtils;
import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import android.content.Context;
import android.content.SharedPreferences;
import android.util.Log;

public class RegisterClient {
    private static final String PREFS_NAME = "ANHSettings";
    private static final String REGID_SETTING_NAME = "ANHRegistrationId";
    private String Backend_Endpoint;
    SharedPreferences settings;
    protected HttpClient httpClient;
    private String authorizationHeader;

    public RegisterClient(Context context, String backendEnpoint) {
        super();
        this.settings = context.getSharedPreferences(PREFS_NAME, 0);
        httpClient =  new DefaultHttpClient();
        Backend_Endpoint = backendEnpoint + "/api/register";
    }

    public String getAuthorizationHeader() {
        return authorizationHeader;
    }

    public void setAuthorizationHeader(String authorizationHeader) {
        this.authorizationHeader = authorizationHeader;
    }

    public void register(String handle, Set<String> tags) throws ClientProtocolException, IOException, JSONException {
        String registrationId = retrieveRegistrationIdOrRequestNewOne(handle);

        JSONObject deviceInfo = new JSONObject();
        deviceInfo.put("Platform", "gcm");
        deviceInfo.put("Handle", handle);
        deviceInfo.put("Tags", new JSONArray(tags));

        int statusCode = upsertRegistration(registrationId, deviceInfo);

        if (statusCode == HttpStatus.SC_OK) {
            return;
        } else if (statusCode == HttpStatus.SC_GONE){
            settings.edit().remove(REGID_SETTING_NAME).commit();
            registrationId = retrieveRegistrationIdOrRequestNewOne(handle);
            statusCode = upsertRegistration(registrationId, deviceInfo);
            if (statusCode != HttpStatus.SC_OK) {
                Log.e("RegisterClient", "Error upserting registration: " + statusCode);
                throw new RuntimeException("Error upserting registration");
            }
        } else {
            Log.e("RegisterClient", "Error upserting registration: " + statusCode);
            throw new RuntimeException("Error upserting registration");
        }
    }

    private int upsertRegistration(String registrationId, JSONObject deviceInfo)
            throws UnsupportedEncodingException, IOException,
            ClientProtocolException {
        HttpPut request = new HttpPut(Backend_Endpoint+"/"+registrationId);
        request.setEntity(new StringEntity(deviceInfo.toString()));
        request.addHeader("Authorization", "Basic "+authorizationHeader);
        request.addHeader("Content-Type", "application/json");
        HttpResponse response = httpClient.execute(request);
        int statusCode = response.getStatusLine().getStatusCode();
        return statusCode;
    }

    private String retrieveRegistrationIdOrRequestNewOne(String handle) throws ClientProtocolException, IOException {
        if (settings.contains(REGID_SETTING_NAME))
            return settings.getString(REGID_SETTING_NAME, null);

        HttpUriRequest request = new HttpPost(Backend_Endpoint+"?handle="+handle);
        request.addHeader("Authorization", "Basic "+authorizationHeader);
        HttpResponse response = httpClient.execute(request);
        if (response.getStatusLine().getStatusCode() != HttpStatus.SC_OK) {
            Log.e("RegisterClient", "Error creating registrationId: " + response.getStatusLine().getStatusCode());
            throw new RuntimeException("Error creating Notification Hubs registrationId");
        }
        String registrationId = EntityUtils.toString(response.getEntity());
        registrationId = registrationId.substring(1, registrationId.length()-1);

        settings.edit().putString(REGID_SETTING_NAME, registrationId).commit();

        return registrationId;
    }
}
