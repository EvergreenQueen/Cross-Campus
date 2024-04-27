using GlobalVars;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LocationManager : MonoBehaviour
{

    //Serialized variables
    [SerializeField] private List<GameObject> mascotList = new List<GameObject>();
    [SerializeField] private TextMeshProUGUI timeAndDayPlaceHolder;

    //Runtime variables
    private TimeSlot currentTimeSlot = TimeSlot.morning;
    private Day currentDay = Day.Monday;
    private Location currentLocation;

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
    public void GoToLocation(string locationName)
    {
        foreach (GameObject mascot in GetMascotsAtLocation(locationName))
            Debug.Log(mascot.GetComponent<Mascot>().mascotName);
    }

    #region HELPER FUNCTIONS 

    private void UpdateTimeAndDayGUI()
    {
        timeAndDayPlaceHolder.text = Calendar.TimeToString(currentTimeSlot) + ", " + Calendar.DayToString(currentDay);
    }

    private List<GameObject> GetMascotsAtLocation(string locationName)
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

    #endregion 
}
