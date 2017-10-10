using GameLogging;
using UnityEngine;

public class PerspectivePieceUiButtonManager : MonoBehaviour {

	PerspectivePieceHolder CurrHolder;

	public Transform[] buttons;

	public Sprite[] CollectedPieces;
	public Sprite[] UncollectedPieces;

	//private bool[] currentDoorPieces;

	public void UpdateHolder(PerspectivePieceHolder h) {
		CurrHolder = h;
	}

	public void UpdatePieces(bool[] playerPieces) {
        //currentDoorPieces = playerPieces;
        BuildDebug.Log("Updating perspective puzzle pieces");
		for (int i = 0; i < buttons.Length; i++) {
			if (playerPieces [i]) {
				buttons [i].GetComponent<UnityEngine.UI.Image> ().sprite = CollectedPieces [i];
			} else {
				buttons [i].GetComponent<UnityEngine.UI.Image> ().sprite = UncollectedPieces [i];
			}
			buttons [i].GetComponent<UnityEngine.UI.Button> ().enabled = playerPieces[i];
		}
	}

	public void SetHolderToPiece(int i) {
		CurrHolder.ChangePiece (i);
	}
}
