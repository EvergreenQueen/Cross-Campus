using UnityEngine;
using GlobalVars;
using Unity.Mathematics;

public class Mascot : MonoBehaviour
{
    // class for all mascot characters

    /* responsibilities: from tech design doc
        1. stores information about a mascot
            a. holds important information for each of our mascots, including name
        2. able to get and update information about the characters
    */

    public string mascotName;
    public Sprite mascotSprite;
    private int affectionMeter; // int 0-100 to indicate affection level?
    private Calendar calendar; // contains schedule (location at each time and day)
    private GlobalVars.Location currentLocation; 
    private Course[] courses; 
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

    public GlobalVars.Location GetLocation() 
    {
        return currentLocation;
    }
    
    public string GetName()
    {
        return mascotName;
    }

    public float GetAffection()
    {
        return affectionMeter;
    }
    
    // increases/decreases affectionMeter by amount (use negative values to decrease)
    //  * affectionMeter stays in the range [0, 100]
    public void IncreaseAffection(int amount)
    {
        affectionMeter = math.clamp(affectionMeter + amount, 0, 100);
    }
}
