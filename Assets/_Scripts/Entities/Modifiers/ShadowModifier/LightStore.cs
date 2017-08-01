using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightStore : MonoBehaviour {

	protected Light[] lights;
	protected bool updateLights;

	// Use this for initialization
	void Start () {
		lights = GetComponentsInChildren<Light> ();
		print (lights.Length.ToString());
	}

	public bool UpdateLights() {
		return updateLights;
	}

	public virtual Light[] GetLights() {
		return lights;
	}
}
