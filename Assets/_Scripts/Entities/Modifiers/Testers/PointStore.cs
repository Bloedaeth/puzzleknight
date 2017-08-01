using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointStore : MonoBehaviour {

	Transform[] points;

	// Use this for initialization
	void Start () {
		points = GetComponentsInChildren<Transform> ();
	}
	
	// Update is called once per frame
	Transform[] GetPoints () {
		return points;
	}
}
