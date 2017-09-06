using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectiveButtonManager : MonoBehaviour {

	PerspectivePieceHolder CurrHolder;


	public void UpdateHolder(PerspectivePieceHolder h) {
		CurrHolder = h;
	}

	public void SetHolderToPiece(int i) {
		CurrHolder.ChangePiece (i);
	}
}
