using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using GlobalVars;

public class Player
{
    private string name;
    private Calendar playerCalendar;
    private Course[] courses;

    public string getName()
    {
        return name;
    }

    public Course[] GetCourses()
    {
        return courses;
    }
}

