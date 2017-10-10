using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameLogging;

public class PressurePlatePuzzle : Puzzle {

	Inventory inv;
	CameraChaser CC;

	bool done = false;
    
	private void Start () {
		inv = FindObjectOfType<Inventory> ();
		CC = GetComponentInChildren<CameraChaser> ();
	}
    
	private void Update () {
		if (!done) {
			CheckFinalizePuzzle ();
		} else {
			if (CC.CurrPoint >= 4) {
				this.solved = true;
				OpenDoor ();
			}
		}
	}

	public override void CheckFinalizePuzzle() {
		if (inv.DoorPieces[(int)DoorPiece.PieceType.Knob])
        {
            BuildDebug.Log("Pressure plate puzzle door opened");
            done = true;
			CC.BeginChase ();
		}
	}
}
