// for displaying tooltips on location button things

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class LocationTemplate : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public string locationName;
    public List<GameObject> mascotList;
    public GameObject tooltipPrefab;
    private GameObject tempTooltip;
    private GameObject hoverSpriteObject;
    private Camera uiCam;
    public Collider boundingPolygon;
    public Image sprite;
    
    private bool tooltipActive = false;
    
    public void OnPointerEnter(PointerEventData data)
    {
        uiCam = data.enterEventCamera;
        tooltipActive = true;
        Debug.Log($"pointer entered {gameObject.name}");
        tempTooltip = Instantiate(tooltipPrefab, this.transform.parent);
        tempTooltip.GetComponent<Tooltip>().doAutoUpdate = false;
        
        tempTooltip.transform.SetAsLastSibling(); // render on top of other ui elements !
        // high z index
        tempTooltip.transform.position = data.position + new Vector2(10, 10);
        
        mascotList = GameObject.Find("LocationManager")?.GetComponent<LocationManager>().GetMascotsAtLocation(locationName);
        
        string mascotListString = "";
        Debug.Log(mascotList.Count);
        for (int i = 0; i < mascotList.Count; i++)
        {
            mascotListString += (mascotList[i].GetComponent<Mascot>().mascotName);
            if (i != mascotList.Count - 1)
            {
                mascotListString += ", "; 
            }
        }
        
        tempTooltip.GetComponent<Tooltip>().SetTooltipText($"Location: {StringUtility.FirstCharacterToUpper(locationName)}\n" +
                                                           $"Mascot(s): {mascotListString}\n");
        
        // little scobby for highlighting the selection, not necessary RN RN RN
        // hoverSpriteObject.transform.position = transform.position + (new Vector3(-10, 0));
        // hoverSpriteObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData data)
    {
        tooltipActive = false;
        Debug.Log($"pointer exited {gameObject.name}");
        Destroy(tempTooltip);
        
        // hoverSpriteObject.SetActive(false);
    }

    public void OnMouseEnter()
    {
        GetComponent<Image>().enabled = true;
    }

    public void OnMouseExit()
    {
        GetComponent<Image>().enabled = false;
    }

    void Update()
    {
        if (tooltipActive)
        {
            // Vector2 localPoint;
            // RectTransformUtility.ScreenPointToLocalPointInRectangle(transform.GetComponent<RectTransform>(),
            //     Input.mousePosition, uiCam, out localPoint);
            // Debug.Log(localPoint);
            // tempTooltip.transform.localPosition = localPoint;
            tempTooltip.transform.position = new Vector3(uiCam.ScreenToWorldPoint(Input.mousePosition).x, uiCam.ScreenToWorldPoint(Input.mousePosition).y) + new Vector3(0.05f, -0.05f);
        }
    }
}
