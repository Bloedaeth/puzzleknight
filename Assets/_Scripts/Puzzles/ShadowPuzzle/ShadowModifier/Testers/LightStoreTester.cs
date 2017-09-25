using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightStoreTester : LightStore {

	private Light[] templights;
	private Vector3[] lightOnPositions;
	private Vector3[] lightOffPositions;

	private int lightCount;
	private float lightChangeTime;
	private float lightChangeRate = 0.5f;

	void Start() {
		if (lights == null) {
			lights = GetComponentsInChildren<Light> ();
		}

		lightCount = 0;

		FlickLights();

		lightOnPositions = new Vector3[3] {new Vector3(0f,1f,0f),new Vector3(-6.5f,1f,6.5f),new Vector3(6.5f,1f,6.5f)};
		lightOffPositions = new Vector3[3] {new Vector3(0f,-2f,0f),new Vector3(-6.5f,-2f,6.5f),new Vector3(6.5f,-2f,6.5f)};
	}

	// Update is called once per frame
	void Update () {
		if (lights == null)
			return;
		
		if (Time.time > lightChangeTime) {
			updateLights = false;

			if (Input.GetKeyDown (KeyCode.Alpha0)) {
				lightCount = 0;
				updateLights = true;
				lightChangeTime = Time.time + lightChangeRate;
				FlickLights ();
			}

			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				lightCount = 1;
				updateLights = true;
				lightChangeTime = Time.time + lightChangeRate;
				FlickLights ();
			}

			if (Input.GetKeyDown (KeyCode.Alpha2)) {
				lightCount = 2;
				updateLights = true;
				lightChangeTime = Time.time + lightChangeRate;
				FlickLights ();
			}

			if (Input.GetKeyDown (KeyCode.Alpha3)) {
				lightCount = 3;
				updateLights = true;
				lightChangeTime = Time.time + lightChangeRate;
				FlickLights ();
			}
		}

		MoveLights();
	}

	void FlickLights () {
		for (int i = 0; i < lights.Length; i++) {
			lights [i].enabled = i < lightCount;
		}
	}

	void MoveLights() {
		for (int i = 0; i < lights.Length; i++) {
			if (lights[i].enabled) {
				lights [i].transform.position = Vector3.Lerp (lights [i].transform.position, lightOnPositions [i], Time.deltaTime);
			} else {
				lights [i].transform.position = Vector3.Lerp (lights [i].transform.position, lightOffPositions [i], Time.deltaTime);
			}
		}
	}

	public override Light[] GetLights() {
		templights = new Light[lightCount];

		for (int i = 0; i < lightCount; i++) {
			templights [i] = lights [i];
		}

		return templights;
	}
}
