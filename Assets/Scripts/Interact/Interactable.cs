using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    public Action interact;

    public void Interact()
    {
        interact.Invoke();
    }
}
