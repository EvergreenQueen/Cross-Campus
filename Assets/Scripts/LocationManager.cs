using System;
using GlobalVars;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Serialization;
using System.IO;
using Random = System.Random;

public class LocationManager : MonoBehaviour
{

    //Serialized variables
    [SerializeField] private List<GameObject> mascotList = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI timeAndDayPlaceHolder;
    [SerializeField] private GameObject eventPopUpPrefab;
    public Canvas uiCanvas; // might wanna do this thru a ui manager tbh

    [SerializeField] private StoryContext basicClassStoryContext; // default context for classes, all params are ignored except it's textasset field
    [SerializeField] private TextAsset basicClassStory;
    [SerializeField] private CourseScriptableObject placeholderCourse;
    

    
    //Runtime variables
    private static LocationManager instance;
    public TimeSlot currentTimeSlot = TimeSlot.morning;
    public Day currentDay = Day.Monday;
    private Location currentLocation;
    
    private EventPopUp eventPopUp;

    // game state stuff
    public SavedTimeAndDay savedTimeAndDay; // SET IN INSPECTOR
    public MascotState mascotState; // set in inspectore (Assets/GameStates/MascotState
    
    public Player player;
    public Calendar playerCalendar;

    private void Awake()
    {
        if (instance != null) 
        {
            Debug.LogWarning("found more than one location manager, uh oh");
        }
        instance = this;
        
        // player should in theory be dontdestroyonloaded
        player = GameObject.Find("Player")?.GetComponent<Player>();
        
        // create new calendar if it's not defined for some reason
        // FIXME move this to the player object itself, because this functionality is not the location manager's responsibility
        if (player)
        {
            playerCalendar = player.GetCalendar();
            if (playerCalendar == null)
            {
                playerCalendar = new Calendar();

                var dayList = new List<Day>();
                dayList.Add(Day.Monday);
                var timesList = new List<TimeSlot>();
                timesList.Add(TimeSlot.evening);
                // playerCalendar = gameObject.AddComponent<Calendar>();
                playerCalendar.AddCourse(placeholderCourse);

                player.SetCalendar(playerCalendar);
            }
        }
    }

    public static LocationManager GetInstance()
    {
        return instance;
    }
    
    private void Start()
    {
        UpdateAllMascotLocations();
        UpdateTimeAndDayGUI();
    }
    
    
    public void ProgressTime()
    {
        Debug.Log($"progressing time, old time slot is {currentTimeSlot}, old day is {currentDay}");
        if (currentTimeSlot == TimeSlot.evening)
        {
            currentTimeSlot = TimeSlot.morning;
            if (currentDay == Day.Saturday) currentDay = Day.Sunday;
            else currentDay++;
        }
        else currentTimeSlot++;
        Debug.Log($"done progressing time, new time slot is {currentTimeSlot}, new day is {currentDay}");

        if (eventPopUp)
        {
            Destroy(eventPopUp.gameObject);
        }
        
        // SATURDAY LIFE EVENTS
        if (currentDay == Day.Saturday)
        {
            foreach (var mascotObj in mascotList)
            {
                var mascot = mascotObj.GetComponent<Mascot>();
                if (mascot.GetHeartLevel() >= 5)
                {
                    // load and play date story probably
                }
            }
        }
        
        UpdateTimeAndDayGUI();
        UpdateAllMascotLocations();
        CheckPlayerScheduleAndNotify();
    }
    
    // called when clicking on a location on the map
    // should get the story given all the contexts and load it
    // TODO add ui indication of what story you're going to get, maybe a menu that pops up? or a tooltip when hovering over?
    //  ^^^ leaning towards a menu that pops up so you can select the mascot you want to talk to in case there's multiple at the same location
    // also the story that is picked WILL be a random one from the m
    public void GoToLocation(string locationName)
    {
        foreach (GameObject mascot in GetMascotsAtLocation(locationName)) // probably have a SEPARATE button for each mascot currently at the location, and each button calls gotolocation for THAT mascot
        {
            var mascotComponent = mascot.GetComponent<Mascot>();
            var mascotName = mascotComponent.mascotName;
            var location = LocationStringToLocation(locationName);
            var isFirstTimeInteraction = !mascotComponent.interactedWith;
            var ctxList = StoryManager.Instance.GetContexts(currentTimeSlot, currentDay, location, mascotName, mascot.GetComponent<Mascot>().GetHeartLevel(), isFirstTimeInteraction);

            if (ctxList.Count == 0)
            {
                Debug.Log($"no stories with current context: time = {currentTimeSlot.ToString()}, day = {currentDay.ToString()}, location = {location.ToString()}, mascot = {mascotName}, heart level = {mascot.GetComponent<Mascot>().GetHeartLevel()}, first time interaction = {isFirstTimeInteraction.ToString()}");
                return;
            }
            
            string debugOutput = "";
            foreach (var item in ctxList)
            {
                debugOutput += item.name + " ";
            }
            Debug.Log("location stories: " + debugOutput);
            
            if (isFirstTimeInteraction)
            {
                var storyContext = ctxList[0];
                mascotComponent.interactedWith = true;

                SaveStateAndJumpToStory(storyContext);
            }
            else
            {
                // pick a random context from the list of contexts
                var storyIdx = UnityEngine.Random.Range(0, ctxList.Count);
                Debug.Log("jumping to random story: " + ctxList[storyIdx]);
                mascotComponent.interactedWith = true;
                SaveStateAndJumpToStory(ctxList[storyIdx]);
            }
        }
    }

    // check if the player has clubs OR even courses at the current time/day slot
    // and notify them in the corner or something
    // give them the option to go to that club or course if they want to
    // for courses probably load a very short story that says "woah you learned things..."
    // for clubs i think we want interactions w/ other mascots to happen so uhhh depending on the club load a story ig
    // unless we're like not doing clubs
    private void CheckPlayerScheduleAndNotify()
    {
        if (player is null)
        {
            player = GameObject.Find("Player").GetComponent<Player>();
        }
        var calendar = player.GetCalendar();
        // check courbses
        var course = calendar.GetCourseAtTime(currentTimeSlot, currentDay);
        Debug.Log($"course right now is {course}");
        if (course != null)
        {
            var eventPopUpObject = Instantiate(eventPopUpPrefab, uiCanvas.transform);
            // eventPopUpObject.transform.position = 
            eventPopUp = eventPopUpObject.GetComponent<EventPopUp>();
            eventPopUp.SetText($"You have {course.name} right now! Go to class? (skipping class may result in bad grades)");
            eventPopUp.AssignButtonEvents(() =>
            {
                // accept callback
                SaveStateAndJumpToStory(basicClassStoryContext);
            }, () =>
            {
                // deny callback...
                course.Skip(); // adds 1 to the "skipped" counter
                // FIXME decrement grade at another time instead of as soon as you press the button??
                course.DecrementGrade(5);
                Destroy(eventPopUp.gameObject);
            });
        }
    }
    
    // save many things related to locations
    public void SaveLocationData()
    {
        // save each mascot
        foreach (GameObject mascot in mascotList)
        {
            mascot.GetComponent<Mascot>().SaveMascot();
        }
        
        // save current time/day and stuff like that
        Debug.Log("attempting to save location data");
        string dirPath = Path.Combine(Application.persistentDataPath, "Map");
        Directory.CreateDirectory(dirPath);
        // writing to a text file, because all we care about is the current day and time, nothing json/object related
        string filePath = Path.Combine(dirPath, "TimeDayData.txt");
        
        FileStream fcreate = File.Open(filePath, FileMode.Create); // create file if it doesn't exist, overwrite it if it does exist
        StreamWriter sw = new StreamWriter(fcreate);
        
        // this writes the enum value as a string which we will parse when reading it
        sw.WriteLine(currentDay);
        sw.WriteLine(currentTimeSlot);
        sw.Close();
        
        Debug.Log("saved location data (current time and day aka the state to path: " + filePath);
    }

    // load many things related to locations
    public void LoadLocationData()
    {
        // load each mascot
        foreach (GameObject mascot in mascotList)
        {
            mascot.GetComponent<Mascot>().LoadMascot();
        }
        
        // 
        Debug.Log("attempting to load location data");
        string dirPath = Path.Combine(Application.persistentDataPath, "Map");
        if (Directory.Exists(dirPath))
        {
            string filePath = Path.Combine(dirPath, "TimeDayData.txt");
            
            FileStream fread = File.Open(filePath, FileMode.Open);
            StreamReader sr = new StreamReader(fread);
            Enum.TryParse(sr.ReadLine(), out Day day);
            Enum.TryParse(sr.ReadLine(), out TimeSlot timeSlot);
            fread.Close();
            
            Debug.Log("parsed day: " + day);
            Debug.Log("current time: " + timeSlot);
            
            currentDay = day;
            currentTimeSlot = timeSlot;
            
            // restore state basically
            UpdateTimeAndDayGUI(); 
            UpdateAllMascotLocations(); 
        }
        else
        {
            Debug.LogError("directory: " + dirPath + " does not exist! aborting time day data load.");
        }
    }
    
    // returns true if location save data exists. mainly used to know whether the main menu should gray out the load game button.
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


    #region HELPER FUNCTIONS 
    
    // update the text that displays the time and day. eventually probably want this to update icons in the ui through some kind of ui manager
    private void UpdateTimeAndDayGUI()
    {
        if (timeAndDayPlaceHolder)
        {
            timeAndDayPlaceHolder.text =
                Calendar.TimeToString(currentTimeSlot) + ", " + Calendar.DayToString(currentDay);
        }
    }

    public List<GameObject> GetMascotsAtLocation(string locationName)
    {
        List<GameObject> mascotsAtLocation = new List<GameObject>();
        foreach (GameObject mascot in mascotList)
        {
            if (Calendar.LocationToString(mascot.GetComponent<Mascot>().GetLocation()) == locationName)
                mascotsAtLocation.Add(mascot);
        }
        return mascotsAtLocation;
    }
    private void UpdateAllMascotLocations()
    {
        foreach (GameObject mascot in mascotList)
            mascot.GetComponent<Mascot>().UpdateLocation(currentTimeSlot, currentDay);
    }

    // given a location (string) convert it to a Location (enum)
    // not case sensitive!
    private Location LocationStringToLocation(string locationName)
    {
        var normalizedLocationName = locationName.ToLower();
        return normalizedLocationName switch
        {
            // {none, pool, library, hub, gym, forest, classroom, botanical_gardens}
            // i sure hope hardcoding this doesn't cause problems later!! :3
            "pool" => Location.pool,
            "library" => Location.library,
            "hub" => Location.hub,
            "gym" => Location.gym,
            "forest" => Location.forest,
            "classroom" => Location.classroom,
            "botanical gardens" => Location.botanical_gardens,
            _ => Location.none
        };
    }



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

    public void UpdateMascotLevel(string mascotName, int amountIncrease)
    {
        foreach (var mascot in mascotList)
        {
            var component = mascot.GetComponent<Mascot>();
            if (component.mascotName.ToLower().Equals(mascotName.ToLower()))
            {
                Debug.Log($"updating mascot level of {mascotName}");
                component.IncreaseBarValue(amountIncrease); // clamping and increasing heart level is handled by mascot script
                // also note that this will not overfill the heart meter if it goes 
            }
        }
    }

    private void SaveStateAndJumpToStory(StoryContext context)
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

    #endregion 
}
