using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using GlobalVariables;

public class Player
{
    private string name;
    private Calendar calendar; // previously was PlayerCalendar, but they have been merged into the same class, because it's not necessary (at this time) to differentiate them
    private GlobalVars.Course[] courses;

    public string GetName()
    {
        return name;
    }

    public GlobalVars.Course[] GetCourses()
    {
        return courses;
    }

    public void SetCourses()
    { 
        
    }

}

