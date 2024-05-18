using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private TextAsset orientationJSON;
    private static SceneChanger instance;

    private void Awake(){
        if(instance != null){
            Debug.LogWarning("Found more than 1 Scene Changer. That's not bueno.");
        }
        instance = this;
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

    // ADDED ARGUMENT TO THIS FUNCTION TO SPECIFY THE DIALOGUE YOU WANT TO PLAY
    public IEnumerator LoadSceneAndCallDialogue(string whichScene, TextAsset dialogue = null){
        switch(whichScene){
            case "orientation":
                AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("OrientationInk", LoadSceneMode.Additive);

                // Wait until the scene is fully loaded
                while (!asyncLoad.isDone)
                {
                    yield return null;
                }
                DialogueManager.GetInstance().EnterDialogueMode(dialogue);

                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("MatthewScene");
                break;
            case "registration":
                AsyncOperation asyncLoad2 = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("ClassSelection", LoadSceneMode.Additive);

                // Wait until the scene is fully loaded
                while (!asyncLoad2.isDone)
                {
                    yield return null;
                }

                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("OrientationInk");
                break;
            // i don't want to break ANYTHING that hao did so i am simply adding to the switch statement and not messing with the above ^^^ - grat
            case "story":
                if (dialogue is null)
                {
                    Debug.LogError("trying to switch to a story with a null dialogue!!! wuh oh! abandoning ship!!");
                    break;
                }
                
                AsyncOperation asyncLoad3 = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("StoryScene", LoadSceneMode.Additive);
                // unload the map scene
                while (!asyncLoad3.isDone)
                {
                    yield return null;
                }

                DialogueManager.GetInstance().EnterDialogueMode(dialogue);
                // there's a better way to do this
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("CampusMap");
                break;
            case "campus_map":
                AsyncOperation asyncLoad4 = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("CampusMap", LoadSceneMode.Additive);
                while (!asyncLoad4.isDone)
                {
                    yield return null;
                }
                
                UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("StoryScene");
                // progress time :>
                LocationManager.GetInstance().ProgressTime();
                break;
            case null:
                break;
        }
        
    }
}
