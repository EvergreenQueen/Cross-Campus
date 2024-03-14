using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    
    public Button start, quit, load, collections, settings;
    UIController control;
    
    void Start() {
        start.onClick.AddListener(onClickStart);
        quit.onClick.AddListener(onClickQuit);
        
    }

    void onClickStart() {
        control = FindObjectOfType<UIController>();
        control.Change();
    }

    void onClickQuit() {
        Debug.Log("Leaving the game");
    }




}