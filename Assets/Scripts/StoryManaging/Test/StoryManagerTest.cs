// -- [ STORY MANAGER TEST ] --
// just a MEASLY test harness for the story manager.
// don't attach this to any object in the scene if you're not testing bc it will print out annoying stuff.

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GlobalVars;
using UnityEngine;
using UnityEngine.Assertions;

public class StoryManagerTest : MonoBehaviour
{
    
    // TODO assertions
    public void Start()
    {
        List<StoryContext> returnedValue;
        
        returnedValue = StoryManager.Instance.GetContexts(TimeSlot.afternoon, Day.Monday, Location.botanical_gardens, "scotty", 0);
        Debug.Log($"test GetContext with afternoon, monday, botanical gardens, scotty, 0\nexpected: a, results: {FormatContextList(returnedValue)}");
        returnedValue = StoryManager.Instance.GetContexts(TimeSlot.afternoon, Day.Wednesday, Location.botanical_gardens, "scotty", 0);
        Debug.Log($"test GetContext with afternoon, wednesday, botanical gardens, scotty, 0\nexpected: a, results: {FormatContextList(returnedValue)}");
        returnedValue = StoryManager.Instance.GetContexts(TimeSlot.afternoon, Day.Friday, Location.botanical_gardens, "scotty", 0);
        Debug.Log($"test GetContext with afternoon, friday, botanical gardens, scotty, 0\nexpected: a c, results: {FormatContextList(returnedValue)}");
        
        returnedValue = StoryManager.Instance.GetContexts(TimeSlot.morning, Day.Friday, Location.botanical_gardens, "scotty", 1);
        Debug.Log($"test GetContext with morning, friday, botanical gardens, scotty, 1\nexpected: b c, results: {FormatContextList(returnedValue)}");
        returnedValue = StoryManager.Instance.GetContexts(TimeSlot.afternoon, Day.Friday, Location.botanical_gardens, "scotty", 1);
        Debug.Log($"test GetContext with afternoon, friday, botanical gardens, scotty, 1\nexpected: a b c, results: {FormatContextList(returnedValue)}");
        returnedValue = StoryManager.Instance.GetContexts(TimeSlot.evening, Day.Friday, Location.botanical_gardens, "scotty", 1);
        Debug.Log($"test GetContext with evening, friday, botanical gardens, scotty, 1\nexpected: b c, results: {FormatContextList(returnedValue)}");
        
        returnedValue = StoryManager.Instance.GetContexts(TimeSlot.morning, Day.Friday, Location.botanical_gardens, "scotty", 0);
        Debug.Log($"test GetContext with morning, friday, botanical gardens, scotty, 0\nexpected: c, results: {FormatContextList(returnedValue)}");
        returnedValue = StoryManager.Instance.GetContexts(TimeSlot.afternoon, Day.Friday, Location.botanical_gardens, "scotty", 0);
        Debug.Log($"test GetContext with afternoon, friday, botanical gardens, scotty, 0\nexpected: a c, results: {FormatContextList(returnedValue)}");
        returnedValue = StoryManager.Instance.GetContexts(TimeSlot.evening, Day.Friday, Location.botanical_gardens, "scotty", 0);
        Debug.Log($"test GetContext with evening, friday, botanical gardens, scotty, 0\nexpected: c, results: {FormatContextList(returnedValue)}");
        
        // returnedValue = StoryManager.Instance.GetContexts(TimeSlot.evening, Day.Friday, Location.botanical_gardens, "oski", 0);
        // Debug.Log($"test GetContext with evening, friday, botanical gardens, oski, 0\nexpected: c d, results: {FormatContextList(returnedValue)}");
        //
        // returnedValue = StoryManager.Instance.GetContexts(TimeSlot.evening, Day.Friday, Location.botanical_gardens, "scotty", 0);
        // Debug.Log($"test GetContext with evening, friday, botanical gardens, scotty, 0\nexpected: e, results: {FormatContextList(returnedValue)}");
        //
        // returnedValue = StoryManager.Instance.GetContexts(TimeSlot.evening, Day.Friday, Location.gym, "scotty", 0);
        // Debug.Log($"test GetContext with evening, friday, gym, scotty, 0\nexpected: f, results: {FormatContextList(returnedValue)}");
        //
        // returnedValue = StoryManager.Instance.GetContexts(TimeSlot.evening, Day.Monday, Location.gym, "scotty", 0);
        // Debug.Log($"test GetContext with evening, monday, gym, scotty, 0\nexpected: g, results: {FormatContextList(returnedValue)}"); 
        //
        // returnedValue = StoryManager.Instance.GetContexts(TimeSlot.morning, Day.Friday, Location.gym, "scotty", 0);
        // Debug.Log($"test GetContext with morning, friday, gym, scotty, 0\nexpected: h, results: {FormatContextList(returnedValue)}");
        
        returnedValue = StoryManager.Instance.GetContexts(TimeSlot.morning, Day.Wednesday, Location.classroom, "scotty", 0);
        Debug.Log($"test GetContext with morning, wednesday, classroom, scotty, 0\nexpected: Woah, results: {FormatContextList(returnedValue)}");
    }

    public string FormatContextList(List<StoryContext> inputList)
    {
        string output = "";
        foreach (var item in inputList)
        {
            if (item != inputList.Last())
            {
                output += item.name + " ";
            }
            else
            {
                output += item.name;
            }
        }
        return output;
    }
}
