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
    [SerializeField] private TextAsset[] lifeEvents;
    [SerializeField] private TextAsset[] dateEvents;
    public Day whatDay;
    private bool dateDay = false;
    // Start is called before the first frame update
    void Start()
    {
        lifeEvents = Resources.LoadAll<TextAsset>("CharacterInkFiles/Life_Events");
        dateEvents = Resources.LoadAll<TextAsset>("CharacterInkFiles/Dates");
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
                btn.GetComponent<Button>().onClick.AddListener(delegate { findFren(foundMascot[i2], foundMascot[i2].mascotName); });
            }
        }
    }

    void findFren(Mascot m, string s){
        confirmationWindow.SetActive(true);
        DialogueManager.GetInstance().EnterDialogueMode(confJSON, s);
        StartCoroutine(ConfirmDeny(m, s));
    }

    IEnumerator ConfirmDeny(Mascot m, string s){
        while(DialogueManager.GetInstance().dialogueIsPlaying){
            yield return null;
        }
        if(DialogueManager.GetInstance().yesno){
            // this is true; player selected yes to call
            if(dateDay){
                // initiate date
                List<TextAsset> charDateEvent = new List<TextAsset>();
                foreach(TextAsset tx in dateEvents){
                    if(tx.name.Contains(s)){
                        // Debug.Log("hit one");
                        charDateEvent.Add(tx);
                    }
                }
                TextAsset datE = charDateEvent[m.GetHeartLevel()];
                SceneChanger.GetInstance().loadDateEvent(datE, m);
            }else{
                // initiate hangout / life event
                List<TextAsset> charLifeEvent = new List<TextAsset>();
                foreach(TextAsset tx in lifeEvents){
                    if(tx.name.Contains(s)){
                        // Debug.Log("hit one");
                        charLifeEvent.Add(tx);
                    }
                }
                TextAsset rand = charLifeEvent[Random.Range(0, charLifeEvent.Count)];
                SceneChanger.GetInstance().loadLifeEvent(rand, m);
            }
        }else{
            // do nothing
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
