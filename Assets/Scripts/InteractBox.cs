using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBox : MonoBehaviour
{
    public bool canSkip;

    private bool canInteract;

    private GameObject collisionDialogue;

    private PlayerMovement playerMovement;
    private DialogueManager dialogueManager;

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        dialogueManager = DialogueManager.instance;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.C))
        {
            if (!playerMovement.isInteracting && canInteract)
            {
                if (collisionDialogue != null)
                {
                    playerMovement.SetInteracting();
                    collisionDialogue.GetComponent<DialogueTrigger>().ActivateTrigger();
                }
            }
            else if (playerMovement.isInteracting && canSkip)
            {
                if (Input.GetKeyDown(KeyCode.C))
                {
                    dialogueManager.DisplayAllText();
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("InteractableDialogue"))
        {
            canInteract = true;
            collisionDialogue = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("InteractableDialogue"))
        {
            canInteract = false;
            collisionDialogue = null;
        }
    }
}
