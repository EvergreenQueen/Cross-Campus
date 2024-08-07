using UnityEngine;
using GlobalVars;
using System.Collections.Generic;
using Ink.Parsed;

// data structure for keeping track of a mascot/player's Clubs and Courses
// use AddCourse(course) / AddClub(club) to add courses/clubs to this calendar
// automatically assigns the course/club's times to the correct time/day slot in this calendar
// use GetCourseAtTime(timeSlot, day) / GetClubAtTime(timeSlot, day) to get the course/club stored at that time and day
// use GetCoursesOnDay(day) / GetClubsOnDay(day) to get a list of the courses/clubs stored on that day, in order of time

// can also be used to give a Mascot certain locations at certain times, without forcing it to have a course/club at
// that time. 

[System.Serializable]
public class Calendar
{
    // struct which holds the timeslot essentially for the time of day. can have a course, a club, and a location.
    // currently the same time and day can have a course and a club, this is easily changeable if this isn't desired behavior
    public struct Activity
    {
        public CourseScriptableObject course;
        public ClubScriptableObject club;
        public Location location;
    }

    // 2d array holding activities for each time/day combination
    // indexed using ints instead of [Time, Day] because it's an array. the time/day are casted to ints to retrieve the value at the indices
    public Activity[,] schedule = new Activity[3, 7];

    // store courses that the player is in
    public List<CourseScriptableObject> courses = new List<CourseScriptableObject>();
    // store clubs that the player is in
    public List<ClubScriptableObject> clubs = new List<ClubScriptableObject>();
    
    /// <summary>
    /// UnEncode time/day/location enums to the strings they correspond to. this doesn't have to be in this class.. probably.. 
    /// </summary>
    public static string TimeToString(TimeSlot time)
    {
        // fancy schmancy c# switch expression woag
        return time switch
        {
            TimeSlot.morning => "Morning",
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
        // public enum Location {none, pool, library, hub, gym, forest, classroom, botanical_gardens}; // naming conventions not final
        return location switch
        {
            Location.none => "none",
            Location.pool => "pool",
            Location.library => "library",
            Location.hub => "hub",
            Location.gym => "gym",
            Location.forest => "forest",
            Location.classroom => "classroom",
            Location.botanical_gardens => "botanical gardens",
            _ => "invalid input!"
        };
    }

    /// <summary>
    /// return List of Courses this Calendar has
    /// </summary>
    public List<CourseScriptableObject> GetCourses() 
    {
        return courses;
    }
    
    /// <summary>
    /// return List of Clubs this Calendar has
    /// </summary>
    public List<ClubScriptableObject> GetClubs() 
    {
        return clubs;
    }
    
    /// <summary>
    /// add Course course to this Calendar
    /// </summary>
    public void AddCourse(CourseScriptableObject course) 
    {
        // set activity's course and location to that of the course
        // add all course times and days
        foreach (var timeSlot in course.times) {
            foreach (var day in course.days) {
                schedule[(int)timeSlot, (int)day].course = course;
                schedule[(int)timeSlot, (int)day].location = course.location;
            }
        }
        
        // since this method is also used to add existing courses to the calendar, check if the
        // list of courses doesn't already contain the course we're adding
        if (!courses.Contains(course)) courses.Add(course);
        Debug.Log($"[calendar] added course {course.name} to calendar");
        Debug.Log($"[calendar] course list is now {courses}");
        
        Verify();
    }
    
    /// <summary>
    /// remove Course course to this Calendar
    /// </summary>
    public void RemoveCourse(CourseScriptableObject course) 
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
        Debug.Log($"removed course {course.name} from calendar");
        
        Verify();
    }
    /// <summary>
    /// add Club club to this Calendar
    /// </summary>
    public void AddClub(ClubScriptableObject club) 
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
        
        // since this method is also used to add existing clubs to the calendar, check if the
        // list of clubs doesn't already contain the club we're adding
        if (!clubs.Contains(club)) clubs.Add(club);
        Debug.Log($"added club {club.name} to calendar");
        
        Verify();
    }
    
    /// <summary>
    /// remove Club club from this Calendar
    /// </summary>
    public void RemoveClub(ClubScriptableObject club) 
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
        Debug.Log($"removed club {club.name} from calendar");
        
        Verify();
    }
    
    /// <summary>
    /// re-check through added courses/clubs in the calendar and add them.
    /// basically, a way to work around the problem of overwriting calendar entries.
    /// will be called after adding/removing anything from the calendar, so a little inefficient but we DO NOT CARE
    /// </summary>
    public void Verify()
    {
        foreach (var course in courses)
        {
            foreach (var timeSlot in course.times)
            {
                foreach (var day in course.days)
                {
                    schedule[(int)timeSlot, (int)day].course = course;
                    schedule[(int)timeSlot, (int)day].location = course.location;
                    schedule[(int)timeSlot, (int)day].club = null;
                }
            }
        }

        foreach (var club in clubs)
        {
            foreach (var timeSlot in club.times)
            {
                foreach (var day in club.days)
                {
                    schedule[(int)timeSlot, (int)day].club = club;
                    schedule[(int)timeSlot, (int)day].course = null;
                    schedule[(int)timeSlot, (int)day].location = club.location;
                }
            }
        }
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
        
        Debug.Log($"location at time {TimeToString(time)} and day {DayToString(day)} set to {LocationToString(location)}");
    }
    
    /// <summary>
    /// returns true if this Calendar contains designated Course
    /// </summary>
    public bool HasCourse(CourseScriptableObject course)
    {
        return courses.Contains(course);
    }
    
    /// <summary>
    /// returns true if this Calendar contains designated Club
    /// </summary>
    public bool HasClub(ClubScriptableObject club) 
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
    public CourseScriptableObject GetCourseAtTime(TimeSlot time, Day day) 
    {
        return schedule[(int)time, (int)day].course;
    }

    /// <summary>
    /// returns List of Courses on Day day (Course at each time slot in the day) in order of time
    /// </summary>
    /// <remarks> elements of the List will be *null* if there is no Course at that time slot </remarks>
    public List<CourseScriptableObject> GetCoursesOnDay(Day day)
    {
        List<CourseScriptableObject> courseList = new List<CourseScriptableObject>();
        for (int i = 0; i < 3; i++) {
            courseList.Add(schedule[i, (int)day].course);
        }
        return courseList;
    }
    
    /// <summary>
    /// returns Club at Time time and Day day for this calendar
    /// </summary>
    /// <remarks> returns null if there is no club at that time and day </remarks>
    public ClubScriptableObject GetClubAtTime(TimeSlot time, Day day)
    {
        return schedule[(int)time, (int)day].club;
    }

    /// <summary>
    /// returns List of Clubs on Day day (Club at each time slot in the day) in order of time
    /// </summary>
    /// <remarks> elements of the List will be *null* if there is no Course at that time slot </remarks>
    public List<ClubScriptableObject> GetClubsOnDay(Day day)
    {
        List<ClubScriptableObject> clubList = new List<ClubScriptableObject>();
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
        
        // add scriptable objects to the schedule
        foreach (var course in courses)
        {
            AddCourse(course);
        }

        foreach (var club in clubs)
        {
            AddClub(club);
        }
    }
}