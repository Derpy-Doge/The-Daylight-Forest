using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;

[Serializable]

public struct DialoguePiece
{
    public string name;
    [TextArea] public string dialogue;
}

public class Dialogue : MonoBehaviour
{
    public List<DialoguePiece> dialogue;

    public TMPro.TMP_Text dialogueName; 
    public TMPro.TMP_Text dialogueText;

    public void StartDialogue()
    {
        gameObject.SetActive(true);
        StartCoroutine(WriteDialoguePiece(dialogue[0]));
    }

    public IEnumerator WriteDialoguePiece(DialoguePiece dialogue)
    {
        dialogueName.SetText(dialogue.name);
        dialogueText.SetText(dialogue.dialogue);

        yield return null;

    }
}
