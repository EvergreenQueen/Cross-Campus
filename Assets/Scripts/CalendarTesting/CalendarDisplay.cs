using GlobalVars;
using TMPro;
using UnityEngine;

public class CalendarDisplay : MonoBehaviour
{
    public GameObject textCoursesDisplay;
    public TextMeshProUGUI tmp_coursesDisplay;
    public GameObject textClubsDisplay;
    public TextMeshProUGUI tmp_clubsDisplay;
    Calendar calendar;

    // Start is called before the first frame update
    void Start()
    {
        calendar = GetComponent<Calendar>();
        tmp_coursesDisplay = textCoursesDisplay.GetComponent<TextMeshProUGUI>();
        tmp_clubsDisplay = textClubsDisplay.GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        // format schedule
        var courseStr = "courses: \n";

        for (var j = Day.Sunday; j <= Day.Saturday; j++) {
            courseStr += Calendar.DayToString(j) + "\t\t";
            if (j == Day.Friday) courseStr += "\t"; // add extra tab to friday to align it better
            courseStr += ": ";      
            var courses = calendar.GetCoursesOnDay(j);
            foreach (var course in courses) {
                if (course != null) {
                    courseStr += course.name + " ";
                }
                else courseStr += "none ";
            }
            courseStr += "\n";
        }

        if (tmp_coursesDisplay)
            tmp_coursesDisplay.text = courseStr;

        var clubStr = "clubs: \n";
        for (var j = Day.Sunday; j <= Day.Saturday; j++) {
            clubStr += Calendar.DayToString(j) + "\t\t";
            if (j == Day.Friday) clubStr += "\t"; // add extra tab to friday to align it better
            clubStr += ": ";            
            var clubs = calendar.GetClubsOnDay(j);
            foreach (var club in clubs) {
                if (club != null) {
                    clubStr += club.name + " ";
                }
                else clubStr += "none ";
            }
            clubStr += "\n";
        }

        if (tmp_clubsDisplay)
            tmp_clubsDisplay.text = clubStr;
    }
}
