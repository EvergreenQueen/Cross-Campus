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

    public StoryContext loadedStoryContext = null;
    
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
        StartCoroutine(LoadSceneAndCallDialogue("OrientationInk", null, orientationJSON));
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
        StartCoroutine(LoadSceneAndCallDialogue("NameScene", null, nameJSON));
    }

    public void loadLifeEvent(TextAsset tx, Mascot m)
    {
        StartCoroutine(LoadSceneAndCallDialogue("life", null, tx));
    }

    public void loadDateEvent(TextAsset tx, Mascot m)
    {
        StartCoroutine(LoadSceneAndCallDialogue("life", null, tx));
    }

    public void loadPhone()
    {
        StartCoroutine(LoadSceneAndCallDialogue("phone"));
    }


    // ADDED ARGUMENT TO THIS FUNCTION TO SPECIFY THE DIALOGUE YOU WANT TO PLAY
    public IEnumerator LoadSceneAndCallDialogue(string whichScene, StoryContext storyContext = null, TextAsset dialogue = null){
        if (storyContext != null) {
            dialogue = storyContext.inkStoryJson;
        }

        sceneToLoad = whichScene;

        switch(whichScene){
            case "OrientationInk":

                yield return StartCoroutine("LoadScene");

                DialogueManager.GetInstance().EnterDialogueMode(dialogue);

                // UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("MatthewScene");
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currScene);
                break;
            
            case "Registration":
                yield return StartCoroutine("LoadScene");

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

                yield return StartCoroutine("LoadScene");


                DialogueManager.GetInstance().EnterDialogueMode(dialogue);
                DialogueManager.GetInstance().storyContext = storyContext;

                // there's *PROBABLY* a better way to do this
                // UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("CampusMap");
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currScene);
               
                break;
            
            case "CampusMap":
                EventSystem e2 = GameObject.Find("EventSystem").GetComponent<EventSystem>();
                e2.enabled = false;
                //GameObject mainCam2 = GameObject.Find("Main Camera");
                //mainCam2.SetActive(false);

                yield return StartCoroutine("LoadScene");
                
                // progress time :> and do state restoration :>
                LocationManager.GetInstance().RetrieveState();
                LocationManager.GetInstance().ProgressTime();

                var ctx = DialogueManager.GetInstance().storyContext;
                if (DialogueManager.GetInstance().successful)
                {
                    if (ctx.mascotNames.Count > 0)
                    {
                        LocationManager.GetInstance().UpdateMascotLevel(ctx.mascotNames[0], ctx.heartExperienceGiven);
                    }
                }
                
                // UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("StoryScene");
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currScene);
                Debug.Log("unloaded old scene");


                break;
            case "NameScene":
                yield return StartCoroutine("LoadScene");

                currScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("NameScene", LoadSceneMode.Additive);

                // unload the map scene
                while (!asyncLoad.isDone)
                {
                    yield return null;
                }
                DialogueManager.GetInstance().EnterDialogueMode(dialogue);
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currScene);
                break;
            case "life":
                currScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                if (dialogue is null)
                {
                    Debug.LogError("trying to switch to a story with a null dialogue!!! wuh oh! abandoning ship!!");
                    break;
                }
                
                asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("LifeAndDateScene", LoadSceneMode.Additive);

                // unload the map scene
                while (!asyncLoad.isDone)
                {
                    yield return null;
                }
                DialogueManager.GetInstance().EnterDialogueMode(dialogue);
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currScene);
                break;
            case "phone":
                currScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("PhoneTestScene", LoadSceneMode.Additive);

                // Wait until the scene is fully loaded
                while (!asyncLoad.isDone)
                {
                    yield return null;
                }

                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currScene);
                break;
            case null:
                break;

        }
    }

    public IEnumerator LoadScene()
    {
        // start fading to black
        fadeToBlackAnimator.SetTrigger("FadeOut");
        animationFinished = false;

        // wait until animation is finished
        while (!animationFinished)
        {
            yield return null;
        }

        currScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
        asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(sceneToLoad, LoadSceneMode.Additive);
        // stay on black screen while loading scene
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
