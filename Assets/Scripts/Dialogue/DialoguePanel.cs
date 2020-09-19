using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialoguePanel : MonoBehaviour
{
    private RectTransform canvasRT;
    private RectTransform dialoguePanelRT;
    private RectTransform dialogueSpriteRT;
    private TextMeshProUGUI dialogueText;

    private DialogueManager dialogueManager;
    private PlayerMovement playerMovement;
    private InteractBox interactBox;

    private void Awake()
    {
        canvasRT = GameObject.Find("DialogueCanvas").GetComponent<RectTransform>();
        dialoguePanelRT = GetComponent<RectTransform>();
        dialogueSpriteRT = canvasRT.Find("DialogueSprite").GetComponent<RectTransform>();
        dialogueText = dialoguePanelRT.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        dialogueManager = FindObjectOfType<DialogueManager>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        interactBox = FindObjectOfType<InteractBox>();

        dialogueSpriteRT.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        LeanTween.alpha(dialoguePanelRT, 1, 0);
        LeanTween.alpha(dialogueSpriteRT, 1, 0).setOnComplete(PrintDialogue);
    }

    private void PrintDialogue()
    {
        dialogueSpriteRT.gameObject.SetActive(true);
        interactBox.canSkip = true;
        dialogueManager.PrintDialogue();
    }

    public void CloseDialogue()
    {
        dialogueSpriteRT.GetComponent<Animator>().Play("DialogueSpriteExit");
        StartCoroutine(CloseAll());
    }

    private void CloseEverything()
    {
        interactBox.canSkip = false;
        LeanTween.alpha(dialoguePanelRT, 0, 0.3f);
        LeanTween.value(dialogueText.gameObject, a => dialogueText.color = a,
            new Color(0, 0, 0, 1), new Color(0, 0, 0, 0), 0.3f).setOnComplete(DisablePanel);
    }

    private IEnumerator CloseAll()
    {
        yield return new WaitForSeconds(0.5f);
        CloseEverything();
    }

    private void DisablePanel()
    {
        dialogueText.color = new Color(0, 0, 0, 1);
        playerMovement.isInteracting = false;
        gameObject.SetActive(false);
    }
}