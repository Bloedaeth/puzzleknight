using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectivePuzzle : Puzzle {

	PerspectivePieceManager ppm;

	void Start() {
		ppm = GetComponentInChildren<PerspectivePieceManager> ();
	}

	public override void CheckFinalizePuzzle() {
		//Run through each holder to see if the current index matches the correct index
		return;
	}
}
