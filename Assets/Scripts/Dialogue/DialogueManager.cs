using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Ink.Runtime;
using System;
using UnityEngine.EventSystems;


public class DialogueManager : MonoBehaviour
{

    private static DialogueManager instance;
    [Header("Dialogue UI")]                          //Makes it more readable in the Inspector
    [SerializeField]
    private GameObject dialoguePanel;

    [SerializeField]
    private TextMeshProUGUI dialogueText;

    private Story currentStory;                     //Current INK file to display;

    
    public bool dialogueIsPlaying { get; private set; }


    [Header("Choices UI")]
    [SerializeField]
    private GameObject[] choices;                   //Here in the Inspector, we drag and drop the Choices game objects from the dialogue panel

    private TextMeshProUGUI[] choicesText;




    private void Awake()
    {
        if (instance != null)
            Debug.Log("Found more than one!");          //Only one dialogue can be playing at a time.
        instance = this;

    }

    public static DialogueManager GetInstance()
    {
        return instance;
    }
    private void Start()
    {
        
        dialogueIsPlaying = false;                                      //We disable the dialogue panel cause the game never starts with a dialogue.
        dialoguePanel.SetActive(false);

        choicesText = new TextMeshProUGUI[choices.Length];                      
        int index = 0;
        foreach (GameObject choice in choices)
        {
            choicesText[index] = choice.GetComponentInChildren<TextMeshProUGUI>();
            index++;
        }
    }

    private void Update()
    {       
        if (!dialogueIsPlaying)                                        //If a dialogue is currently being playing, then when the user presses X the dialogue will continue to the next text.
            return;
        if (Input.GetKeyDown(KeyCode.X))
            ContinueStory();

    }

    public void EnterDialogueMode(TextAsset inkJSON)                    // The start of the dialogue
    {
        currentStory = new Story(inkJSON.text);                     
        dialogueIsPlaying=true;
        dialoguePanel.SetActive(true);

        ContinueStory();
        

    }

    private IEnumerator ExitDialogueMode()
    {
        yield return new WaitForSeconds(0.2f);                          //Its good practice to close things and wait for a few frames so that if the user presses a button while closing the dialogue
                                                                        //this action doesn't transfer to the aftermath.

        dialogueIsPlaying = false;
        dialoguePanel.SetActive(false);                                 //We reset everything.
        dialogueText.text = "";
    }

    private void ContinueStory()
    {
        if (currentStory.canContinue)
        {
            dialogueText.text = currentStory.Continue();
            DisplayChoices();

        }
        else
        {
            StartCoroutine(ExitDialogueMode());
        }

    }

    private void DisplayChoices()
    {
        List<Choice> currentChoices = currentStory.currentChoices;                  //A list of all the choices we have in the current "story"

        //Debug.Log("Current choices are: "+currentChoices.Count+ "and the amout of choices allowed are: "+ choices.Length);
        if (currentChoices.Count > choices.Length)                                  //If we have more choices than we can handle.
        {
            Debug.LogError("Lots of choices were given!");
        }

        int index = 0;
        foreach (Choice choice in currentChoices)
        {
            choices[index].gameObject.SetActive(true);
            choicesText[index].text = choice.text;

            index++;
        }

        for(int i = index; i < choices.Length; i++)
        {
            
            choices[i].gameObject.SetActive(false);
            
        }
        StartCoroutine(SelectFirstChoice());

    }

    private IEnumerator SelectFirstChoice()
    {
        
        EventSystem.current.SetSelectedGameObject(null);
        yield return new WaitForEndOfFrame();
        //EventSystem.current.SetSelectedGameObject(choices[0].gameObject);             //Enable this only if you want the first choice to be pre-selected when the dialogue starts.
    }    

    public void MakeChoice(int choiceIndex)
    {
  
        currentStory.ChooseChoiceIndex(choiceIndex);
        ContinueStory();

    }
}