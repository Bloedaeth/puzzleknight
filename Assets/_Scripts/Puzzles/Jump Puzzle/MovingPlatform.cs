using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour {

	Vector3[] points;
	Vector3 path { get { return -points [prevPoint] + points [currPoint]; } }

	int currPoint;
	int prevPoint;

	float startMoveTime;
	public float moveRate = 1f;

	float moveStage { get { return (Time.time - startMoveTime) / moveRate; } }

	/// <summary>
	/// Determines whether or not the platform does a closed loop, or if the platform goes to the end point, and then back through the points.
	/// 
	/// True: The iteration of a 4 point array will go 0, 1, 2, 3, 2, 1, 0.
	/// False: The iteration of a 4 point array will go 0, 1, 2, 3, 0.
	/// </summary>
	public bool occilating;

	// Use this for initialization
	void Start () {
		MovingPlatformPoint[] p = GetComponentsInChildren<MovingPlatformPoint> ();

		if (p.Length <= 1) {
			enabled = false;
			return;
		}

		currPoint = 0;

		prevPoint = occilating ? 1 : p.Length-1;

		points = new Vector3[p.Length];

		for (int i = 0; i < points.Length; i++) {
			points [i] = p [i].t.position;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > startMoveTime + moveRate) {
			startMoveTime = Time.time;
			IteratePoint (occilating);
		}

		MovePlatform (moveStage);
	}

	void IteratePoint (bool o) {
		if (o) { // If occilating
			if (currPoint == points.Length - 1) {
				currPoint--;
				prevPoint = points.Length - 1;
			} else if (currPoint == 0) {
				currPoint++;
				prevPoint = 0;
			} else if (currPoint < prevPoint) {
				prevPoint--;
				currPoint--;
			} else if (currPoint > prevPoint) {
				prevPoint++;
				currPoint++;
			}
		} else { //If looping
			prevPoint = currPoint;
			if (currPoint == points.Length - 1) {
				currPoint = 0;
			} else {
				currPoint++;
			}
		}
	}

	void MovePlatform(float stage) {
		transform.position = points [prevPoint];

		float scalar = -0.5f * Mathf.Cos (Mathf.PI * stage) + 0.5f;

		Debug.DrawLine (transform.position, transform.position + path);

		transform.position += path * scalar;
	}

	void OnTriggerEnter(Collider o) {
		if (o.CompareTag ("Player")) {
			o.transform.parent = transform;
		}
	}

	void OnTriggerExit(Collider o) {
		if (o.CompareTag ("Player")) {
			o.transform.parent = null;
		}
	}
}
