using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightStore : MonoBehaviour {

	private Light[] lights;

	// Use this for initialization
	void Start () {
		lights = GetComponentsInChildren<Light> ();
		print (lights.Length.ToString());
	}
	
	public Light[] GetLights() {
		return lights;
	}
}
