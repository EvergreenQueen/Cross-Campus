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
    
    public GameObject savedTimeAndDay; // used to preserve global time and day between scenes, see the SavedTimeAndDay script
    
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
        StartCoroutine(LoadSceneAndCallDialogue("orientation", orientationJSON));
    }

    public void loadRegistration(){
        StartCoroutine(LoadSceneAndCallDialogue("registration"));
    }

    public void loadCampusMap()
    {
        StartCoroutine(LoadSceneAndCallDialogue("campus_map"));
    }

    public void loadName()
    {
        StartCoroutine(LoadSceneAndCallDialogue("name_select", nameJSON));
    }

    public void loadLifeEvent(TextAsset tx, Mascot m)
    {
        StartCoroutine(LoadSceneAndCallDialogue("life", tx, m));
    }

    public void loadDateEvent(TextAsset tx, Mascot m)
    {
        StartCoroutine(LoadSceneAndCallDialogue("date", tx, m));
    }

    // ADDED ARGUMENT TO THIS FUNCTION TO SPECIFY THE DIALOGUE YOU WANT TO PLAY
    public IEnumerator LoadSceneAndCallDialogue(string whichScene, TextAsset dialogue = null, Mascot m = null){
        switch(whichScene){
            case "orientation":
                currScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("OrientationInk", LoadSceneMode.Additive);

                // Wait until the scene is fully loaded
                while (!asyncLoad.isDone)
                {
                    yield return null;
                }
                DialogueManager.GetInstance().EnterDialogueMode(dialogue);

                // UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("MatthewScene");
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currScene);
                break;
            
            case "registration":
                currScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("ClassSelection", LoadSceneMode.Additive);

                // Wait until the scene is fully loaded
                while (!asyncLoad.isDone)
                {
                    yield return null;
                }

                // UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("OrientationInk");
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currScene);
                break;
            
            // i don't want to break ANYTHING that hao did so i am simply adding to the switch statement and not messing with the above ^^^ - grat
            // it's ok I WILL BREAK MY OWN STUFF - hao
            case "story":
                currScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                if (dialogue is null)
                {
                    Debug.LogError("trying to switch to a story with a null dialogue!!! wuh oh! abandoning ship!!");
                    break;
                }
                
                EventSystem e = GameObject.Find("EventSystem").GetComponent<EventSystem>();
                e.enabled = false;
                GameObject mainCam = GameObject.Find("Main Camera");
                mainCam.SetActive(false);
                
                asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("StoryScene", LoadSceneMode.Additive);

                // unload the map scene
                while (!asyncLoad.isDone)
                {
                    yield return null;
                }

                DialogueManager.GetInstance().EnterDialogueMode(dialogue);
                // there's a better way to do this
                // UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("CampusMap");
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currScene);
               
                break;
            
            case "campus_map":
                currScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                EventSystem e2 = GameObject.Find("EventSystem").GetComponent<EventSystem>();
                e2.enabled = false;
                GameObject mainCam2 = GameObject.Find("Main Camera");
                mainCam2.SetActive(false);
                
                asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("CampusMap", LoadSceneMode.Additive);
                while (!asyncLoad.isDone)
                {
                    yield return null;
                }
                
                // progress time :>
                LocationManager.GetInstance().RetrieveTimeAndDayState();
                LocationManager.GetInstance().ProgressTime();
                
                // UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("StoryScene");
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currScene);
                break;
            case "name_select":
                currScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name;
                asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("NameScene", LoadSceneMode.Additive);

                // Wait until the scene is fully loaded
                while (!asyncLoad.isDone)
                {
                    yield return null;
                }
                DialogueManager.GetInstance().EnterDialogueMode(dialogue);

                // UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("MatthewScene");
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync(currScene);
                break;
            case "life":
                DialogueManager.GetInstance().EnterDialogueMode(dialogue, "", m);
                break;
            case "date":
                DialogueManager.GetInstance().EnterDialogueMode(dialogue, "", m);
                break;
            case null:
                break;
        }
        
    }
}
