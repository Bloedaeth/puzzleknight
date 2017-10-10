using GameLogging;
using UnityEngine;

public class PerspectivePieceHolder : MonoBehaviour {

	private PerspectivePieceManager parent;

	private GameObject currPiece;
	public Transform pieceLocation;

	private Transform door;
	//private Transform cam;

	private GameObject emptPiece;

	/// <summary>
	/// These are used to identify if the correct piece index is selected.
	/// </summary>
	public int currIndex;
	private int corrIndex;

	public void SetVariables (int currIndex, int corrIndex, PerspectivePieceManager parent, Transform door) {//, Transform cam) {
		this.parent = parent;
		this.currIndex = currIndex;
		this.corrIndex = corrIndex;
		this.door = door;
		//this.cam = cam;
		//pieceLocation = this.gameObject.GetComponentInChildren<Transform> ();
		ProjectToDoorVector ();

		SetupPiece();
	}

	/// <summary>
	/// Determines whether the current index of this holder is the correct index.
	/// </summary>
	/// <returns><c>true</c> if this instance is correct; otherwise, <c>false</c>.</returns>
	public bool IsCorrect() {
		return currIndex == corrIndex;
	}

	public int GetCurrIndex() {
		return currIndex;
	}

	public void ResetPieceLocations()
    {
        BuildDebug.Log("Resetting perspective piece locations");
        for (int i = 0; i < parent.pieces.Length; i++) {
			parent.pieces [i].transform.position = pieceLocation.position;
		}
	}

	private void ProjectToDoorVector() {
		Vector3 proj;
		Vector3 final;

		proj = -door.position + transform.position;
		proj = Vector3.Project (proj, parent.DoorToCam);


		final = door.position + proj;
		transform.position = new Vector3 (final.x, transform.position.y, final.z);
		pieceLocation.position = new Vector3 (pieceLocation.position.x, final.y, pieceLocation.position.z);
	}

	private void SetupPiece() {
		emptPiece = Instantiate (parent.emptyPiece);

		ChangePiece (corrIndex);
	}

	private void ResetPiece()
    {
        BuildDebug.Log("Resetting perspective piece");
        if (currPiece) {
			currPiece.transform.position = parent.hidePosition;
		}

		if (currIndex == -1) {
			currPiece = emptPiece;
		} else {
			currPiece = parent.pieces [currIndex];
			currPiece.GetComponent<PerspectivePiece>().SimulateParticle ();
		}

		currPiece.transform.position = pieceLocation.position;
	}

	public void ChangePiece(int i) {
		if (i >= parent.pieces.Length) {
			Debug.LogError ("Index is greater than pieces length\nIndex = " + i.ToString () + "\npieces.Length = " + parent.pieces.Length.ToString ());
			return;
		}

		int pas = parent.PieceAlreadySet (corrIndex, i);

		if (pas != -1) {
			parent.SwapIndexes (pas, corrIndex);
			if (parent.isActive) parent.holders [pas].ResetPiece ();
		} else {
			currIndex = i;
		}

		ResetPiece ();
	}
}
