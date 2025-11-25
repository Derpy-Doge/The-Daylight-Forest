using NUnit.Framework;
using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using UnityEngine.InputSystem;

[Serializable]

public struct DialoguePiece
{
    public string name;
    [TextArea] public string dialogue;
}

public class Dialogue : MonoBehaviour
{
    public List<DialoguePiece> dialogue;
    public float textSpeed = 0.1f;

    public TMPro.TMP_Text dialogueName; 
    public TMPro.TMP_Text dialogueText;

    private int dialogueIndex;

    public void StartDialogue()
    {
        gameObject.SetActive(true);

        StartCoroutine(WriteDialoguePiece(dialogue[0]));
    }

    public void StopDialogue()
    {
        gameObject.SetActive(false);
    }

    public void NextDialogueOrStop(InputAction.CallbackContext ctx)
    {
        if (ctx.ReadValue<float>() == 0)
            return;

        ++dialogueIndex;

        if (dialogueIndex >= dialogue.Count)
        {
            StopDialogue();
            return;
        }

        StartCoroutine(WriteDialoguePiece(dialogue[dialogueIndex]));
    }

    public IEnumerator WriteDialoguePiece(DialoguePiece dialogue)
    {
        dialogueName.SetText(dialogue.name);
        dialogueText.SetText("");

        for (int i=0; i < dialogue.dialogue.Length; ++i)
        {
            dialogueText.text += dialogue.dialogue[i];
            yield return new WaitForSeconds(textSpeed);
        }

        yield return null;

    }
}
