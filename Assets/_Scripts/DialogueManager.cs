using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DialogueManager : MonoBehaviour {

    public GameObject dBox;
    public Text dtext;
    public bool dialogueActive;

    public string [] dialogueLines;
    public int currentLine;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (dialogueActive && Input.GetKeyUp(KeyCode.E))
        {
            //dBox.SetActive(false);
            //dialogueActive = false;
            currentLine++;
        }

        if (currentLine >= dialogueLines.Length)
        {
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
}
