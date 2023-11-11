using System.Collections;
using System.Collections.Generic;
using Unity.Jobs.LowLevel.Unsafe;
using UnityEngine;

// COPY AND PASTE THIS INTO THE FINAL RESTING PLACE OF CALENDAR AND PLAYERCALENDAR (PROBABLY IN A GLOBAL SCRIPT OR SOMETHING IDK) (OR MAYBE IN THE MASCOT CLASS BUT SINCE THE PLAYER ALSO INHERITS CALENDAR IDK)

// FIXME stub, add real locations
public enum Location {here, there, wayOverThere};

public class Calendar
{
    
    // 3x7 2d array, each storing location enum
    // first dimension is time, second is day, i.e. afternoon on thursday would be schedule[2, 5] (?)
    private Location[,] schedule = new Location[3, 7];

    // Updates the variable schedule by iterating through the Mascot’s courses and adding them based on time and date. -- from tech design doc
    public void UpdateSchedule() {
        // FIXME stub for now, mascot does not have any courses to iterate through
        //  there is also no mascot
        // FIXME iterate through player's courses?/classes?/clubs? overload this function in player or smth
        return;
    }

    // Given a time and a date, returns the Mascot's location. -- from tech design doc
    public Location GetSchedule(int time, int date) {
        // FIXME stub, return location at that time and date
        return schedule[time, date];
    }

}

public class PlayerCalendar : Calendar {
    /* Responsibilities: -- from tech design doc
        Stores a class schedule and club schedule for the Player. 
        Can update and get Class Schedules as needed.
    */ 

    // might not actually need to be arrays of int lists. is probably Classes/Courses or something
    List<int>[] classSchedule;
    List<int>[] clubSchedule;

    void UpdateClassSchedule() {
        // FIXME idk what this is supposed to do
        return;
    }

    List<int>[] GetClassSchedule() {
        return classSchedule;
    }

    void UpdateClubSchedule() {
        // FIXME idk what this is supposed to do
        return;
    }

    List<int>[] GetClubSchedule() {
        return clubSchedule;
    }
}