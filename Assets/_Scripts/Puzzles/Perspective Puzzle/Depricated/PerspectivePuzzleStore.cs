using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectivePuzzleStore : MonoBehaviour {

	public bool puzzleDone;
	public bool checkPuzzle;

	private Vector3 doorToCam;

	public PerspectiveButton button;


	private Vector3 buttonStartLoc;
	private Vector3 buttonEndLoc;

	//Stores the pieces of the perspective puzzle
	public GameObject blockPusher;
	public GameObject gridPiece;
	public GameObject cameraLocation;
	public GameObject doorLocation;

	public Vector3[,] grid;
	public int[,] occupiedGridLocations;
	public const int gridSize = 7;

	private float minPieceDistanceFromCamera = 5f;

	private float minPieceDistance; // equals cameraLocation.z + minPieceDistanceFromCamera
	private float maxPieceDistance; // equals minPieceDistance + (blockPieceSize * pieceCount)

	public GameObject brokenTopLeft;
	public GameObject brokenTopRight;
	public GameObject brokenBottomLeft;
	public GameObject brokenBottomRight;
	public GameObject brokenKnobLeft;
	public GameObject brokenKnobRight;
	public GameObject brokenFrame;
	public GameObject brokenBase;

	public GameObject solvedTopLeft;
	public GameObject solvedTopRight;
	public GameObject solvedBottomLeft;
	public GameObject solvedBottomRight;
	public GameObject solvedKnobLeft;
	public GameObject solvedKnobRight;
	public GameObject solvedFrame;
	public GameObject solvedBase;

	// For iterations
	GameObject[] solvedPieceList;
	GameObject[] brokenPieceList;

    // Runs with the first frame
    private void Start() {
		puzzleDone = false;

		doorToCam = -doorLocation.transform.localPosition + cameraLocation.transform.localPosition;

		PushingBlock.cam = cameraLocation.transform;
		PushingBlock.door = doorLocation.transform;
		PushingBlock.ps = this;

		brokenPieceList = new GameObject[] {
			brokenFrame,
			brokenBase ,
			brokenTopLeft,
			brokenTopRight,
			brokenBottomLeft,
			brokenBottomRight,
			brokenKnobLeft,
			brokenKnobRight
		};

		solvedPieceList = new GameObject[] {
			solvedFrame,
			solvedBase ,
			solvedTopLeft,
			solvedTopRight,
			solvedBottomLeft,
			solvedBottomRight,
			solvedKnobLeft,
			solvedKnobRight
		};

		minPieceDistance = cameraLocation.transform.localPosition.z + minPieceDistanceFromCamera; // equals cameraLocation.z + minPieceDistanceFromCamera
		maxPieceDistance = minPieceDistance + ((blockPusher.transform.localScale.x * 2) * brokenPieceList.Length); // equals minPieceDistance + (blockPieceSize * pieceCount)

		occupiedGridLocations = new int[brokenPieceList.Length,2];

		for (int i = 0; i < brokenPieceList.Length; i++) {
			brokenPieceList [i].GetComponent<MeshRenderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
			solvedPieceList [i].GetComponent<MeshRenderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;

			brokenPieceList [i].transform.localPosition = doorLocation.transform.localPosition;

			occupiedGridLocations [i, 0] = -1;
			occupiedGridLocations [i, 1] = -1;
		}

		ResetPieces ();

		buttonStartLoc = button.transform.localPosition;
		buttonEndLoc = buttonStartLoc - new Vector3 (0,2,0);
	}

    // Runs with every frame
    private void Update() {
		checkPuzzle = button.playerStanding;

		//DrawGrid ();
		//brokenPieceList[0].transform.localPosition = new Vector3 (brokenPieceList[0].transform.localPosition.x, brokenPieceList[0].transform.localPosition.y, brokenPieceList[0].transform.localPosition.z + 0.01f);
	}

	private void DrawGrid () {
		for (int i = 0; i < gridSize; i++) {
			Debug.DrawLine (transform.position + grid [0, i], transform.position + grid [brokenPieceList.Length-1, i], Color.red);
		}
		for (int i = 0; i < brokenPieceList.Length; i++) {
			Debug.DrawLine (transform.position + grid [i, 0], transform.position + grid [i, gridSize-1], Color.green);
		}
		for (int i = 0; i < brokenPieceList.Length; i++) {
			for (int k = 0; k < gridSize; k++) {
				Debug.DrawLine (transform.position, transform.position + grid [i, k], Color.blue);
			}
		}
	}

	private void ResetPieces() {

		GameObject[] blockPushers;
		blockPushers = new GameObject[solvedPieceList.Length];

		for (int i = 0; i < brokenPieceList.Length; i++) {

			float pieceZValue = minPieceDistance + (blockPusher.transform.localScale.x * 2 * i);

			brokenPieceList [i].transform.localPosition = new Vector3 (
				brokenPieceList [i].transform.localPosition.x,
				brokenPieceList [i].transform.localPosition.y, 
				pieceZValue
				);

			brokenPieceList [i].transform.localPosition = Vector3.Project (DoorToPiece (brokenPieceList [i]), doorToCam) + doorLocation.transform.localPosition;

			/*while (SharedLocation (i)) {
				pieceZValue = pieceZValue + (blockPusher.transform.localScale.x * 2);

				if (pieceZValue > maxPieceDistance) {
					//pieceZValue = (cameraLocation.transform.localPosition.z + minPieceDistanceFromCamera);
				}

				brokenPieceList [i].transform.localPosition = new Vector3 (
					brokenPieceList [i].transform.localPosition.x,
					brokenPieceList [i].transform.localPosition.y, 
					pieceZValue
				);

				brokenPieceList [i].transform.localPosition = Vector3.Project (DoorToPiece (brokenPieceList [i]), doorToCam) + doorLocation.transform.localPosition;
			}*/
				

			ResizePiece (brokenPieceList [i]);

			GameObject block = Instantiate (blockPusher, new Vector3 (brokenPieceList [i].transform.position.x, transform.position.y, brokenPieceList [i].transform.position.z), transform.localRotation, this.transform);
			block.GetComponent<PushingBlock> ().Init (brokenPieceList [i], i);
			blockPushers [i] = block;

		} // END FOR

		PushingBlock.FindAllPushers(blockPushers);
		SetupGrid();

		for (int i = 0; i < blockPushers.Length; i++) {
			blockPushers [i].GetComponent<PushingBlock> ().RandomisePosition ();
		}

	}// END ResetPieces()

	private void SetupGrid() {
		grid = new Vector3[brokenPieceList.Length,gridSize];

		for (int i = 0; i < PushingBlock.blockPushers.Length; i++) {
			for (int k = 0; k < gridSize; k++) {
				int posMult = k - gridSize/2;

				grid [i, k] = PushingBlock.blockPushers[i].transform.localPosition + (PushingBlock.blockPushers[i].transform.right * (blockPusher.transform.localScale.x * 2) * posMult);
				GameObject gp = Instantiate (gridPiece, transform.position + grid [i, k], gridPiece.transform.rotation, this.transform);
				if (k - gridSize / 2 == 0) {
					gp.GetComponent<Renderer> ().material.color = Color.cyan;
				}
			}
		}
	}

	/*private bool SharedLocation(int i) {
		for (int k = 0; k < i; k++) {
			if ((brokenPieceList [k].transform.localPosition.x > brokenPieceList [i].transform.localPosition.x - blockPusher.transform.localScale.x && 
					brokenPieceList [k].transform.localPosition.x < brokenPieceList [i].transform.localPosition.x + blockPusher.transform.localScale.x) && 
				(brokenPieceList [k].transform.localPosition.y > brokenPieceList [i].transform.localPosition.y - blockPusher.transform.localScale.y && 
					brokenPieceList [k].transform.localPosition.y < brokenPieceList [i].transform.localPosition.y + blockPusher.transform.localScale.y) && 
				(brokenPieceList [k].transform.localPosition.z > brokenPieceList [i].transform.localPosition.z - blockPusher.transform.localScale.z && 
					brokenPieceList [k].transform.localPosition.z < brokenPieceList [i].transform.localPosition.z + blockPusher.transform.localScale.z)) {
				return true;
			}
		}
		return false;
	}*/

	private Vector3 DoorToPiece(GameObject piece) {
		return piece.transform.localPosition - doorLocation.transform.localPosition;
	}

    /// <summary>Scales a puzzle piece based on the camera location.</summary>
    /// <param name="piece">The puzzle piece to be resized.</param>
	public void ResizePiece(GameObject piece) {
		float scaleFactor;

		float distCameraToDoor = cameraLocation.transform.localPosition.z + -doorLocation.transform.localPosition.z;
		float distCameraToPiece = cameraLocation.transform.localPosition.z + -piece.transform.localPosition.z;

		scaleFactor = distCameraToPiece / distCameraToDoor;

		piece.transform.localScale = new Vector3 (scaleFactor, scaleFactor, scaleFactor);

	}

    /// <summary>Determines whether the puzzle has been solved or not.</summary>
    /// <returns>true if solved, false otherwise.</returns>
	public bool PuzzleSolved() {
		for (int i = 0; i < brokenPieceList.Length; i++) {
			if (brokenPieceList [i].transform.localPosition != Vector3.Project (DoorToPiece (brokenPieceList [i]), doorToCam) + doorLocation.transform.localPosition) {
				return false;
			}
		}
		return true;
	}

    /// <summary>Finalizes the puzzle by building and opening the door and disabling the puzzle.</summary>
	public void FinalizePuzzle() {
		DestroyPuzzlePieces ();
		EnablePuzzlePieces ();

		puzzleDone = true;
		button.Deactivate();
	}

	private void DestroyPuzzlePieces() {
		for (int i = 0; i < brokenPieceList.Length; i++) {
			Destroy (brokenPieceList [i]);
		}
	}

	private void EnablePuzzlePieces() {
		for (int i = 0; i < solvedPieceList.Length; i++) {
			solvedPieceList[i].GetComponent<MeshRenderer> ().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
		}
	}
}