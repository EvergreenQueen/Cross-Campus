using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class ButtonOnOver : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TMP_Text text;
    public GameObject selectedIcon;
    public Sprite hoveredSprite;
    public Sprite unHoveredSprite;
    public float iconOffsetX;
    public void OnPointerEnter(PointerEventData pointerEventData)
    {
        if (text)
        {
            // text.fontStyle |= FontStyles.Bold;
            text.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, 0.2f);
            text.color = new Color32(0xEF, 0xEA, 0x99, 0xFF);

            selectedIcon.transform.position = new Vector3(text.transform.position.x + iconOffsetX, text.transform.position.y);
            selectedIcon.SetActive(true);
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = hoveredSprite;
            
        }
    }

    public void OnPointerExit(PointerEventData pointerEventData)
    {
        if (text)
        {
            // text.fontStyle ^= FontStyles.Bold;
            text.fontMaterial.SetFloat(ShaderUtilities.ID_FaceDilate, 0.0f);
            text.color = new Color32(0xEF, 0x70, 0xC4, 0xFF);
            selectedIcon.SetActive(false);
        }
        else
        {
            gameObject.GetComponent<Image>().sprite = unHoveredSprite;

        }

    }
}