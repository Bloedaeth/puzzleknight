using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Contains the abstract code required for each puzzle.</summary>
public abstract class Puzzle : MonoBehaviour {

	public GameObject leftDoor;
	public GameObject rightDoor;

	protected Quaternion leftDoorOpen;
	protected Quaternion rightDoorOpen;

	/// <summary>
	/// The variables to test if the puzzle has been solved as of yet.
	/// </summary>
	protected bool solved;

	private float doorMoveSpeed = 2f;

	void Awake() {
		if (leftDoor == null || rightDoor == null) {
			Debug.LogError ("One or both of the doors for the" + gameObject.name + "puzzle are not set.");
		}

		leftDoorOpen = Quaternion.Euler (leftDoor.transform.rotation.eulerAngles + new Vector3(0f, -90f, 0f));
		rightDoorOpen = Quaternion.Euler (rightDoor.transform.rotation.eulerAngles + new Vector3(0f, 90f, 0f));
	}

	public void Update() {
		if (solved) {
			OpenDoor ();
		}
	}

	public void OpenDoor() {
		leftDoor.transform.rotation = Quaternion.Lerp (leftDoor.transform.rotation, leftDoorOpen, doorMoveSpeed * Time.deltaTime);
		rightDoor.transform.rotation = Quaternion.Lerp (rightDoor.transform.rotation, rightDoorOpen, doorMoveSpeed * Time.deltaTime);
	}

    /// <summary>Checks if the puzzle is to be solved and finalized, and sets the solved variable accordingly.</summary>
    public abstract void CheckFinalizePuzzle ();
}
