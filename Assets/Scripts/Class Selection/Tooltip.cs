using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


/// <summary>
/// object that appears when hovering over another gameObject
/// </summary>
public class Tooltip : MonoBehaviour
{
    [SerializeField]
    private Camera uiCam;
    public GameObject obj_textBG;
    public GameObject obj_text;
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
    
    /// <summary>
    /// sets the description text and automatically scales the background of it to match the size of the text
    /// </summary>
    /// <param name="text"></param>
    public void SetDescriptionText(string text)
    {
        var textComponent = obj_text.GetComponent<TextMeshProUGUI>();
        textComponent.text = text;
        textComponent.ForceMeshUpdate();
        obj_textBG.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, textComponent.textBounds.size.x + 2*textComponent.margin.x);
        obj_textBG.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, textComponent.textBounds.size.y + 2*textComponent.margin.y);
    }
}
