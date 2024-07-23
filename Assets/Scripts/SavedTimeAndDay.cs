// used to save the current global time and day when transitioning to and from the campus map scene... to do this
// this object is dontdestroyonload-ed when going from the campusmap scene....
// there is ABSOLUTELY a better way to do this but uhhhhhhhh i couldn't think of it :3

using System.Collections;
using System.Collections.Generic;
using GlobalVars;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/SavedTimeAndDay", order = 1)]
public class SavedTimeAndDay : ScriptableObject
{
    public TimeSlot time;
    public Day day;
}
