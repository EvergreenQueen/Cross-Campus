using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SaveSystem<T>
{
    public static void Save(T data, string folder, string file)
    {
        string jsonData = JsonUtility.ToJson(data);
        string directoryPath = Path.Combine(Application.persistentDataPath, folder); // path w/ no file
        string combinedPath = Path.Combine(directoryPath, file); // path with file
        Debug.Log("attempting to write data to path: " + combinedPath);
        Directory.CreateDirectory(directoryPath);
        try
        {
            File.WriteAllText(combinedPath, jsonData);
            Debug.Log("successfully wrote data to path: " + combinedPath);
        }
        catch (Exception e)
        {
            Debug.LogError("failed to write data to path: " + combinedPath);
            Debug.LogError("error " + e.Message);
        }
    }

    /// <summary>
    /// load json data from file in Application.persistentDataPath/folder.
    /// *returns the json data as a string*, in case you want to overwrite an object with JsonUtility.FromJsonOverwrite().
    /// returns null if there is an issue. so you should check for that probably.
    /// </summary>
    public static string Load(string folder, string file)
    {
        string directoryPath = Path.Combine(Application.persistentDataPath, folder);
        string combinedPath = Path.Combine(directoryPath, file);
        Debug.Log("attempting to read data from: " + combinedPath);
        if (Directory.Exists(directoryPath))
        {
            try
            {
                string jsonData = File.ReadAllText(combinedPath);
                // T returnedData = JsonUtility.FromJson<T>(jsonData);
                Debug.Log("successfully read data from path: " + combinedPath);
                // return (T)Convert.ChangeType(returnedData, typeof(T));
                return jsonData;
            }
            catch (Exception e)
            {
                Debug.LogError("failed to read data from path: " + combinedPath);
                Debug.LogError("error " + e.Message);
                return null;
            }
        }
        else
        {
            Debug.Log("directory " + combinedPath + " does not exist!");
            return null;
        }
    }
}
