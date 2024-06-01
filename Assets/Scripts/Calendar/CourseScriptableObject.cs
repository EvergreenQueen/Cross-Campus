using System;
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
    public int timesSkipped = 0;

    public CourseScriptableObject(string name, List<Day> days, List<TimeSlot> times, Location location, int grade)
    {
        this.name = name;
        this.days = days;
        this.times = times;
        this.location = location;
        this.grade = grade;
    }

    // decrease grade by amount
    // i think it'll be on a scale from 0 to 100 as like a percentage, we can do a scale of like
        // A+: 98-100
        // A: 94-97
        // A-: 90-93
        // B+: 87-89
        // B: 83-86
        // B-: 80-82
        // C+: 77-79
        // C: 73-76
        // C-: 70-72
        // D+: 67-69
        // D: 63-66
        // D-: 60-62
        // F: 59 or less
    public void DecrementGrade(int amount)
    {
        var old = grade;
        grade = Math.Clamp(grade - amount, 0, 100);
        Debug.Log($"course {name} grade decreased from {old} to {grade}");
    }

    public string GetLetterGrade()
    {
        if (grade >= 98) return "A+";
        if (grade >= 94) return "A";
        if (grade >= 90) return "A-";
        if (grade >= 87) return "B+";
        if (grade >= 83) return "B";
        if (grade >= 80) return "B-";
        if (grade >= 77) return "C+";
        if (grade >= 73) return "C";
        if (grade >= 70) return "C-";
        if (grade >= 67) return "D+";
        if (grade >= 63) return "D";
        if (grade >= 60) return "D-";
        return "F";
    }

    public void Skip()
    {
        timesSkipped++;
    }
}
