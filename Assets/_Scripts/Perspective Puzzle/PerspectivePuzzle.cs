using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectivePuzzle : Puzzle {

	private PerspectivePuzzleStore ps;

	public GameObject leftDoor;
	public GameObject rightDoor;

	private Quaternion leftDoorOpen;
	private Quaternion rightDoorOpen;

	private Camera cam;

    // Use this for initialization
    private void Start() {
		ps = GetComponent<PerspectivePuzzleStore> ();

		solved = false;
		done = false;

		leftDoorOpen = Quaternion.Euler (leftDoor.transform.rotation.eulerAngles + new Vector3(0f, -90f, 0f));
		rightDoorOpen = Quaternion.Euler (rightDoor.transform.rotation.eulerAngles + new Vector3(0f, 90f, 0f));

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
		} else {
			leftDoor.transform.rotation = Quaternion.Slerp (leftDoor.transform.rotation, leftDoorOpen, doorMoveSpeed * Time.deltaTime);
			rightDoor.transform.rotation = Quaternion.Slerp (rightDoor.transform.rotation, rightDoorOpen, doorMoveSpeed * Time.deltaTime);

		}
	}
    
    /// <summary>Checks if the puzzle is to be solved and finalized, and sets the solved variable accordingly.</summary>
	public override void CheckFinalizePuzzle () {

		if ((cam.transform.position + -ps.cameraLocation.transform.position).magnitude < 0.001f && ps.PuzzleSolved()) {
			solved = true;
		}
	}
}