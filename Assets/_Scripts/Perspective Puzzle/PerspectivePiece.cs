using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerspectivePiece : MonoBehaviour {
	/// This is just to identify what gameObjects are pieces.

	private ParticleSystem particle;
	private ParticleSystem.ShapeModule sm;
	private const float BASE_PIECE_MESH_SCALE = 8.539178f;

	void ResetParticle() {
		particle = GetComponentInChildren<ParticleSystem> ();
		sm = particle.shape;
	}

	public void SimulateParticle() {
		if (particle != null) {
			particle.Play ();
		} else {
			ResetParticle ();
		}
	}

	public void ResizeParticle(float scale) {
		sm.meshScale = BASE_PIECE_MESH_SCALE * scale;
	}

	public GameObject getGameObject() {
		return gameObject;
	}
}
