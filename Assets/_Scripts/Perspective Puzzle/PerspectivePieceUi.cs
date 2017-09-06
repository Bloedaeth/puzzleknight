using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectivePieceUi : MonoBehaviour {

	public Transform PuzzlePieceUi;
	private bool IsOpen = false;

	private PerspectivePieceHolder h;

	float uiOpenRate = 0.2f;
	float uiOpenTime;

	void Start() {
		if (!PuzzlePieceUi) {
			Debug.LogWarning ("PuzzlePieceChooser ui is not set for object " + gameObject.name +".\nUsing Generic Find command.");
			PuzzlePieceUi = FindObjectOfType<PerspectiveButtonManager> ().transform;
		}

		h = GetComponent<PerspectivePieceHolder> ();
	}


	private void OnTriggerEnter(Collider other)
	{
		Player p = other.GetComponent<Player>();
		if (p) {
			p.NearInteractableObject = true;
			PuzzlePieceUi.GetComponent<PerspectiveButtonManager> ().UpdateHolder (h);
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
		PuzzlePieceUi.gameObject.SetActive (IsOpen);
	}

	public void TogglePuzzlePieceUI (bool state) {
		IsOpen = !state;
		TogglePuzzlePieceUI ();
	}
}
