using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdvanceDialogue
{
    // Stores what dialogue every npc is currently on
    public static int[] dialogueTracker;

    public static int SetDialogue(int npcID, int currentDialogue)
    {
        switch (npcID)
        {
            case 0: return 0;
            case 1: return Dresser(npcID, currentDialogue);
        }
        return 0;
    }

    private static int Dresser(int npcID, int currentDialogue)
    {
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