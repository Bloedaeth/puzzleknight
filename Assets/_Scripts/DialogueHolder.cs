using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueHolder : MonoBehaviour {
    public string dialogue;
    private DialogueManager dMan;
    public string[] dialogueLines;


	// Use this for initialization
	void Start () {
        dMan = FindObjectOfType<DialogueManager>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void OnTriggerStay(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            if(Input.GetKeyUp(KeyCode.E))
            {
                //dMan.ShowBox(dialogue);

                 if(!dMan.dialogueActive)
                 {
                    dMan.dialogueLines = dialogueLines;
                    dMan.currentLine = -1;
                    dMan.ShowDialogue();
                 }

            }
        }
    }
}
