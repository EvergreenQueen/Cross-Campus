using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    
    public Button start, quit;
    UIController control;
    
    void Start() {
        start.onClick.AddListener(onClickStart);
        quit.onClick.AddListener(onClickQuit);
    }

    void onClickStart() {
        Debug.Log("Pressed play!");
        control = FindObjectOfType<UIController>();
        control.Change();
        SceneChanger.GetInstance().loadOrientation();
    }

    void onClickQuit() {
        Debug.Log("Leaving the game");
    }




}