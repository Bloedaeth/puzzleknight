using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Particle hider script.
/// 
/// To be placed on the puzzle object that is the root of the puzzle, this script just holds all the particle systems in a puzzle area and disables them when the player is out of the range of the puzzle.
/// </summary>

[RequireComponent(typeof(BoxCollider))]
public class ParticleAndLightHider : MonoBehaviour {

	ParticleSystem[] particles { get { return GetComponentsInChildren<ParticleSystem> (); } }
	Light[] lights {
		get {
			if (includeLights) {
				return GetComponentsInChildren<Light> ();
			} else {
				return new Light[0];
			}
		}
	}
	GameObject[] objects;
	bool defaultApplied = false;

	[SerializeField] private bool includeLights;

	void ResetGameObjects() {
		objects = new GameObject[particles.Length + lights.Length];
		for (int i = 0; i < particles.Length; i++) {
			objects [i] = particles [i].gameObject;
		}

		for (int i = 0; i < lights.Length; i++) {
			objects [i+particles.Length] = lights [i].gameObject;
		}
	}

	void Update() {
		if (Time.time > 0.5f && !defaultApplied) {
			ResetGameObjects ();
			defaultApplied = true;
			SetObjectStates (false);
		}
	}

	void SetObjectStates(bool state) {
		if (defaultApplied) {
			foreach (GameObject g in objects) {
				g.SetActive (state);
			}
		}
	}

	void OnTriggerEnter(Collider o) {
			if (o.CompareTag ("Player")) {
				SetObjectStates (true);
			}
	}

	void OnTriggerExit(Collider o) {
		if (o.CompareTag ("Player")) {
			SetObjectStates (false);
		}
	}
}
