using GameLogging;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectivePieceManager : MonoBehaviour {

	public PerspectivePieceHolder[] holders;

	public Transform door;
	public Transform cam;

	public GameObject[] pieces;
	public Vector3 DoorToCam{ get { return -door.position + cam.position; } }

	public GameObject emptyPiece;
	public Vector3 hidePosition;

	public bool isActive;

	// Use this for initialization
	void Start () {
		isActive = false;

		if (emptyPiece == null) {
			SetupEmpty ();
		}

		SetupPieces ();
		SetupHolders ();
		RescalePieces ();
		HidePieces ();

		isActive = true;
	}

	void SetupEmpty() {
        BuildDebug.Log("Setting up perspective puzzle empty piece");
		emptyPiece = GameObject.CreatePrimitive (PrimitiveType.Cube);
		GameObject.Destroy (emptyPiece.GetComponent<MeshFilter> ());
		GameObject.Destroy (emptyPiece.GetComponent<MeshRenderer> ());
		GameObject.Destroy (emptyPiece.GetComponent<Collider> ());
	}

	void SetupPieces()
    {
        BuildDebug.Log("Setting up perspective pieces");
        PerspectivePiece[] pp = GetComponentsInChildren<PerspectivePiece> ();

		hidePosition = emptyPiece.transform.position;

		if (pp.Length == 0) {
			Debug.LogError ("pp.Length == 0, pp.Length should not = 0");
		}

		pieces = new GameObject[pp.Length];

		for (int i = 0; i < pieces.Length; i++) {
			pieces [i] = pp [i].GetGameObject ();
		}

	}

	void HidePieces()
    {
        BuildDebug.Log("Hiding perspective puzzle pieces");
        for (int i = 1; i < holders.Length - 1; i++) {
			holders [i].ChangePiece (-1);
		}
	}

	void SetupHolders()
    {
        BuildDebug.Log("Setting up perspective puzzle piece holders");
        holders = GetComponentsInChildren<PerspectivePieceHolder> ();

		for (int i = 0; i < holders.Length; i++) {
            holders[i].SetVariables(i, i, this, door);//, cam);
		}
	}

	void RescalePieces ()
    {
        BuildDebug.Log("Scaling perspective puzzle pieces");
        for (int i = 0; i < pieces.Length; i++) {
			RescalePiece (pieces [i]);
		}
	}

	void RescalePiece(GameObject p) {
		Vector3 pVec = -cam.position + p.transform.position;

		float scale = (pVec.magnitude / DoorToCam.magnitude);

		p.transform.localScale = new Vector3(scale,scale,scale);
		p.GetComponent<PerspectivePiece> ().ResizeParticle (scale);
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

		if (piece == -1)
			return -1;

		for (int i = 0; i < holders.Length; i++) {
			if (i != caller && holders[i].GetCurrIndex() == piece) {
				return i;
			}
		}

		return -1;
	}

	public void SwapIndexes(int holderIndex1, int holderIndex2)
    {
        BuildDebug.Log("Swapping piece positions: " + holders[holderIndex1].name + " - " + holders[holderIndex2].name);
        int temp = holders [holderIndex1].currIndex;
		holders [holderIndex1].currIndex = holders [holderIndex2].currIndex;
		holders [holderIndex2].currIndex = temp;
	}


}

