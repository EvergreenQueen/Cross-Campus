using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using GlobalVars;

public class Player
{
    private string name = "Player";
    private Calendar calendar; // previously was PlayerCalendar, but they have been merged into the same class, because it's not necessary (at this time) to differentiate them

    public string GetName()
    {
        return name;
    }

    public void SetName(string newName)
    {
        name = newName;
    }

    public Calendar GetCalendar()
    {
        return calendar;
    }

    public void SetCalendar(Calendar c)
    {
        calendar = c;
    }

}

