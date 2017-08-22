using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectivePuzzle : Puzzle {

	private PerspectivePuzzleStore ps;

	private Camera cam;

    // Use this for initialization
    private void Start() {
		ps = GetComponent<PerspectivePuzzleStore> ();

		solved = false;
		done = false;

		cam = Camera.main;
	}

    // Update is called once per frame
    private void Update() {

		if (!done) {
			if (ps.checkPuzzle) {
				CheckFinalizePuzzle ();
			}
			if (solved) {
				ps.FinalizePuzzle ();
				done = true;
			}
		}
	}
    
    /// <summary>Checks if the puzzle is to be solved and finalized, and sets the solved variable accordingly.</summary>
	public override void CheckFinalizePuzzle () {

		if ((cam.transform.position + -ps.cameraLocation.transform.position).magnitude < 0.001f && ps.PuzzleSolved()) {
			solved = true;
		}
	}
}