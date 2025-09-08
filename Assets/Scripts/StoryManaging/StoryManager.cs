// -- [ STORY MANAGER ] -- 

// USAGE: StoryManager.Instance.GetContexts(time, day, location, mascotList, heartLevel) returns *A LIST OF* stories for the given parameters. 

// script for "Managing" the "Stories"
// interfaces with StoryContexts to decide the context of stories, like when/where they should display, 
// who they should display, what heart level is required, etc.
// searches for StoryContext scriptable objects in the folder Assets/Resources/StoryContexts,
// they can be in subfolders in this folder, but they must at least be under the StoryContexts folder.

// takes location, time/date, mascot & current heart level, and retrieves the correct story to display (by looking
// through the existing StoryContext).
// supports multiple mascots for a given story, just in case we want stories to feature multiple mascots primarily.
// however, you can't specify a list of mascots when searching for the context, just know that contexts support multiple mascots. 

/* UPDATE: 08/26/2024
 * added an extra parameter for stories: whether this is The first time interaction story or not
 * could MAYBE instead just look at the required heart level and see that it's level 0 and just default to that as the first time story,
    but doing it like this is more flexible for a VERY Small amount more effort
 * 
*/


// loosely follows singleton pattern (lol)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GlobalVars;

public class StoryManager : MonoBehaviour
{
    private static StoryManager _instance;
    private StoryContext[] storyContextList;
    
    // property w only getter so that only this class can set the instance, and others can only get it
    public static StoryManager Instance
    {
        get
        {
            if (_instance is null) Debug.LogError("Story Manager is null!!!!!! that's no good!");
            return _instance;
        }
    }
    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
        }
        
        storyContextList = Resources.LoadAll<StoryContext>("StoryContexts");
    }
    
    /// <summary>
    /// get a story given a bunch of paramenters that determine the context
    /// </summary>
    /// <param name="time"></param>
    /// <param name="day"></param>
    /// <param name="location"></param>
    /// <param name="mascotName">must be a string value</param>
    /// <param name="heartLevel"></param>
    /// <param name="isFirstTime"></param>
    /// <returns>list of JSON files of the possible Ink stories to load, list is empty if no story exists for the context</returns>
    public List<StoryContext> GetContexts(Location location, string mascotName,
        int heartLevel, bool isFirstTime = false)
    {
        List<StoryContext> contextList = new List<StoryContext>();
        // highly efficient searching for the context given parameters 😁👍
        foreach (var storyContext in storyContextList)
        {
            if (storyContext.location != location)
            {
                Debug.Log($"location {location.ToString()} does not match for storyContext object {storyContext}");
                continue;
            }
            if (!storyContext.mascots.Contains(mascotName.ToLower()))
            {
                Debug.Log($"mascotName {mascotName} does not match for storyContext object {storyContext} (make sure the name(s) on the scriptable object are lowercase)");
                continue;
            }
            if (storyContext.requiredHeartLevel > heartLevel)
            {
                 Debug.Log($"heart level {heartLevel} is too low for storyContext object {storyContext}");
                continue;
            }
            if (storyContext.firstTimeInteraction != isFirstTime)
            {
                continue;
            }
            
            Debug.Log($"added story {storyContext} to call with parameters: {location.ToString()}, {mascotName}, {heartLevel}");
            contextList.Add(storyContext);
        }
        
        return contextList;
    }
}
