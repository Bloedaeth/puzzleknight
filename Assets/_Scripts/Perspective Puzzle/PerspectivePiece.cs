using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectivePiece : MonoBehaviour {
	/// This is just to identify what gameObjects are pieces.

	private ParticleSystem particle;
	private ParticleSystem.ShapeModule sm;
	private float originalPieceMeshScale;

	void ResetParticle() {
		particle = GetComponentInChildren<ParticleSystem> ();
		sm = particle.shape;
		originalPieceMeshScale = sm.meshScale;
	}

	public void SimulateParticle() {
		if (particle != null) {
			particle.Play ();
		} else {
			ResetParticle ();
		}
	}

	public void ResizeParticle(float scale) {
		sm.meshScale = originalPieceMeshScale * scale;
	}

	public GameObject getGameObject() {
		return gameObject;
	}
}
