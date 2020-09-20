using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractBox : MonoBehaviour
{
    public bool canSkip;

    private bool canInteract;

    private GameObject collisionDialogue;
    private GameObject interactableObject;

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
                else if(interactableObject != null)
                {
                    playerMovement.SetInteracting();
                    interactableObject.GetComponent<Interactable>().Interact();
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
        else if(collision.CompareTag("Interactable"))
        {
            canInteract = true;
            interactableObject = collision.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("InteractableDialogue"))
        {
            canInteract = false;
            collisionDialogue = null;
        }
        else if(collision.CompareTag("Interactable"))
        {
            canInteract = false;
            interactableObject = null;
        }
    }
}
