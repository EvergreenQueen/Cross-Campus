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

    private string name;
    private int affectionMeter; // int 0-100 to indicate affection level?
    private Calendar calendar; // contains schedule (location at each time and day)
    private Location currentLocation; 
    private Course[] courses; 
    private Club club; 

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
    public void UpdateLocation(int time, int day) 
    {
        currentLocation = calendar.GetLocation(time, day); 
    }
    
    public string GetName()
    {
        return name;
    }

    public int GetAffection()
    {
        return affectionMeter;
    }
    
    // increases/decreases affectionMeter by amount (use negative values to decrease)
    //  * affectionMeter stays in the range [0, 100]
    public void IncreaseAffection(int amount)
    {
        affectionMeter = math.clamp(affectionMeter + amount, 0, 100);
    }

    public Club GetClub() 
    {
        return club;
    }
    
    // returns array of courses
    public Course[] GetCourses()
    {
        return courses;
    }
    
    // takes an array of courses and updates Mascot's schedule accordingly
    // TODO add warning for overwriting courses?
    // can use this function to initialize Mascot's courses using their courses property as the argument
    // otherwise use to update their schedule
    public void UpdateScheduleWithCourses(Course[] newCourses)
    {
        for (var i = 0; i < newCourses.Length; i++)
        {
            // calendar.AddLocation(newCourses[i].time, newCourses[i].day, newCourses[i].location);
        }
    }
}
