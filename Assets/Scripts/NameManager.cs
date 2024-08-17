using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NameManager : MonoBehaviour
{
    public InputField name;
    public TextAsset nameJSONpt2;
    
    void Start() {
        name.SetActive(false);
    }

    void Update() {
        if(!DialogueManager.GetInstance().dialogueCurrentlyPlaying){
            name.SetActive(true);
        }else{
            name.SetActive(false);
        }

        if(Input.GetKeyDown(KeyCode.Enter)){
            Player.name = name.text;
            DialogueManager.GetInstance().EnterDialogueMode(nameJSONpt2);
        }
    }
}
