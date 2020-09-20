using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueObject[] dialogueList;
    public string npcName;
    public int npcID;
    public AudioClip typeSound;

    private DialogueManager dialogueManager;
    private int currentArrayCounter;
    private Sprite[] npcSprites;

    private void Start()
    {
        if(typeSound == null)
        {
            typeSound = Resources.Load<AudioClip>("PlayerSound");
        }
        dialogueManager = DialogueManager.instance;
        npcSprites = Resources.LoadAll<Sprite>("CharacterPortraits/" + npcName);

        if(npcID != 0)
        {
            currentArrayCounter = AdvanceDialogue.dialogueTracker[npcID - 1];
        }
    }

    public void ActivateTrigger()
    {
        if(npcID != 0)
        {
            currentArrayCounter = AdvanceDialogue.SetDialogue(npcID, currentArrayCounter);
        }

        dialogueManager.StartDialogue(dialogueList[currentArrayCounter].dialogue,
            dialogueList[currentArrayCounter].responseOptions, npcSprites, dialogueList[currentArrayCounter].id,
            typeSound);

        if (dialogueList[currentArrayCounter].goToNextDialogue)
        {
            currentArrayCounter++;
            if(npcID != 0)
            {
                AdvanceDialogue.SetDialogue(npcID, currentArrayCounter);
            }
        }
    }
}