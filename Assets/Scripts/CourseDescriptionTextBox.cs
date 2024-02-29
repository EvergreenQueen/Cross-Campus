using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CourseDescriptionTextBox : MonoBehaviour
{
    [SerializeField]
    private Camera uiCam;
    public string descriptionText;
    public Vector2 cursorOffset;

    void Start()
    {
        Debug.Log($"description created with position {transform.position}");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 localPoint;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.parent.GetComponent<RectTransform>(), Input.mousePosition, uiCam, out localPoint);
        transform.localPosition = localPoint + cursorOffset;
    }

    public void SetDescriptionText(string text)
    {
        GetComponent<TextMeshProUGUI>().text = text;
    }
}
