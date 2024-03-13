using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        StartCoroutine(LoadSceneAndCallDialogue("orientation"));
    }

    public void loadRegistration(){
        StartCoroutine(LoadSceneAndCallDialogue("registration"));
    }

    IEnumerator LoadSceneAndCallDialogue(string whichScene){
        switch(whichScene){
            case "orientation":
                AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("OrientationInk", LoadSceneMode.Additive);

                // Wait until the scene is fully loaded
                while (!asyncLoad.isDone)
                {
                    yield return null;
                }
                DialogueManager.GetInstance().EnterDialogueMode(orientationJSON);

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
            case null:
                break;
        }
        
    }
}