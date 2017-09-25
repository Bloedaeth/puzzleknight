using UnityEngine;
using UnityEngine.UI;

public class DialogueHolder : MonoBehaviour
{
    public string dialogue;
    private DialogueManager dMan;
    public string[] dialogueLines;
    public Image interact;

	private void Start()
    {
        dMan = FindObjectOfType<DialogueManager>();
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
            interact.enabled = true;
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.CompareTag("Player"))
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

    private void OnTriggerExit(Collider other)
    {
		if(other.CompareTag("Player")) 
		{
			interact.enabled = false;
			dMan.HideDialogue();
		}
    }
}
