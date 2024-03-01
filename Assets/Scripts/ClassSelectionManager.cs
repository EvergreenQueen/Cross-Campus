using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GlobalVars;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClassSelectionManager : MonoBehaviour
{
    private CourseScriptableObject[] fullCourseList;
    // private List<ClubScriptableObject> fullClubList;

    private int courseButtonIndex = 0;
    public SerializableDictionary<TextMeshProUGUI, Course> menuButtons = new SerializableDictionary<TextMeshProUGUI, Course>();
    // grid container for courses
    [SerializeField] private GameObject courseLayout;
    // grid container for clubs
    [SerializeField] private GameObject clubLayout;
    // registration button prefabrication
    [SerializeField] private GameObject registrationButton; 
    
    // this will be a temporary calendar, not the actual final calendar for the player, just to keep track of the
    // courses that are added here.
    // so we don't have to unnecessarily waste time trying to format the showing of the classes and can just use the
    // calendar's already implemented capabilities
    private Calendar calendar; 
    [SerializeField]
    private ClassSelectionCalendarDisplay calendarDisplay;
    
    [SerializeField]
    private GameObject courseSelectionTitle; // displays number of courses left to choose!
    
    private int numCoursesRegistered = 0;

    void Start()
    {
        calendar = GetComponent<Calendar>();
        calendarDisplay = GetComponent<ClassSelectionCalendarDisplay>();
        courseSelectionTitle.GetComponent<TextMeshProUGUI>().text = $"Select your courses! {3 - numCoursesRegistered} courses left to choose!";
        
        // fullClubList = Resources.LoadAll("Clubs").Cast<Club>().ToArray();
        fullCourseList = Resources.LoadAll("Courses").Cast<CourseScriptableObject>().ToArray();
        foreach (CourseScriptableObject course in fullCourseList)
        {
            Button button = Instantiate(registrationButton, courseLayout.transform).GetComponent<Button>();
            button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = course.name;
            button.GetComponent<RegistrationButton>().course = course;
            button.onClick.AddListener(delegate { OnButtonPress(button.gameObject, course); });
            button.gameObject.name = $"b_{course.name}";
            Debug.Log($"created button {button.gameObject.name}, for course {course.name}");
        }
        // foreach (ClubScriptableObject club in fullClubList)
        // {
        //     currentButton = Instantiate(registrationButton, courseLayout.transform).GetComponent<Button>();
        //     currentButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = club.clubName;
        //     currentButton.onClick.AddListener(delegate { AddOrRemoveClub(club); });
        // }
    }

    // function that each button calls when it's pressed. keeps a maximum of three courses that can be registered
    // at the same time. also adds the courses to the calendar when pressed and able
    private void OnButtonPress(GameObject button, CourseScriptableObject course)
    {
        
        if (calendar.HasCourse(course) && numCoursesRegistered is <= 3 and > 0)
        {
            calendar.RemoveCourse(course);
            numCoursesRegistered--;
            button.GetComponent<Button>().image.color = Color.white;
            calendarDisplay.UpdateCalendar(course, false);
            
            
        }
        else if (!calendar.HasCourse(course) && numCoursesRegistered < 3)
        {
            // check for conflicts
            if (CheckConflicts(course))
            {
                Debug.Log($"uh oh! that's a conflict!");
                return;
            }
            calendar.AddCourse(course);
            numCoursesRegistered++;
            button.GetComponent<Button>().image.color = Color.red;
            calendarDisplay.UpdateCalendar(course, true);
        }
        
        courseSelectionTitle.GetComponent<TextMeshProUGUI>().text = $"Select your courses! {3 - numCoursesRegistered} courses left to choose!";
        
    }

    private bool CheckConflicts(CourseScriptableObject course)
    {
        foreach (var time in course.times)
        {
            foreach (var day in course.days)
            {
                if (calendar.GetCourseAtTime(time, day) != null)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
