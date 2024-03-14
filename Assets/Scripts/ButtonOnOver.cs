using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ButtonOnOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text text;
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        text.fontStyle |= FontStyles.Bold;
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        text.fontStyle ^= FontStyles.Bold;
    }
}