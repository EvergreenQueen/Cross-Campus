using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    [Header("Ink Jason for Test Purposes")]
    [SerializeField] private TextAsset inkRoom;
    private bool playerInRange;
    private string bumpedIntoWhat = "";

    private void Awake(){
        playerInRange = false;
    }

    private void Update(){
        if(playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying){
            if(Input.GetKey(KeyCode.C)){
                switch(bumpedIntoWhat){
                    case "Door":
                        DialogueManager.GetInstance().EnterDialogueMode(inkRoom);
                        break;
                    case null:
                        break;
                }
            }
        }else{
        }
    }

    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.gameObject.tag == "Interactable"){
            playerInRange = true;
            bumpedIntoWhat = collider.gameObject.transform.parent.name;
        }
    }

    private void OnTriggerExit2D(Collider2D collider){
        if(collider.gameObject.tag == "Interactable"){
            playerInRange = false;
        }
    }
}
