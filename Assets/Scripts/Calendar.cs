using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using GlobalVars;
using System.Linq;
using UnityEditor.Rendering.Universal;
using System.Globalization;
using System.Diagnostics.Tracing;
using System.ComponentModel;
using System.Collections.Generic;

// data structure for keeping track of a mascot/player's Clubs and Courses
// 

public class Calendar : MonoBehaviour
{
    // struct which holds the timeslot essentially for the time of day. can have a course or a club
    // perhaps make club and course subclasses of an activity superclass. so that each schedule slot can just have an activity. idk
    // and/or mascot events/dates are also things that can be in the activity struct
    public struct Activity
    {
        public Course course;
        public Club club;
        public Location location;
    }

    public Activity[,] schedule = new Activity[3, 7];

    // store courses that the player is in
    private List<Course> courses;
    // store clubs that the player is in
    private List<Club> clubs;
    
    // member functions

    // UnEncode time and day integers to the strings they correspond to. this doesn't have to be in this class.. probably..
    public string TimeToString(GlobalVars.Time time) {
        if (time == GlobalVars.Time.midday) return "midday";
        if (time == GlobalVars.Time.afternoon) return "afternoon";
        if (time == GlobalVars.Time.evening) return "evening";
        return "invalid input!";
    }

    public string DayToString(Day day) {
        if (day == Day.Sunday) return "Sunday";
        if (day == Day.Monday) return "Monday";
        if (day == Day.Tuesday) return "Tuesday";
        if (day == Day.Wednesday) return "Wednesday";
        if (day == Day.Thursday) return "Thursday";
        if (day == Day.Friday) return "Friday";
        if (day == Day.Saturday) return "Saturday";
        return "invalid input!";
    }

    public string LocationToString(Location location) {
        if (location == Location.none) return "none";
        if (location == Location.belltower) return "belltower";
        if (location == Location.wilcoxs_office) return "wilcox's office";
        if (location == Location.phys2000) return "phys2000";
        if (location == Location.et_cetera) return "et cetera";
        return "invalid input!";
    }

    public List<Course> GetCourses() 
    {
        return courses;
    }

    public List<Club> GetClubs() 
    {
        return clubs;
    }

    public void AddCourse(Course course) 
    {
        // set activity's course and location to that of the course
        // add all course times and days
        for (int i = 0; i < course.times.Count; i++) {
            for (int j = 0; j < course.days.Count; j++) {
                schedule[(int)course.times[i], (int)course.days[j]].course = course;
                schedule[(int)course.times[i], (int)course.days[j]].location = course.location;
            }
        }

        courses.Add(course);
    }
    
    public void RemoveCourse(Course course)
    {
        // remove time slots in every day that the course is in
        for (int i = 0; i < course.times.Count; i++) {
            for (int j = 0; j < course.days.Count; j++) {
                var tempCourse = schedule[(int)course.times[i], (int)course.days[j]].course;
                if (tempCourse == course) schedule[(int)course.times[i], (int)course.days[j]].course = null;
                var tempLocation = schedule[(int)course.times[i], (int)course.days[j]].location;
                if (tempCourse == course) schedule[(int)course.times[i], (int)course.days[j]].location = Location.none;
            }
        }

        courses.Remove(course);
    }

    public void AddClub(Club club) 
    {
        // set activity's club and location to that of the club
        // add all club times and days
        // does overwrite club that was there before
        for (int i = 0; i < club.times.Count; i++) {
            for (int j = 0; j < club.days.Count; j++) {
                schedule[(int)club.times[i], (int)club.days[j]].club = club;
                schedule[(int)club.times[i], (int)club.days[j]].location = club.location;
            }
        }

        clubs.Add(club);
    }

    public void RemoveClub(Club club)
    {
        // remove time slots in every day that the club is in
        for (int i = 0; i < club.times.Count; i++) {
            for (int j = 0; j < club.days.Count; j++) {
                // don't overwrite something that overwrote the timeslot previously
                var tempClub = schedule[(int)club.times[i], (int)club.days[j]].club;
                if (tempClub == club) schedule[(int)club.times[i], (int)club.days[j]].club = null;
                var tempLocation = schedule[(int)club.times[i], (int)club.days[j]].location;
                if (tempClub == club) schedule[(int)club.times[i], (int)club.days[j]].location = Location.none;
            }
        }

        clubs.Remove(club);
    }

    public Location GetLocation(GlobalVars.Time time, GlobalVars.Day day)
    {
        return schedule[(int)time, (int)day].location;
    }

    
    // NOTE: returns null if there is no course at that time and day
    public Course GetCourseAtTime(GlobalVars.Time time, GlobalVars.Day day) 
    {
        return schedule[(int)time, (int)day].course;
    }

    public List<Course> GetCoursesOnDay(GlobalVars.Day day)
    {
        List<Course> courseList = new List<Course>();
        for (int i = 0; i < 3; i++) {
            courseList.Add(schedule[i, (int)day].course);
        }
        return courseList;
    }

    // NOTE: returns null if there is no club at that time and day
    public Club GetClubAtTime(GlobalVars.Time time, GlobalVars.Day day)
    {
        return schedule[(int)time, (int)day].club;
    }

    public List<Club> GetClubsOnDay(GlobalVars.Day day)
    {
        List<Club> clubList = new List<Club>();
        for (int i = 0; i < 3; i++) {
            clubList.Add(schedule[i, (int)day].club);
        }
        return clubList;
    }

    // format course to a string (with multiple lines!)
    public string GetCoursesToString() {
        string str = "";
        for (int i = 0; i < courses.Count; i++) {
            string times = "";
            for (int j = 0; j < courses[i].times.Count - 1; j++) {
                times += TimeToString((GlobalVars.Time)courses[i].times[j]) + ", ";
            }
            times += TimeToString((GlobalVars.Time)courses[i].times[courses[i].times.Count - 1]);

            string days = "";
            for (int j = 0; j < courses[i].days.Count; j++) {
                days += DayToString((GlobalVars.Day)courses[i].days[j]) + ", ";
            }
            days += DayToString((GlobalVars.Day)courses[i].days[courses[i].days.Count - 1]);

            string location = LocationToString(courses[i].location);

            str += "course: " + courses[i].name +
                   "\ntimes: " + times + 
                   "\ndays: " + days + 
                   "\nlocation: " + location + 
                   "\n";
        }
        return str;
    }

    void Start() 
    {
        // clear schedule
        for (int i = 0; i < 3; i++) {
            for (int j = 0; j < 7; j++) {
                schedule[i, j].course = null;
                schedule[i, j].club = null;
                schedule[i, j].location = Location.none;
            }
        }

        courses = new List<Course>();
        clubs = new List<Club>();
    }

    void Update()
    {
        
    }
}