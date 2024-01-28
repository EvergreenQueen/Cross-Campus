using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GlobalVars;
using TMPro;
using UnityEditor.Rendering.Universal;
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
            courseStr += calendar.DayToString(j) + "\t\t: ";
            var courses = calendar.GetCoursesOnDay(j);
            for (int i = 0; i < courses.Count; i++) {
                if (courses[i] != null) {
                    courseStr += courses[i].name + " ";
                }
                else courseStr += "none ";
            }
            courseStr += "\n";
        }

        if (tmp_coursesDisplay)
            tmp_coursesDisplay.text = courseStr;

        var clubStr = "clubs: \n";
        for (var j = Day.Sunday; j <= Day.Saturday; j++) {
            clubStr += calendar.DayToString(j) + "\t\t: ";
            var clubs = calendar.GetClubsOnDay(j);
            for (int i = 0; i < clubs.Count; i++) {
                if (clubs[i] != null) {
                    clubStr += clubs[i].name + " ";
                }
                else clubStr += "none ";
            }
            clubStr += "\n";
        }

        if (tmp_clubsDisplay)
            tmp_clubsDisplay.text = clubStr;
    }
}
