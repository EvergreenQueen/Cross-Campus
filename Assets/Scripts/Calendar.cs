using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;
using GlobalVars;

// COPY AND PASTE THIS INTO THE FINAL RESTING PLACE OF CALENDAR AND PLAYERCALENDAR (PROBABLY IN A GLOBAL SCRIPT OR SOMETHING IDK) (OR MAYBE IN THE MASCOT CLASS BUT SINCE THE PLAYER ALSO INHERITS CALENDAR IDK)

// Calendar is used by BOTH Mascot and Player
// therefore it can't be declared in Mascot or Player class and must be declared somewhere independent of them (its own script?) 
// i'm not sure how you would do this

public class Calendar
{
    
    // 3x7 2d array, each storing location enum
    // first dimension is time, second is day, i.e. afternoon on thursday would be schedule[2, 5] (?)
    private Location[,] schedule = new Location[3, 7];

    // Updates the variable schedule by iterating through the Mascot’s courses and adding them based on time and date. -- from tech design doc
    // THIS RESPONSIBILITY IS THE MASCOT'S NOT THE CALENDAR'S, THIS FUNCTION IS IN MASCOT AND IS CALLED UpdateCourses
    public void UpdateSchedule(Course[] courses) {
        // FIXME stub for now, mascot does not have any courses to iterate through
        //  there is also no mascot
        // FIXME iterate through player's courses?/classes?/clubs? overload this function in player or smth
        return;
    }

    // places location into schedule at specified time, day
    public void AddLocation(int time, int day, Location l)
    {
        schedule[time, day] = l;
    }

    // Given a time and a date, returns the Mascot's location. -- from tech design doc
    // originally called GetSchedule(int, int)
    public Location GetLocation(int time, int date) {
        return schedule[time, date];
    }

}

public class PlayerCalendar : Calendar 
{
    /* Responsibilities: -- from tech design doc
        Stores a class schedule and club schedule for the Player. 
        Can update and get Class Schedules as needed.
    */ 

    // might not actually need to be arrays of int lists. is probably Classes/Courses or something
    List<int>[] classSchedule;
    List<int>[] clubSchedule;

    void UpdateClassSchedule() 
    {
        // FIXME idk what this is supposed to do
        return;
    }

    List<int>[] GetClassSchedule() 
    {
        return classSchedule;
    }

    // iterate thru all clubs that the player is in and update the clubScedule accordingly
    void UpdateClubSchedule() 
    {
        
        return;
    }

    List<int>[] GetClubSchedule() 
    {
        return clubSchedule;
    }
}