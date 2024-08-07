using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// stub for game manager that only implements saving and loading, as the game manager should be the one doing that
// put this functionality in the real game manager script when it exists

public class GameManagerSTUB : MonoBehaviour
{
    private static GameManagerSTUB instance;
    private Player player;
    
    void Awake()
    {
        if (instance != null)
        {
            Debug.Log("found more than one game manager, but it's a singleton and there should only be one in the scene.");
        }
        instance = this;
        
        player = GameObject.Find("Player").GetComponent<Player>();
    }

    public static GameManagerSTUB GetInstance()
    {
        return instance;
    }

    
    // TODO access save and load buttons from a menu or something on the campus map scene
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            SaveGame();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadGame();
        }
    }

    public void SaveGame()
    {
        LocationManager.GetInstance().SaveLocationData();
        player.SavePlayer();
    }

    public void LoadGame()
    {
        LocationManager.GetInstance().LoadLocationData();
        player.LoadPlayer();
    }
}
