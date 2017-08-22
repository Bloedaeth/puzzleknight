using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPuzzle : Puzzle {

	/// <summary>
	/// The trigger zone hit variable is used to determine if the trigger zone at the end of the puzzle has been hit, to open the door.
	/// </summary>
	private bool triggerZoneHit;

	// Use this for initialization
	void Start () {
		triggerZoneHit = false;
	}

	void OnTriggerEnter(Collider o) {
		if (o.transform.tag == "Player") {
			CheckFinalizePuzzle ();
		}
	}

	public override void CheckFinalizePuzzle () {
			this.solved = true;
	}
}
