using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;
using Ink.UnityIntegration;
using UnityEngine.UI;
using System;

public class SceneManager : MonoBehaviour
{
    public static event Action<Story> OnCreateStory;

    public GameObject eventManager;

    [SerializeField]
    private TextAsset inkJSONAsset = null;
    public Story story;

    [SerializeField]
    private Canvas canvas = null;

    // UI Prefabs


    [SerializeField]
    private Text textPrefab = null;
    [SerializeField]
    private Button buttonPrefab = null;
    

    public void StartEventDialogue() //Parameter for scene value (whatever that is later)
    {
        RemoveChildren();
        StartStory();
    }

    private void Awake()
    {
        RemoveChildren();
        StartStory();
    }

    void StartStory()
    {
        story = new Story(inkJSONAsset.text);
        if (OnCreateStory != null) OnCreateStory(story);
        RefreshView();
    }

    void RefreshView()
    {
        // Remove all the UI on screen
        RemoveChildren();

        // Read all the content until we can't continue any more
        while (story.canContinue)
        {
            // Continue gets the next line of the story
            string text = story.Continue();
            // This removes any white space from the text.
            text = text.Trim();
            // Display the text on screen!
            CreateContentView(text);
        }

        // Display all the choices, if there are any!
        if (story.currentChoices.Count > 0)
        {
            for (int i = 0; i < story.currentChoices.Count; i++)
            {
                Choice choice = story.currentChoices[i];
                Button button = CreateChoiceView(choice.text.Trim());
                // Tell the button what to do when we press it
                button.onClick.AddListener(delegate {
                    OnClickChoiceButton(choice);
                });
                button.onClick.AddListener(Test);
            }
        }
        // If we've read all the content and there's no choices, the story is finished!
        else
        {
            Button choice = CreateChoiceView("End of story.\nRestart?");
            choice.onClick.AddListener(delegate {
                StartStory();
            });
        }
    }

    void Test()
    {
        Debug.Log("this is working");
    }

    // When we click the choice button, tell the story to choose that choice!
    void OnClickChoiceButton(Choice choice)
    {
        story.ChooseChoiceIndex(choice.index);
        RefreshView();
    }

    // Creates a textbox showing the the line of text
    void CreateContentView(string text)
    {
        Text storyText = Instantiate(textPrefab) as Text;
        storyText.text = text;
        storyText.transform.SetParent(canvas.transform, false);
    }

    // Creates a button showing the choice text
    Button CreateChoiceView(string text)
    {
        // Creates the button from a prefab
        Button choice = Instantiate(buttonPrefab) as Button;
        choice.transform.SetParent(canvas.transform, false);

        // Gets the text from the button prefab
        Text choiceText = choice.GetComponentInChildren<Text>();
        choiceText.text = text;

        // Make the button expand to fit the text
        HorizontalLayoutGroup layoutGroup = choice.GetComponent<HorizontalLayoutGroup>();
        layoutGroup.childForceExpandHeight = false;

        return choice;
    }

    // Destroys all the children of this gameobject (all the UI)
    void RemoveChildren()
    {
        int childCount = canvas.transform.childCount;
        for (int i = childCount - 1; i >= 0; --i)
        {
            Destroy(canvas.transform.GetChild(i).gameObject);
        }
    }

    

}
