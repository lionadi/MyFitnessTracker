package com.example.adriansimionescu.myfitnessandroidapp;

import android.util.Log;

import org.apache.http.HttpEntity;
import org.apache.http.HttpResponse;
import org.apache.http.StatusLine;
import org.apache.http.client.HttpClient;
import org.apache.http.client.methods.HttpGet;
import org.apache.http.impl.client.DefaultHttpClient;

import java.io.BufferedReader;
import java.io.InputStream;
import java.io.InputStreamReader;

/**
 * Created by Adrian Simionescu on 2/4/2015.
 */
public class WebApiHelper {
    public static String GetAsJSON(String relativeURL)
    {
        // No login information so no web service operations can be performed
        if(UserDataContainer.LoginData == null)
            return  null;

        StringBuilder body = new StringBuilder();

        HttpClient httpClient = new DefaultHttpClient();
        HttpGet httpGet = new HttpGet(Constants.WebServiceLocation + "/api" + relativeURL);
        try {
            httpGet.setHeader("Accept","application/json");
            httpGet.setHeader("Content-Type","application/json");
            httpGet.setHeader("Authorization", UserDataContainer.LoginData.token_type + " " + UserDataContainer.LoginData.access_token);

            HttpResponse response = httpClient.execute(httpGet);
            StatusLine statusLine = response.getStatusLine();
            int statusCode = statusLine.getStatusCode();
            if (statusCode == 200) {
                HttpEntity entity = response.getEntity();
                InputStream inputStream = entity.getContent();
                BufferedReader reader = new BufferedReader(
                        new InputStreamReader(inputStream));
                String line;
                while ((line = reader.readLine()) != null) {
                    body.append(line);
                }
                inputStream.close();
            } else {
                Log.d("JSON", "Failed to download file");
            }
        } catch (Exception e) {
            Log.d("readJSONFeed", e.getLocalizedMessage());
        }

        return (body.toString());
    }
}
