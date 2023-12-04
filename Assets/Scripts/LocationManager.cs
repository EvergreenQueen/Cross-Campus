using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GlobalVars;
using UnityEditor.Build.Content;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
    // list of mascots at locations, location indices are given in the Location enum (global variable)
    private List<Mascot>[] locationList;
    // private GameManager gameManager; // FIXME UNCOMMENT WHEN GAMEMANAGER IS IMPLEMENTED


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // gamemanager iterates through mascot list
    // so what does this do.
    // "Updates the locationList by iterating through each Mascot and correctly placing them in the array of Lists."
    // pass list of mascots to this function when calling it from GameManager
    public void UpdateLocationList(List<Mascot> mascotList)
    {
        // iterate through each mascot (in list of mascots)
        for (int i = 0; i < mascotList.Count; i++)
        {
            var location = mascotList[i].GetLocation();
            // place them in array of lists
            locationList[(int)(location)].Append(mascotList[i]);
        }
    }

    public List<Mascot> GetAtLocation(Location location) 
    {
        return locationList[(int)location];
    }
}
