using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>Contains the abstract code required for each puzzle.</summary>
public abstract class Puzzle : MonoBehaviour {

	public bool solved;
	public bool done;

	public float doorMoveSpeed = 0.02f;

    /// <summary>Checks if the puzzle is to be solved and finalized, and sets the solved variable accordingly.</summary>
    public abstract void CheckFinalizePuzzle ();
}
