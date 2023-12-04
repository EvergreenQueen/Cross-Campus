using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using GlobalVars;
using System.Linq;
using UnityEditor.Rendering.Universal;
using System.Globalization;

// COPY AND PASTE THIS INTO THE FINAL RESTING PLACE OF CALENDAR AND PLAYERCALENDAR (PROBABLY IN A GLOBAL SCRIPT OR SOMETHING IDK) (OR MAYBE IN THE MASCOT CLASS BUT SINCE THE PLAYER ALSO INHERITS CALENDAR IDK)

// Calendar is used by BOTH Mascot and Player
// therefore it can't be declared in Mascot or Player class and must be declared somewhere independent of them (its own script? singleton?) 
// i'm not sure how you would do this

public class Calendar
{
    /* how to use!
        * this class is used in both Player and Mascot to hold their responsibilities! locations are stored in 
        * add courses using UpdateCourseSchedule(newCourses), newCourses is an array of courses. they will be added to the list of courses associated with this Calendar (courseSchedule)
        * add clubs in the same way as above, using an array of clubs
        * cannot remove courses/clubs yet haha
    */

    /* todos:
        * be able to store/obtain type of event at a specific time/day, e.g., i have this time and this day, what course/club and at what location do i have at that time
        * ability to remove/edit courseSchedule? instead of just adding
    */
    
    // 3x7 2d array, each storing location enum
    // first dimension is time, second is day, i.e. afternoon on thursday would be schedule[2, 5] (?)
    private Location[,] schedule = new Location[3, 7];

    // store courses that the player is in
    private Course[] courseSchedule;
    // store clubs that the player is in
    private Club[] clubSchedule;

    public void UpdateCourseSchedule(Course[] newCourses)
    {
        // TODO somehow indicate what responsibility the calendar owner has at the given location in the schedule
        for (int i = 0; i < newCourses.Length; i++) 
        {
            if (!courseSchedule.Contains(newCourses[i])) {
                
                var course = newCourses[i];
                courseSchedule.Append(course);
                for (int j = 0; j < course.times.Length; j++) {
                    for (int k = 0; k < course.dates.Length; k++) {
                        AddLocation(course.times[j], course.dates[k], Location.et_cetera); // TODO TODO FIXME CHANGE THIS TO THE LOCATION OF THE COURSE
                    }
                }
            }
        }
    }

    public Course[] GetCourseSchedule() 
    {
        return courseSchedule;
    }

    public void UpdateClubSchedule(Club[] newClubs)
    {
        for (int i = 0; i < newClubs.Length; i++)
        {
            if (!clubSchedule.Contains(newClubs[i])){
                var club = newClubs[i];
                clubSchedule.Append(club);
                for (int j = 0; j < club.times.Length; j++) {
                    for (int k = 0; k < club.dates.Length; k++) {
                        AddLocation(club.times[j], club.dates[k], Location.et_cetera); // TODO TODO FIXME CHANGE THIS TO THE LOCATION OF THE COURSE
                    }
                }
            }
        }
    }
    public Club[] GetClubSchedule() 
    {
        return clubSchedule;
    }

    public void AddLocation(int time, int day, Location l)
    {
        schedule[time, day] = l;
    }

    public Location GetLocation(int time, int day)
    {
        return schedule[time, day];
    }


}