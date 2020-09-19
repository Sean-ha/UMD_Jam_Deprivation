﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public DialogueObject[] dialogueList;
    public string npcName;
    public int npcID;

    private DialogueManager dialogueManager;
    private int currentArrayCounter;
    private Sprite[] npcSprites;

    private void Start()
    {
        dialogueManager = DialogueManager.instance;
        npcSprites = Resources.LoadAll<Sprite>("CharacterPortraits/" + npcName);

        // currentArrayCounter = AdvanceDialogue.dialogueTracker[npcID];
    }

    public void ActivateTrigger()
    {
        // currentArrayCounter = AdvanceDialogue.SetDialogue(npcID, currentArrayCounter);

        dialogueManager.StartDialogue(dialogueList[currentArrayCounter].dialogue,
            dialogueList[currentArrayCounter].responseOptions, npcSprites, dialogueList[currentArrayCounter].id);
        if (dialogueList[currentArrayCounter].goToNextDialogue)
        {
            currentArrayCounter++;
        }
    }
}