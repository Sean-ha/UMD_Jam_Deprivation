using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Opening : MonoBehaviour
{
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
    }

    void Start()
    {
        if(!PlayerStats.playedOpeningCutscene)
        {
            GetComponent<SpriteRenderer>().color = new Color(0, 0, 0, 1);
            playerMovement.SetInteracting();
            StartCoroutine(OpeningCutScene());
        }
    }

    private IEnumerator OpeningCutScene()
    {
        yield return new WaitForSeconds(3f);
        GetComponent<DialogueTrigger>().ActivateTrigger();
    }

    public void FadeAway()
    {
        playerMovement.SetInteracting();
        StartCoroutine(EndDream());
    }

    private IEnumerator EndDream()
    {
        playerMovement.SetInteracting();
        yield return new WaitForSeconds(1f);
        playerMovement.SetInteracting();
        LeanTween.color(gameObject, new Color(0, 0, 0, 0), 6f).setOnComplete(AwakeDialogue);
    }

    private void AwakeDialogue()
    {
        GetComponent<DialogueTrigger>().ActivateTrigger();
    }
}
