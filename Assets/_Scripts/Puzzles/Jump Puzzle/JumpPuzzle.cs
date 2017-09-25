using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpPuzzle: Puzzle {

	Inventory inv;
	CameraChaser CC;

	bool done = false;

	// Use this for initialization
	void Start () {
		inv = FindObjectOfType<Inventory> ();
		CC = GetComponentInChildren<CameraChaser> ();
	}

	// Update is called once per frame
	void Update () {
		if (!done) {
			CheckFinalizePuzzle ();
		} else {
			if (CC.currPoint >= 4) {
				this.solved = true;
				OpenDoor ();
			}
		}
	}

	public override void CheckFinalizePuzzle() {
		if (inv.GetDoorPieces () [(int)DoorPiece.PieceType.Frame]) {
			done = true;
			CC.BeginChase ();
		}
	}
}
