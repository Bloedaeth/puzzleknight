using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereScript : MonoBehaviour {

	public PointStore pnts;

	private Transform dest;
	private Vector3 moveBuffer;

	public float speed = 2f;

	// Use this for initialization
	void Start () {
		if (pnts == null) {
			pnts = GameObject.Find ("PointStore").GetComponent<PointStore> ();
		}


		dest = pnts.GetRandomPoint ();
		moveBuffer = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (dest == null) {
			dest = pnts.GetRandomPoint ();
			return;
		}
		if (transform.position == dest.position) {
			dest = pnts.GetRandomPoint ();
			dest.rotation = new Quaternion(0,Random.Range((float) 0.0, (float) 360.0),0,Random.Range((float) 0.0, (float) 360.0));
		}

		Move ();

		if (Input.GetKeyDown (KeyCode.Space)) {
			Destroy (this.gameObject);
		}
	}


	private void Move() {
		moveBuffer = Vector3.Lerp (moveBuffer, dest.position, Time.deltaTime * speed);
		transform.position = Vector3.Lerp (transform.position, moveBuffer, Time.deltaTime * speed * 0.5f);
		transform.rotation = Quaternion.Slerp (transform.rotation, dest.rotation, Time.deltaTime * speed * 0.5f);

		if ((-transform.position + dest.position).magnitude < 0.03f) {
			transform.position = dest.position;
		}
	}
}
