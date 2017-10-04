using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameLogging;

public class DialogueManager : MonoBehaviour
{
    public GameObject dBox;
    public Text dText;
    public bool dialogueActive;

    public string[] dialogueLines;
    public int currentLine;

    private void Update()
    {
        if(dialogueActive && Input.GetKeyUp(KeyCode.E))
        {
            BuildDebug.Log("Showing next line of dialog");
            //dBox.SetActive(false);
            //dialogueActive = false;
            currentLine++;
        }

        if(currentLine >= dialogueLines.Length)
        {
            BuildDebug.Log("Hiding dialog - all text shown");
            dBox.SetActive(false);
            dialogueActive = false;

            currentLine = 0;
        }

        dText.text = dialogueLines[currentLine];
    }

    public void ShowBox(string dialogue)
    {
        dialogueActive = true;
        dBox.SetActive(true);
        dText.text = dialogue;
    }

    public void ShowDialogue()
    {
        dialogueActive = true;
        dBox.SetActive(true);
    }

    public void HideDialogue()
    {
        dialogueActive = false;
        dBox.SetActive(false);
    }
}
