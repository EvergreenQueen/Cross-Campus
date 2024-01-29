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
// use AddCourse(course) / AddClub(club) to add courses/clubs to this calendar
// automatically assigns the course/club's times to the correct time/day slot in this calendar
// use GetCourseAtTime(timeSlot, day) / GetClubAtTime(timeSlot, day) to get the course/club stored at that time and day
// use GetCoursesOnDay(day) / GetClubsOnDay(day) to get a list of the courses/clubs stored on that day, in order of time
// currently cannot add multiple courses/clubs at the same time

public class Calendar : MonoBehaviour
{
    // struct which holds the timeslot essentially for the time of day. can have a course, a club, and a location.
    // currently the same time and day can have a course and a club, this is easily changeable if this isn't desired behavior

    // * perhaps make club and course subclasses of an activity superclass. so that each schedule slot can just have an activity. idk
    // * and/or mascot events/dates are also things that can be in the activity struct
    public struct Activity
    {
        public Course course;
        public Club club;
        public Location location;
    }

    // 2d array holding activities for each time/day combination
    // indexed using ints instead of [Time, Day] because it's an array. the time/day are casted to ints to retrieve the value at the indices
    public Activity[,] schedule = new Activity[3, 7];

    // store courses that the player is in
    private List<Course> courses;
    // store clubs that the player is in
    private List<Club> clubs;
    
    // member functions

    // UnEncode time/day/location enums to the strings they correspond to. this doesn't have to be in this class.. probably.. (can't put it in GlobalVars because you can't put functions in namespaces)
    // * the solution is to probably make time, day, and location into their own classes
    public string TimeToString(TimeSlot time) {
        if (time == TimeSlot.midday) return "midday";
        if (time == TimeSlot.afternoon) return "afternoon";
        if (time == TimeSlot.evening) return "evening";
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
        if (location == Location.physics_2000) return "physics 2000";
        if (location == Location.et_cetera) return "et cetera";
        return "invalid input!";
    }

    // return List of Courses this Calendar has
    public List<Course> GetCourses() 
    {
        return courses;
    }

    // return List of Clubs this Calendar has
    public List<Club> GetClubs() 
    {
        return clubs;
    }

    // add Course course to this Calendar
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
    
    // remove Course course to this Calendar
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

    // add Club club to this Calendar
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

    // remove Club club from this Calendar
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

    // returns true if this Calendar contains designated Course
    public bool HasCourse(Course course)
    {
        return courses.Contains(course);
    }

    // returns true if this Calendar contains designated Club
    public bool HasClub(Club club) 
    {
        return clubs.Contains(club);
    }

    // returns Location enum of the Activity (Club or Course) at Time time and Day day for this Calendar
    // if there is no Activity for that time/day combination, this returns Location.none
    public Location GetLocation(TimeSlot time, Day day)
    {
        return schedule[(int)time, (int)day].location;
    }

    // returns Course at Time time and Day day for this Calendar
    // NOTE: returns null if there is no course at that time and day
    public Course GetCourseAtTime(TimeSlot time, Day day) 
    {
        return schedule[(int)time, (int)day].course;
    }

    // returns List of Courses on Day day (Course at each time slot in the day) in order of time
    // NOTE: elements of the List will be *null* if there is no Course at that time slot
    public List<Course> GetCoursesOnDay(Day day)
    {
        List<Course> courseList = new List<Course>();
        for (int i = 0; i < 3; i++) {
            courseList.Add(schedule[i, (int)day].course);
        }
        return courseList;
    }

    // returns Club at Time time and Day day for this calendar
    // NOTE: returns null if there is no club at that time and day
    public Club GetClubAtTime(TimeSlot time, Day day)
    {
        return schedule[(int)time, (int)day].club;
    }

    // returns List of Clubs on Day day (Club at each time slot in the day) in order of time
    // NOTE: elements of the List will be *null* if there is no Course at that time slot
    public List<Club> GetClubsOnDay(Day day)
    {
        List<Club> clubList = new List<Club>();
        for (int i = 0; i < 3; i++) {
            clubList.Add(schedule[i, (int)day].club);
        }
        return clubList;
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
}