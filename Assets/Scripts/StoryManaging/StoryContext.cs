// -- [ STORY CONTEXT ] -- 

// scriptable object for keeping track of the "context" for character interactions: 
// including the location, mascot(s) involved, times, and days, as well as the required heart level. 

// one of these will be created for every interaction that can be triggered when the player has free time. 

// created eXcLuSiVeLy for the StoryManager

using System.Collections;
using System.Collections.Generic;
using GlobalVars;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/StoryContext", order = 1)]
public class StoryContext : ScriptableObject
{
    public Location location = Location.none;
    
    // list in case there are multiple mascots in the story and we wish to communicate that info to the player
    // ALSO MAKE SURE THE NAMES ARE LOWERCASE!
    public List<string> mascotNames; 
    public List<TimeSlot> times;
    public List<Day> days;
    
    // minimum heart level to access the interaction
    public int heartLevel; 
    
    public TextAsset inkStoryJson; // json file of the ink story for the interaction
}
