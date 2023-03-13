using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{

    [Header("Visual Cue")]
    [SerializeField]
    private GameObject visualCue;

    [Header("Ink JSON")]
    [SerializeField] private TextAsset inkJSON;


    [SerializeField]
    private bool hasSpoken;                                  //Sometimes we want our NPCs to only be spoken to once, after that we disable the dialogue options for them.
    private bool playerInRange;
    private void Awake()
    {
        playerInRange = false;
        visualCue.SetActive(false);
    }

    private void Start()
    {
        hasSpoken = false;
    }


    private void Update()
    {
        if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying && !hasSpoken)            //If player is in range and no dialogue is active right now and if the NPC has not spoken before.
        {
            visualCue.SetActive(true);
            if (Input.GetKeyDown(KeyCode.E))
            {
                DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
                hasSpoken=true;                                                                        //Once they have spoken, we make true this variable so that dialogue won't open up next time.
            
            }

        }


        else
        {
            visualCue.SetActive(false);

        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        playerInRange = false;
    }

}
