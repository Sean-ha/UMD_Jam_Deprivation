using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitcher : MonoBehaviour
{
    public string sceneToSwitchTo;
    public Vector3 playerPosition;
    public Vector2 directionToFace;
    public int id;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            switch (id)
            {
                case 1:
                    if (!PlayerStats.investigatedDresser)
                    {
                        PlayerMovement playerMovement = collision.GetComponent<PlayerMovement>();
                        playerMovement.SetInteracting();
                        StartCoroutine(WalkRight(playerMovement));
                        GetComponent<DialogueTrigger>().ActivateTrigger();
                        return;
                    }
                    break;
            }

            collision.GetComponent<PlayerMovement>().SetInteracting();
            SceneLoad.instance.SetPosition(playerPosition);
            SceneLoad.instance.SetDirection(directionToFace);
            FadeToBlack.instance.FadeIntoBlack(LoadScene);
        }
    }

    private IEnumerator WalkRight(PlayerMovement playerMovement)
    {
        playerMovement.horizontalInput = .5f;
        yield return new WaitForSeconds(0.25f);
        playerMovement.horizontalInput = 0;
    }

    private void LoadScene()
    {
        SceneManager.LoadScene(sceneToSwitchTo);
    }
}