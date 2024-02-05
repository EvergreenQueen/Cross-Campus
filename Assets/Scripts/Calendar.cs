using UnityEngine;
using GlobalVars;
using System.Collections.Generic;

// data structure for keeping track of a mascot/player's Clubs and Courses
// use AddCourse(course) / AddClub(club) to add courses/clubs to this calendar
// automatically assigns the course/club's times to the correct time/day slot in this calendar
// use GetCourseAtTime(timeSlot, day) / GetClubAtTime(timeSlot, day) to get the course/club stored at that time and day
// use GetCoursesOnDay(day) / GetClubsOnDay(day) to get a list of the courses/clubs stored on that day, in order of time

// can also be used to give a Mascot certain locations at certain times, without forcing it to have a course/club at
// that time. 

public class Calendar : MonoBehaviour
{
    // struct which holds the timeslot essentially for the time of day. can have a course, a club, and a location.
    // currently the same time and day can have a course and a club, this is easily changeable if this isn't desired behavior
    private struct Activity
    {
        public Course course;
        public Club club;
        public Location location;
    }

    // 2d array holding activities for each time/day combination
    // indexed using ints instead of [Time, Day] because it's an array. the time/day are casted to ints to retrieve the value at the indices
    private Activity[,] schedule = new Activity[3, 7];

    // store courses that the player is in
    public List<Course> courses = new();
    // store clubs that the player is in
    public List<Club> clubs = new();
    
    /// <summary>
    /// UnEncode time/day/location enums to the strings they correspond to. this doesn't have to be in this class.. probably.. 
    /// </summary>
    public static string TimeToString(TimeSlot time)
    {
        // fancy schmancy c# switch expression woag
        return time switch
        {
            TimeSlot.midday => "Midday",
            TimeSlot.afternoon => "Afternoon",
            TimeSlot.evening => "Evening",
            _ => "invalid input!"
        };
    }

    public static string DayToString(Day day) 
    {
        return day switch
        {
            Day.Sunday => "Sunday",
            Day.Monday => "Monday",
            Day.Tuesday => "Tuesday",
            Day.Wednesday => "Wednesday",
            Day.Thursday => "Thursday",
            Day.Friday => "Friday",
            Day.Saturday => "Saturday",
            _ => "invalid input!"
        };
    }

    public static string LocationToString(Location location) 
    {
        return location switch
        {
            Location.none => "none",
            Location.belltower => "belltower",
            Location.wilcoxs_office => "wilcox's office",
            Location.physics_2000 => "physics 2000",
            Location.et_cetera => "et cetera",
            _ => "invalid input!"
        };
    }

    /// <summary>
    /// return List of Courses this Calendar has
    /// </summary>
    public List<Course> GetCourses() 
    {
        return courses;
    }
    
    /// <summary>
    /// return List of Clubs this Calendar has
    /// </summary>
    public List<Club> GetClubs() 
    {
        return clubs;
    }
    
    /// <summary>
    /// add Course course to this Calendar
    /// </summary>
    public void AddCourse(Course course) 
    {
        // set activity's course and location to that of the course
        // add all course times and days
        foreach (var timeSlot in course.times) {
            foreach (var day in course.days) {
                schedule[(int)timeSlot, (int)day].course = course;
                schedule[(int)timeSlot, (int)day].location = course.location;
            }
        }

        courses.Add(course);
    }
    
    /// <summary>
    /// remove Course course to this Calendar
    /// </summary>
    public void RemoveCourse(Course course) 
    {
        // remove time slots in every day that the course is in
        foreach (var timeSlot in course.times) {
            foreach (var day in course.days) {
                var tempCourse = schedule[(int)timeSlot, (int)day].course;
                if (tempCourse == course) schedule[(int)timeSlot, (int)day].course = null;
                var tempLocation = schedule[(int)timeSlot, (int)day].location;
                if (tempCourse == course) schedule[(int)timeSlot, (int)day].location = Location.none;
            }
        }

        courses.Remove(course);
    }
    /// <summary>
    /// add Club club to this Calendar
    /// </summary>
    public void AddClub(Club club) 
    {
        // set activity's club and location to that of the club
        // add all club times and days
        // does overwrite club that was there before
        foreach (var timeSlot in club.times) {
            foreach (var day in club.days) {
                schedule[(int)timeSlot, (int)day].club = club;
                schedule[(int)timeSlot, (int)day].location = club.location;
            }
        }

        clubs.Add(club);
    }
    
    /// <summary>
    /// remove Club club from this Calendar
    /// </summary>
    public void RemoveClub(Club club) 
    {
        // remove time slots in every day that the club is in
        foreach (var timeSlot in club.times) {
            foreach (var day in club.days) {
                // don't overwrite something that overwrote the timeslot previously
                var tempClub = schedule[(int)timeSlot, (int)day].club;
                if (tempClub == club) schedule[(int)timeSlot, (int)day].club = null;
                var tempLocation = schedule[(int)timeSlot, (int)day].location;
                if (tempClub == club) schedule[(int)timeSlot, (int)day].location = Location.none;
            }
        }

        clubs.Remove(club);
    }
    
    /// <summary>
    /// set location Location to Time time and Day day. this is so give something a location without forcing it to have
    /// a course or club at that time/day. for mascots. it's for mascots.
    /// </summary>
    /// <remarks>
    /// also overrides course and club at that time/day.
    /// </remarks>
    public void SetLocation(TimeSlot time, Day day, Location location)
    {
        Activity ac = schedule[(int)time, (int)day];
        ac.location = location;
        ac.course = null;
        ac.club = null;
    }
    
    /// <summary>
    /// returns true if this Calendar contains designated Course
    /// </summary>
    public bool HasCourse(Course course)
    {
        return courses.Contains(course);
    }
    
    /// <summary>
    /// returns true if this Calendar contains designated Club
    /// </summary>
    public bool HasClub(Club club) 
    {
        return clubs.Contains(club);
    }
    
    /// <summary>
    /// returns Location enum of the Activity (Club or Course) at Time time and Day day for this Calendar
    /// if there is no Activity for that time/day combination, this returns Location.none
    /// </summary>
    public Location GetLocation(TimeSlot time, Day day)
    {
        return schedule[(int)time, (int)day].location;
    }
    
    /// <summary>
    /// returns Course at Time time and Day day for this Calendar
    /// </summary>
    /// <remarks> returns null if there is no course at that time and day </remarks>
    public Course GetCourseAtTime(TimeSlot time, Day day) 
    {
        return schedule[(int)time, (int)day].course;
    }

    /// <summary>
    /// returns List of Courses on Day day (Course at each time slot in the day) in order of time
    /// </summary>
    /// <remarks> elements of the List will be *null* if there is no Course at that time slot </remarks>
    public List<Course> GetCoursesOnDay(Day day)
    {
        List<Course> courseList = new List<Course>();
        for (int i = 0; i < 3; i++) {
            courseList.Add(schedule[i, (int)day].course);
        }
        return courseList;
    }
    
    /// <summary>
    /// returns Club at Time time and Day day for this calendar
    /// </summary>
    /// <remarks> returns null if there is no club at that time and day </remarks>
    public Club GetClubAtTime(TimeSlot time, Day day)
    {
        return schedule[(int)time, (int)day].club;
    }

    /// <summary>
    /// returns List of Clubs on Day day (Club at each time slot in the day) in order of time
    /// </summary>
    /// <remarks> elements of the List will be *null* if there is no Course at that time slot </remarks>
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
    }
}