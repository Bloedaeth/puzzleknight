using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointStore : MonoBehaviour {

	Transform[] points;

	// Use this for initialization
	void Start () {
		points = GetComponentsInChildren<Transform> ();
	}

	public Transform[] GetPoints () {
		return points;
	}

	public Transform GetRandomPoint() {
		if (points == null)
			return null;
		return points[Random.Range (1, points.Length)];
	}
}
