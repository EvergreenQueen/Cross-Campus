using GlobalVars;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    //All references:
    [SerializeField] LocationManager locationManager;
    //LocationManager

    // All Serialized Vars:
    [SerializeField] public List<GameObject> mascotList = new List<GameObject>();

    //Publically available vars
    public static GameManager instance;
    public Player player;
    public Calendar playerCalendar;
    public TimeSlot currentTimeSlot;
    public Day currentDay;

    //Saving Data:
    public SavedTimeAndDay savedTimeAndDay; // SET IN INSPECTOR
    public MascotState mascotState;

    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("found more than one game manager, but it's a singleton and there should only be one in the scene.");
        }
        instance = this;

        player = GameObject.Find("Player")?.GetComponent<Player>();
    }


    //Dates/Interactions

    public void FinishInteraction(bool success, Mascot mascot)
    {
        if (success) mascot.IncreaseBarValue(2);
        else mascot.DecreaseBarValue(1);
    }

    public void FinishDate(Mascot mascot)
    {
        mascot.IncreaseHeartValue();
    }

    //==
    // Time/Date Updates:
    private void UpdateTimeAndDay()
    {
        if (currentTimeSlot == TimeSlot.evening)
        {
            currentTimeSlot = TimeSlot.morning;
            if (currentDay == Day.Saturday) currentDay = Day.Sunday;
            else currentDay++;
        }
        else currentTimeSlot++;

        // SATURDAY LIFE EVENTS
        //if (currentDay == Day.Saturday)
        //{
        //    foreach (var mascotObj in mascotList)
        //    {
        //        var mascot = mascotObj.GetComponent<Mascot>();
        //        if (mascot.GetHeartLevel() >= 5)
        //        {
        //            // load and play date story probably
        //        }
        //    }
        //}

    }

    public void ProgressTime()
    {
        UpdateTimeAndDay();
        //Insert any code to update other areas
        locationManager.ProgressTime();
    }




    //== 
    // Save/Load
    public void SaveGame()
    {
        LocationManager.GetInstance().SaveLocationData();
        player.SavePlayer();
    }

    // made this a couroutine because it needs to be on the campus map scene to load things properly
    // (and loading scenes using SceneManager.LoadScene doesn't load the scene on the same frame so i needed to use a couroutine)
    // you could probably separate the functionality into more parts (if you wanted to load the game to end up on a different scene or something),
    // but for our purposes i dont think we'll ever need to load the game in a different context
    // campus map is kinda the main scene of the game 
    public IEnumerator LoadGame()
    {
        // EventSystem e2 = GameObject.Find("EventSystem")?.GetComponent<EventSystem>();
        // if (e2)
        // {
        //     e2.enabled = false;
        // }

        // GameObject mainCam2 = GameObject.Find("Main Camera");
        // if (mainCam2)
        // {
        //     mainCam2.SetActive(false);
        // }

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene() !=
            UnityEngine.SceneManagement.SceneManager.GetSceneByName("CampusMap")) // don't load campusmap scene if we're already on it
        {
            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("CampusMap", LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            Debug.Log("loaded campusmap scene");

            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("MainMenu");
        }

        // if (!LocationManager.GetInstance())
        // {
        //     GameObject locationManagerObject = new GameObject();
        //     locationManagerObject.name = "LocationManager";
        //     locationManagerObject.AddComponent<LocationManager>();
        //     Debug.Log("made a new location manager for some reason");
        // }
        LocationManager.GetInstance().LoadLocationData();

        // very silly player gameobject things. will mostly likely be solved once the player is a static class
        if (!player)
        {
            if (GameObject.Find("Player"))
            {
                player = GameObject.Find("Player").GetComponent<Player>();
            }
            else
            {
                GameObject playerObject = new GameObject();
                playerObject.name = "Player"; // ermmmmm
                player = playerObject.AddComponent<Player>();
                DontDestroyOnLoad(playerObject);
            }
        }
        player.LoadPlayer();

        // if (e2)
        // {
        //     Destroy(e2); // the campus map scene already has an event system object, so delete the one that was in the previous scene (probably main menu) if there was one
        // }
    }

    public bool SaveDataExists()
    {
        if (LocationDataExists() && PlayerDataExists())
        {
            return true;
        }
        return false;
    }

    public bool LocationDataExists()
    {
        string dirPath = Path.Combine(Application.persistentDataPath, "Map");
        if (Directory.Exists(dirPath))
        {
            if (File.Exists(Path.Combine(dirPath, "TimeDayData.txt")))
            {
                return true;
            }
        }
        return false;
    }

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

    //HERE
    public void RetrieveState()
    {
        currentDay = savedTimeAndDay.day;
        currentTimeSlot = savedTimeAndDay.time;

        foreach (var mascot in mascotList)
        {
            var component = mascot.GetComponent<Mascot>();
            var data = mascotState.GetData(component.name);
            component.heartLevel = data.heartLevel;
            component.barValue = data.barValue;
            component.interactedWith = data.interactedWith;
        }
    }

    public void SaveStateAndJumpToStory(StoryContext context)
    {
        Debug.Log("jumping to story " + context.name);
        // save time and day state
        savedTimeAndDay.time = currentTimeSlot;
        savedTimeAndDay.day = currentDay;

        foreach (var mascot in mascotList)
        {
            var component = mascot.GetComponent<Mascot>();
            mascotState.UpdateData(component.name, component.heartLevel, component.barValue, component.interactedWith);
        }

        StartCoroutine(SceneChanger.GetInstance().LoadSceneAndCallDialogue("StoryScene", context));
    }

    //==
    //Getters/Setters:

    public static GameManager GetInstance()
    {
        return instance;
    }
}
