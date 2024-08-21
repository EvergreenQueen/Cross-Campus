using GlobalVars;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    //Serialized
    [SerializeField] private Canvas uiCanvas; // might wanna do this thru a ui manager tbh
    [SerializeField] private TextMeshProUGUI timeAndDayPlaceHolder;
    [SerializeField] private GameObject eventPopUpPrefab;
    [SerializeField] private TextAsset basicClassStory;

    //Runtime variables
    private EventPopUp eventPopUp;

    //References
    private LocationManager locationManager;

    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        // If there is an instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {
        locationManager = LocationManager.GetInstance();
    }

    public void UpdateTimeAndDayGUI(TimeSlot currentTimeSlot, Day currentDay)
    {
        timeAndDayPlaceHolder.text = Calendar.TimeToString(currentTimeSlot) + ", " + Calendar.DayToString(currentDay);
    }

    public void ShowNotificationPopup(CourseScriptableObject course)
    {
        var eventPopUpObject = Instantiate(eventPopUpPrefab, uiCanvas.transform);
        // eventPopUpObject.transform.position = 
        eventPopUp = eventPopUpObject.GetComponent<EventPopUp>();
        eventPopUp.SetText($"You have {course.name} right now! Go to class? (skipping class may result in bad grades)");
        eventPopUp.AssignButtonEvents(() =>
        {
            // accept callback
            locationManager.SaveStateAndJumpToStory(basicClassStory);
        }, () =>
        {
            locationManager.SkipClass(course);
            Destroy(eventPopUp.gameObject);
        });


    }
    public void CloseNotificationPopup()
    {
        Destroy(eventPopUp.gameObject);
    }
}
