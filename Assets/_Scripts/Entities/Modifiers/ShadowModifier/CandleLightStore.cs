using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleLightStore : LightStore {

	Vector3[] startPos;
	Vector3[] offset;

	public float offsetFactor = 0.01f;

	private float offsetTime;
	private float offsetRate = 0.05f;

	void Start () {
		if (lights == null) {
			lights = GetComponentsInChildren<Light> ();
		}

		ResetArrays ();
	}

	void Update () {
		if (startPos != null) { 
			if (Time.time > offsetTime) {
				offsetTime = Time.time + offsetRate;
				FlickerLights ();
			}
		} else if (lights != null) {
			ResetArrays ();
		}
	}

	void ResetArrays() {
		startPos = new Vector3[lights.GetLength (0)];
		offset = new Vector3[lights.GetLength (0)];

		for (int i = 0; i < startPos.Length; i++) {
			startPos [i] = lights [i].transform.position;
		}
	}

	void FlickerLights() {
		for (int i = 0; i < offset.Length; i++) {
			offset [i] = new Vector3 (Random.Range (-1f, 1f), Random.Range (-1f, 1f), Random.Range (-1f, 1f));
			offset[i] = offset[i] * offsetFactor;

			lights [i].transform.position = startPos [i] + offset [i];
		}
	}
}
