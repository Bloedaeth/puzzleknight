using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectivePieceManager : MonoBehaviour {

	PerspectivePieceHolder[] holders;

	public Transform door;
	public Transform cam;

	public GameObject[] pieces;
	public Vector3 DoorToCam{ get { return -door.position + cam.position; } }

	public GameObject emptyPiece;

	// Use this for initialization
	void Start () {
		SetupEmpty ();
		SetupPieces ();
		SetupHolders ();

		RescalePieces ();
	}

	void SetupEmpty() {
		emptyPiece = GameObject.CreatePrimitive (PrimitiveType.Cube);
		GameObject.Destroy (emptyPiece.GetComponent<MeshFilter> ());
		GameObject.Destroy (emptyPiece.GetComponent<MeshRenderer> ());
		GameObject.Destroy (emptyPiece.GetComponent<Collider> ());
	}

	void SetupPieces() {
		PerspectivePiece[] pp = GetComponentsInChildren<PerspectivePiece> ();

		pieces = new GameObject[pp.Length];

		for (int i = 0; i < pieces.Length; i++) {
			pieces [i] = pp [i].getGameObject ();
		}

	}

	void SetupHolders() {
		holders = GetComponentsInChildren<PerspectivePieceHolder> ();

		for (int i = 0; i < holders.Length; i++) {
			holders [i].SetVariables ((i >= 1) && (i <= 3) ? -1 : i, i, this, door, cam);
		}
	}

	void RescalePieces () {
		for (int i = 0; i < pieces.Length; i++) {
			RescalePiece (pieces [i]);
		}
	}

	void RescalePiece(GameObject p) {
		Vector3 pVec = -cam.position + p.transform.position;

		float scale = (pVec.magnitude / DoorToCam.magnitude);

		p.transform.localScale = new Vector3(scale,scale,scale);
	}

	public bool IndexesAllCorrect() {
		for (int i = 0; i < holders.Length; i++) {
			if (!holders [i].IsCorrect ()) {
				return false;
			}
		}
		return true;
	}
		
	public int PieceAlreadySet(int caller, int piece) {
		for (int i = 0; i < holders.Length; i++) {
			if (i != caller && holders[i].GetCurrIndex() == piece) {
				return i;
			}
		}

		return -1;
	}

	public void swapIndexes(int holderIndex1, int holderIndex2) {
		int temp = holders [holderIndex1].currIndex;
		holders [holderIndex1].currIndex = holders [holderIndex2].currIndex;
		holders [holderIndex2].currIndex = temp;
	}


}

