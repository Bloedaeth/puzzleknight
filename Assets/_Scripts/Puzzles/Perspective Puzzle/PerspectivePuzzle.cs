using GameLogging;
using UnityEngine;

public class PerspectivePuzzle : Puzzle {

	PerspectivePieceManager ppm;
	Camera c;

	public GameObject door;
	public GameObject pieces;

	MeshRenderer[] fixedPieces;
	MeshRenderer[] brokenPieces;

	void Start() {
		ppm = GetComponentInChildren<PerspectivePieceManager> ();
		c = Camera.main;

		fixedPieces = door.GetComponentsInChildren<MeshRenderer> ();
		brokenPieces = pieces.GetComponentsInChildren<MeshRenderer> ();
	}

	public void CheckCamPosition() {
		if (!solved && (-ppm.cam.transform.position + c.transform.position).magnitude <= 0.03) {
			CheckFinalizePuzzle ();
		}
	}

	void FinilizePieces() {
		for (int i = 0; i < fixedPieces.Length; i++) {
			fixedPieces [i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
		}

		for (int i = 0; i < brokenPieces.Length; i++) {
			brokenPieces [i].enabled = false;
		}
	}

	public override void CheckFinalizePuzzle() {
		//Run through each holder to see if the current index matches the correct index
		if (ppm.IndexesAllCorrect ()) {
            BuildDebug.Log("Perspective puzzle solved.");
			solved = true;
            GetComponent<AudioSource>().Play();
			FinilizePieces ();
            FindObjectOfType<CustomAnalytics>().DoorOpened();
		}
	}
}
