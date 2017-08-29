using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectivePiece : MonoBehaviour {
	/// This is just to identify what gameObjects are pieces.

	private ParticleSystem particle;

	void ResetParticle() {
		particle = GetComponentInChildren<ParticleSystem> ();
	}

	public void SimulateParticle() {
		if (particle != null) {
			particle.Play ();
		} else {
			ResetParticle ();
		}
	}

	public GameObject getGameObject() {
		return gameObject;
	}
}
