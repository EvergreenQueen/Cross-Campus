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
    [SerializeField] private GameObject courseLayout;
    [SerializeField] private GameObject clubLayout;
    [SerializeField] private GameObject registrationButton; // registration button prefabrication
    
    // this will be a temporary calendar, not the actual final calendar for the player, just to keep track of the
    // courses that are added here.
    // so we don't have to unnecessarily waste time trying to format the showing of the classes and can just use the
    // calendar's already implemented capabilities
    private Calendar calendar; 

    void Start()
    {
        calendar = GetComponent<Calendar>();
        
        // fullClubList = Resources.LoadAll("Clubs").Cast<Club>().ToArray();
        fullCourseList = Resources.LoadAll("Courses").Cast<CourseScriptableObject>().ToArray();
        foreach (CourseScriptableObject course in fullCourseList)
        {
            Button button = Instantiate(registrationButton, courseLayout.transform).GetComponent<Button>();
            button.transform.Find("Text").GetComponent<TextMeshProUGUI>().text = course.name;
            button.onClick.AddListener(delegate { AddOrRemoveCourse(button.gameObject, course); });
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

    private void AddOrRemoveCourse(GameObject button, CourseScriptableObject course)
    {
        Debug.Log($"called button {button.gameObject.name}'s onClick function!");
        if (calendar.HasCourse(course))
        {
            calendar.RemoveCourse(course);
            button.GetComponent<Button>().image.color = Color.white;
        }
        else
        {
            calendar.AddCourse(course);
            button.GetComponent<Button>().image.color = Color.red;
        }
    }
    
}
