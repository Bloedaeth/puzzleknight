using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarSpinnar : MonoBehaviour {

	Camera c { get { return Camera.main; } }
	Mesh m { get { return GetComponent<MeshFilter> ().mesh; } }
	Vector3 Euler { get { return transform.rotation.eulerAngles; } }

	/*ParticleSystem p { 
		get {
			if (!PS) {
				PS = GetComponentInChildren<ParticleSystem> ();
			}
			return PS;
		}
	}

	ParticleSystem PS;*/

	bool turned = false;

	float turnAngle = 90f;

	// Update is called once per frame
	void Update () {
		if ( !turned && !CanBeSeen()) {
			//p.Simulate (0);
			RandomTurn ();
		} else if (turned && CanBeSeen()) {
			//p.Stop ();
			turned = false;
		}
	}
		

	void RandomTurn() {
		transform.rotation = Quaternion.Euler (Euler.x, Euler.y + RandomAngle (), Euler.z);

		turned = true;
	}

	float RandomAngle() {
		return turnAngle * Random.Range (0, 3);
	}


	bool CanBeSeen () {
		if ((-transform.position + c.transform.position).magnitude > 50f) {
			return false;
		}

		foreach (Vector3 v in m.vertices) {
			Vector3 vec = new Vector3 (v.x * transform.localScale.x, v.y * transform.localScale.y, v.z * transform.localScale.z);
			vec += transform.position;
			vec -= c.transform.position;

			if (Vector3.Angle (c.transform.forward, vec) < c.fieldOfView/1.2f) {
				return true;
			} else {
			}
		}
		return false;
	}
}
