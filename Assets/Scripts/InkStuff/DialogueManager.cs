using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Ink.Runtime;
using UnityEngine.EventSystems;

public class DialogueManager : MonoBehaviour
{
    [Header("Dialogue UI")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private TextMeshProUGUI displayNameText;
    [SerializeField] private Image displayImage;
    [SerializeField] private Image[] threePositions;
    [SerializeField] private Image backgroundImage;

    [Header("Player")]
    [SerializeField] private GameObject player;

    [Header("Choices UI")]
    [SerializeField] private GameObject[] choices;
    [Header("Typewriter")]
    [SerializeField] private TypewriterEffect typa;
    private TextMeshProUGUI[] choicesText;
    public Story currentStory;


    public StoryContext storyContext;
    public bool successful = true; // if the story should give reputation for the mascot when it finishes, set to false when Bad Choices are selected

    public bool dialogueIsPlaying { get; private set; }
    public bool dialogueCurrentlyPlaying { get; private set; }
    public bool yesno { get; private set;}
    private bool noContinue = false;
    private static DialogueManager instance;
    private string currDialogue;
    private bool firstLine = false;
    private string whoTalkingTo = "";
    private Mascot dating = null;
    private const string SPEAKER_TAG = "speaker";
    private const string PORTRAIT_TAG = "portrait";
    private const string NO_PORTRAIT_TAG = "noportrait";
    private const string BACKGROUND_TAG = "background";
    private const string CIRCLE_TAG = "circle";
    private const string POSITION_TAG = "position";
    private const string EXTRA_TAG = "extra";
    private const string YES_NO_TAG = "yesno";
    private const string HEART_TAG = "heart";


    private void Awake(){
        if(instance != null){
            Debug.LogWarning("Found more than 1 Dialogue Manager. That's not bueno.");
        }
        instance = this;
        dialogueCurrentlyPlaying = false;
        player = GameObject.Find("Player");
        successful = true;
    }

    public static DialogueManager GetInstance(){
        return instance;
    }

    private void Start(){
        //get all of the choices text
        choicesText = new TextMeshProUGUI[choices.Length];
        int i = 0;
        foreach(GameObject choice in choices){
            choicesText[i] = choice.GetComponentInChildren<TextMeshProUGUI>();
            i++;
        }

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);

        Color tempC = displayImage.GetComponent<Image>().color;
        tempC.a = 0f;
        displayImage.GetComponent<Image>().color = tempC;

        displayImage = threePositions[1];
        tempC = displayImage.GetComponent<Image>().color;
        tempC.a = 0f;
        displayImage.GetComponent<Image>().color = tempC;

        displayImage = threePositions[2];
        tempC = displayImage.GetComponent<Image>().color;
        tempC.a = 0f;
        displayImage.GetComponent<Image>().color = tempC;
    }

    private void Update(){
        //return right away if dialogue isn't playing
        if(!dialogueIsPlaying){
            // COMMENT BY GARETT ON 08/26/24: the dialogue trigger component seems like it's deprecated/doesn't do anything so i made this class find the player object that has the player component on it
            // and thus i commented these out bc they were causing error messages and null reference exceptions and such
            // sorry hao if you had Big Plans for the dialoguetrigger class
            
            // DialogueTrigger playerDialogue = player.GetComponent<DialogueTrigger>();
            // playerDialogue.enabled = true;
            return;
        }else{
            // COMMENT BY GARETT ON 08/26/24: above ^^^^
            
            // DialogueTrigger playerDialogue = player.GetComponent<DialogueTrigger>();
            // playerDialogue.enabled = false;
        }

        //handle continuing to next line in dialogue when submit pressed
        if((Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)) && !noContinue){
            ContinueStory();
        }

        if(dialogueText.text != currDialogue){
            dialogueCurrentlyPlaying = true;
        }else{
            dialogueCurrentlyPlaying = false;
        }
    }

    public void EnterDialogueMode(TextAsset inkjson, string param = "", Mascot m = null){
        dating = m;
        whoTalkingTo = param;
        dialoguePanel.SetActive(true);
        currentStory = new Story(inkjson.text);
        dialogueIsPlaying = true;
        firstLine = true;
        // dialogueCurrentlyPlaying = true;

        ContinueStory();
    }

    // public void EnterDialogueModeWithParam(TextAsset inkjson, string param){
    //     whoTalkingTo = param;
    //     dialoguePanel.SetActive(true);
    //     currentStory = new Story(inkjson.text);
    //     dialogueIsPlaying = true;
    //     firstLine = true;
    //     // dialogueCurrentlyPlaying = true;

    //     ContinueStory();
    // }

    private void ExitDialogueMode(){
        StartCoroutine(waiter());
    }

    private void ContinueStory(){
        Player tempPlayer = (Player)FindObjectOfType(typeof(Player));
        string tempString;
        if(tempPlayer){
            tempString = tempPlayer.GetName();
        }else{
            tempString = "Player";
        }
        if(currentStory.canContinue){
            if(firstLine){
                currDialogue = currentStory.Continue();
                // Debug.Log(currDialogue);
                currDialogue = currDialogue.Replace("<Player>", tempString);
                currDialogue = currDialogue.Replace("<WhoCall>", whoTalkingTo);

                dialogueText.text = currDialogue;
                // typa.Type();
                //display choices
                DisplayChoices();
                //handling tags
                HandleTags(currentStory.currentTags);
                firstLine = false;
            }
            else if(dialogueCurrentlyPlaying){
                Debug.Log("What about this?");
                typa.stopTyping();
                dialogueCurrentlyPlaying = false;
                currDialogue = currDialogue.Replace("<Player>", tempString);
                currDialogue = currDialogue.Replace("<WhoCall>", whoTalkingTo);
                dialogueText.text = currDialogue;
            }else{
                Debug.Log("moshi moshi");
                // dialogueText.text = currentStory.Continue();
                currDialogue = currentStory.Continue();
                currDialogue = currDialogue.Replace("<Player>", tempString);
                currDialogue = currDialogue.Replace("<WhoCall>", whoTalkingTo);
                dialogueText.text = currDialogue;
                typa.Type();
                //display choices
                DisplayChoices();
                //handling tags
                HandleTags(currentStory.currentTags);
            }
        }else{
            if(dialogueCurrentlyPlaying){
                typa.stopTyping();
                dialogueCurrentlyPlaying = false;
                dialogueText.text = currDialogue;
            }else{
                Debug.Log("hallo?");
                ExitDialogueMode();
                displayImage.gameObject.SetActive(false);
                firstLine = false;
            }
        }
    }

    private void DisplayChoices(){
        List<Choice> currentChoices = currentStory.currentChoices;
        if(currentChoices.Count > choices.Length){
            Debug.Log("Too many choices.");
        }
        int i = 0;
        foreach(Choice choice in currentChoices){
            noContinue = true;
            choices[i].gameObject.SetActive(true);
            choicesText[i].text = choice.text;
            i++; 
        }
        for(int j=i; j<choices.Length; ++j){
            choices[j].gameObject.SetActive(false);
        }
        StartCoroutine(SelectFirstChoice());
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
                case POSITION_TAG:
                    if(tagValue == "middle"){
                        displayImage = threePositions[1];
                    }
                    else if(tagValue == "left"){
                        displayImage = threePositions[0];
                    }
                    else{
                        displayImage = threePositions[2];
                    }
                    break;
                case SPEAKER_TAG:
                    if(tagValue == "<Player>"){
                        Player tempPlayer = (Player)FindObjectOfType(typeof(Player));
                        if(tempPlayer){
                            tagValue = tempPlayer.GetName();
                        }else{
                            tagValue = "Player";
                        }
                    }
                    displayNameText.text = tagValue;
                    break;
                case PORTRAIT_TAG:
                    Color tmp = displayImage.GetComponent<Image>().color;
                    if(tagValue == ""){
                        tmp.a = 0f;
                        displayImage.GetComponent<Image>().color = tmp;
                    }else{
                        tmp.a = 255f;
                        displayImage.GetComponent<Image>().color = tmp;
                        displayImage.gameObject.SetActive(true);
                        displayImage.sprite = Resources.Load<Sprite>("Sprites/"+tagValue);
                        Debug.Log(tagValue);
                    }
                    break;
                case NO_PORTRAIT_TAG:
                    if(tagValue == "yes"){
                        displayImage = threePositions[0];
                        Color tempD = displayImage.GetComponent<Image>().color;
                        tempD.a = 0f;
                        displayImage.GetComponent<Image>().color = tempD;

                        displayImage = threePositions[1];
                        tempD = displayImage.GetComponent<Image>().color;
                        tempD.a = 0f;
                        displayImage.GetComponent<Image>().color = tempD;

                        displayImage = threePositions[2];
                        tempD = displayImage.GetComponent<Image>().color;
                        tempD.a = 0f;
                        displayImage.GetComponent<Image>().color = tempD;
                    }
                    break;
                case CIRCLE_TAG:
                    Color tmp3 = displayImage.GetComponent<Image>().color;
                    if(tagValue == ""){
                        tmp3.a = 0f;
                        displayImage.GetComponent<Image>().color = tmp3;
                    }else{
                        tmp3.a = 255f;
                        displayImage.GetComponent<Image>().color = tmp3;
                        displayImage.gameObject.SetActive(true);
                        displayImage.sprite = Resources.Load<Sprite>("Circles/"+tagValue);
                        Debug.Log(tagValue);
                    }
                    break;
                case BACKGROUND_TAG:
                    Color tmp2 = backgroundImage.GetComponent<Image>().color;
                    if(tagValue == ""){
                        tmp2.a = 0f;
                        backgroundImage.GetComponent<Image>().color = tmp2;
                    }else{
                        tmp2.a = 255f;
                        backgroundImage.GetComponent<Image>().color = tmp2;
                        backgroundImage.gameObject.SetActive(true);
                        backgroundImage.sprite = Resources.Load<Sprite>("Backgrounds/"+tagValue);
                        Debug.Log(tagValue);
                    }
                    break;
                case EXTRA_TAG:
                    if(tagValue != ""){
                        StartCoroutine(switchScene(tagValue));
                    }
                    break;
                case YES_NO_TAG:
                    if(tagValue == "yes"){
                        yesno = true;
                    }else{
                        yesno = false;
                    }
                    break;
                case HEART_TAG:
                    if(tagValue == "1"){
                        dating.IncreaseBarValue(1);
                    }else{
                        dating.DecreaseBarValue(1);
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
        //dialoguePanel.SetActive(false);
        dialogueText.text = "";
        dialogueIsPlaying = false;
    }

    IEnumerator switchScene(string whatScene){
        while(dialogueIsPlaying){
            yield return null;
        }
        switch(whatScene){
            case "orientation":
                SceneChanger.GetInstance().loadOrientation();
                break;
            case "class_registration":
                SceneChanger.GetInstance().loadRegistration();
                break;
            case "campus_map":
                SceneChanger.GetInstance().loadCampusMap();
                break;
            case null:
                break;
            default:
                Debug.Log("attempted to switch scenes.. but couldnt,., .woahs...g...");
                break;
        }
    }

    private IEnumerator SelectFirstChoice(){
        // Unity requires EventSystem to be cleared first AND THEN set (waiting for the frame)
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        // EventSystem.current.SetSelectedGameObject(choices[0].gameObject);
    }

    public void MakeChoice(int whichOne){
        currentStory.ChooseChoiceIndex(whichOne);
        noContinue = false;
        ContinueStory();
    }
}
