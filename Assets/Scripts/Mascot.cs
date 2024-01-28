using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;
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

    private string mascotName;
    private float affectionMeter; 
    private Calendar calendar; // contains schedule (location at each time and day)
    private Location currentLocation; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Location GetLocation() 
    {
        return currentLocation;
    }
    // change location according to inputted time and day
    // called in LocationManager every time time/day changes
    // public void UpdateLocation(int time, int day) 
    // {
    //     currentLocation = calendar.GetLocation(time, day); 
    //     Debug.Log("mascot " + name + " location changed to " + currentLocation);
    // }
    
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
    public void IncreaseAffection(float amount)
    {
        affectionMeter = math.clamp(affectionMeter + amount, 0, 100);
    }
}
