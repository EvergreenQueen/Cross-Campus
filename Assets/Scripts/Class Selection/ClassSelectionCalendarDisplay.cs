using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using GlobalVars;

public class ClassSelectionCalendarDisplay : MonoBehaviour
{
    private Calendar calendar;
    public GameObject obj_calendarDisplay;
    private TextMeshProUGUI calendarText;
    
    // Start is called before the first frame update
    void Start()
    {
        calendar = GetComponent<Calendar>();
        calendarText = obj_calendarDisplay.GetComponent<TextMeshProUGUI>();
    }
    
    // call after changing calendar's courses/clubs to display it different
    public void UpdateCalendar(CourseScriptableObject course, bool adding)
    {
        // THIS IS A VERY BAD WAY TO DO THIS AND I'M AWARE!!!
        foreach (Day day in course.days)
        {
            foreach (TimeSlot time in course.times)
            {
                // find the right gameObject by the name,,, this was a pain to set up and i know it's pretty bad
                var gridTextObject = GameObject.Find(Calendar.DayToString(day) + Calendar.TimeToString(time));
                if (gridTextObject)
                {
                    if (adding) gridTextObject.GetComponent<TextMeshProUGUI>().text = course.name;
                    else gridTextObject.GetComponent<TextMeshProUGUI>().text = "";
                }
            }
        }
    }
}
