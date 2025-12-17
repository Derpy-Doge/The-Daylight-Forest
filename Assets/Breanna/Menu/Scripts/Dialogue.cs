using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine.InputSystem;

[Serializable]

public struct DialoguePiece
{
    [TextArea] public string dialogue;
}

public class Dialogue : MonoBehaviour
{
    public List<DialoguePiece> dialogue;
    public float textSpeed = 0.1f;

    public TMPro.TMP_Text dialogueText;

    private int dialogueIndex;
    private bool IsDialogueRunning;

    private static Dialogue currentDialogue;

    public void StartDialogue()
    {
        currentDialogue = this;

        StopAllCoroutines();
        gameObject.SetActive(true);
        dialogueIndex = 0;

        StartCoroutine(WriteDialoguePiece(dialogue[0]));
    }

    public void StopDialogue()
    {
        gameObject.SetActive(false);
    }

    public void NextDialogueOrStop(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValue<float>() == 0 || currentDialogue.IsDialogueRunning)
            return;

        ++currentDialogue.dialogueIndex;

        if (currentDialogue.dialogueIndex >= currentDialogue.dialogue.Count)
        {
            currentDialogue.StopDialogue();
            return;
        }

        currentDialogue.StartCoroutine(currentDialogue.WriteDialoguePiece(currentDialogue.dialogue[currentDialogue.dialogueIndex]));
    }

    public IEnumerator WriteDialoguePiece(DialoguePiece dialogue)
    {
        dialogueText.SetText("");

        IsDialogueRunning = true;

        for (int i=0; i < dialogue.dialogue.Length; ++i)
        {
            dialogueText.text += dialogue.dialogue[i];
            yield return new WaitForSeconds(textSpeed);
        }

        IsDialogueRunning = false;


    }
}
