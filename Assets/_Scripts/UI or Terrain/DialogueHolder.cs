using GameLogging;
using UnityEngine;
using UnityEngine.UI;

public class DialogueHolder : MonoBehaviour
{
    private GameObject heyYou;
    public string dialogue;
    private DialogueManager dMan;
    public Sprite[] dialogueLines;
    public Image interact;

	private void Start()
    {
        dMan = FindObjectOfType<DialogueManager>();
        heyYou = transform.GetChild(0).gameObject;
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            BuildDebug.Log("Showing E to Interact image");
            interact.enabled = true;

            if(heyYou.activeInHierarchy)
                heyYou.SetActive(false);
        }
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
                    BuildDebug.Log("Showing dialog");
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
            BuildDebug.Log("Hiding E to interact");
            BuildDebug.Log("Hiding dialog - player left area");
            interact.enabled = false;
			dMan.HideDialogue();
		}
    }
}
