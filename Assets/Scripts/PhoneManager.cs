using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using GlobalVars;

public class PhoneManager : MonoBehaviour
{
    public GameObject scrollviewContent;
    public GameObject phone;
    public GameObject mascotFinderTemplate;
    public GameObject confirmationWindow;
    public TextMeshProUGUI frenfndr;
    public TextAsset confJSON;
    public Day whatDay;
    private bool dateDay = false;
    // Start is called before the first frame update
    void Start()
    {
        phone.SetActive(false);
        // confirmationWindow.SetActive(false);
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
        DialogueManager.GetInstance().EnterDialogueModeWithParam(confJSON, s);
        StartCoroutine(ConfirmDeny());
    }

    IEnumerator ConfirmDeny(){
        while(DialogueManager.GetInstance().dialogueIsPlaying){
            yield return null;
        }
        if(DialogueManager.GetInstance().yesno){
            // this is true; player selected yes to call
        }
    }

    // Update is called once per frame
    void Update()
    {
        whatDay = LocationManager.GetInstance().currentDay;
        if(Input.GetKeyDown(KeyCode.P) && (whatDay == Day.Saturday)){
            // life event phone screen
            dateDay = false;
            if(phone.activeSelf){
                phone.SetActive(false);
            }else{
                phone.SetActive(true);
                frenfndr.text = "Fren Fndr: Life";
            }
        }else if(Input.GetKeyDown(KeyCode.P) && (whatDay == Day.Sunday)){
            // date event phone screen
            dateDay = true;
            if(phone.activeSelf){
                phone.SetActive(false);
            }else{
                phone.SetActive(true);
                frenfndr.text = "Fren Fndr: Date";
            }
        }
    }
}
