using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    void Start()
    {
        Interactable interactable = GetComponent<Interactable>();
        interactable.interact = GetComponent<SceneSwitcher>().EnterScene;
    }
}
