using System.Collections;
using System.Collections.Generic;
using System.Reflection.Emit;
using UnityEngine;

// script for all global variables and structs that are used by multiple scripts. if there is a better way to do this please enlighten me
namespace GlobalVars
{
    // [CreateAssetMenu(fileName = "NewClub", menuName = "Clubs/ClubScriptableObject")]
    // public class Club : ScriptableObject
    // {
    //     public string name;
    //     public int clubLevel;
    //     public int[] dates;
    //     public int[] times;
    // }

    // [CreateAssetMenu(fileName = "NewCourse", menuName = "Courses/CourseScriptableObject")]
    // public class Course : ScriptableObject
    // {
    //     public string name;
    //     public int[] dates;
    //     public int[] times;
    //     public int grade;
    // }
    // public enum Location {in_class, belltower, hub, wilcoxs_office, et_cetera}; // naming conventions not final

    // supporting classes for Calendar!
    public class Club
    {
        public string name;
        public int clubLevel;
        public List<Day> days;
        public List<GlobalVars.Time> times;
        public Location location;

        public Club(string name, List<GlobalVars.Time> times, List<Day> days, Location location) {
            this.name = name;
            this.days = days;
            this.times = times;
            this.location = location;
        }
    }

    public class Course
    {
        public string name;
        public List<Day> days;
        public List<GlobalVars.Time> times;
        public int grade;
        public Location location;

        public Course(string name, List<GlobalVars.Time> times, List<Day> days, Location location) {
            this.name = name;
            this.days = days;
            this.times = times;
            this.location = location;
        }
    }

    public enum Location {none, belltower, hub, wilcoxs_office, phys2000, et_cetera}; // naming conventions not final
    public enum Time {midday, afternoon, evening};
    public enum Day {Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday};
}
