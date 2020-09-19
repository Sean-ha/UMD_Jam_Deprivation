using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu]
public class DialogueObject : ScriptableObject
{
    [TextArea(6, 10)]
    public string[] dialogue;
    public Responses[] responseOptions;
    public int id;
    public bool goToNextDialogue;
}

[Serializable]
public struct Responses
{
    public string responseText;
    public DialogueObject responseObject;
}