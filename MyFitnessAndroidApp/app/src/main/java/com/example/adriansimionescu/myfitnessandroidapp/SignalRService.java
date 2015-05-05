package com.example.adriansimionescu.myfitnessandroidapp;

import android.app.IntentService;
import android.app.Service;
import android.content.Intent;
import android.os.Binder;
import android.os.IBinder;
import android.util.Log;
import android.widget.Toast;

import java.util.concurrent.ExecutionException;

import microsoft.aspnet.signalr.client.SignalRFuture;
import microsoft.aspnet.signalr.client.hubs.HubConnection;
import microsoft.aspnet.signalr.client.hubs.HubProxy;
import microsoft.aspnet.signalr.client.hubs.SubscriptionHandler1;
import microsoft.aspnet.signalr.client.hubs.SubscriptionHandler2;
import microsoft.aspnet.signalr.client.hubs.SubscriptionHandler3;
import microsoft.aspnet.signalr.client.transport.ClientTransport;
import microsoft.aspnet.signalr.client.transport.ServerSentEventsTransport;


public class SignalRService extends Service {

    // Binder given to clients
    private final IBinder binder = new LocalBinder();
    // Registered callbacks
    private ServiceCallbacks serviceCallbacks;


    // Class used for the client Binder.
    public class LocalBinder extends Binder {
        SignalRService getService() {
            // Return this instance of MyService so clients can call public methods
            return SignalRService.this;
        }
    }

    @Override
    public IBinder onBind(Intent intent) {
        return binder;
    }



    @Override
    public void onCreate() {
        super.onCreate();
    }

    public void setCallbacks(ServiceCallbacks callbacks) {
        this.serviceCallbacks = callbacks;
    }

    @SuppressWarnings("deprecation")
    @Override
    public void onStart(Intent intent, int startId) {
        super.onStart(intent, startId);
        Toast.makeText(this, "Service Start", Toast.LENGTH_LONG).show();

        String server = Constants.SignalRGateway;
        HubConnection connection = new HubConnection(server);
        connection.getHeaders().put("username", UserDataContainer.LoginData.userName);
        HubProxy proxy = connection.createHubProxy(Constants.SignalRHubName);

        //SignalRFuture<Void> awaitConnection = connection.start();

        ClientTransport transport = new ServerSentEventsTransport(connection.getLogger());

        SignalRFuture<Void> awaitConnection = connection.start(transport);
        try {
            awaitConnection.get();
            proxy.subscribe(this );
        } catch (InterruptedException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        } catch (ExecutionException e) {
            // TODO Auto-generated catch block
            e.printStackTrace();
        }

        //--HERE IS MY SOLUTION-----------------------------------------------------------
        //Invoke JoinGroup to start receiving broadcast messages
        //proxy.invoke("JoinGroup", "Group1");

        //Then call on() to handle the messages when they are received.
        /*proxy.on( "send", new SubscriptionHandler2<String,String>() {
            @Override
            public void run(String name, String msg) {
                Log.d("result := ", msg);
                Log.d("result := ", name);
            }
        }, String.class, String.class);

        proxy.on( "isDataUpdateRequiredForMobileClient", new SubscriptionHandler3<String,Boolean,String>() {
            @Override
            public void run(String name, Boolean isRequired, String msg) {
                Log.d("result := ", msg);
                Log.d("result := ", name);
            }
        }, String.class, Boolean.class, String.class);*/

        //--------------------------------------------------------------------------------
    }

    public void Send( String name, String message )
    {
        final String fmessage = message;
        final String fname = name;

    }

    public void IsDataUpdateRequiredForMobileClient( String name, boolean isRequired, String message ) {
        final String fmessage = message;
        final String fname = name;
        final boolean fisrequired = isRequired;
        if (serviceCallbacks != null) {
            serviceCallbacks.updateUI();
        }
    }


    @Override
    public void onDestroy() {
        super.onDestroy();
    }

}
