using GlobalVars;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class LocationManager : MonoBehaviour
{

    //Serialized variables
    [SerializeField] private List<GameObject> mascotList = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI timeAndDayPlaceHolder;

    
    //Runtime variables
    private static LocationManager instance;
    private TimeSlot currentTimeSlot = TimeSlot.morning;
    private Day currentDay = Day.Monday;
    private Location currentLocation;

    private void Awake()
    {
        if (instance != null) 
        {
            Debug.LogWarning("found more than one location manager, uh oh");
        }
        instance = this;
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
        if (currentTimeSlot == TimeSlot.evening)
        {
            currentTimeSlot = TimeSlot.morning;
            if (currentDay == Day.Saturday) currentDay = Day.Sunday;
            else currentDay++;
        }
        else currentTimeSlot++;

        UpdateTimeAndDayGUI();

        UpdateAllMascotLocations();

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
                Debug.Log("only one story at location, jumping to story " + storyContext.name);
                StartCoroutine(SceneChanger.GetInstance().LoadSceneAndCallDialogue("story", storyContext));
            }
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

    #endregion 
}
