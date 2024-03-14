using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExampleButton : MonoBehaviour
{
    public GameObject examplebuttonobject;
    public Button examplebutton;

    // Start is called before the first frame update
    void Awake()
    {
        examplebutton = examplebuttonobject.GetComponent<Button>();
        examplebutton.onClick.AddListener(triggerSoundEffect);

    }

    private void OnButtonClick()
    {
        Debug.Log("Button Clicked!");
        // Add your custom logic here when the button is clicked
    }

    private void triggerSoundEffect()
    {
        Debug.Log("E");
        AudioManager.instance.PlayOneShot(FMODEvents.instance.buttonSound);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
