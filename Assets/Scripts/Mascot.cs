using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using GlobalVars;
using Unity.Mathematics;
using UnityEngine.Rendering;

[System.Serializable]
public class Mascot : MonoBehaviour
{
    // class for mascots
    public string mascotName;
    public bool haveMet = false;
    public Sprite mascotSprite;
    public int barValue;
    public int heartLevel;
    public Calendar calendar; 
    public bool interactedWith;
    private Location currentLocation;
    private List<CourseScriptableObject> courses; // course list should be stored in the calendar
    private Club club;

    // Start is called before the first frame update
    void Start()
    {
        calendar?.Verify(); // verify the calendar in case this mascot's course list was set in the inspector
    }

    /// <summary>
    /// call when updating to a new time/day, passing in the time and day. will update this mascot's location to the
    /// location at that time/day. 
    /// </summary>
    public void UpdateLocation(TimeSlot time, Day day)
    {
        // calendar = GetComponent<Calendar>();
        var prevLocation = currentLocation;
        if (calendar is null)
        {
            Debug.Log($"CALENDAR IS NULL FOR MASCOT {mascotName}");
        }
        currentLocation = calendar.GetLocation(time, day);
        Debug.Log($"Mascot {mascotName} location changed from {prevLocation} to {currentLocation}");
    }
    
    /// <summary>
    /// set location at time and day
    /// </summary>
    public void SetLocation(TimeSlot time, Day day, Location location)
    {
        calendar.SetLocation(time, day, location);
    }

    public Location GetLocation() 
    {
        return currentLocation;
    }
    
    public string GetName()
    {
        return mascotName;
    }

    public int GetHeartLevel()
    {
        return heartLevel;
    }

    public float GetBarValue()
    {
        return barValue;
    }

    public bool GetMetYet()
    {
        return haveMet;
    }

    public void IncreaseBarValue(int pips)
    {
        barValue = Math.Clamp(barValue + pips, 0, 5);
        Debug.Log($"bar value increased to {barValue}");
    }

    public void IncreaseHeartValue()
    {
        if (barValue >= 5)
        {
            heartLevel = Math.Clamp(heartLevel + 1, 0, 4);
            barValue = 0;
            Debug.Log($"heart level increased by 1! to {heartLevel}");
            Debug.Log($"bar value reset to 0!");
        }
    }

    public void DecreaseBarValue(int pips)
    {
        barValue = Math.Clamp(barValue - pips, 0, 5);
        Debug.Log($"bar value decreased to {barValue}");
    }

    public void SaveMascot()
    {
        // meow
        // save data in a text file
        // Debug.Log("attempting to save mascot data for mascot " + mascotName);
        // string dirPath = Path.Combine(Application.persistentDataPath, mascotName);
        // Directory.CreateDirectory(dirPath);
        // string filePath = Path.Combine(dirPath, "MascotData.txt");
        // string jsonData = JsonUtility.ToJson(this);
        //
        // FileStream fcreate = File.Open(filePath, FileMode.Create); // create file if it doesn't exist, overwrite it if it does exist
        // StreamWriter sw = new StreamWriter(fcreate);
        //
        // sw.WriteLine(jsonData);
        // Debug.Log("saved mascot data to path: " + filePath);
        // sw.Close();
        
        SaveSystem<Mascot>.Save(this, mascotName, "MascotData.json");
    }

    public void LoadMascot()
    {
        // Debug.Log("attempting to load mascot data for mascot " + mascotName);
        // string dirPath = Path.Combine(Application.persistentDataPath, mascotName);
        // if (Directory.Exists(dirPath))
        // {
        //     string filePath = Path.Combine(dirPath, "MascotData.txt");
        //     
        //     FileStream fread = File.Open(filePath, FileMode.Open);
        //     StreamReader sr = new StreamReader(fread);
        //     
        //     string jsonData = sr.ReadLine();
        //     JsonUtility.FromJsonOverwrite(jsonData, this);
        //     calendar.Verify(); // add courses in loaded calendar to the actual calendar
        //     fread.Close();
        // }
        // else
        // {
        //     Debug.LogError("directory: " + dirPath + " does not exist! aborting mascot load.");
        // }
        
        string mascotJson = SaveSystem<Mascot>.Load(mascotName, "MascotData.json");
        JsonUtility.FromJsonOverwrite(mascotJson, this);
        calendar.Verify();
    }
}
