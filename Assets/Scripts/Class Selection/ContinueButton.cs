using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ContinueButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private TextMeshProUGUI text;
    
    public GameObject hoverSpriteObject;

    public void Start()
    {
        text = this.GetComponentInChildren<TextMeshProUGUI>();
    }

    public void OnPointerEnter(PointerEventData data)
    {
        if (!GetComponent<Button>().IsInteractable()) return;
        // this is a bitmap
        this.text.fontStyle = FontStyles.Bold;
        hoverSpriteObject.transform.position = transform.position + (new Vector3(text.bounds.min.x + -15, text.bounds.min.y + 0.5f*text.bounds.size.y));
        hoverSpriteObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData data)
    {
        if (!GetComponent<Button>().IsInteractable()) return;
        this.text.fontStyle = FontStyles.Normal;
        hoverSpriteObject.SetActive(false);
    }

}
