using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GlobalVars;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ClassSelectionManager : MonoBehaviour
{
    private CourseScriptableObject[] fullCourseList;
    // private List<ClubScriptableObject> fullClubList;

    private int courseButtonIndex = 0;

    public SerializableDictionary<TextMeshProUGUI, Course> menuButtons =
        new SerializableDictionary<TextMeshProUGUI, Course>();

    // grid container for courses
    [SerializeField] private GameObject obj_courseLayout;

    // grid container for clubs
    [SerializeField] private GameObject obj_clubLayout;

    // registration button prefabrication
    [SerializeField] private GameObject obj_registrationButton;

    // displays right below where courses are selected when a conflicting course is selected, displaying the conflicting course(s)
    [SerializeField] private GameObject obj_timeConflictNotifier;

    // displays number of courses left to choose!
    [SerializeField] private GameObject obj_courseSelectionTitle;

    [SerializeField] private GameObject obj_continueButton; // (submit button, only allowed to press when 3 courses have been registered)
    
    [SerializeField] private GameObject obj_calendarTitle;

    // this will be a temporary calendar, not the actual final calendar for the player, just to keep track of the
    // courses that are added here.
    // so we don't have to unnecessarily waste time trying to format the showing of the classes and can just use the
    // calendar's already implemented capabilities
    private Calendar calendar;
    private ClassSelectionCalendarDisplay calendarDisplay;

    [SerializeField] private int numCoursesRegistered = 0;

    void Start()
    {
        calendar = GetComponent<Calendar>();
        calendarDisplay = GetComponent<ClassSelectionCalendarDisplay>();
        obj_courseSelectionTitle.GetComponent<TextMeshProUGUI>().text =
            $"Select your courses! {3 - numCoursesRegistered} courses left to choose!";
        obj_calendarTitle.SetActive(true);

        // fullClubList = Resources.LoadAll("Clubs").Cast<Club>().ToArray();
        fullCourseList = Resources.LoadAll("Courses").Cast<CourseScriptableObject>().ToArray();
        foreach (CourseScriptableObject course in fullCourseList)
        {
            Button button = Instantiate(obj_registrationButton, obj_courseLayout.transform).GetComponent<Button>();
            button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = course.name;
            button.GetComponent<RegistrationButton>().course = course;
            button.onClick.AddListener(delegate { OnButtonPress(button.gameObject, course); });
            button.gameObject.name = $"b_{course.name}";
            Debug.Log($"created button {button.gameObject.name}, for course {course.name}");
        }

        obj_continueButton.GetComponent<Button>().interactable = false;
        obj_continueButton.GetComponent<Button>().onClick.AddListener(FinishClassSelection);

        // foreach (ClubScriptableObject club in fullClubList)
        // {
        //     currentButton = Instantiate(obj_registrationButton, obj_courseLayout.transform).GetComponent<Button>();
        //     currentButton.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = club.clubName;
        //     currentButton.onClick.AddListener(delegate { AddOrRemoveClub(club); });
        // }
    }

    // function that each button calls when it's pressed. keeps a maximum of three courses that can be registered
    // at the same time. also adds the courses to the calendar when pressed and able
    private void OnButtonPress(GameObject button, CourseScriptableObject course)
    {
        // COURSE REMOVAL
        if (calendar.HasCourse(course) && numCoursesRegistered is <= 3 and > 0)
        {
            calendar.RemoveCourse(course);
            numCoursesRegistered--;
            button.GetComponent<Button>().image.color = Color.white;
            calendarDisplay.UpdateCalendar(course, false);
            
            // deactivate time conflict notifier when removing a course, i think the player gets it by now
            if (obj_timeConflictNotifier.activeSelf)
            {
                obj_timeConflictNotifier.SetActive(false);
            }

            if (numCoursesRegistered == 2)
            {
                DeactivateContinueButton();
            }
            // re-enable title just in case
            else if (numCoursesRegistered == 0)
            {
                obj_calendarTitle.SetActive(true);
            }
        }
        // COURSE ADDING
        else if (!calendar.HasCourse(course) && numCoursesRegistered < 3)
        {
            // check for conflicts
            var conflictList = CheckConflicts(course);
            if (conflictList.Count > 0)
            {
                Debug.Log($"uh oh! that's a conflict!");
                var textMeshPro = obj_timeConflictNotifier.GetComponent<TextMeshProUGUI>();
                var conflictString = "";
                conflictString += "Time conflict! Add a different course or remove ";
                foreach (var conflict in conflictList)
                {
                    if (conflict == conflictList.Last())
                    {
                        conflictString += conflict.name;
                    }
                    else
                    {
                        conflictString += conflict.name + ", ";
                    }
                }
                textMeshPro.text = conflictString;
                
                obj_timeConflictNotifier.SetActive(true);
                return;
            }

            calendar.AddCourse(course);
            numCoursesRegistered++;
            button.GetComponent<Button>().image.color = Color.red;
            calendarDisplay.UpdateCalendar(course, true);
            
            obj_calendarTitle.SetActive(false);
            
            // deactivate time conflict notifier when adding a course, i think the player gets it by now
            if (obj_timeConflictNotifier.activeSelf)
            {
                obj_timeConflictNotifier.SetActive(false);
            }

            if (numCoursesRegistered == 3)
            {
                ActivateContinueButton();
            }
        }

        obj_courseSelectionTitle.GetComponent<TextMeshProUGUI>().text =
            $"Select your courses! {3 - numCoursesRegistered} courses left to choose!";
    }

    private List<CourseScriptableObject> CheckConflicts(CourseScriptableObject course)
    {
        List<CourseScriptableObject> conflictingCourseList = new List<CourseScriptableObject>();
        foreach (var time in course.times)
        {
            foreach (var day in course.days)
            {
                if (calendar.GetCourseAtTime(time, day) != null && !conflictingCourseList.Contains(calendar.GetCourseAtTime(time, day)))
                {
                    conflictingCourseList.Add(calendar.GetCourseAtTime(time, day));
                }
            }
        }

        return conflictingCourseList;
    }

    private void ActivateContinueButton()
    {
        Button continueButton = obj_continueButton.GetComponent<Button>();
        continueButton.interactable = true;
    }

    private void DeactivateContinueButton()
    {
        Button continueButton = obj_continueButton.GetComponent<Button>();
        continueButton.interactable = false;
    }

    private void FinishClassSelection()
    {
        Debug.Log("congratulations you finished class selection :3");
        // TODO switch scene, add calendar to player
    }
}