using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoad : MonoBehaviour
{
    public static SceneLoad instance;

    private static Vector3 playerPosition;

    private static Vector2 directionToFace;
    private static bool shouldMovePlayer;

    private GameObject player;

    // This object should only be in the beginning scene (or save points)
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += UpdatePlayer;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= UpdatePlayer;
    }

    private void UpdatePlayer(Scene scene, LoadSceneMode mode)
    {
        if (shouldMovePlayer)
        {
            player = FindObjectOfType<PlayerMovement>().gameObject;
            Transform playerTransform = player.transform;

            // If z position is not zero, then use the default position.
            if (playerPosition.z == 0)
            {
                // Move player to correct position
                playerTransform.localPosition = playerPosition;
            }

            // Makes the player face the proper direction upon loading scene
            player.GetComponent<Animator>().SetFloat("LastHorizontal", directionToFace.x);
            player.GetComponent<Animator>().SetFloat("LastVertical", directionToFace.y);

            shouldMovePlayer = false;
        }
    }

    public void SetPosition(Vector3 position)
    {
        playerPosition = position;
        shouldMovePlayer = true;
    }

    public void SetDirection(Vector2 direction)
    {
        directionToFace = direction;
    }
}