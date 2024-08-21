using GlobalVars;
using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public TimeSlot currentTimeSlot;
    public Day currentDay;

    // Start is called before the first frame update
    private void Awake()
    {
        
    }

    private void UpdateTimeAndDay()
    {
        if (currentTimeSlot == TimeSlot.evening)
        {
            currentTimeSlot = TimeSlot.morning;
            if (currentDay == Day.Saturday) currentDay = Day.Sunday;
            else currentDay++;
        }
        else currentTimeSlot++;
    }

    public void NextTime()
    {
        UpdateTimeAndDay();
        //Insert any code to update other areas
    }
    
}
