using GlobalVars;
using TMPro;
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
    
    public CourseScriptableObject courseAtTime;
    public ClubScriptableObject clubAtTime;

    // Start is called before the first frame update
    void Start()
    {
        tmp_currentTimeDisplay = obj_currentTimeDisplay.GetComponent<TextMeshProUGUI>();
        btn_progressTime = obj_progressTime.GetComponent<Button>();

        btn_progressTime.onClick.AddListener(delegate {ProgressTime();});

        currentDay = Day.Sunday;
        currentTimeSlot = TimeSlot.morning;
    }

    // Update is called once per frame
    void Update()
    {
        var dayString = Calendar.DayToString(currentDay);
        var timeString = Calendar.TimeToString(currentTimeSlot);
        var displayText = string.Format($"current time: {timeString} on {dayString}\n");

        displayText += "current activity: ";
        courseAtTime = calendar.GetCourseAtTime(currentTimeSlot, currentDay);
        clubAtTime = calendar.GetClubAtTime(currentTimeSlot, currentDay);
        var courseName = "";
        var clubName = "";
        var locationName = "none";

        bool clubWasNull = false;

        // check club first so that course overwrites club information if there's a conflict (don't skip class kids)
        if (clubAtTime) {
            clubName = clubAtTime.name;
            locationName = Calendar.LocationToString(clubAtTime.location);
        }
        else clubWasNull = true;

        if (courseAtTime) {
            courseName = courseAtTime.name;
            locationName = Calendar.LocationToString(courseAtTime.location);
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
            currentTimeSlot = TimeSlot.morning;
            if (currentDay == Day.Saturday) currentDay = Day.Sunday;
            else currentDay++;
        }
        else currentTimeSlot++;
    }
}
