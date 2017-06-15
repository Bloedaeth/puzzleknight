using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blocks : MonoBehaviour {

	public static PerspectivePuzzleStore ps;
	public static Transform door;
	public static Transform cam;

	public Blocks(PerspectivePuzzleStore puzzleStore, Transform doorLocation, Transform cameraLocation) {
		ps = puzzleStore;
		door = doorLocation;
		cam = cameraLocation;
	}

}
