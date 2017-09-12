using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectivePieceHolderUiInteractor : MonoBehaviour {

	public PerspectivePieceUiButtonManager ButtonManager;
	public bool IsOpen = false;

	private PerspectivePieceHolder h;

	float uiOpenRate = 0.2f;
	float uiOpenTime;

	void Start() {
		if (!ButtonManager) {
			Debug.LogWarning ("PuzzlePieceChooser ui is not set for object " + gameObject.name +".\nUsing Generic Find command.");
			ButtonManager = FindObjectOfType<PerspectivePieceUiButtonManager> ();
		}

		h = GetComponent<PerspectivePieceHolder> ();
	}


	private void OnTriggerEnter(Collider other)
	{
		Player p = other.GetComponent<Player>();
		if (p) {
			p.NearInteractableObject = true;
			ButtonManager.UpdateHolder (h);
			ButtonManager.UpdatePieces(p.GetComponent<Inventory>().GetDoorPieces());
		}
		
	}

	private void OnTriggerStay(Collider other)
	{
		if(Input.GetKeyDown(KeyCode.E) && Time.time > uiOpenTime)
		{
			Player p = other.GetComponent<Player>();
			if (p) {
				uiOpenTime = Time.time + uiOpenRate;
				TogglePuzzlePieceUI ();
				p.StopMovement ();
			}
		}
	}

	private void OnTriggerExit(Collider other)
	{
		Player p = other.GetComponent<Player>();
		if (p) {
			p.NearInteractableObject = false;
			p.StopMovement (false);
			TogglePuzzlePieceUI (false);
		}
	}

	void TogglePuzzlePieceUI () {
		IsOpen = !IsOpen;
		ButtonManager.gameObject.SetActive (IsOpen);
	}

	public void TogglePuzzlePieceUI (bool state) {
		IsOpen = !state;
		TogglePuzzlePieceUI ();
	}
}
