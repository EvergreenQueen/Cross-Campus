using GlobalVars;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.UI;
using UnityEngine;
using UnityEngine.Serialization;

public class LocationManager : MonoBehaviour
{

    //Serialized variables
    [SerializeField] private List<GameObject> mascotList = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI timeAndDayPlaceHolder;
    [SerializeField] private GameObject eventPopUpPrefab;
    public Canvas uiCanvas; // might wanna do this thru a ui manager tbh
    
    [SerializeField] private TextAsset basicClassStory;
    [SerializeField] private CourseScriptableObject placeholderCourse;
    

    
    //Runtime variables
    private static LocationManager instance;
    public TimeSlot currentTimeSlot = TimeSlot.morning;
    public Day currentDay = Day.Monday;
    private Location currentLocation;
    
    private EventPopUp eventPopUp;
    public SavedTimeAndDay savedTimeAndDay; // SET IN INSPECTOR
    
    public Player player;
    public Calendar playerCalendar;

    private void Awake()
    {
        if (instance != null) 
        {
            Debug.LogWarning("found more than one location manager, uh oh");
        }
        instance = this;

        if (player == null)
        {
            player = new Player();
            if (!playerCalendar)
            {
                var dayList = new List<Day>();
                dayList.Add(Day.Monday);
                var timesList = new List<TimeSlot>();
                timesList.Add(TimeSlot.evening);
                playerCalendar = gameObject.AddComponent<Calendar>();
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
        
        // saturday life events
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
    public void GoToLocation(string locationName)
    {
        foreach (GameObject mascot in GetMascotsAtLocation(locationName))
        {
            var mascotName = mascot.GetComponent<Mascot>().mascotName;
            var location = LocationStringToLocation(locationName);
            var ctxList = StoryManager.Instance.GetContexts(currentTimeSlot, currentDay, location, mascotName, mascot.GetComponent<Mascot>().GetHeartLevel());

            string debugOutput = "";
            foreach (var item in ctxList)
            {
                debugOutput += item.name + " ";
            }
            Debug.Log("location stories: " + debugOutput);

            if (ctxList.Count == 1)
            {
                var storyContext = ctxList[0];
                SaveStateAndJumpToStory(storyContext);
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
                SaveStateAndJumpToStory(basicClassStory);
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

    #region HELPER FUNCTIONS 

    private void UpdateTimeAndDayGUI()
    {
        timeAndDayPlaceHolder.text = Calendar.TimeToString(currentTimeSlot) + ", " + Calendar.DayToString(currentDay);
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

    public void RetrieveTimeAndDayState()
    {
        currentDay = savedTimeAndDay.day;
        currentTimeSlot = savedTimeAndDay.time;
    }

    private void SaveStateAndJumpToStory(TextAsset story)
    {
        Debug.Log("jumping to story " + story.name);
        // save time and day state
        savedTimeAndDay.time = currentTimeSlot;
        savedTimeAndDay.day = currentDay;
        StartCoroutine(SceneChanger.GetInstance().LoadSceneAndCallDialogue("story", story));
    }

    #endregion 
}
