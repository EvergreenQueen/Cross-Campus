using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CourseDescriptionTextBox : MonoBehaviour
{
    [SerializeField]
    private Camera uiCam;
    public string descriptionText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCam, out localPoint);
        transform.localPosition = localPoint;
    }
}
