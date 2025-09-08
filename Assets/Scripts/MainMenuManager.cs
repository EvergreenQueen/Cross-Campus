using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour {
    
    public Button start, quit, load, collections, settings;
    MainMenuUIController control;
    
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
        control = FindObjectOfType<MainMenuUIController>();
        control.Change();
        SceneChanger.GetInstance().loadOrientation();
        // SceneChanger.GetInstance().loadName();
    }

    void OnClickLoad()
    {
        StartCoroutine(GameManagerSTUB.GetInstance().LoadGame());
    }

    void OnClickQuit() {
        Debug.Log("Leaving the game");
        Application.Quit();
    }

    


}