using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GlobalVars;
using TMPro;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.UI;

public class ExampleLocationManager : MonoBehaviour
{
    // example location manager for testing purposes (stub stub stub stub stub)
    // "progress time" button increments the time, then retrieves the club and course at that time/day from Calendar

    // set ye olde fields in inspector
    public GameObject obj_progressTime;
    public GameObject obj_currentTimeDisplay;
    public Calendar calendar;

    public TextMeshProUGUI tmp_currentTimeDisplay;
    public Button btn_progressTime;

    private Day currentDay;
    private TimeSlot currentTimeSlot;

    // Start is called before the first frame update
    void Start()
    {
        tmp_currentTimeDisplay = obj_currentTimeDisplay.GetComponent<TextMeshProUGUI>();
        btn_progressTime = obj_progressTime.GetComponent<Button>();

        btn_progressTime.onClick.AddListener(delegate {ProgressTime();});

        currentDay = Day.Sunday;
        currentTimeSlot = TimeSlot.midday;
    }

    // Update is called once per frame
    void Update()
    {
        var dayString = calendar.DayToString(currentDay);
        var timeString = calendar.TimeToString(currentTimeSlot);
        var displayText = string.Format($"current time: {timeString} on {dayString}\n");

        displayText += "current activity: ";
        var courseAtTime = calendar.GetCourseAtTime(currentTimeSlot, currentDay);
        var clubAtTime = calendar.GetClubAtTime(currentTimeSlot, currentDay);
        var courseName = "";
        var clubName = "";
        var locationName = "none";

        bool clubWasNull = false;

        // check club first so that course overwrites club information if there's a conflict (don't skip class kids)
        if (clubAtTime != null) {
            clubName = clubAtTime.name;
            locationName = calendar.LocationToString(clubAtTime.location);
        }
        else clubWasNull = true;

        if (courseAtTime != null) {
            courseName = courseAtTime.name;
            locationName = calendar.LocationToString(courseAtTime.location);
        }
        else if (clubWasNull == true) {
            clubName = "";
            courseName = "free time!";
        }

        displayText += courseName + " " + clubName;
           
        displayText += string.Format($"\ncurrent location: {locationName} ");

        tmp_currentTimeDisplay.text = displayText;
    }

    void ProgressTime()
    {

        if (currentTimeSlot == TimeSlot.evening) {
            currentTimeSlot = TimeSlot.midday;
            if (currentDay == Day.Saturday) currentDay = Day.Sunday;
            else currentDay++;
        }
        else currentTimeSlot++;
    }
}
