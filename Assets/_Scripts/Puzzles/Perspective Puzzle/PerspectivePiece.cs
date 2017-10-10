using GameLogging;
using UnityEngine;

public class PerspectivePiece : MonoBehaviour {
	/// This is just to identify what gameObjects are pieces.

	private ParticleSystem particle;
	private ParticleSystem.ShapeModule sm;
	private float originalPieceMeshScale;

	void ResetParticle()
    {
        BuildDebug.Log("Resetting perspective piece particles");
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

	public GameObject GetGameObject() {
		return gameObject;
	}
}
