using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
    
    public Button start, quit;
    
    void Start() {
        start.onClick.AddListener(onClickStart);
        quit.onClick.AddListener(onClickQuit);
    }

    void onClickStart() {
        Debug.Log("Pressed play!");
    }

    void onClickQuit() {
        Debug.Log("Leaving the game");
    }




}