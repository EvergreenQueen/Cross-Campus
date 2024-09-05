using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private TextAsset orientationJSON;
    [SerializeField] private TextAsset nameJSON;
    [SerializeField] private string currScene;
    [SerializeField] private AsyncOperation asyncLoad;
    private static SceneChanger instance;

    private string sceneToLoad;
    
    public GameObject savedTimeAndDay; // used to preserve global time and day between scenes, see the SavedTimeAndDay script

    public Animator fadeToBlackAnimator;

    public bool animationFinished;
    
    private void Awake(){
        if (instance != null && instance != this) 
        {
            Debug.LogWarning("Found more than 1 Scene Changer. That's not bueno.");
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public static SceneChanger GetInstance(){
        return instance;
    }

    public void loadOrientation(){
        // UnityEngine.SceneManagement.SceneManager.LoadScene("OrientationInk");
        // DialogueManager.GetInstance().EnterDialogueMode(orientationJSON);
        StartCoroutine(LoadSceneAndCallDialogue("Orientation", orientationJSON));
    }

    public void loadRegistration(){
        StartCoroutine(LoadSceneAndCallDialogue("Registration"));
    }

    public void loadCampusMap()
    {
        StartCoroutine(LoadSceneAndCallDialogue("CampusMap"));
    }

    public void loadName()
    {
        StartCoroutine(LoadSceneAndCallDialogue("NameSelect", nameJSON));
    }

    // ADDED ARGUMENT TO THIS FUNCTION TO SPECIFY THE DIALOGUE YOU WANT TO PLAY
    public IEnumerator LoadSceneAndCallDialogue(string whichScene, TextAsset dialogue = null){


        switch(whichScene){
            case "Orientation":
                yield return LoadScene("Orientation");

                DialogueManager.GetInstance().EnterDialogueMode(dialogue);

                // UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("MatthewScene");
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currScene);
                break;
            
            case "Registration":
                yield return LoadScene("ClassSelection");

                // UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("OrientationInk");
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currScene);
                break;
            
            // i don't want to break ANYTHING that hao did so i am simply adding to the switch statement and not messing with the above ^^^ - grat
            // it's ok I WILL BREAK MY OWN STUFF - hao
            case "StoryScene":

                Debug.Log("time for story scene");

                EventSystem e1 = GameObject.Find("EventSystem").GetComponent<EventSystem>();
                e1.enabled = false;
                //GameObject mainCam1 = GameObject.Find("Main Camera");
                //mainCam1.SetActive(false);

                yield return LoadScene("StoryScene");


                DialogueManager.GetInstance().EnterDialogueMode(dialogue);
                // there's a better way to do this
                // UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("CampusMap");
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currScene);
               
                break;
            
            case "CampusMap":
                EventSystem e2 = GameObject.Find("EventSystem").GetComponent<EventSystem>();
                e2.enabled = false;
                //GameObject mainCam2 = GameObject.Find("Main Camera");
                //mainCam2.SetActive(false);

                yield return LoadScene("CampusMap");
                
                // progress time :>
                LocationManager.GetInstance().RetrieveState();
                LocationManager.GetInstance().ProgressTime();
                
                // UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("StoryScene");
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currScene);


                break;
            case "NameSelect":
                yield return LoadScene("NameSelect");

                DialogueManager.GetInstance().EnterDialogueMode(dialogue);

                // UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("MatthewScene");
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currScene);
                break;

            case null:
                break;

        }
    }

    public IEnumerator LoadScene(string sceneName)
    {
        fadeToBlackAnimator.SetTrigger("FadeOut");
        animationFinished = false;

        while (!animationFinished)
        {
            yield return null;
        }

        currScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        fadeToBlackAnimator.SetTrigger("FadeIn");
    }

    public void OnAnimationComplete()
    {
        animationFinished = true;
    }

    public void OnAnimationStart()
    {
        animationFinished = false;
    }
}
