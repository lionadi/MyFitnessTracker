package com.example.adriansimionescu.myfitnessandroidapp;

import java.io.FileInputStream;
import java.io.FileOutputStream;
import java.io.ObjectInputStream;
import java.io.ObjectOutputStream;
import android.content.Context;
import android.os.Environment;
import android.util.Log;

/**
 * Created by Adrian Simionescu on 2/3/2015.
 */
public class DeviceDataStorage {
    public static <T> void SerializeObjectToDeviceStorage(T objectToBeSerialized, String fileName)
    {
        try
        {
            FileOutputStream fos = new FileOutputStream(Environment.getExternalStorageDirectory().getAbsolutePath()+ "/" + fileName);
            ObjectOutputStream oos = new ObjectOutputStream(fos);
            oos.writeObject(objectToBeSerialized);
            oos.close();
        }
        catch (Exception ex)
        {
            Log.w("myApp", ex.toString());
        }
    }

    public static <T> T DeSerializeObjectToDeviceStorage(String fileName)
    {
        T objectToReturn = null;
        try
        {
            FileInputStream fis = new FileInputStream(Environment.getExternalStorageDirectory().getAbsolutePath()+ "/" + fileName);
            ObjectInputStream ois  = new ObjectInputStream (fis);
            objectToReturn = (T) ois.readObject();
            ois.close();
        }
        catch (Exception ex)
        {
            Log.w("myApp", ex.toString());
        }

        return(objectToReturn);
    }
}
