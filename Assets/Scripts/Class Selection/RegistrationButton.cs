using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RegistrationButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CourseScriptableObject course;
    public GameObject courseDescriptionTextBox;
    
    private GameObject tempTextBox;
    private TextMeshProUGUI text;
    
    public GameObject hoverSpriteObject;

    public void Start()
    {
        text = this.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        Debug.Log($"pointer entered {gameObject.name}");
        tempTextBox = Instantiate(courseDescriptionTextBox, this.transform.parent.parent);
        tempTextBox.transform.SetAsLastSibling(); // render on top of other ui elements !
        // high z index
        tempTextBox.transform.position = data.position + new Vector2(10, 10);
        
        string timesString = "", daysString = "";
        // we formatting
        foreach (var time in course.times)
        {
            if (time == course.times.Last())
            {
                timesString += Calendar.TimeToString(time);
            }
            else
            {
                timesString += Calendar.TimeToString(time) + ", ";
            }
        }

        foreach (var day in course.days)
        {
            if (day == course.days.Last())
            {
                daysString += Calendar.DayToString(day);
            }
            else
            {
                daysString += Calendar.DayToString(day) + ", ";
            }
        }
        
        tempTextBox.GetComponent<Tooltip>().SetTooltipText($"Course: {course.name}\n" +
                                                                                $"Times: {timesString}\n" +
                                                                                $"Days: {daysString}\n");
        // tempTextBox.GetComponentInChildren<TextMeshProUGUI>().text = $"Course: {course.name}\n" +
        //                                                              $"Times: {timesString}\n" +
        //                                                              $"Days: {daysString}\n";
        
        // this is a bitmap
        this.text.fontStyle = FontStyles.Bold;
        hoverSpriteObject.transform.position = transform.position + (new Vector3(text.bounds.min.x + -10, text.bounds.min.y + 0.5f*text.bounds.size.y));
        hoverSpriteObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData data)
    {
        Debug.Log($"pointer exited {gameObject.name}");
        Destroy(tempTextBox);
        
        this.text.fontStyle = FontStyles.Normal;
        hoverSpriteObject.SetActive(false);
    }

}
