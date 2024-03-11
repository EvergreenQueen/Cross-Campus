using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using GlobalVars;

public class Player
{
    private string name;
    private Calendar calendar; // previously was PlayerCalendar, but they have been merged into the same class, because it's not necessary (at this time) to differentiate them
    private Course[] courses;

    public string GetName()
    {
        return name;
    }

    public Course[] GetCourses()
    {
        return courses;
    }

    public void SetCourses()
    { 
        
    }

}

