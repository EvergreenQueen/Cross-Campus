using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [Header("Player")]
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Image displayImage;
    private Story currentStory;
    public bool dialogueIsPlaying { get; private set; }
    private static DialogueManager instance;
    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string EXTRA_TAG = "extra";

    private void Awake(){
        if(instance != null){
            Debug.LogWarning("Found more than 1 Dialogue Manager. That's not bueno.");
        }
        instance = this;
    }

    public static DialogueManager GetInstance(){
        return instance;
    }

    private void Start(){
        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);
    }

    private void Update(){
        //return right away if dialogue isn't playing
        if(!dialogueIsPlaying){
            DialogueTrigger playerDialogue = player.GetComponent<DialogueTrigger>();
            playerDialogue.enabled = true;
            return;
        }else{
            DialogueTrigger playerDialogue = player.GetComponent<DialogueTrigger>();
            playerDialogue.enabled = false;
        }

        //handle continuing to next line in dialogue when submit pressed
        if(Input.GetKeyDown(KeyCode.Space)){
            ContinueStory();
        }
    }

    public void EnterDialogueMode(TextAsset inkjson){
        currentStory = new Story(inkjson.text);
        dialogueIsPlaying = true;
        dialoguePanel.SetActive(true);

        ContinueStory();
    }

    private void ExitDialogueMode(){
        StartCoroutine(waiter());
    }

    private void ContinueStory(){
        if(currentStory.canContinue){
            dialogueText.text = currentStory.Continue();
            //handling tags
            HandleTags(currentStory.currentTags);
        }else{
            Debug.Log("hallo?");
            ExitDialogueMode();
            displayImage.gameObject.SetActive(false);
        }
    }

    private void HandleTags(List<string> currentTags){
        foreach(string tag in currentTags){
            //parse tag
            string[] splitTag = tag.Split(":");
            if(splitTag.Length != 2){
                Debug.LogError("No lmao");
            }
            string tagKey = splitTag[0].Trim();
            string tagValue = splitTag[1].Trim();

            switch(tagKey){
                case SPEAKER_TAG:
                    displayNameText.text = tagValue;
                    break;
                case PORTRAIT_TAG:
                    Color tmp = displayImage.GetComponent<Image>().color;
                    if(tagValue == "none"){
                        tmp.a = 0f;
                        displayImage.GetComponent<Image>().color = tmp;
                    }else{
                        tmp.a = 255f;
                        displayImage.GetComponent<Image>().color = tmp;
                        displayImage.gameObject.SetActive(true);
                        displayImage.sprite = Resources.Load<Sprite>(tagValue);
                        Debug.Log(tagValue);
                    }
                    break;
                case EXTRA_TAG:
                    if(tagValue != ""){
                        StartCoroutine(switchScene(tagValue));
                    }
                    break;
                default:
                    Debug.LogError("This aint s'possed ta happen");
                    break;
            }
        }
    }

    IEnumerator waiter(){
        yield return new WaitForSeconds(0.2f);
        dialoguePanel.SetActive(false);
        dialogueText.text = "";
        dialogueIsPlaying = false;
    }

    IEnumerator switchScene(string whatScene){
        while(dialogueIsPlaying){
            yield return null;
        }
        switch(whatScene){
            case "scottysbedroomlmao":
                break;
            case null:
                break;
        }
    }
}
