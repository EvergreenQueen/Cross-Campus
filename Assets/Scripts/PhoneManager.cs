using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PhoneManager : MonoBehaviour
{
    public GameObject scrollviewContent;
    public GameObject mascotFinderTemplate;
    public GameObject confirmationWindow;
    public TextAsset confJSON;
    // Start is called before the first frame update
    void Start()
    {
        confirmationWindow.SetActive(false);
        Mascot[] foundMascot = FindObjectsOfType<Mascot>();
        for(int i=0; i<foundMascot.Length; ++i){
            Debug.Log(foundMascot[i].mascotName);
        }
        for(int i=0; i<foundMascot.Length; ++i){
            if(foundMascot[i].GetMetYet()){
                GameObject btn = (GameObject)Instantiate(mascotFinderTemplate);
                btn.transform.SetParent(scrollviewContent.transform);
                Transform txt = btn.transform.GetChild(0);
                txt.GetComponent<TMP_Text>().text = foundMascot[i].mascotName;
                int i2 = i;
                btn.GetComponent<Button>().onClick.AddListener(delegate { findFren(foundMascot[i2].mascotName); });
            }
        }
    }

    void findFren(string s){
        confirmationWindow.SetActive(true);
        // mght reformat ths once we fgure out how the dates work
        switch(s){
            case "Scotty":
                DialogueManager.GetInstance().EnterDialogueModeWithParam(confJSON, s);
                Debug.Log("Touched Scotty");
                break;
            case "Triton":
                Debug.Log("Touched Triton");
                break;
            case "Sammy":
                Debug.Log("Touched Sammy");
                break;
            case "Ru":
                Debug.Log("Touched Ru");
                break;
            case "Peter":
                Debug.Log("Touched Peter");
                break;
            case "Oski":
                Debug.Log("Touched Oski");
                break;
            case "Oleander":
                Debug.Log("Touched Oleander");
                break;
            case "Norm":
                Debug.Log("Touched Norm");
                break;
            case "Josie":
                DialogueManager.GetInstance().EnterDialogueModeWithParam(confJSON, s);
                Debug.Log("Touched Josie");
                break;
            case "Joe":
                Debug.Log("Touched Joe");
                break;
            case "Gunrock":
                Debug.Log("Touched Gunrock");
                break;
            case "":
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
