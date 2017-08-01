using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereScript : MonoBehaviour {

	public PointStore pnts;

	private Transform dest;
	private Vector3 buffer;

	public int currPoint;

	public float speed = 2f;

	// Use this for initialization
	void Start () {
		currPoint = 0;
		dest = point1;
		buffer = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (transform.position == dest.position) {
			if (currPoint == 0) {
				dest = point1;
				currPoint = 1;
			} else {
				dest = point2;
				currPoint = 0;
			}
		}

		Move ();
	}


	private void Move() {
		buffer = Vector3.Lerp (buffer, dest.position, Time.deltaTime * speed);
		transform.position = Vector3.Lerp (transform.position, buffer, Time.deltaTime * speed * 0.5f);

		if ((-transform.position + dest.position).magnitude < 0.03f) {
			transform.position = dest.position;
		}
	}
}
