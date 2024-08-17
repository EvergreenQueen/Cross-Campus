using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using GlobalVars;
using UnityEngine.Serialization;
using System.IO;

public class Player : MonoBehaviour
{
    public string playerName;
    public Calendar calendar; // previously was PlayerCalendar, but they have been merged into the same class, because it's not necessary (at this time) to differentiate them
    
    [SerializeField]
    private CourseScriptableObject placeholderCourse;
    private static Player instance;

    private void Awake()
    {
        // calendar check
        if (calendar == null)
        {
            calendar = new Calendar();
            if (placeholderCourse != null)
            {
                calendar.AddCourse(placeholderCourse);
            }
        }
        else
        {
            calendar.Verify();
        }

        // singleton check + slap on dontdestroyonload
        if (instance != null && instance != this) 
        {
            Debug.LogWarning("Found more than 1 Player. That's not bueno.");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }
    
    public string GetName()
    {
        return playerName;
    }

    public Calendar GetCalendar()
    {
        return calendar;
    }

    public void SetCalendar(Calendar c)
    {
        calendar = c;
    }

    public void SavePlayer()
    {
        // save the player object itself as a json rather than pieces of it in different files
        SaveSystem<Player>.Save(this, "Player", "PlayerData.json");
        
        // save course grades to a text file.
        // probably isn't the player's responsibility to serialize this information buuuutttt uhhhhmmm
        // also the reason we care about this is because although the scriptableobjects are serialized already, if the 
        // player wants to go back to a previous save we need to overwrite the values that are serialized
        string dirPath = Path.Combine(Application.persistentDataPath, "Player");
        Directory.CreateDirectory(dirPath);
        string filePath = Path.Combine(dirPath, "CourseGrades.txt");
        
        FileStream fcreate = File.Open(filePath, FileMode.Create); // create file if it doesn't exist, overwrite it if it does exist
        StreamWriter sw = new StreamWriter(fcreate);
        
        CourseScriptableObject[] fullCourseList = Resources.LoadAll("Courses").Cast<CourseScriptableObject>().ToArray();
        foreach (var course in fullCourseList)
        {
            sw.WriteLine(course.name + ": " + course.grade);
        }
        sw.Close();
    }

    public void LoadPlayer()
    {
        string playerJson = SaveSystem<Player>.Load("Player", "PlayerData.json");
        JsonUtility.FromJsonOverwrite(playerJson, this);
        calendar.Verify(); // repopulate calendar internal data structure because it doesn't get jsonified
        
        // read course grades from text file and load them into each coursescriptableobject
        string dirPath = Path.Combine(Application.persistentDataPath, "Player");
        Directory.CreateDirectory(dirPath);
        string filePath = Path.Combine(dirPath, "CourseGrades.txt");
        
        FileStream fread = File.Open(filePath, FileMode.Open);
        StreamReader sr = new StreamReader(fread);
        
        CourseScriptableObject[] fullCourseList = Resources.LoadAll("Courses").Cast<CourseScriptableObject>().ToArray();

        while (!sr.EndOfStream)
        {
            var line = sr.ReadLine();
            string[] split = line.Split(": ");
            foreach (var course in fullCourseList)
            {
                if (course.name == split[0])
                {
                    course.grade = int.Parse(split[1]);
                }
            }
        }
        
        sr.Close();
    }
    
    // returns true if player data exists in the persistent data path of the game. mainly used to know if we should gray out the load game button on the main menu scene.
    public bool PlayerDataExists()
    {
        string dirPath = Path.Combine(Application.persistentDataPath, "Player");
        string filePath = Path.Combine(dirPath, "CourseGrades.txt");
        
        if (Directory.Exists(dirPath) && File.Exists(filePath))
        {
            return true;
        }
        return false;
    }
}

