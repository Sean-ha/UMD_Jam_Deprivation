using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceDialogue
{
    // Stores what dialogue every npc is currently on
    public static int[] dialogueTracker;

    private static int npcID;
    private static int currentDialogue;

    public static int SetDialogue(int id, int currDialog)
    {
        npcID = id;
        currentDialogue = currDialog;

        switch (npcID)
        {
            case 0: return 0;
            case 1: return Dresser();
            case 2: return Bed();
            case 3: return Sen();
            case 4: return Bera();
        }
        return 0;
    }

    private static int Bera()
    {
        // After player hears about the bed, go to normal functionality
        if(PlayerStats.beraDialogue1)
        {
            dialogueTracker[npcID - 1] = currentDialogue;
            return currentDialogue;
        }
        // If player talked to Sen, then talk about the bed.
        else if(PlayerStats.senDialogue1)
        {
            dialogueTracker[npcID - 1] = 1;
            return 1;
        }
        return currentDialogue;
    }

    private static int Sen()
    {
        if(PlayerStats.beraDialogue1)
        {
            dialogueTracker[npcID - 1] = 2;
            return 2;
        }
        dialogueTracker[npcID - 1] = currentDialogue;
        return currentDialogue;
    }

    private static int Bed()
    {
        if(PlayerStats.wentUnderBed)
        {
            dialogueTracker[npcID - 1] = 2;
            return 2;
        }
        if(PlayerStats.beraDialogue1)
        {
            dialogueTracker[npcID - 1] = 1;
            return 1;
        }
        return currentDialogue;
    }

    private static int Dresser()
    {
        // Different dialogue if you've already talked to Bera
        if(PlayerStats.beraDialogue1)
        {
            dialogueTracker[npcID - 1] = 2;
            return 2;
        }
        if(currentDialogue == 0)
        {
            PlayerStats.investigatedDresser = true;
        }
        dialogueTracker[npcID - 1] = currentDialogue;
        return currentDialogue;
    }

    private static int WG(int currentDialogue)
    {
        /*if()
        {
        }*/
        dialogueTracker[0] = currentDialogue;
        return currentDialogue;
    }
}