using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalVars;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/CourseScriptableObject", order = 1)]
public class CourseScriptableObject : ScriptableObject
{
    public string name;
    public List<Day> days;
    public List<TimeSlot> times;
    public Location location;
    public int grade;
}
