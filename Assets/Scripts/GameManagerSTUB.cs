using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.SceneManagement;

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
        
        player = GameObject.Find("Player")?.GetComponent<Player>();
    }

    public static GameManagerSTUB GetInstance()
    {
        return instance;
    }

    
    // TODO access save and load buttons from a menu or something on the campus map scene
    void Update()
    {
        // if (Input.GetKeyDown(KeyCode.S))
        // {
        //     SaveGame();
        // }
        // if (Input.GetKeyDown(KeyCode.L))
        // {
        //     StartCoroutine(LoadGame());
        // }
    }

    public void SaveGame()
    {
        LocationManager.GetInstance().SaveLocationData();
        player.SavePlayer();
    }

    // made this a couroutine because it needs to be on the campus map scene to load things properly
    // (and loading scenes using SceneManager.LoadScene doesn't load the scene on the same frame so i needed to use a couroutine)
    // you could probably separate the functionality into more parts (if you wanted to load the game to end up on a different scene or something),
    // but for our purposes i dont think we'll ever need to load the game in a different context
    // campus map is kinda the main scene of the game 
    public IEnumerator LoadGame()
    {
        // EventSystem e2 = GameObject.Find("EventSystem")?.GetComponent<EventSystem>();
        // if (e2)
        // {
        //     e2.enabled = false;
        // }
        
        // GameObject mainCam2 = GameObject.Find("Main Camera");
        // if (mainCam2)
        // {
        //     mainCam2.SetActive(false);
        // }

        if (UnityEngine.SceneManagement.SceneManager.GetActiveScene() !=
            UnityEngine.SceneManagement.SceneManager.GetSceneByName("CampusMap")) // don't load campusmap scene if we're already on it
        {
            AsyncOperation asyncLoad = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync("CampusMap", LoadSceneMode.Additive);
            while (!asyncLoad.isDone)
            {
                yield return null;
            }
            Debug.Log("loaded campusmap scene");
            
            UnityEngine.SceneManagement.SceneManager.UnloadSceneAsync("MainMenu");
        }

        // if (!LocationManager.GetInstance())
        // {
        //     GameObject locationManagerObject = new GameObject();
        //     locationManagerObject.name = "LocationManager";
        //     locationManagerObject.AddComponent<LocationManager>();
        //     Debug.Log("made a new location manager for some reason");
        // }
        LocationManager.GetInstance().LoadLocationData();
        
        // very silly player gameobject things. will mostly likely be solved once the player is a static class
        if (!player)
        {
            if (GameObject.Find("Player"))
            {
                player = GameObject.Find("Player").GetComponent<Player>();
            }
            else
            {
                GameObject playerObject = new GameObject();
                playerObject.name = "Player"; // ermmmmm
                player = playerObject.AddComponent<Player>();
                DontDestroyOnLoad(playerObject);
            }
        }
        player.LoadPlayer();

        // if (e2)
        // {
        //     Destroy(e2); // the campus map scene already has an event system object, so delete the one that was in the previous scene (probably main menu) if there was one
        // }
    }

    public bool SaveDataExists()
    {
        if (LocationDataExists() && PlayerDataExists())
        {
            return true;
        }
        return false;
    }
    
    public bool LocationDataExists()
    {
        string dirPath = Path.Combine(Application.persistentDataPath, "Map");
        if (Directory.Exists(dirPath))
        {
            if (File.Exists(Path.Combine(dirPath, "TimeDayData.txt")))
            {
                return true;
            }
        }
        return false;
    }
    
    public bool PlayerDataExists()
    {
        string dirPath = Path.Combine(Application.persistentDataPath, "Player");
        string filePath = Path.Combine(dirPath, "CourseGrades.txt");
        
        if (Directory.Exists(dirPath) && File.Exists(filePath))
        {
            return true;
        }
        return false;
    }
}
