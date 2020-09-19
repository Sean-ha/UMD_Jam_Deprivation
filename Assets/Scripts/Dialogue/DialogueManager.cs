using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    public GameObject dialoguePanel;

    private TextMeshProUGUI dialogueText;
    private TextMeshProUGUI placeholderText;

    private bool isOnPlayerSprite;

    private Image dialogueSprite;
    private Sprite[] currentSprites;
    private Sprite[] playerSprites;

    private bool displayingText;
    private bool isPaused;
    public static bool readyForNextDialogue;

    private float textSpeed = 30;
    private float textIndex;
    private float textIndexPrev;

    private string textFormatted;
    private string textTrue;
    private string[] dialogues;

    private int formattedTracker;
    private int trueTracker;
    private int dialogueArrayTracker;

    private List<int> charsToShake;

    private Responses[] responses;


    private void Awake()
    {
        instance = this;
        dialogueText = dialoguePanel.transform.Find("DialogueText").GetComponent<TextMeshProUGUI>();
        placeholderText = dialoguePanel.transform.Find("PlaceholderText").GetComponent<TextMeshProUGUI>();
        dialogueSprite = dialoguePanel.transform.parent.Find("DialogueSprite").GetComponent<Image>();
        playerSprites = Resources.LoadAll<Sprite>("CharacterPortraits/Player");
    }

    private void Update()
    {
        if (displayingText && !isPaused)
        {
            textIndexPrev = textIndex;
            textIndex += Time.deltaTime * textSpeed;

            int charactersToReveal = Mathf.FloorToInt(textIndex) - Mathf.FloorToInt(textIndexPrev);

            for (int i = 0; i < charactersToReveal; i++)
            {
                // Upon reaching the end of the current dialogue string
                if (trueTracker >= textTrue.Length)
                {
                    displayingText = false;
                    StartCoroutine(HaltProceed());
                    return;
                }
                if (formattedTracker < textFormatted.Length && textFormatted[formattedTracker] == '\n')
                {
                    dialogueText.text += "\n";
                    formattedTracker++;
                    trueTracker++;
                    continue;
                }
                char c = textTrue[trueTracker];
                if (c == '@')
                {
                    trueTracker++;
                    StartCoroutine(PauseTyping());
                    return;
                }
                // Skips typing out tags and automatically adds the whole tag to the dialogue
                else if (c == '<')
                {
                    int tempTracker = trueTracker + 1;
                    while (textTrue[tempTracker] != '>')
                    {
                        tempTracker++;
                    }
                    int difference = tempTracker - trueTracker + 1;
                    dialogueText.text += textTrue.Substring(trueTracker, difference);
                    formattedTracker += difference;
                    trueTracker += difference;
                    continue;
                }
                dialogueText.text += textTrue[trueTracker];
                formattedTracker++;
                trueTracker++;
            }
        }
        // Text shaking
        if (dialogueText.isActiveAndEnabled)
        {
            dialogueText.ForceMeshUpdate();
            TMP_TextInfo textInfo = dialogueText.textInfo;
            TMP_MeshInfo[] cachedMeshInfo = textInfo.CopyMeshInfoVertexData();

            foreach (int index in charsToShake)
            {
                if (index < textInfo.characterInfo.Length)
                {

                    TMP_CharacterInfo charInfo = textInfo.characterInfo[index];

                    // Do not shake characters if they are not visible
                    if (!charInfo.isVisible)
                    {
                        continue;
                    }

                    int materialIndex = textInfo.characterInfo[index].materialReferenceIndex;

                    // Gets index of first vertex used by this text element.
                    int vertexIndex = textInfo.characterInfo[index].vertexIndex;

                    Vector3[] sourceVertices = cachedMeshInfo[materialIndex].vertices;

                    // Determine the center point of each character.
                    Vector2 charMidBasline = (sourceVertices[vertexIndex + 0] + sourceVertices[vertexIndex + 2]) / 2;
                    Vector3 offset = charMidBasline;
                    Vector3[] destinationVertices = textInfo.meshInfo[materialIndex].vertices;

                    destinationVertices[vertexIndex + 0] = sourceVertices[vertexIndex + 0] - offset;
                    destinationVertices[vertexIndex + 1] = sourceVertices[vertexIndex + 1] - offset;
                    destinationVertices[vertexIndex + 2] = sourceVertices[vertexIndex + 2] - offset;
                    destinationVertices[vertexIndex + 3] = sourceVertices[vertexIndex + 3] - offset;

                    Vector3 random = Random.insideUnitCircle * .6f;

                    destinationVertices[vertexIndex + 0] += random;
                    destinationVertices[vertexIndex + 1] += random;
                    destinationVertices[vertexIndex + 2] += random;
                    destinationVertices[vertexIndex + 3] += random;

                    destinationVertices[vertexIndex + 0] += offset;
                    destinationVertices[vertexIndex + 1] += offset;
                    destinationVertices[vertexIndex + 2] += offset;
                    destinationVertices[vertexIndex + 3] += offset;

                    // Updates text meshes
                    for (int i = 0; i < textInfo.meshInfo.Length; i++)
                    {
                        textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                        dialogueText.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
                    }
                }
            }
        }
    }

    // Called when dialogue finishes typing naturally. Ensures players don't accidentally hit 'x' right after
    // dialogue finishes, causing them to miss what was said.
    private IEnumerator HaltProceed()
    {
        yield return new WaitForSeconds(0.3f);
        readyForNextDialogue = true;
    }

    // Sets up dialogue and UI, gets ready to print.
    public void StartDialogue(string[] dialogue, Responses[] response, Sprite[] npcSprites, int id)
    {
        dialoguePanel.SetActive(true);

        dialogues = dialogue;
        responses = response;
        currentSprites = npcSprites;

        PrepareDialogue();
    }

    // Gets the proper string to print by processing the tags included in the string and adding 
    // necessary text effects
    public void PrepareDialogue()
    {
        charsToShake = new List<int>();
        formattedTracker = 0;
        trueTracker = 0;
        textIndex = 0;
        dialogueText.text = "";
        isPaused = false;
        readyForNextDialogue = false;

        textTrue = dialogues[dialogueArrayTracker];

        // Checks for sprite altering
        if(textTrue.Length >= 3)
        {
            string substr = textTrue.Substring(0, 2);
            if(substr == "##")
            {
                int index = System.Int32.Parse(textTrue[2].ToString());
                dialogueSprite.sprite = currentSprites[index];
                dialogueSprite.SetNativeSize();

                if(isOnPlayerSprite) 
                {
                    isOnPlayerSprite = false;
                    // This is to play the entry animation again. It works at least
                    dialogueSprite.gameObject.SetActive(false);
                    dialogueSprite.gameObject.SetActive(true);
                }
                textTrue = textTrue.Remove(0, 3);
            }
            else if(substr == "$$")
            {
                int index = System.Int32.Parse(textTrue[2].ToString());
                dialogueSprite.sprite = playerSprites[index];
                dialogueSprite.SetNativeSize();
                if(!isOnPlayerSprite) 
                {
                    isOnPlayerSprite = true;
                    dialogueSprite.gameObject.SetActive(false);
                    dialogueSprite.gameObject.SetActive(true);
                }
                textTrue = textTrue.Remove(0, 3);
            }
        }
        
        string formattedText = textTrue.Replace("@", "");
        formattedText = ProcessTags(formattedText, true);
        textTrue = ProcessTags(textTrue, false);
        textFormatted = GetFormattedText(formattedText);
    }

    // Allows dialogue to be printed on screen
    public void PrintDialogue()
    {
        displayingText = true;
    }

    // When the dialogue is finished. Resets all dialogue info.
    private void EndDialogue()
    {
        formattedTracker = 0;
        trueTracker = 0;
        dialogueArrayTracker = 0;
        textIndex = 0;

        readyForNextDialogue = false;
        displayingText = false;
        isPaused = false;
        displayingText = false;

        dialoguePanel.GetComponent<DialoguePanel>().CloseDialogue();
    }

    // Called when the player presses 'C'. Advances the current dialogue to the end.
    public void DisplayAllText()
    {
        if (readyForNextDialogue)
        {
            dialogueArrayTracker++;
            // Upon reaching the end of the current dialogue
            if (dialogueArrayTracker >= dialogues.Length)
            {
                // If this dialogue has responses, then display them and let the player choose.
                if (responses.Length > 0)
                {

                }
                else
                {
                    EndDialogue();
                }
            }
            else
            {
                PrepareDialogue();
                PrintDialogue();
            }
        }
        // Instantly finishes the typing effect and displays all text at once
        else if (displayingText)
        {
            StopCoroutine(PauseTyping());
            isPaused = false;
            displayingText = false;
            readyForNextDialogue = true;
            dialogueText.text = textFormatted;
        }
    }

    IEnumerator PauseTyping()
    {
        isPaused = true;
        yield return new WaitForSeconds(0.4f);
        isPaused = false;
    }

    private string GetFormattedText(string text)
    {
        string[] words = text.Split(' ');

        float width = dialogueText.rectTransform.sizeDelta.x - 75;
        float space = GetWordSize(" ");

        string newText = string.Empty;
        float count = 0;

        for (int i = 0; i < words.Length; i++)
        {
            float size = GetWordSize(words[i]);

            if (i == 0)
            {
                newText += words[i];
                count += size;
            }
            else if (count + space > width || count + space + size > width)
            {
                newText += "\n";
                newText += words[i];
                count = size;
            }
            else if (count + space + size <= width)
            {
                newText += " " + words[i];
                count += space + size;
            }
        }
        return newText;
    }

    private float GetWordSize(string word)
    {
        placeholderText.text = word;
        return placeholderText.preferredWidth;
    }

    // Removes all <> tags from the dialogue and stores their info accordingly
    private string ProcessTags(string dialogue, bool addToList)
    {
        int stringLength = dialogue.Length;
        bool inShakeTag = false;

        string noTagString = dialogue;
        int dialogueTracker = 0;

        for (int i = 0; i < stringLength; i++)
        {
            // Note: '<' will never be the last character of a dialogue string
            if (noTagString[i] == '<')
            {
                if (noTagString[i + 1] == '/')
                {
                    // End of shake text
                    if (noTagString[i + 2] == 'a')
                    {
                        dialogue = dialogue.Remove(dialogueTracker, 4);
                        noTagString = noTagString.Remove(i, 4);
                        stringLength = noTagString.Length;
                        inShakeTag = false;
                    }
                    else
                    {
                        int closeTagIndex = i + 2;
                        while (noTagString[closeTagIndex] != '>')
                        {
                            closeTagIndex++;
                        }
                        noTagString = noTagString.Remove(i, closeTagIndex - i + 1);
                        stringLength = noTagString.Length;
                        dialogueTracker += closeTagIndex - i + 1;
                        i--;
                        continue;
                    }
                }
                // Start shake text
                else if (noTagString[i + 1] == 'a')
                {
                    dialogue = dialogue.Remove(dialogueTracker, 3);
                    noTagString = noTagString.Remove(i, 3);
                    stringLength = noTagString.Length;
                    inShakeTag = true;
                }
                // Ignore other tags for purposes of index calculations
                else
                {
                    int closeTagIndex = i + 1;
                    while (noTagString[closeTagIndex] != '>')
                    {
                        closeTagIndex++;
                    }
                    noTagString = noTagString.Remove(i, closeTagIndex - i + 1);
                    stringLength = noTagString.Length;
                    dialogueTracker += closeTagIndex - i + 1;
                    i--;
                    continue;
                }
            }
            if (inShakeTag && addToList)
            {
                charsToShake.Add(i);
            }
            dialogueTracker++;
        }

        return dialogue;
    }
}