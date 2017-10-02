using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using GameLogging;

public class DialogueManager : MonoBehaviour
{
    public GameObject dBox;
    public Text dtext;
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

        dtext.text = dialogueLines[currentLine];
    }

    public void ShowBox(string dialogue)
    {
        dialogueActive = true;
        dBox.SetActive(true);
        dtext.text = dialogue;
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
