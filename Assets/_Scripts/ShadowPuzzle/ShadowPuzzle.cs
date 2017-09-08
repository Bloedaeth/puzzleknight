using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowPuzzle : Puzzle {


	void OnTriggerEnter(Collider o) {
		if (o.transform.tag == "Player") {
			CheckFinalizePuzzle ();
		}
	}

	public override void CheckFinalizePuzzle () {
			this.solved = true;
	}
}
