using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NameManager : MonoBehaviour
{
    public TMP_InputField name;
    public TextAsset nameJSONpt2;
    private string input;
    
    void Start() {
        name.gameObject.SetActive(false);
    }

    void Update() {
        if(!DialogueManager.GetInstance().dialogueIsPlaying){
            name.gameObject.SetActive(true);
        }else{
            name.gameObject.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.Return)){
            Player player = GameObject.Find("Player")?.GetComponent<Player>();
            player.SetName(name.text);
            DialogueManager.GetInstance().EnterDialogueMode(nameJSONpt2);
        }
    }

    public void ReadStrngParam(string s){
        input = s;
    }
}
