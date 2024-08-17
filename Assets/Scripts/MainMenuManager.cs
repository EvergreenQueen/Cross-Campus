using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
    
    public Button start, quit, load, collections, settings;
    UIController control;
    
    void Start() {
        start.onClick.AddListener(OnClickStart);
        if (GameManagerSTUB.GetInstance().SaveDataExists())
        {
            load.onClick.AddListener(OnClickLoad);
        }
        else
        {
            load.interactable = false;
        }
        quit.onClick.AddListener(OnClickQuit);
        
        Debug.Log(gameObject);
    }

    void OnClickStart() {
        control = FindObjectOfType<UIController>();
        control.Change();
        // SceneChanger.GetInstance().loadOrientation();
        SceneChanger.GetInstance().loadName();
    }

    void OnClickLoad()
    {
        GameManagerSTUB.GetInstance().LoadGame();
    }

    void OnClickQuit() {
        Debug.Log("Leaving the game");
        Application.Quit();
    }

    


}