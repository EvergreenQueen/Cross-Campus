using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    [Header("Ink Jason For Start of Game")]
    [SerializeField] private TextAsset inkjson;
    // Start is called before the first frame update
    void Start()
    {
        DialogueManager.GetInstance().EnterDialogueMode(inkjson);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
