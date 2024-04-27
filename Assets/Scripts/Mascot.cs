using System;
using System.Collections.Generic;
using UnityEngine;
using GlobalVars;
using Unity.Mathematics;
using UnityEngine.Rendering;

public class Mascot : MonoBehaviour
{
    // class for mascots
    public string mascotName;
    public Sprite mascotSprite;
    [SerializeField] private int barValue;
    [SerializeField] private int heartLevel;
    private Calendar calendar; // component
    private Location currentLocation;
    private List<CourseScriptableObject> courses;
    private Club club;

    // Start is called before the first frame update
    void Start()
    {
        calendar = GetComponent<Calendar>();
    }

    /// <summary>
    /// call when updating to a new time/day, passing in the time and day. will update this mascot's location to the
    /// location at that time/day. 
    /// </summary>
    public void UpdateLocation(TimeSlot time, Day day)
    {
        var prevLocation = currentLocation;
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

    public float GetHeartLevel()
    {
        return heartLevel;
    }

    public float GetBarValue()
    {
        return barValue;
    }

    public void IncreaseBarValue(int pips)
    {
        barValue = Math.Clamp(barValue + pips, 0, 5);
        Debug.Log($"bar value increased to {barValue}");
        if (barValue == 5)
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
        // TODO reduce heart level when it's zero? maybe?
    }
}
