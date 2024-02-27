using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class RegistrationButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public CourseScriptableObject course;
    public GameObject courseDescriptionTextBox;
    
    private GameObject tempTextBox;

    public void OnPointerEnter(PointerEventData data)
    {
        Debug.Log($"pointer entered {gameObject.name}");
        tempTextBox = Instantiate(courseDescriptionTextBox, this.transform);
        tempTextBox.transform.position = data.position;
    }

    public void OnPointerExit(PointerEventData data)
    {
        Debug.Log($"pointer exited {gameObject.name}");
        Destroy(tempTextBox);
    }

}
