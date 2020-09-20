using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    void Start()
    {
        GetComponent<Interactable>().interact = GetComponent<SceneSwitcher>().EnterScene;
    }
}
