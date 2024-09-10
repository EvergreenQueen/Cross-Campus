using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    [Header("Ink Jason for Test Purposes")]
    [SerializeField] private TextAsset talkingTo;
    private bool startedIntro = false;

    void Awake(){
    
    }

    private void Update(){
        if(talkingTo != null && !startedIntro){
            DialogueManager.GetInstance().EnterDialogueMode(talkingTo);
            startedIntro = true;
        }
    }

    //functions down here handle whatever button is pressed
}
