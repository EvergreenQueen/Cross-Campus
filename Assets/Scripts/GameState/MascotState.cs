using System.Collections;
using System.Collections.Generic;
using GlobalVars;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/MascotState", order = 1)]
public class MascotState : ScriptableObject
{
    public class MascotData 
    {
        public int heartLevel;
        public int barValue;
        public bool interactedWith;
    }

    
    public Dictionary<string, MascotData> mascotDataDict = new Dictionary<string, MascotData>();
    

    // use to create an entry in the dictionary for mascot with name mascotName
    public MascotData CreateData(string mascotName, int heartLevel, int barValue, bool interactedWith)
    {
        var data = new MascotData();
        data.heartLevel = heartLevel;   
        data.barValue = barValue;
        data.interactedWith = interactedWith;

        // entries are not case sensitive
        mascotDataDict.Add(mascotName.ToLower(), data);

        return data;
    }

    public void UpdateData(string mascotName, int heartLevel, int barValue, bool interactedWith)
    {
        var lowercasedName = mascotName.ToLower();
        if (mascotDataDict.ContainsKey(lowercasedName))
        {

            var data = mascotDataDict[lowercasedName];

            if (data != null)
            {
                data.heartLevel = heartLevel;
                data.barValue = barValue;
                data.interactedWith = interactedWith;

                // dont know if its a copy so set in the dictionary just in case
                mascotDataDict[lowercasedName] = data;
            }
        }
        else
        {
            Debug.LogError("trying to update a non-existent mascotData entry: " + mascotName + ", creating an entry for it!");
            CreateData(mascotName, heartLevel, barValue, interactedWith);
        }
    }

    public MascotData GetData(string mascotName)
    {
        var data = mascotDataDict[mascotName.ToLower()];
        if (data != null)
        {
            return data;
        }
        else
        {
            Debug.LogError("trying to access a non-existent mascotData entry with name: " + mascotName);
            return null;
        }
    }
}
