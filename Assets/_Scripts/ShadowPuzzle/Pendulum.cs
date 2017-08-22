using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour {

	public bool SlowTime;

	void Awake () {
		SlowTime = false;
	}

	void OnTriggerEnter(Collider o) {
		if (o.GetComponent<TimeFreezeCollider>() != null) {
			SlowTime = true;
		}
	}

	void OnTriggerExit(Collider o) {
		if (o.GetComponent<TimeFreezeCollider>() != null) {

			SlowTime = false;
		}
	}
}
