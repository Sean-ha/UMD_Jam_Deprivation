using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartGame : MonoBehaviour
{
    // Called when player presses "Start Game" on title screen
    public void OnStartGameButtonClick()
    {
        AdvanceDialogue.dialogueTracker = new int[20];
        SoundManager.PlaySound(SoundManager.Sound.Click);
        SceneManager.LoadScene("RuiBedroom");
    }
}
