// -- [ EVENT POP UP ] --

// pops up in the Corner of the screen when the player has a non-required event that they should be prompted to attend/not attend
// has an accept button and a deny button

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class EventPopUp : MonoBehaviour
{
    [SerializeField] GameObject acceptButtonObject;
    [SerializeField] GameObject denyButtonObject;
    
    public void SetText(string t)
    {
        GetComponentInChildren<TextMeshProUGUI>().text = t;
    }
    
    // specify the functions that will be called when the accept/deny buttons are pressed
    public void AssignButtonEvents(UnityAction acceptCallback, UnityAction denyCallback)
    {
        acceptButtonObject.GetComponent<Button>().onClick.AddListener(acceptCallback);
        // acceptButtonObject.GetComponent<Button>().onClick.AddListener(() => Destroy(gameObject)); // die after the button is clicked
        denyButtonObject.GetComponent<Button>().onClick.AddListener(denyCallback);
        // denyButtonObject.GetComponent<Button>().onClick.AddListener(() => Destroy(gameObject)); // die after the button is clicked
    }
}
