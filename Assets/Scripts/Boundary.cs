using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{
    public Vector2 directionToMove;

    private DialogueTrigger dialogueTrigger;
    private PlayerMovement playerMovement;

    private void Awake()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        dialogueTrigger = GetComponent<DialogueTrigger>();
    }

    private IEnumerator WalkAway(PlayerMovement playerMovement)
    {
        playerMovement.horizontalInput = directionToMove.x;
        playerMovement.verticalInput = directionToMove.y;
        yield return new WaitForSeconds(0.25f);
        playerMovement.horizontalInput = 0;
    }

    private void TriggerBoundary()
    {
        PlayerMovement playerMovement = FindObjectOfType<PlayerMovement>();
        playerMovement.SetInteracting();
        StartCoroutine(WalkAway(playerMovement));
        GetComponent<DialogueTrigger>().ActivateTrigger();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            TriggerBoundary();
        }
    }
}
