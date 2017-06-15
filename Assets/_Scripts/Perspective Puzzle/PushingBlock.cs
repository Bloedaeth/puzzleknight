using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushingBlock : MonoBehaviour {

	public static PerspectivePuzzleStore ps;
	public static Transform door;
	public static Transform cam;
	public static int gridSize;

	public static PushingBlock[] blockPushers;

	public int blockNumber;

	private bool hide;

	// an array of length 2, holding x and y coordinates of the grid int[][] in the puzzle store
	public int[] gridLocation;

	private Vector3 currPosition;
	private Vector3 pointPosition;

	private Transform piece;

	private float pushTime;
	private float pushRate = 0.5f;

	void Start() {
		gridSize = PerspectivePuzzleStore.gridSize;
	}

	public void init (GameObject piece, int blockNumber) {
		this.piece = piece.transform;
		this.blockNumber = blockNumber;

		hide = false;

		pointPosition = transform.localPosition;
		currPosition = transform.localPosition;

		gridLocation = new int[2] { -1, -1 };
	}

	public static void FindAllPushers(GameObject[] blockPushersObjects) {

		blockPushers = new PushingBlock[blockPushersObjects.Length];

		for (int i = 0; i < blockPushers.Length; i++) {
			blockPushers [i] = blockPushersObjects [i].GetComponent<PushingBlock> ();
		}
	}

	public void RandomisePosition() {
		int x = Random.Range (0, blockPushers.Length);
		int y = Random.Range (0, PerspectivePuzzleStore.gridSize);

		bool applyGrid = true;

		if (SharedGridLocation (new int[] {x,y})) {
			applyGrid = false;
		}

		if (applyGrid) {
			gridLocation = new int[] {x, y};
			ps.occupiedGridLocations [blockNumber, 0] = x;
			ps.occupiedGridLocations [blockNumber, 1] = y;
			UpdatePointPosition ();
		} else {
			RandomisePosition ();
		}
	}

	private bool SharedGridLocation(int[] newGridLocation) {
		for (int i = 0; i < ps.occupiedGridLocations.GetLength(0); i++) { 
			if (ps.occupiedGridLocations [i, 0] == newGridLocation [0] && ps.occupiedGridLocations [i, 1] == newGridLocation [1]) {
				print ("Shares location: [" + ps.occupiedGridLocations [i, 0].ToString () + ":" + ps.occupiedGridLocations [i, 0].ToString () + "]\nHeld by Block number: " + i.ToString ());
				return true;
			}
		}
		return false;
	}

	private void UpdatePointPosition () {
		ps.occupiedGridLocations [blockNumber, 0] = gridLocation [0];
		ps.occupiedGridLocations [blockNumber, 1] = gridLocation [1];

		pointPosition = ps.grid [gridLocation [0],gridLocation [1]];
	}

	// Update is called once per frame
	void Update () {
		if (Vector3.Distance (currPosition, pointPosition) > 0.03f) {
			currPosition = Vector3.Lerp (currPosition, pointPosition, 0.1f);
		} else {
			currPosition = pointPosition;
		}
			
		transform.localPosition = currPosition;

		if (piece != null) {
			piece.position = new Vector3 (transform.position.x, piece.position.y, transform.position.z);
		}

		if (ps != null) {
			hide = ps.puzzleDone;
		}

		if (hide) {
			//HideMe ();
		}

		if (Input.GetKeyDown (KeyCode.P)) {
			RandomisePosition ();
		}
	}

	private void HideMe() {
		pointPosition -= 0.5f * transform.up;

		Destroy (this, 2f);
	}

	private bool PlayerFacingBlock(Transform player) {
		GameObject transformCopy = Instantiate (new GameObject (), transform.position, transform.rotation);

		transformCopy.transform.LookAt (player);
		float ang = Vector3.Angle (transformCopy.transform.forward, player.forward);

		Destroy (transformCopy);

		return ang > 135 && ang < 225;
	}

	void OnTriggerStay(Collider o) {


		if (!ps.puzzleDone &&  o.gameObject.tag.ToLower () == "player" && pushTime < Time.time) {

			// find out if the player is facing the block
			if (!PlayerFacingBlock (o.transform)) {
				return;
			}

			// find out which side of the block the player is standing at.

			Vector3 vec, projectVecForward, projectVecRight;
			vec = (o.transform.position - transform.position);
			projectVecForward = Vector3.Project (vec, transform.forward);
			projectVecRight = Vector3.Project (vec, transform.right);

			int prevX = gridLocation [0];
			int prevY = gridLocation [1];

			if (projectVecRight.magnitude > projectVecForward.magnitude) { // if the right vector magnitude is greater than the forward vector, then the player is on the left or right
				// PLayer is on horizontal axis (left or right)
				if (-projectVecRight.normalized == transform.right) {
					gridLocation [1] += 1;
				} else {
					gridLocation [1] -= 1;
				}
			} else {
				// PLayer is on vertical axis (forward or back)
				if (-projectVecForward.normalized == transform.forward) {
					gridLocation [0] += 1;
				} else {
					gridLocation [0] -= 1;
				}
			}

			print (gridLocation[0].ToString() + ":" + gridLocation[1].ToString());

			if (ValidGridLocation ()) {
				UpdatePointPosition ();
			} else {
				gridLocation[0] = prevX;
				gridLocation[1] = prevY;
			}
				
			pushTime = Time.time + pushRate;

		} // end if
	} // end OnTriggerStay

	private bool ValidGridLocation() {
		if (gridLocation [0] < 0 || gridLocation [0] >= blockPushers.Length) {
			return false;
		}

		if (gridLocation [1] < 0 || gridLocation [1] >= gridSize) {
			return false;
		}

		return !SharedGridLocation (gridLocation);
	}
} // PUSHING BLOCK SCRIPT CLASS
