using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalVars;

public class LocationManager : MonoBehaviour
{
    [SerializeField]
    private Dictionary<Location, List<Mascot>> locations;

    void Start()
    {
        locations = new Dictionary<Location, List<Mascot>>();
        foreach (Location location in System.Enum.GetValues(typeof(Location)))
        {
            locations[location] = new List<Mascot>();
        }
    }

    // Method to get mascots at a specific location
    List<Mascot> GetMascotsAt(Location location)
    {
        if (locations.ContainsKey(location))
        {
            return locations[location];
        }
        else
        {
            Debug.LogError("Location not found: " + location);
            return null;
        }
    }

    void UpdateMascotLocations()
    {
        // Create a temporary dictionary to hold the updated locations
        Dictionary<Location, List<Mascot>> updatedLocations = new Dictionary<Location, List<Mascot>>();

        foreach (var entry in locations)
        {
            Location currentLocation = entry.Key;
            List<Mascot> mascots = entry.Value;

            for (int i = 0; i < mascots.Count; i++)
            {
                Location newLocation = mascots[i].GetLocation();
                if (!updatedLocations.ContainsKey(newLocation))
                {
                    updatedLocations[newLocation] = new List<Mascot>();
                }
                updatedLocations[newLocation].Add(mascots[i]);
            }
        }

        // Apply the changes to the original locations dictionary
        locations.Clear();
        foreach (var entry in updatedLocations)
        {
            locations[entry.Key] = entry.Value;
        }
    }

}
