using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class PhoneChanger : MonoBehaviour
{
    private bool isChanging = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Story dm = DialogueManager.GetInstance().currentStory;
        if (dm && !dm.canContinue && !isChanging){
            isChanging = true;
            StartCoroutine(changeScene());
        }
    }

    IEnumerator changeScene(){
        yield return new WaitForSeconds(1.0f);
        SceneChanger.GetInstance().loadPhone();
    }
}
