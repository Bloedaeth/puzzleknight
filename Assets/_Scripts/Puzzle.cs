using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Puzzle : MonoBehaviour {
// This will contain the abstract codes for each puzzle

	public bool solved;
	public bool done;

	public float doorMoveSpeed = 0.02f;

	// Use this to make sure that the puzzles are to be finilised. If all pieces are in location, and the puzzle is solved return true, false if otherwise
	public abstract void CheckFinalizePuzzle ();

}
