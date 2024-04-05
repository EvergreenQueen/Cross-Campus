using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    [Header("Ink Jason for Test Purposes")]
    // [SerializeField] private TextAsset testInk;
    // [SerializeField] private TextAsset scottyInk1;
    private string talkingTo = "";
    private bool startedIntro = false;

    private void Awake(){
        // talkingTo = "Introduction";
    }

    void Start(){
    }

    private void Update(){
        // if(!startedIntro){
        //     DialogueManager.GetInstance().EnterDialogueMode(testInk);
        //     startedIntro = true;
        //     talkingTo = "Scotty";
        // }
        // if(!DialogueManager.GetInstance().dialogueIsPlaying){
        //     if(Input.GetKey(KeyCode.Space)){
        //         switch(talkingTo){
        //             case "Scotty":
        //                 DialogueManager.GetInstance().EnterDialogueMode(scottyInk1);
        //                 break;
        //             case null:
        //                 break;
        //         }
        //     }
        // }else{
        // }
    }

    //functions down here handle whatever button is pressed
}
